using System.Runtime.InteropServices;

namespace EPIC.ClearView.Native
{
    public partial class Kernel32
    {
        [LibraryImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool CloseHandle(IntPtr hObject);

    }
}
