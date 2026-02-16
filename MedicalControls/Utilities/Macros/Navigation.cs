using EPIC.MedicalControls.Utilities.Extensions;
using System.Windows;
using System.Windows.Controls;

namespace EPIC.MedicalControls.Utilities.Macros
{
    public static class Navigation
    {

        // Token: 0x06000143 RID: 323 RVA: 0x0000BA4C File Offset: 0x00009C4C
        public static void CloseTab(object o)
        {
            if (o is FrameworkElement)
            {
                if (!(o is TabItem))
                {
                    o = ((DependencyObject)o).FindAncestor<TabItem>();
                }
                MessageBoxResult messageBoxResult = MessageBoxResult.Yes;
                //if (FormChecker.Events.Keys.Any((FrameworkElement x) => x.GetAncestors().Any((DependencyObject y) => y.Equals(o)) && FormChecker.Events[x].IsChanged))
                //{
                //	messageBoxResult = Xceed.Wpf.Toolkit.MessageBox.Show("Some items have not been saved, are you sure you want to close?", null, MessageBoxButton.YesNo);
                //}
                if (messageBoxResult == MessageBoxResult.Yes)
                {
                    TabControl tabControl = ((TabItem)o).FindAncestor<TabControl>();
                    tabControl.Items.Remove(o);
                }
                return;
            }
            throw new NotImplementedException();
        }

    }
}
