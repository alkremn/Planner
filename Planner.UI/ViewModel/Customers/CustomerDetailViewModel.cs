using FamFamFam.Flags.Wpf;
using Planner.UI.Events;
using Prism.Events;
using System;
using System.Windows;
using System.Windows.Input;

namespace Planner.UI.ViewModel
{
    class CustomerDetailViewModel : ViewModelBase, ICustomerDetailViewModel
    {
        private IEventAggregator _eventAggregator;
        private CountryData _selectedCountry;

        public bool CountryIsValid { get; set; }

        public CountryData SelectedCountry
        {
            get { return _selectedCountry; }
            set
            {
                OnPropertyChanged(ref _selectedCountry, value);
                Customer._SelectedCountry = value;
            }
        }

        public CustomerWrapper Customer { get; set; }

        public CustomerDetailViewModel(IEventAggregator eventAggregator, CustomerWrapper customer)
        {
            _eventAggregator = eventAggregator;
            Customer = customer;
            SelectedCountry = Customer._SelectedCountry;
            SaveCommand = new RelayCommand(OnCustomerSave, CanCustomerSave);
            CancelCommand = new RelayCommand(OnCancel);
        }

        public ICommand SaveCommand { get; private set; }
        public ICommand CancelCommand { get; private set; }

        private void OnCustomerSave()
        {
            _eventAggregator.GetEvent<NewCustomerCreatedEvent>().Publish(Customer);
        }

        private void OnCancel()
        {
            _eventAggregator.GetEvent<NewCustomerCreatedEvent>().Publish(null);
            bool isvalid = Customer.IsValid();
        }

        private bool CanCustomerSave()
        {
            return Customer.IsValid();
        }
    }
}
