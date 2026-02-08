using System;
using System.Drawing;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using WindowsNative;

namespace EPIC.Utilities.Extensions
{
	// Token: 0x02000059 RID: 89
	public static class IconExtensions
	{
		// Token: 0x060002D9 RID: 729 RVA: 0x000177B0 File Offset: 0x000159B0
		public static ImageSource ToImageSource(this Icon icon)
		{
			Bitmap bitmap = icon.ToBitmap();
			IntPtr hbitmap = bitmap.GetHbitmap();
			ImageSource result = Imaging.CreateBitmapSourceFromHBitmap(hbitmap, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
			Gdi32.DeleteObject(hbitmap);
			return result;
		}
	}
}
