using Planner.UI.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Planner.UI
{
    public class DayToStringLabelConverter : BaseValueConverter<DayToStringLabelConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return "";
            var day = (Day)value;

            if(parameter == null)
            {
                return day.Date.ToLongDateString();
            }
            else
            {
                return $"{CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(day.Date.Month)}, {day.Date.Year}";
            }


        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
