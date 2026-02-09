using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using EPICClearViewDL.EntityClasses;
using EPICClearViewDL.Linq;

namespace EPIC.Utilities.Converters
{
	// Token: 0x02000052 RID: 82
	[ValueConversion(typeof(object), typeof(bool))]
	public class PermissionBooleanConverter : IValueConverter
	{
		// Token: 0x060002BE RID: 702 RVA: 0x00016EBC File Offset: 0x000150BC
		public object Convert(object key, Type targetType, object parameter, CultureInfo culture)
		{
			object result;
			if (StaticEnvironment.IsInDesignMode)
			{
				result = true;
			}
			else
			{
				MainWindow mainWindow = Application.Current.MainWindow as MainWindow;
				UserEntity userEntity = (parameter is UserEntity) ? ((UserEntity)parameter) : ((mainWindow != null) ? mainWindow.User : null);
				if (!string.IsNullOrEmpty((string)key))
				{
					try
					{
						List<RoleEntity> list;
						if (!(parameter is RoleEntity))
						{
							if (userEntity == null)
							{
								list = (from x in new LinqMetaData().Role
								where x.Name == "Anonymous"
								select x).ToList<RoleEntity>();
							}
							else
							{
								list = (from x in userEntity.Roles
								select x.Role).ToList<RoleEntity>();
							}
						}
						else
						{
							list = new List<RoleEntity>
							{
								(RoleEntity)parameter
							};
						}
						List<RoleEntity> source = list;
						if (source.Any((RoleEntity x) => x.Permissions.Any((RolePermissionEntity y) => y.Permission.Name == (string)key)))
						{
							return true;
						}
						if (!new LinqMetaData().Permission.Any((PermissionEntity x) => x.Name == (string)key))
						{
							new PermissionEntity
							{
								Name = key.ToString(),
								Description = ""
							}.Save();
						}
					}
					catch
					{
						return PermissionBooleanConverter.AnonymousPermissions.Contains(key);
					}
				}
				result = false;
			}
			return result;
		}

		// Token: 0x060002BF RID: 703 RVA: 0x00017158 File Offset: 0x00015358
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}

		// Token: 0x04000167 RID: 359
		private static readonly string[] AnonymousPermissions = new string[]
		{
			"Application",
			"Exit"
		};
	}
}
