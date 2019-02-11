using System;
using System.Globalization;
using System.Windows.Data;

namespace Planner.UI
{
    public class WidthConverter : BaseValueConverter<WidthConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var size = (double)value;
            var daySize = size / 7;
            return daySize;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
