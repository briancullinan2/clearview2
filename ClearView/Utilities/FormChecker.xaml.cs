using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using EPIC.ClearView.Utilities.Extensions;
using Xceed.Wpf.Toolkit;

namespace EPIC.ClearView.Utilities
{
	// Token: 0x02000044 RID: 68
	public partial class FormChecker : ResourceDictionary, IStyleConnector
	{
		// Token: 0x17000080 RID: 128
		// (get) Token: 0x06000237 RID: 567 RVA: 0x00012548 File Offset: 0x00010748
		// (set) Token: 0x06000238 RID: 568 RVA: 0x0001255E File Offset: 0x0001075E
		public static FormChecker.EventsCollection Events { get; private set; } = new FormChecker.EventsCollection();

		// Token: 0x0600023A RID: 570 RVA: 0x000125B0 File Offset: 0x000107B0
		private void OnChanged(object control, bool changed, RoutedEventArgs e)
		{
			this._changedDictionary[control] = changed;
			using (Dictionary<FrameworkElement, FormChecker.CheckEvents>.Enumerator enumerator = FormChecker.Subscriptions.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<FrameworkElement, FormChecker.CheckEvents> subscription = enumerator.Current;
					bool flag;
					if (control is FrameworkElement)
					{
						flag = !((FrameworkElement)control).GetAncestors().Any(delegate(DependencyObject x)
						{
							KeyValuePair<FrameworkElement, FormChecker.CheckEvents> subscription2 = subscription;
							return object.Equals(x, subscription2.Key);
						});
					}
					else
					{
						flag = true;
					}
					if (!flag)
					{
						KeyValuePair<FrameworkElement, FormChecker.CheckEvents> subscription3 = subscription;
						subscription3.Value.OnChanged(control, e);
					}
				}
			}
		}

