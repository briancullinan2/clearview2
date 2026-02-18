using EPIC.CameraInterface.Native;
using EPIC.CameraInterface.Utilities;
using OpenCvSharp;
using SensorTechnology;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace EPIC.CameraInterface.Interfaces
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
                    Task.Run(new Action(this.Open));
                }
                else
                {
                    ICapturable orig = CameraManager.Current.Cameras.FirstOrDefault((ICapturable x) => x.UniqueIdentifier == this.UniqueIdentifier);
                    int index = CameraManager.Current.Cameras.IndexOf(orig);
                    this._cameraCapture = StCam.Open((uint)index);
                    if (!StCam.CreatePreviewWindow(this._cameraCapture, "Preview", 0U, 0, 0, 0U, 0U, IntPtr.Zero, IntPtr.Zero, 1))
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
                    this.PixelFormat = MatType.CV_8UC3;
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
                    Log.Debug("Camera opened.");
                }
            }
            catch (Exception ex)
            {
                Log.Error("There was an error opening the Sentech camera.", ex);
                throw;
            }
            finally
            {
                this._waiter.Release();
            }
        }


        public void Capture()
        {
            if (!StCam.StartTransfer(this._cameraCapture))
            {
                this.LogError(StCam.GetLastError(this._cameraCapture));
            }
            //this._callback = new StCam.fStCamPreviewBitmapCallbackFunc(this.Callback);
            //if (!StCam.AddPreviewBitmapCallback(this._cameraCapture, this._callback, IntPtr.Zero, out this._mDwPreviewGdiCallbackNo))
            //{
            //    this.LogError(StCam.GetLastError(this._cameraCapture));
            //}
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
                    Task.Run(new Action(this.Open));
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
                    Task.Run(new Action(this.Close));
                }
            }
        }
        /// <summary>
        /// Sets the format of the pixels.
        /// Mapped Sentech formats to OpenCvSharp MatType:
        /// 1U -> 8 bit (Gray) -> CV_8UC1
        /// 4U -> 24 bit (BGR) -> CV_8UC3
        /// 8U -> 32 bit (BGRA) -> CV_8UC4
        /// </summary>
        public MatType PixelFormat
        {
            get
            {
                if (!StCam.GetPreviewPixelFormat(this._cameraCapture, out this._dwPreviewPixelFormat))
                {
                    this.LogError(StCam.GetLastError(this._cameraCapture));
                }

                uint dwPreviewPixelFormat = this._dwPreviewPixelFormat;

                if (dwPreviewPixelFormat == 1U) return MatType.CV_8UC1; // 8-bit Gray
                if (dwPreviewPixelFormat == 4U) return MatType.CV_8UC3; // 24-bit BGR
                if (dwPreviewPixelFormat == 8U) return MatType.CV_8UC4; // 32-bit BGRA

                throw new NotImplementedException($"Unknown Sentech format: {dwPreviewPixelFormat}");
            }
            set
            {
                // Convert MatType back to Sentech ID
                if (value == MatType.CV_8UC3)
                {
                    this._dwPreviewPixelFormat = 4U;
                }
                else if (value == MatType.CV_8UC4)
                {
                    this._dwPreviewPixelFormat = 8U;
                }
                else if (value == MatType.CV_8UC1)
                {
                    this._dwPreviewPixelFormat = 1U;
                }
                else
                {
                    throw new NotImplementedException($"MatType {value} not supported by Sentech wrapper");
                }

                if (!StCam.SetPreviewPixelFormat(this._cameraCapture, this._dwPreviewPixelFormat))
                {
                    this.LogError(StCam.GetLastError(this._cameraCapture));
                }
            }
        }

        private void Callback(IntPtr pbyteBitmap, uint dwBufferSize, uint width, uint height, uint dwFrameNo, uint previewPixelFormat, IntPtr lpContext, IntPtr lpReserved)
        {
            this._waiter.WaitOne();

            try
            {
                // 1. Determine OpenCV Type and Stride based on Sentech format
                MatType type;
                int stride = (int)width; // Base stride (width * channels)

                if (previewPixelFormat == 1U)
                {
                    type = MatType.CV_8UC1;
                    // stride *= 1;
                }
                else if (previewPixelFormat == 4U)
                {
                    type = MatType.CV_8UC3;
                    stride *= 3;
                }
                else if (previewPixelFormat == 8U)
                {
                    type = MatType.CV_8UC4;
                    stride *= 4;
                }
                else
                {
                    throw new NotImplementedException($"Format {previewPixelFormat} not supported");
                }

                // 2. Create a Mat wrapper around the existing memory (Zero Copy)
                // Note: pbyteBitmap comes from the camera driver. We only read it.
                using (Mat rawMat = Mat.FromPixelData((int)height, (int)width, type, pbyteBitmap, stride))
                {
                    // 3. Perform Binning using your new OpenCvSharp Binner
                    using (Mat resized = Utilities.Rasterizer.Binner.Bin(rawMat, 2, Utilities.Rasterizer.BinMethod.Maximum))
                    {
                        // 4. Convert Mat to HBITMAP (GDI Handle) for legacy interop
                        if (this._captured != null)
                        {
                            IntPtr hBitmap = MatToHBitmap(resized);

                            // Pass ownership of hBitmap to the subscriber
                            this._captured(hBitmap);

                            // Clean up local reference if the subscriber is expected to delete it. 
                            // *HOWEVER*: Your original code had "Gdi32.DeleteObject(hBitmap)" INSIDE this try block.
                            // If the _captured event is synchronous and consumes the bitmap immediately (e.g. draws it to UI), 
                            // we delete it here.
                            Gdi32.DeleteObject(hBitmap);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Assuming Log is available
                // Log.Error("Error in frame callback, continuing.", ex);
                Console.WriteLine(ex);
            }
            finally
            {
                this._waiter.Release();
            }
        }

        /// <summary>
        /// Creates a GDI HBITMAP from an OpenCvSharp Mat without using System.Drawing.
        /// </summary>
        private IntPtr MatToHBitmap(Mat mat)
        {
            int width = mat.Width;
            int height = mat.Height;
            int channels = mat.Channels();

            // Prepare BITMAPINFO
            Gdi32.BITMAPINFO bmi = new Gdi32.BITMAPINFO();
            bmi.bmiHeader.biSize = Marshal.SizeOf(typeof(Gdi32.BITMAPINFOHEADER));
            bmi.bmiHeader.biWidth = width;
            // Negative height creates a top-down DIB (matches OpenCV memory layout)
            bmi.bmiHeader.biHeight = -height;
            bmi.bmiHeader.biPlanes = 1;
            bmi.bmiHeader.biBitCount = (short)(channels * 8);
            bmi.bmiHeader.biCompression = 0; // BI_RGB

            // Create DIB Section
            IntPtr bitsPtr;
            IntPtr hBitmap = Gdi32.CreateDIBSection(IntPtr.Zero, in bmi, 0, out bitsPtr, IntPtr.Zero, 0);

            if (hBitmap == IntPtr.Zero)
                throw new Exception("Failed to create DIB section");

            // Copy pixels from Mat to GDI Buffer
            // We must handle stride (Step) differences. 
            // DIB sections are DWORD aligned (multiple of 4 bytes).
            // OpenCV Mats are also usually aligned, but we should be careful.

            long srcStep = mat.Step();
            int rowLength = width * channels; // bytes per row (unpadded)

            // Calculate DIB stride (must be multiple of 4)
            int destStride = (rowLength + 3) & ~3;

            if (srcStep == destStride && mat.IsContinuous())
            {
                // Fast copy: Everything is contiguous and aligned
                unsafe
                {
                    Buffer.MemoryCopy((void*)mat.Data, (void*)bitsPtr, destStride * height, destStride * height);
                }
            }
            else
            {
                // Row-by-row copy
                unsafe
                {
                    byte* srcPtr = (byte*)mat.Data;
                    byte* dstPtr = (byte*)bitsPtr;

                    for (int y = 0; y < height; y++)
                    {
                        Buffer.MemoryCopy(srcPtr, dstPtr, destStride, rowLength);
                        srcPtr += srcStep;
                        dstPtr += destStride;
                    }
                }
            }

            return hBitmap;
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
                    Task.Run(new Action(this.Close));
                }
                else
                {
                    if (!StCam.StopTransfer(this._cameraCapture))
                    {
                        this.LogError(StCam.GetLastError(this._cameraCapture));
                    }
                    StCam.Close(this._cameraCapture);
                    Log.Debug("Camera closed.");
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
            string strErrorMsg;
            int dwFlags = 4096;
            IntPtr ptrlpSource = IntPtr.Zero;
            if (Kernel32.FormatMessage(dwFlags, ptrlpSource, (int)dwErrorCode, 0, out strErrorMsg, 1024, IntPtr.Zero) == 0)
            {
                ptrlpSource = Kernel32.LoadLibraryEx("StCamMsg.dll", IntPtr.Zero, 1);
                dwFlags |= 2048;
                if (!ptrlpSource.Equals(IntPtr.Zero))
                {
                    Kernel32.FormatMessage(dwFlags, ptrlpSource, (int)dwErrorCode, 0, out strErrorMsg, 1024, IntPtr.Zero);
                    Kernel32.FreeLibrary(ptrlpSource);
                }
            }
            Log.Error(strErrorMsg.ToString());
            throw new Exception(strErrorMsg.ToString());
        }

        // Token: 0x06000082 RID: 130 RVA: 0x00003F2F File Offset: 0x0000212F
        public void Dispose()
        {
            this.Close();
        }

        // Token: 0x04000017 RID: 23
        private readonly Semaphore _waiter;

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
