using EPIC.ClearView.Utilities;
using EPIC.ClearView.Utilities.Logging;
using System.Data;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Threading;

namespace EPIC.ClearView
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        public App()
        {
            //Application.ResourceAssembly = typeof(App).Assembly;
        }



        // Token: 0x06000346 RID: 838 RVA: 0x0001AF80 File Offset: 0x00019180
        private void Application_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            try
            {
                e.Handled = true;
                Log.Fatal("A fatal error occurred.", e.Exception);
            }
            catch
            {
            }
        }

        // Token: 0x06000347 RID: 839 RVA: 0x0001AFF0 File Offset: 0x000191F0
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

        }

        internal static void CurrentDomain_AssemblyLoad(object? sender, AssemblyLoadEventArgs args)
        {
            Log.Debug(string.Format("Loaded: {0}", args.LoadedAssembly.FullName));
        }

        internal static System.Reflection.Assembly? CurrentDomain_AssemblyResolve(object? sender, ResolveEventArgs args)
        {
            // fix assembly naming convention expectation from namespace, cropping up somewhere inside i looked in every xaml
            var namespaces = AppDomain.CurrentDomain.GetAssemblies().Select(a => (
                //all: a.GetCustomAttributes().FirstOrDefault(attr => attr.GetType().Name.Contains("Namespace")),
                attr: a.GetCustomAttributes().FirstOrDefault(attr => attr.GetType().Name.Contains("Namespace") && !String.IsNullOrEmpty(attr.GetType().GetProperty("Namespace")?.GetValue(attr) as string)),
                assembly: a
            )).Where(obj => obj.attr != null).Select(obj => (
                attr: obj.attr,
                assembly: obj.assembly,
                root: obj.attr.GetType().GetProperty("Namespace")?.GetValue(obj.attr) as string
            ));

            var existing = namespaces.FirstOrDefault(a => a.root == args.Name.Split(',')[0]).assembly;
            if (existing != null)
            {
                return existing;
            }

            var location = String.IsNullOrEmpty(typeof(App).Assembly.Location) ? AppDomain.CurrentDomain.BaseDirectory : System.IO.Path.GetDirectoryName(typeof(App).Assembly.Location);

            string expectedPath = System.IO.Path.Combine(location, args.Name + ".dll");

            if (System.IO.File.Exists(expectedPath))
            {
                Log.Debug($"Fallback: {expectedPath}");

                // TODO: try with this to fix searching issues?
                //byte[] rawAssembly = File.ReadAllBytes(realFile);
                //_assembly = AppDomain.CurrentDomain.Load(rawAssembly);

                return Assembly.LoadFrom(expectedPath);
            }

            return null;
        }
        // Token: 0x06000348 RID: 840 RVA: 0x0001B078 File Offset: 0x00019278
        internal void Load_Plugins()
        {
            if (this._plugins == null)
            {
                this._plugins = new List<Type>();
                string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? "";
                if (!Directory.Exists(Path.Combine(path, "Plugins")))
                {
                    return;
                }
                string[] files = Directory.GetFiles(Path.Combine(path, "Plugins"));
                foreach (string text in files)
                {
                    if (!(Path.GetExtension(text) != ".dll"))
                    {
                        try
                        {
                            Assembly assembly = Assembly.LoadFrom(text);
                            List<Type> list = (from x in assembly.GetTypes()
                                               where typeof(IPluggable).IsAssignableFrom(x)
                                               select x).ToList<Type>();
                            foreach (Type type in list)
                            {
                                this._plugins.Add(type);
                                RuntimeHelpers.RunClassConstructor(type.TypeHandle);
                            }
                        }
                        catch (Exception ex)
                        {
                            Log.Debug(string.Format("There was an error loading the plugin: {0}", text), ex);
                        }
                    }
                }
            }
        }

        // Token: 0x0400018D RID: 397
        private List<Type> _plugins;

    }
}
