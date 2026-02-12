using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Markup;
using System.Windows.Navigation;
using EPICClearViewDL.EntityClasses;
using EPICClearViewDL.Linq;

namespace EPIC.Report
{
	// Token: 0x02000040 RID: 64
	public partial class Results : Page
	{
		// Token: 0x17000076 RID: 118
		// (get) Token: 0x06000210 RID: 528 RVA: 0x00011D18 File Offset: 0x0000FF18
		// (set) Token: 0x06000211 RID: 529 RVA: 0x00011D3A File Offset: 0x0000FF3A
		public FixedDocument Document
		{
			get
			{
				return (FixedDocument)base.GetValue(Results.DocumentProperty);
			}
			set
			{
				base.SetValue(Results.DocumentProperty, value);
				this.Viewer.FitToWidth();
			}
		}

		// Token: 0x17000077 RID: 119
		// (get) Token: 0x06000212 RID: 530 RVA: 0x00011D58 File Offset: 0x0000FF58
		// (set) Token: 0x06000213 RID: 531 RVA: 0x00011D7C File Offset: 0x0000FF7C
		public FingerSetEntity Fingers
		{
			get
			{
				return (FingerSetEntity)base.GetValue(Results.FingersProperty);
			}
			set
			{
				base.SetValue(Results.FingersProperty, value);
				this.Patient = value.Patient;
				FixedDocument fixedDocument = new FixedDocument();
				FixedPage[] array = new FixedPage[]
				{
					new SystemSummary(value, this.Viewer).Page,
					new ReportNotes(value, this.Viewer).Page,
					new SystemDetails(value, this.Viewer).Page
				};
				foreach (FixedPage fixedPage in array)
				{
					((ContentControl)fixedPage.Parent).Content = null;
					fixedDocument.Pages.Add(new PageContent
					{
						Child = fixedPage
					});
				}
				this.Document = fixedDocument;
			}
		}

		// Token: 0x17000078 RID: 120
		// (get) Token: 0x06000214 RID: 532 RVA: 0x00011E50 File Offset: 0x00010050
		// (set) Token: 0x06000215 RID: 533 RVA: 0x00011E72 File Offset: 0x00010072
		public PatientEntity Patient
		{
			get
			{
				return (PatientEntity)base.GetValue(Results.PatientProperty);
			}
			set
			{
				base.SetValue(Results.PatientProperty, value);
			}
		}

		// Token: 0x06000216 RID: 534 RVA: 0x00011E82 File Offset: 0x00010082
		public Results()
		{
			this.InitializeComponent();
		}

		// Token: 0x06000217 RID: 535 RVA: 0x00011E9C File Offset: 0x0001009C
		private void Results_Loaded(object sender, RoutedEventArgs e)
		{
			if (base.NavigationService != null)
			{
				int num = base.NavigationService.CurrentSource.OriginalString.IndexOf("?", StringComparison.InvariantCultureIgnoreCase);
				int fingerSetId;
				if (num >= 0 && int.TryParse(HttpUtility.ParseQueryString(base.NavigationService.CurrentSource.OriginalString.Substring(num))["fingerSetId"], out fingerSetId))
				{
					this.Fingers = new LinqMetaData().FingerSet.FirstOrDefault((FingerSetEntity x) => x.FingerSetId == (long)fingerSetId);
				}
			}
		}

		// Token: 0x06000218 RID: 536 RVA: 0x00011F9C File Offset: 0x0001019C
		private void Document_Navigate(object sender, RequestNavigateEventArgs e)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000219 RID: 537 RVA: 0x00011FA4 File Offset: 0x000101A4
		private void Results_SizeChanged(object sender, SizeChangedEventArgs e)
		{
			this.Viewer.FitToWidth();
		}

		// Token: 0x04000116 RID: 278
		public static readonly DependencyProperty DocumentProperty = DependencyProperty.Register("Document", typeof(FixedDocument), typeof(Results), new PropertyMetadata(null));

		// Token: 0x04000117 RID: 279
		public static readonly DependencyProperty FingersProperty = DependencyProperty.Register("Fingers", typeof(FingerSetEntity), typeof(Results), new PropertyMetadata(null));

		// Token: 0x04000118 RID: 280
		public static readonly DependencyProperty PatientProperty = DependencyProperty.Register("Patient", typeof(PatientEntity), typeof(Results), new PropertyMetadata(null));
	}
}
