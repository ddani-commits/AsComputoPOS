using System.Windows.Controls;
using TamoPOS.Services;
using System.ComponentModel;

namespace TamoPOS.Controls
{
    public partial class LoginControl : UserControl
    {
        private IAuthenticationService? _authenticationService;

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
            _authenticationService!.Login(Email.Text, PasswordBox.Password);
        }

        private void Input_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                _authenticationService?.Login(Email.Text, PasswordBox.Password);
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Email.Focus();
        }
    }
}
