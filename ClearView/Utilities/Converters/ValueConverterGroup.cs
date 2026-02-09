using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace EPIC.ClearView.Utilities.Converters
{
	// Token: 0x02000057 RID: 87
	[ContentProperty("Converters")]
	public class ValueConverterGroup : IValueConverter
	{
		// Token: 0x060002D3 RID: 723 RVA: 0x00017646 File Offset: 0x00015846
		public ValueConverterGroup()
		{
			this._converters.CollectionChanged += this.OnConvertersCollectionChanged;
		}

		// Token: 0x1700008D RID: 141
		// (get) Token: 0x060002D4 RID: 724 RVA: 0x00017674 File Offset: 0x00015874
		public ObservableCollection<IValueConverter> Converters
		{
			get
			{
				return this._converters;
			}
		}

		// Token: 0x060002D5 RID: 725 RVA: 0x0001768C File Offset: 0x0001588C
		object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			object obj = value;
			for (int i = 0; i < this.Converters.Count; i++)
			{
				IValueConverter valueConverter = this.Converters[i];
				obj = valueConverter.Convert(obj, targetType, parameter, culture);
				if (obj == Binding.DoNothing)
				{
					break;
				}
			}
			return obj;
		}

		// Token: 0x060002D6 RID: 726 RVA: 0x000176EC File Offset: 0x000158EC
		object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			object obj = value;
			for (int i = this.Converters.Count - 1; i > -1; i--)
			{
				IValueConverter valueConverter = this.Converters[i];
				obj = valueConverter.ConvertBack(obj, targetType, parameter, culture);
				if (obj == Binding.DoNothing)
				{
					break;
				}
			}
			return obj;
		}

		// Token: 0x060002D7 RID: 727 RVA: 0x0001774D File Offset: 0x0001594D
		private void OnConvertersCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
		}

		// Token: 0x0400016D RID: 365
		private readonly ObservableCollection<IValueConverter> _converters = new ObservableCollection<IValueConverter>();
	}
}
