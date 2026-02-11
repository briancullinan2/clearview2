using ICSharpCode.BamlDecompiler;
using ICSharpCode.Decompiler.Metadata;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Baml2006;
using System.Xaml;
using System.Xml;
using System.Xml.Linq;

namespace EPIC.PermissionGenerator
{
    public static class Generate
    {
        private static System.Reflection.Assembly _assembly;

        // ok new strategy load the entire mock app in dev mode to generate permissions files, then restart the app to bind them with the DLL plugin compiled
        [STAThread]
        public static void Main(string[] args2)
        {
            var args = GetNormalArgs(args2);

            if (System.Windows.Application.Current == null)
            {
                new System.Windows.Application { ShutdownMode = ShutdownMode.OnExplicitShutdown };
            }

            var realFile = System.IO.Path.Combine(Environment.CurrentDirectory, args[0]);

            if (!System.IO.File.Exists(realFile))
            {
                // Try the parent directory (for designer hosting scenarios)
                Console.WriteLine($"File not found: {realFile}");
                realFile = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, args[0]);
            }

            if (System.IO.File.Exists(realFile))
            {
                Console.WriteLine("Starting Permission Generation...");
                AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
                byte[] rawAssembly = File.ReadAllBytes(realFile);
                _assembly = AppDomain.CurrentDomain.Load(rawAssembly);
                //_assembly = System.Reflection.Assembly.LoadFrom(realFile);
                GenerateFromAssembly(_assembly);
            }
            else
            {
                Console.WriteLine($"File not found: {realFile}");
            }
        }


        private static System.Reflection.Assembly? CurrentDomain_AssemblyResolve(object? sender, ResolveEventArgs args)
        {
            // 1. Check if it's your main generator or a known friend
            if (args.Name.Contains("PermissionGenerator")) return _assembly;

            // 2. Extract the simple name (e.g., "MyLibrary, Version=1.0..." -> "MyLibrary")
            string assemblyName = new AssemblyName(args.Name).Name ?? string.Empty;

            // 3. Get the directory of your current plugin/extension
            // Assuming _assembly is your entry point to this folder
            var location = String.IsNullOrEmpty(_assembly.Location) ? AppDomain.CurrentDomain.BaseDirectory : Path.GetDirectoryName(_assembly.Location);

            string expectedPath = Path.Combine(location, assemblyName + ".dll");

            if (File.Exists(expectedPath))
            {
                // Load it into the current context so the BAML reader is happy
                return Assembly.LoadFrom(expectedPath);
            }

            return null;
        }


        // Utility to check some basic type information before the class actually loads
        public static System.Type? ScanRootClass(System.Reflection.Assembly assembly, string bamlPath)
        {
            string xamlName = bamlPath.Trim('\\').Trim('/');
            var qualified = new Uri("pack://application:,,,/" + assembly.GetName().Name + ";component/" + xamlName, UriKind.Absolute);
            //var relative = new Uri(qualified.LocalPath, UriKind.Relative);
            var byteStream = GetAssemblyResource(assembly, bamlPath);
            if (byteStream == null)
            {
                Console.WriteLine($"Failed to load BAML resource: {bamlPath}");

                if (System.IO.File.Exists(qualified.LocalPath))
                {
                    byteStream = File.ReadAllBytes(qualified.LocalPath);
                }
                else
                {
                    Console.WriteLine($"File not found: {qualified.LocalPath}");

                    byteStream = GetAssemblyResource(assembly, qualified.LocalPath.Replace(".baml", ".xaml"));
                    if (byteStream == null)
                    {
                        Console.WriteLine($"Failed to load BAML resource: {bamlPath}");

                        if (System.IO.File.Exists(qualified.LocalPath.Replace(".baml", ".xaml")))
                        {
                            byteStream = File.ReadAllBytes(qualified.LocalPath.Replace(".baml", ".xaml"));
                        }
                        else
                        {
                            Console.WriteLine($"File not found: {qualified.LocalPath.Replace(".baml", ".xaml")}");
                            return null;
                        }
                    }
                }
            }

            return ScanRootClass(assembly, byteStream, qualified);
        }

