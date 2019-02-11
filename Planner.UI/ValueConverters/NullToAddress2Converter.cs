using System;
using System.Globalization;

namespace Planner.UI
{
    public class NullToAddress2Converter : BaseValueConverter<NullToAddress2Converter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null && (string)value == "null")
                return "";

            return value;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
