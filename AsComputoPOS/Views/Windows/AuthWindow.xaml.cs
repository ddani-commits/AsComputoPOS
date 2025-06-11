using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;
using UiDesktopApp1.Services;
using UiDesktopApp1.ViewModels.Windows;

namespace UiDesktopApp1.Views.Windows
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
            Application.Current.Exit += OnAppExit;
            var users = _authenticationService.HasUsers();

            Debug.WriteLine("app has users: ", users);

            if (users == true)
            {
                LoginControl.SetAuthenticationService(_authenticationService);
                LoginControl.Visibility = Visibility.Visible;
                RegisterControl.Visibility = Visibility.Collapsed;
            }
            else
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
                System.Windows.MessageBox.Show("Authentication failed. Please try again.", "Authentication Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public static void ShowAuthWindow()
        {
            var mainWindow = App.Services.GetRequiredService<MainWindow>();
            Application.Current.MainWindow = mainWindow;
            mainWindow.Show();

            var authWindow = App.Services.GetRequiredService<AuthWindow>();
            authWindow.Owner = mainWindow;
            var result = authWindow.ShowDialog();

        }

        private void OnAppExit(object? sender, ExitEventArgs e)
        {
            if (this.IsVisible)
            {
                this.Close();
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.Exit -= OnAppExit;
            //_authenticationService.Logout(); // Ensure to logout when the window is closed

        }
    }
}