        // Utility to check some basic type information before the class actually loads
        public static System.Type? ScanRootClass(System.Reflection.Assembly assembly, byte[] byteStream, Uri qualified)
        {
            Type? rootType = null;
            string? xClassValue = null;


            using (var stream = new MemoryStream(byteStream))
            using (var reader = new XamlXmlReader(stream))
            {
                while (reader.Read())
                {
                    // Debug/Console log for every node
                    Console.WriteLine($"[DEBUG] Node: {reader.NodeType,-15} | Type: {reader.Type?.Name,-15} | Member: {reader.Member?.Name,-15} | Value: {reader.Value}");

                    if (reader.NodeType == XamlNodeType.StartObject && rootType == null)
                    {
                        rootType = reader.Type?.UnderlyingType;
                        return rootType;
                    }

                    // Specifically look for the x:Class member on the root object
                    if (reader.NodeType == XamlNodeType.StartMember && reader.Member?.Name == "Class")
                    {
                        if (reader.Read() && reader.NodeType == XamlNodeType.Value)
                        {
                            xClassValue = reader.Value as string;
                        }
                    }


                    if (rootType != null && xClassValue != null)
                    {
                        break;
                    }
                }
            }

            if (rootType == null)
            {
                Console.WriteLine("Failed to determine root type from XAML. Permissions generation will be skipped.");
                return null;
            }

            Console.WriteLine($"--- Final Analysis ---\nRoot Element: {rootType.Name}\nx:Class Target: {xClassValue ?? "None"}\n----------------------");
            return rootType;
        }


        // Utility to check some basic type information before the class actually loads
        public static System.Type? ScanBamlClass(System.Reflection.Assembly assembly, byte[] byteStream, Uri qualified)
        {
            Type? rootType = null;
            string? xClassValue = null;


            using (var stream = new MemoryStream(byteStream))
            using (var reader = new Baml2006Reader(stream, new XamlReaderSettings()
            {
                ProvideLineInfo = true,
                BaseUri = qualified,
                ValuesMustBeString = false,
                LocalAssembly = assembly
            }))
            {
                while (reader.Read())
                {
                    // Debug/Console log for every node
                    Console.WriteLine($"[DEBUG] Node: {reader.NodeType,-15} | Type: {reader.Type?.Name,-15} | Member: {reader.Member?.Name,-15} | Value: {reader.Value}");

                    if (reader.NodeType == XamlNodeType.StartObject && rootType == null)
                    {
                        rootType = reader.Type?.UnderlyingType;
                        return rootType;
                    }

                    // Specifically look for the x:Class member on the root object
                    if (reader.NodeType == XamlNodeType.StartMember && reader.Member?.Name == "Class")
                    {
                        if (reader.Read() && reader.NodeType == XamlNodeType.Value)
                        {
                            xClassValue = reader.Value as string;
                        }
                    }


                    if (reader.NodeType == XamlNodeType.StartMember)
                    {
                        // Use reader.Member.IsDirective to find x:Class
                        if (reader.Member != null && reader.Member.IsDirective && reader.Member.Name == "Class")
                        {
                            reader.Read();
                            xClassValue = reader.Value?.ToString();
                            // This is your Window/UserControl class name!
                        }
                    }


                    if (rootType != null && xClassValue != null)
                    {
                        break;
                    }

                }
            }

            if (rootType == null)
            {
                Console.WriteLine("Failed to determine root type from XAML. Permissions generation will be skipped.");
                return null;
            }

            Console.WriteLine($"--- Final Analysis ---\nRoot Element: {rootType.Name}\nx:Class Target: {xClassValue ?? "None"}\n----------------------");
            return rootType;
        }

        public static object? ConvertBamlToXaml(System.Reflection.Assembly assembly, byte[] byteStream, Uri qualified)
        {
            XamlObjectWriter writer;

