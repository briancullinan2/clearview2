using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using EPICClearView.Macros;

namespace EPICClearView.Capture
{
	// Token: 0x0200006D RID: 109
	public partial class Manage : Page
	{
		// Token: 0x06000342 RID: 834 RVA: 0x0001AEBB File Offset: 0x000190BB
		public Manage()
		{
			InitializeComponent();
			Navigation.InsertRibbon(this);
		}

		// Token: 0x06000343 RID: 835 RVA: 0x0001AED4 File Offset: 0x000190D4
		private void Locations_Click(object sender, RoutedEventArgs e)
		{
			this.Navigator.Source = new Uri("https://google.com", UriKind.Absolute);
		}
	}
}
