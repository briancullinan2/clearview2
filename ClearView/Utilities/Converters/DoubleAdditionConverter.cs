using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;


namespace EPIC.ClearView.Utilities.Converters
{
	// Token: 0x02000051 RID: 81
	[ValueConversion(typeof(double), typeof(double))]
	public class DoubleAdditionConverter : IValueConverter
	{
		// Token: 0x060002BB RID: 699 RVA: 0x00016C50 File Offset: 0x00014E50
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			object result;
			if (!targetType.IsAssignableFrom(typeof(double)) || value == null || value.GetType() != typeof(double) || parameter == null)
			{
				result = DependencyProperty.UnsetValue;
			}
			else
			{
				Type type = parameter.GetType();
				double num;
				if (type == typeof(double))
				{
					num = (double)parameter;
				}
				else
				{
					if (!(type == typeof(string)))
					{
						return DependencyProperty.UnsetValue;
					}
					string s = (string)parameter;
					try
					{
						num = double.Parse(s, CultureInfo.InvariantCulture);
					}
					catch
					{
						return DependencyProperty.UnsetValue;
					}
				}
				double num2 = (double)value;
				num2 += num;
				result = num2;
			}
			return result;
		}

		// Token: 0x060002BC RID: 700 RVA: 0x00016D48 File Offset: 0x00014F48
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			object result;
			if (targetType != typeof(double) || value == null || value.GetType() != typeof(double) || parameter == null)
			{
				result = DependencyProperty.UnsetValue;
			}
			else
			{
				Type type = parameter.GetType();
				double num;
				if (type == typeof(double))
				{
					num = (double)parameter;
				}
				else
				{
					if (!(type == typeof(string)))
					{
						return DependencyProperty.UnsetValue;
					}
					string s = (string)parameter;
					try
					{
						num = double.Parse(s, CultureInfo.InvariantCulture);
					}
					catch
					{
						return DependencyProperty.UnsetValue;
					}
				}
				double num2 = (double)value;
				num2 -= num;
				result = num2;
			}
			return result;
		}
	}
}
