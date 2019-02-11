using System.Collections.Generic;
using System.Threading.Tasks;
using Planner.DataAccess;

namespace Planner.UI.DataService
{
    public interface ICustomerDataService
    {
        Task<List<CustomerWrapper>> GetAllCustomersAsync();

        Task SaveNewCustomerAsync(CustomerWrapper newCustomer, User user);

        Task<bool> DeleteCustomerAsync(CustomerWrapper selectedCustomer);

        Task UpdateCustomerAsync(CustomerWrapper updatedCustomer, User user);
    }
}