using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Planner.UI
{
    public class BoolToColorConverter : BaseValueConverter<BoolToColorConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var timeIsSelected = (bool)value;

            if (timeIsSelected)
                return "Grey";
            else
                return "Red";
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
