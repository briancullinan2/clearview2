using EPIC.MedicalControls.Macros;

namespace EPIC.MedicalControls.Themes.Application
{
    public partial class TabControlStyles
    {

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Navigation.CloseTab(sender);
        }
    }
}
