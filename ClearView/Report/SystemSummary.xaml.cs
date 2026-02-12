using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Markup;
using EPICClearViewDL.EntityClasses;

namespace EPIC.Report
{
	// Token: 0x02000043 RID: 67
	public partial class SystemSummary : ContentControl
	{
		// Token: 0x1700007D RID: 125
		// (get) Token: 0x0600022D RID: 557 RVA: 0x00012380 File Offset: 0x00010580
		// (set) Token: 0x0600022E RID: 558 RVA: 0x000123A2 File Offset: 0x000105A2
		public FingerSetEntity Fingers
		{
			get
			{
				return (FingerSetEntity)base.GetValue(SystemSummary.FingersProperty);
			}
			set
			{
				base.SetValue(SystemSummary.FingersProperty, value);
				this.Patient = value.Patient;
			}
		}

		// Token: 0x1700007E RID: 126
		// (get) Token: 0x0600022F RID: 559 RVA: 0x000123C0 File Offset: 0x000105C0
		// (set) Token: 0x06000230 RID: 560 RVA: 0x000123E2 File Offset: 0x000105E2
		public PatientEntity Patient
		{
			get
			{
				return (PatientEntity)base.GetValue(SystemSummary.PatientProperty);
			}
			set
			{
				base.SetValue(SystemSummary.PatientProperty, value);
			}
		}

		// Token: 0x1700007F RID: 127
		// (get) Token: 0x06000231 RID: 561 RVA: 0x000123F4 File Offset: 0x000105F4
		// (set) Token: 0x06000232 RID: 562 RVA: 0x00012416 File Offset: 0x00010616
		public DocumentViewer Document
		{
			get
			{
				return (DocumentViewer)base.GetValue(SystemSummary.DocumentProperty);
			}
			set
			{
				base.SetValue(SystemSummary.DocumentProperty, value);
			}
		}

		// Token: 0x06000233 RID: 563 RVA: 0x00012426 File Offset: 0x00010626
		public SystemSummary(FingerSetEntity set, DocumentViewer doc)
		{
			this.InitializeComponent();
			this.Page.DataContext = this;
			this.Fingers = set;
			this.Document = doc;
		}

		// Token: 0x04000122 RID: 290
		public static readonly DependencyProperty FingersProperty = DependencyProperty.Register("Fingers", typeof(FingerSetEntity), typeof(SystemSummary), new PropertyMetadata(null));

		// Token: 0x04000123 RID: 291
		public static readonly DependencyProperty PatientProperty = DependencyProperty.Register("Patient", typeof(PatientEntity), typeof(SystemSummary), new PropertyMetadata(null));

		// Token: 0x04000124 RID: 292
		public static readonly DependencyProperty DocumentProperty = DependencyProperty.Register("Document", typeof(DocumentViewer), typeof(SystemSummary), new PropertyMetadata(null));
	}
}
