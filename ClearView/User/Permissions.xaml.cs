using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Markup;
using EPIC.Macros;
using EPIC.Resources;
using EPIC.Utilities.Converters;
using EPIC.Utilities.Extensions;
using EPICClearViewDL.EntityClasses;
using EPICClearViewDL.HelperClasses;
using EPICClearViewDL.Linq;
using Fluent;
using log4net;

namespace EPIC.User
{
	// Token: 0x0200004C RID: 76
	public partial class Permissions : Page, IStyleConnector
	{
		// Token: 0x0600028D RID: 653 RVA: 0x0001519C File Offset: 0x0001339C
		public Permissions()
		{
			this.InitializeComponent();
			Navigation.InsertRibbon(this);
			base.Loaded += delegate(object sender2, RoutedEventArgs args2)
			{
				FormChecker.Events[this].Changed += this.OnChanged;
				FormChecker.Events[this].Unchanged += this.OnUnchanged;
			};
			this.Roles.ItemsSource = new ObservableCollection<RoleEntity>(new LinqMetaData().Role.ToList<RoleEntity>());
			((ObservableCollection<RoleEntity>)this.Roles.ItemsSource).CollectionChanged += this.OnRoleCollectionChanged;
			this._permissionConverter = new PermissionBooleanConverter();
			this._permissions = new Dictionary<RoleEntity, Dictionary<PermissionEntity, bool>>();
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
			foreach (object obj in this.Roles.ItemsSource)
			{
				RoleEntity role = (RoleEntity)obj;
				this.AddColumn(role);
			}
			this.PermissionsGrid.ItemsSource = from x in new LinqMetaData().Permission
			where x.Description != ""
			orderby x.ParentId, x.ParentId == (decimal?)x.PermissionId descending, x.Description
			select x;
		}

		// Token: 0x0600028E RID: 654 RVA: 0x000154DC File Offset: 0x000136DC
		private void AddColumn(RoleEntity role)
		{
			this._permissions.Add(role, new Dictionary<PermissionEntity, bool>());
			FrameworkElementFactory frameworkElementFactory = new FrameworkElementFactory(typeof(CheckBox));
			frameworkElementFactory.SetValue(ToggleButton.IsCheckedProperty, new Binding("Name")
			{
				Mode = BindingMode.OneWay,
				Converter = this._permissionConverter,
				ConverterParameter = role
			});
			frameworkElementFactory.SetValue(FrameworkElement.HorizontalAlignmentProperty, HorizontalAlignment.Center);
			frameworkElementFactory.SetValue(FrameworkElement.VerticalAlignmentProperty, VerticalAlignment.Center);
			frameworkElementFactory.AddHandler(ToggleButton.CheckedEvent, new RoutedEventHandler(this.PermissionChecked));
			frameworkElementFactory.AddHandler(ToggleButton.UncheckedEvent, new RoutedEventHandler(this.PermissionChecked));
			this.PermissionsGrid.Columns.Add(new DataGridTemplateColumn
			{
				Width = 150.0,
				Header = role.Name,
				CellTemplate = new DataTemplate
				{
					VisualTree = frameworkElementFactory
				}
			});
		}

		// Token: 0x0600028F RID: 655 RVA: 0x000156EC File Offset: 0x000138EC
		private void PermissionChecked(object sender, RoutedEventArgs args)
		{
			if (!this._suppressChecking)
			{
				this._suppressChecking = true;
				CheckBox checkBox = sender as CheckBox;
				if (checkBox != null)
				{
					DataGridCell dataGridCell = checkBox.FindAncestor<DataGridCell>();
					if (dataGridCell != null)
					{
						PermissionEntity permissionEntity = dataGridCell.DataContext as PermissionEntity;
						if (permissionEntity != null)
						{
							int num = this.PermissionsGrid.Columns.IndexOf(dataGridCell.Column);
							RoleEntity role = this._permissions.Keys.ToList<RoleEntity>()[num - 1];
							this._permissions[role][permissionEntity] = (checkBox.IsChecked ?? false);
							if (permissionEntity.ParentId != null && permissionEntity.ParentId != permissionEntity.PermissionId)
							{
								List<PermissionEntity> source = permissionEntity.Parent.Children.Except(new PermissionEntity[]
								{
									permissionEntity.Parent
								}).ToList<PermissionEntity>();
								CheckBox checkFromEntity = this.GetCheckFromEntity(permissionEntity.Parent, num);
								if (checkFromEntity != null)
								{
									if (source.All((PermissionEntity x) => this._permissions[role].ContainsKey(x) && this._permissions[role][x]))
									{
										checkFromEntity.IsChecked = new bool?(true);
									}
									else if (source.All((PermissionEntity x) => !this._permissions[role].ContainsKey(x) || !this._permissions[role][x]))
									{
										checkFromEntity.IsChecked = new bool?(false);
									}
									else
									{
										checkFromEntity.IsChecked = null;
									}
								}
							}
							if (permissionEntity.Children.Any<PermissionEntity>())
							{
								foreach (CheckBox checkBox2 in from child in permissionEntity.Children.Except(new PermissionEntity[]
								{
									permissionEntity
								})
								select this.GetCheckFromEntity(child, this._permissions.Keys.ToList<RoleEntity>().IndexOf(role) + 1) into child
								where child != null
								select child)
								{
									checkBox2.IsChecked = new bool?(checkBox.IsChecked ?? false);
								}
								foreach (PermissionEntity key in permissionEntity.Children.Except(new PermissionEntity[]
								{
									permissionEntity
								}))
								{
									this._permissions[role][key] = (checkBox.IsChecked ?? false);
								}
							}
							this._suppressChecking = false;
						}
					}
				}
			}
		}

