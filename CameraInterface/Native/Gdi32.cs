using System.Runtime.InteropServices;

namespace EPIC.CameraInterface.Native
{
    public partial class Gdi32
    {

        // Token: 0x06000063 RID: 99
        [LibraryImport("gdi32")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool DeleteObject(IntPtr o);

        [LibraryImport("gdi32.dll")]
        public static partial IntPtr CreateDIBSection(IntPtr hdc, in BITMAPINFO pbmi, uint usage, out IntPtr ppvBits, IntPtr hSection, uint dwOffset);

        [StructLayout(LayoutKind.Sequential)]
        public struct BITMAPINFO
        {
            public BITMAPINFOHEADER bmiHeader;
            public int bmiColors; // technically an array, but 0 for RGB
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct BITMAPINFOHEADER
        {
            public int biSize;
            public int biWidth;
            public int biHeight;
            public short biPlanes;
            public short biBitCount;
            public int biCompression;
            public int biSizeImage;
            public int biXPelsPerMeter;
            public int biYPelsPerMeter;
            public int biClrUsed;
            public int biClrImportant;
        }
    }
}
