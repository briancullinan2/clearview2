using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using System.Windows; // ensure Application and DispatcherPriority resolve when compiled in WPF project
using log4net;
using SensorTechnology;

namespace EPIC.CameraInterface
{
	/// <summary>
	/// Controls Sentech cameras.
	/// </summary>
	// Token: 0x0200000E RID: 14
	public class Sentech : ICapturable, IDisposable
	{
		// Token: 0x06000062 RID: 98 RVA: 0x000035B0 File Offset: 0x000017B0
		internal Sentech()
		{
			this._waiter = new Semaphore(1, 1);
		}

		// Token: 0x06000063 RID: 99
		[DllImport("gdi32")]
		private static extern int DeleteObject(IntPtr o);

		// Token: 0x06000064 RID: 100
		[DllImport("kernel32.dll", CharSet = CharSet.Auto)]
		private static extern int FormatMessage(int dwFlags, IntPtr lpSource, int dwMessageId, int dwLanguageId, StringBuilder lpBuffer, int nSize, IntPtr arguments);

		// Token: 0x06000065 RID: 101
		[DllImport("kernel32.dll", CharSet = CharSet.Auto)]
		private static extern IntPtr LoadLibraryEx(string lpszLibFile, IntPtr hFile, int dwFlags);

		// Token: 0x06000066 RID: 102
		[DllImport("kernel32.dll")]
		private static extern int FreeLibrary(IntPtr hModule);

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x06000067 RID: 103 RVA: 0x000035D4 File Offset: 0x000017D4
		// (set) Token: 0x06000068 RID: 104 RVA: 0x000035DC File Offset: 0x000017DC
		public double FramesPerSecond
		{
			get
			{
				return this._fps;
			}
			set
			{
				this._fps = value;
			}
		}

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x06000069 RID: 105 RVA: 0x000035E5 File Offset: 0x000017E5
		// (set) Token: 0x0600006A RID: 106 RVA: 0x00003611 File Offset: 0x00001811
		public int Brightness
		{
			get
			{
				if (!StCam.GetDigitalGain(this._cameraCapture, out this._digitalGain))
				{
					this.LogError(StCam.GetLastError(this._cameraCapture));
				}
				return (int)this._digitalGain;
			}
			set
			{
				this._digitalGain = (ushort)value;
				if (!StCam.SetDigitalGain(this._cameraCapture, this._digitalGain))
				{
					this.LogError(StCam.GetLastError(this._cameraCapture));
				}
			}
		}

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x0600006B RID: 107 RVA: 0x0000363F File Offset: 0x0000183F
		// (set) Token: 0x0600006C RID: 108 RVA: 0x0000366B File Offset: 0x0000186B
		public int Gain
		{
			get
			{
				if (!StCam.GetGain(this._cameraCapture, out this._gain))
				{
					this.LogError(StCam.GetLastError(this._cameraCapture));
				}
				return (int)this._gain;
			}
			set
			{
				this._gain = (ushort)value;
				if (!StCam.SetGain(this._cameraCapture, this._gain))
				{
					this.LogError(StCam.GetLastError(this._cameraCapture));
				}
			}
		}

