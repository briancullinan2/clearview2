using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

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

        protected override void OnStartup(StartupEventArgs e)
        {
            //Application.ResourceAssembly = typeof(App).Assembly;
            //base.OnStartup(e);

            // Use a full Pack URI to be explicit if the names are mismatched
            // Format: /AssemblyName;component/PathToXAML
            //var resourceUri = new Uri("/EPICClearView;component/" + StartupUri.ToString(), UriKind.Relative);

            //MainWindow = (Window)Application.LoadComponent(resourceUri);
            //MainWindow.Show();
        }

    }
}
