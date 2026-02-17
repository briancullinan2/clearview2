using System.Globalization;
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
                MainWindow mainWindow = System.Windows.Application.Current.MainWindow as MainWindow;
                string permission = values.FirstOrDefault((object x) => x is string) as string;
                /*
				object obj;
				if ((obj = values.FirstOrDefault((object x) => x is DataLayer.Entities.User)) == null)
				{
					obj = ((mainWindow != null) ? mainWindow.User : null);
				}
				DataLayer.Entities.User DataLayer.Entities.User = obj as DataLayer.Entities.User;
				if (!string.IsNullOrEmpty(permission))
				{
					try
					{
						List<DataLayer.Entities.Role> list;
						if (DataLayer.Entities.User == null)
						{
							list = (from x in new LinqMetaData().Role
							where x.Name == "Anonymous"
							select x).ToList<DataLayer.Entities.Role>();
						}
						else
						{
							list = (from x in DataLayer.Entities.User.Roles
							select x.Role).ToList<DataLayer.Entities.Role>();
						}
						List<DataLayer.Entities.Role> source = list;
						if (source.Any((DataLayer.Entities.Role x) => x.Permissions.Any((RoleDataLayer.Entities.Permission y) => y.Permission.Name == permission)))
						{
							return Visibility.Visible;
						}
						if (!new LinqMetaData().Permission.Any((DataLayer.Entities.Permission x) => x.Name == permission))
						{
							new DataLayer.Entities.Permission
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
