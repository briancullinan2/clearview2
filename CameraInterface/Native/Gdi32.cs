using System.Runtime.InteropServices;

namespace EPIC.CameraInterface.Native
{
    public partial class Gdi32
    {

        // Token: 0x06000063 RID: 99
        [LibraryImport("gdi32")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool DeleteObject(IntPtr o);

    }
}
