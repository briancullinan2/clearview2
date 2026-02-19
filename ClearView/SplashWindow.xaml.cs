using System.Diagnostics;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;
using System.Windows.Threading;

namespace EPIC.ClearView
{
    // Token: 0x02000047 RID: 71
    public partial class SplashWindow : Window
    {
        private static IDictionary<Assembly, int> _loaded = new Dictionary<Assembly, int>();
        // Token: 0x06000256 RID: 598 RVA: 0x00013D64 File Offset: 0x00011F64
        private static void CurrentDomainOnAssemblyLoad(object? sender, AssemblyLoadEventArgs args)
        {
            //SplashWindow._total += args.LoadedAssembly.GetReferencedAssemblies().Except(_loaded.Keys.Select(ass => ass.GetName())).Count();
            SplashWindow._completed++;
            if (SplashWindow._splash == null)
            {
                return;
            }
            SplashWindow._splash.Dispatcher.Invoke(delegate ()
            {
                SplashWindow._splash.Version.Content = System.Reflection.Assembly.GetExecutingAssembly()
                                          .GetCustomAttribute<System.Reflection.AssemblyInformationalVersionAttribute>()?
                                          .InformationalVersion; ;
                SplashWindow._splash.Message.Content = string.Format("Loaded: {0}", args.LoadedAssembly.FullName);
                SplashWindow._splash.Progress.Value = (double)((int)((double)SplashWindow._completed / (double)SplashWindow._total * 100.0));

            });
            //SplashWindow._splash.Dispatcher.Invoke(() => { }, System.Windows.Threading.DispatcherPriority.Render);
            //Dispatcher.PushFrame(new DispatcherFrame());
            SplashWindow.DoEvents();
        }

        // Token: 0x06000257 RID: 599 RVA: 0x00013DA2 File Offset: 0x00011FA2
        private static void AppOnNavigated(object sender, NavigationEventArgs navigationEventArgs)
        {
            MainWindowOnContentRendered();
            SplashWindow._app.Navigated -= SplashWindow.AppOnNavigated;
        }

        private static void AppOnActivated(object? sender, EventArgs e)
        {
            MainWindowOnContentRendered();
            _app.Activated -= SplashWindow.AppOnActivated;
        }

        // Token: 0x06000258 RID: 600 RVA: 0x00013E5C File Offset: 0x0001205C
        public static void MainWindowOnContentRendered()
        {
            if (_splash == null)
            {
                return;
            }

            SplashWindow._app.Dispatcher.BeginInvoke(new Action(delegate ()
            {
                if (_splash == null)
                {
                    return;
                }

                SplashWindow._app.MainWindow.Activate();
                SplashWindow._app.Load_Plugins();
                AppDomain.CurrentDomain.AssemblyLoad -= SplashWindow.CurrentDomainOnAssemblyLoad;

                // hide this last
                SplashWindow._splash.Close();
                _splash = null;
            }), DispatcherPriority.ApplicationIdle, new object[0]);
        }

        /*
        public string Version
        {
            get
            {
                var infoVersion = Assembly.GetExecutingAssembly()
                                          .GetCustomAttribute<AssemblyInformationalVersionAttribute>()?
                                          .InformationalVersion;
                return infoVersion;
            }
        }
        */

        public new object Icon
        {
            get
            {
                return System.Windows.Application.Current.MainWindow.Icon;
            }
        }

        public bool Interrupted
        {
            get
            {
                return (bool)base.GetValue(InterruptedProperty);
            }
            set
            {
                base.SetValue(InterruptedProperty, value);
            }
        }
        public static readonly DependencyProperty InterruptedProperty = DependencyProperty.Register("Interrupted", typeof(bool), typeof(SplashWindow), new PropertyMetadata(false));


        // Token: 0x06000259 RID: 601 RVA: 0x00013EBE File Offset: 0x000120BE
        private SplashWindow()
        {
            // MAKE IT EARLIER!
            //AppDomain.CurrentDomain.AssemblyLoad += SplashWindow.CurrentDomainOnAssemblyLoad;
            this.InitializeComponent();
        }

        // Token: 0x0600025A RID: 602 RVA: 0x00013ED0 File Offset: 0x000120D0
        //[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        [DebuggerStepThrough]
        private static void DoEvents()
        {
            DispatcherFrame dispatcherFrame = new DispatcherFrame();
            Dispatcher.CurrentDispatcher?.BeginInvoke(DispatcherPriority.Render, new DispatcherOperationCallback(SplashWindow.ExitFrames), dispatcherFrame);
            try
            {
                Dispatcher.PushFrame(dispatcherFrame);
            }
            catch (Exception ex) when (ex is InvalidOperationException or NullReferenceException)
            {
            }
        }


        // Token: 0x0600025B RID: 603 RVA: 0x00013F20 File Offset: 0x00012120
        private static object ExitFrames(object frame)
        {
            ((DispatcherFrame)frame).Continue = false;
            return null;
        }

        [STAThread] // Required for WPF UI threads
        public static void Main()
        {
            log4net.Config.XmlConfigurator.Configure(new System.IO.FileInfo("log4net.xml"));
            SplashWindow._total += Assembly.GetEntryAssembly().GetReferencedAssemblies().Count();
            AppDomain.CurrentDomain.AssemblyLoad += SplashWindow.CurrentDomainOnAssemblyLoad;
            AppDomain.CurrentDomain.AssemblyResolve += App.CurrentDomain_AssemblyResolve;
            AppDomain.CurrentDomain.AssemblyLoad += App.CurrentDomain_AssemblyLoad;
            AppDomain.CurrentDomain.DomainUnload += delegate (object? o, EventArgs args)
            {
                AppDomain.CurrentDomain.AssemblyLoad -= SplashWindow.CurrentDomainOnAssemblyLoad;
                AppDomain.CurrentDomain.AssemblyResolve -= App.CurrentDomain_AssemblyResolve;
                AppDomain.CurrentDomain.AssemblyLoad -= App.CurrentDomain_AssemblyLoad;
            };

            _splash = new SplashWindow();

            _splash.Dispatcher.Invoke(new Action(_splash.StartApp));

            _splash.ShowDialog();
        }


        private CancellationTokenSource _cts = new();

        [STAThread] // Required for WPF UI threads
        private async void StartApp()
        {
            // Set to 15ms
            using var timer = new PeriodicTimer(TimeSpan.FromMilliseconds(1000));

            try
            {
                while (await timer.WaitForNextTickAsync(_cts.Token))
                {

                    // Check for 'Shift' or 'F12' during splash
                    if (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.F12) || Keyboard.IsKeyDown(Key.F8) || Keyboard.IsKeyDown(Key.Pause))
                    {
                        // TODO ConfigurationCombo.Visibility = Visible
                        Interrupted = true;
                        return;
                    }

                    // 1. Initialize the App instance manually
                    _app = new EPIC.ClearView.App();
                    _app.Navigated += AppOnNavigated;
                    _app.Activated += AppOnActivated;
                    _app.InitializeComponent();

                    // 3. Start the app loop
                    // This will block until app.Shutdown() is called
                    _app.Run();
                }
            }
            catch (OperationCanceledException)
            { /* Handle shutdown */
            }
        }

        // Token: 0x04000136 RID: 310
        private static SplashWindow? _splash;

        // Token: 0x04000137 RID: 311
        private static int _total;

        // Token: 0x04000138 RID: 312
        private static int _completed;

        // Token: 0x04000139 RID: 313
        private static App _app;

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            StartApp();
        }
    }
}
