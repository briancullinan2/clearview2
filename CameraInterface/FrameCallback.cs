using System;

namespace EPIC.CameraInterface
{
	/// <summary>
	/// The callback for when a frame is captured and stored in memory.
	/// </summary>
	/// <param name="hBitmap"></param>
	// Token: 0x0200000C RID: 12
	// (Invoke) Token: 0x06000058 RID: 88
	public delegate void FrameCallback(IntPtr hBitmap);
}
