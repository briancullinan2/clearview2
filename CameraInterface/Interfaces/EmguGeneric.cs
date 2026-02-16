using Emgu.CV;
using Emgu.CV.CvEnum;
using EPIC.CameraInterface.Utilities;

namespace EPIC.CameraInterface.Interfaces
{

    /// <summary>
    /// Controls all other cameras using the Emgu Open CV library.
    /// </summary>
    // Token: 0x0200000B RID: 11
    public class EmguGeneric : ICapturable, IDisposable
    {
        // Token: 0x0600003E RID: 62 RVA: 0x00003248 File Offset: 0x00001448
        internal EmguGeneric()
        {
        }

        // Token: 0x0600003F RID: 63 RVA: 0x00003250 File Offset: 0x00001450
        public void Dispose()
        {
            this.Close();
        }

        // Token: 0x06000040 RID: 64 RVA: 0x00003258 File Offset: 0x00001458
        public void Open()
        {
            try
            {
                this._capture = new VideoCapture();
                this.Height = 240;
                this.Width = 320;
                this._capture.ImageGrabbed += this.ProcessFrame;
                this._capture.Start();
                var success = this._capture.Grab();
            }
            catch (Exception ex)
            {
                Log.Error("There was an error starting OpenCV.", ex);
            }
        }

        public void Capture()
        {
            try
            {
                var success = this._capture.Grab();
            }
            catch (Exception ex)
            {
                Log.Debug("An error occurred while capturing the frame.", ex);
            }
        }


        private void ProcessFrame(object? sender, EventArgs e)
        {
            Mat frame = new Mat();
            _capture.Retrieve(frame, 0);
            Captured(frame.ToBitmap().GetHbitmap());
        }

        // Token: 0x14000003 RID: 3
        // (add) Token: 0x06000042 RID: 66 RVA: 0x00003364 File Offset: 0x00001564
        // (remove) Token: 0x06000043 RID: 67 RVA: 0x0000339C File Offset: 0x0000159C
        public event FrameCallback Captured;

        // Token: 0x06000044 RID: 68 RVA: 0x000033D1 File Offset: 0x000015D1
        public bool Is(string hardwareName)
        {
            return hardwareName != null;
        }

        // Token: 0x06000045 RID: 69 RVA: 0x000033DA File Offset: 0x000015DA
        public void Close()
        {
            if (this._capture != null)
            {
                this._capture.ImageGrabbed -= this.ProcessFrame;
                this._capture.Stop();
                this._capture.Dispose();
            }
        }

        // Token: 0x1700000D RID: 13
        // (get) Token: 0x06000046 RID: 70 RVA: 0x00003411 File Offset: 0x00001611
        // (set) Token: 0x06000047 RID: 71 RVA: 0x0000341F File Offset: 0x0000161F
        public double FramesPerSecond
        {
            get
            {
                return this._capture.Get(CapProp.Fps);
            }
            set
            {
                this._capture.Set(CapProp.Fps, value);
            }
        }

        // Token: 0x1700000E RID: 14
        // (get) Token: 0x06000048 RID: 72 RVA: 0x0000342E File Offset: 0x0000162E
        // (set) Token: 0x06000049 RID: 73 RVA: 0x00003443 File Offset: 0x00001643
        public int Brightness
        {
            get
            {
                return (int)Math.Round(this._capture.Get(CapProp.Brightness));
            }
            set
            {
                this._capture.Set(CapProp.Brightness, (double)value);
            }
        }

        // Token: 0x1700000F RID: 15
        // (get) Token: 0x0600004A RID: 74 RVA: 0x00003454 File Offset: 0x00001654
        // (set) Token: 0x0600004B RID: 75 RVA: 0x00003469 File Offset: 0x00001669
        public int Gain
        {
            get
            {
                return (int)Math.Round(this._capture.Get(CapProp.Gain));
            }
            set
            {
                this._capture.Set(CapProp.Gain, (double)value);
            }
        }

        // Token: 0x17000010 RID: 16
        // (get) Token: 0x0600004C RID: 76 RVA: 0x0000347A File Offset: 0x0000167A
        // (set) Token: 0x0600004D RID: 77 RVA: 0x0000348F File Offset: 0x0000168F
        public int Exposure
        {
            get
            {
                return (int)Math.Round(this._capture.Get(CapProp.Exposure));
            }
            set
            {
                this._capture.Set(CapProp.Exposure, (double)value);
            }
        }

        // Token: 0x17000011 RID: 17
        // (get) Token: 0x0600004E RID: 78 RVA: 0x000034A0 File Offset: 0x000016A0
        // (set) Token: 0x0600004F RID: 79 RVA: 0x000034AF File Offset: 0x000016AF
        public int Height
        {
            get
            {
                return (int)this._capture.Get(CapProp.FrameHeight);
            }
            set
            {
                this._capture.Set(CapProp.FrameHeight, (double)value);
            }
        }

        // Token: 0x17000012 RID: 18
        // (get) Token: 0x06000050 RID: 80 RVA: 0x000034BF File Offset: 0x000016BF
        // (set) Token: 0x06000051 RID: 81 RVA: 0x000034CE File Offset: 0x000016CE
        public int Width
        {
            get
            {
                return (int)this._capture.Get(CapProp.FrameWidth);
            }
            set
            {
                this._capture.Set(CapProp.FrameWidth, (double)value);
            }
        }

        // Token: 0x17000013 RID: 19
        // (get) Token: 0x06000052 RID: 82 RVA: 0x000034DE File Offset: 0x000016DE
        // (set) Token: 0x06000053 RID: 83 RVA: 0x000034E6 File Offset: 0x000016E6
        public string DisplayName { get; set; }

        // Token: 0x17000014 RID: 20
        // (get) Token: 0x06000054 RID: 84 RVA: 0x000034EF File Offset: 0x000016EF
        // (set) Token: 0x06000055 RID: 85 RVA: 0x000034F7 File Offset: 0x000016F7
        public object UniqueIdentifier { get; set; }

        // Token: 0x04000011 RID: 17
        private VideoCapture _capture;
    }
}
