using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using EPIC.Macros;
using EPICClearViewDL.EntityClasses;
using EPICClearViewDL.Linq;

namespace EPIC.Capture
{
	// Token: 0x02000068 RID: 104
	public partial class Search : Page
	{
		// Token: 0x06000324 RID: 804 RVA: 0x0001A27D File Offset: 0x0001847D
		public Search()
		{
			this.InitializeComponent();
			Navigation.InsertRibbon(this);
		}

		// Token: 0x06000325 RID: 805 RVA: 0x0001A296 File Offset: 0x00018496
		private void Search_TextChanged(object sender, TextChangedEventArgs e)
		{
		}

		// Token: 0x06000326 RID: 806 RVA: 0x0001A29C File Offset: 0x0001849C
		private void ViewReport_Click(object sender, RoutedEventArgs e)
		{
			Uri uri = new Uri(string.Format("/Report/Results.xaml?fingerSetId={0}", (from x in new LinqMetaData().FingerSet
			orderby x.FingerSetId descending
			select x).First<FingerSetEntity>().FingerSetId), UriKind.Relative);
			Navigation.ShowTab(uri, true);
		}
	}
}
