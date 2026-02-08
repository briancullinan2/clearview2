using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using EPICClearView.Properties;

namespace EPICClearView.Utilities
{
	// Token: 0x0200004D RID: 77
	public class ClearViewConfiguration
	{
		// Token: 0x1400000A RID: 10
		// (add) Token: 0x0600029F RID: 671 RVA: 0x000163DC File Offset: 0x000145DC
		// (remove) Token: 0x060002A0 RID: 672 RVA: 0x00016418 File Offset: 0x00014618
		public static event ClearViewConfiguration.SettingsChangedEvent Changed;

		// Token: 0x060002A1 RID: 673 RVA: 0x00016452 File Offset: 0x00014652
		private ClearViewConfiguration()
		{
			this.SetupConfigWatcher();
		}

		// Token: 0x060002A2 RID: 674 RVA: 0x00016464 File Offset: 0x00014664
		private void SetupConfigWatcher()
		{
			ClearViewConfiguration._watcher = new FileSystemWatcher
			{
				Path = Path.GetDirectoryName(ClearViewConfiguration._configuration.FilePath),
				Filter = Path.GetFileName(ClearViewConfiguration._configuration.FilePath),
				NotifyFilter = NotifyFilters.LastWrite,
				EnableRaisingEvents = true
			};
			ClearViewConfiguration._watcher.Changed += this.ConfigChangedCallback;
		}

		// Token: 0x060002A3 RID: 675 RVA: 0x000164D4 File Offset: 0x000146D4
		private void ConfigChangedCallback(object sender, FileSystemEventArgs fileSystemEventArgs)
		{
		}

		// Token: 0x17000087 RID: 135
		// (get) Token: 0x060002A4 RID: 676 RVA: 0x00016554 File Offset: 0x00014754
		public static ClearViewConfiguration Current
		{
			get
			{
				ClearViewConfiguration result;
				if ((result = ClearViewConfiguration._instance) == null)
				{
					result = (ClearViewConfiguration._instance = new ClearViewConfiguration());
				}
				return result;
			}
		}


		// Token: 0x1700008B RID: 139
		// (get) Token: 0x060002AB RID: 683 RVA: 0x000168E4 File Offset: 0x00014AE4
		public string Title
		{
			get
			{
				FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location);
				return string.Format("{0} ({1})", versionInfo.ProductName, versionInfo.ProductVersion);
			}
		}

		// Token: 0x1700008C RID: 140
		// (get) Token: 0x060002AC RID: 684 RVA: 0x0001691C File Offset: 0x00014B1C
		public string Version
		{
			get
			{
				FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location);
				return versionInfo.ProductVersion;
			}
		}

		// Token: 0x060002AD RID: 685 RVA: 0x00016944 File Offset: 0x00014B44
		public static string NewGuid()
		{
			return ""; // ClearViewConfiguration.ToHexString(ClearViewConfiguration.Current.Device.UidQualifier, 6) + "-" + Guid.NewGuid();
		}

		// Token: 0x060002AE RID: 686 RVA: 0x00016980 File Offset: 0x00014B80
		private static string ToHexString(int value, int length)
		{
			if (length > 8)
			{
				length = 8;
			}
			char[] array = new char[length];
			for (int i = array.Length - 1; i >= 0; i--)
			{
				array[i] = ClearViewConfiguration.HexDigitChars[value & 15];
				value >>= 4;
			}
			return new string(array);
		}


		// Token: 0x04000160 RID: 352
		private static ClearViewConfiguration _instance;

        // Token: 0x04000161 RID: 353
        private static Configuration _configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

        // Token: 0x04000162 RID: 354
        private static FileSystemWatcher _watcher;

		// Token: 0x04000165 RID: 357
		private static readonly char[] HexDigitChars = new char[]
		{
			'0',
			'1',
			'2',
			'3',
			'4',
			'5',
			'6',
			'7',
			'8',
			'9',
			'a',
			'b',
			'c',
			'd',
			'e',
			'f'
		};

		// Token: 0x0200004E RID: 78
		// (Invoke) Token: 0x060002B2 RID: 690
		public delegate void SettingsChangedEvent(object sender, FileSystemEventArgs evt);
	}
}