		// Token: 0x0600023C RID: 572 RVA: 0x00012C2C File Offset: 0x00010E2C
		private void ElementLoaded(object sender, RoutedEventArgs e)
		{
			FrameworkElement frameworkElement = sender as FrameworkElement;
			if (frameworkElement != null && !this._valuesDictionary.ContainsKey(frameworkElement))
			{
				using (Dictionary<FrameworkElement, FormChecker.CheckEvents>.Enumerator enumerator = FormChecker.Subscriptions.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						KeyValuePair<FrameworkElement, FormChecker.CheckEvents> subscription = enumerator.Current;
						KeyValuePair<FrameworkElement, FormChecker.CheckEvents> subscription3 = subscription;
						bool flag;
						if (!subscription3.Value.ControlsAssigned)
						{
							flag = !frameworkElement.GetAncestors().Any(delegate(DependencyObject x)
							{
								KeyValuePair<FrameworkElement, FormChecker.CheckEvents> subscription2 = subscription;
								return object.Equals(x, subscription2.Key);
							});
						}
						else
						{
							flag = true;
						}
						if (!flag)
						{
							subscription3 = subscription;
							subscription3.Value.AssignControls(this, this._changedDictionary, this._valuesDictionary, this._savingDictionary);
						}
					}
				}
				TextBox textBox = sender as TextBox;
				if (textBox != null)
				{
					this._savingDictionary[textBox] = delegate(object o)
					{
						this._valuesDictionary[o] = textBox.Text;
					};
					this._savingDictionary[textBox](textBox);
					textBox.TextChanged += delegate(object o, TextChangedEventArgs args)
					{
						this.OnChanged(o, (string)this._valuesDictionary[o] != textBox.Text, args);
					};
				}
				CheckBox checkBox = sender as CheckBox;
				if (checkBox != null)
				{
					this._savingDictionary[checkBox] = delegate(object o)
					{
						this._valuesDictionary[o] = checkBox.IsChecked;
					};
					this._savingDictionary[checkBox](checkBox);
					checkBox.Checked += delegate(object o, RoutedEventArgs args)
					{
						this.OnChanged(o, (bool?)this._valuesDictionary[o] != checkBox.IsChecked, args);
					};
					checkBox.Unchecked += delegate(object o, RoutedEventArgs args)
					{
						this.OnChanged(o, (bool?)this._valuesDictionary[o] != checkBox.IsChecked, args);
					};
				}
				ListBox listbox = sender as ListBox;
				if (listbox != null)
				{
					this._savingDictionary[listbox] = delegate(object o)
					{
						this._valuesDictionary[o] = listbox.SelectedItems.OfType<object>().ToList<object>();
					};
					this._savingDictionary[listbox](listbox);
					listbox.SelectionChanged += delegate(object o, SelectionChangedEventArgs args)
					{
						this.OnChanged(o, !listbox.SelectedItems.OfType<object>().All(new Func<object, bool>(((List<object>)this._valuesDictionary[o]).Contains)) || !((List<object>)this._valuesDictionary[o]).All(new Func<object, bool>(listbox.SelectedItems.OfType<object>().Contains<object>)), args);
					};
				}
				DatePicker datePicker = sender as DatePicker;
				if (datePicker != null)
				{
					this._savingDictionary[datePicker] = delegate(object o)
					{
						this._valuesDictionary[o] = datePicker.SelectedDate;
					};
					this._savingDictionary[datePicker](datePicker);
					datePicker.SelectedDateChanged += delegate(object o, SelectionChangedEventArgs args)
					{
						this.OnChanged(o, datePicker.SelectedDate != (DateTime?)this._valuesDictionary[o], args);
					};
				}
				ComboBox comboBox = sender as ComboBox;
				if (comboBox != null)
				{
					this._savingDictionary[comboBox] = delegate(object o)
					{
						this._valuesDictionary[o] = comboBox.SelectedItem;
					};
					this._savingDictionary[comboBox](comboBox);
					comboBox.SelectionChanged += delegate(object o, SelectionChangedEventArgs args)
					{
						this.OnChanged(o, (comboBox.SelectedItem == null && this._valuesDictionary[o] != null) || (comboBox.SelectedItem != null && this._valuesDictionary[o] == null) || (comboBox.SelectedItem != null && !comboBox.SelectedItem.Equals(this._valuesDictionary[o])), args);
					};
				}
				DecimalUpDown decimalUpDown = sender as DecimalUpDown;
				if (decimalUpDown != null)
				{
					this._savingDictionary[decimalUpDown] = delegate(object o)
					{
						this._valuesDictionary[o] = decimalUpDown.Value;
					};
					this._savingDictionary[decimalUpDown](decimalUpDown);
					decimalUpDown.ValueChanged += delegate(object o, RoutedPropertyChangedEventArgs<object> args)
					{
						this.OnChanged(o, decimalUpDown.Value != (decimal?)this._valuesDictionary[o], args);
					};
				}
				IntegerUpDown integerUpDown = sender as IntegerUpDown;
				if (integerUpDown != null)
				{
					this._savingDictionary[integerUpDown] = delegate(object o)
					{
						this._valuesDictionary[o] = integerUpDown.Value;
					};
					this._savingDictionary[integerUpDown](integerUpDown);
					integerUpDown.ValueChanged += delegate(object o, RoutedPropertyChangedEventArgs<object> args)
					{
						this.OnChanged(o, integerUpDown.Value != (int?)this._valuesDictionary[o], args);
					};
				}
				RadioButton radionButton = sender as RadioButton;
				if (radionButton != null)
				{
					this._savingDictionary[radionButton] = delegate(object o)
					{
						this._valuesDictionary[o] = radionButton.IsChecked;
					};
					this._savingDictionary[radionButton](radionButton);
					radionButton.Checked += delegate(object o, RoutedEventArgs args)
					{
						this.OnChanged(o, radionButton.IsChecked != (bool?)this._valuesDictionary[o], args);
					};
					radionButton.Unchecked += delegate(object o, RoutedEventArgs args)
					{
						this.OnChanged(o, radionButton.IsChecked != (bool?)this._valuesDictionary[o], args);
					};
				}
			}
		}

		// Token: 0x0600023F RID: 575 RVA: 0x00013264 File Offset: 0x00011464
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		void IStyleConnector.Connect(int connectionId, object target)
		{
			switch (connectionId)
			{
			case 1:
			{
				EventSetter eventSetter = new EventSetter();
				eventSetter.Event = FrameworkElement.LoadedEvent;
				eventSetter.Handler = new RoutedEventHandler(this.ElementLoaded);
				((Style)target).Setters.Add(eventSetter);
				break;
			}
			case 2:
			{
				EventSetter eventSetter = new EventSetter();
				eventSetter.Event = FrameworkElement.LoadedEvent;
				eventSetter.Handler = new RoutedEventHandler(this.ElementLoaded);
				((Style)target).Setters.Add(eventSetter);
				break;
			}
			case 3:
			{
				EventSetter eventSetter = new EventSetter();
				eventSetter.Event = FrameworkElement.LoadedEvent;
				eventSetter.Handler = new RoutedEventHandler(this.ElementLoaded);
				((Style)target).Setters.Add(eventSetter);
				break;
			}
			case 4:
			{
				EventSetter eventSetter = new EventSetter();
				eventSetter.Event = FrameworkElement.LoadedEvent;
				eventSetter.Handler = new RoutedEventHandler(this.ElementLoaded);
				((Style)target).Setters.Add(eventSetter);
				break;
			}
			case 5:
			{
				EventSetter eventSetter = new EventSetter();
				eventSetter.Event = FrameworkElement.LoadedEvent;
				eventSetter.Handler = new RoutedEventHandler(this.ElementLoaded);
				((Style)target).Setters.Add(eventSetter);
				break;
			}
			case 6:
			{
				EventSetter eventSetter = new EventSetter();
				eventSetter.Event = FrameworkElement.LoadedEvent;
				eventSetter.Handler = new RoutedEventHandler(this.ElementLoaded);
				((Style)target).Setters.Add(eventSetter);
				break;
			}
			case 7:
			{
				EventSetter eventSetter = new EventSetter();
				eventSetter.Event = FrameworkElement.LoadedEvent;
				eventSetter.Handler = new RoutedEventHandler(this.ElementLoaded);
				((Style)target).Setters.Add(eventSetter);
				break;
			}
			case 8:
			{
				EventSetter eventSetter = new EventSetter();
				eventSetter.Event = FrameworkElement.LoadedEvent;
				eventSetter.Handler = new RoutedEventHandler(this.ElementLoaded);
				((Style)target).Setters.Add(eventSetter);
				break;
			}
			}
		}

		// Token: 0x04000127 RID: 295
		private readonly Dictionary<object, bool> _changedDictionary = new Dictionary<object, bool>();

		// Token: 0x04000128 RID: 296
		private readonly Dictionary<object, object> _valuesDictionary = new Dictionary<object, object>();

		// Token: 0x04000129 RID: 297
		private readonly Dictionary<object, Action<object>> _savingDictionary = new Dictionary<object, Action<object>>();

		// Token: 0x0400012A RID: 298
		private static readonly Dictionary<FrameworkElement, FormChecker.CheckEvents> Subscriptions = new Dictionary<FrameworkElement, FormChecker.CheckEvents>();

		// Token: 0x02000045 RID: 69
		public class EventsCollection
		{
			// Token: 0x17000081 RID: 129
			// (get) Token: 0x06000240 RID: 576 RVA: 0x00013478 File Offset: 0x00011678
			public ICollection<FrameworkElement> Keys
			{
				get
				{
					return FormChecker.Subscriptions.Keys;
				}
			}

			// Token: 0x17000082 RID: 130
			public FormChecker.CheckEvents this[FrameworkElement parent]
			{
				get
				{
					if (!FormChecker.Subscriptions.ContainsKey(parent))
					{
						FormChecker.Subscriptions.Add(parent, new FormChecker.CheckEvents(null, parent, null, null, null));
						parent.Unloaded += delegate(object sender, RoutedEventArgs args)
						{
							FormChecker.Subscriptions.Remove(parent);
						};
					}
					return FormChecker.Subscriptions[parent];
				}
			}
		}

		// Token: 0x02000046 RID: 70
		public class CheckEvents
		{
			// Token: 0x14000006 RID: 6
			// (add) Token: 0x06000243 RID: 579 RVA: 0x00013540 File Offset: 0x00011740
			// (remove) Token: 0x06000244 RID: 580 RVA: 0x0001357C File Offset: 0x0001177C
			public event RoutedEventHandler Changed;

			// Token: 0x14000007 RID: 7
			// (add) Token: 0x06000245 RID: 581 RVA: 0x000135B8 File Offset: 0x000117B8
			// (remove) Token: 0x06000246 RID: 582 RVA: 0x000135F4 File Offset: 0x000117F4
			public event RoutedEventHandler Unchanged;

			// Token: 0x17000083 RID: 131
			// (get) Token: 0x06000247 RID: 583 RVA: 0x00013650 File Offset: 0x00011850
			public bool IsChanged
			{
				get
				{
					return this._controls != null && this._controls.Keys.Intersect(this.Objects).Any((object x) => this._controls[x]);
				}
			}

			// Token: 0x17000084 RID: 132
			// (get) Token: 0x06000248 RID: 584 RVA: 0x00013698 File Offset: 0x00011898
			public bool ControlsAssigned
			{
				get
				{
					return this._controls != null;
				}
			}

			// Token: 0x06000249 RID: 585 RVA: 0x000136B8 File Offset: 0x000118B8
			public void OnChanged(object control, RoutedEventArgs e)
			{
				RoutedEventHandler routedEventHandler;
				if (this.IsChanged)
				{
					routedEventHandler = this.Changed;
				}
				else
				{
					routedEventHandler = this.Unchanged;
				}
				if (routedEventHandler != null)
				{
					routedEventHandler(control, e);
				}
			}

			// Token: 0x0600024A RID: 586 RVA: 0x000136F4 File Offset: 0x000118F4
			public CheckEvents(FormChecker checker, FrameworkElement parent, Dictionary<object, bool> controls, Dictionary<object, object> values, Dictionary<object, Action<object>> saving)
			{
				this._checker = checker;
				this._parent = parent;
				this._controls = controls;
				this._values = values;
				this._saving = saving;
			}

			// Token: 0x0600024B RID: 587 RVA: 0x00013745 File Offset: 0x00011945
			public CheckEvents(FrameworkElement parent, FormChecker.CheckEvents checkEvents) : this(checkEvents._checker, parent, checkEvents._controls, checkEvents._values, checkEvents._saving)
			{
			}

			// Token: 0x0600024C RID: 588 RVA: 0x0001376C File Offset: 0x0001196C
			public void AssignControls(FormChecker checker, Dictionary<object, bool> controls, Dictionary<object, object> values, Dictionary<object, Action<object>> saving)
			{
				this._checker = checker;
				this._controls = controls;
				this._values = values;
				this._saving = saving;
				foreach (FrameworkElement sender in this._inclusions)
				{
					this._checker.ElementLoaded(sender, null);
				}
			}

			// Token: 0x0600024D RID: 589 RVA: 0x000137EC File Offset: 0x000119EC
			public void Save()
			{
				if (this._controls != null)
				{
					foreach (object obj in this.Objects.ToList<object>())
					{
						FrameworkElement frameworkElement = (FrameworkElement)obj;
						this._controls[frameworkElement] = false;
						this._saving[frameworkElement](frameworkElement);
					}
				}
				this.OnChanged(this, new RoutedEventArgs());
			}

			// Token: 0x17000085 RID: 133
			// (get) Token: 0x0600024E RID: 590 RVA: 0x00013B8C File Offset: 0x00011D8C
			public IEnumerable<object> Objects
			{
				get
				{
					if (this.ControlsAssigned)
					{
						foreach (KeyValuePair<object, bool> control2 in this._controls.Where(delegate(KeyValuePair<object, bool> control)
						{
							bool result;
							if (control.Key is Control)
							{
								List<DependencyObject> source = ((Control)control.Key).GetAncestors().ToList<DependencyObject>();
								bool flag;
								if (source.Any((DependencyObject x) => x.Equals(this._parent)) && !this._exclusions.Contains(control.Key))
								{
									flag = source.All((DependencyObject x) => !this._exclusions.Contains(x));
								}
								else
								{
									flag = false;
								}
								result = flag;
							}
							else
							{
								result = false;
							}
							return result;
						}))
						{
							KeyValuePair<object, bool> keyValuePair = control2;
							yield return keyValuePair.Key;
						}
					}
					yield break;
				}
			}

			// Token: 0x0600024F RID: 591 RVA: 0x00013BAD File Offset: 0x00011DAD
			public void Exclude(FrameworkElement exclusion)
			{
				this._exclusions.Add(exclusion);
			}

			// Token: 0x06000250 RID: 592 RVA: 0x00013BC0 File Offset: 0x00011DC0
			public void Include(FrameworkElement inclusion)
			{
				if (inclusion == null)
				{
					throw new ArgumentNullException("inclusion");
				}
				this._inclusions.Add(inclusion);
				if (this._checker != null)
				{
					this._checker.ElementLoaded(inclusion, null);
				}
			}

			// Token: 0x0400012F RID: 303
			private FormChecker _checker;

			// Token: 0x04000130 RID: 304
			private readonly FrameworkElement _parent;

			// Token: 0x04000131 RID: 305
			private Dictionary<object, bool> _controls;

			// Token: 0x04000132 RID: 306
			private Dictionary<object, object> _values;

			// Token: 0x04000133 RID: 307
			private Dictionary<object, Action<object>> _saving;

			// Token: 0x04000134 RID: 308
			private readonly List<FrameworkElement> _exclusions = new List<FrameworkElement>();

			// Token: 0x04000135 RID: 309
			private readonly List<FrameworkElement> _inclusions = new List<FrameworkElement>();
		}
	}
}
