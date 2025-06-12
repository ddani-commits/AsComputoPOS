using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using TamoPOS.Services;

namespace TamoPOS.Controls
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
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }

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
            Debug.WriteLine(FirstName);
            Debug.WriteLine(LastName);
            Debug.WriteLine(Email);
            _authenticationService.Register(FirstName, LastName, Email);
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        // Notify property changes for data binding
        private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
