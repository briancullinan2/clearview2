using System;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using System.Windows.Controls.Ribbon;
using EPIC.DataLayer.Extensions;
using EPIC.DataLayer.Commands;

namespace EPIC.DataLayer.Macros
{
	// Token: 0x02000016 RID: 22
	public static class Navigation
	{

        // Token: 0x1700004A RID: 74
        // (get) Token: 0x06000142 RID: 322 RVA: 0x0000B9C0 File Offset: 0x00009BC0
        public static ICommand CloseTabCommand
        {
            get
            {
                return new RelayCommand(new Action<object>(Navigation.CloseTab), null);
            }
        }

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
				//	messageBoxResult = MessageBox.Show("Some items have not been saved, are you sure you want to close?", null, MessageBoxButton.YesNo);
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
