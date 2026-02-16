using System.Runtime.InteropServices;

namespace EPIC.ClearView.Native
{
    public partial class User32
    {
        public const uint WDA_EXCLUDEFROMCAPTURE = 0x00000011;
        public const uint WDA_NONE = 0x00000000;
        [LibraryImport("user32.dll")] public static partial IntPtr GetDesktopWindow();
        [LibraryImport("user32.dll")] public static partial IntPtr GetWindowDC(IntPtr hwnd);
        [LibraryImport("user32.dll")] public static partial int ReleaseDC(IntPtr hwnd, IntPtr hdc);
        [LibraryImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool SetWindowDisplayAffinity(IntPtr hwnd, uint affinity);
    }
}
