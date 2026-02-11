using EPIC.PermissionGenerator.Extensions;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Xaml;
using System.Xml;

namespace EPIC.PermissionGenerator
{
    public static class DesignTimeHelper
    {

        static DesignTimeHelper()
        {
            /*
            var dte = (EnvDTE.DTE)Marshal.GetActiveObject("VisualStudio.DTE");
            var activeDoc = dte.ActiveDocument;

            if (activeDoc != null)
            {
                string fullPath = activeDoc.FullName; // "C:\Source\MedicalMemory\Views\MainWindow.xaml"
                string pCsPath = fullPath.Replace(".xaml", ".p.cs");

                // Now you can write exactly where you belong
                File.WriteAllText(pCsPath, generatedCode);
            }
            */
        }

        public static readonly DependencyProperty TriggerMetadataGenProperty =
            DependencyProperty.RegisterAttached(
                "TriggerMetadataGen",
                typeof(FrameworkElement),
                typeof(DesignTimeHelper),
                new PropertyMetadata(null, OnTriggerChanged));

        // This is the "Magic" property the designer sees
        [DesignOnly(true)]
        public static FrameworkElement TriggerMetadataGen
        {
            get => null;
            set
            {
                if (DesignerProperties.GetIsInDesignMode(value as FrameworkElement))
                {
                    // Execute your VisualTreeParser HERE
                    // It runs inside XDesProc.exe (The Designer Process)

                }
            }
        }

        public static string GetNamespaceFromPackUri(Uri baseUri)
        {
            // 1. Get the local path: "/MyProject;component/Views/MainWindow.xaml"
            string path = baseUri.LocalPath;

            // 2. Remove the ";component/" part and the extension
            // Result: "MyProject/Views/MainWindow"
            string cleanPath = path.Replace(";component/", "/")
                                   .Replace(".xaml", "");

            // 3. Remove the filename (MainWindow) to get just the folder structure
            // Result: "MyProject/Views"
            string folderPath = System.IO.Path.GetDirectoryName(cleanPath);

            // 4. Convert slashes to dots and ensure CamelCase
            // Result: "MyProject.Views"
            return String.Join(",\n    ", folderPath.Replace('\\', '/').TrimStart('/')
                                .Split('/')
                                .Select(s => System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(s)));
        }

        private static void OnTriggerChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (DesignerProperties.GetIsInDesignMode(d) && d is FrameworkElement root)
            {
                MakePermissions(BaseUriHelper.GetBaseUri(root), root, Assembly.GetExecutingAssembly());
            }

        }

        public static void MakePermissions(Uri baseUri, FrameworkElement root, Assembly assembly)
        {
            var name = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(Path.GetFileNameWithoutExtension(baseUri.LocalPath));

            var application = assembly.GetTypes().FirstOrDefault(t => typeof(System.Windows.Application).IsAssignableFrom(t));

            var Permissions = Utilities.IntrospectXaml(assembly, root as FrameworkElement, baseUri.LocalPath);
            var defaultNamespace = GetNamespaceFromPackUri(baseUri);

            string CODE_TEMPLATE = $@"
namespace {application.Namespace}.{defaultNamespace} {{
    public partial class {name} {{
        public static readonly System.Collections.Generic.List<DataLayer.Entities.Permission> Permissions = 
            new System.Collections.Generic.List<DataLayer.Entities.Permission> {{
                {String.Join(",\n    ", Permissions.Select(p => $@"(""{p.Name}"", ""{p.Description}"", {(p.IsActionable ? "true" : "false")})"))}
            }};
    }}
}}";
            // Generate the .p.cs or .meta.json file name based on the class type
            string fileName = root.GetType().Name + ".p.cs";
            File.WriteAllText(Path.Combine(_outputPath, fileName), CODE_TEMPLATE);
        }

        private static string _outputPath = "";


