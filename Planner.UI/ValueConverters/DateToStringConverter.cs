using System;
using System.Globalization;

namespace Planner.UI
{
    public class DateToStringConverter : BaseValueConverter<DateToStringConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value != null)
            {
                DateTime date = (DateTime)value;
                var timeZone = TimeZoneInfo.Local;
                var convertedTime = TimeZoneInfo.ConvertTimeFromUtc(date, timeZone);

                return string.Format($"{convertedTime.Month}/{convertedTime.Day}/{convertedTime.Year}");
            }
            else
            {
                return string.Empty;
            }
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