		/// <summary>
		/// Sets the clock speed of the camera.
		/// </summary>
		// Token: 0x1700001D RID: 29
		// (get) Token: 0x0600006D RID: 109 RVA: 0x0000369C File Offset: 0x0000189C
		// (set) Token: 0x0600006E RID: 110 RVA: 0x000036EC File Offset: 0x000018EC
		public Sentech.ClockSpeed Clock
		{
			get
			{
				uint clockMode;
				uint clock;
				if (!StCam.GetClock(this._cameraCapture, out clockMode, out clock))
				{
					this.LogError(StCam.GetLastError(this._cameraCapture));
				}
				switch (clockMode)
				{
				case 0U:
					return Sentech.ClockSpeed.Normal;
				case 1U:
					return Sentech.ClockSpeed.Diviging2;
				case 2U:
					return Sentech.ClockSpeed.Dividing4;
				default:
					throw new NotImplementedException();
				}
			}
			set
			{
				uint clockMode;
				switch (value)
				{
				case Sentech.ClockSpeed.Normal:
					clockMode = 0U;
					break;
				case Sentech.ClockSpeed.Dividing4:
					clockMode = 2U;
					break;
				case Sentech.ClockSpeed.Diviging2:
					clockMode = 1U;
					break;
				default:
					throw new NotImplementedException();
				}
				if (!StCam.SetClock(this._cameraCapture, clockMode, 0U))
				{
					this.LogError(StCam.GetLastError(this._cameraCapture));
				}
			}
		}

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x0600006F RID: 111 RVA: 0x00003744 File Offset: 0x00001944
		// (set) Token: 0x06000070 RID: 112 RVA: 0x0000379C File Offset: 0x0000199C
		public int Height
		{
			get
			{
				if (!StCam.GetImageSize(this._cameraCapture, out this._dwReserved, out this._wScanMode, out this._dwOffsetX, out this._dwOffsetY, out this._dwWidth, out this._dwHeight))
				{
					this.LogError(StCam.GetLastError(this._cameraCapture));
				}
				return (int)this._dwHeight;
			}
			set
			{
				this._dwHeight = (uint)value;
				if (!StCam.SetImageSize(this._cameraCapture, this._dwReserved, this._wScanMode, this._dwOffsetX, this._dwOffsetY, this._dwWidth, this._dwHeight))
				{
					this.LogError(StCam.GetLastError(this._cameraCapture));
				}
			}
		}

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x06000071 RID: 113 RVA: 0x000037F2 File Offset: 0x000019F2
		// (set) Token: 0x06000072 RID: 114 RVA: 0x0000382A File Offset: 0x00001A2A
		public int Width
		{
			get
			{
				StCam.GetImageSize(this._cameraCapture, out this._dwReserved, out this._wScanMode, out this._dwOffsetX, out this._dwOffsetY, out this._dwWidth, out this._dwHeight);
				return (int)this._dwWidth;
			}
			set
			{
				this._dwWidth = (uint)value;
				StCam.SetImageSize(this._cameraCapture, this._dwReserved, this._wScanMode, this._dwOffsetX, this._dwOffsetY, this._dwWidth, this._dwHeight);
			}
		}

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x06000073 RID: 115 RVA: 0x00003863 File Offset: 0x00001A63
		// (set) Token: 0x06000074 RID: 116 RVA: 0x0000386B File Offset: 0x00001A6B
		public string DisplayName { get; set; }

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x06000075 RID: 117 RVA: 0x00003874 File Offset: 0x00001A74
		// (set) Token: 0x06000076 RID: 118 RVA: 0x0000387C File Offset: 0x00001A7C
		public object UniqueIdentifier { get; set; }

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x06000077 RID: 119 RVA: 0x00003885 File Offset: 0x00001A85
		// (set) Token: 0x06000078 RID: 120 RVA: 0x000038B7 File Offset: 0x00001AB7
		public int Exposure
		{
			get
			{
				if (!StCam.GetShutterSpeed(this._cameraCapture, out this._shutterLine, out this._shutterClock))
				{
					this.LogError(StCam.GetLastError(this._cameraCapture));
				}
				return (int)this._shutterLine;
			}
			set
			{
				this._shutterLine = (ushort)value;
				if (!StCam.SetShutterSpeed(this._cameraCapture, this._shutterLine, 0))
				{
					this.LogError(StCam.GetLastError(this._cameraCapture));
				}
			}
		}

