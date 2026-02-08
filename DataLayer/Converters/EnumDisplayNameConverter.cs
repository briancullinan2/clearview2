using System;
using System.Globalization;
using System.Windows.Data;
using EPIC.DataLayer.Extensions;

namespace EPIC.DataLayer.Converters
{
	// Token: 0x02000054 RID: 84
	[ValueConversion(typeof(object), typeof(string))]
	public class EnumDisplayNameConverter : IValueConverter
	{
		// Token: 0x060002C6 RID: 710 RVA: 0x00017208 File Offset: 0x00015408
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			object result;
			if (value is Enum)
			{
				result = ((Enum)value).GetDisplayText();
			}
			else
			{
				result = string.Empty;
			}
			return result;
		}

		// Token: 0x060002C7 RID: 711 RVA: 0x0001723E File Offset: 0x0001543E
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
