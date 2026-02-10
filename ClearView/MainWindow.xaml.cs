using EPIC.ClearView.Macros;
using EPIC.ClearView.Utilities.Logging;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace EPIC.ClearView
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            ContentRendered += MainWindow_OnContentRendered;
            InitializeComponent();
        }

        private void MainWindow_OnContentRendered(object? sender, EventArgs e)
        {
            SplashWindow.MainWindowOnContentRendered();
            ContentRendered -= MainWindow_OnContentRendered;
        }

        // Token: 0x06000353 RID: 851 RVA: 0x0001B508 File Offset: 0x00019708
        private void NextTab(object o)
        {
            TabItem tabItem = this.Tabs.Items.OfType<TabItem>().FirstOrDefault((TabItem x) => x.IsSelected);
            if (tabItem != null)
            {
                int num = this.Tabs.Items.IndexOf(tabItem);
                this.Tabs.Items.OfType<TabItem>().Skip((num == this.Tabs.Items.Count - 1) ? 0 : (num + 1)).First<TabItem>().IsSelected = true;
            }
        }

        // Token: 0x06000354 RID: 852 RVA: 0x0001B5BC File Offset: 0x000197BC
        private void PreviousTab(object o)
        {
            TabItem tabItem = this.Tabs.Items.OfType<TabItem>().FirstOrDefault((TabItem x) => x.IsSelected);
            if (tabItem != null)
            {
                int num = this.Tabs.Items.IndexOf(tabItem);
                this.Tabs.Items.OfType<TabItem>().Skip((num == 0) ? (this.Tabs.Items.Count - 1) : (num - 1)).First<TabItem>().IsSelected = true;
            }
        }

        // Token: 0x06000355 RID: 853 RVA: 0x0001B774 File Offset: 0x00019974
        private void ClockThread()
        {
        }

        // Token: 0x06000356 RID: 854 RVA: 0x0001B920 File Offset: 0x00019B20
        private void AccountManage_Click(object sender, RoutedEventArgs e)
        {
            Uri uri = new Uri("/Capture/Manage.xaml", UriKind.Relative);
            Navigation.ShowTab(uri, true);
        }

        // Token: 0x06000357 RID: 855 RVA: 0x0001B944 File Offset: 0x00019B44
        private void Patients_Click(object sender, RoutedEventArgs e)
        {
            Uri uri = new Uri("/Patient/Search.xaml", UriKind.Relative);
            Navigation.ShowTab(uri, true);
        }

        // Token: 0x06000358 RID: 856 RVA: 0x0001B968 File Offset: 0x00019B68
        private void Captures_Click(object sender, RoutedEventArgs e)
        {
            Uri uri = new Uri("/Capture/Search.xaml", UriKind.Relative);
            Navigation.ShowTab(uri, true);
        }

        // Token: 0x06000359 RID: 857 RVA: 0x0001B98C File Offset: 0x00019B8C
        private void UserAdd_Click(object sender, RoutedEventArgs e)
        {
            Uri uri = new Uri("/User/Add.xaml", UriKind.Relative);
            Navigation.ShowTab(uri, false);
        }

        // Token: 0x0600035A RID: 858 RVA: 0x0001B9B0 File Offset: 0x00019BB0
        private void UserSearch_Click(object sender, RoutedEventArgs e)
        {
            Uri uri = new Uri("/User/Search.xaml", UriKind.Relative);
            Navigation.ShowTab(uri, true);
        }

        // Token: 0x0600035B RID: 859 RVA: 0x0001B9D4 File Offset: 0x00019BD4
        private void ApplicationSettings_Click(object sender, RoutedEventArgs e)
        {
            Uri uri = new Uri("/Capture/Settings.xaml", UriKind.Relative);
            Navigation.ShowTab(uri, true);
        }

        // Token: 0x0600035C RID: 860 RVA: 0x0001B9F8 File Offset: 0x00019BF8
        private void UserPermission_Click(object sender, RoutedEventArgs e)
        {
            Uri uri = new Uri("/User/Permissions.xaml", UriKind.Relative);
            Navigation.ShowTab(uri, true);
        }

        // Token: 0x0600035D RID: 861 RVA: 0x0001BA1C File Offset: 0x00019C1C
        private void Calibrate_Click(object sender, RoutedEventArgs e)
        {
            Uri uri = new Uri("/Capture/Calibrate.xaml", UriKind.Relative);
            Navigation.ShowTab(uri, true);
        }

        // Token: 0x0600035E RID: 862 RVA: 0x0001BA40 File Offset: 0x00019C40
        private void NewCapture_Click(object sender, RoutedEventArgs e)
        {
        }

        // Token: 0x0600035F RID: 863 RVA: 0x0001BAB8 File Offset: 0x00019CB8
        private void Alerts_ViewAll(object sender, RoutedEventArgs e)
        {
        }

        // Token: 0x06000360 RID: 864 RVA: 0x0001BB8C File Offset: 0x00019D8C
        private void Alerts_View(object sender, RoutedEventArgs e)
        {
        }

        // Token: 0x06000361 RID: 865 RVA: 0x0001BC58 File Offset: 0x00019E58
        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
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

                if (this.AlertsBox == null /* || this.AlertsBox.Visibility != Visibility.Visible */)
                {
                    return;
                }
                this.AlertsBox.ItemsSource = null;
                this.AlertsBox.ItemsSource = (from x in context.Messages
                                              where x.IsActive
                                              orderby x.CreateTime descending
                                              select x).ToList();
                this.AlertsBox.SelectedItem = this.AlertsBox.Items.OfType<DataLayer.Entities.Message>().FirstOrDefault<DataLayer.Entities.Message>();
            }
            catch (Exception ex)
            {
                Log.Error("There was an error retrieving the alerts.", ex);
            }
        }

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

        private void AccountWelcome_Checked(object sender, RoutedEventArgs e)
        {
            Uri uri = new Uri("/Welcome.xaml", UriKind.Relative);
            Navigation.ShowTab(uri, true);
        }

        private void AccountWelcome_Unchecked(object sender, RoutedEventArgs e)
        {
            var tab = this.Tabs.Items.OfType<TabItem>().FirstOrDefault(x => (x.Content as Frame).Source.OriginalString.Trim('/').Contains("Welcome.xaml"));
            Navigation.CloseTab(tab);
        }

    }
}
