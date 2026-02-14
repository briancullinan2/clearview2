using System.Runtime.InteropServices;

namespace EPIC.ClearView.Native
{
    public partial class Gdi32
    {
        [LibraryImport("gdi32")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool DeleteObject(IntPtr o);
        [LibraryImport("gdi32.dll")] public static partial IntPtr CreateCompatibleDC(IntPtr hdc);
        [LibraryImport("gdi32.dll")] public static partial IntPtr CreateCompatibleBitmap(IntPtr hdc, int nWidth, int nHeight);
        [LibraryImport("gdi32.dll")] public static partial IntPtr SelectObject(IntPtr hdc, IntPtr hgdiobj);
        [return: MarshalAs(UnmanagedType.Bool)]
        [LibraryImport("gdi32.dll")] public static partial bool DeleteDC(IntPtr hdc);
        [return: MarshalAs(UnmanagedType.Bool)]
        [LibraryImport("gdi32.dll")] public static partial bool BitBlt(IntPtr hdcDest, int nXDest, int nYDest, int nWidth, int nHeight, IntPtr hdcSrc, int nXSrc, int nYSrc, int dwRop);
    }
}
