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
using TamoPOS.Controls.PointOfSalePanel;
using System.Windows.Controls;
using TamoPOS.Services;

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

            var checkoutPanel = _serviceProvider.GetRequiredService<CheckoutPanel>();
            var checkoutPanelViewModel = _serviceProvider.GetRequiredService<CheckoutPanelViewModel>();

            // manually set content dialog service, otherwise it is null
            // if required from IServiceProvider null, is passed as dependency is null
            checkoutPanelViewModel.SetContentDialogService(contentDialogService);

            checkoutPanel.SetViewModel(checkoutPanelViewModel);

            // Set in the View because it needs an empty constructor
            MainContainer.Children.Add(checkoutPanel);
            Grid.SetColumn(checkoutPanel, 1);
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
            //SidePanelColumn.Width = new GridLength(1, GridUnitType.Star);

            if (args.Page is POSPage)
            {
                SidePanelColumn.Width = new GridLength(1, GridUnitType.Star);
            }
            else
            {
                SidePanelColumn.Width = new GridLength(0);
            }
        }
    }
}