		// Token: 0x06000290 RID: 656 RVA: 0x00015A88 File Offset: 0x00013C88
		private CheckBox GetCheckFromEntity(PermissionEntity child, int column)
		{
			DataGridRow dataGridRow = this.PermissionsGrid.ItemContainerGenerator.ContainerFromItem(child) as DataGridRow;
			if (dataGridRow != null)
			{
				DataGridCell dataGridCell = dataGridRow.FindChild(null).ItemContainerGenerator.ContainerFromIndex(column) as DataGridCell;
				if (dataGridCell != null)
				{
					return dataGridCell.FindChild(null);
				}
			}
			return null;
		}

		// Token: 0x06000291 RID: 657 RVA: 0x00015B1C File Offset: 0x00013D1C
		private void RemoveColumn(RoleEntity role)
		{
			this.PermissionsGrid.Columns.Remove(this.PermissionsGrid.Columns.First((DataGridColumn x) => x.Header.ToString() == role.Name));
		}

		// Token: 0x06000292 RID: 658 RVA: 0x00015B68 File Offset: 0x00013D68
		private void OnRoleCollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
		{
			if (args.NewItems != null)
			{
				foreach (object obj in args.NewItems)
				{
					RoleEntity role = (RoleEntity)obj;
					this.AddColumn(role);
				}
			}
			if (args.OldItems != null)
			{
				foreach (object obj2 in args.OldItems)
				{
					RoleEntity role = (RoleEntity)obj2;
					this.RemoveColumn(role);
				}
			}
			this._collectionChanged = true;
			this.Save.IsEnabled = true;
		}

		// Token: 0x06000293 RID: 659 RVA: 0x00015C58 File Offset: 0x00013E58
		private void OnChanged(object sender, RoutedEventArgs routedEventArgs)
		{
			this.Save.IsEnabled = true;
		}

		// Token: 0x06000294 RID: 660 RVA: 0x00015C68 File Offset: 0x00013E68
		private void OnUnchanged(object sender, RoutedEventArgs routedEventArgs)
		{
			if (!this._collectionChanged)
			{
				this.Save.IsEnabled = false;
			}
		}

		// Token: 0x06000295 RID: 661 RVA: 0x00015C92 File Offset: 0x00013E92
		private void AddRole_Click(object sender, RoutedEventArgs e)
		{
			this.RolesTab.IsSelected = true;
			((ObservableCollection<RoleEntity>)this.Roles.ItemsSource).Add(new RoleEntity());
		}

		// Token: 0x06000296 RID: 662 RVA: 0x00015CC0 File Offset: 0x00013EC0
		private void RemoveRole_Click(object sender, RoutedEventArgs e)
		{
			Button button = sender as Button;
			if (button != null)
			{
				DataGridCell dataGridCell = button.FindAncestor<DataGridCell>();
				RoleEntity roleEntity = dataGridCell.DataContext as RoleEntity;
				if (roleEntity != null)
				{
					((ObservableCollection<RoleEntity>)this.Roles.ItemsSource).Remove(roleEntity);
					return;
				}
			}
			throw new NotImplementedException();
		}

