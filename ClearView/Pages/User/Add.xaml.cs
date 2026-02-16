using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using EPIC.ClearView.Utilities.Macros;
using EPIC.DataLayer;

namespace EPIC.ClearView.User
{
	// Token: 0x02000048 RID: 72
	public partial class Add : Page
	{
		// Token: 0x17000086 RID: 134
		// (get) Token: 0x06000260 RID: 608 RVA: 0x00013FD4 File Offset: 0x000121D4
		// (set) Token: 0x06000261 RID: 609 RVA: 0x00013FF6 File Offset: 0x000121F6
		public DataLayer.Entities.User User
		{
			get
			{
				return (DataLayer.Entities.User)base.GetValue(Add.UserProperty);
			}
			set
			{
				base.SetValue(Add.UserProperty, value);
			}
		}

		// Token: 0x06000262 RID: 610 RVA: 0x00014044 File Offset: 0x00012244
		public Add()
		{
			this.InitializeComponent();
			Navigation.InsertRibbon(this);
			base.Loaded += delegate(object sender2, RoutedEventArgs args2)
			{
				//FormChecker.Events[this].Changed += this.OnChanged;
				//FormChecker.Events[this].Unchanged += this.OnUnchanged;
			};
		}

		// Token: 0x06000263 RID: 611 RVA: 0x0001407D File Offset: 0x0001227D
		private void OnChanged(object sender, RoutedEventArgs routedEventArgs)
		{
			this.Save.IsEnabled = true;
		}

		// Token: 0x06000264 RID: 612 RVA: 0x0001408D File Offset: 0x0001228D
		private void OnUnchanged(object sender, RoutedEventArgs routedEventArgs)
		{
			this.Save.IsEnabled = false;
		}

		// Token: 0x06000265 RID: 613 RVA: 0x00014140 File Offset: 0x00012340
		private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
		{
			if (e.NewSize.Width < 400.0 || e.NewSize.Height < 400.0)
			{
				if (((Grid)base.Content).Children.OfType<WrapPanel>().Any<WrapPanel>())
				{
					TabControl tabControl = new TabControl
					{
						TabStripPlacement = Dock.Left
					};
					((Grid)base.Content).Children.OfType<WrapPanel>().First<WrapPanel>().Children.OfType<GroupBox>().ToList<GroupBox>().ForEach(delegate(GroupBox x)
					{
						tabControl.Items.Add(new TabItem
						{
							Header = x.Header,
							Content = x.Content
						});
					});
					((Grid)base.Content).Children.Remove(((Grid)base.Content).Children.OfType<WrapPanel>().First<WrapPanel>());
					((Grid)base.Content).Children.Add(tabControl);
				}
			}
			else if (((Grid)base.Content).Children.OfType<TabControl>().Any<TabControl>())
			{
				WrapPanel wrapPanel = new WrapPanel
				{
					HorizontalAlignment = HorizontalAlignment.Left,
					VerticalAlignment = VerticalAlignment.Top,
					Orientation = Orientation.Horizontal
				};
				((Grid)base.Content).Children.OfType<TabControl>().First<TabControl>().Items.OfType<TabItem>().ToList<TabItem>().ForEach(delegate(TabItem x)
				{
					wrapPanel.Children.Add(new GroupBox
					{
						Header = x.Header,
						Content = x.Content,
						VerticalAlignment = VerticalAlignment.Top,
						HorizontalAlignment = HorizontalAlignment.Left
					});
				});
				((Grid)base.Content).Children.Remove(((Grid)base.Content).Children.OfType<TabControl>().First<TabControl>());
				((Grid)base.Content).Children.Add(wrapPanel);
			}
		}

		// Token: 0x06000266 RID: 614 RVA: 0x0001433F File Offset: 0x0001253F
		private void Save_Click(object sender, RoutedEventArgs e)
		{
			//this.User.Save();
		}

		// Token: 0x06000267 RID: 615 RVA: 0x0001434E File Offset: 0x0001254E
		private void SaveClose_Click(object sender, RoutedEventArgs e)
		{
			this.Save_Click(sender, e);
			Navigation.CloseTab(this);
		}

		// Token: 0x04000140 RID: 320
		public static readonly DependencyProperty UserProperty = DependencyProperty.Register("User", typeof(DataLayer.Entities.User), typeof(Add), new PropertyMetadata(new DataLayer.Entities.User()));

        private void RibbonToggleButton_Unchecked(object sender, RoutedEventArgs e)
        {
			Navigation.CloseTab(this);
        }
    }
}
