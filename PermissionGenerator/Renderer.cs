using Castle.DynamicProxy;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xaml;

namespace EPIC.PermissionGenerator
{
    public class XamlInterceptor : IInterceptor
    {
        private readonly Dictionary<string, object> _values = new();

        public void Intercept(IInvocation invocation)
        {
            string name = invocation.Method.Name;

            if (name.Contains("Source") && name.Contains("set_") /* invocation.Proxy is ResourceDictionary dict */)
            {
                var uri = invocation.Arguments[0] as Uri;
                // Logic: Redirect "themes/clearview.xaml" to a local file path or embedded resource
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

    /*
    public class IdentityTheftNamingScope : INamingScope
    {
        public string GetNextFullName(Type baseType) => baseType.FullName; // NO SUFFIX
        public INamingScope SafeSubScope() => this;
    }
    */

    public static class Renderer
    {

        private static readonly ProxyGenerator _generator = new ProxyGenerator();

        [STAThread]
        public static void Main(string[] args2)
        {
            if (System.Windows.Application.Current == null)
            {
                new System.Windows.Application { ShutdownMode = ShutdownMode.OnExplicitShutdown };
            }

            string xamlString = File.ReadAllText(args[0]);
            UIElement? rootElement;
            /*
            {
                rootElement = (UIElement)XamlReader.Load(stream);
            }
            */


            Type? rootType = null;
            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(xamlString)))
            using (var reader = new XamlXmlReader(stream))
            {
                while (reader.Read())
                {
                    if (reader.NodeType == XamlNodeType.StartObject)
                    {
                        rootType = reader.Type?.UnderlyingType;
                        break;
                    }
                }
                // TODO: make a long console message that describes the whole reader state
            }

            if (rootType == null)
            {
                Console.WriteLine("Failed to determine root type from XAML. Permissions generation will be skipped.");
                return;
            }

            // Create an extended object of the root type from XamlAutoMock
            var scope = new ModuleScope(
                false,
                true, // We don't want to deal with strong naming
                      //new IdentityTheftNamingScope(),
                "EPICClearView", // Default/Ignore
                "EPICClearView.dll", // Default/Ignore
                "EPIC.ClearView", // <--- THIS is your targetNamespace
                "EPICClearView.dll"  // <--- THIS is the "fake" DLL name
            );


            // 3. Bridge it to the Generator
            var builder = new DefaultProxyBuilder(scope);
            var generator = new ProxyGenerator(builder);
            var mockInstance = _generator.CreateClassProxy(rootType, new XamlInterceptor());

            var cleanXaml = Regex.Replace(xamlString, @"x:Class=""[^""]*""", "");
            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(cleanXaml)))
            using (var reader = new XamlXmlReader(stream))
            using (var writer = new XamlObjectWriter(reader.SchemaContext, new XamlObjectWriterSettings
            {
                IgnoreCanConvert = true,
                RootObjectInstance = mockInstance // This allows any unknown types to be mocked instead of throwing an error
            }))
            {
                XamlServices.Transform(reader, writer);
                rootElement = writer.Result as UIElement;
            }

            if (rootElement == null)
            {
                Console.WriteLine("Failed to parse XAML into a FrameworkElement. Permissions generation will be skipped.");
                return;
            }

            SaveBitmap(rootElement, args[3]);

            SaveElement(rootElement, args[3]);
        }

        public static void SaveBitmap(UIElement element, string path)
        {
            // Placeholder: Implement logic to render the UIElement to a bitmap and save it
            // This would involve using RenderTargetBitmap and encoding it to PNG or JPEG
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

            using (var stream = System.IO.File.Create(Path.Join(path, "RenderedView.png")))
            {
                encoder.Save(stream);
            }
            Console.WriteLine("Visual capture saved to " + path + "/RenderedView.png");
        }

        public static void SaveElement(UIElement element, string path)
        {
            string xamlString = System.Windows.Markup.XamlWriter.Save(element);
            File.WriteAllText(Path.Join(path, "RenderedXaml.xaml"), xamlString);
        }
    }
}
