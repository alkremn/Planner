using System.Security;

namespace Planner.UI
{
    public interface IHavePassword
    {
        SecureString SecurePassword { get; }
    }
}
