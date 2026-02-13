using System.Runtime.InteropServices;

namespace EPIC.ClearView.Native
{
    public partial class Gdi32
    {
        [LibraryImport("gdi32")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool DeleteObject(IntPtr o);
    }
}
