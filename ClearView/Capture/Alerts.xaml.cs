using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using EPIC.Macros;
using EPICClearViewDL.EntityClasses;
using EPICClearViewDL.Linq;

namespace EPIC.Capture
{
	// Token: 0x02000002 RID: 2
	public partial class Alerts : Page
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002258 File Offset: 0x00000458
		public Alerts()
		{
			this.InitializeComponent();
			base.Loaded += delegate(object sender, RoutedEventArgs args)
			{
				if (base.NavigationService != null)
				{
					int num = base.NavigationService.CurrentSource.OriginalString.IndexOf("?", StringComparison.InvariantCultureIgnoreCase);
					int messageId;
					if (num >= 0 && int.TryParse(HttpUtility.ParseQueryString(base.NavigationService.CurrentSource.OriginalString.Substring(num))["messageId"], out messageId))
					{
						this.Messages.ItemsSource = from x in new LinqMetaData().Message
						where x.MessageId == (long)messageId
						select x;
					}
					else
					{
						this.Messages.ItemsSource = from x in new LinqMetaData().Message
						where x.IsActive
						orderby x.CreateTime descending
						select x;
					}
					this.Total = new LinqMetaData().Message.Count((MessageEntity x) => x.IsActive);
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
			new LinqMetaData().Message.ToList<MessageEntity>().ForEach(delegate(MessageEntity x)
			{
				x.IsActive = false;
				x.Save();
			});
			MainWindow mainWindow = Application.Current.MainWindow as MainWindow;
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
