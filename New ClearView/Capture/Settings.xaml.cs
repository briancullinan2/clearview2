using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Markup;
using EPIC.ClearView.Macros;
using EPIC.Resources;
using EPIC.ClearView.Utilities;
using EPIC.ClearView.Utilities.Extensions;
using Xceed.Wpf.Toolkit;
using EPIC.DataLayer.Entities;
using EPIC.DataLayer.Customization;

namespace EPIC.ClearView.Capture
{
	// Token: 0x02000006 RID: 6
	public partial class Settings : Page
	{
		// Token: 0x17000021 RID: 33
		// (get) Token: 0x0600006E RID: 110 RVA: 0x00005418 File Offset: 0x00003618
		// (set) Token: 0x0600006F RID: 111 RVA: 0x0000543A File Offset: 0x0000363A
		public DeviceCalibrationSetting CalibrationSetting
		{
			get
			{
				return (DeviceCalibrationSetting)base.GetValue(Settings.CalibrationSettingProperty);
			}
			set
			{
				base.SetValue(Settings.CalibrationSettingProperty, value);
			}
		}

        // Token: 0x06000070 RID: 112 RVA: 0x000054C8 File Offset: 0x000036C8
        public Settings()
		{
			InitializeComponent();
			Navigation.InsertRibbon(this);
			FormChecker.Events[this.SettingsTab].Changed += this.OnChanged;
			FormChecker.Events[this.SettingsTab].Unchanged += this.OnUnchanged;
            /*
			this.DeviceSelect.Loaded += delegate(object sender, RoutedEventArgs args)
			{
				FormChecker.Events[this.SettingsTab].Include(this.DeviceSelect.FindChild<ComboBox>(null));
			};
			List<Device> list = new LinqMetaData().Device.ToList<Device>();
			foreach (string newItem in (from x in list select x.Camera).Distinct<string>())
			{
				this.CameraSelect.Items.Add(newItem);
			}
			if (CameraManager.Current.Cameras == null)
			{
				CameraManager.Current.Changed += this.AddCurrentCameras;
			}
			else
			{
				this.AddCurrentCameras(null);
			}
			this.Firmware.ItemsSource = from x in Assembly.GetAssembly(typeof(IControllable)).GetTypes()
			where x.GetInterfaces().Contains(typeof(IControllable))
			select x.Name;
			this.DeviceSelect.ItemsSource = list;
			this.DeviceSelect.SelectedItem = ClearViewConfiguration.Current.Device;
			*/
        }

        /*
		// Token: 0x06000071 RID: 113 RVA: 0x00005728 File Offset: 0x00003928
		private void AddCurrentCameras(CameraManager.CamerasChangedEventArgs args)
		{
			this.CameraSelect.Dispatcher.Invoke(delegate()
			{
				foreach (ICapturable capturable in CameraManager.Current.Cameras)
				{
					if (!this.CameraSelect.Items.Contains(capturable.DisplayName))
					{
						this.CameraSelect.Items.Add(capturable.DisplayName);
					}
				}
			});
		}
		*/

        // Token: 0x06000072 RID: 114 RVA: 0x00005748 File Offset: 0x00003948
        private void CameraSelect_SelectionChanged(object sender, SelectionChangedEventArgs selectionChangedEventArgs)
		{
		}

		// Token: 0x06000073 RID: 115 RVA: 0x0000574B File Offset: 0x0000394B
		private void OnChanged(object sender, RoutedEventArgs routedEventArgs)
		{
			this.Save.IsEnabled = true;
		}

		// Token: 0x06000074 RID: 116 RVA: 0x0000575B File Offset: 0x0000395B
		private void OnUnchanged(object sender, RoutedEventArgs routedEventArgs)
		{
			this.Save.IsEnabled = false;
		}

		// Token: 0x06000075 RID: 117 RVA: 0x0000576B File Offset: 0x0000396B
		private void NewDevice_Click(object sender, RoutedEventArgs e)
		{
			this.CreateDevice();
		}

		// Token: 0x06000076 RID: 118 RVA: 0x00005778 File Offset: 0x00003978
		private void CreateDevice()
		{
			//string serialNumber = "TEMP" + (new LinqMetaData().Device.Count((Device x) => x.SerialNumber.StartsWith("TEMP")) + 1).ToString("D3");
			Device deviceEntity = new Device
            {
				UniqueIdentifier = "",
				UidQualifier = 0,
				DeviceState = 1,
				SerialNumber = "",
				DateIssued = DateTime.UtcNow,
				RevisionLevel = "",
				ScansAvailable = 0,
				ScansCompleted = 0,
				LastActivityTime = new DateTime?(DateTime.UtcNow),
				IsDefault = true,
				Firmware = "HRFirmware",
				Camera = "EmguGeneric"
            };
			//deviceEntity.Save();
			ClearViewConfiguration.Current.Device = deviceEntity;
			//this.DeviceSelect.ItemsSource = new LinqMetaData().Device.ToList<Device>();
			this.DeviceSelect.SelectedItem = ClearViewConfiguration.Current.Device;
		}

		// Token: 0x06000077 RID: 119 RVA: 0x00005948 File Offset: 0x00003B48
		private void DeviceSelect_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (e.AddedItems.Count == 0)
			{
				this.DeviceSelect.SelectedItem = e.RemovedItems.OfType<Device>().FirstOrDefault<Device>();
			}
			else
			{
				Device device = e.AddedItems.OfType<Device>().FirstOrDefault<Device>();
				if (device != null)
				{
					IEnumerable<DeviceSetting> itemsSource = from x in Enumerable.Range(0, Math.Max(4, device.Settings.Count()))
					select device.Settings.Skip(x).FirstOrDefault<DeviceSetting>() ?? Settings.DefaultSettings.Skip(x).First<DeviceSetting>();
					this.Configurations.ItemsSource = itemsSource;
					this.CalibrationSetting = (device.CalibrationSettings.FirstOrDefault<DeviceCalibrationSetting>() ?? Settings.DefaultCalibration);
				}
				else
				{
					this.Configurations.ItemsSource = null;
				}
				this.Configurations.Items.Refresh();
			}
		}

