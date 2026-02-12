
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Principal;
using System.Windows;

namespace EPIC.ClearView.Utilities
{
    public static class LocalAuthentication
    {


        public static void LogPatientData(string networkPath, string domain, string username, string password, byte[] data)
        {
            IntPtr token = IntPtr.Zero;
            try
            {
                // 1. Authenticate the specific contractor/doctor
                bool success = LogonUser(username, domain, password, 2, 0, ref token);
                if (!success) throw new Win32Exception(Marshal.GetLastWin32Error());

                using (WindowsIdentity identity = new WindowsIdentity(token))
                {
                    // 2. Wrap the IO logic in an impersonation block
                    WindowsIdentity.RunImpersonated(identity.AccessToken, () =>
                    {
                        // This code runs as the specific user, not the app process
                        File.WriteAllBytes(Path.Combine(networkPath, "patient_log.dat"), data);
                    });
                }
            }
            finally
            {
                if (token != IntPtr.Zero) CloseHandle(token);
            }
        }

        [DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        static extern bool LogonUser(string lpszUsername, string lpszDomain, string lpszPassword, int dwLogonType, int dwLogonProvider, ref IntPtr phToken);
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool CloseHandle(IntPtr hObject);

        public static void RelaunchAsDoctor(string username, string domain, string password)
        {
            var securePassword = new SecureString();
            foreach (char c in password) securePassword.AppendChar(c);

            ProcessStartInfo info = new ProcessStartInfo
            {
                FileName = Process.GetCurrentProcess().MainModule.FileName,
                UserName = username,
                Domain = domain,
                Password = securePassword,
                UseShellExecute = false, // Required for credentials
                LoadUserProfile = true   // Ensures they get their specific network mappings
            };

            Process.Start(info);
            Application.Current.Shutdown();
        }

    }
}
