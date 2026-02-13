using EPIC.DataLayer.Extensions;
using EPIC.MedicalControls.Native;
using EPIC.MedicalControls.Utilities;
using System.ComponentModel;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media.Imaging;

namespace EPIC.MedicalControls.Controls
{
    // Token: 0x0200000E RID: 14
    public class FingerImage : ProcessingImage
    {
        // Token: 0x1700003E RID: 62
        // (get) Token: 0x060000D9 RID: 217 RVA: 0x00007EEC File Offset: 0x000060EC
        // (set) Token: 0x060000DA RID: 218 RVA: 0x00007F0E File Offset: 0x0000610E
        public BitmapSource Finger
        {
            get
            {
                return (BitmapSource)base.GetValue(FingerImage.FingerProperty);
            }
            private set
            {
                base.SetValue(FingerImage.FingerProperty, value);
            }
        }

        // Token: 0x1700003F RID: 63
        // (get) Token: 0x060000DB RID: 219 RVA: 0x00007F20 File Offset: 0x00006120
        // (set) Token: 0x060000DC RID: 220 RVA: 0x00007F42 File Offset: 0x00006142
        public BitmapSource Orientation
        {
            get
            {
                return (BitmapSource)base.GetValue(FingerImage.OrientationProperty);
            }
            private set
            {
                base.SetValue(FingerImage.OrientationProperty, value);
            }
        }


