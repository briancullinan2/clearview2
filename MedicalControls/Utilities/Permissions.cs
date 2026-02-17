using EPIC.DataLayer;
using EPIC.MedicalControls.Utilities.Extensions;
using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Baml2006;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Controls.Ribbon;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xaml;
using System.Xml;
using System.Xml.Linq;

namespace EPIC.MedicalControls.Utilities
{
    public class DesignTimePermissionsViewModel : DependencyObject, INotifyPropertyChanged
    {
        public Page? Page { get; set; }

        public ObservableCollection<DataLayer.Entities.Role>? RolesData { get; set; }

        public ObservableCollection<DataLayer.Entities.Permission>? PermissionData { get; set; }

        public IEnumerable<RibbonTab> RibbonTabs
        {
            /*  get;
            {
                if (DesignerProperties.GetIsInDesignMode(new DependencyObject()))
                {
                }

                if (Page == null)
                {
                    return [];
                }
                return Page.Resources.Values.OfType<RibbonTab>();
            }
            */
            get
            {
                return (IEnumerable<RibbonTab>)base.GetValue(RibbonTabsProperty);
            }
            private set
            {
                base.SetValue(RibbonTabsProperty, value);
            }

        }

        public static readonly DependencyProperty RibbonTabsProperty =
        DependencyProperty.RegisterAttached("RibbonTabs", typeof(IEnumerable<RibbonTab>), typeof(DesignTimePermissionsViewModel),
        new PropertyMetadata(new Collection<RibbonTab>()));

        public static readonly DependencyProperty PassPageToViewModelProperty =
        DependencyProperty.RegisterAttached("PassPageToViewModel", typeof(bool), typeof(DesignTimePermissionsViewModel),
        new PropertyMetadata(false, (d, e) =>
        {
            if (d is Page p && p.DataContext is DesignTimePermissionsViewModel vm)
                vm.RibbonTabs = p.Resources.Values.OfType<RibbonTab>(); // ViewModel property set at design time
        }));
        public static void SetPassPageToViewModel(UIElement element, bool value) => element.SetValue(PassPageToViewModelProperty, value);
        public static bool GetPassPageToViewModel(UIElement element) => (bool)element.GetValue(PassPageToViewModelProperty);

        public event PropertyChangedEventHandler? PropertyChanged;


        public DesignTimePermissionsViewModel()
        {


            if (DesignerProperties.GetIsInDesignMode(new DependencyObject()))
            {
                RolesData = new ObservableCollection<DataLayer.Entities.Role>
                {
                    new DataLayer.Entities.Role { Name = "Administrator", Description = "Full system access" },
                    new DataLayer.Entities.Role { Name = "Read Only", Description = "View only permissions" },
                    new DataLayer.Entities.Role { Name = "Patient Data", Description = "Access to medical records" }
                };
            }
            else
            {
                RolesData = new ObservableCollection<DataLayer.Entities.Role>(TranslationContext.Current["Data Source=:memory:"].Roles.ToList());
                PermissionData = new ObservableCollection<DataLayer.Entities.Permission>(TranslationContext.Current["Data Source=:memory:"].Permissions.ToList());

            }
        }
    }
    public static class Permissions
    {

        public static IEnumerable<string> GetBamlFiles()
        {
            var assembly = Assembly.GetExecutingAssembly();
            return GetBamlFiles(assembly);
        }

        public static IEnumerable<string> GetBamlFiles(string assemblyFile)
        {
            var assembly = Assembly.LoadFrom(assemblyFile);
            return GetBamlFiles(assembly);
        }
        public static IEnumerable<string> GetBamlFiles(Assembly assembly)
        {
            // BAML resources are usually stored in [AssemblyName].g.resources
            var resourceName = assembly.GetName().Name + ".g.resources";

            using (var stream = assembly.GetManifestResourceStream(resourceName))
            {
                if (stream == null)
                    yield break;

                using (var reader = new System.Resources.ResourceReader(stream))
                {
                    foreach (DictionaryEntry entry in reader)
                    {
                        if (entry.Key.ToString().EndsWith(".baml"))
                        {
                            // We found a page! Now we introspect it.
                            yield return entry.Key.ToString();
                        }
                    }
                }
            }
        }


