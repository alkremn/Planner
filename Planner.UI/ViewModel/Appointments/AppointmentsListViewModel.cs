using Planner.UI.Events;
using Prism.Events;
using System.Collections.ObjectModel;
using System.Windows;

namespace Planner.UI.ViewModel
{
    public delegate void AddAppointmentDelegate(int id);
    public delegate void ModifyAppointmentDelegate(int id);

    public class AppointmentsListViewModel : ViewModelBase , IAppointmentsListViewModel
    {
        private IEventAggregator _eventAggregator;
        private ObservableCollection<CustomerWrapper> _customers;
        private ObservableCollection<AppointmentWrapper> _appointments;
        private CustomerWrapper _selectedCustomer;
        private bool _customerIsLoading;
        private bool _appointmentIsLoading;
        private bool _noCustomerLabelIsVisible;

        public ObservableCollection<CustomerWrapper> Customers
        {
            get { return _customers; }
            set
            {
                OnPropertyChanged(ref _customers, value);
            }
        }

        public ObservableCollection<AppointmentWrapper> Appointments
        {
            get { return _appointments; }
            set
            {
                OnPropertyChanged(ref _appointments, value);
            }
        }

        public CustomerWrapper SelectedCustomer
        {
            get { return _selectedCustomer; }
            set
            {
                OnPropertyChanged(ref _selectedCustomer, value);
                if(value != null)
                    _eventAggregator.GetEvent<SelectedCustomerChangedEvent>().Publish(_selectedCustomer._Id);
            }
        }

        public bool CustomerIsLoading
        {
            get { return _customerIsLoading; }
            set
            {
                OnPropertyChanged(ref _customerIsLoading, value);
                if (!value && Customers.Count == 0)
                {
                    NoCustomerLabelIsVisible = true;
                }
                if (!value && Customers.Count > 0)
                {
                    NoCustomerLabelIsVisible = false;
                }
            }
            
        }

        public bool AppointmentIsLoading
        {
            get { return _appointmentIsLoading; }
            set
            {
                OnPropertyChanged(ref _appointmentIsLoading, value);
            }
        }

        public bool NoCustomerLabelIsVisible
        {
            get { return _noCustomerLabelIsVisible; }
            set
            {
                OnPropertyChanged(ref _noCustomerLabelIsVisible, value);
            }
        }

        private Visibility _customerLoadingVisibility = Visibility.Hidden;
        private Visibility _customerListVisibility = Visibility.Hidden;
        private Visibility _appointmentLoadingVisibility = Visibility.Hidden;
        private Visibility _appointmentListVisibility = Visibility.Hidden;
        private Visibility _noAppointmentsVisibility = Visibility.Hidden;
        private Visibility _noCustomerVisibility = Visibility.Hidden;

        public Visibility CustomerLoadingVisibility
        {
            get { return _customerLoadingVisibility; }
            set { OnPropertyChanged(ref _customerLoadingVisibility, value); }
        }

        public Visibility CustomerListVisibility
        {
            get { return _customerListVisibility; }
            set { OnPropertyChanged(ref _customerListVisibility, value); }
        }

        public Visibility AppointmentLoadingVisibility
        {
            get { return _appointmentLoadingVisibility; }
            set { OnPropertyChanged(ref _appointmentLoadingVisibility, value); }
        }

        public Visibility AppointmentListVisibility
        {
            get { return _appointmentListVisibility; }
            set { OnPropertyChanged(ref _appointmentListVisibility, value); }
        }

        public Visibility NoAppointmentsVisibility
        {
            get { return _noAppointmentsVisibility; }
            set { OnPropertyChanged(ref _noAppointmentsVisibility, value); }
        }

        public Visibility NoCustomerVisibility
        {
            get { return _noCustomerVisibility; }
            set { OnPropertyChanged(ref _noCustomerVisibility, value); }
        }

        public AppointmentsListViewModel(IEventAggregator eventAggregator, ObservableCollection<CustomerWrapper> customers,
           ObservableCollection<AppointmentWrapper> appointments)
        {
            _eventAggregator = eventAggregator;
            _customers = customers;
            _appointments = appointments;
            CusAppListItemControl.AddAppointmentEvent += ((id) => OnAddAppointment(id));
           
            _eventAggregator.GetEvent<CustomerIsLoadingEvent>().Subscribe(OnCustomerLoading);
            _eventAggregator.GetEvent<AppointmentIsLoadingEvent>().Subscribe(OnAppointmentLoading);
        }


        public AppointmentWrapper SelectedApointment { get; set; }
       
        private void OnNewAppointmentCreate()
        {
            _eventAggregator.GetEvent<NewCustomerViewEvent>().Publish("New Customer triggered");
            SelectedApointment = null;
        }

        private void OnAddAppointment(int id)
        {
            _eventAggregator.GetEvent<AddAppointmentEvent>().Publish(id);
        }

        private void OnAppointmentLoading(bool isLoading)
        {
            if (isLoading)
            {
                AppointmentLoadingVisibility = Visibility.Visible;
                AppointmentListVisibility = Visibility.Hidden;
                NoAppointmentsVisibility = Visibility.Hidden;
            }
            else
            {
                AppointmentLoadingVisibility = Visibility.Hidden;
                AppointmentListVisibility = Visibility.Visible;

                if (Appointments.Count > 0)
                    NoAppointmentsVisibility = Visibility.Hidden;
                else
                    NoAppointmentsVisibility = Visibility.Visible;
            }
        }
        private void OnCustomerLoading(bool isLoading)
        {
            CustomerIsLoading = isLoading;
            if (!isLoading)
            {
                if(_customers.Count > 0)
                    SelectedCustomer = _customers[0];
            }
        }
    }
}
