using EPIC.ClearView.Utilities.Commands;
using EPIC.MedicalControls.Utilities.Extensions;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Ribbon;
using System.Windows.Input;
using System.Windows.Navigation;

namespace EPIC.ClearView.Utilities.Macros
{
    // Token: 0x02000016 RID: 22
    public static class Navigation
    {
        // Token: 0x0600013F RID: 319 RVA: 0x0000B544 File Offset: 0x00009744
        public static void Add(RibbonButton button, string group)
        {
            Application.Current.Dispatcher.BeginInvoke(new Action(delegate ()
            {
                RibbonGroup ribbonGroupBox = ((MainWindow)Application.Current.MainWindow).MainRibbon.FindChild<RibbonGroup>(group);
                ribbonGroupBox.Items.Add(button);
            }), new object[0]);
        }

        // Token: 0x06000140 RID: 320 RVA: 0x0000B74C File Offset: 0x0000994C
        public static void Add(RibbonGroup group, string tab)
        {
            Application.Current.Dispatcher.BeginInvoke(new Action(delegate ()
            {
                if (Application.Current.MainWindow != null)
                {
                    RibbonTab tabItem = ((MainWindow)Application.Current.MainWindow).MainRibbon.FindChild<RibbonTab>(tab);
                    if (tabItem != null)
                    {
                        tabItem.Items.Add(group);
                    }
                    ((INotifyCollectionChanged)((MainWindow)Application.Current.MainWindow).MainRibbon.Items).CollectionChanged += delegate (object? sender, NotifyCollectionChangedEventArgs args)
                    {
                        if (args.NewItems != null && (tabItem = args.NewItems.OfType<RibbonTab>().FirstOrDefault((RibbonTab x) => x.Name == tab)) != null)
                        {
                            tabItem.Items.Add(group);
                        }
                        else if (args.OldItems != null && (tabItem = args.OldItems.OfType<RibbonTab>().FirstOrDefault((RibbonTab x) => x.Name == tab)) != null)
                        {
                            tabItem.Items.Remove(group);
                        }
                    };
                }
            }), new object[0]);
        }

        // Token: 0x06000141 RID: 321 RVA: 0x0000B978 File Offset: 0x00009B78
        public static void Add(TabItem item, string tab)
        {
            Application.Current.Dispatcher.BeginInvoke(new Action(delegate ()
            {
                if (Application.Current.MainWindow != null)
                {
                    TabControl tabControl = Application.Current.MainWindow.FindChild<TabControl>(tab);
                    if (tabControl != null)
                    {
                        tabControl.Items.Add(item);
                    }
                    Action<FrameworkElement> addTab = delegate (FrameworkElement element)
                    {
                        if ((tabControl = element.FindChild<TabControl>(tab)) != null)
                        {
                            tabControl.Items.Add(item);
                        }
                    };
                    Action<FrameworkElement> removeTab = delegate (FrameworkElement element)
                    {
                        if ((tabControl = element.FindChild<TabControl>(tab)) != null)
                        {
                            tabControl.Items.Remove(item);
                        }
                    };
                    Application.Current.LoadCompleted += delegate (object sender, NavigationEventArgs args)
                    {
                        if (args.Content is Page)
                        {
                            ((Page)args.Content).Loaded += delegate (object o, RoutedEventArgs eventArgs)
                            {
                                addTab((Page)o);
                            };
                            ((Page)args.Content).Unloaded += delegate (object o, RoutedEventArgs eventArgs)
                            {
                                removeTab((Page)o);
                            };
                        }
                    };
                }
            }), new object[0]);
        }

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

