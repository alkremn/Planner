using System;
using System.Globalization;

namespace Planner.UI
{
    public class DateToTimeFormatConverter : BaseValueConverter<DateToTimeFormatConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return null;
            
            var timeValue = TimeZoneInfo.ConvertTimeFromUtc((DateTime)value, TimeZoneInfo.Local);

            return timeValue.ToString("h:mm tt");
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
