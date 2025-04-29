using System.Diagnostics;
using AsComputoPOS.Services;
using CommunityToolkit.Mvvm.Input;

namespace AsComputoPOS.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        public INavigationService Navigation { get; }
        public string Greeting { get; } = "Welcome to Avalonia!";
        public MainWindowViewModel(INavigationService navigationService)
        {
            Navigation = navigationService;
        }
        [RelayCommand]
        public void NavigateToFirstPage()
        {
            Navigation.NavigateTo<FirstPageViewModel>();
        }
        [RelayCommand]
        public void CheckCurrentView()
        {
            Debug.WriteLine(Navigation.CurrentViewModel.ToString());
        }
    }
}
