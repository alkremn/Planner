using Planner.UI.Events;
using Prism.Events;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Planner.UI.ViewModel
{
    class AppointmentDetailViewModel : ViewModelBase, IAppointmentDetailViewModel
    {
        #region Private members
        private const int START_TIME_HOUR = 8;
        private const int END_TIME_HOUR = 17;
        private const int MINITES_IN_HOUR = 60;
        private const int TIME_PACE = 15;

        private IEventAggregator _eventAggregator;
        private TimeSpan? _selectedStartTime = null;
        private TimeSpan? _selectedEndTime = null;
        private bool _typeIsSelected;
        private bool _startTimeIsSelected;
        private bool _endTimeIsSelected;
        
        #endregion


        public AppointmentDetailViewModel(IEventAggregator eventAggregator, CustomerWrapper customer, 
            AppointmentWrapper appointment)
        {
            _eventAggregator = eventAggregator;
            Customer = customer;
            Appointment = appointment;
            Type = new ObservableCollection<string> { "New Appt", "Follow up"};
            TypeSelected = Appointment._Type;
            AvailableStartTime = new ObservableCollection<TimeSpan>();
            AvailableEndTime = new ObservableCollection<TimeSpan>();
            CreateTime(new TimeSpan(START_TIME_HOUR,0,0), AvailableStartTime);

            if(Appointment._Title != null)
            {
                Appointment._SelectedDate = Appointment._Start;
                var startTime = TimeZoneInfo.ConvertTimeFromUtc(Appointment._Start, TimeZoneInfo.Local);
                var endTime = TimeZoneInfo.ConvertTimeFromUtc(Appointment._End, TimeZoneInfo.Local);
                SelectedStartTime = new TimeSpan(startTime.Hour, startTime.Minute, 0);
                CreateTime(SelectedStartTime ?? new TimeSpan(), AvailableStartTime);
                SelectedEndTime = new TimeSpan(endTime.Hour, endTime.Minute, 0);
            }
            SaveCommand = new RelayCommand(OnAppointmentSave, CanAppointmentSave);
            CancelCommand = new RelayCommand(OnCancel);
        }

        public CustomerWrapper Customer { get; set; }
        public AppointmentWrapper Appointment { get; set; }
        public ObservableCollection<string> Type { get; set; }
        public ObservableCollection<TimeSpan> AvailableStartTime { get; set; }
        public ObservableCollection<TimeSpan> AvailableEndTime { get; set; }

        public ICommand SaveCommand { get; private set; }
        public ICommand CancelCommand { get; private set; }

        private string _typeSelected;

        public string TypeSelected
        {
            get { return _typeSelected; }
            set
            {
                OnPropertyChanged(ref _typeSelected, value);
                Appointment._Type = value;
                if(value != null)
                    TypeIsSelected = true;
            }
        }

        public TimeSpan? SelectedStartTime
        {
            get { return _selectedStartTime; }
            set
            {
                OnPropertyChanged(ref _selectedStartTime, value);
                var time = value ?? new TimeSpan();
                if (time.Hours < 0)
                    return;
              
                StartTimeIsSelected = true;
                CreateTime(time + new TimeSpan(0,15,0), AvailableEndTime);
            }
        }

        public TimeSpan? SelectedEndTime
        {
            get { return _selectedEndTime; }
            set
            {
                OnPropertyChanged(ref _selectedEndTime, value);
                var time = value ?? new TimeSpan();
                if (time.Hours < 0)
                    return;

                EndTimeIsSelected = true;
            }
        }

        public bool StartTimeIsSelected
        {
            get { return _startTimeIsSelected; }
            set
            {
                OnPropertyChanged(ref _startTimeIsSelected, value);
            }
        }

        public bool EndTimeIsSelected
        {
            get { return _endTimeIsSelected; }
            set
            {
                OnPropertyChanged(ref _endTimeIsSelected, value);
                _endTimeIsSelected = true;
            }
        }

        public bool TypeIsSelected
        {
            get { return _typeIsSelected; }
            set
            {
                OnPropertyChanged(ref _typeIsSelected, value);
            }
        }

        private void OnAppointmentSave()
        {
            Appointment.CreateStartEnd((SelectedStartTime ?? new TimeSpan()), SelectedEndTime ?? new TimeSpan());
            _eventAggregator.GetEvent<SaveNewAppointmentEvent>().Publish(Appointment);
        }

        private void OnCancel()
        {
            _eventAggregator.GetEvent<SaveNewAppointmentEvent>().Publish(null);
        }

         private bool CanAppointmentSave()
        {
            return Appointment.IsValid() && StartTimeIsSelected && EndTimeIsSelected;
        }

        private void CreateTime(TimeSpan startTime, ObservableCollection<TimeSpan> timeCollection)
        {
            timeCollection.Clear();
            int mins = startTime.Minutes;
            for (int hour = startTime.Hours; hour < END_TIME_HOUR; hour++)
            {
                for(int min = mins; min < MINITES_IN_HOUR; min += TIME_PACE)
                {
                    timeCollection.Add(new TimeSpan(hour, min, 0));
                }
                mins = 0;
            }
            OnPropertyChanged(nameof(AvailableEndTime));
        }
    }
}