		/// <summary>
		/// Sets the format of the pixels, Sentech supports 8 bit raw, 24 BGR or 32 BGR.
		/// </summary>
		// Token: 0x17000023 RID: 35
		// (get) Token: 0x06000079 RID: 121 RVA: 0x000038E8 File Offset: 0x00001AE8
		// (set) Token: 0x0600007A RID: 122 RVA: 0x00003948 File Offset: 0x00001B48
		public PixelFormat PixelFormat
		{
			get
			{
				if (!StCam.GetPreviewPixelFormat(this._cameraCapture, out this._dwPreviewPixelFormat))
				{
					this.LogError(StCam.GetLastError(this._cameraCapture));
				}
				uint dwPreviewPixelFormat = this._dwPreviewPixelFormat;
				if (dwPreviewPixelFormat == 1U)
				{
					return PixelFormat.Format8bppIndexed;
				}
				if (dwPreviewPixelFormat == 4U)
				{
					return PixelFormat.Format24bppRgb;
				}
				if (dwPreviewPixelFormat != 8U)
				{
					throw new NotImplementedException();
				}
				return PixelFormat.Format32bppRgb;
			}
			set
			{
				if (value != PixelFormat.Format24bppRgb)
				{
					if (value != PixelFormat.Format32bppRgb)
					{
						if (value != PixelFormat.Format8bppIndexed)
						{
							throw new NotImplementedException();
						}
						this._dwPreviewPixelFormat = 1U;
					}
					else
					{
						this._dwPreviewPixelFormat = 8U;
					}
				}
				else
				{
					this._dwPreviewPixelFormat = 4U;
				}
				if (!StCam.SetPreviewPixelFormat(this._cameraCapture, this._dwPreviewPixelFormat))
				{
					this.LogError(StCam.GetLastError(this._cameraCapture));
				}
			}
		}

		// Token: 0x0600007B RID: 123 RVA: 0x000039C8 File Offset: 0x00001BC8
		public void Open()
		{
			if (this._cameraCapture != IntPtr.Zero)
			{
				return;
			}
			try
			{
				if (!this._waiter.WaitOne(0))
				{
					Application.Current.Dispatcher.BeginInvoke(new Action(this.Open), DispatcherPriority.Send, new object[0]);
				}
				else
				{
					ICapturable orig = CameraManager.Current.Cameras.FirstOrDefault((ICapturable x) => x.UniqueIdentifier == this.UniqueIdentifier);
					int index = CameraManager.Current.Cameras.IndexOf(orig);
					this._cameraCapture = StCam.Open((uint)index);
					if (!StCam.CreatePreviewWindow(this._cameraCapture, "Preview", 0U, 0, 0, 0U, 0U, IntPtr.Zero, IntPtr.Zero, true))
					{
						this.LogError(StCam.GetLastError(this._cameraCapture));
					}
					if (!StCam.SetSharpnessMode(this._cameraCapture, 0, 0, 0))
					{
						this.LogError(StCam.GetLastError(this._cameraCapture));
					}
					if (!StCam.SetALCMode(this._cameraCapture, 0))
					{
						this.LogError(StCam.GetLastError(this._cameraCapture));
					}
					StCam.SetMirrorMode(this._cameraCapture, 3);
					this.PixelFormat = PixelFormat.Format32bppRgb;
					this.Clock = Sentech.ClockSpeed.Diviging2;
					this.Gain = 115;
					this.Brightness = 115;
					this.Exposure = 575;
					if (!StCam.StartTransfer(this._cameraCapture))
					{
						this.LogError(StCam.GetLastError(this._cameraCapture));
					}
					this._callback = new StCam.fStCamPreviewBitmapCallbackFunc(this.Callback);
					if (!StCam.AddPreviewBitmapCallback(this._cameraCapture, this._callback, IntPtr.Zero, out this._mDwPreviewGdiCallbackNo))
					{
						this.LogError(StCam.GetLastError(this._cameraCapture));
					}
					Sentech.Log.Debug("Camera opened.");
				}
			}
			catch (Exception ex)
			{
				Sentech.Log.Error("There was an error opening the Sentech camera.", ex);
				throw;
			}
			finally
			{
				this._waiter.Release();
			}
		}

