using System;
using System.Globalization;
using System.Windows.Data;
using EPICClearView.Properties;
using EPICClearViewDL.EntityClasses;

namespace EPIC.Utilities.Converters
{
	// Token: 0x02000053 RID: 83
	[ValueConversion(typeof(object), typeof(string))]
	public class DeviceSettingStringConverter : IValueConverter
	{
		// Token: 0x060002C3 RID: 707 RVA: 0x00017194 File Offset: 0x00015394
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			object result;
			if (value is DeviceSettingEntity)
			{
				result = string.Format(Resources.DeviceSettingStringConverter_String, ((DeviceSettingEntity)value).Voltage, ((DeviceSettingEntity)value).Brightness, ((DeviceSettingEntity)value).Gain);
			}
			else
			{
				result = string.Empty;
			}
			return result;
		}

		// Token: 0x060002C4 RID: 708 RVA: 0x000171F6 File Offset: 0x000153F6
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
