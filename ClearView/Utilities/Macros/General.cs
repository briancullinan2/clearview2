using EPIC.CameraInterface;
using EPIC.CameraInterface.Interfaces;
using EPIC.CameraInterface.Utilities;
using EPIC.DataLayer.Customization;
using EPIC.DataLayer.Utilities.Extensions;
using EPIC.MedicalControls.Controls.Capture;
using Microsoft.VisualBasic.FileIO;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows;

namespace EPIC.ClearView.Utilities.Macros
{
    // Token: 0x02000017 RID: 23
    public static class General
    {
        // Token: 0x06000146 RID: 326 RVA: 0x0000BFEC File Offset: 0x0000A1EC
        public static void Capture(DataLayer.Entities.DeviceSetting settings, CaptureCallback callback, bool onlyClosest)
        {
            General.Capture(settings.Frequency, settings.PulseDuration, settings.PulseWidth, settings.ExposureDelay, settings.Voltage, settings.Brightness, settings.Gain, settings.ExposureDelay, callback, settings.Device, onlyClosest);
        }

        // Token: 0x06000147 RID: 327 RVA: 0x0000C04C File Offset: 0x0000A24C
        public static void Capture(PWM0Frequency frequency, PulseDuration duration, PulseWidth width, int exposureDelay, Voltage voltage, int brightness, int gain, int exposure, CaptureCallback callback, DataLayer.Entities.Device deviceEntity, bool onlyClosest)
        {
            if (deviceEntity == null)
            {
                deviceEntity = ClearViewConfiguration.Current.Device;
            }
            General.StartExposureParams startExposureParams = new General.StartExposureParams
            {
                Camera = CameraManager.Current.Cameras.FirstOrDefault<ICapturable>(),
                // TODO: finish device
                //Device = DeviceManager.Current.Devices.FirstOrDefault<IControllable>(),
                Callback = callback,
                DeviceEntity = deviceEntity,
                Duration = duration,
                Frequency = frequency,
                Voltage = voltage,
                ExposureDelay = exposureDelay,
                PulseWidth = width,
                Brightness = brightness,
                Gain = gain,
                Exposure = exposure,
                OnlyClosest = onlyClosest
            };
            Task.Run(() => General.StartExposure(startExposureParams))
                .ContinueWith(t => General.EndExposure(startExposureParams), TaskContinuationOptions.NotOnFaulted)
                .ContinueWith(t => Log.Error("There was an error capturing.", t.Exception?.InnerException ?? t.Exception), TaskContinuationOptions.OnlyOnFaulted);
        }

        // Token: 0x06000148 RID: 328 RVA: 0x0000C19C File Offset: 0x0000A39C
        private static void EndExposure(General.StartExposureParams sep)
        {
            long closest = General.GetBestImage(sep);
            List<Tuple<long, Bitmap, DataLayer.Entities.Image>> list = new List<Tuple<long, Bitmap, DataLayer.Entities.Image>>();
            List<KeyValuePair<long, Bitmap>> list2 = sep.Captures.ToList<KeyValuePair<long, Bitmap>>();
            if (sep.OnlyClosest)
            {
                list2 = (from x in list2
                         where x.Key == closest
                         select x).ToList<KeyValuePair<long, Bitmap>>();
            }
            try
            {
                DataLayer.Entities.Capture captureEntity = new DataLayer.Entities.Capture
                {
                    CaptureTime = DateTime.UtcNow,
                    DeviceId = sep.DeviceEntity.DeviceId,
                    Frequency = sep.Frequency,
                    PulseDuration = sep.Duration,
                    PulseWidth = sep.PulseWidth,
                    ExposureDelay = sep.ExposureDelay,
                    Voltage = sep.Voltage,
                    Brightness = sep.Brightness,
                    Gain = sep.Gain,
                    Exposure = sep.Exposure
                }
                ;
                foreach (KeyValuePair<long, Bitmap> keyValuePair in list2)
                {
                    byte[] array = Compression.CompressImage(keyValuePair.Value);
                    if (array != null)
                    {
                        DataLayer.Entities.Image imageEntity = new DataLayer.Entities.Image
                        {
                            ImageData = array
                        };
                        captureEntity.Images.Add(new DataLayer.Entities.ImageCapture
                        {
                            Image = imageEntity
                        });
                        list.Add(new Tuple<long, Bitmap, DataLayer.Entities.Image>(keyValuePair.Key, keyValuePair.Value, imageEntity));
                    }
                }
                captureEntity.Save(true);
                int closestIndex = (from x in list
                                    select x.Item1).ToList<long>().IndexOf(closest);
                CaptureResults results = new CaptureResults(list, closestIndex, captureEntity);
                if (sep.Callback != null)
                {
                    sep.Callback.BeginInvoke(results, null, null);
                }
            }
            catch (Exception ex)
            {
                Log.Error("There was an error capturing a scan.", ex);
                int closestIndex = (from x in list
                                    select x.Item1).ToList<long>().IndexOf(closest);
                CaptureResults results = new CaptureResults(list, closestIndex, null);
                if (sep.Callback != null)
                {
                    sep.Callback.BeginInvoke(results, null, null);
                }
            }
        }

