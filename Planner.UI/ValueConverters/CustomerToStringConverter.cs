using System;
using System.Globalization;

namespace Planner.UI
{
    public class CustomerToStringConverter : BaseValueConverter<CustomerToStringConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            CustomerWrapper customer = (CustomerWrapper)value;
            if (customer._Firstname != null)
                return "Edit Customer";
            else
                return "New Customer";
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
