// System.Management is available on Windows; add conditional using if package is present
using EPIC.CameraInterface.Interfaces;
using EPIC.CameraInterface.Utilities;
using OpenCvSharp;
using System.Text.RegularExpressions;
//using Windows.Devices.Enumeration;
//using Windows.Media.Capture;

namespace EPIC.CameraInterface
{
    // Token: 0x02000007 RID: 7
    public class CameraManager : IDisposable
    {
        // Token: 0x14000001 RID: 1
        // (add) Token: 0x0600000F RID: 15 RVA: 0x00002778 File Offset: 0x00000978
        // (remove) Token: 0x06000010 RID: 16 RVA: 0x000027B0 File Offset: 0x000009B0
        public event CameraManager.CamerasChanged? Changed;

        // Token: 0x17000001 RID: 1
        // (get) Token: 0x06000011 RID: 17 RVA: 0x000027E5 File Offset: 0x000009E5
        public static CameraManager Current
        {
            get
            {
                CameraManager result;
                if ((result = CameraManager._instance) == null)
                {
                    result = (CameraManager._instance = new CameraManager());
                }
                return result;
            }
        }

        // Token: 0x17000002 RID: 2
        // (get) Token: 0x06000012 RID: 18 RVA: 0x000027FB File Offset: 0x000009FB
        public List<ICapturable>? Cameras
        {
            get
            {
                return this._currentCameras;
            }
        }

        private bool _isScanning = false;

        // Configurable limit: Most systems won't have more than 4 cameras connected.
        // Scanning is slow (requires opening the device), so keep this reasonable.
        private const int MaxCameraIndexSearch = 8;


        public CameraManager()
        {
            // Initial scan
            Task.Run(() => ScanCameras());
        }

        /// <summary>
        /// Call this from your higher-level library when you detect a hardware change.
        /// </summary>
        public void OnDevicesChanged()
        {
            if (!_isScanning)
            {
                Task.Run(() => ScanCameras());
            }
        }