        // Token: 0x06000149 RID: 329 RVA: 0x0000C4B8 File Offset: 0x0000A6B8
        private static long GetBestImage(General.StartExposureParams sep)
        {
            long min = sep.Captures.Keys.Min((long x) => Math.Abs(x - (long)sep.ExposureDelay));
            return sep.Captures.First((KeyValuePair<long, Bitmap> x) => Math.Abs(x.Key - (long)sep.ExposureDelay) == min).Key;
        }

        // Token: 0x0600014A RID: 330 RVA: 0x0000C55C File Offset: 0x0000A75C
        private static void StartExposure(General.StartExposureParams sep)
        {
            uint num;
            IntPtr intPtr;
            // TODO: finish device
            //if ((intPtr = Avrt.AvSetMmThreadCharacteristics("Capture", ref num)) == IntPtr.Zero)
            //{
            //    throw new Win32Exception();
            //}
            Stopwatch sw = sep.Stopwatch;
            Dictionary<long, Bitmap> captures = sep.Captures;
            sep.Camera.Brightness = sep.Brightness;
            sep.Camera.Gain = sep.Gain;
            sep.Camera.Exposure = sep.Exposure;
            // TODO: finish device
            //sep.Device.SetFrequency(sep.Frequency);
            //sep.Device.SetExposureVoltage(sep.Voltage);
            //sep.Device.SetPulseDuration(sep.Duration);
            FrameCallback value = delegate (IntPtr bitmap)
            {
                Bitmap value2 = Image.FromHbitmap(bitmap);
                long elapsedMilliseconds = sw.ElapsedMilliseconds;
                captures.Add(elapsedMilliseconds, value2);
            };
            sep.Camera.Captured += value;
            sw.Start();
            // TODO: finish device
            //sep.Device.StartExposure();
            long elapsedTicks = sw.ElapsedTicks;
            while (sw.ElapsedTicks <= (int)sep.Duration * 10000 + elapsedTicks)
            {
                Thread.Sleep(1);
            }
            sep.Camera.Captured -= value;
            sw.Stop();
            // TODO: finish device
            //if (!Avrt.AvRevertMmThreadCharacteristics(intPtr))
            //{
            //    throw new Win32Exception();
            //}
        }

        // Token: 0x0600014B RID: 331 RVA: 0x0000C6E4 File Offset: 0x0000A8E4
        public static void Connect(FrameCallback frameCallback, string cameraName, string cameraMessage = "Loading cameras...", string deviceMessage = "Loading devices...", string reconnectMessage = "Reconnecting...")
        {
            General.ConnectedInfo connectedInfo = new General.ConnectedInfo
            {
                FrameCallback = frameCallback,
                CameraMessage = cameraMessage,
                DeviceMessage = deviceMessage,
                ReconnectMessage = reconnectMessage,
                CameraName = cameraName
            };
            Task.Run(() => General.ConnectThread(connectedInfo))
                .ContinueWith(t => Log.Error("There was an error connecting to the camera.", t.Exception?.InnerException ?? t.Exception), TaskContinuationOptions.OnlyOnFaulted);
        }

