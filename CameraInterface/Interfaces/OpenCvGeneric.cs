using EPIC.CameraInterface.Native;
using EPIC.CameraInterface.Utilities;
using OpenCvSharp;
using System.Runtime.InteropServices;

namespace EPIC.CameraInterface.Interfaces
{
    /// <summary>
    /// Controls all other cameras using the OpenCvSharp library.
    /// </summary>
    public class OpenCvGeneric : ICapturable, IDisposable
    {
        private VideoCapture _capture;
        private CancellationTokenSource _cts;
        private Task _captureTask;
        public int DeviceIndex { get; set; } = 0;

        internal OpenCvGeneric()
        {
        }

        public void Dispose()
        {
            this.Close();
        }

        public void Open()
        {
            try
            {
                // Open default camera (Index 0)
                this._capture = new VideoCapture(0);

                if (!this._capture.IsOpened())
                {
                    Log.Error("Failed to open OpenCV camera device.");
                    return;
                }

                // Set Default properties
                this.Height = 240;
                this.Width = 320;

                // OpenCvSharp does not have an internal event loop like Emgu.
                // We must start a background task to poll frames continuously.
                this._cts = new CancellationTokenSource();
                this._captureTask = Task.Run(() => CaptureLoop(this._cts.Token), this._cts.Token);
            }
            catch (Exception ex)
            {
                Log.Error("There was an error starting OpenCV.", ex);
            }
        }

        private void CaptureLoop(CancellationToken token)
        {
            // Reuse Mat to avoid memory allocation pressure
            using (Mat frame = new Mat())
            {
                while (!token.IsCancellationRequested && this._capture != null && !this._capture.IsDisposed)
                {
                    try
                    {
                        // 1. Grab (Hardware Sync)
                        if (this._capture.Grab())
                        {
                            // 2. Decode/Retrieve
                            // Note: We use Retrieve inside the loop to simulate Emgu's behavior
                            if (this._capture.Retrieve(frame))
                            {
                                if (!frame.Empty())
                                {
                                    ProcessFrame(frame);
                                }
                            }
                        }
                        else
                        {
                            // Prevent CPU spinning if camera is not ready
                            Thread.Sleep(10);
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.Debug("Error in capture loop.", ex);
                    }
                }
            }
        }

        public void Capture()
        {
            // Emgu's Capture() called Grab(). 
            // Since we are running a continuous CaptureLoop, we don't need to manually grab.
            // Calling Grab() here might conflict with the background thread.
            // Leaving empty or logging is safest.
        }

        private void ProcessFrame(Mat frame)
        {
            if (this.Captured != null)
            {
                try
                {
                    IntPtr hBitmap = MatToHBitmap(frame);

                    // Fire event
                    this.Captured(hBitmap);

                    // TODO: i think i traced all the paths 1 level up from captured so it could be queued for a few seconds for alternating threads to frame it.
                    //Gdi32.DeleteObject(hBitmap);
                }
                catch (Exception ex)
                {
                    Log.Error("Error processing frame conversion.", ex);
                }
            }
        }
        /// <summary>
        /// Creates a GDI HBITMAP directly from an OpenCvSharp Mat.
        /// This replaces "bitmap.GetHbitmap()".
        /// </summary>
        private IntPtr MatToHBitmap(Mat mat)
        {
            int width = mat.Width;
            int height = mat.Height;

            // 1. Prepare GDI BITMAPINFO
            // We use a negative height to tell GDI this is a "Top-Down" image, 
            // which matches OpenCV's memory layout.
            Gdi32.BITMAPINFO bmi = new Gdi32.BITMAPINFO();
            bmi.bmiHeader.biSize = Marshal.SizeOf(typeof(Gdi32.BITMAPINFOHEADER));
            bmi.bmiHeader.biWidth = width;
            bmi.bmiHeader.biHeight = -height;
            bmi.bmiHeader.biPlanes = 1;
            bmi.bmiHeader.biBitCount = 24; // standard OpenCV BGR is 24-bit
            bmi.bmiHeader.biCompression = 0; // BI_RGB

            // 2. Allocate DIB Section (GDI Memory)
            IntPtr pBits;
            IntPtr hBitmap = Gdi32.CreateDIBSection(IntPtr.Zero, ref bmi, 0, out pBits, IntPtr.Zero, 0);

            if (hBitmap == IntPtr.Zero)
            {
                throw new Exception("GDI CreateDIBSection failed.");
            }

            // 3. Copy Pixels (Mat.Data -> DIB Pointer)
            // We must handle stride (padding) differences.
            // GDI DIBs are aligned to 4 bytes (DWORD).
            unsafe
            {
                long srcStep = mat.Step();
                byte* srcPtr = (byte*)mat.Data;
                byte* dstPtr = (byte*)pBits;

                // Calculate the GDI stride (width * 3 bytes, rounded up to nearest 4)
                int dstStride = ((width * 3) + 3) & ~3;
                int rowBytes = width * 3;

                // If strides match, we can copy the whole block at once
                if (srcStep == dstStride && mat.IsContinuous())
                {
                    Buffer.MemoryCopy(srcPtr, dstPtr, dstStride * height, dstStride * height);
                }
                else
                {
                    // Otherwise, copy row by row
                    for (int y = 0; y < height; y++)
                    {
                        Buffer.MemoryCopy(srcPtr, dstPtr, dstStride, rowBytes);
                        srcPtr += srcStep;
                        dstPtr += dstStride;
                    }
                }
            }

            return hBitmap;
        }
        public event FrameCallback Captured;

        public bool Is(string hardwareName)
        {
            return hardwareName != null;
        }

        public void Close()
        {
            // 1. Stop the loop
            if (this._cts != null)
            {
                this._cts.Cancel();
                // Optional: Wait for task to finish (avoiding deadlocks on UI thread)
                // try { this._captureTask.Wait(500); } catch { }
                this._cts.Dispose();
                this._cts = null;
            }

            // 2. Release Camera
            if (this._capture != null)
            {
                this._capture.Release();
                this._capture.Dispose();
                this._capture = null;
            }
        }

        // Mapped CapProp -> VideoCaptureProperties
        public double FramesPerSecond
        {
            get => this._capture?.Get(VideoCaptureProperties.Fps) ?? 0;
            set => this._capture?.Set(VideoCaptureProperties.Fps, value);
        }

        public int Brightness
        {
            get => (int)Math.Round(this._capture?.Get(VideoCaptureProperties.Brightness) ?? 0);
            set => this._capture?.Set(VideoCaptureProperties.Brightness, (double)value);
        }

        public int Gain
        {
            get => (int)Math.Round(this._capture?.Get(VideoCaptureProperties.Gain) ?? 0);
            set => this._capture?.Set(VideoCaptureProperties.Gain, (double)value);
        }

        public int Exposure
        {
            get => (int)Math.Round(this._capture?.Get(VideoCaptureProperties.Exposure) ?? 0);
            set => this._capture?.Set(VideoCaptureProperties.Exposure, (double)value);
        }

        public int Height
        {
            get => (int)(this._capture?.Get(VideoCaptureProperties.FrameHeight) ?? 0);
            set => this._capture?.Set(VideoCaptureProperties.FrameHeight, (double)value);
        }

        public int Width
        {
            get => (int)(this._capture?.Get(VideoCaptureProperties.FrameWidth) ?? 0);
            set => this._capture?.Set(VideoCaptureProperties.FrameWidth, (double)value);
        }

        public string DisplayName { get; set; }

        public object UniqueIdentifier { get; set; }
    }
}