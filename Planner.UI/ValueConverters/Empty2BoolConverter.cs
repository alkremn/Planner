using System;
using System.Globalization;
using System.Windows.Data;

namespace Planner.UI
{
    class Empty2BoolConverter : BaseValueConverter<Empty2BoolConverter>
    {
      
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
           return string.IsNullOrEmpty((string)value);
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