		// Token: 0x06000078 RID: 120 RVA: 0x00005A40 File Offset: 0x00003C40
		private void DeviceSelect_TextChanged(object sender, TextChangedEventArgs textChangedEventArgs)
		{
			Device deviceEntity = this.DeviceSelect.SelectedItem as Device;
			if (deviceEntity != null)
			{
				deviceEntity.SerialNumber = this.DeviceSelect.Text;
			}
		}

		// Token: 0x06000079 RID: 121 RVA: 0x00005A7B File Offset: 0x00003C7B
		private void Configurations_LoadingRow(object sender, DataGridRowEventArgs e)
		{
			e.Row.Header = string.Format("Configuration {0}", e.Row.GetIndex() + 1);
		}

		// Token: 0x0600007A RID: 122 RVA: 0x00005AA8 File Offset: 0x00003CA8
		private void Save_Click(object sender, RoutedEventArgs e)
		{
			/*
			using (Transaction transaction = new Transaction(IsolationLevel.ReadCommitted, "setting save"))
			{
				try
				{
					foreach (object obj in ((IEnumerable)this.DeviceSelect.Items))
					{
						Device deviceEntity = (Device)obj;
						if (deviceEntity.IsDefault && !object.Equals(deviceEntity, this.DeviceSelect.SelectedItem))
						{
							deviceEntity.IsDefault = false;
							transaction.Add(deviceEntity);
							deviceEntity.Save();
						}
						else if (object.Equals(deviceEntity, this.DeviceSelect.SelectedItem))
						{
							((Device)this.DeviceSelect.SelectedItem).IsDefault = true;
							transaction.Add((Device)this.DeviceSelect.SelectedItem);
							((Device)this.DeviceSelect.SelectedItem).Save();
							foreach (object obj2 in ((IEnumerable)this.Configurations.Items))
							{
								DeviceSetting deviceSettingEntity = (DeviceSetting)obj2;
								deviceSettingEntity.DeviceId = deviceEntity.DeviceId;
								transaction.Add(deviceSettingEntity);
								deviceSettingEntity.Save();
							}
							this.CalibrationSetting.DeviceId = deviceEntity.DeviceId;
							transaction.Add(this.CalibrationSetting);
							this.CalibrationSetting.Save();
							transaction.Commit();
							FormChecker.Events[this.SettingsTab].Save();
							this.DeviceSelect.ItemsSource = new LinqMetaData().Device;
						}
					}
				}
				catch (Exception ex)
				{
					transaction.Rollback();
					Settings.Log.Error(Capture.Settings_SaveError, ex);
					MessageBox.Show(Application.Current.MainWindow, Capture.Settings_SaveError);
				}
			}
			*/
		}

		// Token: 0x0600007B RID: 123 RVA: 0x00005D40 File Offset: 0x00003F40
		private void SaveClose_Click(object sender, RoutedEventArgs e)
		{
			this.Save_Click(sender, e);
			Navigation.CloseTab(this);
		}

		// Token: 0x0400003D RID: 61
		private static readonly IEnumerable<DeviceSetting> DefaultSettings = new List<DeviceSetting>
		{
			new DeviceSetting
            {
				Frequency = 1100,
				PulseDuration = 500,
				PulseWidth = 14,
				ExposureDelay = 320,
				Voltage = 110,
				Brightness = 80,
				Gain = 56
			},
			new DeviceSetting
            {
				Frequency = 1100,
				PulseDuration = 500,
				PulseWidth = 14,
				ExposureDelay = 320,
				Voltage = 110,
				Brightness = 85,
				Gain = 59
			},
			new DeviceSetting
            {
				Frequency = 1100,
				PulseDuration = 500,
				PulseWidth = 14,
				ExposureDelay = 320,
				Voltage = 110,
				Brightness = 95,
				Gain = 55
			},
			new DeviceSetting
            {
				Frequency = 1100,
				PulseDuration = 500,
				PulseWidth = 14,
				ExposureDelay = 320,
				Voltage = 110,
				Brightness = 90,
				Gain = 60
			}
		};

		// Token: 0x0400003E RID: 62
		private static readonly DeviceCalibrationSetting DefaultCalibration = new DeviceCalibrationSetting
        {
			SigmaRegionOuter = 0.0,
			SigmaRegionInner = 0.0,
			SigmaRegionCorona = 0.0,
			SigmaRegionHighP = 0.0,
			SigmaRegionTotal = 0.0,
			SigmaRegionClump = 0.0,
			ThresholdPercentOuter = 0.0,
			ThresholdPercentInner = 0.0,
			ThresholdPercentCorona = 0.0,
			ThresholdPercentHighP = 0.0,
			ThresholdPercentTotal = 0.0,
			ThresholdPercentClumps = 0.0,
			SigmaMeansOuter = 0.0,
			SigmaMeansInner = 0.0,
			SigmaMeansCorona = 0.0,
			SigmaMeansHighP = 0.0,
			SigmaMeansTotal = 0.0,
			SigmaMeansClumps = 0.0,
			BinDepth = 0
		};

		// Token: 0x0400003F RID: 63
		public static readonly DependencyProperty CalibrationSettingProperty = DependencyProperty.Register("CalibrationSetting", typeof(DeviceCalibrationSetting), typeof(Settings), new PropertyMetadata(DefaultCalibration));
	}
}
