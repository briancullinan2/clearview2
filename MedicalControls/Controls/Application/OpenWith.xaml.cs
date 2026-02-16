using EPIC.MedicalControls.Utilities.Extensions;
using Microsoft.Win32;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;

namespace EPIC.MedicalControls.Controls
{
    // Token: 0x02000011 RID: 17
    public partial class OpenWith : Window, IStyleConnector
    {
        // Token: 0x06000110 RID: 272 RVA: 0x00009920 File Offset: 0x00007B20
        public OpenWith(string file)
        {
            this._file = file;
            this.InitializeComponent();
            IEnumerable<string> recommended = this.RecommendedPrograms(Path.GetExtension(this._file)).Select(delegate (string x)
            {
                string result;
                if ((result = this.GetRegisteredApplication(x)) == null)
                {
                    result = (OpenWith.GetFullPath(x) ?? OpenWith.FindAppPath(x));
                }
                return result;
            });
            IEnumerable<OpenWith.ProgramWithIcon> itemsSource = this.ConvertPrograms(recommended);
            this.Programs.ItemsSource = itemsSource;
        }

        // Token: 0x06000111 RID: 273 RVA: 0x00009EAC File Offset: 0x000080AC
        private IEnumerable<OpenWith.ProgramWithIcon> ConvertPrograms(IEnumerable<string> recommended)
        {
            foreach (string x in recommended)
            {
                if (!string.IsNullOrEmpty(x))
                {
                    string[] filepath = x.Replace("rundll32.exe", "").Replace("\"", "").Split(new char[]
                    {
                        '\\'
                    });
                    for (int i = 0; i < filepath.Length; i++)
                    {
                        string aggr = "";
                        foreach (string part in filepath.Skip(i))
                        {
                            if (aggr != "" && !aggr.EndsWith("\\"))
                            {
                                aggr += "\\";
                            }
                            string[] subparts = part.Split(new char[]
                            {
                                ' '
                            });
                            foreach (string subpart in subparts)
                            {
                                aggr += ((aggr.EndsWith("\\") || aggr == "") ? subpart : (" " + subpart));
                                if (File.Exists(aggr))
                                {
                                    FileVersionInfo fileinfo = FileVersionInfo.GetVersionInfo(aggr);
                                    yield return new OpenWith.ProgramWithIcon
                                    {
                                        ProgramPath = fileinfo.FileName,
                                        DisplayName = fileinfo.FileDescription,
                                        Icon = System.Drawing.Icon.ExtractAssociatedIcon(aggr).ToImageSource()
                                    };
                                }
                            }
                        }
                    }
                }
            }
            yield break;
        }

        // Token: 0x06000112 RID: 274 RVA: 0x00009F1C File Offset: 0x0000811C
        private IEnumerable<string> RecommendedPrograms(string ext)
        {
            List<string> progs = new List<string>();
            string str = "Software\\Microsoft\\Windows\\CurrentVersion\\Explorer\\FileExts\\" + ext;
            using (RegistryKey registryKey = Registry.CurrentUser.OpenSubKey(str + "\\OpenWithList"))
            {
                if (registryKey != null)
                {
                    string text = (string)registryKey.GetValue("MRUList");
                    if (text != null)
                    {
                        string text2 = text;
                        for (int i = 0; i < text2.Length; i++)
                        {
                            string item3 = registryKey.GetValue(text2[i].ToString(CultureInfo.InvariantCulture)).ToString();
                            if (!progs.Contains(item3))
                            {
                                progs.Add(item3);
                            }
                        }
                    }
                }
            }
            using (RegistryKey registryKey = Registry.CurrentUser.OpenSubKey(str + "\\OpenWithProgids"))
            {
                if (registryKey != null)
                {
                    progs.AddRange(registryKey.GetValueNames());
                }
            }
            using (RegistryKey registryKey = Registry.ClassesRoot.OpenSubKey(ext + "\\OpenWithList"))
            {
                if (registryKey != null)
                {
                    foreach (string item2 in from item in registryKey.GetSubKeyNames()
                                             where !progs.Contains(item)
                                             select item)
                    {
                        progs.Add(item2);
                    }
                }
            }
            using (RegistryKey registryKey = Registry.ClassesRoot.OpenSubKey(ext + "\\OpenWithProgids"))
            {
                if (registryKey != null)
                {
                    foreach (string item2 in from item in registryKey.GetValueNames()
                                             where !progs.Contains(item)
                                             select item)
                    {
                        progs.Add(item2);
                    }
                }
            }
            return progs;
        }