            using (var stream = new MemoryStream(byteStream))
            using (var reader = new Baml2006Reader(stream, new XamlReaderSettings()
            {
                ProvideLineInfo = true,
                BaseUri = qualified,
                ValuesMustBeString = false,
                LocalAssembly = assembly
            }))
            {
                writer = new XamlObjectWriter(reader.SchemaContext, new XamlObjectWriterSettings
                {
                    // This is the "Designer Secret": 
                    // You can provide a root object that just ignores everything it doesn't know.
                    IgnoreCanConvert = true
                });
                while (reader.Read())
                {
                    try
                    {
                        writer.WriteNode(reader);
                    }
                    catch { }
                }
            }

            var element = writer.Result;
            if (element == null)
            {
                Console.WriteLine("Failed to parse XAML into a FrameworkElement. Permissions generation will be skipped.");
            }
            return element;
        }


        public static string ConvertBamlToXamlString(System.Reflection.Assembly assembly, byte[] byteStream, Uri qualified)
        {
            // 1. Setup the BAML Reader
            // Note: We provide a generic SchemaContext to avoid being picky about local types
            using (var stream = new MemoryStream(byteStream))
            using (var reader = new Baml2006Reader(stream, new XamlReaderSettings()
            {
                ProvideLineInfo = true,
                BaseUri = qualified,
                ValuesMustBeString = true,
                LocalAssembly = assembly
            }))
            {
                // 2. Setup the XML Writer for the output string
                var sw = new StringWriter();

                using (var xmlWriter = XmlWriter.Create(sw, new XmlWriterSettings { Indent = true, OmitXmlDeclaration = true }))
                {
                    // 3. The XamlXmlWriter is the bridge that turns nodes back into XAML text
                    var xamlWriter = new XamlXmlWriter(xmlWriter, reader.SchemaContext);

                    // 4. THE PIPELINE: Just pull from reader and push to writer
                    // This avoids XamlObjectWriter's "Type Loading" logic
                    XamlServices.Transform(reader, xamlWriter);
                }

                return sw.ToString();
            }
        }

        public static string ConvertXamlObjectToXaml(object obj)
        {
            string xamlString = XamlServices.Save(obj);

            return xamlString;
        }


        public static byte[] DecompileBamlStream(System.Reflection.Assembly assembly, byte[] bamlStream)
        {
            var location = String.IsNullOrEmpty(assembly.Location) ? Path.Join(AppDomain.CurrentDomain.BaseDirectory, assembly.ManifestModule.ScopeName ?? (assembly.GetName().Name + ".dll")) : assembly.Location;
            var resolver = new UniversalAssemblyResolver(location, false, null);
            var peFile = new ICSharpCode.Decompiler.Metadata.PEFile(location);
            //var typeSystem = new DecompilerTypeSystem(peFile, resolver);

            // 2. The Decompiler constructor now takes the TypeSystem
            var decompiler = new ICSharpCode.BamlDecompiler.XamlDecompiler(peFile, resolver, new BamlDecompilerSettings());

            // 3. THIS is the method that replaces the old ReadDocument
            var result = decompiler.Decompile(new MemoryStream(bamlStream));
            XDocument xamlDocument = result.Xaml;
            //var bamlDocument = ICSharpCode.Decompiler.Disassembler..ReadDocument(bamlStream, CancellationToken.None);

            // 3. Return as a clean string for your Token Reader
            var xamlBytes = xamlDocument.ToString();

            return System.Text.Encoding.UTF8.GetBytes(xamlBytes);
        }


