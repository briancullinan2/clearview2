using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace EPIC.ClearView.Patient
{
	// Token: 0x02000021 RID: 33
	[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
	[CompilerGenerated]
	[DebuggerNonUserCode]
	internal class Patient
	{
		// Token: 0x060001BB RID: 443 RVA: 0x00011692 File Offset: 0x0000F892
		internal Patient()
		{
		}

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x060001BC RID: 444 RVA: 0x000116A0 File Offset: 0x0000F8A0
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static ResourceManager ResourceManager
		{
			get
			{
				if (object.ReferenceEquals(Patient.resourceMan, null))
				{
					ResourceManager resourceManager = new ResourceManager("EPIC.ClearView.Patient.Patient", typeof(Patient).Assembly);
					Patient.resourceMan = resourceManager;
				}
				return Patient.resourceMan;
			}
		}

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x060001BD RID: 445 RVA: 0x000116EC File Offset: 0x0000F8EC
		// (set) Token: 0x060001BE RID: 446 RVA: 0x00011703 File Offset: 0x0000F903
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static CultureInfo Culture
		{
			get
			{
				return Patient.resourceCulture;
			}
			set
			{
				Patient.resourceCulture = value;
			}
		}

		// Token: 0x040000F6 RID: 246
		private static ResourceManager resourceMan;

		// Token: 0x040000F7 RID: 247
		private static CultureInfo resourceCulture;
	}
}
