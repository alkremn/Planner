using System.Collections.Generic;
using System.Security;
using System.Threading.Tasks;
using Planner.DataAccess;

namespace Planner.UI.Data
{
    public interface IAuthenticationDataService
    {
        Task<User> UserAuthenticateAsync(string username, SecureString password);

        Task<List<User>> GetAllUsersAsync();
    }
}