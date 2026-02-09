using System;
using System.Runtime.InteropServices;

namespace WindowsNative
{
    public static class Gdi32
    {
        [DllImport("gdi32.dll")]
        public static extern int DeleteObject(IntPtr o);
    }
}