        public static void GenerateFromAssembly(System.Reflection.Assembly assembly)
        {
            // TODO: loop through all bamls in assembly
            var bamls = PermissionGenerator.Utilities.GetBamlFiles(assembly);
            Console.WriteLine($"Found {bamls.Count()} BAML files in assembly {assembly.FullName}");

            //foreach (var bam in bamls)
            //{
            //    string xamlName = bam.Trim('\\').Trim('/').Replace(".baml", ".xaml");
            //    Uri uri = new Uri($"/{assembly.GetName().Name};component/{xamlName}", UriKind.Relative);
            //    Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary { Source = uri });
            //}

            foreach (var bam in bamls)
            {
                Console.WriteLine($"Processing BAML: {bam}");
                string xamlName = bam.Trim('\\').Trim('/').Replace(".baml", ".xaml");
                var qualified = new Uri("pack://application:,,,/" + assembly.GetName().Name + ";component/" + xamlName, UriKind.Absolute);
                var relative = new Uri(qualified.LocalPath, UriKind.Relative);
                var byteStream = GetAssemblyResource(assembly, bam);
                if (byteStream == null)
                {
                    Console.WriteLine($"Failed to load BAML resource: {bam}");
                    continue;
                }

                // var root = System.Windows.Application.LoadComponent(relative);
                //var xamlStream = DecompileBamlStream(assembly, byteStream);
                //TODO: try XamlWriter.Save(myObject)
                var info = ScanBamlClass(assembly, byteStream.Skip(4).ToArray(), qualified);
                // TODO: create mock assembly first, so its FullyMocked.QialifiedNamespace.And.Class > CastleMock.Mock.GeneratedClass > Original.Qualified.NamespaceAnd.Class > xClassWPFInheritedControl
                //var obj = ConvertBamlToXaml(assembly, byteStream.Skip(4).ToArray(), qualified);
                var xamlString = ConvertBamlToXamlString(assembly, byteStream.Skip(4).ToArray(), qualified);

                if (info != null)
                {
                    //MakePermissions(new Uri(bam, UriKind.Relative), root, assembly);
                }
                else
                {
                    Console.WriteLine($"Failed to load FrameworkElement for XAML for BAML: {bam}");
                }
            }
        }


        public static FrameworkElement? LoadSideLoadedComponent(System.Reflection.Assembly assembly, string resourceName)
        {
            // resourceName is usually "views/mainwindow.baml" (note the .baml extension)
            string resourcePath = $"{assembly.GetName().Name}.g.resources";
            using (var stream = assembly.GetManifestResourceStream(resourcePath))
            {
                using (var reader = new System.Resources.ResourceReader(stream))
                {
                    var byteStream = new MemoryStream(GetAssemblyResource(assembly, resourceName));
                    // Use internal WPF Reflection or a Stream to load
                    return System.Windows.Markup.XamlReader.Load(byteStream) as FrameworkElement;
                }
            }
        }


        public static byte[]? GetAssemblyResource(System.Reflection.Assembly assembly, string resourceName)
        {
            // resourceName is usually "views/mainwindow.baml" (note the .baml extension)
            string resourcePath = $"{assembly.GetName().Name}.g.resources";
            using (var stream = assembly.GetManifestResourceStream(resourcePath))
            {
                using (var reader = new System.Resources.ResourceReader(stream))
                {
                    reader.GetResourceData(resourceName.ToLower(), out _, out byte[] data);
                    // Use internal WPF Reflection or a Stream to load
                    return data;
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


        public static void MakePermissions(Uri baseUri, FrameworkElement root, System.Reflection.Assembly assembly, string _outputPath = ".")
        {
            var name = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(System.IO.Path.GetFileNameWithoutExtension(baseUri.LocalPath));

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
            string fileName = application.Namespace + "." + defaultNamespace + "." + name + ".p.cs";
            System.IO.File.WriteAllText(System.IO.Path.Combine(_outputPath, fileName), CODE_TEMPLATE);
        }


        public static void GenerateContent()
        {
            // TODO: make a window and page control and put the smaller content in it for display
        }


        public static void GenerateScreenshot()
        {

        }


        public static string[] GetNormalArgs(string[] args2)
        {
            Console.WriteLine("UnFuck these args: " + Environment.CommandLine);
            var args = Environment.CommandLine.Trim('"').ToString().TrimStart(Environment.ProcessPath).TrimStart("dll").Trim("\" ").TrimEnd('\\').ToString().Split('|');
            // This is just to ensure the static constructor runs when the assembly is loaded in the designer.
            foreach (var arg in args)
            {
                Console.WriteLine($"Argument: {arg}");
            }
            return args;
        }

    }
}
