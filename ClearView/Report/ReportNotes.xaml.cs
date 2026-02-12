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
	// Token: 0x0200003F RID: 63
	public partial class ReportNotes : ContentControl
	{
		// Token: 0x17000073 RID: 115
		// (get) Token: 0x06000206 RID: 518 RVA: 0x00011B50 File Offset: 0x0000FD50
		// (set) Token: 0x06000207 RID: 519 RVA: 0x00011B72 File Offset: 0x0000FD72
		public FingerSetEntity Fingers
		{
			get
			{
				return (FingerSetEntity)base.GetValue(ReportNotes.FingersProperty);
			}
			set
			{
				base.SetValue(ReportNotes.FingersProperty, value);
				this.Patient = value.Patient;
			}
		}

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x06000208 RID: 520 RVA: 0x00011B90 File Offset: 0x0000FD90
		// (set) Token: 0x06000209 RID: 521 RVA: 0x00011BB2 File Offset: 0x0000FDB2
		public PatientEntity Patient
		{
			get
			{
				return (PatientEntity)base.GetValue(ReportNotes.PatientProperty);
			}
			set
			{
				base.SetValue(ReportNotes.PatientProperty, value);
			}
		}

		// Token: 0x17000075 RID: 117
		// (get) Token: 0x0600020A RID: 522 RVA: 0x00011BC4 File Offset: 0x0000FDC4
		// (set) Token: 0x0600020B RID: 523 RVA: 0x00011BE6 File Offset: 0x0000FDE6
		public DocumentViewer Document
		{
			get
			{
				return (DocumentViewer)base.GetValue(ReportNotes.DocumentProperty);
			}
			set
			{
				base.SetValue(ReportNotes.DocumentProperty, value);
			}
		}

		// Token: 0x0600020C RID: 524 RVA: 0x00011BF6 File Offset: 0x0000FDF6
		public ReportNotes(FingerSetEntity set, DocumentViewer doc)
		{
			this.InitializeComponent();
			this.Page.DataContext = this;
			this.Fingers = set;
			this.Document = doc;
		}

		// Token: 0x04000111 RID: 273
		public static readonly DependencyProperty FingersProperty = DependencyProperty.Register("Fingers", typeof(FingerSetEntity), typeof(ReportNotes), new PropertyMetadata(null));

		// Token: 0x04000112 RID: 274
		public static readonly DependencyProperty PatientProperty = DependencyProperty.Register("Patient", typeof(PatientEntity), typeof(ReportNotes), new PropertyMetadata(null));

		// Token: 0x04000113 RID: 275
		public static readonly DependencyProperty DocumentProperty = DependencyProperty.Register("Document", typeof(DocumentViewer), typeof(ReportNotes), new PropertyMetadata(null));
	}
}
