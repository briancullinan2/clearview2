using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;

namespace EPIC.DataLayer.Converters
{
    public class UriTabExistsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null && parameter != null && value is ItemCollection items && parameter is string uriTarget)
            {
                string target = uriTarget.Trim('/');
                var result =  items.OfType<TabItem>().Any(x =>
                    x.Content is Frame f && f.Source != null && f.Source.OriginalString.Trim('/').Contains(target));
                return result;
            }
            return false;
        }

        // Token: 0x060002C7 RID: 711 RVA: 0x0001723E File Offset: 0x0001543E
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
