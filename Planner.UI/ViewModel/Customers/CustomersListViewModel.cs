using Planner.UI.Events;
using Prism.Events;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace Planner.UI.ViewModel
{
    public class CustomersListViewModel : ViewModelBase, ICustomersListViewModel
    {
        private IEventAggregator _eventAggregator;
        private ObservableCollection<CustomerWrapper> _customers;

        public ObservableCollection<CustomerWrapper> Customers
        {
            get { return _customers; }
            set
            {
                OnPropertyChanged(ref _customers, value);
            }
        }
       
        private Visibility _loadingVisibility = Visibility.Visible;
        private Visibility _listVisibility = Visibility.Hidden;
        private Visibility _noResultsVisibility = Visibility.Hidden;

        public Visibility LoadingVisibility
        {
            get { return _loadingVisibility; }
            set { OnPropertyChanged(ref _loadingVisibility, value); }
        }

        public Visibility ListVisibility
        {
            get { return _listVisibility; }
            set { OnPropertyChanged(ref _listVisibility, value); }
        }

        public Visibility NoResultsVisibility
        {
            get { return _noResultsVisibility; }
            set { OnPropertyChanged(ref _noResultsVisibility, value); }
        }

        public CustomersListViewModel(IEventAggregator eventAggregator, ObservableCollection<CustomerWrapper> customers)
        {
            _eventAggregator = eventAggregator;
            _customers = customers;

            _eventAggregator.GetEvent<CustomerIsLoadingEvent>().Subscribe(OnCustomerSaving);

            
            NewCustomerCommand = new RelayCommand(OnNewCustomerCreate);
        }

        public ICommand NewCustomerCommand { get; private set; }

        private void OnCustomerSaving(bool isSaving)
        {
            if (isSaving)
            {
                LoadingVisibility = Visibility.Visible;
                ListVisibility = Visibility.Hidden;
                NoResultsVisibility = Visibility.Hidden;
            }
            else
            {
                LoadingVisibility = Visibility.Hidden;
                ListVisibility = Visibility.Visible;

                if (_customers.Count > 0)
                    NoResultsVisibility = Visibility.Hidden;
                else
                    NoResultsVisibility = Visibility.Visible;
            }   
        }

        private void OnNewCustomerCreate()
        {
            _eventAggregator.GetEvent<NewCustomerViewEvent>().Publish(null);
        }
    }
}
