using EPIC.MedicalControls.Utilities.Extensions;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Markup;

namespace EPIC.MedicalControls.Themes
{
    public partial class GeneralTemplate : ResourceDictionary, IStyleConnector, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            var frameworkElement = sender as Hyperlink;
            if (frameworkElement == null) return;

            // 2. The DataContext IS your row's Model/ViewModel
            var selectedEntity = frameworkElement.DataContext as DataLayer.Entities.Permission;
            var Permissions = Utilities.Permissions.IntrospectXaml(selectedEntity.Assembly, selectedEntity.Baml);
            var DataGrid = (frameworkElement.FindAncestor<DataGrid>());
            var PermissionData = DataGrid.ItemsSource as ICollection<DataLayer.Entities.Permission>;
            foreach (var permission in Permissions)
            {
                PermissionData.Add(permission);
            }

        }

    }
}