        // Token: 0x0600014C RID: 332 RVA: 0x0000C808 File Offset: 0x0000AA08
        private static General.ConnectedInfo ConnectThread(General.ConnectedInfo connectedInfo)
        {
            if (CameraManager.Current.Cameras == null)
            {
                CameraManager.CamerasChanged value = delegate (CameraManager.CamerasChangedEventArgs args)
                {
                    General.CloseWindows(connectedInfo);
                };
                CameraManager.Current.Changed += value;
                System.Windows.Application.Current.Dispatcher.BeginInvoke(new Action<string>(delegate (string message)
                {
                    Xceed.Wpf.Toolkit.MessageBox.Show(System.Windows.Application.Current.MainWindow, message);
                }), new object[]
                {
                    connectedInfo.CameraMessage
                });
                while (CameraManager.Current.Cameras == null)
                {
                    Thread.Sleep(100);
                }
                CameraManager.Current.Changed -= value;
            }
            // TODO: finish device
            /*
             if (DeviceManager.Current.Devices == null)
             {
                 DeviceManager.DevicesChanged value2 = delegate (DeviceManager.DevicesChangedEventArgs args)
                 {
                     General.CloseWindows(connectedInfo);
                 };
                 DeviceManager.Current.Changed += value2;
                 Application.Current.Dispatcher.BeginInvoke(new Action<string>(delegate (string message)
                 {
                     Xceed.Wpf.Toolkit.MessageBox.Show(Application.Current.MainWindow, message);
                 }), new object[]
                 {
                     connectedInfo.DeviceMessage
                 });
                 while (DeviceManager.Current.Devices == null)
                 {
                     Thread.Sleep(100);
                 }
                 DeviceManager.Current.Changed -= value2;
             }
             */
            ICapturable capturable = CameraManager.Current.Cameras.FirstOrDefault((ICapturable x) => x.DisplayName.Equals(connectedInfo.CameraName));
            if (capturable != null)
            {
                try
                {
                    capturable.Captured += connectedInfo.FrameCallback;
                    connectedInfo.Camera = capturable;
                }
                catch (Exception ex)
                {
                    Log.Debug("There was an error connecting to the camera, reconnecting.", ex);
                    try
                    {
                        System.Windows.Application.Current.Dispatcher.BeginInvoke(new Action<string>(delegate (string message)
                        {
                            Xceed.Wpf.Toolkit.MessageBox.Show(System.Windows.Application.Current.MainWindow, message);
                        }), new object[]
                        {
                            connectedInfo.ReconnectMessage
                        });
                        capturable.Captured -= connectedInfo.FrameCallback;
                        capturable.Captured += connectedInfo.FrameCallback;
                        connectedInfo.Camera = capturable;
                    }
                    catch (Exception ex2)
                    {
                        Log.Error("There was an error connecting to the camera.", ex2);
                        connectedInfo.Camera = null;
                    }
                    finally
                    {
                        General.CloseWindows(connectedInfo);
                    }
                }
            }
            // TODO: finish device
            /*
            IControllable controllable = DeviceManager.Current.Devices.FirstOrDefault<IControllable>();
            if (controllable != null)
            {
                try
                {
                    controllable.Open();
                    connectedInfo.Device = controllable;
                }
                catch (Exception ex)
                {
                    Log.Debug("There was an error connecting to the device, reconnecting.", ex);
                    try
                    {
                        Application.Current.Dispatcher.BeginInvoke(new Action<string>(delegate (string message)
                        {
                            Xceed.Wpf.Toolkit.MessageBox.Show(Application.Current.MainWindow, message);
                        }), new object[]
                        {
                            connectedInfo.ReconnectMessage
                        });
                        controllable.Close();
                        controllable.Open();
                        connectedInfo.Device = controllable;
                    }
                    catch (Exception ex2)
                    {
                        Log.Error("There was an error connecting to the device.", ex2);
                        connectedInfo.Device = null;
                    }
                    finally
                    {
                        General.CloseWindows(connectedInfo);
                    }
                }
            }
            */
            return connectedInfo;
        }

        // Token: 0x0600014D RID: 333 RVA: 0x0000CCD0 File Offset: 0x0000AED0
        private static void CloseWindows(General.ConnectedInfo connectedInfo)
        {
            System.Windows.Application.Current.Dispatcher.BeginInvoke(new Action<string, string, string>(delegate (string deviceMessage, string cameraMessage, string reconnectMessage)
            {
                foreach (Window window in System.Windows.Application.Current.Windows.OfType<Window>())
                {
                    if (window.Content is Xceed.Wpf.Toolkit.MessageBox && (((Xceed.Wpf.Toolkit.MessageBox)window.Content).Text == cameraMessage || ((Xceed.Wpf.Toolkit.MessageBox)window.Content).Text == deviceMessage || ((Xceed.Wpf.Toolkit.MessageBox)window.Content).Text == reconnectMessage))
                    {
                        window.Close();
                    }
                }
            }), new object[]
            {
                connectedInfo.DeviceMessage,
                connectedInfo.CameraMessage,
                connectedInfo.ReconnectMessage
            });
        }