        [STAThread]
        public static void Main(string[] args2)
        {
            Console.WriteLine("Fuck: " + Environment.CommandLine);
            var args = Environment.CommandLine.Trim('"').ToString().TrimStart(Environment.ProcessPath).Trim("dll").Trim("\" ").TrimEnd('\\').ToString().Split('|');
            _outputPath = args[2] ?? "";
            // This is just to ensure the static constructor runs when the assembly is loaded in the designer.
            foreach (var arg in args)
            {
                Console.WriteLine($"Argument: {arg}");
            }
            // 1. Initialize a "Headless" Application context
            // This is required if the XAML uses {StaticResource} or {DynamicResource}
            if (System.Windows.Application.Current == null)
            {
                new System.Windows.Application { ShutdownMode = ShutdownMode.OnExplicitShutdown };
            }

            // 2. Load your XAML string or File
            string xaml = File.ReadAllText(args[0]);
            //Console.WriteLine("Loading XAML from: " + xaml);
            Uri baseUri = new Uri("file:///" + args[3] + "/", UriKind.Absolute);
            XamlObjectWriter writer;

            using (var sr = new StringReader(xaml))
            using (var xr = XmlReader.Create(sr))
            using (var xamlReader = new XamlXmlReader(xr, new XamlXmlReaderSettings { ProvideLineInfo = true, BaseUri = baseUri }))
            {
                writer = new XamlObjectWriter(xamlReader.SchemaContext, new XamlObjectWriterSettings
                {
                    // This is the "Designer Secret": 
                    // You can provide a root object that just ignores everything it doesn't know.
                    IgnoreCanConvert = true
                });

                int ignoreDepth = 0;
                while (xamlReader.Read())
                {
                    // 1. Determine if this node is a "bad" trigger
                    bool isTrigger = false;
                    if (ignoreDepth == 0)
                    {
                        if (xamlReader.NodeType == XamlNodeType.StartMember)
                        {
                            var n = xamlReader.Member?.Name ?? "";
                            if (xamlReader.Member.IsEvent || n.EndsWith("Class") || n.EndsWith("Source") || n.EndsWith("Binding")) isTrigger = true;
                        }
                        else if (xamlReader.NodeType == XamlNodeType.StartObject)
                        {
                            var t = xamlReader.Type?.Name ?? "";
                            if (t == "BitmapImage" || t == "ImageSource" || t == "ResourceDictionary" || t == "Style") isTrigger = true;
                        }
                    }

                    // 2. Adjust depth BEFORE writing for "Enter" nodes
                    if (isTrigger)
                    {
                        ignoreDepth = 1;
                    }
                    else if (ignoreDepth > 0)
                    {
                        if (xamlReader.NodeType == XamlNodeType.StartObject ||
                            xamlReader.NodeType == XamlNodeType.GetObject ||
                            xamlReader.NodeType == XamlNodeType.StartMember)
                        {
                            ignoreDepth++;
                        }
                    }

                    // 3. THE GATEKEEPER
                    if (ignoreDepth == 0)
                    {
                        writer.WriteNode(xamlReader);
                    }

                    // 4. Adjust depth AFTER writing for "Exit" nodes
                    if (ignoreDepth > 0 && !isTrigger)
                    {
                        if (xamlReader.NodeType == XamlNodeType.EndObject ||
                            xamlReader.NodeType == XamlNodeType.EndMember)
                        {
                            ignoreDepth--;
                        }
                    }

                    // Log AFTER the logic so you see the state the NEXT node will face
                    Console.WriteLine($"NodeType: {xamlReader.NodeType}, Member: {xamlReader.Member?.Name}, Type: {xamlReader.Type?.Name}, NewDepth: {ignoreDepth}");
                }
            }
            //ParserContext context = new ParserContext();

            var element = writer.Result as FrameworkElement;
            if (element == null)
            {
                Console.WriteLine("Failed to parse XAML into a FrameworkElement. Permissions generation will be skipped.");
                return;
            }
            // CRITICAL: This allows relative paths like "themes/clearview.xaml" to resolve
            Size renderSize = new Size(1024, 768);
            element.Measure(renderSize);
            element.Arrange(new Rect(renderSize));
            element.UpdateLayout();

            // 3. Render it to a Bitmap in memory
            RenderTargetBitmap rtb = new RenderTargetBitmap(
                (int)renderSize.Width,
                (int)renderSize.Height,
                96, 96, // DPI
                PixelFormats.Pbgra32);
            rtb.Render(element);

            // 4. Save to a file so you can open it
            var encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(rtb));

            using (var stream = System.IO.File.Create(Path.Join(args[3], "RenderedView.png")))
            {
                encoder.Save(stream);
            }
            Console.WriteLine("Visual capture saved to " + args[3] + "/RenderedView.png");

            var root = writer.Result as FrameworkElement;
            /*Console.WriteLine(JsonSerializer.Serialize(writer.Result, new JsonSerializerOptions
            {
                WriteIndented = true,
                NumberHandling = System.Text.Json.Serialization.JsonNumberHandling.AllowNamedFloatingPointLiterals,
                IgnoreReadOnlyProperties = true,
                ReferenceHandler = ReferenceHandler.IgnoreCycles
            }));*/
            if (root == null)
            {
                return;
            }
            var name = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(Path.GetFileNameWithoutExtension(args[0]));

            var Permissions = Utilities.IntrospectXaml(null, root, args[0]);
            Console.WriteLine("Loading XAML cleaned " + root.GetAllLogicalChildren().Concat(root.GetAllChildren()).Distinct().Count() + " : " + root?.ToString());
            if (Permissions.Count() == 0)
            {
                return;
            }

            var defaultNamespace = GetNamespaceFromPackUri(new Uri(args[0]));

            string CODE_TEMPLATE = $@"
namespace {args[1]}.{defaultNamespace} {{
    public partial class {name} {{
        public static readonly System.Collections.Generic.List<DataLayer.Entities.Permission> Permissions = 
            new System.Collections.Generic.List<DataLayer.Entities.Permission> {{
                {String.Join(",\n    ", Permissions.Select(p => $@"(""{p.Name}"", ""{p.Description}"", {(p.IsActionable ? "true" : "false")})"))}
            }};
    }}
}}";
            // Generate the .p.cs or .meta.json file name based on the class type
            string fileName = root.GetType().Name + ".p.cs";
            Console.WriteLine($@"Writing {Permissions.Count()} permissions to {fileName}");
            File.WriteAllText(Path.Combine(_outputPath, fileName), CODE_TEMPLATE);

        }
    }

}


