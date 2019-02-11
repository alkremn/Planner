using Planner.DataAccess;
using Planner.UI.Data;
using Planner.UI.Events;
using Prism.Events;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.IO;
using System;
using Planner.UI.Exceptions;

namespace Planner.UI.ViewModel
{
    public class LoginViewModel : ViewModelBase, ILoginViewModel
    {
        private const string FILE_NAME = "user_log.txt";

        private readonly IEventAggregator _eventAggregator;

        #region Private Fields

        private readonly IAuthenticationDataService _authDataService;
        private string _errorText;

        #endregion

        #region Constructors

        public LoginViewModel(IAuthenticationDataService authDataService,
            IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            _authDataService = authDataService;
            LoginCommand = new RelayCommand<object>(async (parameter) => await LoginAsync(parameter), (parameter) => CanLogin());
        }
        #endregion

        public string Username { get; set; }

        public string ErrorText
        {
            get { return _errorText; }
            set { OnPropertyChanged(ref _errorText, value); }
        }


        private bool _loginIsRunning;
        public bool LoginIsRunning
        {
            get { return _loginIsRunning; }
            set
            {
                OnPropertyChanged(ref _loginIsRunning, value);
            }
        }

        public ICommand LoginCommand { get; set; }


        public async Task LoginAsync(object parameter)
        {
            if (LoginIsRunning)
                return;

            User authUser = null;
            ErrorText = "";
            var password = (parameter as IHavePassword).SecurePassword;

            var validationMessage = ValidateUserInput(Username, password);
            if (validationMessage.Length == 0)
            {
                LoginIsRunning = true;
                try
                {
                    authUser = await _authDataService.UserAuthenticateAsync(Username, password);
                    if (authUser != null)
                    {
                        SaveUserLoginInfo(authUser);
                        _eventAggregator.GetEvent<UserAuthenticationEvent>().Publish(authUser);
                    }
                }
                catch (AuthenticationException ex) // catches AuthenticationException is user does not exists in database
                {
                    ErrorText = ex.Message;
                }
                finally
                {
                    LoginIsRunning = false;
                }
               
            }
            else
            {
                ErrorText = validationMessage.ToString();
            }
        }

        #region Private Methods

        private bool CanLogin()
        {
            return !LoginIsRunning;
        }

        private StringBuilder ValidateUserInput(string username, SecureString password)
        {
            var errorMessage = new StringBuilder();

            if (string.IsNullOrEmpty(username))
                errorMessage.Append($"{Resources.GlobalStrings.UsernameLengthErrorText}\n");
               
            if (string.IsNullOrEmpty(password.Unsecure()))
                errorMessage.Append(Resources.GlobalStrings.PasswordLengthErrorText);

            return errorMessage;
        }

        // saves login information to the log file
        private void SaveUserLoginInfo(User user)
        {
            var currentDir = Directory.GetCurrentDirectory();

            var filePath = Path.Combine(currentDir, FILE_NAME);

            using (var file = new StreamWriter(filePath, true))
            {
                file.WriteLine($"{user.UserId},{user.UserName},{DateTime.Now}");
            }
        }
        #endregion
    }
}
