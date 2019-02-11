using Planner.UI.DataService;
using System.Windows.Controls;
using Prism.Events;
using Planner.UI.Events;
using Planner.DataAccess;
using System.Collections.Generic;
using System.Windows.Input;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using System.Windows;
using Planner.UI.Exceptions;
using System.Net.NetworkInformation;
using Planner.UI.Data;

namespace Planner.UI.ViewModel
{
    public delegate void DeleteCustomerDelegate(int id);
    public delegate void ModifyCustomerDelegate(int id);
    public delegate void DeleteAppointmentDelegate(int id);

    public class MainPageViewModel : ViewModelBase, IMainPageViewModel
    {
        #region Private Fields
        private readonly ICustomerDataService _customerDataService;
        private readonly IAppointmentDataService _appointmentDataService;
        private readonly IAuthenticationDataService _authDataService;
        private const int REMINDER_TIME = 15;

        public User _user { get; }
        private ObservableCollection<CustomerWrapper> _customers;
        private ObservableCollection<AppointmentWrapper> _allAppointments;
        private ObservableCollection<AppointmentWrapper> _customerAppointments;
        private ObservableCollection<User> _users;
        private CustomersListViewModel _customerListVM;
        private AppointmentsListViewModel _appointmentListVM;
        private IEventAggregator _eventAggregator;
        private UserControl _currentContent;
        private Dictionary<ApplicationControls, UserControl> Controls { get; set; }
        #endregion

        #region Public Constructor

        public MainPageViewModel(ICustomerDataService customerDataService,
                                IAppointmentDataService appointmentDataService,
                                IAuthenticationDataService authDataService, User user)
        {
            _customerDataService = customerDataService;
            _appointmentDataService = appointmentDataService;
            _authDataService = authDataService;
            _user = user;
            _customers = new ObservableCollection<CustomerWrapper>();
            _allAppointments = new ObservableCollection<AppointmentWrapper>();
            _customerAppointments = new ObservableCollection<AppointmentWrapper>();
            _users = new ObservableCollection<User>();
            _eventAggregator = new EventAggregator();
            _customerListVM = new CustomersListViewModel(_eventAggregator, _customers);
            _appointmentListVM = new AppointmentsListViewModel(_eventAggregator, _customers, _customerAppointments);

            //Initializing all events
            InitializeEvents();

            //creates controls for the main page 
            Controls = new Dictionary<ApplicationControls, UserControl>
            {
                [ApplicationControls.CustomersList] = new CustomersListControl(_customerListVM),
                [ApplicationControls.AppointmentsList] = new AppointmentsListControl(_appointmentListVM),
                [ApplicationControls.Calendar] = new CalendarControl(new CalendarViewModel(_eventAggregator, _allAppointments)),
                [ApplicationControls.Report] = new ReportControl(
                                                new ReportViewModel(_eventAggregator, _customers, _allAppointments, _users))
            };

            //setting up onLoad control
            CurrentContent = Controls[ApplicationControls.CustomersList];

            //initializing Commands
            CustomersCommand = new RelayCommand(OnCustomersControlLoad);
            AppointmentsCommand = new RelayCommand(OnAppointmentsControlLoad);
            CalendarCommand = new RelayCommand(OnCalendarControlLoad);
            ReportCommand = new RelayCommand(OnReportControlLoad);
            InitCusAppList();
        }

        #endregion

        #region Public Fields

        public bool IsLoading { get; private set; }

        public ICommand CustomersCommand { get; private set; }

        public ICommand AppointmentsCommand { get; private set; }

        public ICommand CalendarCommand { get; private set; }

        public ICommand ReportCommand { get; private set; }

        public UserControl CurrentContent
        {
            get { return _currentContent; }
            set
            {
                OnPropertyChanged(ref _currentContent, value);
            }
        }
        #endregion

        #region Private Methods

        // Initializes Customer, Appointment lists
        private async void InitCusAppList()
        {
            if (await CheckInternetConnection())
            {
                await LoadCustomerData();
            }
            else
            {
                MessageBox.Show("There is a problem with connection to the remote server", "Connection Error");
            }
        }

        // loads Customers from Database
        private async Task LoadCustomerData()
        {
            _eventAggregator.GetEvent<CustomerIsLoadingEvent>().Publish(true);
            var customers = await _customerDataService.GetAllCustomersAsync();
            var appointments = await _appointmentDataService.GetAppointmentsAsync();
            var users = await _authDataService.GetAllUsersAsync();
            Task.WaitAll();

            _customers.Clear();
            customers.ForEach(c => _customers.Add(c));

            _allAppointments.Clear();
            appointments.ForEach(app => _allAppointments.Add(app));

            _users.Clear();
            users.ForEach(user => _users.Add(user));

            _eventAggregator.GetEvent<CustomerIsLoadingEvent>().Publish(false);

            //onLoad checks appointment times in 15 mins
            CheckRemider();
        }

