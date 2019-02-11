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
    public partial class AppListItemControl : UserControl
    {
        public static event ModifyAppointmentDelegate ModifyAppointmentEvent;
        public static event DeleteAppointmentDelegate DeleteAppointmentEvent;

        public AppListItemControl()
        {
            InitializeComponent();
        }

        private void Modify_App_Button_Click(object sender, RoutedEventArgs e)
        {
            ModifyAppointmentEvent(int.Parse(AppointmentId.Text));
        }

        private void Delete_App_Button_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show($"Please, Confirm to delete this appointment?", "Warning", MessageBoxButton.YesNo);

            if (result == MessageBoxResult.Yes)
                DeleteAppointmentEvent(int.Parse(AppointmentId.Text));
        }
    }
}
