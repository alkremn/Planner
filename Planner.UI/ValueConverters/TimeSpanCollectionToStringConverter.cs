using System;
using System.Collections.ObjectModel;
using System.Globalization;

namespace Planner.UI
{
    public class TimeSpanCollectionToStringConverter : BaseValueConverter<TimeSpanCollectionToStringConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return null;
            var timeString = new ObservableCollection<string>();
            var timeSpans = (ObservableCollection<TimeSpan>)value;
            
            foreach(var time in timeSpans){

                if (time.Hours == 0)
                {
                    timeString.Add($"12:{time.Minutes.ToString("D2")} AM");
                }
                else
                {
                    if (time.Hours < 13)
                    {
                        timeString.Add($"{(time.Hours).ToString("D2")}:{time.Minutes.ToString("D2")} AM");
                    }
                    else
                    {
                        timeString.Add($"{(time.Hours - 12).ToString("D2")}:{time.Minutes.ToString("D2")} PM");
                    }
                }

            }
            return timeString;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
