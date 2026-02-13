using EPIC.ClearView.Controls;
using EPIC.ClearView.Utilities;
using Microsoft.Win32;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Web;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Xceed.Wpf.Toolkit;

namespace EPIC.ClearView.Capture
{
    // Token: 0x02000005 RID: 5
    public partial class Scan : Page
    {
        // Token: 0x1700001C RID: 28
        // (get) Token: 0x0600004B RID: 75 RVA: 0x00003CAC File Offset: 0x00001EAC
        // (set) Token: 0x0600004C RID: 76 RVA: 0x00003CCE File Offset: 0x00001ECE
        private DataLayer.Entities.Patient Patient
        {
            get
            {
                return (DataLayer.Entities.Patient)base.GetValue(Scan.PatientProperty);
            }
            set
            {
                base.SetValue(Scan.PatientProperty, value);
            }
        }

        // Token: 0x1700001D RID: 29
        // (get) Token: 0x0600004D RID: 77 RVA: 0x00003CE0 File Offset: 0x00001EE0
        // (set) Token: 0x0600004E RID: 78 RVA: 0x00003D02 File Offset: 0x00001F02
        public FingerImage Selected
        {
            get
            {
                return (FingerImage)base.GetValue(Scan.SelectedProperty);
            }
            set
            {
                base.SetValue(Scan.SelectedProperty, value);
            }
        }

        // Token: 0x1700001E RID: 30
        // (get) Token: 0x0600004F RID: 79 RVA: 0x00003D2C File Offset: 0x00001F2C
        public DataLayer.Entities.DeviceSetting Settings
        {
            get
            {
                return ClearViewConfiguration.Current.Device.Settings.FirstOrDefault((DataLayer.Entities.DeviceSetting x) => x.IsDefault) ?? ClearViewConfiguration.Current.Device.Settings.First<DataLayer.Entities.DeviceSetting>();
            }
        }

        // Token: 0x1700001F RID: 31
        // (get) Token: 0x06000050 RID: 80 RVA: 0x00003D8C File Offset: 0x00001F8C
        // (set) Token: 0x06000051 RID: 81 RVA: 0x00003DAE File Offset: 0x00001FAE
        private DataLayer.Entities.FingerSet FingerSet
        {
            get
            {
                return (DataLayer.Entities.FingerSet)base.GetValue(Scan.FingerSetProperty);
            }
            set
            {
                base.SetValue(Scan.FingerSetProperty, value);
            }
        }

        // Token: 0x06000052 RID: 82 RVA: 0x00003DF0 File Offset: 0x00001FF0
        public Scan()
        {
            this.InitializeComponent();
            base.Unloaded += delegate (object sender, RoutedEventArgs args)
            {
                EPIC.ClearView.Macros.General.Connect(new FrameCallback(this.FrameCallback), ClearViewConfiguration.Current.Device.Camera, "Loading cameras...", "Loading devices...", "Reconnecting...");
            };
        }

        // Token: 0x06000053 RID: 83 RVA: 0x00003E29 File Offset: 0x00002029
        private void Capture_Click(object sender, RoutedEventArgs e)
        {
            EPIC.ClearView.Macros.General.Capture(this.Settings, new EPIC.ClearView.Macros.General.CaptureCallback(this.CaptureCallback), true);
            this._isFrozen = true;
        }

        // Token: 0x06000054 RID: 84 RVA: 0x00003E4C File Offset: 0x0000204C
        private void CaptureCallback(EPIC.ClearView.Controls.CaptureResults results)
        {
            if (!base.Dispatcher.CheckAccess())
            {
                base.Dispatcher.Invoke(new Action<EPIC.ClearView.Controls.CaptureResults>(this.CaptureCallback), new object[]
                {
                    results
                });
            }
            else
            {
                this.Capture.Source = results.ClosestImageSource;
                this.Selected.StoreAndProcess(results);
            }
        }

