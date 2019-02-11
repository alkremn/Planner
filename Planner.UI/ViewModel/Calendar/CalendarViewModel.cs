using Planner.UI.Events;
using Planner.UI.Model;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace Planner.UI.ViewModel
{
    public class CalendarViewModel : ViewModelBase, ICalendarViewModel
    {
        private const int YEAR_PERIOD = 10;
        private IEventAggregator _eventAggregator;
        private bool _appointmentIsLoading = true;
        private bool _noAppointmentLabelIsVisible;
        private ObservableCollection<AppointmentWrapper> _appointments;
        private string _selectedMonth;
        private string _selectedYear;
        private string _selectedDateLabel;
        private string _monthWeekLabel;
        private Day _selectedDate;

        public CalendarViewModel(IEventAggregator eventAggregator, ObservableCollection<AppointmentWrapper> appointments)
        {
            _eventAggregator = eventAggregator;
            _appointments = appointments;
            Days = new ObservableCollection<Day>();
            CurrentWeek = new ObservableCollection<Day>();
            CurrentAppointments = new ObservableCollection<AppointmentWrapper>();
            Years = new ObservableCollection<string>();
            BuildYears(DateTime.Now);

            BuildCurrentCalendarMonth();
            BuildCurrentWeek();

            MonthCommand = new RelayCommand(OnMonthDisplayChanged, CanMonthDisplayChanged);
            WeekCommand = new RelayCommand(OnWeekDisplayChanged, CanWeekDispayChanged);
            ShowMonthCommand = new RelayCommand(OnShowMonthChanged, CanShowMonthChanged);
            _eventAggregator.GetEvent<AppointmentIsLoadingEvent>().Subscribe(OnAppointmentLoading);
        }

        public ICommand MonthCommand { get; set; }
        public ICommand WeekCommand { get; set; }
        public ICommand ShowMonthCommand { get; set; }

        public ObservableCollection<Day> Days { get; set; }
        public ObservableCollection<Day> CurrentWeek { get; set; }
        public ObservableCollection<string> Months { get; set; } = new ObservableCollection<string>
        { "January", "February", "March", "April", "May", "June", "July", "August",
            "September", "October", "November", "December" };

        public ObservableCollection<string> Years { get; set; }

        public string SelectedMonth
        {
            get { return _selectedMonth; }
            set { OnPropertyChanged(ref _selectedMonth, value); }
        }

        public string SelectedDateLabel
        {
            get { return _selectedDateLabel; }
            set { OnPropertyChanged(ref _selectedDateLabel, value); }
        }

        public string MonthWeekLabel
        {
            get { return _monthWeekLabel; }
            set { OnPropertyChanged(ref _monthWeekLabel, value); }
        }

        public string SelectedYear
        {
            get { return _selectedYear; }
            set { OnPropertyChanged(ref _selectedYear, value); }
        }

        public ObservableCollection<AppointmentWrapper> CurrentAppointments { get; set; }

        public bool AppointmentIsLoading
        {
            get { return _appointmentIsLoading; }
            set
            {
                OnPropertyChanged(ref _appointmentIsLoading, value);

            }
        }

        public bool NoAppointmentLabelIsVisible
        {
            get { return _noAppointmentLabelIsVisible; }
            set
            {
                OnPropertyChanged(ref _noAppointmentLabelIsVisible, value);
            }
        }

        public Day SelectedDate
        {
            get { return _selectedDate; }
            set
            {
                OnPropertyChanged(ref _selectedDate, value);
                if (value != null && _appointments.Count > 0)
                    LoadDayAppointments(value);
            }
        }

        private int DayOfWeekNumber(DayOfWeek dow)
        {
            return Convert.ToInt32(dow.ToString("D"));
        }

        private void OnMonthDisplayChanged()
        {
            BuildCurrentCalendarMonth();
        }

        private bool CanMonthDisplayChanged()
        {
            return true;
        }

        private void OnWeekDisplayChanged()
        {
            BuildCalendar(DateTime.Today);
            SelectedMonth = Months[DateTime.Now.Month - 1];
            SelectedYear = DateTime.Now.Year.ToString();
            FilterCurrentAppointments(CurrentWeek.ToList());
            MonthWeekLabel = "This week";
        }

        private bool CanWeekDispayChanged()
        {
            return true;
        }

        // Show button action methods
        private bool CanShowMonthChanged()
        {
            return true;
        }

        private void OnShowMonthChanged()
        {
            SelectedDate = null;
            int selectedMonth = Months.IndexOf(SelectedMonth) + 1;
            int selectedYear = int.Parse(SelectedYear);
            BuildCalendar(new DateTime(selectedYear, selectedMonth, 1));
            FilterCurrentAppointments(Days.ToList());
            MonthWeekLabel = "";
            SelectedDateLabel = $"{Months[selectedMonth - 1]}, {selectedYear}";
        }

        private void OnAppointmentLoading(bool appIsLoading)
        {

            AppointmentIsLoading = appIsLoading;
            if (!appIsLoading)
            {
                OnMonthDisplayChanged();
            }
        }

        private void LoadDayAppointments(Day selectedDay)
        {
            CurrentAppointments.Clear();

            foreach (var app in _appointments)
            {
                var convertedAppTimeToUtc = TimeZoneInfo.ConvertTimeFromUtc(app._Start, TimeZoneInfo.Local);
                if (convertedAppTimeToUtc.Date == selectedDay.Date.Date)
                    CurrentAppointments.Add(app);
            }
            MonthWeekLabel = selectedDay.Date.Date.ToShortDateString();
            NoAppointmentLabelIsVisible = CurrentAppointments.Count > 0 ? false : true;
        }

        private void FilterCurrentAppointments(List<Day> days)
        {
            CurrentAppointments.Clear();

            days.ForEach(day =>
            {
                foreach (var app in _appointments)
                {
                    if (day.Date.Year == app._Start.Year && day.Date.Month == app._Start.Month && day.Date.Day == app._Start.Day)
                    {
                        CurrentAppointments.Add(app);
                    }
                }
            });
            if (!AppointmentIsLoading)
                NoAppointmentLabelIsVisible = CurrentAppointments.Count > 0 ? false : true;
        }

        private void BuildYears(DateTime targetDate)
        {
            var lowerBound = targetDate.Year - YEAR_PERIOD;
            var upperBound = targetDate.Year + YEAR_PERIOD;
            for (int year = lowerBound; year < upperBound; year++)
            {
                Years.Add(year.ToString());
            }
        }

        private void BuildCalendar(DateTime targetDate)
        {
            Days.Clear();

            DateTime d = new DateTime(targetDate.Year, targetDate.Month, 1);
            int offset = DayOfWeekNumber(d.DayOfWeek);
            if (offset != 1) d = d.AddDays(-offset);

            // 6 weeks  * 7 days = 42
            for (int i = 1; i <= 42; i++)
            {
                Day day = new Day { Date = d, IsTargetMonth = targetDate.Month == d.Month, IsToday = d == DateTime.Today };
                Days.Add(day);
                d = d.AddDays(1);
            }
        }

        private void BuildCurrentCalendarMonth()
        {
            BuildCalendar(DateTime.Today);
            SelectedMonth = Months[DateTime.Now.Month - 1];
            SelectedYear = DateTime.Now.Year.ToString();
            SelectedDateLabel = $"{SelectedMonth}, {SelectedYear}";
            MonthWeekLabel = "This Month";
            FilterCurrentAppointments(Days.ToList());
        }

        private void BuildCurrentWeek()
        {
            var CurrentWeekDay = DateTime.Today.Date.AddDays(-1 * (int)DateTime.Today.DayOfWeek);
            for (int i = 0; i < 7; i++)
            {
                Day day = new Day { Date = CurrentWeekDay, };
                CurrentWeek.Add(day);
                CurrentWeekDay = CurrentWeekDay.AddDays(1);
            }
        }
    }
}
