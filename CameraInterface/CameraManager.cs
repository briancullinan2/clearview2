using System;
using System.Collections.Generic;
using System.Linq;
// System.Management is available on Windows; add conditional using if package is present
using System.Management;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows;
using Microsoft.Win32; // Registry
using DirectShowLib;
using log4net;

namespace EPIC.CameraInterface
{
	// Token: 0x02000007 RID: 7
	public class CameraManager
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
		public List<ICapturable> Cameras
		{
			get
			{
				return this._currentCameras;
			}
		}

		// Token: 0x06000013 RID: 19 RVA: 0x00002844 File Offset: 0x00000A44
		private CameraManager()
		{
            // Watch for PnP device instance operation events (plug/unplug) and forward them
            // to the existing WmiWatcherOnPluggedIn handler as a dictionary of properties.
            try
            {
                var query = new WqlEventQuery("SELECT * FROM __InstanceOperationEvent WITHIN 2 WHERE TargetInstance ISA 'Win32_PnPEntity'");
                this._deviceWatcher = new ManagementEventWatcher(query);
                this._deviceWatcher.EventArrived += (s, e) =>
                {
                    try
                    {
                        var target = e.NewEvent["TargetInstance"] as ManagementObject;
                        if (target == null) return;
                        var props = new Dictionary<string, object>();
                        foreach (PropertyData pd in target.Properties)
                        {
                            props[pd.Name] = pd.Value;
                        }
                        this.WmiWatcherOnPluggedIn(props);
                    }
                    catch (Exception ex)
                    {
                        CameraManager.Log.Error("Error handling device arrival event.", ex);
                    }
                };
                this._deviceWatcher.Start();
            }
            catch (Exception)
            {
                // ignore when ManagementEventWatcher isn't available or fails to start
            }
			this._interfaces = from x in Assembly.GetAssembly(typeof(CameraManager)).GetTypes()
			where x.GetInterfaces().Contains(typeof(ICapturable))
			select x;
            // Use ParallelWork.Start.Work if available; otherwise run synchronously
            try
            {
                Application.Current.Dispatcher.Invoke(new Action(this.GetCameras));
            }
            catch
            {
                // fallback to synchronous execution if ParallelWork isn't present
                try { this.GetCameras(); } catch (Exception ex) { CameraManager.Log.Error("There was an error getting the cameras.", ex); }
            }
            // Register application shutdown if WPF Application is available
            try
            {
                Application.Current.Dispatcher.Invoke(delegate()
                {
                    Application.Current.Exit += this.ShutdownCameras;
                });
            }
            catch
            {
                // ignore when not running under WPF host during build or headless execution
            }
		}

		// Token: 0x06000014 RID: 20 RVA: 0x00002910 File Offset: 0x00000B10
        private void ShutdownCameras(object sender, EventArgs eventArgs)
		{
            try
            {
                if (this._deviceWatcher != null)
                {
                    this._deviceWatcher.Stop();
                    this._deviceWatcher.EventArrived -= null;
                    this._deviceWatcher.Dispose();
                    this._deviceWatcher = null;
                }
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

		// Token: 0x06000015 RID: 21 RVA: 0x0000296C File Offset: 0x00000B6C
		private void WmiWatcherOnPluggedIn(Dictionary<string, object> properties)
		{
			if (properties.ContainsKey("Description") && properties["Description"].ToString().Contains("Camera"))
			{
                Application.Current.Dispatcher.Invoke(new Action(this.GetCameras));
			}
		}

		// Token: 0x06000016 RID: 22 RVA: 0x000029BC File Offset: 0x00000BBC
		private void GetCameras()
		{
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

		// Token: 0x06000017 RID: 23 RVA: 0x00002CFC File Offset: 0x00000EFC
		private IEnumerable<string> GetPluggedInDevices()
		{
			using (ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT Name FROM CIM_LogicalDevice"))
			{
				List<ManagementBaseObject> collection = searcher.Get().OfType<ManagementBaseObject>().ToList<ManagementBaseObject>();
				foreach (ManagementObject obj in collection)
				{
					string? name;
					try
					{
						name = obj.GetPropertyValue("Description")?.ToString();
					}
					catch (ManagementException)
					{
						try
						{
                            name = obj.GetPropertyValue("Name")?.ToString();
                        }
                        catch ( Exception e )
						{
							continue;
						}
					}
					if (!string.IsNullOrEmpty(name))
					{
						yield return name;
					}
				}
			}
			yield break;
		}

		// Token: 0x06000018 RID: 24 RVA: 0x00003178 File Offset: 0x00001378
		private IEnumerable<ICapturable> GetValidCameras(IEnumerable<DsDevice> devices)
		{
			List<string> pluggedin = this.GetPluggedInDevices().ToList<string>();
			string list = string.Join("\n", pluggedin);
			CameraManager.Log.Debug("Plugged in devices:\n" + list);
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
					CameraManager.Log.Error("Error getting device name.", ex);
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
						CameraManager.Log.Error("Error loading camera list.", ex2);
					}
					if (camera != null)
					{
						yield return camera;
					}
				}
			}
			yield break;
		}

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
				CameraManager.Log.Error("There was an error connecting to the device.", e);

                result = default(TCapturable);
			}
			return result;
		}

        // Token: 0x04000005 RID: 5
        private static readonly ILog Log = log4net.LogManager.GetLogger(typeof(CameraManager));

		// Token: 0x04000006 RID: 6
		private static CameraManager _instance;

		// Token: 0x04000007 RID: 7
		private static Regex isGuid = new Regex("(\\{){0,1}[0-9a-fA-F]{8}\\-[0-9a-fA-F]{4}\\-[0-9a-fA-F]{4}\\-[0-9a-fA-F]{4}\\-[0-9a-fA-F]{12}(\\}){0,1}$", RegexOptions.Compiled);

		// Token: 0x04000008 RID: 8
		private List<ICapturable> _currentCameras;

		// Token: 0x04000009 RID: 9
		private readonly IEnumerable<Type> _interfaces;

		// ManagementEventWatcher to observe plug/unplug events for PnP devices
		private ManagementEventWatcher? _deviceWatcher;

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
