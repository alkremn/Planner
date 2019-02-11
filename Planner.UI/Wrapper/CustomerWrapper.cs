using FamFamFam.Flags.Wpf;
using System;
using System.ComponentModel;
using System.Linq;

namespace Planner.UI
{
    public class CustomerWrapper : IDataErrorInfo
    {
        private static readonly int firstNameMaxLength = 20;
        private static readonly int lastNameMaxLength = 24;
        private static readonly int addressMaxLength = 50;
        private static readonly int phoneNumberMaxLength = 19;
        private static readonly int postalCodeMaxLength = 10;


        private string _fullname;

        public int _Id { get; set; }

        public bool _Active { get; set; }

        public int _AddressId { get; set; }

        public int _CityId { get; set; }

        public int _CountryId { get; set; }

        public string _Firstname { get; set; }

        public string _Lastname { get; set; }

        public DateTime _CreatedDate { get; set; }

        public string _CreatedBy { get; set; }

        public int _AppointmentCount { get; set; }

        public bool _ExistingCustomer { get; set; } = false;


        //on set split Fullname to Firstname and Lastname
        public string _Fullname
        {
            get { return _Firstname + " " + _Lastname; }
            set
            {
                _fullname = value;
                var listOfNames = _fullname.Trim().Split(' ');
                _Firstname = listOfNames[0];
                _Lastname = listOfNames[listOfNames.Length - 1];
            }
        }

        public CountryData _SelectedCountry
        {
            get { return CountryData.AllCountries.FirstOrDefault(c => c.Name == _Country); }
            set
            {
                _Country = value.Name;
                _CountryIsValid = true;
            }
        }

        public bool _CountryIsValid { get; set; } = false;

        public string _Address1 { get; set; }

        public string _Address2 { get; set; }

        public string _PhoneNumber { get; set; }

        public string _City { get; set; }

        public string _PostalCode { get; set; }

        public string _Country { get; set; }


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
                    case "_Firstname":
                        if (string.IsNullOrWhiteSpace(_Firstname))
                            result = "Fistname is required!";
                        if (_Firstname != null && _Firstname.Length > firstNameMaxLength)
                            result = "Firstname is too long";
                        break;
                    case "_Lastname":
                        if (string.IsNullOrWhiteSpace(_Lastname))
                            result = "Lastname is required!";
                        if (_Lastname != null && _Lastname.Length > lastNameMaxLength)
                            result = "Lastname is too long";
                        break;
                    case "_Address1":
                        if (string.IsNullOrWhiteSpace(_Address1))
                        {
                            result = "Address is required!";
                        }
                        else if(_Address1.Length >= addressMaxLength){
                            result = "Address is too long!";
                        }
                        break;
                    case "_Address2":
                        if (!string.IsNullOrWhiteSpace(_Address2) && _Address2.Length >= addressMaxLength)
                            result = "Address is too long!";
                        break;
                    case "_PhoneNumber":
                        if (string.IsNullOrEmpty(_PhoneNumber))
                        {
                            result = "Phone number is required!";
                        }
                        else if (!(_PhoneNumber.Length < phoneNumberMaxLength && IsNumber(_PhoneNumber)))
                        {
                            result = "Phone number is Invalid!";
                        }
                        break;
                    case "_City":
                        if (string.IsNullOrWhiteSpace(_City))
                            result = "City is required!";
                        break;
                    case "_PostalCode":
                        if (string.IsNullOrWhiteSpace(_PostalCode))
                            result = "Postal Code is required!";
                        if (_PostalCode != null && _PostalCode.Length >= postalCodeMaxLength)
                            result = "Postal Code is too long!";
                        break;
                    case "_SelectedCountry":
                        if (string.IsNullOrWhiteSpace(_SelectedCountry.ToString()))
                            result = "Country is required!";
                        break;
                }
                return result;
            }
        }
        //returns true if all fields are valid
        public bool IsValid()
        {
            var firstNameIsValid = (!string.IsNullOrWhiteSpace(_Firstname)) && _Firstname.Length <= firstNameMaxLength;
            var lastNameIsValid = (!string.IsNullOrWhiteSpace(_Lastname)) && _Lastname.Length <= lastNameMaxLength;
            var address1IsValid = !string.IsNullOrWhiteSpace(_Address1) && _Address1.Length < addressMaxLength;
            var address2IsValid = string.IsNullOrWhiteSpace(_Address2) || _Address2.Length < addressMaxLength;
            var phoneNumberIsValid = (!string.IsNullOrWhiteSpace(_PhoneNumber) && _PhoneNumber.Length < phoneNumberMaxLength && IsNumber(_PhoneNumber));
            var cityIsValid = (!string.IsNullOrWhiteSpace(_City));
            var postalCodeIsValid = (!string.IsNullOrWhiteSpace(_PostalCode) && _PostalCode.Length < postalCodeMaxLength);
            var countryIsValid = _SelectedCountry.Name != null;

            return firstNameIsValid && lastNameIsValid && address1IsValid && address2IsValid && phoneNumberIsValid && cityIsValid
                && postalCodeIsValid && countryIsValid;
        }

        private bool IsNumber(string stringNumber)
        {
            try
            {
                long code = long.Parse(stringNumber);
            }
            catch (FormatException)
            {
                return false;
            }
            return true;
        }
    }
}