        public static IEnumerable<DataLayer.Entities.Permission> IntrospectXaml(System.Reflection.Assembly assembly, FrameworkElement root, string name)
        {

            if (root == null)
                return [];

            var titleNamespace = Path.GetDirectoryName(name)?.Replace('\\', '/').Split('/')
                                     .Select(s => System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(s));
            string defaultNamespace = string.Join(".", titleNamespace ?? []);
            var allChildren = root.GetAllLogicalChildren().Concat(root.GetAllChildren()).Distinct();
            var controlChildren = allChildren
                                  .OfType<FrameworkElement>()
                                  .Where(e => e is ICommandSource || e is ButtonBase || !String.IsNullOrWhiteSpace(e.Name) || !String.IsNullOrWhiteSpace(e.GetDescriptor()));
            var permissions = controlChildren.Select(e => new DataLayer.Entities.Permission
            {
                Name = e.BuildAncestralAddress(root), // Using your JS-style logic
                Description = "Interaction access to " + e.GetType().Name + " labeled " + e.GetDescriptor() + " in " + name.Replace(".baml", ".xaml"),
                IsActionable = e is ICommandSource || e is ButtonBase
            });
            return permissions;
        }

        /*
        public static IEnumerable<DataLayer.Entities.Permission?> IntrospectXDocument(XDocument root, string bamlName)
        {
            if (root?.Root == null) yield return null;

            // List of local names that imply ICommandSource or ButtonBase in WPF
            var actionableTypes = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                "Button", "MenuItem", "CheckBox", "RadioButton", "RepeatButton", "Hyperlink"
            };

            // Flatten the document in order
            var allChildren = root.Root.DescendantsAndSelf();
            foreach (var child in allChildren)
            {
                string typeName = child.Name.LocalName;
                string descriptor = child.GetNameAttribute(); // Uses our XElement extension for Content/Header
                bool isActionable = actionableTypes.Contains(typeName);

                // Filter: Only create permissions for actionable items or items with descriptive labels
                if (!isActionable && string.IsNullOrWhiteSpace(descriptor))
                    continue;

                yield return new DataLayer.Entities.Permission
                {
                    // Uses BuildXAddress from our previous extension
                    Name = $"{child.BuildXAddress()}.{typeName}",

                    Description = $"Interaction access to {typeName} labeled '{descriptor}' in the {bamlName} baml",

                    IsActionable = isActionable
                };
            }
        }
        */

        public static IEnumerable<DataLayer.Entities.Permission> IntrospectXaml(System.Reflection.Assembly assembly, string bamlPath)
        {
            // Load the object graph without rendering it
            var qualified = new Uri("pack://application:,,,/" + assembly.GetName().Name + ";component/" + bamlPath, UriKind.Absolute);
            var relative = new Uri(qualified.LocalPath, UriKind.Relative);
            var byteStream = GetAssemblyResource(assembly, bamlPath);
            var info = ScanBamlClass(assembly, byteStream.Skip(4).ToArray(), qualified);
            if (typeof(Application).IsAssignableFrom(info))
            {
                return [];
            }
            var root = Application.LoadComponent(new Uri(bamlPath.Replace(".baml", ".xaml"), UriKind.Relative)) as FrameworkElement;
            //var name = Path.GetFileNameWithoutExtension();

            return IntrospectXaml(assembly, root, bamlPath);
        }

        /*

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
                //byte[] rawAssembly1 = File.ReadAllBytes(System.IO.Path.Combine(Environment.CurrentDirectory, ".dll"));
                //_assembly = AppDomain.CurrentDomain.Load(rawAssembly1);
                //byte[] rawAssembly2 = File.ReadAllBytes(System.IO.Path.Combine(Environment.CurrentDirectory, "CameraInterface.dll"));
                //_assembly = AppDomain.CurrentDomain.Load(rawAssembly2);
                byte[] rawAssembly = File.ReadAllBytes(realFile);
                _assembly = AppDomain.CurrentDomain.Load(rawAssembly);
                //AppDomain.CurrentDomain.
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

            var existing = AppDomain.CurrentDomain.GetAssemblies()
                                                    .FirstOrDefault(a => a.GetName().Name == assemblyName);
            if (existing != null)
            {
                return existing;
            }

            // 3. Get the directory of your current plugin/extension
            // Assuming _assembly is your entry point to this folder
            var location = String.IsNullOrEmpty(_assembly.Location) ? AppDomain.CurrentDomain.BaseDirectory : Path.GetDirectoryName(_assembly.Location);

            string expectedPath = Path.Combine(location, assemblyName + ".dll");

            if (File.Exists(expectedPath))
            {
                Console.WriteLine($"Expected {expectedPath}");
                // Load it into the current context so the BAML reader is happy
                return Assembly.LoadFrom(expectedPath);
            }

            return null;
        }
        */


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
            var assemblies = AppDomain.CurrentDomain.GetAssemblies()
                 .Where(a => a.FullName.Contains("EPICClearView") || a.FullName.Contains("CustomEnums"))
                 .ToList();

