using System;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Interop;
using System.Windows.Media;

namespace EPIC.Utilities.Extensions
{
	// Token: 0x0200005A RID: 90
	public static class InteropExtensions
	{
		// Token: 0x060002DA RID: 730 RVA: 0x000177F0 File Offset: 0x000159F0
		public static System.Windows.Forms.IWin32Window GetIWin32Window(this Visual visual)
		{
			HwndSource hwndSource = PresentationSource.FromVisual(visual) as HwndSource;
			System.Windows.Forms.IWin32Window result;
			if (hwndSource != null)
			{
				System.Windows.Forms.IWin32Window win32Window = new OldWindow(hwndSource.Handle);
				result = win32Window;
			}
			else
			{
				result = null;
			}
			return result;
		}
	}
}
