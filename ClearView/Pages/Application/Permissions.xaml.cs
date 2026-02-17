using EPIC.ClearView.Utilities.Macros;
using EPIC.MedicalControls.Utilities.Extensions;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;

namespace EPIC.ClearView.Pages.Application
{
    // Token: 0x0200004C RID: 76
    public partial class Permissions : Page
    {
        // Token: 0x0600028D RID: 653 RVA: 0x0001519C File Offset: 0x0001339C
        public Permissions()
        {
            ViewModel = new MedicalControls.Utilities.DesignTimePermissionsViewModel()
            {
                Page = this
            };
            DataContext = ViewModel;

            this.InitializeComponent();

            Navigation.InsertRibbon(this);
            base.Loaded += delegate (object sender2, RoutedEventArgs args2)
            {
                //FormChecker.Events[this].Changed += this.OnChanged;
                //FormChecker.Events[this].Unchanged += this.OnUnchanged;
            };


            // we specify the memory data context here because we know the information will be synchronized before close.
            //   this will be a standard formchecker.xaml function
            ViewModel.RolesData.CollectionChanged += this.OnRoleCollectionChanged;
            var view = CollectionViewSource.GetDefaultView(ViewModel.RolesData);
            view.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));

            AddMissingRoles();

            // start with xaml level permissions
            // Do this after setting your ItemsSource
            var view2 = CollectionViewSource.GetDefaultView(ViewModel.PermissionData);
            view2.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));

            AddInitialRoleColumns();

            AddXamlPermissions();
        }



        // TODO: make this a permission for admins to turn off
        public void AddMissingRoles()
        {

            if (ViewModel.RolesData.Count == 0)
            {
                ViewModel.RolesData.Add(new DataLayer.Entities.Role
                {
                    Name = "Admin",
                    Description = "General administrator, full control"
                });
                ViewModel.RolesData.Add(new DataLayer.Entities.Role
                {
                    Name = "Client",
                    Description = "General client, like a doctor or nurse"
                });
                ViewModel.RolesData.Add(new DataLayer.Entities.Role
                {
                    Name = "Tech",
                    Description = "General technician, device certified"
                });
                ViewModel.RolesData.Add(new DataLayer.Entities.Role
                {
                    Name = "Guest",
                    Description = "General guest, for emergent use"
                });
            }

        }


        public void AddInitialRoleColumns()
        {
            foreach (object obj in this.ViewModel.RolesData)
            {
                DataLayer.Entities.Role role = (DataLayer.Entities.Role)obj;
                this.AddColumn(role);
            }
        }


        public void AddXamlPermissions()
        {
            var assembly = typeof(App).Assembly;
            var _bamls = MedicalControls.Utilities.Permissions.GetBamlFiles(assembly);

            foreach (string bamlPath in _bamls)
            {
                var name = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(Path.GetFileNameWithoutExtension(bamlPath));
                string defaultNamespace = string.Join(".", (Path.GetDirectoryName(bamlPath) ?? "").Replace('\\', '/')
                                                .Split('/')
                                                .Select(s => System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(s)));

                ViewModel.PermissionData.Add(new DataLayer.Entities.Permission
                {
                    Assembly = assembly,
                    IsPageAccess = true,
                    Simplified = (!String.IsNullOrWhiteSpace(defaultNamespace)
                            ? (defaultNamespace + ".") : "") + name,
                    Baml = bamlPath,
                    Name = "Pages." + typeof(App).Namespace + (!String.IsNullOrWhiteSpace(defaultNamespace)
                            ? ("." + defaultNamespace) : "") + "." + name + ".Access",
                    Description = "Page access to " + name + ".baml in the " + (String.IsNullOrWhiteSpace(defaultNamespace)
                            ? "top" : defaultNamespace) + " file layer"
                });
            }
        }


        // TODO: add fancy as fuck assembly level introspection with reflection to automatically generate permissions sets for:
        //    * every actionable control, buttons, texts, headers, closes, ribbons, etc
        //    * tab ownership and sharing, login switching, hiding panels but leaving data open
        //    * automatically collapsing or hiding elements based on permissions
        //    * database row level permissions, tech can add scans but not mailing address, this group can access these patients
        //    * content sharing permissions, reporting, syncronizing, data sharing signatures

        /*

        }
        */

        /*
        ((ObservableCollection<DataLayer.Entities.Role>)this.Roles.ItemsSource).CollectionChanged += this.OnRoleCollectionChanged;
        this._permissionConverter = new PermissionBooleanConverter();
        FrameworkElementFactory frameworkElementFactory = new FrameworkElementFactory(typeof(Label));
        frameworkElementFactory.SetValue(ContentControl.ContentProperty, new Binding("Description")
        {
            Mode = BindingMode.TwoWay,
            UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
        });
        frameworkElementFactory.SetValue(Control.FontWeightProperty, new Binding
        {
            Converter = new PermissionHasChildrenFontWeightConverter()
        });
        frameworkElementFactory.SetValue(ToggleButton.IsThreeStateProperty, new Binding
        {
            Converter = new PermissionHasChildrenTristateConverter()
        });
        this.PermissionsGrid.Columns.Add(new DataGridTemplateColumn
        {
            Header = "Description",
            CellTemplate = new DataTemplate
            {
                VisualTree = frameworkElementFactory
            }
        });
        this.PermissionsGrid.ItemsSource = from x in new LinqMetaData().Permission
        where x.Description != ""
        orderby x.ParentId, x.ParentId == (decimal?)x.PermissionId descending, x.Description
        select x;
        */

        // Token: 0x0600028E RID: 654 RVA: 0x000154DC File Offset: 0x000136DC
        private void AddColumn(DataLayer.Entities.Role role, bool canResize = false)
        {
            var col = this.PermissionsGrid.Columns.FirstOrDefault((DataGridColumn x) => x.Header.ToString() == role.Name);
            if (col != null)
            {
                return;
            }
            FrameworkElementFactory frameworkElementFactory;
            DataTemplate template;
            if (role.Name == "Description")
            {
                frameworkElementFactory = new FrameworkElementFactory(typeof(Label));
                template = new DataTemplate { VisualTree = frameworkElementFactory };
            }
            else if (role.Name == "Name")
            {
                frameworkElementFactory = new FrameworkElementFactory(typeof(Label));
                template = new DataTemplate { VisualTree = frameworkElementFactory };
            }
            else
            {
                frameworkElementFactory = new FrameworkElementFactory(typeof(CheckBox));
                template = new DataTemplate { VisualTree = frameworkElementFactory };
            }

            if (role.Name == "Name")
            {
                frameworkElementFactory.SetValue(Label.ContentProperty, new Binding(role.Name)
                {
                    Mode = BindingMode.TwoWay,
                    UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                });
                frameworkElementFactory.SetValue(FrameworkElement.HorizontalAlignmentProperty, HorizontalAlignment.Left);
                frameworkElementFactory.SetValue(FrameworkElement.VerticalAlignmentProperty, VerticalAlignment.Top);
            }
            else if (role.Name != "Description")
            {
                frameworkElementFactory.SetValue(FrameworkElement.HorizontalAlignmentProperty, HorizontalAlignment.Center);
                frameworkElementFactory.SetValue(FrameworkElement.VerticalAlignmentProperty, VerticalAlignment.Center);
            }
            //frameworkElementFactory.SetValue(ToggleButton.IsCheckedProperty, new Binding("Name")
            //{
            //    Mode = BindingMode.OneWay,
            //Converter = this._permissionConverter,
            //ConverterParameter = role
            //});
            frameworkElementFactory.AddHandler(ToggleButton.CheckedEvent, new RoutedEventHandler(this.PermissionChecked));
            frameworkElementFactory.AddHandler(ToggleButton.UncheckedEvent, new RoutedEventHandler(this.PermissionChecked));
            this.PermissionsGrid.Columns.Add(new DataGridTemplateColumn
            {
                Width = role.Name == "Name" || role.Name == "Description" ? 300.0 : 150.0,
                Header = role.Name,
                CanUserSort = canResize,
                CanUserResize = canResize,
                CellTemplate = template
            });
        }

        // Token: 0x0600028F RID: 655 RVA: 0x000156EC File Offset: 0x000138EC
        private void PermissionChecked(object sender, RoutedEventArgs args)
        {
            if (!this._suppressChecking)
            {
                /*
				this._suppressChecking = true;
				CheckBox checkBox = sender as CheckBox;
				if (checkBox != null)
				{
					DataGridCell dataGridCell = checkBox.FindAncestor<DataGridCell>();
					if (dataGridCell != null)
					{
						DataLayer.Entities.Permission DataLayer.Entities.Permission = dataGridCell.DataContext as DataLayer.Entities.Permission;
						if (DataLayer.Entities.Permission != null)
						{
							int num = this.PermissionsGrid.Columns.IndexOf(dataGridCell.Column);
							DataLayer.Entities.Role role = this._permissions.Keys.ToList<DataLayer.Entities.Role>()[num - 1];
							this._permissions[role][DataLayer.Entities.Permission] = (checkBox.IsChecked ?? false);
							if (DataLayer.Entities.Permission.ParentId != null && DataLayer.Entities.Permission.ParentId != DataLayer.Entities.Permission.PermissionId)
							{
								List<DataLayer.Entities.Permission> source = DataLayer.Entities.Permission.Parent.Children.Except(new DataLayer.Entities.Permission[]
								{
									DataLayer.Entities.Permission.Parent
								}).ToList<DataLayer.Entities.Permission>();
								CheckBox checkFromEntity = this.GetCheckFromEntity(DataLayer.Entities.Permission.Parent, num);
								if (checkFromEntity != null)
								{
									if (source.All((DataLayer.Entities.Permission x) => this._permissions[role].ContainsKey(x) && this._permissions[role][x]))
									{
										checkFromEntity.IsChecked = new bool?(true);
									}
									else if (source.All((DataLayer.Entities.Permission x) => !this._permissions[role].ContainsKey(x) || !this._permissions[role][x]))
									{
										checkFromEntity.IsChecked = new bool?(false);
									}
									else
									{
										checkFromEntity.IsChecked = null;
									}
								}
							}
							if (DataLayer.Entities.Permission.Children.Any<DataLayer.Entities.Permission>())
							{
								foreach (CheckBox checkBox2 in from child in DataLayer.Entities.Permission.Children.Except(new DataLayer.Entities.Permission[]
								{
									DataLayer.Entities.Permission
								})
								select this.GetCheckFromEntity(child, this._permissions.Keys.ToList<DataLayer.Entities.Role>().IndexOf(role) + 1) into child
								where child != null
								select child)
								{
									checkBox2.IsChecked = new bool?(checkBox.IsChecked ?? false);
								}
								foreach (DataLayer.Entities.Permission key in DataLayer.Entities.Permission.Children.Except(new DataLayer.Entities.Permission[]
								{
									DataLayer.Entities.Permission
								}))
								{
									this._permissions[role][key] = (checkBox.IsChecked ?? false);
								}
							}
							this._suppressChecking = false;
						}
					}
				}
				*/
            }
        }

        // Token: 0x06000290 RID: 656 RVA: 0x00015A88 File Offset: 0x00013C88
        private CheckBox GetCheckFromEntity(DataLayer.Entities.Permission child, int column)
        {
            /*
			DataGridRow dataGridRow = this.PermissionsGrid.ItemContainerGenerator.ContainerFromItem(child) as DataGridRow;
			if (dataGridRow != null)
			{
				DataGridCell dataGridCell = dataGridRow.FindChild(null).ItemContainerGenerator.ContainerFromIndex(column) as DataGridCell;
				if (dataGridCell != null)
				{
					return dataGridCell.FindChild(null);
				}
			}
			*/
            return null;
        }

        // Token: 0x06000291 RID: 657 RVA: 0x00015B1C File Offset: 0x00013D1C
        private void RemoveColumn(DataLayer.Entities.Role role)
        {
            var col = this.PermissionsGrid.Columns.FirstOrDefault((DataGridColumn x) => x.Header.ToString() == role.Name);
            if (col == null)
            {
                return;
            }
            var columns = this.PermissionsGrid.Columns.Where(x => x != col);
            this.PermissionsGrid.Columns.Clear();
            PermissionsGrid.ItemsSource = null;
            Dispatcher.BeginInvoke(new Action(() =>
            {
                this.AddColumn(new DataLayer.Entities.Role { Name = "Name" }, canResize: true);
                this.AddColumn(new DataLayer.Entities.Role { Name = "Description" }, canResize: true);
                foreach (object obj in this.ViewModel.RolesData)
                {
                    DataLayer.Entities.Role role = (DataLayer.Entities.Role)obj;
                    this.AddColumn(role);
                }
                PermissionsGrid.ItemsSource = ViewModel.PermissionData;
                PermissionsGrid.UpdateLayout();

            }), System.Windows.Threading.DispatcherPriority.Loaded);
        }

        // Token: 0x06000292 RID: 658 RVA: 0x00015B68 File Offset: 0x00013D68
        private void OnRoleCollectionChanged(object? sender, NotifyCollectionChangedEventArgs args)
        {
            if (args.NewItems != null)
            {
                foreach (object obj in args.NewItems)
                {
                    DataLayer.Entities.Role role = (DataLayer.Entities.Role)obj;
                    this.AddColumn(role);
                }
            }
            if (args.OldItems != null)
            {
                foreach (object obj2 in args.OldItems)
                {
                    DataLayer.Entities.Role role = (DataLayer.Entities.Role)obj2;
                    this.RemoveColumn(role);
                }
            }
            this._collectionChanged = true;
            //this.Save.IsEnabled = true;
        }

        // Token: 0x06000293 RID: 659 RVA: 0x00015C58 File Offset: 0x00013E58
        private void OnChanged(object sender, RoutedEventArgs routedEventArgs)
        {
            //this.Save.IsEnabled = true;
        }

        // Token: 0x06000294 RID: 660 RVA: 0x00015C68 File Offset: 0x00013E68
        private void OnUnchanged(object sender, RoutedEventArgs routedEventArgs)
        {
            if (!this._collectionChanged)
            {
                //this.Save.IsEnabled = false;
            }
        }

        // Token: 0x06000295 RID: 661 RVA: 0x00015C92 File Offset: 0x00013E92
        private void AddRole_Click(object sender, RoutedEventArgs e)
        {
            this.RolesTab.IsSelected = true;
            ((ObservableCollection<DataLayer.Entities.Role>)this.Roles.ItemsSource).Add(new DataLayer.Entities.Role());
        }

        // Token: 0x06000296 RID: 662 RVA: 0x00015CC0 File Offset: 0x00013EC0
        private void RemoveRole_Click(object sender, RoutedEventArgs e)
        {
            Button? button = sender as Button;
            if (button != null)
            {
                DataGridCell dataGridCell = button.FindAncestor<DataGridCell>();
                DataLayer.Entities.Role? Role = dataGridCell.DataContext as DataLayer.Entities.Role;
                if (Role != null)
                {
                    ((ObservableCollection<DataLayer.Entities.Role>)this.Roles.ItemsSource).Remove(Role);
                    return;
                }
            }
            throw new NotImplementedException();
        }

        // Token: 0x06000297 RID: 663 RVA: 0x00015DBC File Offset: 0x00013FBC
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            /*
			using (Transaction transaction = new Transaction(IsolationLevel.ReadCommitted, "edit permissions"))
			{
				try
				{
					foreach (KeyValuePair<DataLayer.Entities.Role, Dictionary<DataLayer.Entities.Permission, bool>> keyValuePair in this._permissions.ToList<KeyValuePair<DataLayer.Entities.Role, Dictionary<DataLayer.Entities.Permission, bool>>>())
					{
						transaction.Add(keyValuePair.Key);
						if (!((ObservableCollection<DataLayer.Entities.Role>)this.Roles.ItemsSource).Contains(keyValuePair.Key))
						{
							this._permissions.Remove(keyValuePair.Key);
							keyValuePair.Key.Permissions.DeleteMulti();
							keyValuePair.Key.Delete();
						}
						else
						{
							using (Dictionary<DataLayer.Entities.Permission, bool>.Enumerator enumerator2 = keyValuePair.Value.GetEnumerator())
							{
								while (enumerator2.MoveNext())
								{
									KeyValuePair<DataLayer.Entities.Permission, bool> permission = enumerator2.Current;
									bool flag;
									if (keyValuePair.Key.Permissions.Any(delegate(RoleDataLayer.Entities.Permission x)
									{
										decimal permissionId2 = x.PermissionId;
										KeyValuePair<DataLayer.Entities.Permission, bool> permission2 = permission;
										return permissionId2 == permission2.Key.PermissionId;
									}))
									{
										KeyValuePair<DataLayer.Entities.Permission, bool> permission3 = permission;
										flag = permission3.Value;
									}
									else
									{
										flag = true;
									}
									if (flag)
									{
										if (!keyValuePair.Key.Permissions.All(delegate(RoleDataLayer.Entities.Permission x)
										{
											decimal permissionId2 = x.PermissionId;
											KeyValuePair<DataLayer.Entities.Permission, bool> permission2 = permission;
											return permissionId2 != permission2.Key.PermissionId;
										}))
										{
											goto IL_1F2;
										}
										KeyValuePair<DataLayer.Entities.Permission, bool> permission3 = permission;
										if (!permission3.Value)
										{
											goto IL_1F2;
										}
										permission3 = permission;
										decimal permissionId = permission3.Key.PermissionId;
										permission3 = permission;
										bool flag2 = !(permissionId != permission3.Key.ParentId);
										IL_1F3:
										if (!flag2)
										{
											RoleDataLayer.Entities.Permission roleDataLayer.Entities.Permission = keyValuePair.Key.Permissions.AddNew();
											RoleDataLayer.Entities.Permission roleDataLayer.Entities.Permission2 = roleDataLayer.Entities.Permission;
											permission3 = permission;
											roleDataLayer.Entities.Permission2.PermissionId = permission3.Key.PermissionId;
											continue;
										}
										continue;
										IL_1F2:
										flag2 = true;
										goto IL_1F3;
									}
									RoleDataLayer.Entities.Permission roleDataLayer.Entities.Permission3 = keyValuePair.Key.Permissions.First(delegate(RoleDataLayer.Entities.Permission x)
									{
										decimal permissionId2 = x.PermissionId;
										KeyValuePair<DataLayer.Entities.Permission, bool> permission2 = permission;
										return permissionId2 == permission2.Key.PermissionId;
									});
									keyValuePair.Key.Permissions.Remove(roleDataLayer.Entities.Permission3);
									transaction.Add(roleDataLayer.Entities.Permission3);
									roleDataLayer.Entities.Permission3.Delete();
								}
							}
							if (((ObservableCollection<DataLayer.Entities.Role>)this.Roles.ItemsSource).Contains(keyValuePair.Key))
							{
								keyValuePair.Key.Save(true);
							}
						}
					}
					this._collectionChanged = false;
					FormChecker.Events[this].Save();
					transaction.Commit();
				}
				catch (Exception ex)
				{
					transaction.Rollback();
					Permissions.Log.Error("There was an error saving the pemrissions.", ex);
				}
			}
			MainWindow mainWindow = Application.Current.MainWindow as MainWindow;
			if (mainWindow != null)
			{
				DataLayer.Entities.User user = mainWindow.User;
				mainWindow.User = new LinqMetaData().User.First((DataLayer.Entities.User x) => x.UserId == user.UserId);
			}
			*/
        }

        // Token: 0x06000298 RID: 664 RVA: 0x000161E8 File Offset: 0x000143E8
        private void Name_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox? textBox = sender as TextBox;
            if (textBox != null)
            {
                DataGridCell dataGridCell = textBox.FindAncestor<DataGridCell>();
                if (dataGridCell != null)
                {
                    DataLayer.Entities.Role Role = dataGridCell.DataContext as DataLayer.Entities.Role;
                    if (Role != null)
                    {
                        int index = ViewModel.RolesData.IndexOf(Role);
                        this.PermissionsGrid.Columns[index + 2].Header = textBox.Text;
                    }
                }
            }
        }

        /*
        // Token: 0x0600029B RID: 667 RVA: 0x00016370 File Offset: 0x00014570
        [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DebuggerNonUserCode]
        void IStyleConnector.Connect(int connectionId, object target)
        {
            switch (connectionId)
            {
                case 4:
                    ((TextBox)target).TextChanged += this.Name_TextChanged;
                    break;
                case 5:
                    ((Button)target).Click += this.RemoveRole_Click;
                    break;
            }
        }
        */

        // Token: 0x04000155 RID: 341
        //private readonly PermissionBooleanConverter _permissionConverter;

        // Token: 0x04000157 RID: 343
        private bool _collectionChanged;

        // Token: 0x04000158 RID: 344
        private bool _suppressChecking;

        public MedicalControls.Utilities.DesignTimePermissionsViewModel ViewModel { get; private set; }

        private void RibbonToggleButton_Unchecked(object sender, RoutedEventArgs e)
        {
            Navigation.CloseTab(this);
        }

        private void Search_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Hyperlink_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {

        }

        /*
        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var frameworkElement = sender as Hyperlink;
            if (frameworkElement == null) return;

            // 2. The DataContext IS your row's Model/ViewModel
            var selectedEntity = frameworkElement.DataContext as VirtualPermission;
            var Permissions = Utilities.Permissions.IntrospectXaml(assembly, selectedEntity.Baml);
            foreach (var permission in Permissions)
            {
                PermissionData.Add(permission);
            }

        }
        */
    }

}