        // loads all Appointments from Database
        private async Task LoadAppointments()
        {
            _eventAggregator.GetEvent<AppointmentIsLoadingEvent>().Publish(true);
            var appointments = await _appointmentDataService.GetAppointmentsAsync();

            _allAppointments.Clear();
            appointments.ForEach(app => _allAppointments.Add(app));



            _eventAggregator.GetEvent<AppointmentIsLoadingEvent>().Publish(false);
        }

        // Loading Appointments on selected Customer changed
        private void OnSelectedCustomerChanged(int customerId)
        {
            _eventAggregator.GetEvent<AppointmentIsLoadingEvent>().Publish(true);
            var cusAppointments = _allAppointments.Where(a => a._CustomerId == customerId).ToList();

            cusAppointments.Sort((a, b) => a._Start.CompareTo(b._Start));

            _customerAppointments.Clear();
            cusAppointments.ForEach(app => _customerAppointments.Add(app));

            _eventAggregator.GetEvent<AppointmentIsLoadingEvent>().Publish(false);
        }

        private void OnNewCustomerView(string obj)
        {
            Controls[ApplicationControls.CustomerDetail] = new CustomerDetailControl(
                    new CustomerDetailViewModel(_eventAggregator, new CustomerWrapper()));
            CurrentContent = Controls[ApplicationControls.CustomerDetail];
        }

        private async void OnNewCustomerCreate(CustomerWrapper newCustomer)
        {
            Controls[ApplicationControls.CustomerDetail] = null;
            CurrentContent = Controls[ApplicationControls.CustomersList];

            _eventAggregator.GetEvent<CustomerIsLoadingEvent>().Publish(true);

            if (newCustomer != null)
            {
                //catches exeption if phone number is not a number
                try
                {
                    if (newCustomer._ExistingCustomer)
                    {
                        await _customerDataService.UpdateCustomerAsync(newCustomer, _user);
                    }
                    else
                    {
                        await _customerDataService.SaveNewCustomerAsync(newCustomer, _user);
                    }
                }
                catch (InvalidCustomerDataException ex)
                {
                    MessageBox.Show(ex.Message, "Error");
                }

                await LoadCustomerData();
            }
            _eventAggregator.GetEvent<CustomerIsLoadingEvent>().Publish(false);
        }

        private void OnCustomerModify(int selectedCustomerId)
        {
            var selectedCustomer = _customers.First(c => c._Id == selectedCustomerId);

            selectedCustomer._ExistingCustomer = true;
            Controls[ApplicationControls.CustomerDetail] = new CustomerDetailControl(
                        new CustomerDetailViewModel(_eventAggregator, selectedCustomer));
            CurrentContent = Controls[ApplicationControls.CustomerDetail];
        }

        private async Task OnCustomerDelete(int id)
        {
            var selectedCustomer = FindCustomerById(id);
            if (selectedCustomer == null)
                throw new ArgumentException("No user");

            var selCusAppointmnets = _allAppointments.Where(app => app._CustomerId == selectedCustomer._Id).ToList();

            Controls[ApplicationControls.CustomerDetail] = null;
            CurrentContent = Controls[ApplicationControls.CustomersList];

            _eventAggregator.GetEvent<CustomerIsLoadingEvent>().Publish(true);

            await _customerDataService.DeleteCustomerAsync(selectedCustomer);
            await _appointmentDataService.DeleteAppointmentListAsync(selCusAppointmnets);
            _customers.Remove(selectedCustomer);
            selCusAppointmnets.ForEach(app => _allAppointments.Remove(app));
            _customerAppointments.Clear();

            _eventAggregator.GetEvent<CustomerIsLoadingEvent>().Publish(false);
            _eventAggregator.GetEvent<AppointmentIsLoadingEvent>().Publish(false);
        }

        private void OnAppointmentModify(int selectedAppointmentId)
        {
            var selectedAppointment = FindAppointmnetById(selectedAppointmentId);
            var selectedCustomer = FindCustomerById(selectedAppointment._CustomerId);
            selectedAppointment.ExistingAppointment = true;
            Controls[ApplicationControls.AppointmentDetail] = new AppointmentDetailControl(
                        new AppointmentDetailViewModel(_eventAggregator, selectedCustomer, selectedAppointment));
            CurrentContent = Controls[ApplicationControls.AppointmentDetail];
        }

        private async Task OnAppointmentDelete(int selectedAppointmentId)
        {
            var selectedAppointment = FindAppointmnetById(selectedAppointmentId);
            var selectedCustomer = FindCustomerById(selectedAppointment._CustomerId);
            selectedCustomer._AppointmentCount--;

            if (selectedAppointment == null)
                throw new ArgumentException("No such appointment");

            Controls[ApplicationControls.AppointmentDetail] = null;
            CurrentContent = Controls[ApplicationControls.AppointmentsList];

            _eventAggregator.GetEvent<AppointmentIsLoadingEvent>().Publish(true);

            await _appointmentDataService.DeleteAppointmentAsync(selectedAppointment, _user);
            _allAppointments.Remove(selectedAppointment);
            _customerAppointments.Remove(selectedAppointment);

            _eventAggregator.GetEvent<AppointmentIsLoadingEvent>().Publish(false);
        }

        private CustomerWrapper FindCustomerById(int id)
        {
            return _customers.ToList().FirstOrDefault(c => c._Id == id);
        }

