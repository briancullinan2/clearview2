using EPIC.DeviceInterface.Interfaces;
using FTD2XX_NET;
using log4net;

namespace EPIC.DeviceInterface
{
    /// <summary>
    /// General manager for connecting and disconnecting to devices.
    /// </summary>
    // Token: 0x02000002 RID: 2
    public class DeviceManager : IDisposable
    {
        // Token: 0x14000001 RID: 1
        // (add) Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
        // (remove) Token: 0x06000002 RID: 2 RVA: 0x00002088 File Offset: 0x00000288
        public event DeviceManager.DevicesChanged Changed;

        // Token: 0x17000001 RID: 1
        // (get) Token: 0x06000003 RID: 3 RVA: 0x000020BD File Offset: 0x000002BD
        public static DeviceManager Current
        {
            get
            {
                DeviceManager result;
                if ((result = DeviceManager._instance) == null)
                {
                    result = (DeviceManager._instance = new DeviceManager());
                }
                return result;
            }
        }

        // Token: 0x17000002 RID: 2
        // (get) Token: 0x06000004 RID: 4 RVA: 0x000020D3 File Offset: 0x000002D3
        public List<IControllable>? Devices
        {
            get
            {
                return this._currentDevices;
            }
        }

        // Token: 0x06000005 RID: 5 RVA: 0x000020DC File Offset: 0x000002DC
        private DeviceManager()
        {
            //WMIWatcher.PluggedIn += new WMIWatcher.WMIEventHandler(this.WmiWatcherOnPluggedIn);
            //WMIWatcher.Unplugged += new WMIWatcher.WMIEventHandler(this.WmiWatcherOnPluggedIn);
            Task.Run(new Action(this.GetDevices));
        }

        // Token: 0x06000006 RID: 6 RVA: 0x00002144 File Offset: 0x00000344
        private void ShutdownDevices()
        {
            if (this.Devices == null)
            {
                return;
            }
            foreach (IControllable device in this.Devices)
            {
                device.Close();
            }
        }

        public void Dispose()
        {
            this.ShutdownDevices();
        }


        /*
        // Token: 0x06000007 RID: 7 RVA: 0x000021A0 File Offset: 0x000003A0
        private void WmiWatcherOnPluggedIn(Dictionary<string, object> properties)
        {
            if (properties.ContainsKey("DeviceID") && properties["DeviceID"].ToString().Contains("FTDIBUS"))
            {
                Task.Run(new Action(this.GetDevices));
            }
        }
        */


        // Token: 0x06000008 RID: 8 RVA: 0x00002200 File Offset: 0x00000400
        private void GetDevices()
        {
            FTDI ftdi = new FTDI();
            FTDI.FT_DEVICE_INFO_NODE[] deviceList = new FTDI.FT_DEVICE_INFO_NODE[10];
            if (ftdi.GetDeviceList(deviceList) == null)
            {
                int safety = 10;
                for (; ; )
                {
                    if (!deviceList.Any((FTDI.FT_DEVICE_INFO_NODE x) => x != null && x.LocId == 0U) && safety != 0)
                    {
                        break;
                    }
                    Thread.Sleep(100);
                    ftdi.GetDeviceList(deviceList);
                    safety--;
                }
                List<IControllable> result = this.GetValidDevices(deviceList).ToList<IControllable>();
                List<IControllable> oldCameras = this._currentDevices;
                this._currentDevices = result;
                if (oldCameras != null)
                {
                    List<IControllable> newCameras = this._currentDevices.Except(oldCameras).ToList<IControllable>();
                    List<IControllable> removedCameras = oldCameras.Except(this._currentDevices).ToList<IControllable>();
                    if (newCameras.Any<IControllable>() && removedCameras.Any<IControllable>() && this.Changed != null)
                    {
                        this.Changed(new DeviceManager.DevicesChangedEventArgs
                        {
                            NewCameras = newCameras,
                            OldCameras = removedCameras
                        });
                        return;
                    }
                }
                else if (this.Changed != null)
                {
                    this.Changed(new DeviceManager.DevicesChangedEventArgs
                    {
                        NewCameras = this._currentDevices,
                        OldCameras = null
                    });
                }
            }
        }

        // Token: 0x06000009 RID: 9 RVA: 0x000024E4 File Offset: 0x000006E4
        private IEnumerable<IControllable> GetValidDevices(IEnumerable<FTDI.FT_DEVICE_INFO_NODE> devices)
        {
            foreach (FTDI.FT_DEVICE_INFO_NODE device in devices)
            {
                if (device != null)
                {
                    yield return new HRFirmware
                    {
                        UniqueIdentifier = device.LocId
                    };
                }
            }
            yield break;
        }

        // Token: 0x0600000A RID: 10 RVA: 0x00002508 File Offset: 0x00000708
        public TControllable Connect<TControllable>(uint location) where TControllable : class, IControllable
        {
            TControllable result;
            try
            {
                TControllable device = Activator.CreateInstance(typeof(TControllable), new object[]
                {
                    location
                }) as TControllable;
                result = device;
            }
            catch (Exception e)
            {
                DeviceManager.Log.Error("There was an error connecting to the device.", e);
                result = default(TControllable);
            }
            return result;
        }

        // Token: 0x04000002 RID: 2
        private static readonly ILog Log = LogManager.GetLogger(typeof(DeviceManager));

        // Token: 0x04000003 RID: 3
        private static DeviceManager _instance;

        // Token: 0x04000004 RID: 4
        private List<IControllable> _currentDevices;

        // Token: 0x02000003 RID: 3
        public class DevicesChangedEventArgs
        {
            // Token: 0x17000003 RID: 3
            // (get) Token: 0x0600000D RID: 13 RVA: 0x0000258A File Offset: 0x0000078A
            // (set) Token: 0x0600000E RID: 14 RVA: 0x00002592 File Offset: 0x00000792
            public IEnumerable<IControllable> NewCameras { get; set; }

            // Token: 0x17000004 RID: 4
            // (get) Token: 0x0600000F RID: 15 RVA: 0x0000259B File Offset: 0x0000079B
            // (set) Token: 0x06000010 RID: 16 RVA: 0x000025A3 File Offset: 0x000007A3
            public IEnumerable<IControllable> OldCameras { get; set; }
        }

        // Token: 0x02000004 RID: 4
        // (Invoke) Token: 0x06000013 RID: 19
        public delegate void DevicesChanged(DeviceManager.DevicesChangedEventArgs args);
    }
}