        // Token: 0x17000040 RID: 64
        // (get) Token: 0x060000DD RID: 221 RVA: 0x00007F54 File Offset: 0x00006154
        // (set) Token: 0x060000DE RID: 222 RVA: 0x00007F6C File Offset: 0x0000616C
        public Bitmap FingerBitmap
        {
            get
            {
                return this._fingerImage;
            }
            set
            {
                this._fingerImage = value;
                IntPtr hbitmap = value.GetHbitmap();
                this.Finger = Imaging.CreateBitmapSourceFromHBitmap(hbitmap, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
                Gdi32.DeleteObject(hbitmap);
            }
        }

        // Token: 0x17000041 RID: 65
        // (get) Token: 0x060000DF RID: 223 RVA: 0x00007FAC File Offset: 0x000061AC
        // (set) Token: 0x060000E0 RID: 224 RVA: 0x00007FC3 File Offset: 0x000061C3
        public DataLayer.Entities.Calibration Calibration { get; set; }

        // Token: 0x17000042 RID: 66
        // (get) Token: 0x060000E1 RID: 225 RVA: 0x00007FCC File Offset: 0x000061CC
        // (set) Token: 0x060000E2 RID: 226 RVA: 0x00008044 File Offset: 0x00006244
        public DataLayer.Entities.ImageAlignment Result
        {
            get
            {
                return (DataLayer.Entities.ImageAlignment)base.GetValue(FingerImage.ResultProperty);
            }
            private set
            {
                base.SetValue(FingerImage.ResultProperty, value);
                this.Result.PropertyChanged += delegate (object sender, PropertyChangedEventArgs args)
                {
                    if (args.PropertyName == "CenterX" || args.PropertyName == "CenterY" || args.PropertyName == "Angle")
                    {
                        this.Reorient();
                    }
                };
                this.Reorient();
            }
        }

        // Token: 0x17000043 RID: 67
        // (get) Token: 0x060000E3 RID: 227 RVA: 0x00008074 File Offset: 0x00006274
        // (set) Token: 0x060000E4 RID: 228 RVA: 0x00008096 File Offset: 0x00006296
        public bool Filtered
        {
            get
            {
                return (bool)base.GetValue(FingerImage.FilteredProperty);
            }
            set
            {
                base.SetValue(FingerImage.FilteredProperty, value);
            }
        }

        // Token: 0x14000004 RID: 4
        // (add) Token: 0x060000E5 RID: 229 RVA: 0x000080AC File Offset: 0x000062AC
        // (remove) Token: 0x060000E6 RID: 230 RVA: 0x000080E8 File Offset: 0x000062E8
        private event FingerImage.ProcessingFinishedHandler _finished;

        // Token: 0x14000005 RID: 5
        // (add) Token: 0x060000E7 RID: 231 RVA: 0x00008124 File Offset: 0x00006324
        // (remove) Token: 0x060000E8 RID: 232 RVA: 0x00008166 File Offset: 0x00006366
        public event FingerImage.ProcessingFinishedHandler Finished
        {
            add
            {
                this._finished += value;
                if (this.Result != null && this._finished != null)
                {
                    this._finished(this, this.Result);
                }
            }
            remove
            {
                this._finished -= value;
            }
        }

        // Token: 0x060000E9 RID: 233 RVA: 0x00008171 File Offset: 0x00006371
        public FingerImage()
        {
            base.InitializeComponent();
            ToolTipService.SetShowDuration(this, 60000);
        }

        // Token: 0x060000EA RID: 234 RVA: 0x0000818F File Offset: 0x0000638F
        public FingerImage(CaptureResults results) : this()
        {
            this.StoreAndProcess(results);
        }

        // Token: 0x060000EB RID: 235 RVA: 0x000081A4 File Offset: 0x000063A4
        private void Reorient()
        {
            using (Bitmap bitmap = new Bitmap(320, 240))
            {
                using (Graphics graphics = Graphics.FromImage(bitmap))
                {
                    graphics.TranslateTransform((float)((int)this.Result.CenterX), (float)((int)this.Result.CenterY));
                    graphics.RotateTransform((float)this.Result.Angle);
                    int num = Convert.ToInt32(this.Result.RadiusX - 10.0);
                    int num2 = Convert.ToInt32(this.Result.RadiusY - 10.0);
                    graphics.DrawEllipse(Pens.Yellow, -num, -num2, num * 2, num2 * 2);
                    graphics.DrawLine(Pens.Red, 0, -num2, 0, num2 + 50);
                    graphics.DrawLine(Pens.Red, -num, 0, num, 0);
                    graphics.RotateTransform(-(float)this.Result.Angle);
                }
                IntPtr hbitmap = bitmap.GetHbitmap();
                this.Orientation = Imaging.CreateBitmapSourceFromHBitmap(hbitmap, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
                Gdi32.DeleteObject(hbitmap);
            }
        }

        // Token: 0x060000EC RID: 236 RVA: 0x00008344 File Offset: 0x00006544
        public sealed override void StoreAndProcess(CaptureResults results)
        {
            if (!base.Dispatcher.CheckAccess())
            {
                base.Dispatcher.Invoke(new Action<CaptureResults>(this.StoreAndProcess), new object[]
                {
                    results
                });
            }
            else
            {
                base.Image = results.ClosestImageSource;
                Task.Run(() => this.StartProcessing(results))
                    .ContinueWith(t => Log.Error("There was an error while processing the finger results.", t.Exception?.InnerException ?? t.Exception), TaskContinuationOptions.OnlyOnFaulted);
            }
        }

        // Token: 0x060000ED RID: 237 RVA: 0x000084F4 File Offset: 0x000066F4
        private void StartProcessing(CaptureResults results)
        {
            try
            {
                string finger = string.Empty;
                bool filtered = false;
                base.Dispatcher.Invoke(delegate ()
                {
                    this.State = States.Processing;
                    this.Value = 0.0;
                    finger = (string)this.Tag;
                    filtered = this.Filtered;
                });
                int num = 0;
                foreach (Tuple<long, Bitmap, DataLayer.Entities.Image> tuple in results.Tuples)
                {
                    base.Dispatcher.Invoke<States>(() => base.State = States.Paused);
                    base.Dispatcher.Invoke<States>(() => base.State = States.Processing);
                    DataLayer.Entities.ImageAlignment alignment;
                    if (tuple.Item3.ImageAlignment == null)
                    {
                        alignment = Maths.GetAlignment(tuple.Item2, this._fingerImage);
                        if (this._fingerImage != null)
                        {
                            alignment.FingerImage = new DataLayer.Entities.Image
                            {
                                ImageData = Compression.CompressImage(this._fingerImage)
                            }
                            ;
                        }
                        alignment.Filtered = new bool?(filtered);
                        alignment.Finger = finger;
                        alignment.ImageId = tuple.Item3.ImageId;
                        alignment.Save(true);
                    }
                    else
                    {
                        alignment = tuple.Item3.ImageAlignment;
                    }
                    double percent = Math.Round((double)num / (double)results.Images.Count * 100.0);
                    base.Dispatcher.Invoke(new Action<bool>(delegate (bool b)
                    {
                        this.Value = percent;
                        if (b)
                        {
                            this.Result = alignment;
                        }
                    }), new object[]
                    {
                        num == results.ClosestIndex && alignment != null
                    });
                    num++;
                    if (num == results.Tuples.Count)
                    {
                        base.Dispatcher.Invoke<States>(() => base.State = States.Complete);
                    }
                    if (this._finished != null)
                    {
                        this._finished(this, alignment);
                    }
                }
            }
            finally
            {
                results.Dispose();
            }
        }

        // Token: 0x060000EE RID: 238 RVA: 0x000087E3 File Offset: 0x000069E3
        public void ProcessSectors()
        {
            Task.Run(new Action(this.StartProcessSectors)).ContinueWith(t =>
            {
                Log.Error("There was an error while processing the finger results.", t.Exception?.InnerException ?? t.Exception);
            });
        }

        // Token: 0x060000EF RID: 239 RVA: 0x000088A0 File Offset: 0x00006AA0
        private void StartProcessSectors()
        {
            string finger = string.Empty;
            DataLayer.Entities.ImageAlignment alignment = null;
            base.Dispatcher.Invoke(delegate ()
            {
                this.State = States.Processing;
                this.Value = 0.0;
                finger = (string)this.Tag;
                alignment = this.Result;
            });
            if (alignment != null)
            {
                IEnumerable<DataLayer.Entities.ImageSector> enumerable = null;
                if (alignment.Image.ImageSectors.Count() == 0 && this.Calibration != null)
                {
                    enumerable = Maths.GetImageSectors(Compression.DecompressImage(alignment.Image.ImageData), Compression.DecompressImage(this.Calibration.Image.ImageData), alignment.Angle, alignment.CenterX, alignment.CenterY, this.Calibration.NoiseLevel, finger);
                }
                if (enumerable != null)
                {
                    foreach (DataLayer.Entities.ImageSector sector in enumerable)
                    {
                        sector.Image = alignment.Image;
                        sector.Save();
                    }
                    alignment.Image.Refetch();
                }
            }
            base.Dispatcher.Invoke<States>(() => base.State = States.Complete);
            if (this._finished != null)
            {
                this._finished(this, alignment);
            }
        }

        // Token: 0x04000084 RID: 132
        private const int ELLIPSE_FIT_ADJUSTMENT = 10;

        // Token: 0x04000086 RID: 134
        private Bitmap _fingerImage;

        // Token: 0x04000087 RID: 135
        public static readonly DependencyProperty FingerProperty = DependencyProperty.Register("Finger", typeof(BitmapSource), typeof(FingerImage), new PropertyMetadata(null));

        // Token: 0x04000088 RID: 136
        public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register("Orientation", typeof(BitmapSource), typeof(FingerImage), new PropertyMetadata(null));

        // Token: 0x04000089 RID: 137
        public static readonly DependencyProperty ResultProperty = DependencyProperty.Register("Result", typeof(DataLayer.Entities.ImageAlignment), typeof(FingerImage), new PropertyMetadata(null));

        // Token: 0x0400008A RID: 138
        public static readonly DependencyProperty FilteredProperty = DependencyProperty.Register("Filtered", typeof(bool), typeof(FingerImage), new PropertyMetadata(false));

        // Token: 0x0200000F RID: 15
        // (Invoke) Token: 0x060000F9 RID: 249
        public delegate void ProcessingFinishedHandler(FingerImage sender, DataLayer.Entities.ImageAlignment alignment);
    }

}



