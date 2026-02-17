using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media.Imaging;

namespace EPIC.Report.Resources
{
	// Token: 0x02000041 RID: 65
	public partial class SystemButton : Button
	{
		// Token: 0x0600021D RID: 541 RVA: 0x00012100 File Offset: 0x00010300
		public SystemButton()
		{
			this.InitializeComponent();
		}

		// Token: 0x17000079 RID: 121
		// (get) Token: 0x0600021E RID: 542 RVA: 0x00012114 File Offset: 0x00010314
		// (set) Token: 0x0600021F RID: 543 RVA: 0x00012136 File Offset: 0x00010336
		public BitmapSource Image
		{
			get
			{
				return (BitmapSource)base.GetValue(SystemButton.ImageProperty);
			}
			set
			{
				base.SetValue(SystemButton.ImageProperty, value);
			}
		}

		// Token: 0x0400011B RID: 283
		public static readonly DependencyProperty ImageProperty = DependencyProperty.Register("Image", typeof(BitmapSource), typeof(SystemButton), new PropertyMetadata(null));
	}
}
