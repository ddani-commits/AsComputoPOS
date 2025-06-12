using System.Diagnostics;
using System.Windows.Controls;
using TamoPOS.Services;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TamoPOS.Controls
{
    public partial class LoginControl : UserControl
    {
        private IAuthenticationService _authenticationService;
        private string _email = string.Empty;
        private string _password = string.Empty;

        public string Password
        {
            get => _password;
            set { _password = value; OnPropertyChanged(); }
        }

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
            var password = PasswordBox.Password; 
            _authenticationService.Login(email, password);
            Debug.WriteLine($"Login attempt with Email: {email} and Password: {password}");
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