        // Token: 0x06000144 RID: 324 RVA: 0x0000BDA4 File Offset: 0x00009FA4
        public static void InsertRibbon(FrameworkElement frameworkElement)
        {
            MainWindow? main = Application.Current.MainWindow as MainWindow;
            if (main == null)
            {
                return;
            }
            frameworkElement.Loaded += delegate (object sender, RoutedEventArgs args)
            {
                {

                    IEnumerable<Ribbon> ribbons = [];
                    if (frameworkElement.Resources.Values.OfType<Ribbon>().Any<Ribbon>())
                    {
                        ribbons = ribbons.Concat(frameworkElement.Resources.Values.OfType<Ribbon>());
                    }
                    if (frameworkElement.FindChild<Ribbon>() is Ribbon pageRibbon)
                    {
                        ribbons = ribbons.Concat([pageRibbon]);
                    }
                    foreach (Ribbon ribbon in ribbons)
                    {
                        var temporary = new Collection<Control>();
                        frameworkElement.Resources.Values.OfType<Ribbon>().FirstOrDefault()?.Items.OfType<RibbonTab>()?.FirstOrDefault()?.IsSelected = true;
                        foreach (Control item in ribbon.Items)
                        {
                            temporary.Add(item);
                            item.Tag = ribbon;
                        }

                        foreach (Control item in temporary)
                        {
                            ribbon.Items.Remove(item);
                            main.MainRibbon.Items.Add(item);
                            if (typeof(RibbonTab).IsAssignableFrom(item.GetType()))
                            {
                                (item as RibbonTab)?.IsSelected = true;
                            }
                        }
                    }

                }
                if (frameworkElement.Resources.Values.OfType<IEnumerable<RibbonTab>>().Any())
                {
                    foreach (IEnumerable<RibbonTab> ribbon in frameworkElement.Resources.Values.OfType<IEnumerable<RibbonTab>>())
                    {
                        foreach (Control item in ribbon)
                        {
                            main.MainRibbon.Items.Add(item);
                        }
                    }
                    frameworkElement.Resources.Values.OfType<IEnumerable<RibbonTab>>().FirstOrDefault()?.FirstOrDefault()?.IsSelected = true;
                }

                if (frameworkElement.Resources.Values.OfType<RibbonContextualTabGroup>().Any<RibbonContextualTabGroup>())
                {
                    foreach (RibbonContextualTabGroup item in frameworkElement.Resources.Values.OfType<RibbonContextualTabGroup>())
                    {
                        main.MainRibbon.ContextualTabGroups.Add(item);
                    }
                }
                if (frameworkElement.Resources.Values.OfType<RibbonTab>().Any<RibbonTab>())
                {
                    foreach (RibbonTab item2 in frameworkElement.Resources.Values.OfType<RibbonTab>())
                    {
                        main.MainRibbon.Items.Add(item2);
                    }
                    frameworkElement.Resources.Values.OfType<RibbonTab>().First<RibbonTab>().IsSelected = true;
                }
            };
            frameworkElement.Unloaded += delegate (object sender, RoutedEventArgs e)
            {
                {
                    IEnumerable<Ribbon> ribbons = [];
                    if (frameworkElement.Resources.Values.OfType<Ribbon>().Any<Ribbon>())
                    {
                        ribbons = ribbons.Concat(frameworkElement.Resources.Values.OfType<Ribbon>());
                    }
                    if (frameworkElement.FindChild<Ribbon>() is Ribbon pageRibbon)
                    {
                        ribbons = ribbons.Concat([pageRibbon]);
                    }

                    foreach (Ribbon ribbon in ribbons)
                    {
                        var temporary = new Collection<Control>();
                        foreach (Control item in main.MainRibbon.Items)
                        {
                            if (item.Tag == ribbon)
                            {
                                temporary.Add(item);
                            }
                        }

                        foreach (Control item in temporary)
                        {
                            main.MainRibbon.Items.Remove(item);
                            ribbon.Items.Add(item);
                        }
                    }
                }
                if (frameworkElement.Resources.Values.OfType<IEnumerable<RibbonTab>>().Any())
                {
                    foreach (IEnumerable<RibbonTab> ribbon in frameworkElement.Resources.Values.OfType<IEnumerable<RibbonTab>>())
                    {
                        foreach (Control item in ribbon)
                        {
                            main.MainRibbon.Items.Remove(item);
                        }

                    }
                }
                if (frameworkElement.Resources.Values.OfType<RibbonContextualTabGroup>().Any<RibbonContextualTabGroup>())
                {
                    foreach (RibbonContextualTabGroup item in frameworkElement.Resources.Values.OfType<RibbonContextualTabGroup>())
                    {
                        main.MainRibbon.ContextualTabGroups.Remove(item);
                    }
                }
                if (frameworkElement.Resources.Values.OfType<RibbonTab>().Any<RibbonTab>())
                {
                    foreach (RibbonTab item2 in frameworkElement.Resources.Values.OfType<RibbonTab>())
                    {
                        main.MainRibbon.Items.Remove(item2);
                    }
                }
            };
        }

        // Token: 0x06000145 RID: 325 RVA: 0x0000BEC8 File Offset: 0x0000A0C8
        public static void ShowTab(Uri uri, bool onlyOne = true)
        {
            MainWindow mainWindow = Application.Current.MainWindow as MainWindow;
            if (mainWindow != null)
            {
                TabItem tabItem = mainWindow.Tabs.Items.OfType<TabItem>().FirstOrDefault((TabItem x) => x.Content is Frame && ((Frame)x.Content).Source.OriginalString.Trim(new char[]
                {
                    '/'
                }) == uri.OriginalString.Trim(new char[]
                {
                    '/'
                }));
                if (onlyOne && tabItem != null)
                {
                    tabItem.IsSelected = true;
                }
                else
                {
                    Frame frame = new Frame
                    {
                        Source = uri,
                        NavigationUIVisibility = NavigationUIVisibility.Hidden
                    };
                    TabItem newTab = new TabItem
                    {
                        Content = frame,
                        Header = new Grid(),
                        //HeaderTemplate = mainWindow.TryFindResource("TabClosable") as DataTemplate
                    };
                    frame.LoadCompleted += delegate (object sender, NavigationEventArgs args)
                    {
                        newTab.Header = ((Page)frame.Content).Title;
                    };
                    mainWindow.Tabs.Items.Add(newTab);
                    newTab.IsSelected = true;
                }
            }
        }
    }
}
