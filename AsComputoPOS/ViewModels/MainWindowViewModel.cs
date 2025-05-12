using System.Diagnostics;
using AsComputoPOS.Services;
using AsComputoPOS.ViewModels.PointOfSale;
using CommunityToolkit.Mvvm.Input;

namespace AsComputoPOS.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        public INavigationService Navigation { get; }
        public MainWindowViewModel(INavigationService navigationService)
        {
            Navigation = navigationService;
            // Skip auth if debugging
            if (IsDebugMode())
            {
                Debug.WriteLine("Debug mode is enabled");
                Navigation.NavigateTo<PointOfSaleViewModel>();
            }
            else
            {
                if (!CheckIfDbExists())
                {
                    Navigation.NavigateTo<RegisterViewModel>();
                }
                else
                {
                    Navigation.NavigateTo<LoginViewModel>();
                }
            }
        }

        public bool IsDebugMode()
        {
            return Debugger.IsAttached;
        }


        public bool CheckIfDbExists()
        {
            return false;
        }
        
        [RelayCommand]
        public void CheckCurrentView()
        {
            Debug.WriteLine(Navigation.CurrentViewModel.ToString());
        }
    }
}
