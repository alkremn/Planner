using System;
using System.Globalization;
using System.Windows.Data;

namespace Planner.UI
{
    public class HeightConverter : BaseValueConverter<HeightConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var size = (double)value;
            var daySize = size / 6;
            return daySize;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
