using Planner.UI.ViewModel;
using System.Security;
using System.Windows.Controls;

namespace Planner.UI.Pages
{
    /// <summary>
    /// Interaction logic for LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Page, IHavePassword
    {
        public LoginPage(ILoginViewModel loginVM)
        {
            InitializeComponent();
            DataContext = loginVM;
        }

        public SecureString SecurePassword => PasswordText.SecurePassword;

    }
}
