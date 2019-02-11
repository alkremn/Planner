using Planner.DataAccess;
using Planner.UI.Exceptions;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Security;
using System.Threading.Tasks;

namespace Planner.UI.Data
{
    public class AuthenticationDataService : IAuthenticationDataService
    {
        private Func<PlannerDbContext> _contextCreator;

        public AuthenticationDataService(Func<PlannerDbContext> contextCreator)
        {
            _contextCreator = contextCreator;
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            return await Task.Run(async () =>
            {
                using (var context = _contextCreator())
                {
                    return await context.Users.ToListAsync();
                }
            });
        }

        public async Task<User> UserAuthenticateAsync(string username, SecureString password)
        {
            var user = await Task.Run(async () =>
            {
                using (var context = _contextCreator())
                {
                    var pass = password.Unsecure();

                    if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(pass))
                        throw new ArgumentException();

                    return await context.Users.SingleOrDefaultAsync(
                               u => u.UserName == username && u.Password == pass);
                }
            });

            if (user == null)
                throw new AuthenticationException(Resources.GlobalStrings.AuthenticatonErrorText);
            else
                return user;
        }
        public void CreateUser(User user)
        {
            using (var context = _contextCreator())
            {
                context.Users.Add(user);
                context.Entry(user).State = EntityState.Added;
                context.SaveChanges();
            }
        }
    }
}
