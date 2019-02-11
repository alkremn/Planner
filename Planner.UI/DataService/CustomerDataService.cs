using Planner.DataAccess;
using Planner.Model;
using Planner.UI.Exceptions;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace Planner.UI.DataService
{
    public class CustomerDataService : ICustomerDataService
    {
        private Func<PlannerDbContext> _contextCreator;

        //Constructor takes function pointer as a parameter
        public CustomerDataService(Func<PlannerDbContext> contextCreator)
        {
            _contextCreator = contextCreator;
        }

        // Gets all Customers from Database
        public async Task<List<CustomerWrapper>> GetAllCustomersAsync()
        {
            var customers = await Task.Run(async () =>
            {
                using (var context = _contextCreator())
                {
                    return await (from cus in context.Customers
                                  join add in context.Addresses
                                  on cus.AddressId equals add.AddressId
                                  join ci in context.Cities
                                  on add.CityId equals ci.CityId
                                  join country in context.Countries
                                  on ci.CountryId equals country.CountryId
                                  select new CustomerWrapper
                                  {
                                      _Id = cus.CustomerId,
                                      _Active = cus.Active,
                                      _CreatedDate = cus.CreateDate,
                                      _CreatedBy = cus.CreatedBy,
                                      _Fullname = cus.CustomerName,
                                      _AddressId = add.AddressId,
                                      _Address1 = add.Address1,
                                      _Address2 = add.Address2,
                                      _PhoneNumber = add.Phone,
                                      _PostalCode = add.PostalCode,
                                      _CityId = ci.CityId,
                                      _City = ci.City1,
                                      _CountryId = country.CountryId,
                                      _Country = country.Country1,
                                  }).ToListAsync();
                }
            });

            return await Task.Run(async () =>
            {
                using (var context = _contextCreator())
                {
                    foreach (var customer in customers)
                    {
                        int userId = int.Parse(customer._CreatedBy);
                        var user = await context.Users.SingleAsync(u => u.UserId == userId);
                        customer._CreatedBy = user.UserName;

                        int appointmentCount = 0;
                        
                        foreach(var app in context.Appointments)
                        {
                            if (customer._Id == app.CustomerId)
                                appointmentCount++;
                        }
                        customer._AppointmentCount = appointmentCount;
                    }
                    return customers;
                }
            });
           
        }

        public async Task<bool> DeleteCustomerAsync(CustomerWrapper selectedCustomer)
        {
            await Task.Run(async () =>
            {
                using (var context = _contextCreator())
                {
                    var customer = context.Customers.First(c => c.CustomerId == selectedCustomer._Id);
                    var address = context.Addresses.FirstOrDefault(a => a.AddressId == customer.AddressId);
                    var city = context.Cities.FirstOrDefault(ci => ci.CityId == address.CityId);
                    var country = context.Countries.FirstOrDefault(cnry => cnry.CountryId == city.CountryId);

                    context.Customers.Remove(customer);
                    context.Addresses.Remove(address);
                    context.Cities.Remove(city);
                    context.Countries.Remove(country);

                    context.Entry(customer).State = EntityState.Deleted;
                    context.Entry(address).State = EntityState.Deleted;
                    context.Entry(city).State = EntityState.Deleted;
                    context.Entry(country).State = EntityState.Deleted;

                    await context.SaveChangesAsync();
                }
            });
            return true;
        }

        public async Task SaveNewCustomerAsync(CustomerWrapper newCustomer, User user)
        {
            await Task.Run(async () =>
            {
                using (var context = _contextCreator())
                {
                    var customerId = context.Customers.Count();
                    var addressId = context.Addresses.Count();
                    var cityId = context.Cities.Count();
                    var countryId = context.Countries.Count();

                    var customer = CreateCustomer(customerId, addressId, newCustomer, user);
                    var address = CreateAddress(addressId, cityId, newCustomer, user);
                    var city = CreateCity(cityId, countryId, newCustomer, user);
                    var country = CreateCountry(countryId, newCustomer, user);

                    ValidateCustomerData(address);

                    context.Customers.Add(customer);
                    context.Addresses.Add(address);
                    context.Cities.Add(city);
                    context.Countries.Add(country);

                    context.Entry(customer).State = EntityState.Added;
                    context.Entry(address).State = EntityState.Added;
                    context.Entry(city).State = EntityState.Added;
                    context.Entry(country).State = EntityState.Added;

                    await context.SaveChangesAsync();
                }
            });
        }

        public async Task UpdateCustomerAsync(CustomerWrapper updatedCustomer, User user)
        {
            await Task.Run(async () =>
            {
                using (var context = _contextCreator())
                {
                    var customer = CreateCustomer(updatedCustomer._Id, updatedCustomer._AddressId, updatedCustomer, user);
                    var address = CreateAddress(updatedCustomer._AddressId, updatedCustomer._CityId, updatedCustomer, user);
                    var city = CreateCity(updatedCustomer._CityId, updatedCustomer._CountryId, updatedCustomer, user);
                    var country = CreateCountry(updatedCustomer._CountryId, updatedCustomer, user);

                    ValidateCustomerData(address);

                    context.Entry(customer).State = EntityState.Modified;
                    context.Entry(address).State = EntityState.Modified;
                    context.Entry(city).State = EntityState.Modified;
                    context.Entry(country).State = EntityState.Modified;

                    await context.SaveChangesAsync();
                }
            });
        }

        private Customer CreateCustomer(int customerId, int addressId, CustomerWrapper newCustomer, User user)
        {
            var dayOfCreation = newCustomer._CreatedDate;
            if (!newCustomer._ExistingCustomer)
                dayOfCreation = DateTime.UtcNow;

            return new Customer
            {
                CustomerId = customerId,
                CustomerName = newCustomer._Fullname,
                AddressId = addressId,
                Active = newCustomer._Active,
                CreateDate = dayOfCreation,
                CreatedBy = user.UserId.ToString(),
                LastUpdate = DateTime.UtcNow,
                LastUpdateBy = user.UserId.ToString()
            };
        }

        private Address CreateAddress(int addressId, int cityId, CustomerWrapper newCustomer, User user)
        {
            return new Address
            {
                AddressId = addressId,
                Address1 = newCustomer._Address1,
                Address2 = string.IsNullOrEmpty(newCustomer._Address2) ? "null" : newCustomer._Address2,
                CityId = cityId,
                Phone = newCustomer._PhoneNumber,
                PostalCode = newCustomer._PostalCode,
                CreateDate = DateTime.UtcNow,
                CreatedBy = user.UserId.ToString(),
                LastUpdate = DateTime.UtcNow,
                LastUpdateBy = user.UserId.ToString()
            };
        }

        private City CreateCity(int cityId, int countryId, CustomerWrapper newCustomer, User user)
        {
            return new City
            {
                CityId = cityId,
                City1 = newCustomer._City,
                CountryId = countryId,
                CreateDate = DateTime.UtcNow,
                CreatedBy = user.UserId.ToString(),
                LastUpdate = DateTime.UtcNow,
                LastUpdateBy = user.UserId.ToString()
            };
        }

        private Country CreateCountry(int countryId, CustomerWrapper newCustomer, User user)
        {
            return new Country
            {
                CountryId = countryId,
                Country1 = newCustomer._Country,
                CreateDate = DateTime.UtcNow,
                CreatedBy = user.UserId.ToString(),
                LastUpdate = DateTime.UtcNow,
                LastUpdateBy = user.UserId.ToString()
            };
        }

        //Checks if phone number is digits
        private bool ValidateCustomerData(Address address)
        {
            try
            {
                long num = long.Parse(address.Phone);
            }
            catch (FormatException)
            {
                throw new InvalidCustomerDataException("Invalid phone number");
            }
            return true;
        }


    }
}
