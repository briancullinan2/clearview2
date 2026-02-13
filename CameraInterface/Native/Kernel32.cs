using System.Runtime.InteropServices;

namespace EPIC.CameraInterface.Native
{
    public partial class Kernel32
    {


        // Token: 0x06000064 RID: 100
        [LibraryImport("kernel32.dll", StringMarshalling = StringMarshalling.Utf16)]
        public static partial int FormatMessage(int dwFlags, IntPtr lpSource, int dwMessageId, int dwLanguageId, out string lpBuffer, int nSize, IntPtr arguments);

        // Token: 0x06000065 RID: 101
        [LibraryImport("kernel32.dll", StringMarshalling = StringMarshalling.Utf16)]
        public static partial IntPtr LoadLibraryEx(string lpszLibFile, IntPtr hFile, int dwFlags);

        // Token: 0x06000066 RID: 102
        [LibraryImport("kernel32.dll")]
        public static partial int FreeLibrary(IntPtr hModule);

    }
}
