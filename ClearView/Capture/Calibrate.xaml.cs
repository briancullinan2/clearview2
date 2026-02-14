using EPIC.CameraInterface;
using EPIC.ClearView.Macros;
using EPIC.ClearView.Native;
using EPIC.ClearView.Utilities;
using EPIC.DataLayer.Extensions;
using EPIC.MedicalControls.Controls;
using Microsoft.VisualBasic.FileIO;
using Microsoft.Win32;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using System.Windows.Threading;


namespace EPIC.ClearView.Capture
{
    // Token: 0x02000003 RID: 3
    public partial class Calibrate : Page
    {
        // Token: 0x17000002 RID: 2
        // (get) Token: 0x0600000A RID: 10 RVA: 0x0000241C File Offset: 0x0000061C
        // (set) Token: 0x0600000B RID: 11 RVA: 0x0000243E File Offset: 0x0000063E
        public CalibrationImage Selected
        {
            get
            {
                return (CalibrationImage)base.GetValue(Calibrate.SelectedProperty);
            }
            set
            {
                base.SetValue(Calibrate.SelectedProperty, value);
            }
        }

        // Token: 0x0600000C RID: 12 RVA: 0x000024AC File Offset: 0x000006AC
        public Calibrate()
        {
            this.InitializeComponent();
            base.Loaded += delegate (object sender, RoutedEventArgs args)
            {
                General.Connect(new FrameCallback(this.FrameCallback), ClearViewConfiguration.Current?.Device?.Camera, "Loading cameras...", "Loading devices...", "Reconnecting...");
            };
            base.Unloaded += delegate (object sender, RoutedEventArgs args)
            {
                General.Disconnect(new FrameCallback(this.FrameCallback), ClearViewConfiguration.Current?.Device?.Camera);
            };
            base.Dispatcher.ShutdownStarted += delegate (object sender, EventArgs args)
            {
                this._calibration = null;
            };
        }