        // Token: 0x0600014E RID: 334 RVA: 0x0000CD5C File Offset: 0x0000AF5C
        public static void Disconnect(FrameCallback frameCallback, string cameraName)
        {
            ICapturable capturable = CameraManager.Current.Cameras.FirstOrDefault((ICapturable x) => x.DisplayName.Equals(cameraName));
            if (capturable != null)
            {
                capturable.Captured -= frameCallback;
            }
            // TODO: finish device
            /*
            IControllable controllable = DeviceManager.Current.Devices.FirstOrDefault<IControllable>();
            if (controllable != null)
            {
                controllable.Close();
            }
            */
        }

        // Token: 0x0600014F RID: 335 RVA: 0x0000CDC8 File Offset: 0x0000AFC8
        public static void Delete(string outputFile)
        {
            if (Directory.Exists(outputFile))
            {
                FileSystem.DeleteDirectory(outputFile, DeleteDirectoryOption.DeleteAllContents);
            }
            if (File.Exists(outputFile))
            {
                FileSystem.DeleteFile(outputFile, UIOption.OnlyErrorDialogs, RecycleOption.DeletePermanently);
            }
        }

        // Token: 0x02000018 RID: 24
        // (Invoke) Token: 0x0600015B RID: 347
        public delegate void CaptureCallback(CaptureResults results);

        // Token: 0x02000019 RID: 25
        private class StartExposureParams
        {
            // Token: 0x1700004B RID: 75
            // (get) Token: 0x0600015E RID: 350 RVA: 0x0000CE1C File Offset: 0x0000B01C
            // (set) Token: 0x0600015F RID: 351 RVA: 0x0000CE50 File Offset: 0x0000B050
            public Stopwatch Stopwatch
            {
                get
                {
                    if (this._stopwatch == null)
                    {
                        this.Stopwatch = new Stopwatch();
                    }
                    return this._stopwatch;
                }
                set
                {
                    this._stopwatch = value;
                }
            }

            // Token: 0x1700004C RID: 76
            // (get) Token: 0x06000160 RID: 352 RVA: 0x0000CE5C File Offset: 0x0000B05C
            // (set) Token: 0x06000161 RID: 353 RVA: 0x0000CE73 File Offset: 0x0000B073
            public CaptureCallback Callback { get; set; }

            // Token: 0x1700004D RID: 77
            // (get) Token: 0x06000162 RID: 354 RVA: 0x0000CE7C File Offset: 0x0000B07C
            // (set) Token: 0x06000163 RID: 355 RVA: 0x0000CEB0 File Offset: 0x0000B0B0
            public Dictionary<long, Bitmap> Captures
            {
                get
                {
                    if (this._captures == null)
                    {
                        this.Captures = new Dictionary<long, Bitmap>();
                    }
                    return this._captures;
                }
                set
                {
                    this._captures = value;
                }
            }

            // Token: 0x1700004E RID: 78
            // (get) Token: 0x06000164 RID: 356 RVA: 0x0000CEBC File Offset: 0x0000B0BC
            // (set) Token: 0x06000165 RID: 357 RVA: 0x0000CED3 File Offset: 0x0000B0D3
            public ICapturable Camera { get; set; }

            // Token: 0x1700004F RID: 79
            // (get) Token: 0x06000166 RID: 358 RVA: 0x0000CEDC File Offset: 0x0000B0DC
            // (set) Token: 0x06000167 RID: 359 RVA: 0x0000CEF3 File Offset: 0x0000B0F3
            // TODO: finish device
            //public IControllable Device { get; set; }

            // Token: 0x17000050 RID: 80
            // (get) Token: 0x06000168 RID: 360 RVA: 0x0000CEFC File Offset: 0x0000B0FC
            // (set) Token: 0x06000169 RID: 361 RVA: 0x0000CF13 File Offset: 0x0000B113
            public PWM0Frequency Frequency { get; set; }

            // Token: 0x17000051 RID: 81
            // (get) Token: 0x0600016A RID: 362 RVA: 0x0000CF1C File Offset: 0x0000B11C
            // (set) Token: 0x0600016B RID: 363 RVA: 0x0000CF33 File Offset: 0x0000B133
            public Voltage Voltage { get; set; }

            // Token: 0x17000052 RID: 82
            // (get) Token: 0x0600016C RID: 364 RVA: 0x0000CF3C File Offset: 0x0000B13C
            // (set) Token: 0x0600016D RID: 365 RVA: 0x0000CF53 File Offset: 0x0000B153
            public PulseDuration Duration { get; set; }