        private AppointmentWrapper FindAppointmnetById(int id)
        {
            return _allAppointments.ToList().FirstOrDefault(app => app._AppointmentId == id);
        }


        private void OnCustomersControlLoad()
        {
            CurrentContent = Controls[ApplicationControls.CustomersList];
        }


        private void OnAppointmentsControlLoad()
        {
            CurrentContent = Controls[ApplicationControls.AppointmentsList];
        }
        private void OnCalendarControlLoad()
        {
            CurrentContent = Controls[ApplicationControls.Calendar];
        }

        private void OnAddAppointmentView(int customerId)
        {
            if (customerId >= 0)
            {
                var selectedCustomer = _customers.ToList().FindLast(c => c._Id == customerId);

                Controls[ApplicationControls.AppointmentDetail] = new AppointmentDetailControl(
                       new AppointmentDetailViewModel(_eventAggregator, selectedCustomer,
                       new AppointmentWrapper { _CustomerId = selectedCustomer._Id }));
                CurrentContent = Controls[ApplicationControls.AppointmentDetail];
            }
        }

        private async void OnNewAppointmentCreate(AppointmentWrapper appointment)
        {
            Controls[ApplicationControls.AppointmentDetail] = null;
            CurrentContent = Controls[ApplicationControls.AppointmentsList];

            _eventAggregator.GetEvent<AppointmentIsLoadingEvent>().Publish(true);

            if (appointment != null)
            {
                if (appointment.ExistingAppointment)
                {
                    await _appointmentDataService.ModifyAppointmentAsync(appointment, _user);
                    OnSelectedCustomerChanged(appointment._CustomerId);
                }
                else
                {
                    await _appointmentDataService.SaveNewAppointmentAsync(appointment, _user);
                    OnSelectedCustomerChanged(appointment._CustomerId);
                }
                await LoadAppointments();
                _customerAppointments.Remove(appointment);
                _customerAppointments.Add(appointment);
                SortAppointments(_customerAppointments);

                var customer = FindCustomerById(appointment._CustomerId);
                customer._AppointmentCount++;
            }
            _eventAggregator.GetEvent<AppointmentIsLoadingEvent>().Publish(false);
        }

        private void SortAppointments(ObservableCollection<AppointmentWrapper> appToSort)
        {
            var listToSort = appToSort.ToList();
            listToSort.Sort((a, b) => a._Start.Date.CompareTo(b._Start.Date));
            _customerAppointments.Clear();
            listToSort.ForEach(a => _customerAppointments.Add(a));
        }

        private void OnReportControlLoad()
        {
            CurrentContent = Controls[ApplicationControls.Report];
            _eventAggregator.GetEvent<ReportButtonClickedEvent>().Publish(true);
        }

        //Checks appointmets start time in next 15 minutes
        private void CheckRemider()
        {
            if (_allAppointments.Count == 0)
                return;

            var currentTimeUtc = DateTime.UtcNow;
            var sb = new StringBuilder();

            foreach (var app in _allAppointments)
            {
                var timeUntilApp = app._Start - currentTimeUtc;

                if (timeUntilApp.Hours == 0 && timeUntilApp.Minutes >= 0 && timeUntilApp <= new TimeSpan(0, REMINDER_TIME, 0))
                {
                    var appTime = TimeZoneInfo.ConvertTimeFromUtc(app._Start, TimeZoneInfo.Local);
                    sb.Append($"You have a meeting with {app._CustomerName} at {appTime.ToString("h mm tt")}\n");
                }
            }

            if (sb.Length > 0)
                MessageBox.Show(sb.ToString(), "Reminder");

        }

        // Subscribing to events
        private void InitializeEvents()
        {
            _eventAggregator.GetEvent<NewCustomerViewEvent>().Subscribe(OnNewCustomerView);
            _eventAggregator.GetEvent<NewCustomerCreatedEvent>().Subscribe(OnNewCustomerCreate);
            _eventAggregator.GetEvent<AddAppointmentEvent>().Subscribe(OnAddAppointmentView);
            _eventAggregator.GetEvent<SaveNewAppointmentEvent>().Subscribe(OnNewAppointmentCreate);
            _eventAggregator.GetEvent<SelectedCustomerChangedEvent>().Subscribe(OnSelectedCustomerChanged);

            CustomerListItemControl.ModifyCustomerEvent += ((id) => OnCustomerModify(id));
            CustomerListItemControl.DeleteCustomerEvent += (async (id) => await OnCustomerDelete(id));

            AppListItemControl.ModifyAppointmentEvent += ((id) => OnAppointmentModify(id));
            AppListItemControl.DeleteAppointmentEvent += (async (id) => await OnAppointmentDelete(id));
        }

        // Pings www.google.com for internet connection
        private async Task<bool> CheckInternetConnection()
        {
            Ping myPing = new Ping();
            try
            {
                var pingReplay = await myPing.SendPingAsync("google.com");
                if (pingReplay.Status == IPStatus.Success)
                    return true;
            }
            catch (Exception)
            {
                return false;
            }
            return false;
        }
        #endregion
    }
}
