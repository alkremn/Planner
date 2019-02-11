using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using Planner.UI.Events;
using Planner.UI.Model;
using System.Globalization;
using Planner.DataAccess;

namespace Planner.UI.ViewModel
{
    public class ReportViewModel : ViewModelBase, IReportViewModel
    {
        private IEventAggregator _eventAggregator;
        private ObservableCollection<AppointmentWrapper> _appointments;
        private ObservableCollection<CustomerWrapper> _customers;
        private ObservableCollection<User> _users;
        private bool _monthTypeLabelIsVisible;
        private bool _dataIsLoading = true;

        public ReportViewModel(IEventAggregator eventAggregator, ObservableCollection<CustomerWrapper> customers,
           ObservableCollection<AppointmentWrapper> appointments, ObservableCollection<User> users)
        {
            _eventAggregator = eventAggregator;
            _customers = customers;
            _appointments = appointments;
            _users = users;
            AppTypeCountList = new ObservableCollection<AppTypeCountByMonth>();
            AppTimeByConList = new ObservableCollection<AppTimeByConsultant>();
            Customers = new ObservableCollection<CustomerWrapper>();
            _eventAggregator.GetEvent<AppointmentIsLoadingEvent>().Subscribe(OnAppointmentLoading);
        }

        // Public Properties
        public ObservableCollection<AppTypeCountByMonth> AppTypeCountList { get; set; }
        public ObservableCollection<AppTimeByConsultant> AppTimeByConList { get; set; }
        public ObservableCollection<CustomerWrapper> Customers { get; set; }

        public bool MonthTypeLabelIsVisible
        {
            get { return _monthTypeLabelIsVisible; }
            set { OnPropertyChanged(ref _monthTypeLabelIsVisible, value); }
        }

        public bool DataIsLoading
        {
            get { return _dataIsLoading; }
            set { OnPropertyChanged(ref _dataIsLoading, value); }
        }

        private void GenerateAppTypesByMonthReport(List<AppointmentWrapper> appointments)
        {
            AppTypeCountList.Clear();
            appointments.ForEach((app) => 
            {
                string month = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(app._Start.Date.Month);
                var appointment = GetAppointByMonth(AppTypeCountList, month);

                if (appointment == null)
                {
                    appointment = new AppTypeCountByMonth();
                    if(app._Type == "New Appt")
                    {
                        appointment.NewAppointment++;
                    }
                    else
                    {
                        appointment.FollowUp++;
                    }
                    appointment.Month = month;
                    AppTypeCountList.Add(appointment);
                }
                else
                {
                    if (app._Type == "New Appt")
                    {
                        appointment.NewAppointment++;
                    }
                    else
                    {
                        appointment.FollowUp++;
                    }
                }
            });
        }

        private void GenerateConsultantsReport(List<AppointmentWrapper> appointments, List<User> users)
        {
            AppTimeByConList.Clear();
            
            foreach(var user in users)
            {
                var userAppointments = _appointments.Where(app => int.Parse(app._CreatedBy) == user.UserId);

                if (userAppointments.Count() > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    if(userAppointments.Count() > 0)
                    {
                        foreach(var app in userAppointments)
                        {
                            var appStartTime = TimeZoneInfo.ConvertTimeFromUtc(app._Start, TimeZoneInfo.Local);
                            var appEndTime = TimeZoneInfo.ConvertTimeFromUtc(app._End, TimeZoneInfo.Local);
                            sb.Append($"| {appStartTime.ToString("h:mm tt")} - {appEndTime.ToString("h:mm tt")} ");
                        }
                        sb.Append(" |");
                    }
                    AppTimeByConList.Add(new AppTimeByConsultant
                    {
                        ConsultName = user.UserName,
                        AppointmentTimes = sb.ToString()
                    });
                }
            }
        }

        private void GenetateCustomerNumberReport(List<CustomerWrapper> customers)
        {
            Customers.Clear();
            customers.ForEach(c => Customers.Add(c)); // Using lambda to add each customer from the list to Observable collection
        }

        private void OnAppointmentLoading(bool isLoading)
        {
            if (!isLoading)
            {
                GenerateAppTypesByMonthReport(_appointments.ToList());
                GenerateConsultantsReport(_appointments.ToList(), _users.ToList());
                GenetateCustomerNumberReport(_customers.ToList());

                if (AppTypeCountList.Count == 0)
                {
                    _monthTypeLabelIsVisible = true;
                }
                DataIsLoading = false;
            }
        }
      
        private AppTypeCountByMonth GetAppointByMonth(ObservableCollection<AppTypeCountByMonth> collection, string month)
        {
            foreach(var app in collection)
            {
                if(app.Month == month)
                {
                    return app;
                }
            }
            return null;
        }
    }
}