            // 2. Create a Schema Context that EXPLICITLY knows about them
            var schemaContext = new XamlSchemaContext(assemblies);
            // 1. Setup the BAML Reader
            // Note: We provide a generic SchemaContext to avoid being picky about local types
            using (var stream = new MemoryStream(byteStream))
            using (var reader = new Baml2006Reader(stream, new XamlReaderSettings()
            {
                ProvideLineInfo = true,
                BaseUri = qualified,
                ValuesMustBeString = true,
                LocalAssembly = assembly,
            })
            /*{
                SchemaContext = schemaContext
            }*/)
            {
                // 2. Setup the XML Writer for the output string
                var sw = new StringWriter();

                using (var xmlWriter = XmlWriter.Create(sw, new XmlWriterSettings { Indent = true, OmitXmlDeclaration = true }))
                {
                    var writer = new XamlXmlWriter(xmlWriter, reader.SchemaContext);

                    while (reader.Read())
                    {
                        try
                        {
                            // 1. ALWAYS write Namespaces and Objects
                            if (reader.NodeType == XamlNodeType.NamespaceDeclaration ||
                                reader.NodeType == XamlNodeType.StartObject ||
                                reader.NodeType == XamlNodeType.EndObject)
                            {
                                writer.WriteNode(reader);
                                continue;
                            }

                            // 2. The Logic for Members (Properties)
                            if (reader.NodeType == XamlNodeType.StartMember)
                            {
                                var m = reader.Member;

                                // Essential for the tree: Key, Items, Initialization, and Directives
                                if (m.IsDirective || m.Name == "_Items" || m.Name == "_Initialization")
                                {
                                    writer.WriteNode(reader);
                                    continue;
                                }

                                // Check if the member is a complex type (could contain more UI elements)
                                // If it's a Collection or a complex Class, we MUST write the StartMember
                                if (m.Type != null && (m.Type.IsCollection || m.Type.IsDictionary || !m.Type.IsArray))
                                {
                                    writer.WriteNode(reader);
                                    continue;
                                }

                                // If we got here, it's a simple property (Width, Margin, etc.) -> SKIP
                                reader.Skip();
                                continue;
                            }

                            // 3. Catch-all for EndMember and Value
                            writer.WriteNode(reader);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"[SKIPPING NODE] {reader.NodeType}: {ex.Message}");
                            if (reader.NodeType == XamlNodeType.StartObject)
                            {
                                reader.Skip();
                            }
                        }
                    }
                    writer.Close();


                    return sw.ToString();
                }
            }
        }

        public static XDocument ConvertBamlToXDocument(System.Reflection.Assembly assembly, byte[] byteStream, Uri qualified)
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
                var sw = new XDocument();

                using (var xmlWriter = sw.CreateWriter())
                {
                    var writer = new XamlXmlWriter(xmlWriter, reader.SchemaContext);

                    while (reader.Read())
                    {
                        try
                        {
                            // Attempt to write the current node
                            writer.WriteNode(reader);
                        }
                        catch (Exception ex) when (ex is XamlXmlWriterException || ex is InvalidOperationException || ex is KeyNotFoundException)
                        {
                            // A.R.S. § 18-105: Log the failure but maintain system availability
                            Console.WriteLine($"[SKIPPING NODE] {reader.NodeType}: {ex.Message}");

                            // If the failure happened at the start of an object, we must skip its children
                            if (reader.NodeType == XamlNodeType.StartObject)
                            {
                                reader.Skip();
                            }
                            // Note: If a StartMember fails, the writer usually stays in a valid state
                            // but a StartObject failure requires skipping to the matching EndObject.
                        }
                        catch { }
                    }
                    writer.Close();
                }

                return sw;
            }
        }



        public static string ConvertXamlObjectToXaml(object obj)
        {
            string xamlString = XamlServices.Save(obj);

            return xamlString;
        }


        /*

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

        private static ProxyGenerator _generator = new ProxyGenerator();

        public class XamlInterceptor : IInterceptor
        {
            private readonly Dictionary<string, object> _values = new();

            public void Intercept(IInvocation invocation)
            {
                string name = invocation.Method.Name;

                if (name.Contains("Source") && name.Contains("set_") /* invocation.Proxy is ResourceDictionary dict *//*)
                {
                    var uri = invocation.Arguments[0] as Uri;
                    // Logic: Redirect "/MedicalControls;component/Themes/ClearView.xaml" to a local file path or embedded resource
                    // invocation.Arguments[0] = new Uri("C:/SaaS/Resources/clearview.xaml");
                }

                // 1. Property Setters (e.g., Background="Red")
                if (name.StartsWith("set_"))
                {
                    _values[name.Substring(4)] = invocation.Arguments[0];
                    invocation.Proceed(); // Still call the base for real WPF DPs
                    return;
                }

                // 2. Event Wire-ups (e.g., Click="Submit_Click")
                // We catch 'add_Click' and just return. No MethodNotFoundException.
                if (name.StartsWith("add_") || name.StartsWith("remove_"))
                {
                    return;
                }

                invocation.Proceed();
            }
        }

        public static Type CreateIdentityTheftType(System.Type rootType)
        {

            // Create an extended object of the root type from XamlAutoMock
            var scope = new Castle.DynamicProxy.ModuleScope(
                    false,
                    true, // We don't want to deal with strong naming
                    "EPICClearView", // Default/Ignore
                    "EPICClearView.dll", // Default/Ignore
                    "EPIC.ClearView", // <--- THIS is your targetNamespace
                    "EPICClearView.dll"  // <--- THIS is the "fake" DLL name
                );


            // 3. Bridge it to the Generator
            var builder = new DefaultProxyBuilder(scope);
            var generator = new ProxyGenerator(builder);
            var mockInstance = _generator.CreateClassProxy(rootType, new XamlInterceptor());

            // 2. We inherit from UserControl so it has a Template, Content, etc.
            // Castle generates a type like 'Castle.Proxies.UserControlProxy'
            // But our NamingStrategy will force it to be 'Target.Namespace.ClassName'
            var proxyType = builder.CreateClassProxyType(
                typeof(System.Windows.Controls.UserControl),
                Type.EmptyTypes,
                ProxyGenerationOptions.Default
            );

            return proxyType;
        }
        */

        public static void GenerateFromAssembly(System.Reflection.Assembly assembly)
        {
            // TODO: loop through all bamls in assembly
            var bamls = Permissions.GetBamlFiles(assembly);
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

                //var xamlStream = DecompileBamlStream(assembly, byteStream);
                //TODO: try XamlWriter.Save(myObject)
                var info = ScanBamlClass(assembly, byteStream.Skip(4).ToArray(), qualified);
                // TODO: create mock assembly first, so its FullyMocked.QialifiedNamespace.And.Class > CastleMock.Mock.GeneratedClass > Original.Qualified.NamespaceAnd.Class > xClassWPFInheritedControl
                //var xamlString = ConvertBamlToXamlString(assembly, byteStream.Skip(4).ToArray(), qualified);
                //var component = LoadSideLoadedComponent(assembly, qualified.LocalPath);
                //Console.WriteLine(xamlString);
                //var xdoc = ConvertBamlToXDocument(assembly, byteStream.Skip(4).ToArray(), qualified);
                //var permissions = Utilities.IntrospectXDocument(xdoc, qualified.LocalPath).ToList();


                //var mock = CreateIdentityTheftType(info);
                //var obj = ConvertBamlToXaml(assembly, byteStream.Skip(4).ToArray(), qualified);
                //var embeddedApp = assembly.GetTypes().FirstOrDefault(t => typeof(System.Windows.Application).IsAssignableFrom(t) && !t.IsAbstract);
                //var embedded = Activator.CreateInstance(embeddedApp) as System.Windows.Application;
                //var loadComponentMethod = embeddedApp.GetMethod("LoadComponent",
                //                                                        BindingFlags.Static | BindingFlags.Public,
                //                                                        null,
                //                                                        new[] { typeof(Uri) },
                //                                                        null);
                //var root = loadComponentMethod.Invoke(embeddedApp, new object[] { relative }); ;
                //var permissions = Utilities.IntrospectXaml("/" + assembly.GetName().Name + ";component/" + String.Join("", qualified.Segments.Skip(2).ToArray()));


                //foreach (var permit in permissions)
                //{
                //    Console.WriteLine($@"Permission: {permit.Name} - {permit.Description}");
                //}



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
                    reader.GetResourceData(resourceName.ToLower(), out _, out byte[] data);
                    // Use internal WPF Reflection or a Stream to load
                    return System.Windows.Markup.XamlReader.Load(new MemoryStream(data)) as FrameworkElement;
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

            var Permissions = IntrospectXaml(assembly, root as FrameworkElement, baseUri.LocalPath);
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

        public static RenderTargetBitmap RenderBitmap(UIElement element)
        {
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
            rtb.Freeze();
            return rtb;
        }

        public static void SaveBitmap(UIElement element, string path)
        {
            // Placeholder: Implement logic to render the UIElement to a bitmap and save it
            // This would involve using RenderTargetBitmap and encoding it to PNG or JPEG
            // CRITICAL: This allows relative paths like "/MedicalControls;component/Themes/ClearView.xaml" to resolve
            var rtb = RenderBitmap(element);

            // 4. Save to a file so you can open it
            var encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(rtb));

            using (var stream = System.IO.File.Create(Path.Join(path, "RenderedView.png")))
            {
                encoder.Save(stream);
            }
            Console.WriteLine("Visual capture saved to " + path + "/RenderedView.png");
        }

    }
}
