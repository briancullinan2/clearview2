using EPIC.ClearView.Utilities;
using EPIC.ClearView.Utilities.Macros;
using EPIC.MedicalControls.Utilities.Extensions;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Ribbon;
using System.Windows.Input;
using System.Windows.Navigation;

namespace EPIC.ClearView
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static readonly RoutedUICommand ShowTab = new RoutedUICommand("Show Tab", "ShowTab", typeof(MainWindow));
        public static readonly RoutedUICommand ToggleTab = new RoutedUICommand("Toggle Tab", "ToggleTab", typeof(MainWindow));
        public static readonly RoutedUICommand CloseTab = new RoutedUICommand("Close Tab", "CloseTab", typeof(MainWindow));
        public MainWindow()
        {
            ContentRendered += MainWindow_OnContentRendered;

            CommandBindings.Add(new CommandBinding(ShowTab, (s, e) =>
            {
                if (typeof(Type).IsAssignableFrom(e.Parameter?.GetType()))
                {
                    Navigation.ShowTab(e.Parameter as Type);
                }
                else if ((e.Parameter?.ToString() is string parameter) && !string.IsNullOrWhiteSpace(parameter))
                {
                    Navigation.ShowTab(parameter);
                }
            }));
            Resources["ShowTabCommand"] = ShowTab;
            System.Windows.Application.Current.Resources["ShowTabCommand"] = ShowTab;

            CommandBindings.Add(new CommandBinding(ToggleTab, (s, e) =>
            {
                if (typeof(Type).IsAssignableFrom(e.Parameter?.GetType()))
                {
                    Navigation.ToggleTab(e.Parameter as Type);
                }
                else if ((e.Parameter?.ToString() is string parameter) && !string.IsNullOrWhiteSpace(parameter))
                {
                    Navigation.ToggleTab(parameter);
                }
            }));
            Resources["ToggleTabCommand"] = ToggleTab;
            System.Windows.Application.Current.Resources["ToggleTabCommand"] = ToggleTab;

            CommandBindings.Add(new CommandBinding(CloseTab, (s, e) =>
            {
                Navigation.CloseTab(e.Parameter ?? e.OriginalSource ?? e.Source ?? s);
            }));
            Resources["CloseTabCommand"] = CloseTab;
            System.Windows.Application.Current.Resources["CloseTabCommand"] = CloseTab;

            InitializeComponent();
            InputBindings.Add(new InputBinding(new Utilities.Commands.RelayCommand(new Action<object>(this.NextTab), null), new KeyGesture(Key.Tab, ModifierKeys.Control)));
            InputBindings.Add(new InputBinding(new Utilities.Commands.RelayCommand(new Action<object>(this.PreviousTab), null), new KeyGesture(Key.Tab, ModifierKeys.Control | ModifierKeys.Shift)));
            Task.Run(new Action(ClockThread));
            /*
            // apply invert
            
            using Microsoft.Graphics.Canvas.Effects;
            using Microsoft.UI.Composition;
            using Microsoft.UI.Xaml.Hosting;

            var compositor = ElementCompositionPreview.GetElementVisual(this).Compositor;

            // Create the Invert Effect
            var invertEffect = new InvertEffect
            {
                Source = new CompositionEffectSourceParameter("Input")
            };

            var factory = compositor.CreateEffectFactory(invertEffect);
            var effectBrush = factory.CreateBrush();

            // This "grabs" the pixels behind the element to invert them
            effectBrush.SetSourceParameter("Input", compositor.CreateBackdropBrush());

            targetVisual.Brush = effectBrush;
            */

            MedicalControls.Utilities.Macros.Mica.SubscribeFuzz(this, Tabs, BackgroundLayer);
        }

        private void MainWindow_OnContentRendered(object? sender, EventArgs e)
        {
            SplashWindow.MainWindowOnContentRendered();
            ContentRendered -= MainWindow_OnContentRendered;
            UpdateSize();
        }

        // Token: 0x06000353 RID: 851 RVA: 0x0001B508 File Offset: 0x00019708
        private void NextTab(object o)
        {
            TabItem? tabItem = this.Tabs.Items.OfType<TabItem>().FirstOrDefault((TabItem x) => x.IsSelected);
            if (tabItem != null)
            {
                int num = this.Tabs.Items.IndexOf(tabItem);
                this.Tabs.Items.OfType<TabItem>().Skip((num == this.Tabs.Items.Count - 1) ? 0 : (num + 1)).First<TabItem>().IsSelected = true;
            }
        }

        // Token: 0x06000354 RID: 852 RVA: 0x0001B5BC File Offset: 0x000197BC
        private void PreviousTab(object o)
        {
            TabItem? tabItem = this.Tabs.Items.OfType<TabItem>().FirstOrDefault((TabItem x) => x.IsSelected);
            if (tabItem != null)
            {
                int num = this.Tabs.Items.IndexOf(tabItem);
                this.Tabs.Items.OfType<TabItem>().Skip((num == 0) ? (this.Tabs.Items.Count - 1) : (num - 1)).First<TabItem>().IsSelected = true;
            }
        }

        private CancellationTokenSource _cts = new();
        // Token: 0x06000355 RID: 853 RVA: 0x0001B774 File Offset: 0x00019974
        private async void ClockThread()
        {
            using var timer = new PeriodicTimer(TimeSpan.FromMilliseconds(900));
            try
            {
                while (await timer.WaitForNextTickAsync(_cts.Token) && !this._isClosing)
                {
                    try
                    {
                        await Dispatcher.BeginInvoke(new Action(delegate ()
                        {
                            Assembly.Content = string.Format("Assembly: {0}", System.Reflection.Assembly.GetAssembly(typeof(App)));
                            this.Clock.Content = DateTime.Now.ToLongTimeString();
                            //if (DeviceManager.Current.Devices != null && DeviceManager.Current.Devices.Any<IControllable>())
                            //{
                            //    Scanner.Content = string.Format("Scanner: {0}", "Available");
                            //}
                            //else
                            {
                                Scanner.Content = string.Format("Scanner: {0}", "Unavailable");
                            }
                            bool flag;
                            if (CameraInterface.CameraManager.Current.Cameras != null)
                            {
                                flag = !CameraInterface.CameraManager.Current.Cameras.Any((CameraInterface.Interfaces.ICapturable x) => x.DisplayName == ClearViewConfiguration.Current.Device?.Camera);
                            }
                            else
                            {
                                flag = true;
                            }
                            if (!flag)
                            {
                                Camera.Content = string.Format("Camera: {0}", "Available");
                            }
                            else
                            {
                                Camera.Content = string.Format("Camera: {0}", "Unavailable");
                            }
                        }));
                    }
                    catch
                    {
                    }
                    finally
                    {
                    }
                }
            }
            catch (OperationCanceledException) { /* Handle shutdown */ }
        }


        // Token: 0x06000356 RID: 854 RVA: 0x0001B920 File Offset: 0x00019B20
        private void AccountManage_Click(object sender, RoutedEventArgs e)
        {
            Navigation.ShowTab<Pages.Capture.Manage>("capture/manage.xaml", true);
        }

        // Token: 0x06000357 RID: 855 RVA: 0x0001B944 File Offset: 0x00019B44
        private void Patients_Click(object sender, RoutedEventArgs e)
        {
            Navigation.ShowTab<Pages.Patient.Search>("patient/search.xaml", true);
        }

        // Token: 0x06000358 RID: 856 RVA: 0x0001B968 File Offset: 0x00019B68
        private void Captures_Click(object sender, RoutedEventArgs e)
        {
            Navigation.ShowTab<Pages.Capture.Search>("capture/search.xaml", true);
        }

        // Token: 0x06000359 RID: 857 RVA: 0x0001B98C File Offset: 0x00019B8C
        private void UserAdd_Click(object sender, RoutedEventArgs e)
        {
            Navigation.ShowTab<Pages.User.Add>("user/add.xaml", true);
        }

        // Token: 0x0600035A RID: 858 RVA: 0x0001B9B0 File Offset: 0x00019BB0
        private void UserSearch_Click(object sender, RoutedEventArgs e)
        {
            Navigation.ShowTab<Pages.User.Search>("user/search.xaml", true);
        }

        // Token: 0x0600035B RID: 859 RVA: 0x0001B9D4 File Offset: 0x00019BD4
        private void ApplicationSettings_Click(object sender, RoutedEventArgs e)
        {
            Navigation.ShowTab<Pages.Application.Settings>("settings.xaml", true);
        }

        // Token: 0x0600035C RID: 860 RVA: 0x0001B9F8 File Offset: 0x00019BF8
        private void UserPermission_Click(object sender, RoutedEventArgs e)
        {
            Navigation.ShowTab<Pages.Application.Permissions>("permissions.xaml", true);
        }

        // Token: 0x0600035D RID: 861 RVA: 0x0001BA1C File Offset: 0x00019C1C
        private void Calibrate_Click(object sender, RoutedEventArgs e)
        {
            Navigation.ShowTab<Pages.Capture.Calibrate>("calibrate.xaml", true);
        }

        // Token: 0x0600035E RID: 862 RVA: 0x0001BA40 File Offset: 0x00019C40
        private void NewCapture_Click(object sender, RoutedEventArgs e)
        {
            Navigation.ShowTab<Pages.Capture.Scan>("scan.xaml", true);
        }

        // Token: 0x0600035F RID: 863 RVA: 0x0001BAB8 File Offset: 0x00019CB8
        private void Alerts_ViewAll(object sender, RoutedEventArgs e)
        {
            Uri uri = new Uri("/Capture/Alerts.xaml", UriKind.Relative);
            TabItem? tabItem = Navigation.FindTab<Pages.Application.Alerts>();
            if (tabItem != null)
            {
                ((Frame)tabItem.Content).Source = uri;
                tabItem.IsSelected = true;
            }
            else
            {
                Navigation.ShowTab(uri, true);
            }
        }

        // Token: 0x06000360 RID: 864 RVA: 0x0001BB8C File Offset: 0x00019D8C
        private void Alerts_View(object sender, RoutedEventArgs e)
        {
            Uri uri;
            if (Alerts?.FirstOrDefault() != null)
            {
                uri = new Uri("Alerts.xaml?messageId=" + Alerts.First().MessageId, UriKind.Relative);
            }
            else
            {
                uri = new Uri("Alerts.xaml", UriKind.Relative);

            }
            TabItem? tabItem = Navigation.FindTab<Pages.Application.Alerts>(); ;
            if (tabItem != null)
            {
                ((Frame)tabItem.Content).Source = uri;
                tabItem.IsSelected = true;
            }
            else
            {
                Navigation.ShowTab(uri, true);
            }
        }

        // Token: 0x06000361 RID: 865 RVA: 0x0001BC58 File Offset: 0x00019E58
        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            UpdateSize();
        }

        public void UpdateSize()
        {
            RibbonTab ribbonTab = AlertsFace.TryFindParent<RibbonTab>();
            if (ribbonTab == null)
            {
                return;
            }
            RibbonGroup ribbonGroupBox = AlertsFace.TryFindParent<RibbonGroup>();
            double num2 = ribbonGroupBox.Items.OfType<Button>().Sum((Button x) => x.ActualWidth);

            double num3 = ribbonTab.Items.OfType<RibbonGroup>().Except([ribbonGroupBox])
                                         .Sum((RibbonGroup x) => x.ActualWidth);

            double num = ribbonTab.ActualWidth - ribbonTab.Items.OfType<RibbonGroup>().Count() * 10; // + ((e != null) ? (e.NewSize.Width - e.PreviousSize.Width - 20.0) : 0.0);
            double num4 = Math.Round(num - num3 - num2);
            AlertsFace.MaxWidth = ((num4 > AlertsFace.MinWidth) ? num4 : AlertsFace.MinWidth);
            ribbonTab.UpdateLayout();
        }

        // Token: 0x06000362 RID: 866 RVA: 0x0001BD7C File Offset: 0x00019F7C
        public void UpdateAlerts()
        {
            try
            {
                var context = DataLayer.TranslationContext.Current["Data Source=:memory:"];
                if (AlertsFace == null)
                {
                    return;
                }

                //if (this.AlertsBox == null /* || this.AlertsBox.Visibility != Visibility.Visible */)
                //{
                //    return;
                //}
                //this.AlertsBox.ItemsSource = null;
                AlertsFace.ItemsSource = null;
                Alerts = (from x in context.Messages
                          where x.IsActive
                          orderby x.CreateTime descending
                          select x).ToList();
                //this.AlertsBox.ItemsSource = Alerts;
                AlertsFace.ItemsSource = Alerts;
                //this.AlertsBox.SelectedItem = Alerts.FirstOrDefault<DataLayer.Entities.Message>();
                //AlertsFace.Text = Alerts.FirstOrDefault<DataLayer.Entities.Message>()?.Body;
            }
            catch (Exception ex)
            {
                Utilities.Logging.Log.Error("There was an error retrieving the alerts.", ex);
            }
        }

        public IEnumerable<DataLayer.Entities.Message>? Alerts
        {
            get
            {
                return (IEnumerable<DataLayer.Entities.Message>?)base.GetValue(MainWindow.AlertsProperty);
            }
            private set
            {
                base.SetValue(MainWindow.AlertsProperty, value);
            }
        }
        public static readonly DependencyProperty AlertsProperty = DependencyProperty.Register("Alerts", typeof(IEnumerable<DataLayer.Entities.Message>), typeof(MainWindow), new PropertyMetadata(null));


        // Token: 0x06000363 RID: 867 RVA: 0x0001BE84 File Offset: 0x0001A084
        private void ApplicationChangeLogin_Click(object sender, RoutedEventArgs e)
        {
        }

        // Token: 0x06000364 RID: 868 RVA: 0x0001BE9B File Offset: 0x0001A09B
        private void Window_Closing(object sender, CancelEventArgs e)
        {
        }

        // Token: 0x06000365 RID: 869 RVA: 0x0001BEA5 File Offset: 0x0001A0A5
        private void AlertsBox_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            this.UpdateAlerts();
        }

        // Token: 0x06000366 RID: 870 RVA: 0x0001BF0C File Offset: 0x0001A10C
        private void Login_OnLoadCompleted(object sender, NavigationEventArgs e)
        {

        }

        // Token: 0x04000194 RID: 404
        public static readonly DependencyProperty UserProperty = DependencyProperty.Register("User", typeof(object), typeof(MainWindow), new PropertyMetadata(null));
        private bool _isClosing;

        private void AccountWelcome_Checked(object sender, RoutedEventArgs e)
        {
            //Uri uri = new Uri("/Pages/Application/Welcome.xaml", UriKind.Relative);
            Navigation.ShowTab<Pages.Application.Welcome>("welcome.xaml", true);
        }


    }
}
