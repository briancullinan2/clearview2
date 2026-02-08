using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace EPIC.Properties
{
	// Token: 0x02000070 RID: 112
	[CompilerGenerated]
	[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
	[DebuggerNonUserCode]
	internal class Resources
	{
		// Token: 0x0600037D RID: 893 RVA: 0x0001C3ED File Offset: 0x0001A5ED
		internal Resources()
		{
		}

		// Token: 0x17000099 RID: 153
		// (get) Token: 0x0600037E RID: 894 RVA: 0x0001C3F8 File Offset: 0x0001A5F8
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static ResourceManager ResourceManager
		{
			get
			{
				if (object.ReferenceEquals(Resources.resourceMan, null))
				{
					ResourceManager resourceManager = new ResourceManager("EPIC.Properties.Resources", typeof(Resources).Assembly);
					Resources.resourceMan = resourceManager;
				}
				return Resources.resourceMan;
			}
		}

		// Token: 0x1700009A RID: 154
		// (get) Token: 0x0600037F RID: 895 RVA: 0x0001C444 File Offset: 0x0001A644
		// (set) Token: 0x06000380 RID: 896 RVA: 0x0001C45B File Offset: 0x0001A65B
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static CultureInfo Culture
		{
			get
			{
				return Resources.resourceCulture;
			}
			set
			{
				Resources.resourceCulture = value;
			}
		}

		// Token: 0x1700009B RID: 155
		// (get) Token: 0x06000381 RID: 897 RVA: 0x0001C464 File Offset: 0x0001A664
		internal static string ConfigurationSettings_ConfigChangedCallback_Error
		{
			get
			{
				return Resources.ResourceManager.GetString("ConfigurationSettings_ConfigChangedCallback_Error", Resources.resourceCulture);
			}
		}

		// Token: 0x1700009C RID: 156
		// (get) Token: 0x06000382 RID: 898 RVA: 0x0001C48C File Offset: 0x0001A68C
		internal static string ConfigurationSettings_ConfigurationChanged
		{
			get
			{
				return Resources.ResourceManager.GetString("ConfigurationSettings_ConfigurationChanged", Resources.resourceCulture);
			}
		}

		// Token: 0x1700009D RID: 157
		// (get) Token: 0x06000383 RID: 899 RVA: 0x0001C4B4 File Offset: 0x0001A6B4
		internal static string DeviceSettingStringConverter_String
		{
			get
			{
				return Resources.ResourceManager.GetString("DeviceSettingStringConverter_String", Resources.resourceCulture);
			}
		}

		// Token: 0x040001BB RID: 443
		private static ResourceManager resourceMan;

		// Token: 0x040001BC RID: 444
		private static CultureInfo resourceCulture;
	}
}
