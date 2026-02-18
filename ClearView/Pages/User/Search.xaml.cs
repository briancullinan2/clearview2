using EPIC.ClearView.Utilities.Macros;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace EPIC.ClearView.Pages.User
{
    // Token: 0x0200006A RID: 106
    public partial class Search : Page
    {
        public ImageBrush RibbonBackground { get; private set; }

        // Token: 0x06000335 RID: 821 RVA: 0x0001AB30 File Offset: 0x00018D30
        public Search()
        {
            this.InitializeComponent();
            Navigation.InsertRibbon(this);
        }

        // Token: 0x06000336 RID: 822 RVA: 0x0001AB64 File Offset: 0x00018D64
        private void NewUser_Click(object sender, RoutedEventArgs e)
        {
            Navigation.ShowTab(new Uri("/User/Add.xaml", UriKind.Relative));
        }

        // Token: 0x06000337 RID: 823 RVA: 0x0001AB98 File Offset: 0x00018D98
        private void Search_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void RibbonToggleButton_Unchecked(object sender, RoutedEventArgs e)
        {
            Navigation.CloseTab(this);
        }

        private void UserPermission_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
