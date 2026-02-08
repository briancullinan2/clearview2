using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace EPIC.Capture
{
	// Token: 0x02000004 RID: 4
	[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
	[CompilerGenerated]
	[DebuggerNonUserCode]
	public class Capture
	{
		// Token: 0x06000030 RID: 48 RVA: 0x0000389A File Offset: 0x00001A9A
		internal Capture()
		{
		}

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000031 RID: 49 RVA: 0x000038A8 File Offset: 0x00001AA8
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public static ResourceManager ResourceManager
		{
			get
			{
				if (object.ReferenceEquals(Capture.resourceMan, null))
				{
					ResourceManager resourceManager = new ResourceManager("EPIC.Capture.Capture", typeof(Capture).Assembly);
					Capture.resourceMan = resourceManager;
				}
				return Capture.resourceMan;
			}
		}

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000032 RID: 50 RVA: 0x000038F4 File Offset: 0x00001AF4
		// (set) Token: 0x06000033 RID: 51 RVA: 0x0000390B File Offset: 0x00001B0B
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public static CultureInfo Culture
		{
			get
			{
				return Capture.resourceCulture;
			}
			set
			{
				Capture.resourceCulture = value;
			}
		}

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000034 RID: 52 RVA: 0x00003914 File Offset: 0x00001B14
		public static string Calibrate_AutoCalibration
		{
			get
			{
				return Capture.ResourceManager.GetString("Calibrate_AutoCalibration", Capture.resourceCulture);
			}
		}

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000035 RID: 53 RVA: 0x0000393C File Offset: 0x00001B3C
		public static string Calibrate_CalibrationSettings
		{
			get
			{
				return Capture.ResourceManager.GetString("Calibrate_CalibrationSettings", Capture.resourceCulture);
			}
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000036 RID: 54 RVA: 0x00003964 File Offset: 0x00001B64
		public static string Calibrate_CapturedImage
		{
			get
			{
				return Capture.ResourceManager.GetString("Calibrate_CapturedImage", Capture.resourceCulture);
			}
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000037 RID: 55 RVA: 0x0000398C File Offset: 0x00001B8C
		public static string Calibrate_Close
		{
			get
			{
				return Capture.ResourceManager.GetString("Calibrate_Close", Capture.resourceCulture);
			}
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000038 RID: 56 RVA: 0x000039B4 File Offset: 0x00001BB4
		public static string Calibrate_DutyCycle
		{
			get
			{
				return Capture.ResourceManager.GetString("Calibrate_DutyCycle", Capture.resourceCulture);
			}
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000039 RID: 57 RVA: 0x000039DC File Offset: 0x00001BDC
		public static string Calibrate_ImagePreview
		{
			get
			{
				return Capture.ResourceManager.GetString("Calibrate_ImagePreview", Capture.resourceCulture);
			}
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x0600003A RID: 58 RVA: 0x00003A04 File Offset: 0x00001C04
		public static string Calibrate_ResetImages
		{
			get
			{
				return Capture.ResourceManager.GetString("Calibrate_ResetImages", Capture.resourceCulture);
			}
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x0600003B RID: 59 RVA: 0x00003A2C File Offset: 0x00001C2C
		public static string Calibrate_StopDutyCycle
		{
			get
			{
				return Capture.ResourceManager.GetString("Calibrate_StopDutyCycle", Capture.resourceCulture);
			}
		}

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x0600003C RID: 60 RVA: 0x00003A54 File Offset: 0x00001C54
		public static string Calibrate_StopReconnect
		{
			get
			{
				return Capture.ResourceManager.GetString("Calibrate_StopReconnect", Capture.resourceCulture);
			}
		}

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x0600003D RID: 61 RVA: 0x00003A7C File Offset: 0x00001C7C
		public static string Calibrate_Title
		{
			get
			{
				return Capture.ResourceManager.GetString("Calibrate_Title", Capture.resourceCulture);
			}
		}

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x0600003E RID: 62 RVA: 0x00003AA4 File Offset: 0x00001CA4
		public static string Settings_BoostVoltage
		{
			get
			{
				return Capture.ResourceManager.GetString("Settings_BoostVoltage", Capture.resourceCulture);
			}
		}

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x0600003F RID: 63 RVA: 0x00003ACC File Offset: 0x00001CCC
		public static string Settings_Brightness
		{
			get
			{
				return Capture.ResourceManager.GetString("Settings_Brightness", Capture.resourceCulture);
			}
		}

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000040 RID: 64 RVA: 0x00003AF4 File Offset: 0x00001CF4
		public static string Settings_Cancel
		{
			get
			{
				return Capture.ResourceManager.GetString("Settings_Cancel", Capture.resourceCulture);
			}
		}

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x06000041 RID: 65 RVA: 0x00003B1C File Offset: 0x00001D1C
		public static string Settings_Default
		{
			get
			{
				return Capture.ResourceManager.GetString("Settings_Default", Capture.resourceCulture);
			}
		}

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000042 RID: 66 RVA: 0x00003B44 File Offset: 0x00001D44
		public static string Settings_ExposureDelay
		{
			get
			{
				return Capture.ResourceManager.GetString("Settings_ExposureDelay", Capture.resourceCulture);
			}
		}

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x06000043 RID: 67 RVA: 0x00003B6C File Offset: 0x00001D6C
		public static string Settings_Frequency
		{
			get
			{
				return Capture.ResourceManager.GetString("Settings_Frequency", Capture.resourceCulture);
			}
		}

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x06000044 RID: 68 RVA: 0x00003B94 File Offset: 0x00001D94
		public static string Settings_Gain
		{
			get
			{
				return Capture.ResourceManager.GetString("Settings_Gain", Capture.resourceCulture);
			}
		}

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x06000045 RID: 69 RVA: 0x00003BBC File Offset: 0x00001DBC
		public static string Settings_PulseDuration
		{
			get
			{
				return Capture.ResourceManager.GetString("Settings_PulseDuration", Capture.resourceCulture);
			}
		}

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x06000046 RID: 70 RVA: 0x00003BE4 File Offset: 0x00001DE4
		public static string Settings_PulseWidth
		{
			get
			{
				return Capture.ResourceManager.GetString("Settings_PulseWidth", Capture.resourceCulture);
			}
		}

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x06000047 RID: 71 RVA: 0x00003C0C File Offset: 0x00001E0C
		public static string Settings_SaveChanges
		{
			get
			{
				return Capture.ResourceManager.GetString("Settings_SaveChanges", Capture.resourceCulture);
			}
		}

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x06000048 RID: 72 RVA: 0x00003C34 File Offset: 0x00001E34
		public static string Settings_SaveError
		{
			get
			{
				return Capture.ResourceManager.GetString("Settings_SaveError", Capture.resourceCulture);
			}
		}

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x06000049 RID: 73 RVA: 0x00003C5C File Offset: 0x00001E5C
		public static string Settings_SerialNumber
		{
			get
			{
				return Capture.ResourceManager.GetString("Settings_SerialNumber", Capture.resourceCulture);
			}
		}

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x0600004A RID: 74 RVA: 0x00003C84 File Offset: 0x00001E84
		public static string Settings_Title
		{
			get
			{
				return Capture.ResourceManager.GetString("Settings_Title", Capture.resourceCulture);
			}
		}

		// Token: 0x04000022 RID: 34
		private static ResourceManager resourceMan;

		// Token: 0x04000023 RID: 35
		private static CultureInfo resourceCulture;
	}
}
