using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media.Imaging;
using EPIC.Utilities;

namespace EPIC.Controls
{
	// Token: 0x02000008 RID: 8
	public abstract partial class ProcessingImage : UserControl
	{
		// Token: 0x17000024 RID: 36
		// (get) Token: 0x06000089 RID: 137 RVA: 0x000063DC File Offset: 0x000045DC
		// (set) Token: 0x0600008A RID: 138 RVA: 0x000063FE File Offset: 0x000045FE
		public States State
		{
			get
			{
				return (States)base.GetValue(ProcessingImage.StateProperty);
			}
			set
			{
				base.SetValue(ProcessingImage.StateProperty, value);
			}
		}

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x0600008B RID: 139 RVA: 0x00006414 File Offset: 0x00004614
		// (set) Token: 0x0600008C RID: 140 RVA: 0x00006436 File Offset: 0x00004636
		public BitmapSource Image
		{
			get
			{
				return (BitmapSource)base.GetValue(ProcessingImage.ImageProperty);
			}
			set
			{
				base.SetValue(ProcessingImage.ImageProperty, value);
			}
		}

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x0600008D RID: 141 RVA: 0x00006448 File Offset: 0x00004648
		// (set) Token: 0x0600008E RID: 142 RVA: 0x0000646A File Offset: 0x0000466A
		public double Value
		{
			get
			{
				return (double)base.GetValue(ProcessingImage.ValueProperty);
			}
			set
			{
				base.SetValue(ProcessingImage.ValueProperty, value);
			}
		}

		// Token: 0x0600008F RID: 143 RVA: 0x0000647F File Offset: 0x0000467F
		public ProcessingImage()
		{
			this.InitializeComponent();
		}

		// Token: 0x06000090 RID: 144
		public abstract void StoreAndProcess(CaptureResults results);

		// Token: 0x04000061 RID: 97
		public static readonly DependencyProperty StateProperty = DependencyProperty.Register("State", typeof(States), typeof(ProcessingImage), new PropertyMetadata(States.Queued));

		// Token: 0x04000062 RID: 98
		public static readonly DependencyProperty ImageProperty = DependencyProperty.Register("Image", typeof(BitmapSource), typeof(ProcessingImage), new PropertyMetadata(StaticEnvironment.IsInDesignMode ? new BitmapImage(new Uri("/Resources/Calibration_Sentech.bmp", UriKind.Relative)) : null));

		// Token: 0x04000063 RID: 99
		public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(double), typeof(ProcessingImage), new PropertyMetadata(0.0));

		// Token: 0x04000064 RID: 100
		public static readonly DependencyProperty StatusProperty = DependencyProperty.Register("Status", typeof(string), typeof(ProcessingImage), new PropertyMetadata(null));
	}
}
