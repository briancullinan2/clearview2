using EPIC.DataLayer.Extensions;
using Microsoft.VisualBasic;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Markup;

namespace EPIC.MedicalControls.Utilities
{
    public partial class GeneralTemplate : ResourceDictionary, IStyleConnector
    {

        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var frameworkElement = sender as Hyperlink;
            if (frameworkElement == null) return;

            // 2. The DataContext IS your row's Model/ViewModel
            var selectedEntity = frameworkElement.DataContext as DataLayer.Entities.Permission;
            var Permissions = Utilities.Permissions.IntrospectXaml(assembly, selectedEntity.Baml);
            var PermissionData = (frameworkElement.FindAncestor<DataGrid>()).ItemsSource as Collection;
            foreach (var permission in Permissions)
            {
                PermissionData.Add(permission);
            }

        }

    }
}
