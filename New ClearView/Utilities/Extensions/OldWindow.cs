using System;
using System.Windows.Forms;

namespace EPIC.Utilities.Extensions
{
	// Token: 0x0200005B RID: 91
	public class OldWindow : IWin32Window
	{
		// Token: 0x060002DB RID: 731 RVA: 0x00017828 File Offset: 0x00015A28
		public OldWindow(IntPtr handle)
		{
			this.Handle = handle;
		}

		// Token: 0x1700008E RID: 142
		// (get) Token: 0x060002DC RID: 732 RVA: 0x0001783C File Offset: 0x00015A3C
		// (set) Token: 0x060002DD RID: 733 RVA: 0x00017853 File Offset: 0x00015A53
		public IntPtr Handle { get; private set; }
	}
}