		/// <summary>
		/// Occurs when a frame is captures from the camera.
		/// </summary>
		// Token: 0x14000004 RID: 4
		// (add) Token: 0x0600007C RID: 124 RVA: 0x00003BD4 File Offset: 0x00001DD4
		// (remove) Token: 0x0600007D RID: 125 RVA: 0x00003C44 File Offset: 0x00001E44
		public event FrameCallback Captured
		{
			[MethodImpl(MethodImplOptions.Synchronized)]
			add
			{
				FrameCallback log = this._captured;
				FrameCallback handler2;
				do
				{
					handler2 = log;
					FrameCallback handler3 = (FrameCallback)Delegate.Combine(handler2, value);
					log = Interlocked.CompareExchange<FrameCallback>(ref this._captured, handler3, handler2);
				}
				while (log != handler2);
				if (this._cameraCapture == IntPtr.Zero)
				{
					Application.Current.Dispatcher.BeginInvoke(new Action(this.Open), DispatcherPriority.Send, new object[0]);
				}
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			remove
			{
				FrameCallback log = this._captured;
				FrameCallback handler2;
				do
				{
					handler2 = log;
					FrameCallback handler3 = (FrameCallback)Delegate.Remove(handler2, value);
					log = Interlocked.CompareExchange<FrameCallback>(ref this._captured, handler3, handler2);
				}
				while (log != handler2);
				if (this._captured == null)
				{
					Application.Current.Dispatcher.BeginInvoke(new Action(this.Close), DispatcherPriority.Send, new object[0]);
				}
			}
		}

		// Token: 0x0600007E RID: 126 RVA: 0x00003CAC File Offset: 0x00001EAC
		private void Callback(IntPtr pbyteBitmap, uint dwBufferSize, uint width, uint height, uint dwFrameNo, uint previewPixelFormat, IntPtr lpContext, IntPtr lpReserved)
		{
			this._waiter.WaitOne();
			int stride = (int)width;
			PixelFormat pixelFormat;
			if (previewPixelFormat != 1U)
			{
				if (previewPixelFormat != 4U)
				{
					if (previewPixelFormat != 8U)
					{
						throw new NotImplementedException();
					}
					stride *= 4;
					pixelFormat = PixelFormat.Format32bppRgb;
				}
				else
				{
					stride *= 3;
					pixelFormat = PixelFormat.Format24bppRgb;
				}
			}
			else
			{
				pixelFormat = PixelFormat.Format8bppIndexed;
			}
			using (Bitmap bitmap = new Bitmap((int)width, (int)height, stride, pixelFormat, pbyteBitmap))
			{
				using (Bitmap resized = Binner.Bin(bitmap, 2, BinMethod.Maximum))
				{
					try
					{
						IntPtr hBitmap = resized.GetHbitmap();
						if (this._captured != null)
						{
							this._captured(hBitmap);
						}
						Sentech.DeleteObject(hBitmap);
					}
					catch (Exception ex)
					{
						Sentech.Log.Error("Error in frame callback, continuing.", ex);
					}
				}
			}
			this._waiter.Release();
		}

		// Token: 0x0600007F RID: 127 RVA: 0x00003DA0 File Offset: 0x00001FA0
		public bool Is(string hardwareName)
		{
			return !string.IsNullOrEmpty(hardwareName) && (hardwareName.Contains("StUSB") || hardwareName.Contains("Sentech"));
		}

		// Token: 0x06000080 RID: 128 RVA: 0x00003DC8 File Offset: 0x00001FC8
		public void Close()
		{
			if (this._cameraCapture == IntPtr.Zero)
			{
				return;
			}
			try
			{
				this._captured = null;
				if (!this._waiter.WaitOne(0))
				{
					Application.Current.Dispatcher.BeginInvoke(new Action(this.Close), DispatcherPriority.Send, new object[0]);
				}
				else
				{
					if (!StCam.StopTransfer(this._cameraCapture))
					{
						this.LogError(StCam.GetLastError(this._cameraCapture));
					}
					StCam.Close(this._cameraCapture);
					Sentech.Log.Debug("Camera closed.");
					this._cameraCapture = IntPtr.Zero;
				}
			}
			finally
			{
				this._waiter.Release();
			}
		}

		// Token: 0x06000081 RID: 129 RVA: 0x00003E88 File Offset: 0x00002088
		private void LogError(uint dwErrorCode)
		{
			StringBuilder strErrorMsg = new StringBuilder(255);
			int dwFlags = 4096;
			IntPtr ptrlpSource = IntPtr.Zero;
			if (Sentech.FormatMessage(dwFlags, ptrlpSource, (int)dwErrorCode, 0, strErrorMsg, strErrorMsg.Capacity, IntPtr.Zero) == 0)
			{
				ptrlpSource = Sentech.LoadLibraryEx("StCamMsg.dll", IntPtr.Zero, 1);
				dwFlags |= 2048;
				if (!ptrlpSource.Equals(IntPtr.Zero))
				{
					Sentech.FormatMessage(dwFlags, ptrlpSource, (int)dwErrorCode, 0, strErrorMsg, strErrorMsg.Capacity, IntPtr.Zero);
					Sentech.FreeLibrary(ptrlpSource);
				}
			}
			Sentech.Log.Error(strErrorMsg.ToString());
			throw new Exception(strErrorMsg.ToString());
		}

		// Token: 0x06000082 RID: 130 RVA: 0x00003F2F File Offset: 0x0000212F
		public void Dispose()
		{
			this.Close();
		}

		// Token: 0x04000017 RID: 23
		private readonly Semaphore _waiter;

		// Token: 0x04000018 RID: 24
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(typeof(Sentech));

		// Token: 0x04000019 RID: 25
		private IntPtr _cameraCapture;

		// Token: 0x0400001A RID: 26
		private StCam.fStCamPreviewBitmapCallbackFunc _callback;

		// Token: 0x0400001B RID: 27
		private uint _mDwPreviewGdiCallbackNo;

		// Token: 0x0400001C RID: 28
		private uint _dwReserved;

		// Token: 0x0400001D RID: 29
		private ushort _wScanMode;

		// Token: 0x0400001E RID: 30
		private uint _dwOffsetX;

		// Token: 0x0400001F RID: 31
		private uint _dwOffsetY;

		// Token: 0x04000020 RID: 32
		private uint _dwWidth;

		// Token: 0x04000021 RID: 33
		private uint _dwHeight;

		// Token: 0x04000022 RID: 34
		private uint _dwPreviewPixelFormat;

		// Token: 0x04000023 RID: 35
		private ushort _gain;

		// Token: 0x04000024 RID: 36
		private ushort _digitalGain;

		// Token: 0x04000025 RID: 37
		private ushort _shutterLine;

		// Token: 0x04000026 RID: 38
		private ushort _shutterClock;

		// Token: 0x04000027 RID: 39
		private double _fps = 10.0;

		// Token: 0x04000028 RID: 40
		private FrameCallback _captured;

		/// <summary>
		/// Modes for the speed of the internal CPU clock in the Sentech camera.
		/// </summary>
		// Token: 0x0200000F RID: 15
		public enum ClockSpeed
		{
			/// <summary>
			/// Runs at 36.818 MHz
			/// </summary>
			// Token: 0x0400002C RID: 44
			Normal,
			/// <summary>
			/// Runs at 18.409 MHz
			/// </summary>
			// Token: 0x0400002D RID: 45
			Dividing4,
			/// <summary>
			/// Runs at 9.204 MHz
			/// </summary>
			// Token: 0x0400002E RID: 46
			Diviging2
		}
	}
}
