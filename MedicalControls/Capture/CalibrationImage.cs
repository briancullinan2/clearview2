using EPIC.DataAnalysis;
using EPIC.DataLayer.Customization;
using EPIC.DataLayer.Extensions;
using EPIC.MedicalControls.Annotations;
using EPIC.MedicalControls.Utilities;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace EPIC.MedicalControls.Controls
{
    // Token: 0x02000009 RID: 9
    public class CalibrationImage : ProcessingImage, INotifyPropertyChanged
    {
        // Token: 0x14000001 RID: 1
        // (add) Token: 0x06000094 RID: 148 RVA: 0x00006608 File Offset: 0x00004808
        // (remove) Token: 0x06000095 RID: 149 RVA: 0x00006644 File Offset: 0x00004844
        private event CalibrationImage.ProcessingFinishedHandler _finished;

        // Token: 0x14000002 RID: 2
        // (add) Token: 0x06000096 RID: 150 RVA: 0x00006680 File Offset: 0x00004880
        // (remove) Token: 0x06000097 RID: 151 RVA: 0x000066C2 File Offset: 0x000048C2
        public event CalibrationImage.ProcessingFinishedHandler Finished
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

        // Token: 0x17000027 RID: 39
        // (get) Token: 0x06000098 RID: 152 RVA: 0x000066D0 File Offset: 0x000048D0
        // (set) Token: 0x06000099 RID: 153 RVA: 0x000066F2 File Offset: 0x000048F2
        public DataLayer.Entities.ImageCalibration Result
        {
            get
            {
                return (DataLayer.Entities.ImageCalibration)base.GetValue(CalibrationImage.ResultProperty);
            }
            private set
            {
                base.SetValue(CalibrationImage.ResultProperty, value);
            }
        }

        // Token: 0x17000028 RID: 40
        // (get) Token: 0x0600009A RID: 154 RVA: 0x00006704 File Offset: 0x00004904
        // (set) Token: 0x0600009B RID: 155 RVA: 0x0000671C File Offset: 0x0000491C
        public int? Brightness
        {
            get
            {
                return this._brightness;
            }
            private set
            {
                this._brightness = value;
                this.OnPropertyChanged("Brightness");
            }
        }

        // Token: 0x17000029 RID: 41
        // (get) Token: 0x0600009C RID: 156 RVA: 0x00006734 File Offset: 0x00004934
        // (set) Token: 0x0600009D RID: 157 RVA: 0x0000674C File Offset: 0x0000494C
        public int? Gain
        {
            get
            {
                return this._gain;
            }
            private set
            {
                this._gain = value;
                this.OnPropertyChanged("Gain");
            }
        }

        // Token: 0x1700002A RID: 42
        // (get) Token: 0x0600009E RID: 158 RVA: 0x00006764 File Offset: 0x00004964
        // (set) Token: 0x0600009F RID: 159 RVA: 0x0000677C File Offset: 0x0000497C
        public int? Exposure
        {
            get
            {
                return this._exposure;
            }
            private set
            {
                this._exposure = value;
                this.OnPropertyChanged("Exposure");
            }
        }

        // Token: 0x1700002B RID: 43
        // (get) Token: 0x060000A0 RID: 160 RVA: 0x00006794 File Offset: 0x00004994
        // (set) Token: 0x060000A1 RID: 161 RVA: 0x000067AC File Offset: 0x000049AC
        public Voltage? Voltage
        {
            get
            {
                return this._voltage;
            }
            private set
            {
                this._voltage = value;
                this.OnPropertyChanged("Voltage");
            }
        }

        // Token: 0x1700002C RID: 44
        // (get) Token: 0x060000A2 RID: 162 RVA: 0x000067C4 File Offset: 0x000049C4
        // (set) Token: 0x060000A3 RID: 163 RVA: 0x000067E6 File Offset: 0x000049E6
        public BitmapSource Colorized
        {
            get
            {
                return (BitmapSource)base.GetValue(CalibrationImage.ColorizedProperty);
            }
            set
            {
                base.SetValue(CalibrationImage.ColorizedProperty, value);
            }
        }

        // Token: 0x1700002D RID: 45
        // (get) Token: 0x060000A4 RID: 164 RVA: 0x000067F8 File Offset: 0x000049F8
        // (set) Token: 0x060000A5 RID: 165 RVA: 0x0000681A File Offset: 0x00004A1A
        public CalibrationImage.MathsOptions Options
        {
            get
            {
                return (CalibrationImage.MathsOptions)base.GetValue(CalibrationImage.OptionsProperty);
            }
            set
            {
                base.SetValue(CalibrationImage.OptionsProperty, value);
            }
        }

        // Token: 0x060000A6 RID: 166 RVA: 0x0000682F File Offset: 0x00004A2F
        public CalibrationImage() : base()
        {
            ToolTipService.SetShowDuration(this, 60000);
        }

        // Token: 0x060000A7 RID: 167 RVA: 0x000068EC File Offset: 0x00004AEC
        public CalibrationImage(DataLayer.Entities.ImageCalibration img) : this()
        {
            if (img.Image.Capture != null)
            {
                DataLayer.Entities.Capture capture = img.Image.Capture.Capture;
                this.Brightness = new int?(capture.Brightness);
                this.Gain = new int?(capture.Gain);
                this.Exposure = new int?(capture.Exposure);
                this.Voltage = new Voltage?(capture.Voltage);
            }
            base.Image = Compression.GetImageSource(img.Image);
            this.Result = img;
            if (img.Colorized == null)
            {
                Task.Run(() => ProcessOne(img))
                    .ContinueWith(t => this._finished?.Invoke(this, t.Result as DataLayer.Entities.ImageCalibration), TaskContinuationOptions.NotOnFaulted)
                    .ContinueWith(t => Utilities.Log.Error("Constructor-initiated task failed.", t.Exception?.InnerException ?? t.Exception), TaskContinuationOptions.OnlyOnFaulted);
            }
            else
            {
                this.Colorized = Compression.GetImageSource(img.Colorized);
                base.Dispatcher.Invoke<States>(() => State = ((!img.Failed) ? States.Complete : States.Failed));
            }
            base.MouseEnter += this.Completed_ShowToolTip;
            base.MouseLeave += this.Completed_HideToolTip;
        }


        // Token: 0x060000A8 RID: 168 RVA: 0x00006A98 File Offset: 0x00004C98
        public CalibrationImage(CaptureResults results) : this()
        {
            this.StoreAndProcess(results);
        }

        // Token: 0x060000A9 RID: 169 RVA: 0x00006B24 File Offset: 0x00004D24
        private DataLayer.Entities.ImageCalibration ProcessOne(DataLayer.Entities.ImageCalibration img)
        {
            DataLayer.Entities.DeviceCalibrationSetting settings = img.CalibrationSetting;
            base.Dispatcher.Invoke(delegate ()
            {
                base.State = States.Processing;
                base.Value = 0.0;
            });
            using (Bitmap bitmap = Compression.DecompressImage(img.Image.ImageData))
            {
                DataLayer.Entities.Image colorized;
                DataLayer.Entities.ImageCalibration result = Maths.CheckCalibration(bitmap, settings, out colorized);
                if (result != null)
                {
                    img.Colorized = colorized;
                    img.Save(true);
                    base.Dispatcher.Invoke(delegate ()
                    {
                        this.Colorized = Compression.GetImageSource(colorized);
                    });
                }
                base.Dispatcher.Invoke<States>(() => this.State = ((result != null && !result.Failed) ? States.Complete : States.Failed));
            }
            return img;
        }

        // Token: 0x060000AA RID: 170 RVA: 0x00006C50 File Offset: 0x00004E50
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
                DataLayer.Entities.Capture capture = results.Capture;
                if (capture != null)
                {
                    this.Brightness = new int?(capture.Brightness);
                    this.Gain = new int?(capture.Gain);
                    this.Exposure = new int?(capture.Exposure);
                    this.Voltage = new Voltage?(capture.Voltage);
                }
                Task.Run(() => this.StartProcessing(results))
                    .ContinueWith(t => Utilities.Log.Error("There was an error while processing the calibration results.", t.Exception?.InnerException ?? t.Exception), TaskContinuationOptions.OnlyOnFaulted);
            }
        }


        private CancellationTokenSource _cts = new();


        // Token: 0x060000AB RID: 171 RVA: 0x00006E7C File Offset: 0x0000507C
        private async void StartProcessing(CaptureResults results)
        {
            try
            {
                CalibrationImage.MathsOptions options = (CalibrationImage.MathsOptions)0;
                base.Dispatcher.Invoke(delegate ()
                {
                    this.State = States.Processing;
                    this.Value = 0.0;
                    options = this.Options;
                }, DispatcherPriority.Background);
                int num = 0;
                foreach (Bitmap bitmap in results.Images)
                {
                    Dispatcher.BeginInvoke(() => base.State = States.Paused);
                    // TODO: wait for other threads because it could break MATLAB?
                    Dispatcher.BeginInvoke(() => base.State = States.Processing);

                    var settings = DeviceSetting.Device.Calibrations.FirstOrDefault();
                    DataLayer.Entities.Image colorized;
                    DataLayer.Entities.ImageCalibration result = Maths.CheckCalibration(bitmap, settings, out colorized) as DataLayer.Entities.ImageCalibration;
                    if ((options & CalibrationImage.MathsOptions.OldCalibration) == CalibrationImage.MathsOptions.OldCalibration)
                    {
                        result = Maths.DoesImagePassValidation(bitmap, settings, out colorized) as DataLayer.Entities.ImageCalibration;
                    }
                    bool flag = false;
                    if (num == results.ClosestIndex && result != null)
                    {
                        flag = !result.Failed;
                    }
                    if (result != null)
                    {
                        result.Image = results[bitmap].Item3;
                        result.Colorized = colorized;
                        if ((options & CalibrationImage.MathsOptions.OldCalibration) != CalibrationImage.MathsOptions.OldCalibration)
                        {
                            result.Save(true);
                        }
                    }
                    double percent = Math.Round((double)num / (double)results.Images.Count * 100.0);
                    base.Dispatcher.Invoke(new Action<bool>(delegate (bool b)
                    {
                        this.Value = percent;
                        if (b)
                        {
                            this.Colorized = Compression.GetImageSource(colorized);
                            this.Result = result;
                            this.MouseEnter += this.Completed_ShowToolTip;
                            this.MouseLeave += this.Completed_HideToolTip;
                        }
                    }), new object[]
                    {
                        num == results.ClosestIndex && result != null
                    });
                    num++;
                    if (num == results.Images.Count)
                    {
                        base.Dispatcher.Invoke(new Action<bool>(delegate (bool p)
                        {
                            base.State = (p ? States.Complete : States.Failed);
                        }), new object[]
                        {
                            flag
                        });
                    }
                    if (this._finished != null)
                    {
                        this._finished(this, result);
                    }
                }
            }
            finally
            {
                results.Dispose();
            }
        }

        // Token: 0x060000AC RID: 172 RVA: 0x0000718C File Offset: 0x0000538C
        public void Clear()
        {
            if (!base.Dispatcher.CheckAccess())
            {
                base.Dispatcher.Invoke(new Action(this.Clear));
            }
            else
            {
                base.Image = null;
                this.Result = null;
                base.MouseEnter -= this.Completed_ShowToolTip;
                base.MouseLeave -= this.Completed_HideToolTip;
                this.Colorized = null;
                this.Brightness = null;
                this.Gain = null;
                this.Exposure = null;
                this.Voltage = null;
                base.State = States.Queued;
            }
        }

        // Token: 0x060000AD RID: 173 RVA: 0x00007250 File Offset: 0x00005450
        private void Completed_HideToolTip(object sender, MouseEventArgs e)
        {
            if (base.ToolTip != null)
            {
                ((ToolTip)base.ToolTip).IsOpen = false;
            }
        }

        // Token: 0x060000AE RID: 174 RVA: 0x00007280 File Offset: 0x00005480
        private void Completed_ShowToolTip(object sender, MouseEventArgs mouseEventArgs)
        {
            if (base.ToolTip == null)
            {
                FrameworkElementFactory frameworkElementFactory = new FrameworkElementFactory(typeof(TextBlock));
                frameworkElementFactory.SetValue(TextBlock.TextProperty, new Binding("Item.Header")
                {
                    RelativeSource = new RelativeSource(RelativeSourceMode.FindAncestor)
                    {
                        AncestorType = typeof(DataGridRow)
                    }
                });
                DataGrid dataGrid = new DataGrid
                {
                    CanUserResizeRows = false,
                    CanUserReorderColumns = false,
                    IsReadOnly = true,
                    AutoGenerateColumns = false,
                    RowHeaderTemplate = new DataTemplate
                    {
                        VisualTree = frameworkElementFactory
                    }
                };
                dataGrid.Columns.Add(new DataGridTextColumn
                {
                    Header = "Outer",
                    Binding = new Binding("Outer")
                });
                dataGrid.Columns.Add(new DataGridTextColumn
                {
                    Header = "Inner",
                    Binding = new Binding("Inner")
                });
                dataGrid.Columns.Add(new DataGridTextColumn
                {
                    Header = "Corona",
                    Binding = new Binding("Corona")
                });
                dataGrid.Columns.Add(new DataGridTextColumn
                {
                    Header = "HighP",
                    Binding = new Binding("HighP")
                });
                dataGrid.Columns.Add(new DataGridTextColumn
                {
                    Header = "Total",
                    Binding = new Binding("Total")
                });
                dataGrid.Columns.Add(new DataGridTextColumn
                {
                    Header = "Clumps",
                    Binding = new Binding("Clumps")
                });
                dataGrid.ItemsSource = new List<CalibrationImage.GridRow>
                {
                    new CalibrationImage.GridRow
                    {
                        Header = "Failed Pixels",
                        Outer = this.Result.OuterFailures.ToString("F0"),
                        Inner = this.Result.InnerFailures.ToString("F0"),
                        Corona = this.Result.CoronaFailures.ToString("F0"),
                        HighP = this.Result.HighPFailures.ToString("F0"),
                        Total = this.Result.TotalFailures.ToString("F0"),
                        Clumps = this.Result.ClumpsFailures.ToString("F0")
                    },
                    new CalibrationImage.GridRow
                    {
                        Header = "Total Pixels",
                        Outer = this.Result.OuterTotalPixels.ToString("F0"),
                        Inner = this.Result.InnerTotalPixels.ToString("F0"),
                        Corona = this.Result.CoronaTotalPixels.ToString("F0"),
                        HighP = this.Result.HighPTotalPixels.ToString("F0"),
                        Total = this.Result.TotalTotalPixels.ToString("F0"),
                        Clumps = this.Result.ClumpsTotalPixels.ToString("F0")
                    },
                    new CalibrationImage.GridRow
                    {
                        Header = "% Failed",
                        Outer = this.Result.OuterFailurePercent.ToString("F2"),
                        Inner = this.Result.InnerFailurePercent.ToString("F2"),
                        Corona = this.Result.CoronaFailurePercent.ToString("F2"),
                        HighP = this.Result.HighPFailurePercent.ToString("F2"),
                        Total = this.Result.TotalFailurePercent.ToString("F2"),
                        Clumps = this.Result.ClumpsFailurePercent.ToString("F2")
                    },
                    new CalibrationImage.GridRow
                    {
                        Header = "Status",
                        Outer = (this.Result.OuterFailed ? "Failed" : "Passed"),
                        Inner = (this.Result.InnerFailed ? "Failed" : "Passed"),
                        Corona = (this.Result.CoronaFailed ? "Failed" : "Passed"),
                        HighP = (this.Result.HighPFailed ? "Failed" : "Passed"),
                        Total = (this.Result.TotalFailed ? "Failed" : "Passed"),
                        Clumps = (this.Result.ClumpsFailed ? "Failed" : "Passed")
                    },
                    new CalibrationImage.GridRow
                    {
                        Header = "Diff",
                        Outer = this.Result.OuterMeanDiff.ToString("F2"),
                        Inner = this.Result.InnerMeanDiff.ToString("F2"),
                        Corona = this.Result.CoronaMeanDiff.ToString("F2"),
                        HighP = this.Result.HighPMeanDiff.ToString("F2")
                    },
                    new CalibrationImage.GridRow
                    {
                        Header = "Status",
                        Outer = (this.Result.OuterMeanFailed ? "Failed" : "Passed"),
                        Inner = (this.Result.InnerMeanFailed ? "Failed" : "Passed"),
                        Corona = (this.Result.CoronaMeanFailed ? "Failed" : "Passed"),
                        HighP = (this.Result.HighPMeanFailed ? "Failed" : "Passed")
                    }
                };
                ToolTip toolTip = new ToolTip
                {
                    Content = dataGrid,
                    StaysOpen = true,
                    PlacementTarget = this
                };
                base.ToolTip = toolTip;
            }
            ((ToolTip)base.ToolTip).IsOpen = true;
        }

        // Token: 0x14000003 RID: 3
        // (add) Token: 0x060000AF RID: 175 RVA: 0x000079A0 File Offset: 0x00005BA0
        // (remove) Token: 0x060000B0 RID: 176 RVA: 0x000079DC File Offset: 0x00005BDC
        public event PropertyChangedEventHandler? PropertyChanged;

        // Token: 0x060000B1 RID: 177 RVA: 0x00007A18 File Offset: 0x00005C18
        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler? propertyChanged = this.PropertyChanged;
            if (propertyChanged != null)
            {
                propertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        // Token: 0x0400006B RID: 107
        public static readonly DependencyProperty ResultProperty = DependencyProperty.Register("Result", typeof(DataLayer.Entities.ImageCalibration), typeof(CalibrationImage), new PropertyMetadata(null));

        // Token: 0x0400006C RID: 108
        private int? _brightness;

        // Token: 0x0400006D RID: 109
        private int? _gain;

        // Token: 0x0400006E RID: 110
        private int? _exposure;

        // Token: 0x0400006F RID: 111
        private DataLayer.Customization.Voltage? _voltage;

        // Token: 0x04000070 RID: 112
        public static readonly DependencyProperty ColorizedProperty = DependencyProperty.Register("Colorized", typeof(BitmapSource), typeof(CalibrationImage), new PropertyMetadata(null));

        // Token: 0x04000071 RID: 113
        public static readonly DependencyProperty OptionsProperty = DependencyProperty.Register("Options", typeof(CalibrationImage.MathsOptions), typeof(CalibrationImage), new PropertyMetadata(null));

        // Token: 0x0200000A RID: 10
        [Flags]
        public enum MathsOptions
        {
            // Token: 0x04000076 RID: 118
            OldCalibration = 1
        }

        // Token: 0x0200000B RID: 11
        // (Invoke) Token: 0x060000BB RID: 187
        public delegate void ProcessingFinishedHandler(CalibrationImage sender, DataLayer.Entities.ImageCalibration? calibration);

        // Token: 0x0200000C RID: 12
        private class GridRow
        {
            // Token: 0x1700002E RID: 46
            // (get) Token: 0x060000BE RID: 190 RVA: 0x00007AE0 File Offset: 0x00005CE0
            // (set) Token: 0x060000BF RID: 191 RVA: 0x00007AF7 File Offset: 0x00005CF7
            public string Outer { get; set; }

            // Token: 0x1700002F RID: 47
            // (get) Token: 0x060000C0 RID: 192 RVA: 0x00007B00 File Offset: 0x00005D00
            // (set) Token: 0x060000C1 RID: 193 RVA: 0x00007B17 File Offset: 0x00005D17
            public string Inner { get; set; }

            // Token: 0x17000030 RID: 48
            // (get) Token: 0x060000C2 RID: 194 RVA: 0x00007B20 File Offset: 0x00005D20
            // (set) Token: 0x060000C3 RID: 195 RVA: 0x00007B37 File Offset: 0x00005D37
            public string Corona { get; set; }

            // Token: 0x17000031 RID: 49
            // (get) Token: 0x060000C4 RID: 196 RVA: 0x00007B40 File Offset: 0x00005D40
            // (set) Token: 0x060000C5 RID: 197 RVA: 0x00007B57 File Offset: 0x00005D57
            public string HighP { get; set; }

            // Token: 0x17000032 RID: 50
            // (get) Token: 0x060000C6 RID: 198 RVA: 0x00007B60 File Offset: 0x00005D60
            // (set) Token: 0x060000C7 RID: 199 RVA: 0x00007B77 File Offset: 0x00005D77
            public string Total { get; set; }

            // Token: 0x17000033 RID: 51
            // (get) Token: 0x060000C8 RID: 200 RVA: 0x00007B80 File Offset: 0x00005D80
            // (set) Token: 0x060000C9 RID: 201 RVA: 0x00007B97 File Offset: 0x00005D97
            public string Clumps { get; set; }

            // Token: 0x17000034 RID: 52
            // (get) Token: 0x060000CA RID: 202 RVA: 0x00007BA0 File Offset: 0x00005DA0
            // (set) Token: 0x060000CB RID: 203 RVA: 0x00007BB7 File Offset: 0x00005DB7
            public string Header { get; set; }
        }
    }
}
