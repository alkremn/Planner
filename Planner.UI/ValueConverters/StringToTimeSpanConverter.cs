using System;
using System.Globalization;

namespace Planner.UI
{
    public class StringToTimeSpanConverter : BaseValueConverter<StringToTimeSpanConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return null;
            var timeSpan = (TimeSpan)value;

            if (timeSpan.Hours == 0)
            {
                return $"12:{timeSpan.Minutes.ToString("D2")} AM";
            }
            else
            {
                if (timeSpan.Hours < 13)
                {
                    return $"{(timeSpan.Hours).ToString("D2")}:{timeSpan.Minutes.ToString("D2")} AM";
                }
                else
                {
                    return $"{(timeSpan.Hours - 12).ToString("D2")}:{timeSpan.Minutes.ToString("D2")} PM";
                }
            }
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                string time = (string)value;
                var timeStrings = time.Split(':',' ');
                int hour = int.Parse(timeStrings[0]);
                int mins = int.Parse(timeStrings[1]);

                switch (timeStrings[2])
                {
                    case "AM":
                        if (hour == 12)
                            return new TimeSpan(0, mins, 0);
                        else
                            return new TimeSpan(hour, mins, 0);
                    case "PM":
                        return new TimeSpan(hour + 12, mins, 0);
                }
            }
            return new TimeSpan(-1, 0, 0);
        }
    }
}
