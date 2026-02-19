using EPIC.ClearView.Utilities.Macros;
using System.Web;
using System.Windows;
using System.Windows.Controls;

namespace EPIC.ClearView.Pages.Application
{
    // Token: 0x02000002 RID: 2
    public partial class Alerts : Page
    {
        // Token: 0x06000001 RID: 1 RVA: 0x00002258 File Offset: 0x00000458
        public Alerts()
        {
            this.InitializeComponent();
            Navigation.InsertRibbon(this);
            base.Loaded += delegate (object sender, RoutedEventArgs args)
            {
                var messages = DataLayer.TranslationContext.Current["Data Source=:memory:"].Messages;

                if (base.NavigationService != null)
                {
                    int? num = base.NavigationService.CurrentSource?.OriginalString.IndexOf("?", StringComparison.InvariantCultureIgnoreCase);
                    int messageId;
                    if (NavigationService.CurrentSource != null
                        && num != null && num >= 0
                        && int.TryParse(HttpUtility.ParseQueryString(base.NavigationService.CurrentSource.OriginalString.Substring((int)num))["messageId"], out messageId))
                    {
                        this.Messages.ItemsSource = (from x in messages
                                                     where x.MessageId == (long)messageId
                                                     select x).ToList();
                    }
                    else
                    {
                        this.Messages.ItemsSource = (from x in messages
                                                     where x.IsActive
                                                     orderby x.CreateTime descending
                                                     select x).ToList();
                    }
                    this.Total = messages.Count((DataLayer.Entities.Message x) => x.IsActive);
                }
            };
        }

        // Token: 0x17000001 RID: 1
        // (get) Token: 0x06000002 RID: 2 RVA: 0x00002294 File Offset: 0x00000494
        // (set) Token: 0x06000003 RID: 3 RVA: 0x000022B6 File Offset: 0x000004B6
        protected int Total
        {
            get
            {
                return (int)base.GetValue(Alerts.TotalProperty);
            }
            set
            {
                base.SetValue(Alerts.TotalProperty, value);
            }
        }

        // Token: 0x06000004 RID: 4 RVA: 0x000022E0 File Offset: 0x000004E0
        private void Dismiss_Click(object sender, RoutedEventArgs e)
        {
            var messages = DataLayer.TranslationContext.Current["Data Source=:memory:"].Messages;
            //new messages.ToList<DataLayer.Entities.Message>().ForEach(delegate (DataLayer.Entities.Message x)
            //{
            //    x.IsActive = false;
            //    x.Save();
            //});
            MainWindow mainWindow = System.Windows.Application.Current.MainWindow as MainWindow;
            if (mainWindow != null)
            {
                mainWindow.UpdateAlerts();
            }
            Navigation.CloseTabCommand.Execute(this);
        }

        // Token: 0x04000001 RID: 1
        protected static readonly DependencyProperty TotalProperty = DependencyProperty.Register("Total", typeof(int), typeof(Alerts), new PropertyMetadata(0));
    }
}
