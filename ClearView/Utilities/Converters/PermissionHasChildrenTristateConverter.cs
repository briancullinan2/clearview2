using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace EPIC.Utilities.Converters
{
	// Token: 0x02000050 RID: 80
	public class PermissionHasChildrenTristateConverter : IValueConverter
	{
		// Token: 0x060002B8 RID: 696 RVA: 0x00016BF0 File Offset: 0x00014DF0
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is DataLayer.Entities.Permission)
			{
				object result;
				// TODO: use reflection to select data layer assemblies and then search for ADO compatible entities
				if (((DataLayer.Entities.Permission)value).Children.Any<DataLayer.Entities.Permission>())
				{
					result = true;
				}
				else
				{
					result = false;
				}
				return result;
			}
			throw new NotImplementedException();
		}

		// Token: 0x060002B9 RID: 697 RVA: 0x00016C40 File Offset: 0x00014E40
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
