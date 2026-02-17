using System.Windows;

namespace EPIC.ClearView.Themes.User
{
    public partial class Add : ResourceDictionary
    {

        // Token: 0x060001B6 RID: 438 RVA: 0x0001151E File Offset: 0x0000F71E
        private void SaveClose_Click(object sender, RoutedEventArgs e)
        {
            //this.Save_Click(sender, e);
            //Navigation.CloseTab(this);
        }

        private void RibbonToggleButton_Unchecked(object sender, RoutedEventArgs e)
        {
            SaveClose_Click(sender, e);
        }
    }
}
