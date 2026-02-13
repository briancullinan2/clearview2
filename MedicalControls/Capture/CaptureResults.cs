using EPIC.DataLayer.Entities;
using EPIC.MedicalControls.Native;
using System.Drawing;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;

namespace EPIC.MedicalControls.Controls
{
    // Token: 0x0200000D RID: 13
    public class CaptureResults : IDisposable
    {
        // Token: 0x060000CD RID: 205 RVA: 0x00007BC8 File Offset: 0x00005DC8
        public CaptureResults(DataLayer.Entities.DeviceCalibrationSetting settings, List<Tuple<long, Bitmap, DataLayer.Entities.Image>> images, int closestIndex, DataLayer.Entities.Capture capture)
        {
            this._settings = settings;
            this._images = images;
            this._closest = closestIndex;
            this._capture = capture;
            this._closestHBitmap = this._images[this._closest].Item2.GetHbitmap();
        }

        // Token: 0x17000035 RID: 53
        // (get) Token: 0x060000CE RID: 206 RVA: 0x00007C14 File Offset: 0x00005E14
        public BitmapSource ClosestImageSource
        {
            get
            {
                BitmapSource result;
                if ((result = this._closestSource) == null)
                {
                    result = (this._closestSource = Imaging.CreateBitmapSourceFromHBitmap(this._closestHBitmap, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions()));
                }
                return result;
            }
        }

        // Token: 0x17000036 RID: 54
        // (get) Token: 0x060000CF RID: 207 RVA: 0x00007C6C File Offset: 0x00005E6C
        public List<Bitmap> Images
        {
            get
            {
                return (from x in this._images
                        select x.Item2).ToList<Bitmap>();
            }
        }

        // Token: 0x17000037 RID: 55
        // (get) Token: 0x060000D0 RID: 208 RVA: 0x00007CB0 File Offset: 0x00005EB0
        public List<Tuple<long, Bitmap, DataLayer.Entities.Image>> Tuples
        {
            get
            {
                return this._images;
            }
        }

        public DataLayer.Entities.DeviceCalibrationSetting DeviceSettings
        {
            get
            {
                return this._settings;
            }
        }

        // Token: 0x17000038 RID: 56
        // (get) Token: 0x060000D1 RID: 209 RVA: 0x00007CC8 File Offset: 0x00005EC8
        public int ClosestIndex
        {
            get
            {
                return this._closest;
            }
        }

        // Token: 0x17000039 RID: 57
        // (get) Token: 0x060000D2 RID: 210 RVA: 0x00007CE0 File Offset: 0x00005EE0
        public DataLayer.Entities.Capture? Capture
        {
            get
            {
                Tuple<long, Bitmap, DataLayer.Entities.Image>? tuple = this._images.FirstOrDefault<Tuple<long, Bitmap, DataLayer.Entities.Image>>();
                if (tuple == null)
                {
                    throw new NotSupportedException();
                }
                DataLayer.Entities.ImageCapture capture = tuple.Item3.Capture;
                DataLayer.Entities.Capture? result;
                if (capture == null)
                {
                    result = null;
                }
                else
                {
                    result = capture.Capture;
                }
                return result;
            }
        }


        // Token: 0x060000D3 RID: 211 RVA: 0x00007D30 File Offset: 0x00005F30
        public void Dispose()
        {
            Gdi32.DeleteObject(this._closestHBitmap);
            foreach (Tuple<long, Bitmap, DataLayer.Entities.Image> tuple in this._images)
            {
                tuple.Item2.Dispose();
            }
        }

        // Token: 0x1700003A RID: 58
        public Tuple<long, Bitmap, DataLayer.Entities.Image>? this[Bitmap image]
        {
            get
            {
                return this._images.FirstOrDefault((Tuple<long, Bitmap, DataLayer.Entities.Image> x) => x.Item2.Equals(image));
            }
        }

        // Token: 0x1700003B RID: 59
        public Tuple<long, Bitmap, DataLayer.Entities.Image>? this[long image]
        {
            get
            {
                return this._images.FirstOrDefault((Tuple<long, Bitmap, DataLayer.Entities.Image> x) => x.Item1.Equals(image));
            }
        }

        // Token: 0x1700003C RID: 60
        public Tuple<long, Bitmap, DataLayer.Entities.Image> this[int image]
        {
            get
            {
                return this._images[image];
            }
        }

        // Token: 0x1700003D RID: 61
        public Tuple<long, Bitmap, DataLayer.Entities.Image>? this[DataLayer.Entities.Image image]
        {
            get
            {
                return this._images.FirstOrDefault((Tuple<long, Bitmap, DataLayer.Entities.Image> x) => x.Item3.Equals(image));
            }
        }

        private DeviceCalibrationSetting _settings;

        // Token: 0x0400007E RID: 126
        private readonly List<Tuple<long, Bitmap, DataLayer.Entities.Image>> _images;

        // Token: 0x0400007F RID: 127
        private readonly int _closest;

        // Token: 0x04000080 RID: 128
        private readonly DataLayer.Entities.Capture _capture;

        // Token: 0x04000081 RID: 129
        private BitmapSource _closestSource;

        // Token: 0x04000082 RID: 130
        private readonly IntPtr _closestHBitmap;
    }
}
