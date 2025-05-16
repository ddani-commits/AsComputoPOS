using System.Diagnostics;
using AsComputoPOS.Services;
using AsComputoPOS.ViewModels.PointOfSale;
using CommunityToolkit.Mvvm.Input;

namespace AsComputoPOS.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        public INavigationService Navigation { get; }
        public MainWindowViewModel(INavigationService navigationService, IAuthenticationService authenticationService)
        {
            Navigation = navigationService;
            authenticationService.HasUsers();

            if (authenticationService.HasUsers()) 
            {
                Debug.WriteLine("There are users");
                Navigation.NavigateTo<LoginViewModel>();
            } 
            else
            {
                Debug.WriteLine("No users");
                Navigation.NavigateTo<RegisterViewModel>();
            }

            // Skip auth if debugging
            //if (IsDebugMode())
            //{
            //    Debug.WriteLine("Debug mode is enabled");
            //    Navigation.NavigateTo<PointOfSaleViewModel>();
            //}
        }

        public bool IsDebugMode()
        {
            return Debugger.IsAttached;
        }
        
        [RelayCommand]
        public void CheckCurrentView()
        {
            Debug.WriteLine(Navigation.CurrentViewModel.ToString());
        }
    }                                                                                                                                                                                                                                                                                                                       
}
