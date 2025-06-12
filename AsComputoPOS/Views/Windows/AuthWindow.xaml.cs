using System.Diagnostics;
using TamoPOS.Services;
using TamoPOS.ViewModels.Windows;

namespace TamoPOS.Views.Windows
{
    public partial class AuthWindow : Window
    {
        private readonly IAuthenticationService _authenticationService;
        public AuthWindowViewModel ViewModel;
        public AuthWindow(AuthWindowViewModel viewModel, IAuthenticationService authenticationService)
        {
            ViewModel = viewModel;
            DataContext = ViewModel;
            InitializeComponent();
            _authenticationService = authenticationService;

            _authenticationService.AuthenticationStateChanged += OnAuthenticationStateChanged;
            var users = _authenticationService.HasUsers();

            Debug.WriteLine("app has users: ", users);

            if (users == true)
            {
                LoginControl.SetAuthenticationService(_authenticationService);
                LoginControl.Visibility = Visibility.Visible;
                RegisterControl.Visibility = Visibility.Collapsed;
            } else
            {
                RegisterControl.SetAuthenticationService(_authenticationService);
                LoginControl.Visibility = Visibility.Collapsed;
                RegisterControl.Visibility = Visibility.Visible;    
            }

            Debug.WriteLine(LoginControl.Visibility);
            Debug.WriteLine(RegisterControl.Visibility);
        
        }

        // Either login or register must return true to the DialogResult variable to be able
        // to continue to MainWindow
        public void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void OnAuthenticationStateChanged(object? sender, EventArgs e)
        {
            if (_authenticationService.IsAuthenticated == true)
            {
                this.DialogResult = true;
                this.Close();
            }
            else
            {
                this.DialogResult = false;
            }
        }
    }
}