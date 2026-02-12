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
	// Token: 0x02000042 RID: 66
	public partial class SystemDetails : ContentControl
	{
		// Token: 0x1700007A RID: 122
		// (get) Token: 0x06000223 RID: 547 RVA: 0x000121B8 File Offset: 0x000103B8
		// (set) Token: 0x06000224 RID: 548 RVA: 0x000121DA File Offset: 0x000103DA
		public FingerSetEntity Fingers
		{
			get
			{
				return (FingerSetEntity)base.GetValue(SystemDetails.FingersProperty);
			}
			set
			{
				base.SetValue(SystemDetails.FingersProperty, value);
				this.Patient = value.Patient;
			}
		}

		// Token: 0x1700007B RID: 123
		// (get) Token: 0x06000225 RID: 549 RVA: 0x000121F8 File Offset: 0x000103F8
		// (set) Token: 0x06000226 RID: 550 RVA: 0x0001221A File Offset: 0x0001041A
		public PatientEntity Patient
		{
			get
			{
				return (PatientEntity)base.GetValue(SystemDetails.PatientProperty);
			}
			set
			{
				base.SetValue(SystemDetails.PatientProperty, value);
			}
		}

		// Token: 0x1700007C RID: 124
		// (get) Token: 0x06000227 RID: 551 RVA: 0x0001222C File Offset: 0x0001042C
		// (set) Token: 0x06000228 RID: 552 RVA: 0x0001224E File Offset: 0x0001044E
		public DocumentViewer Document
		{
			get
			{
				return (DocumentViewer)base.GetValue(SystemDetails.DocumentProperty);
			}
			set
			{
				base.SetValue(SystemDetails.DocumentProperty, value);
			}
		}

		// Token: 0x06000229 RID: 553 RVA: 0x0001225E File Offset: 0x0001045E
		public SystemDetails(FingerSetEntity set, DocumentViewer doc)
		{
			this.InitializeComponent();
			this.Page.DataContext = this;
			this.Fingers = set;
			this.Document = doc;
		}

		// Token: 0x0400011D RID: 285
		public static readonly DependencyProperty FingersProperty = DependencyProperty.Register("Fingers", typeof(FingerSetEntity), typeof(SystemDetails), new PropertyMetadata(null));

		// Token: 0x0400011E RID: 286
		public static readonly DependencyProperty PatientProperty = DependencyProperty.Register("Patient", typeof(PatientEntity), typeof(SystemDetails), new PropertyMetadata(null));

		// Token: 0x0400011F RID: 287
		public static readonly DependencyProperty DocumentProperty = DependencyProperty.Register("Document", typeof(DocumentViewer), typeof(SystemDetails), new PropertyMetadata(null));
	}
}
