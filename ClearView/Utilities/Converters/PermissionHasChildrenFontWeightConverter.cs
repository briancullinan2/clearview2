using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using EPICClearViewDL.EntityClasses;

namespace EPIC.Utilities.Converters
{
	// Token: 0x02000055 RID: 85
	public class PermissionHasChildrenFontWeightConverter : IValueConverter
	{
		// Token: 0x060002C9 RID: 713 RVA: 0x00017250 File Offset: 0x00015450
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is DataLayer.Entities.Permission)
			{
				object result;
				if (((DataLayer.Entities.Permission)value).Children.Any<DataLayer.Entities.Permission>())
				{
					result = FontWeights.Bold;
				}
				else
				{
					result = FontWeights.Normal;
				}
				return result;
			}
			throw new NotImplementedException();
		}

		// Token: 0x060002CA RID: 714 RVA: 0x000172A8 File Offset: 0x000154A8
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
