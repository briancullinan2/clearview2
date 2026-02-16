using System.Windows;
using System.Windows.Controls.Ribbon;

namespace EPIC.MedicalControls.Utilities
{
    public partial class ServiceRibbon : Ribbon
    {
        private void ApplicationChangeLogin_Click(object sender, System.Windows.RoutedEventArgs e)
        {

        }


        private void AccountWelcome_Checked(object sender, RoutedEventArgs e)
        {
            Uri uri = new Uri("/Welcome.xaml", UriKind.Relative);
            //Navigation.ShowTab(uri, true);
        }

        private void AccountWelcome_Unchecked(object sender, RoutedEventArgs e)
        {
            //var tab = this.Tabs.Items.OfType<TabItem>().FirstOrDefault(x => (x.Content as Frame).Source.OriginalString.Trim('/').Contains("Welcome.xaml"));
            //Navigation.CloseTab(tab);
        }

        // Token: 0x06000356 RID: 854 RVA: 0x0001B920 File Offset: 0x00019B20
        private void AccountManage_Click(object sender, RoutedEventArgs e)
        {
        }

        // Token: 0x06000357 RID: 855 RVA: 0x0001B944 File Offset: 0x00019B44
        private void Patients_Click(object sender, RoutedEventArgs e)
        {
        }

        // Token: 0x06000358 RID: 856 RVA: 0x0001B968 File Offset: 0x00019B68
        private void Captures_Click(object sender, RoutedEventArgs e)
        {
        }

        // Token: 0x06000359 RID: 857 RVA: 0x0001B98C File Offset: 0x00019B8C
        private void UserAdd_Click(object sender, RoutedEventArgs e)
        {
        }

        // Token: 0x0600035A RID: 858 RVA: 0x0001B9B0 File Offset: 0x00019BB0
        private void UserSearch_Click(object sender, RoutedEventArgs e)
        {
        }

        // Token: 0x0600035B RID: 859 RVA: 0x0001B9D4 File Offset: 0x00019BD4
        private void ApplicationSettings_Click(object sender, RoutedEventArgs e)
        {
        }

        // Token: 0x0600035C RID: 860 RVA: 0x0001B9F8 File Offset: 0x00019BF8
        private void UserPermission_Click(object sender, RoutedEventArgs e)
        {
        }

        // Token: 0x0600035D RID: 861 RVA: 0x0001BA1C File Offset: 0x00019C1C
        private void Calibrate_Click(object sender, RoutedEventArgs e)
        {
        }

        // Token: 0x0600035E RID: 862 RVA: 0x0001BA40 File Offset: 0x00019C40
        private void NewCapture_Click(object sender, RoutedEventArgs e)
        {
        }

        // Token: 0x0600035F RID: 863 RVA: 0x0001BAB8 File Offset: 0x00019CB8
        private void Alerts_ViewAll(object sender, RoutedEventArgs e)
        {
        }

        private void Alerts_View(object sender, RoutedEventArgs e)
        {
        }

    }
}
