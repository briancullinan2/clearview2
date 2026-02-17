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
            //this.Users.ItemsSource = new LinqMetaData().User.ToList<DataLayer.Entities.User>();
            // Check if we are currently inside the Visual Studio Designer
            /*
            if (System.ComponentModel.DesignerProperties.GetIsInDesignMode(this))
            {
                // The designer hasn't fully 'arranged' the layout yet in the ctor,
                // so we use a Dispatcher call to run it after the first render pass.
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    // Arizona System Note: 'path' should be absolute for the designer
                    string projectDir = AppDomain.CurrentDomain.BaseDirectory;
                    var renderedSource = Utilities.Permissions.RenderBitmap(FindResource("SearchOptions") as FrameworkElement);
                    this.RibbonBackground = new ImageBrush(renderedSource)
                    {
                        Stretch = Stretch.UniformToFill,
                        AlignmentY = AlignmentY.Top
                    };
                }), System.Windows.Threading.DispatcherPriority.ContextIdle);
            }
            */
        }

        // Token: 0x06000336 RID: 822 RVA: 0x0001AB64 File Offset: 0x00018D64
        private void NewUser_Click(object sender, RoutedEventArgs e)
        {
            Navigation.ShowTab(new Uri("/User/Add.xaml", UriKind.Relative));
            //if (base.NavigationService != null)
            //{
            //    base.NavigationService.Source = new Uri("/User/Add.xaml", UriKind.Relative);
            //}
        }

        // Token: 0x06000337 RID: 823 RVA: 0x0001AB98 File Offset: 0x00018D98
        private void Search_TextChanged(object sender, TextChangedEventArgs e)
        {
            /*
			if (!string.IsNullOrEmpty(this.SearchText.Text))
			{
				this.Users.ItemsSource = new LinqMetaData().User.Search(new List<Expression<Func<DataLayer.Entities.User, object>>>
				{
					(DataLayer.Entities.User x) => x.FirstName,
					(DataLayer.Entities.User x) => x.LastName,
					(DataLayer.Entities.User x) => x.Username,
					(DataLayer.Entities.User x) => (object)x.IsActive
				}, this.SearchText.Text, false);
			}
			else
			{
				this.Users.ItemsSource = new LinqMetaData().User.ToList<DataLayer.Entities.User>();
			}
			*/
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
