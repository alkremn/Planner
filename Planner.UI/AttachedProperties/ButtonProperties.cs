using System.Windows;
using System.Windows.Controls;

namespace Planner.UI
{
    public class ButtonProperties
    {
        #region IsBusy
        public static bool GetIsBusy(DependencyObject element)
        {
            return (bool)element.GetValue(IsBusyProperty);
        }

        public static void SetIsBusy(DependencyObject element, bool value)
        {
            element.SetValue(IsBusyProperty, value);
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsBusyProperty =
            DependencyProperty.RegisterAttached("IsBusy", typeof(bool), typeof(ButtonProperties), new PropertyMetadata(false));
        #endregion

    }
}