        // Token: 0x0600000D RID: 13 RVA: 0x00002607 File Offset: 0x00000807
        private void FrameCallback(IntPtr hBitmap)
        {
            this._lastBitmap = System.Drawing.Image.FromHbitmap(hBitmap);
            base.Dispatcher.BeginInvoke(new Action(delegate ()
            {
                if (!this._updating.WaitOne(0))
                {
                    this._droppedFrames = ++this._droppedFrames;
                }
                else
                {
                    if (this._stopwatch.ElapsedMilliseconds > 1000L)
                    {
                        Log.Debug(string.Format("Dropped {0} frames.", this._droppedFrames));
                        this._droppedFrames = 0m;
                        this._stopwatch.Reset();
                    }
                    IntPtr hbitmap = this._lastBitmap.GetHbitmap();
                    BitmapSource source = Imaging.CreateBitmapSourceFromHBitmap(hbitmap, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
                    this.Preview.Source = source;
                    Gdi32.DeleteObject(hbitmap);
                    this._updating.Release();
                }
            }), DispatcherPriority.Send, new object[0]);
        }

        // Token: 0x0600000E RID: 14 RVA: 0x00002638 File Offset: 0x00000838
        private void AutoCalibrate_Checked(object sender, RoutedEventArgs e)
        {
            if (this.Configurations.SelectedItem == null)
            {
                return;
            }
            if (this.AutoCalibrate.IsChecked != null && this.AutoCalibrate.IsChecked.Value)
            {
                this.Reset_Click(sender, e);
                this._calibration = new DataLayer.Entities.Calibration
                {
                    DeviceId = ((DataLayer.Entities.DeviceSetting)this.Configurations.SelectedItem).DeviceId
                }
                ;
                this.FillImages();
            }
        }

        // Token: 0x0600000F RID: 15 RVA: 0x000026B4 File Offset: 0x000008B4
        private void FillImages()
        {
            try
            {
                DataLayer.Entities.DeviceSetting settings = (DataLayer.Entities.DeviceSetting)this.Configurations.SelectedItem;
                EPIC.ClearView.Macros.General.Capture(settings, new EPIC.ClearView.Macros.General.CaptureCallback(this.CaptureCallback), true);
            }
            catch (Exception ex)
            {
                Log.Error("There was an error while taking a calibration image.", ex);
            }
        }

        // Token: 0x06000010 RID: 16 RVA: 0x0000274C File Offset: 0x0000094C
        private void CaptureCallback(CaptureResults results)
        {
            if (!base.Dispatcher.CheckAccess())
            {
                base.Dispatcher.Invoke(new Action<CaptureResults>(this.CaptureCallback), new object[]
                {
                    results
                });
            }
            else
            {
                this.Capture.Source = results.ClosestImageSource;
                bool flag = this.Scroller.ExtentHeight < this.Scroller.ViewportHeight || this.Scroller.VerticalOffset - this.Scroller.ExtentHeight < 10.0;
                CalibrationImage calibrationImage = this.CalibrationImages.Children.OfType<CalibrationImage>().FirstOrDefault((CalibrationImage x) => x.Image == null);
                if (calibrationImage == null)
                {
                    calibrationImage = new CalibrationImage(results)
                    {
                        Margin = new Thickness(5.0),
                        HorizontalAlignment = HorizontalAlignment.Left
                    };
                    calibrationImage.MouseRightButtonUp += this.Load_Image;
                    calibrationImage.Finished += this.Calibration_Finished;
                    this.CalibrationImages.Children.Add(calibrationImage);
                }
                else
                {
                    calibrationImage.StoreAndProcess(results);
                }
                if (flag)
                {
                    this.Scroller.ScrollToRightEnd();
                }
                bool flag2;
                if (this.AutoCalibrate.IsChecked != null && this.AutoCalibrate.IsChecked.Value)
                {
                    if (this.CalibrationImages.Children.OfType<CalibrationImage>().Count<CalibrationImage>() >= 10)
                    {
                        if (!this.CalibrationImages.Children.OfType<CalibrationImage>().Take(10).Any((CalibrationImage x) => x.Image == null))
                        {
                            goto IL_1CE;
                        }
                    }
                    flag2 = false;
                    goto IL_202;
                }
            IL_1CE:
                flag2 = (this.DutyCycle.IsChecked == null || !this.DutyCycle.IsChecked.Value);
            IL_202:
                if (!flag2)
                {
                    this.FillImages();
                }
                else
                {
                    this.AutoCalibrate.IsChecked = new bool?(false);
                }
            }
        }

        // Token: 0x06000011 RID: 17 RVA: 0x0000297C File Offset: 0x00000B7C
        private void StoreAndProcessImages(IEnumerable<string> filenames, CalibrationImage image, List<CalibrationImage> images)
        {
            int num = images.IndexOf(image);
            foreach (string text in filenames)
            {
                string fileName = Path.GetFileName(text);
                if (fileName != null)
                {
                    try
                    {
                        num = ((fileName.Substring(1, 1) == "L") ? 5 : 0) + (int.Parse(fileName.Substring(0, 1)) - 1);
                        image = images[num];
                    }
                    catch
                    {
                        try
                        {
                            num = int.Parse(fileName.Substring(fileName.IndexOf("#", StringComparison.InvariantCultureIgnoreCase) + 1, 2));
                            image = images[num];
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
                    ImageData = image2
                }
                ;
                imageEntity.Save();
                CaptureResults results = new CaptureResults(new List<Tuple<long, Bitmap, DataLayer.Entities.Image>>
                {
                    new Tuple<long, Bitmap, DataLayer.Entities.Image>(0L, bitmap, imageEntity)
                }, 0, null);
                image.StoreAndProcess(results);
            }
        }

        // Token: 0x06000012 RID: 18 RVA: 0x00002B5C File Offset: 0x00000D5C
        private void Load_Image(object sender, MouseButtonEventArgs e)
        {
            CalibrationImage clicked = sender as CalibrationImage;
            if (clicked != null)
            {
                List<CalibrationImage> images = this.CalibrationImages.Children.OfType<CalibrationImage>().ToList<CalibrationImage>();
                int num = images.IndexOf(clicked);
                OpenFileDialog dialog = new OpenFileDialog
                {
                    Filter = string.Format("Images|{0}*.bmp;*#{1}.bmp|Calibration Images|*Calibration*.bmp", num % 5 + 1 + ((num < 5) ? "R" : "L"), num + 1),
                    FilterIndex = 2,
                    Multiselect = true,
                    RestoreDirectory = true
                };
                if (!(dialog.ShowDialog(Application.Current.MainWindow) != true))
                {
                    this.Reset_Click(null, null);
                    if (this._calibration == null)
                    {
                        this._calibration = new DataLayer.Entities.Calibration
                        {
                            DeviceId = ((DataLayer.Entities.DeviceSetting)this.Configurations.SelectedItem).DeviceId
                        }
                        ;
                    }
                    Task.Run(() => this.StoreAndProcessImages(dialog.FileNames, clicked, images)).ContinueWith(t =>
                    {
                        Log.Error("There was an error loading the calibration images.", t.Exception?.InnerException ?? t.Exception);
                        base.Dispatcher.Invoke<MessageBoxResult>(() => Xceed.Wpf.Toolkit.MessageBox.Show("There was an error loading the calibration images."));
                    }, TaskContinuationOptions.OnlyOnFaulted);
                }
            }
        }

        // Token: 0x06000013 RID: 19 RVA: 0x00002CE0 File Offset: 0x00000EE0
        private void Reset_Click(object sender, RoutedEventArgs e)
        {
            this._calibration = null;
            foreach (CalibrationImage calibrationImage in this.CalibrationImages.Children.OfType<CalibrationImage>().ToList<CalibrationImage>())
            {
                calibrationImage.Clear();
            }
            this.CalibrationImages.Children.OfType<CalibrationImage>().Skip(10).ToList<CalibrationImage>().ForEach(delegate (CalibrationImage x)
            {
                this.CalibrationImages.Children.Remove(x);
            });
        }

        // Token: 0x06000014 RID: 20 RVA: 0x00002D80 File Offset: 0x00000F80
        private void DutyCycle_Checked(object sender, RoutedEventArgs e)
        {
            if (this.DutyCycle.IsChecked != null && this.DutyCycle.IsChecked.Value)
            {
                this._calibration = null;
                this.FillImages();
            }
        }

        // Token: 0x06000015 RID: 21 RVA: 0x00002E74 File Offset: 0x00001074
        private void CreateCalibrationExport(List<DataLayer.Entities.ImageCalibration> calibrations)
        {
            this._progress = ProgressDialog.Show("Export Progress", new string[]
            {
                "Item Progress",
                "Overall Progress"
            });
            this._progress.Update(new Tuple<string, double>[]
            {
                new Tuple<string, double>("", 0.0),
                new Tuple<string, double>("Creating Directory", 0.0)
            });
            this._tempPath = Path.Combine(Path.GetTempPath(), "CalibrationExport_" + Guid.NewGuid());
            FileSystem.CreateDirectory(this._tempPath);
            string str = DateTime.Now.ToString("yyMMddfff");
            Action<string, int> reportProgress = delegate (string s, int i)
            {
                Log.Debug(s);
                this._progress.Update(new Tuple<string, double>[]
                {
                    new Tuple<string, double>(s, (double)i),
                    new Tuple<string, double>("Exporting Images", 20.0)
                });
            };
            string calibrationDir = Path.Combine(this._tempPath, "CalibrationImages");
            Export.Calibration(calibrations, calibrationDir, reportProgress);
            Action<string, int> reportProgress2 = delegate (string s, int i)
            {
                Log.Debug(s);
                this._progress.Update(new Tuple<string, double>[]
                {
                    new Tuple<string, double>(s, (double)i),
                    new Tuple<string, double>("Saving Calibration Data", 40.0)
                });
            };
            string filename = Path.Combine(this._tempPath, "CalibrationData_" + str + ".csv");
            Excel.CreateCSVFromImageCalibrations(calibrations, filename, reportProgress2);
            this._progress.Update(new Tuple<string, double>[]
            {
                new Tuple<string, double>("", 0.0),
                new Tuple<string, double>("Generating Zip File", 60.0)
            });
            string text = Path.Combine(Path.GetTempPath(), "Calibration_" + str + ".zip");
            ZipFile.CreateFromDirectory(this._tempPath, text, CompressionLevel.Fastest, false);
            this._outputFile = text;
            if (!string.IsNullOrEmpty(this._saveLocation))
            {
                this._progress.Update(new Tuple<string, double>[]
                {
                    new Tuple<string, double>("", 0.0),
                    new Tuple<string, double>("Copying Zip File", 80.0)
                });
                this.CopyToDestination(this._outputFile, this._saveLocation);
            }
            else
            {
                this._progress.Update(new Tuple<string, double>[]
                {
                    new Tuple<string, double>("", 0.0),
                    new Tuple<string, double>("Waiting for User Input", 80.0)
                });
                this._progress.Pause();
            }
        }

        // Token: 0x06000016 RID: 22 RVA: 0x00003114 File Offset: 0x00001314
        private void CopyToDestination(string fromFile, string toFile)
        {
            try
            {
                if (!string.IsNullOrEmpty(fromFile) && !string.IsNullOrEmpty(toFile))
                {
                    FileSystem.CopyFile(fromFile, toFile, true);
                    this._progress.Update(new Tuple<string, double>[]
                    {
                        new Tuple<string, double>("", 0.0),
                        new Tuple<string, double>("Deleting Temporary Files", 100.0)
                    });
                    General.Delete(this._tempPath);
                    General.Delete(this._outputFile);
                    this._saveLocation = null;
                    this._outputFile = null;
                    Application.Current.Dispatcher.BeginInvoke(new Action(delegate ()
                    {
                        this._progress.Close();
                        new OpenWith(toFile)
                        {
                            Owner = Application.Current.MainWindow
                        }.ShowDialog();
                    }), new object[0]);
                }
            }
            catch (Exception ex)
            {
                Log.Error("There was an error copying the file.", ex);
            }
        }

        // Token: 0x06000017 RID: 23 RVA: 0x0000355C File Offset: 0x0000175C
        private void Calibration_Finished(CalibrationImage sender, DataLayer.Entities.ImageCalibration result)
        {
            if (this._calibration != null)
            {
                if (result != null)
                {
                    lock (this._calibration)
                    {
                        try
                        {
                            this._calibration.TimeCalibrated = DateTime.UtcNow;
                            this._calibration.Calibrations.Add(result);
                            this._calibration.Save(true);
                        }
                        catch (Exception ex)
                        {
                            Log.Error("There was an error saving the calibration.", ex);
                            base.Dispatcher.Invoke<MessageBoxResult>(() => Xceed.Wpf.Toolkit.MessageBox.Show("There was an error saving the calibration.", "Calibration Results", MessageBoxButton.OK));
                            return;
                        }
                    }
                }

                base.Dispatcher.Invoke(delegate ()
                {
                    List<CalibrationImage> source = this.CalibrationImages.Children.OfType<CalibrationImage>().ToList<CalibrationImage>();
                    if (!source.Any((CalibrationImage x) => x.State != States.Complete && x.State != States.Failed))
                    {
                        MessageBoxResult messageBoxResult;
                        if (source.All((CalibrationImage x) => !x.Result.Failed))
                        {
                            ClearViewConfiguration.Current.Calibration = this._calibration;
                            messageBoxResult = Xceed.Wpf.Toolkit.MessageBox.Show("Calibration passed.  Would you like to save the images to disk?", "Calibration Results", MessageBoxButton.YesNo);
                        }
                        else
                        {
                            messageBoxResult = Xceed.Wpf.Toolkit.MessageBox.Show("Calibration failed!  Would you like to save the images to disk?", "Calibration Results", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
                        }
                        if (messageBoxResult == MessageBoxResult.Yes)
                        {
                            List<DataLayer.Entities.ImageCalibration> imageCalibrations = source.Select(ci => (DataLayer.Entities.ImageCalibration)ci.Result).ToList<DataLayer.Entities.ImageCalibration>();
                            Task.Run(() => this.CreateCalibrationExport(imageCalibrations)).ContinueWith(t =>
                            {
                                Log.Error("An error occured while exporting.", t.Exception?.InnerException ?? t.Exception);
                                if (this._progress != null)
                                {
                                    this._progress.Error("An error occured while exporting.");
                                }
                            }, TaskContinuationOptions.OnlyOnFaulted);

                            SaveFileDialog saveFileDialog = new SaveFileDialog
                            {
                                Filter = "Compressed Zip Files|*.zip",
                                FilterIndex = 1
                            };
                            if (saveFileDialog.ShowDialog(Application.Current.MainWindow) == true)
                            {
                                this._saveLocation = saveFileDialog.FileName;
                                if (this._outputFile != null)
                                {
                                    Task.Run(() => this.CopyToDestination(this._outputFile, this._saveLocation));
                                }
                            }
                            else
                            {
                                Task.Run(() =>
                                {
                                    General.Delete(this._tempPath);
                                    General.Delete(this._outputFile);
                                });
                                if (this._progress != null)
                                {
                                    this._progress.Close();
                                }
                            }
                        }
                    }
                });
            }
        }

        // Token: 0x06000018 RID: 24 RVA: 0x0000365C File Offset: 0x0000185C
        private void Select_Image(object sender, MouseButtonEventArgs e)
        {
            CalibrationImage calibrationImage = sender as CalibrationImage;
            if (calibrationImage != null)
            {
                if (this.Selected != null)
                {
                    this.Selected.SetValue(Control.BorderBrushProperty, DependencyProperty.UnsetValue);
                }
                this.Selected = calibrationImage;
                this.Selected.BorderBrush = System.Windows.Media.Brushes.Red;
                this.Capture.Source = this.Selected.Image;
            }
        }

        // Token: 0x04000007 RID: 7
        private ICapturable _camera;

        // Token: 0x04000008 RID: 8
        // TODO: finish device
        //private IControllable _device;

        // Token: 0x04000009 RID: 9
        private DataLayer.Entities.Calibration _calibration;

        // Token: 0x0400000A RID: 10
        private string _tempPath;

        // Token: 0x0400000B RID: 11
        private string _saveLocation;

        // Token: 0x0400000C RID: 12
        private string _outputFile;

        // Token: 0x0400000D RID: 13
        private ProgressDialog _progress;

        // Token: 0x0400000E RID: 14
        private Bitmap _lastBitmap;

        // Token: 0x0400000F RID: 15
        private readonly Semaphore _updating = new Semaphore(1, 1);

        // Token: 0x04000010 RID: 16
        private decimal _droppedFrames;

        // Token: 0x04000011 RID: 17
        private readonly Stopwatch _stopwatch = Stopwatch.StartNew();

        // Token: 0x04000012 RID: 18
        public static readonly DependencyProperty SelectedProperty = DependencyProperty.Register("Selected", typeof(CalibrationImage), typeof(Calibrate), new PropertyMetadata(null));
    }
}
