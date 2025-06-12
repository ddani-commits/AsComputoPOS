using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using UiDesktopApp1.Services;

namespace UiDesktopApp1.Controls
{
    public partial class RegisterControl : UserControl
    {
        private IAuthenticationService _authenticationService;

        private string _firstName = "";
        private string _lastName = "";
        private string _email = "";
        private string _password = string.Empty;
        private string _confirmPassword = string.Empty;

        public string FirstName { 
            get => _firstName; 
            set { _firstName = value; OnPropertyChanged(); } 
        }
        public string LastName
        {
            get => _lastName;
            set { _lastName = value; OnPropertyChanged(); }
        }
        public string Email
        {
            get => _email;
            set { _email = value; OnPropertyChanged(); }
        }
        public string Password
        {
            get => _password;
            set { _password = value; OnPropertyChanged(); }
        }

        public string ConfirmPassword
        {  
            get => _confirmPassword;
            set { _confirmPassword = value; OnPropertyChanged(); }
        }

        public RegisterControl() 
        {
            InitializeComponent();
            DataContext = this;
        }

        public RegisterControl(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
            InitializeComponent();
            DataContext = this;
        }

        public void SetAuthenticationService(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e) {
            var password = PasswordBox.Password;
            var confirmPassword = ConfirmPasswordBox.Password;
            if (string.IsNullOrWhiteSpace(FirstName) || string.IsNullOrWhiteSpace(LastName) || string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(confirmPassword))
            {
                System.Windows.MessageBox.Show("Please fill in all fields.", "Registration Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                return;
            }
            if(password != confirmPassword)
            {
                  System.Windows.MessageBox.Show("Las contraseñas no coinciden.", "Registration Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                return;
            }
            _authenticationService.Register(FirstName, LastName, Email,password);
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        // Notify property changes for data binding
        private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
