using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;
using System.Windows.Input;
using TamoPOS.ViewModels.Controls;
using TamoPOS.ViewModels.Windows;
using TamoPOS.Views.Pages;
using Wpf.Ui;
using Wpf.Ui.Abstractions;
using Wpf.Ui.Appearance;
using Wpf.Ui.Controls;

namespace TamoPOS.Views.Windows
{
    public partial class MainWindow : INavigationWindow
    {
        public MainWindowViewModel ViewModel { get; }
        private IServiceProvider _serviceProvider;

        public MainWindow(
            MainWindowViewModel viewModel,
            INavigationViewPageProvider navigationViewPageProvider,
            INavigationService navigationService,
            IContentDialogService contentDialogService,
            IServiceProvider serviceProvider
        )
        {
            ViewModel = viewModel;
            _serviceProvider = serviceProvider;
            DataContext = this;

            SystemThemeWatcher.Watch(this);

            InitializeComponent();
            SetPageService(navigationViewPageProvider);

            navigationService.SetNavigationControl(RootNavigation);
            contentDialogService.SetDialogHost(RootContentDialog); //This references the element with the x:Name "RootContentDialog" in MainWindow.xaml

            checkoutPanel.SetViewModel(_serviceProvider.GetRequiredService<CheckoutPanelViewModel>());

            this.PreviewKeyDown += MainWindow_PreviewKeyDown;
        }

        private DateTime? _lastKeystrokeTime = null;
        private char? _lastKeystrokeValue = null;

        private void MainWindow_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            var focusedElement = Keyboard.FocusedElement;
            var now = DateTime.Now;

            // Ignore keystrokes in text inputs
            if (focusedElement is TextBox || focusedElement is PasswordBox) return;

            // Letras (A-Z)
            //if (e.Key >= Key.A && e.Key <= Key.Z) Debug.Write($"Letra: {e.Key}");

            // Números (0-9, parte superior del teclado)
            else if (e.Key >= Key.D0 && e.Key <= Key.D9)
            {
                if(_lastKeystrokeTime != null)
                {
                    var timeSinceLastKeyStroke = now - _lastKeystrokeTime.Value;
                    Debug.WriteLine("Time since last keystroke: " + timeSinceLastKeyStroke.TotalMilliseconds + "ms");
                    Debug.WriteLine($"{e.Key - Key.D0}");
                    //Debug.WriteLine(e.Key);
                    //Debug.WriteLine(Key.D0);
                }
            }
            // Números (teclado numérico)
            else if (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9) { }

            _lastKeystrokeTime = now;
        }

        #region INavigationWindow methods

        public INavigationView GetNavigation() => RootNavigation;

        public bool Navigate(Type pageType) => RootNavigation.Navigate(pageType);

        public void SetPageService(INavigationViewPageProvider navigationViewPageProvider) => RootNavigation.SetPageProviderService(navigationViewPageProvider);

        public void ShowWindow() => Show();

        public void CloseWindow() => Close();

        #endregion INavigationWindow methods

        /// <summary>
        /// Raises the closed event, actually closing the application.
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

        private void RootNavigation_Navigated(NavigationView sender, NavigatedEventArgs args)
        {
            if(args.Page is POSPage)
            {
                SidePanelColumn.Width = new GridLength(1, GridUnitType.Star);
            } else
            {
                SidePanelColumn.Width = new GridLength(0);
            }
        }
    }
}