        // Token: 0x06000113 RID: 275 RVA: 0x0000A200 File Offset: 0x00008400
        private string GetRegisteredApplication(string strProgID)
        {
            RegistryKey registryKey = RegistryKey.OpenBaseKey(RegistryHive.ClassesRoot, Environment.Is64BitOperatingSystem ? RegistryView.Registry64 : RegistryView.Registry32);
            string result;
            try
            {
                RegistryKey registryKey2 = registryKey.OpenSubKey(strProgID + "\\shell\\open\\command") ?? registryKey.OpenSubKey("\\Applications\\" + strProgID + "\\shell\\open\\command");
                if (registryKey2 == null)
                {
                    return null;
                }
                result = registryKey2.GetValue(null).ToString();
                registryKey2.Close();
            }
            catch (Exception)
            {
                return null;
            }
            return result;
        }

        // Token: 0x06000114 RID: 276 RVA: 0x0000A2CC File Offset: 0x000084CC
        private static string GetFullPath(string fileName)
        {
            string result;
            if (File.Exists(fileName))
            {
                result = Path.GetFullPath(fileName);
            }
            else
            {
                string environmentVariable = Environment.GetEnvironmentVariable("PATH");
                if (environmentVariable != null)
                {
                    result = (from path in environmentVariable.Split(new char[]
                    {
                        ';'
                    })
                              select Path.Combine(path, fileName)).FirstOrDefault(new Func<string, bool>(File.Exists));
                }
                else
                {
                    result = null;
                }
            }
            return result;
        }

        // Token: 0x06000115 RID: 277 RVA: 0x0000A694 File Offset: 0x00008894
        private static string FindAppPath(string appName)
        {
            if (string.IsNullOrEmpty(appName))
            {
                throw new ArgumentNullException("appName");
            }
            using (RegistryKey key = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstall"))
            {
                if (key != null)
                {
                    var list = (from skName in key.GetSubKeyNames()
                                let subkey = key.OpenSubKey(skName)
                                select new
                                {
                                    name = (subkey.GetValue("DisplayName") as string),
                                    path = (subkey.GetValue("InstallLocation") as string)
                                }).ToList();
                    var list2 = list.FindAll(program => program.path != null && File.Exists(Path.Combine(program.path, appName)));
                    return (list2.Count > 0) ? Path.Combine(list2[0].path, appName) : null;
                }
            }
            return null;
        }

        // Token: 0x06000116 RID: 278 RVA: 0x0000A7DC File Offset: 0x000089DC
        private void Open_Click(object sender, RoutedEventArgs e)
        {
            if (!(e is MouseButtonEventArgs) || ((MouseButtonEventArgs)e).ClickCount > 1)
            {
                OpenWith.ProgramWithIcon programWithIcon = this.Programs.SelectedItem as OpenWith.ProgramWithIcon;
                if (programWithIcon != null)
                {
                    ProcessStartInfo startInfo = new ProcessStartInfo(programWithIcon.ProgramPath)
                    {
                        WindowStyle = ProcessWindowStyle.Normal,
                        ErrorDialog = true,
                        Arguments = this._file
                    };
                    Process.Start(startInfo);
                    base.DialogResult = new bool?(true);
                    base.Close();
                }
            }
        }

        // Token: 0x06000117 RID: 279 RVA: 0x0000A870 File Offset: 0x00008A70
        private void Close_Click(object sender, RoutedEventArgs e)
        {
            base.DialogResult = null;
            base.Close();
        }

        // Token: 0x0600011A RID: 282 RVA: 0x0000A948 File Offset: 0x00008B48
        [DebuggerNonUserCode]
        [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        void IStyleConnector.Connect(int connectionId, object target)
        {
            if (connectionId == 2)
            {
                ((Grid)target).MouseDown += new MouseButtonEventHandler(this.Open_Click);
            }
        }

        // Token: 0x040000A0 RID: 160
        private string _file;

        // Token: 0x02000012 RID: 18
        private class ProgramWithIcon
        {
            // Token: 0x17000047 RID: 71
            // (get) Token: 0x0600011D RID: 285 RVA: 0x0000A978 File Offset: 0x00008B78
            // (set) Token: 0x0600011E RID: 286 RVA: 0x0000A98F File Offset: 0x00008B8F
            public string ProgramPath { get; set; }

            // Token: 0x17000048 RID: 72
            // (get) Token: 0x0600011F RID: 287 RVA: 0x0000A998 File Offset: 0x00008B98
            // (set) Token: 0x06000120 RID: 288 RVA: 0x0000A9AF File Offset: 0x00008BAF
            public string DisplayName { get; set; }

            // Token: 0x17000049 RID: 73
            // (get) Token: 0x06000121 RID: 289 RVA: 0x0000A9B8 File Offset: 0x00008BB8
            // (set) Token: 0x06000122 RID: 290 RVA: 0x0000A9CF File Offset: 0x00008BCF
            public ImageSource Icon { get; set; }
        }
    }
}
