using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
namespace Shared
{
    public class InverseBooleanConverter : IValueConverter
    {//converts a positivebool to negative (essentially it's a !isTrue for the XAML)
        public object Convert(object value, Type targetType, object parameter,
              System.Globalization.CultureInfo culture)
        {
            //if (targetType != typeof(bool))
              //  throw new InvalidOperationException("The target must be a boolean");

            return !(bool)value;
        }

        public object ConvertBack(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