		// Token: 0x06000297 RID: 663 RVA: 0x00015DBC File Offset: 0x00013FBC
		private void Save_Click(object sender, RoutedEventArgs e)
		{
			using (Transaction transaction = new Transaction(IsolationLevel.ReadCommitted, "edit permissions"))
			{
				try
				{
					foreach (KeyValuePair<RoleEntity, Dictionary<PermissionEntity, bool>> keyValuePair in this._permissions.ToList<KeyValuePair<RoleEntity, Dictionary<PermissionEntity, bool>>>())
					{
						transaction.Add(keyValuePair.Key);
						if (!((ObservableCollection<RoleEntity>)this.Roles.ItemsSource).Contains(keyValuePair.Key))
						{
							this._permissions.Remove(keyValuePair.Key);
							keyValuePair.Key.Permissions.DeleteMulti();
							keyValuePair.Key.Delete();
						}
						else
						{
							using (Dictionary<PermissionEntity, bool>.Enumerator enumerator2 = keyValuePair.Value.GetEnumerator())
							{
								while (enumerator2.MoveNext())
								{
									KeyValuePair<PermissionEntity, bool> permission = enumerator2.Current;
									bool flag;
									if (keyValuePair.Key.Permissions.Any(delegate(RolePermissionEntity x)
									{
										decimal permissionId2 = x.PermissionId;
										KeyValuePair<PermissionEntity, bool> permission2 = permission;
										return permissionId2 == permission2.Key.PermissionId;
									}))
									{
										KeyValuePair<PermissionEntity, bool> permission3 = permission;
										flag = permission3.Value;
									}
									else
									{
										flag = true;
									}
									if (flag)
									{
										if (!keyValuePair.Key.Permissions.All(delegate(RolePermissionEntity x)
										{
											decimal permissionId2 = x.PermissionId;
											KeyValuePair<PermissionEntity, bool> permission2 = permission;
											return permissionId2 != permission2.Key.PermissionId;
										}))
										{
											goto IL_1F2;
										}
										KeyValuePair<PermissionEntity, bool> permission3 = permission;
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
											RolePermissionEntity rolePermissionEntity = keyValuePair.Key.Permissions.AddNew();
											RolePermissionEntity rolePermissionEntity2 = rolePermissionEntity;
											permission3 = permission;
											rolePermissionEntity2.PermissionId = permission3.Key.PermissionId;
											continue;
										}
										continue;
										IL_1F2:
										flag2 = true;
										goto IL_1F3;
									}
									RolePermissionEntity rolePermissionEntity3 = keyValuePair.Key.Permissions.First(delegate(RolePermissionEntity x)
									{
										decimal permissionId2 = x.PermissionId;
										KeyValuePair<PermissionEntity, bool> permission2 = permission;
										return permissionId2 == permission2.Key.PermissionId;
									});
									keyValuePair.Key.Permissions.Remove(rolePermissionEntity3);
									transaction.Add(rolePermissionEntity3);
									rolePermissionEntity3.Delete();
								}
							}
							if (((ObservableCollection<RoleEntity>)this.Roles.ItemsSource).Contains(keyValuePair.Key))
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
				UserEntity user = mainWindow.User;
				mainWindow.User = new LinqMetaData().User.First((UserEntity x) => x.UserId == user.UserId);
			}
		}

		// Token: 0x06000298 RID: 664 RVA: 0x000161E8 File Offset: 0x000143E8
		private void Name_TextChanged(object sender, TextChangedEventArgs e)
		{
			TextBox textBox = sender as TextBox;
			if (textBox != null)
			{
				DataGridCell dataGridCell = textBox.FindAncestor<DataGridCell>();
				if (dataGridCell != null)
				{
					RoleEntity roleEntity = dataGridCell.DataContext as RoleEntity;
					if (roleEntity != null)
					{
						int index = this._permissions.Keys.ToList<RoleEntity>().IndexOf(roleEntity) + 1;
						this.PermissionsGrid.Columns[index].Header = textBox.Text;
					}
				}
			}
		}

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

		// Token: 0x04000154 RID: 340
		private static readonly ILog Log = LogManager.GetLogger(typeof(Permissions));

		// Token: 0x04000155 RID: 341
		private readonly PermissionBooleanConverter _permissionConverter;

		// Token: 0x04000156 RID: 342
		private readonly Dictionary<RoleEntity, Dictionary<PermissionEntity, bool>> _permissions;

		// Token: 0x04000157 RID: 343
		private bool _collectionChanged;

		// Token: 0x04000158 RID: 344
		private bool _suppressChecking;
	}
}
