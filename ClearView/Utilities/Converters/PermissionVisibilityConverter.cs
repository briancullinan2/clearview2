using EPIC.ClearView;
using EPIC.ClearView.Utilities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace EPIC.ClearView.Utilities.Converters
{
	// Token: 0x02000056 RID: 86
	[ValueConversion(typeof(object), typeof(Visibility))]
	public class PermissionVisibilityConverter : IMultiValueConverter
	{
		// Token: 0x060002CC RID: 716 RVA: 0x00017360 File Offset: 0x00015560
		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
		{
			object result;
			if (StaticEnvironment.IsInDesignMode)
			{
				result = Visibility.Visible;
			}
			else
			{
				MainWindow mainWindow = Application.Current.MainWindow as MainWindow;
				string permission = values.FirstOrDefault((object x) => x is string) as string;
                /*
				object obj;
				if ((obj = values.FirstOrDefault((object x) => x is UserEntity)) == null)
				{
					obj = ((mainWindow != null) ? mainWindow.User : null);
				}
				UserEntity userEntity = obj as UserEntity;
				if (!string.IsNullOrEmpty(permission))
				{
					try
					{
						List<RoleEntity> list;
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
						List<RoleEntity> source = list;
						if (source.Any((RoleEntity x) => x.Permissions.Any((RolePermissionEntity y) => y.Permission.Name == permission)))
						{
							return Visibility.Visible;
						}
						if (!new LinqMetaData().Permission.Any((PermissionEntity x) => x.Name == permission))
						{
							new PermissionEntity
							{
								Name = permission,
								Description = ""
							}.Save();
						}
					}
					catch
					{
						return PermissionVisibilityConverter.AnonymousPermissions.Contains(permission) ? Visibility.Visible : Visibility.Collapsed;
					}
				}
				*/
                result = Visibility.Collapsed;
			}
			return result;
		}

		// Token: 0x060002CD RID: 717 RVA: 0x0001760C File Offset: 0x0001580C
		public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}

		// Token: 0x04000169 RID: 361
		private static readonly string[] AnonymousPermissions = new string[]
		{
			"Application",
			"Exit"
		};
	}
}
