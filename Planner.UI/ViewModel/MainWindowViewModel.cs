using Planner.DataAccess;
using Planner.UI.Data;
using Planner.UI.DataService;
using Planner.UI.Events;
using Planner.UI.Pages;
using Prism.Events;
using System.Windows.Controls;

namespace Planner.UI.ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {
        private Page _currentPage;
        private IEventAggregator _eventAggregator;
        private ICustomerDataService _customerDataService;
        private IAppointmentDataService _appointmentDataService;
        private IAuthenticationDataService _authDataService;

        public MainWindowViewModel(IEventAggregator eventAggregator)
        {
            _customerDataService = new CustomerDataService(() => new PlannerDbContext());
            _appointmentDataService = new AppointmentDataService(() => new PlannerDbContext());
            _authDataService = new AuthenticationDataService(() => new PlannerDbContext());
            _eventAggregator = eventAggregator;
            CurrentPage = new LoginPage(new LoginViewModel(_authDataService, _eventAggregator));
            _eventAggregator.GetEvent<UserAuthenticationEvent>().Subscribe(OnUserAuthenticated);

        }

        public Page CurrentPage
        {
            get { return _currentPage; }
            set
            {
                OnPropertyChanged(ref _currentPage, value);
            }
        }
        private void OnUserAuthenticated(User user)
        {
            CurrentPage = new MainPage(
                new MainPageViewModel(_customerDataService, _appointmentDataService, _authDataService, user));
        }
    }
}