        private void ScanCameras()
        {
            lock (this)
            {
                if (_isScanning) return;
                _isScanning = true;
            }

            try
            {
                List<ICapturable> foundCameras = new List<ICapturable>();

                // Pure OpenCV approach: "Brute Force" scan indices 0 to N.
                // Note: This can be slow as opening a non-existent camera takes time to timeout.
                for (int i = 0; i < MaxCameraIndexSearch; i++)
                {
                    // We use 'using' to immediately close the test connection
                    // We just want to check if it exists.
                    try
                    {
                        // VideoCapture(i) attempts to open the hardware driver
                        using (var tempCapture = new VideoCapture(i))
                        {
                            if (tempCapture.IsOpened())
                            {
                                // Success: A camera exists at this index.
                                // Since we can't get the registry name via OpenCV easily,
                                // we name it generically.
                                string name = $"Camera {i}";
                                string id = $"opencv_index_{i}";

                                // Create the wrapper
                                var camera = new OpenCvGeneric
                                {
                                    DeviceIndex = i,
                                    DisplayName = name,
                                    UniqueIdentifier = id
                                };

                                foundCameras.Add(camera);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.Debug($"Index {i} skipped or failed: {ex.Message}");
                    }
                }

                UpdateList(foundCameras);
            }
            finally
            {
                _isScanning = false;
            }
        }

        private void UpdateList(List<ICapturable> result)
        {
            var oldCameras = _currentCameras;
            _currentCameras = result;

            if (this.Changed == null)
            {
                return;
            }
            // Determine differences based on UniqueIdentifier (Index)
            var newCameras = result.Where(r => !oldCameras.Any(o => o.UniqueIdentifier.ToString() == r.UniqueIdentifier.ToString())).ToList();
            var removedCameras = oldCameras.Where(o => !result.Any(r => r.UniqueIdentifier.ToString() == o.UniqueIdentifier.ToString())).ToList();

            if (newCameras.Any() || removedCameras.Any())
            {
                this.Changed(new CamerasChangedEventArgs
                {
                    NewCameras = newCameras,
                    OldCameras = removedCameras
                });
            }
            // Determine if this is the very first load
            else if (oldCameras.Count == 0 && result.Count > 0)
            {
                this.Changed(new CamerasChangedEventArgs
                {
                    NewCameras = result,
                    OldCameras = null
                });
            }
        }

        // Token: 0x06000014 RID: 20 RVA: 0x00002910 File Offset: 0x00000B10
        private void ShutdownCameras()
        {
            try
            {
                /*
                if (this._deviceWatcher != null)
                {
                    this._deviceWatcher.Stop();
                    this._deviceWatcher.EventArrived -= null;
                    this._deviceWatcher.Dispose();
                    this._deviceWatcher = null;
                }
                */
            }
            catch
            {
            }
            if (this.Cameras == null)
            {
                return;
            }
            foreach (ICapturable camera in this.Cameras)
            {
                camera.Close();
            }
        }


        public void Dispose()
        {
            this.ShutdownCameras();
        }

        // Token: 0x06000015 RID: 21 RVA: 0x0000296C File Offset: 0x00000B6C
        /*
        private void WmiWatcherOnPluggedIn(Dictionary<string, object> properties)
        {
            if (properties.ContainsKey("Description") && properties["Description"].ToString().Contains("Camera"))
            {
                Task.Run(new Action(this.GetCameras));
            }
        }
        */


        // Token: 0x06000016 RID: 22 RVA: 0x000029BC File Offset: 0x00000BBC
        /*
        private void GetCameras()
        {
            //var devices = await DeviceInformation.FindAllAsync(DeviceClass.VideoCapture);
            List<ICapturable> result = this.GetValidCameras(DirectShowLib.DsDevice.GetDevicesOfCat(DirectShowLib.FilterCategory.VideoInputDevice)).ToList<ICapturable>();
            List<ICapturable> oldCameras = this._currentCameras;
            this._currentCameras = result;
            if (oldCameras != null)
            {
                List<ICapturable> newCameras = this._currentCameras.Except(oldCameras).ToList<ICapturable>();
                List<ICapturable> removedCameras = oldCameras.Except(this._currentCameras).ToList<ICapturable>();
                if (newCameras.Any<ICapturable>() && removedCameras.Any<ICapturable>() && this.Changed != null)
                {
                    this.Changed(new CameraManager.CamerasChangedEventArgs
                    {
                        NewCameras = newCameras,
                        OldCameras = removedCameras
                    });
                    return;
                }
            }
            else if (this.Changed != null)
            {
                this.Changed(new CameraManager.CamerasChangedEventArgs
                {
                    NewCameras = this._currentCameras,
                    OldCameras = null
                });
            }
        }
        */

        // Token: 0x06000017 RID: 23 RVA: 0x00002CFC File Offset: 0x00000EFC
        /*
        private IEnumerable<string> GetPluggedInDevices()
        {
            // Filter by 'Present' status and limit to likely camera categories
            // "SELECT Name, Description FROM Win32_PnPEntity WHERE Present = True"
            var searcher = new ManagementObjectSearcher(
                "SELECT Name, Description FROM Win32_PnPEntity WHERE ConfigManagerErrorCode = 0");

            var devices = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            using (var collection = searcher.Get())
            {
                foreach (ManagementBaseObject obj in collection)
                {
                    var desc = obj["Description"]?.ToString();
                    var name = obj["Name"]?.ToString();

                    if (!string.IsNullOrEmpty(desc)) devices.Add(desc);
                    if (!string.IsNullOrEmpty(name)) devices.Add(name);
                }
            }
            return devices;
        }
        */

        // Token: 0x06000018 RID: 24 RVA: 0x00003178 File Offset: 0x00001378
        /*
        private IEnumerable<ICapturable> GetValidCameras(IEnumerable<DsDevice> devices)
        {
            List<string> pluggedin = this.GetPluggedInDevices().ToList<string>();
            string list = string.Join("\n", pluggedin);
            Log.Debug("Plugged in devices:\n" + list);
            foreach (DsDevice device in devices)
            {
                string deviceName = device.Name;
                try
                {
                    Match match;
                    if (device.DevicePath.StartsWith("@device:sw:") && (match = CameraManager.isGuid.Match(device.DevicePath)).Success)
                    {
                        RegistryKey key = Registry.ClassesRoot.OpenSubKey("CLSID\\" + match.Value);
                        if (key != null)
                        {
                            deviceName = key.GetValue(null).ToString();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Log.Error("Error getting device name.", ex);
                    continue;
                }
                foreach (Type @interface in from y in this._interfaces
                                            orderby y == typeof(EmguGeneric)
                                            select y)
                {
                    ICapturable camera = null;
                    try
                    {
                        ConstructorInfo parameterlessCtor = @interface.GetConstructors(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).Where((ConstructorInfo c) => c.GetParameters().Length == 0).FirstOrDefault<ConstructorInfo>();
                        if (parameterlessCtor == null)
                        {
                            continue;
                        }
                        camera = (ICapturable)parameterlessCtor.Invoke(null);
                        if (camera.Is(deviceName) && pluggedin.Contains(deviceName))
                        {
                            camera.DisplayName = deviceName;
                            camera.UniqueIdentifier = device.DevicePath;
                        }
                        else
                        {
                            camera = null;
                        }
                    }
                    catch (Exception ex2)
                    {
                        Log.Error("Error loading camera list.", ex2);
                    }
                    if (camera != null)
                    {
                        yield return camera;
                    }
                }
            }
            yield break;
        }
        */

        // Token: 0x06000019 RID: 25 RVA: 0x0000319C File Offset: 0x0000139C
        public TCapturable Connect<TCapturable>() where TCapturable : class, ICapturable
        {
            TCapturable result;
            try
            {
                TCapturable camera = Activator.CreateInstance(typeof(TCapturable)) as TCapturable;
                result = camera;
            }
            catch (Exception e)
            {
                Log.Error("There was an error connecting to the device.", e);

                result = default(TCapturable);
            }
            return result;
        }


        // Token: 0x04000006 RID: 6
        private static CameraManager _instance;

        // Token: 0x04000007 RID: 7
        private static Regex isGuid = new Regex("(\\{){0,1}[0-9a-fA-F]{8}\\-[0-9a-fA-F]{4}\\-[0-9a-fA-F]{4}\\-[0-9a-fA-F]{4}\\-[0-9a-fA-F]{12}(\\}){0,1}$", RegexOptions.Compiled);

        // Token: 0x04000008 RID: 8
        private List<ICapturable> _currentCameras;

        // Token: 0x04000009 RID: 9
        private readonly IEnumerable<Type> _interfaces;

        // ManagementEventWatcher to observe plug/unplug events for PnP devices
        //private ManagementEventWatcher? _deviceWatcher;

        // Token: 0x02000008 RID: 8
        public class CamerasChangedEventArgs
        {
            // Token: 0x17000003 RID: 3
            // (get) Token: 0x06000020 RID: 32 RVA: 0x0000321E File Offset: 0x0000141E
            // (set) Token: 0x06000021 RID: 33 RVA: 0x00003226 File Offset: 0x00001426
            public IEnumerable<ICapturable> NewCameras { get; set; }

            // Token: 0x17000004 RID: 4
            // (get) Token: 0x06000022 RID: 34 RVA: 0x0000322F File Offset: 0x0000142F
            // (set) Token: 0x06000023 RID: 35 RVA: 0x00003237 File Offset: 0x00001437
            public IEnumerable<ICapturable> OldCameras { get; set; }
        }

        // Token: 0x02000009 RID: 9
        // (Invoke) Token: 0x06000026 RID: 38
        public delegate void CamerasChanged(CameraManager.CamerasChangedEventArgs args);
    }
}
