using Planner.UI.ViewModel;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Planner.UI
{
    /// <summary>
    /// Interaction logic for CustomerListItemControl.xaml
    /// </summary>
    public partial class CustomerListItemControl : UserControl
    {
        public static event DeleteCustomerDelegate DeleteCustomerEvent;
        public static event ModifyCustomerDelegate ModifyCustomerEvent;

        public CustomerListItemControl()
        {
            InitializeComponent();
        }

        private void Delete_Button_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result =  MessageBox.Show($"Please, Confirm to delete this customer?", "Warning", MessageBoxButton.YesNo);

            if ( result == MessageBoxResult.Yes)
                DeleteCustomerEvent(int.Parse(CustomerId.Text));
        }

        private void Modify_Button_Click(object sender, RoutedEventArgs e)
        {
            ModifyCustomerEvent(int.Parse(CustomerId.Text));
        }
    }
}
