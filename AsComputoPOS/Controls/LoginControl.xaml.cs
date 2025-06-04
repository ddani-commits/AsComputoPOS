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
            _authenticationService.HasUsers();
            _authenticationService.Login(_email, _password);
        }
    }
}
