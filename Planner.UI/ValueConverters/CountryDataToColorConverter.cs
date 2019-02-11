using FamFamFam.Flags.Wpf;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;

namespace Planner.UI
{
    public class CountryDataToColorConverter : BaseValueConverter<CountryDataToColorConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            CountryData selectedCountry = (CountryData)value;

            if (selectedCountry.Name != null)
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