            // Token: 0x17000053 RID: 83
            // (get) Token: 0x0600016E RID: 366 RVA: 0x0000CF5C File Offset: 0x0000B15C
            // (set) Token: 0x0600016F RID: 367 RVA: 0x0000CF73 File Offset: 0x0000B173
            public int ExposureDelay { get; set; }

            // Token: 0x17000054 RID: 84
            // (get) Token: 0x06000170 RID: 368 RVA: 0x0000CF7C File Offset: 0x0000B17C
            // (set) Token: 0x06000171 RID: 369 RVA: 0x0000CF93 File Offset: 0x0000B193
            public DataLayer.Entities.Device? DeviceEntity { get; set; }

            // Token: 0x17000055 RID: 85
            // (get) Token: 0x06000172 RID: 370 RVA: 0x0000CF9C File Offset: 0x0000B19C
            // (set) Token: 0x06000173 RID: 371 RVA: 0x0000CFB3 File Offset: 0x0000B1B3
            public PulseWidth PulseWidth { get; set; }

            // Token: 0x17000056 RID: 86
            // (get) Token: 0x06000174 RID: 372 RVA: 0x0000CFBC File Offset: 0x0000B1BC
            // (set) Token: 0x06000175 RID: 373 RVA: 0x0000CFD3 File Offset: 0x0000B1D3
            public int Brightness { get; set; }

            // Token: 0x17000057 RID: 87
            // (get) Token: 0x06000176 RID: 374 RVA: 0x0000CFDC File Offset: 0x0000B1DC
            // (set) Token: 0x06000177 RID: 375 RVA: 0x0000CFF3 File Offset: 0x0000B1F3
            public int Gain { get; set; }

            // Token: 0x17000058 RID: 88
            // (get) Token: 0x06000178 RID: 376 RVA: 0x0000CFFC File Offset: 0x0000B1FC
            // (set) Token: 0x06000179 RID: 377 RVA: 0x0000D013 File Offset: 0x0000B213
            public int Exposure { get; set; }

            // Token: 0x17000059 RID: 89
            // (get) Token: 0x0600017A RID: 378 RVA: 0x0000D01C File Offset: 0x0000B21C
            // (set) Token: 0x0600017B RID: 379 RVA: 0x0000D033 File Offset: 0x0000B233
            public bool OnlyClosest { get; set; }

            // Token: 0x040000BE RID: 190
            private Stopwatch _stopwatch;

            // Token: 0x040000BF RID: 191
            private Dictionary<long, Bitmap> _captures;
        }

        // Token: 0x0200001A RID: 26
        // (Invoke) Token: 0x0600017E RID: 382
        //public delegate void ConnectedHandler(IControllable device, ICapturable camera);

        // Token: 0x0200001B RID: 27
        private class ConnectedInfo
        {
            // Token: 0x1700005A RID: 90
            // (get) Token: 0x06000181 RID: 385 RVA: 0x0000D044 File Offset: 0x0000B244
            // (set) Token: 0x06000182 RID: 386 RVA: 0x0000D05B File Offset: 0x0000B25B
            public string CameraName { get; set; }

            // Token: 0x1700005B RID: 91
            // (get) Token: 0x06000183 RID: 387 RVA: 0x0000D064 File Offset: 0x0000B264
            // (set) Token: 0x06000184 RID: 388 RVA: 0x0000D07B File Offset: 0x0000B27B
            public string CameraMessage { get; set; }

            // Token: 0x1700005C RID: 92
            // (get) Token: 0x06000185 RID: 389 RVA: 0x0000D084 File Offset: 0x0000B284
            // (set) Token: 0x06000186 RID: 390 RVA: 0x0000D09B File Offset: 0x0000B29B
            public string DeviceMessage { get; set; }

            // Token: 0x1700005D RID: 93
            // (get) Token: 0x06000187 RID: 391 RVA: 0x0000D0A4 File Offset: 0x0000B2A4
            // (set) Token: 0x06000188 RID: 392 RVA: 0x0000D0BB File Offset: 0x0000B2BB
            public string ReconnectMessage { get; set; }

            // Token: 0x1700005E RID: 94
            // (get) Token: 0x06000189 RID: 393 RVA: 0x0000D0C4 File Offset: 0x0000B2C4
            // (set) Token: 0x0600018A RID: 394 RVA: 0x0000D0DB File Offset: 0x0000B2DB
            public FrameCallback FrameCallback { get; set; }

            // Token: 0x040000CD RID: 205
            public ICapturable Camera;

            // Token: 0x040000CE RID: 206
            // TODO: finish device
            //public IControllable Device;
        }
    }
}
