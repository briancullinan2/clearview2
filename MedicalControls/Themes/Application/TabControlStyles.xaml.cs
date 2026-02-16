using EPIC.MedicalControls.Utilities.Macros;
using System.Windows;
using System.Windows.Markup;

namespace EPIC.MedicalControls.Themes.Application
{
    public partial class TabControlStyles : ResourceDictionary, IStyleConnector
    {

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Navigation.CloseTab(sender);
        }
    }
}
