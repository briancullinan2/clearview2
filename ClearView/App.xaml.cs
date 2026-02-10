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
            AppDomain.CurrentDomain.AssemblyResolve += (sender, args) =>
            {
                // If something looks for "Company.Project", return the current assembly
                if (args.Name.StartsWith("EPIC.ClearView"))
                {
                    return typeof(App).Assembly;
                }
                return null;
            };
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
            log4net.Config.XmlConfigurator.Configure(new System.IO.FileInfo("log4net.xml"));
            base.OnStartup(e);
            AppDomain.CurrentDomain.AssemblyLoad += delegate (object? o, AssemblyLoadEventArgs args)
            {
                Log.Debug(string.Format("Loaded: {0}", args.LoadedAssembly.FullName));
            };
            AppDomain.CurrentDomain.DomainUnload += delegate (object? o, EventArgs args)
            {
            };

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