        // Token: 0x06000055 RID: 85 RVA: 0x00003EB0 File Offset: 0x000020B0
        private void FrameCallback(IntPtr bitmap)
        {
            if (!base.Dispatcher.CheckAccess())
            {
                base.Dispatcher.Invoke(new Action<IntPtr>(this.FrameCallback), DispatcherPriority.Send, new object[]
                {
                    bitmap
                });
            }
            else
            {
                if (this._isFrozen && this.Selected.FingerBitmap == null)
                {
                    this.Selected.FingerBitmap = System.Drawing.Image.FromHbitmap(bitmap);
                }
                if (!this._isFrozen)
                {
                    BitmapSource source = Imaging.CreateBitmapSourceFromHBitmap(bitmap, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
                    this.Preview.Source = source;
                }
            }
        }

        // Token: 0x06000056 RID: 86 RVA: 0x00003F6C File Offset: 0x0000216C
        private void Scan_Loaded(object sender, RoutedEventArgs e)
        {
            General.Connect(new FrameCallback(this.FrameCallback), ClearViewConfiguration.Current.Device.Camera, "Loading cameras...", "Loading devices...", "Reconnecting...");
            if (base.NavigationService != null)
            {
                int num = base.NavigationService.CurrentSource.OriginalString.IndexOf("?", StringComparison.InvariantCultureIgnoreCase);
                int patientId;
                if (num >= 0 && int.TryParse(HttpUtility.ParseQueryString(base.NavigationService.CurrentSource.OriginalString.Substring(num))["patientId"], out patientId))
                {
                    this.Patient = new LinqMetaData().Patient.FirstOrDefault((PatientEntity x) => x.PatientId == patientId);
                }
                int fingerSetId;
                if (num >= 0 && int.TryParse(HttpUtility.ParseQueryString(base.NavigationService.CurrentSource.OriginalString.Substring(num))["fingerSetId"], out fingerSetId))
                {
                    this.FingerSet = new LinqMetaData().FingerSet.FirstOrDefault((DataLayer.Entities.FingerSet x) => x.FingerSetId == (long)fingerSetId);
                    base.Title = "View Scan";
                }
            }
            if (this.FingerSet == null && (ClearViewConfiguration.Current.Calibration == null || ClearViewConfiguration.Current.Calibration.TimeCalibrated < DateTime.UtcNow.Subtract(new TimeSpan(24, 0, 0))))
            {
                MessageBoxResult messageBoxResult = Xceed.Wpf.Toolkit.MessageBox.Show("A calibration has not been performed in the last 24 hours, would you like to perform a calibration now?", "Calibration Error", MessageBoxButton.YesNo);
                if (messageBoxResult == MessageBoxResult.Yes)
                {
                    Navigation.ShowTab(new Uri("/Capture/Calibrate.xaml", UriKind.Relative), true);
                }
            }
            else if (this.FingerSet == null)
            {
                List<FingerImage> list = this.ImagesWOBarrier.Children.OfType<FingerImage>().ToList<FingerImage>();
                List<FingerImage> list2 = this.ImagesWBarrier.Children.OfType<FingerImage>().ToList<FingerImage>();
                for (int i = 0; i < 10; i++)
                {
                    DataLayer.Entities.Calibration calibration = ClearViewConfiguration.Current.Calibration.Calibrations.Skip(i).First<DataLayer.Entities.Calibration>();
                    list[i].Calibration = calibration;
                    list2[i].Calibration = calibration;
                }
            }
            if (this.FingerSet == null)
            {
                this.FingerSet = new DataLayer.Entities.FingerSet();
            }
            this.Select_Image(this.ImagesWOBarrier.Children.OfType<FingerImage>().First<FingerImage>(), null);
        }

        // Token: 0x06000057 RID: 87 RVA: 0x000042C5 File Offset: 0x000024C5
        private void Live_Click(object sender, RoutedEventArgs e)
        {
            this._isFrozen = false;
        }

        // Token: 0x06000058 RID: 88 RVA: 0x00004348 File Offset: 0x00002548
        private void Select_Image(object sender, MouseButtonEventArgs e)
        {
            FingerImage fingerImage = sender as FingerImage;
            if (fingerImage != null)
            {
                if (this.Selected != null)
                {
                    this.Selected.SetValue(Control.BorderBrushProperty, DependencyProperty.UnsetValue);
                }
                this.Selected = fingerImage;
                string finger = (string)this.Selected.Tag;
                this.Selected.BorderBrush = System.Windows.Media.Brushes.Red;
                this.Capture.Source = this.Selected.Image;
                object obj = base.Resources["Hand" + finger];
                if (finger.Contains("L"))
                {
                    this.LeftHand.Source = (BitmapImage)obj;
                    this.RightHand.Source = (BitmapImage)base.Resources["Hand0R"];
                }
                else
                {
                    this.RightHand.Source = (BitmapImage)obj;
                    this.LeftHand.Source = (BitmapImage)base.Resources["Hand0L"];
                }
                DataLayer.Entities.ImageAlignmentDataLayer.Entities.ImageAlignment = this.FingerSet.Images.FirstOrDefault((DataLayer.Entities.ImageAlignment x) => x.Finger == finger && x.Filtered == this.Selected.Filtered);
                if (DataLayer.Entities.ImageAlignment != null)
                {
                    this._isFrozen = true;
                    if (this.Selected.FingerBitmap == null)
                    {
                        this.Selected.FingerBitmap = this.NotAvailable;
                    }
                    this.Preview.Dispatcher.BeginInvoke(new Action(delegate ()
                    {
                        this.Preview.Source = this.Selected.Finger;
                    }), new object[0]);
                }
                else
                {
                    this.Capture.Source = null;
                    this._isFrozen = false;
                }
            }
        }

        // Token: 0x17000020 RID: 32
        // (get) Token: 0x06000059 RID: 89 RVA: 0x00004530 File Offset: 0x00002730
        private Bitmap NotAvailable
        {
            get
            {
                if (this._notAvailable == null)
                {
                    this._notAvailable = new Bitmap(320, 240);
                    using (Graphics graphics = Graphics.FromImage(this._notAvailable))
                    {
                        graphics.FillRectangle(System.Drawing.Brushes.DarkGray, 0, 0, this._notAvailable.Width, this._notAvailable.Height);
                        graphics.DrawLine(Pens.White, 0, 0, this._notAvailable.Width, this._notAvailable.Height);
                        graphics.DrawLine(Pens.White, this._notAvailable.Width, 0, 0, this._notAvailable.Height);
                        Font font = new Font((base.FontFamily.FamilyNames.Values != null) ? new System.Drawing.FontFamily(string.Join(",", base.FontFamily.FamilyNames.Values)) : new System.Drawing.FontFamily(GenericFontFamilies.Serif), (float)base.FontSize, System.Drawing.FontStyle.Regular);
                        SizeF sizeF = graphics.MeasureString("Image not available.", font);
                        graphics.DrawString("Image not available", font, System.Drawing.Brushes.White, (float)this._notAvailable.Width / 2f - sizeF.Width / 2f, (float)this._notAvailable.Height / 2f - sizeF.Height / 2f);
                    }
                }
                return this._notAvailable;
            }
        }

        // Token: 0x0600005A RID: 90 RVA: 0x0000475C File Offset: 0x0000295C
        private void Load_Image(object sender, MouseButtonEventArgs e)
        {
            FingerImage clicked = sender as FingerImage;
            if (clicked != null)
            {
                bool flag = this.ImagesWBarrier.Children.Contains(clicked);
                List<FingerImage> unfilteredImages = this.ImagesWOBarrier.Children.OfType<FingerImage>().ToList<FingerImage>();
                List<FingerImage> filteredImages = this.ImagesWBarrier.Children.OfType<FingerImage>().ToList<FingerImage>();
                List<FingerImage> clickedImages = flag ? filteredImages : unfilteredImages;
                int num = clickedImages.IndexOf(clicked);
                OpenFileDialog dialog = new OpenFileDialog
                {
                    Filter = string.Format("Images|{0}*.bmp;*#{1}.bmp|Finger Images|*.bmp", num % 5 + 1 + ((num < 5) ? "R" : "L"), num + 1),
                    FilterIndex = 2,
                    Multiselect = true,
                    RestoreDirectory = true
                };
                if (!(dialog.ShowDialog(Application.Current.MainWindow) != true))
                {
                    Start.Work(delegate ()
                    {
                        this.StoreAndProcessImages(dialog.FileNames, clicked, clickedImages, unfilteredImages, filteredImages);
                    }, ThreadPriority.Normal).OnException(delegate (Exception ex)
                    {
                        Scan.Log.Error("There was an error loading the finger images.", ex);
                        base.Dispatcher.Invoke<MessageBoxResult>(() => Xceed.Wpf.Toolkit.MessageBox.Show("There was an error loading the finger images."));
                    }).RunNow();
                }
            }
        }

        // Token: 0x0600005B RID: 91 RVA: 0x000048D4 File Offset: 0x00002AD4
        private void StoreAndProcessImages(IEnumerable<string> filenames, FingerImage image, List<FingerImage> clickedImages, List<FingerImage> unfilteredImages, List<FingerImage> filteredImages)
        {
            int num = clickedImages.IndexOf(image);
            foreach (string text in filenames)
            {
                string fileName = Path.GetFileName(text);
                if (fileName != null)
                {
                    try
                    {
                        num = ((fileName.Substring(1, 1) == "L") ? 5 : 0) + (int.Parse(fileName.Substring(0, 1)) - 1);
                        if (fileName.Contains("wofilter"))
                        {
                            image = unfilteredImages[num];
                        }
                        else if (fileName.Contains("filter"))
                        {
                            image = filteredImages[num];
                        }
                        else
                        {
                            image = clickedImages[num];
                        }
                    }
                    catch
                    {
                        try
                        {
                            num = int.Parse(fileName.Substring(fileName.IndexOf("#", StringComparison.InvariantCultureIgnoreCase) + 1, 2));
                            if (fileName.Contains("wofilter"))
                            {
                                image = unfilteredImages[num];
                            }
                            else if (fileName.Contains("filter"))
                            {
                                image = filteredImages[num];
                            }
                            else
                            {
                                image = clickedImages[num];
                            }
                        }
                        catch
                        {
                            num++;
                        }
                    }
                }
                Bitmap bitmap = new Bitmap(text);
                byte[] image2 = Compression.CompressImage(bitmap);
                DataLayer.Entities.Image imageEntity = new DataLayer.Entities.Image
                {
                    Image = image2
                };
                imageEntity.Save();
                CaptureResults results = new CaptureResults(new List<Tuple<long, Bitmap, DataLayer.Entities.Image>>
                {
                    new Tuple<long, Bitmap, DataLayer.Entities.Image>(0L, bitmap, imageEntity)
                }, 0, null);
                image.StoreAndProcess(results);
            }
        }

        // Token: 0x0600005C RID: 92 RVA: 0x00004BDC File Offset: 0x00002DDC
        private void FingerImage_Finished(FingerImage sender, DataLayer.Entities.ImageAlignment alignment)
        {
            if (alignment != null)
            {
                DataLayer.Entities.FingerSet fingerSet = null;
                base.Dispatcher.Invoke(delegate ()
                {
                    fingerSet = this.FingerSet;
                });
                lock (fingerSet)
                {
                    try
                    {
                        fingerSet.Calibration = ClearViewConfiguration.Current.Calibration;
                        fingerSet.SoftwareVersion = ClearViewConfiguration.Current.Title;
                        fingerSet.TimeScanned = DateTime.UtcNow;
                        fingerSet.Images.Add(alignment);
                        fingerSet.Save(true);
                    }
                    catch (Exception ex)
                    {
                        Scan.Log.Error("There was an error saving the finger image.", ex);
                        base.Dispatcher.Invoke<MessageBoxResult>(() => Xceed.Wpf.Toolkit.MessageBox.Show("There was an error saving the finger image.", "Scan Results", MessageBoxButton.OK));
                    }
                }
                base.Dispatcher.Invoke(delegate ()
                {
                    bool flag2;
                    if (this.ImagesWOBarrier.Children.OfType<FingerImage>().All((FingerImage x) => x.Result != null))
                    {
                        flag2 = !this.ImagesWBarrier.Children.OfType<FingerImage>().All((FingerImage x) => x.Result != null);
                    }
                    else
                    {
                        flag2 = true;
                    }
                    if (!flag2)
                    {
                    }
                });
            }
        }

        // Token: 0x0600005D RID: 93 RVA: 0x00004D38 File Offset: 0x00002F38
        private void ViewReport_Click(object sender, RoutedEventArgs e)
        {
            List<FingerImage> list = this.ImagesWOBarrier.Children.OfType<FingerImage>().ToList<FingerImage>();
            List<FingerImage> list2 = this.ImagesWBarrier.Children.OfType<FingerImage>().ToList<FingerImage>();
            list.ForEach(delegate (FingerImage x)
            {
                x.ProcessSectors();
            });
            list2.ForEach(delegate (FingerImage x)
            {
                x.ProcessSectors();
            });
        }

        // Token: 0x0600005E RID: 94 RVA: 0x00004DC0 File Offset: 0x00002FC0
        private void Move_Click(object sender, RoutedEventArgs e)
        {
            string text = (string)((FrameworkElement)sender).Tag;
            string text2 = text;
            if (text2 != null)
            {
                if (!(text2 == "CounterClockwise"))
                {
                    if (!(text2 == "Clockwise"))
                    {
                        if (!(text2 == "Left"))
                        {
                            if (!(text2 == "Right"))
                            {
                                if (!(text2 == "Up"))
                                {
                                    if (text2 == "Down")
                                    {
                                        this.Y.Value = (this.Y.Value + 1) % (int)this.Selected.Image.Height;
                                    }
                                }
                                else
                                {
                                    this.Y.Value = (this.Y.Value - 1) % (int)this.Selected.Image.Height;
                                }
                            }
                            else
                            {
                                this.X.Value = (this.X.Value + 1) % (int)this.Selected.Image.Width;
                            }
                        }
                        else
                        {
                            this.X.Value = (this.X.Value - 1) % (int)this.Selected.Image.Width;
                        }
                    }
                    else
                    {
                        this.Angle.Value = (this.Angle.Value + 1) % 360;
                    }
                }
                else
                {
                    this.Angle.Value = (this.Angle.Value - 1) % 360;
                }
            }
        }

        // Token: 0x04000025 RID: 37
        private bool _isFrozen;

        // Token: 0x04000026 RID: 38
        private Bitmap _notAvailable;

        // Token: 0x04000027 RID: 39
        public static readonly DependencyProperty PatientProperty = DependencyProperty.Register("Patient", typeof(DataLayer.Entities.Patient), typeof(Scan), new PropertyMetadata(new DataLayer.Entities.Patient()));

        // Token: 0x04000028 RID: 40
        public static readonly DependencyProperty SelectedProperty = DependencyProperty.Register("Selected", typeof(FingerImage), typeof(Scan), new PropertyMetadata(null));

        // Token: 0x04000029 RID: 41
        public static readonly DependencyProperty FingerSetProperty = DependencyProperty.Register("FingerSet", typeof(DataLayer.Entities.FingerSet), typeof(Scan), new PropertyMetadata(null));
    }
}
