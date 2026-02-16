using System.Runtime.InteropServices;

namespace EPIC.ClearView.Native
{
    public partial class DwmApi
    {
        [LibraryImport("dwmapi.dll")]
        public static partial int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int attrValue, int attrSize);
        public const int DWMWA_CLOAK = 13;
        public const int DWM_CLOAKED_APP = 0x0000001;  // Cloaked by the app itself
        public const int DWM_UNCLOAK = 0x0000000;      // Visible
    }
}
