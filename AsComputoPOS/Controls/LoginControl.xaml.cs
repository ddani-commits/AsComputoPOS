using System.Diagnostics;
using System.Windows.Controls;
using UiDesktopApp1.Services;

namespace UiDesktopApp1.Controls
{
    public partial class LoginControl : UserControl
    {
        private IAuthenticationService _authenticationService;
        private string _email = string.Empty;
        private string _password = string.Empty;

        public LoginControl()
        {
            InitializeComponent();
        }

        public void SetAuthenticationService(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            var email = Email.Text;
            var password = Password.Text;
            _authenticationService.Login(email, password);
            Debug.WriteLine($"Login attempt with Email: {email} and Password: {password}");
        }
    }
}
