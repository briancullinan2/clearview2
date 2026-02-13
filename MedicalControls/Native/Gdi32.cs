using System.Runtime.InteropServices;

namespace EPIC.MedicalControls.Native
{
    internal partial class Gdi32
    {
        // Token: 0x06000063 RID: 99
        [LibraryImport("gdi32")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static partial bool DeleteObject(IntPtr o);
    }

}

