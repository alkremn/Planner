using System;

namespace Planner.UI.Exceptions
{
    public class AuthenticationException : Exception
    {
        public AuthenticationException(string message)
            :base(message)
        {
        }
    }
}
