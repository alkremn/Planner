using System;
using System.ComponentModel;

namespace Planner.UI
{
    public class AppointmentWrapper : IDataErrorInfo
    {
        public bool ExistingAppointment { get; set; }

        public int _AppointmentId { get; set; }

        public int _CustomerId { get; set; }

        public string _CustomerName { get; set; }

        public string _Title { get; set; }

        public string _Description { get; set; }

        public string _Location { get; set; }

        public string _Contact { get; set; }

        public DateTime _Start { get; set; }

        public DateTime _End { get; set; }

        public DateTime _CreateDate { get; set; }

        public string _CreatedBy { get; set; }

        public DateTime _LastUpdate { get; set; }

        public string _LastUpdateBy { get; set; }

        public DateTime? _SelectedDate { get; set; } = null;

        public string _Type { get; set; }

        //creates Start and End time of the appointment
        public void CreateStartEnd(TimeSpan start, TimeSpan end)
        {
            var selectedDate = _SelectedDate ?? new DateTime();
            var startTime = new DateTime(selectedDate.Year, selectedDate.Month, selectedDate.Day, start.Hours, start.Minutes, 0);
            var endTime = new DateTime(selectedDate.Year, selectedDate.Month, selectedDate.Day, end.Hours, end.Minutes, 0);

            //Converts time to UTC time
            _Start = TimeZoneInfo.ConvertTimeToUtc(startTime, TimeZoneInfo.Local);
            _End = TimeZoneInfo.ConvertTimeToUtc(endTime, TimeZoneInfo.Local);
        }

        //error checking methods and properties
        public string Error => null;

        public string resultError = "";

        public string this[string propertyName]
        {
            get
            {
                string result = "";

                switch (propertyName)
                {
                    case "_Title":
                        if (string.IsNullOrEmpty(_Title))
                            result = "Title is required!";
                        break;
                    case "_Description":
                        if (string.IsNullOrEmpty(_Description))
                            result = "Description is required!";
                        break;
                    case "_Location":
                        if (string.IsNullOrEmpty(_Location))
                            result = "Location is required!";
                        break;
                    case "_Contact":
                        if (string.IsNullOrEmpty(_Contact))
                            result = "Contact is required!";
                        break;
                    case "_SelectedDate":
                        if (_SelectedDate == null)
                            result = "Date is required!";
                        break;
                }
                return result;
            }
        }

        //returns true if all field are not empty
        public bool IsValid()
        {
            return !(string.IsNullOrWhiteSpace(_Title) || string.IsNullOrWhiteSpace(_Description)
                    || string.IsNullOrWhiteSpace(_Location) || string.IsNullOrWhiteSpace(_Contact) ||
                    string.IsNullOrEmpty(_Type) || !_SelectedDate.HasValue);
        }
    }
}
