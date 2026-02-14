using EPIC.ClearView.Native;
using System.Windows.Media.Imaging;

namespace EPIC.ClearView.Macros
{
    public static class Mica
    {
        public static BitmapSource CaptureRegion(int x, int y, int width, int height)
        {
            IntPtr hDesk = User32.GetDesktopWindow();
            IntPtr hSrce = User32.GetWindowDC(hDesk);
            IntPtr hDest = Gdi32.CreateCompatibleDC(hSrce);
            IntPtr hBmp = Gdi32.CreateCompatibleBitmap(hSrce, width, height);
            IntPtr hOld = Gdi32.SelectObject(hDest, hBmp);

            // 0x00CC0020 is SRCCOPY
            Gdi32.BitBlt(hDest, 0, 0, width, height, hSrce, x, y, 0x00CC0020);

            // Convert HBITMAP to WPF BitmapSource
            BitmapSource bps = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                hBmp, IntPtr.Zero, System.Windows.Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());

            // Cleanup unmanaged junk
            Gdi32.SelectObject(hDest, hOld);
            Gdi32.DeleteObject(hBmp);
            Gdi32.DeleteDC(hDest);
            User32.ReleaseDC(hDesk, hSrce);

            return bps;
        }
    }
}
