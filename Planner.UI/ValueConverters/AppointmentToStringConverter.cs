using System;
using System.Globalization;

namespace Planner.UI
{
    public class AppointmentToStringConverter : BaseValueConverter<AppointmentToStringConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var app = (AppointmentWrapper)value;
            if (app._Title != null)
                return "Edit Appointment";
            else
                return "New Appointment";
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
