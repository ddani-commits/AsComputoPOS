using DocumentFormat.OpenXml.Drawing.Diagrams;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;
using System.Windows.Controls.Ribbon.Primitives;
using TamoPOS.Services;
using TamoPOS.ViewModels.Windows;
using Wpf.Ui;
using Wpf.Ui.Abstractions;
using Wpf.Ui.Appearance;
using Wpf.Ui.Controls;

namespace TamoPOS.Views.Windows
{
    public partial class MainWindow : INavigationWindow
    {
        public MainWindowViewModel ViewModel { get; }

        public MainWindow(
            MainWindowViewModel viewModel,
            INavigationViewPageProvider navigationViewPageProvider,
            INavigationService navigationService,
            IContentDialogService contentDialogService
        )
        {
            ViewModel = viewModel;
            DataContext = this;

            SystemThemeWatcher.Watch(this);

            InitializeComponent();
            SetPageService(navigationViewPageProvider);

            navigationService.SetNavigationControl(RootNavigation);
            contentDialogService.SetDialogHost(RootContentDialog); //This references the element with the x:Name "RootContentDialog" in MainWindow.xaml

            //Loaded += MainWindow_Loaded;
        }

        #region INavigationWindow methods

        public INavigationView GetNavigation() => RootNavigation;

        public bool Navigate(Type pageType) => RootNavigation.Navigate(pageType);

        public void SetPageService(INavigationViewPageProvider navigationViewPageProvider) => RootNavigation.SetPageProviderService(navigationViewPageProvider);

        public void ShowWindow() => Show();

        public void CloseWindow() => Close();

        #endregion INavigationWindow methods

        /// <summary>
        /// Raises the closed event.
        /// </summary>
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.Shutdown();
        }

        INavigationView INavigationWindow.GetNavigation()
        {
            throw new NotImplementedException();
        }

        public void SetServiceProvider(IServiceProvider serviceProvider)
        {
            throw new NotImplementedException();
        }

        private void NavigationViewItem_Click(object sender, RoutedEventArgs e)
        {
            CloseWindow();
        }

        private void MinimizeWindowButton(object sender, RoutedEventArgs e)
        {
            WindowState= WindowState.Minimized;
        }

        private void Maximize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Maximized;
        }
        private void SignOutButton_Click(object sender, RoutedEventArgs e)
        {
            var authService = App.Services.GetRequiredService<IAuthenticationService>();
            authService.Logout();
            var authWindow = App.Services.GetRequiredService<AuthWindow>();
            authWindow.Owner = this;
            var result = authWindow.ShowDialog();
            if (result != true)
            {
                this.Close();
                Debug.WriteLine("Holi");
                
            }
        }
    }
}
