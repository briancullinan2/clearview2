using EPIC.CameraInterface.Utilities;
using System;

namespace EPIC.CameraInterface.Interfaces
{
	/// <summary>
	/// Controls connection to cameras through a common interface.
	/// </summary>
	// Token: 0x0200000A RID: 10
	public interface ICapturable : IDisposable
	{
		/// <summary>
		/// Opens communication with the camera.
		/// </summary>
		// Token: 0x06000029 RID: 41
		void Open();

		/// <summary>
		/// Occurs when a frame is captured and stored in memory.
		/// </summary>
		// Token: 0x14000002 RID: 2
		// (add) Token: 0x0600002A RID: 42
		// (remove) Token: 0x0600002B RID: 43
		event FrameCallback Captured;

		/// <summary>
		/// Checks if the display name of a camera is of implemented type.
		/// </summary>
		/// <param name="hardwareName"></param>
		/// <returns></returns>
		// Token: 0x0600002C RID: 44
		bool Is(string hardwareName);

		/// <summary>
		/// Closes communication with the camera.
		/// </summary>
		// Token: 0x0600002D RID: 45
		void Close();

		void Capture();

        /// <summary>
        /// Desired frames per second.
        /// </summary>
        // Token: 0x17000005 RID: 5
        // (get) Token: 0x0600002E RID: 46
        // (set) Token: 0x0600002F RID: 47
        double FramesPerSecond { get; set; }

		/// <summary>
		/// Brightness adjustment for the camera.
		/// </summary>
		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000030 RID: 48
		// (set) Token: 0x06000031 RID: 49
		int Brightness { get; set; }

		/// <summary>
		/// Gain adjustment for the camera.
		/// </summary>
		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000032 RID: 50
		// (set) Token: 0x06000033 RID: 51
		int Gain { get; set; }

		/// <summary>
		/// The exposure time.
		/// </summary>
		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000034 RID: 52
		// (set) Token: 0x06000035 RID: 53
		int Exposure { get; set; }

		/// <summary>
		/// The height of the desired output.
		/// </summary>
		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000036 RID: 54
		// (set) Token: 0x06000037 RID: 55
		int Height { get; set; }

		/// <summary>
		/// The width of the desired output.
		/// </summary>
		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000038 RID: 56
		// (set) Token: 0x06000039 RID: 57
		int Width { get; set; }

		/// <summary>
		/// The display name of the camera.
		/// </summary>
		// Token: 0x1700000B RID: 11
		// (get) Token: 0x0600003A RID: 58
		// (set) Token: 0x0600003B RID: 59
		string DisplayName { get; set; }

		/// <summary>
		/// A unique object used to identify the device such as the hardware ID.
		/// </summary>
		// Token: 0x1700000C RID: 12
		// (get) Token: 0x0600003C RID: 60
		// (set) Token: 0x0600003D RID: 61
		object UniqueIdentifier { get; set; }
	}
}
