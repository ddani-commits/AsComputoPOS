using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AsComputoPOS.Services;
using AsComputoPOS.ViewModels;
using AsComputoPOS.ViewModels.PointOfSale;
using CommunityToolkit.Mvvm.Input;

namespace AsComputoPOS.ViewModels
{
    public partial class LoginViewModel: ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IAuthenticationService _authenticationService;
        public string Email { get; set; } = "";
        public string Password { get; set; } = "";
        public LoginViewModel(INavigationService navigationService, IAuthenticationService authenticationService) {
            _navigationService = navigationService;
            _authenticationService = authenticationService;
        }

        [RelayCommand]
        public void Login()
        {
            if (string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Password))
            {
                // Show error message
                return;
            }
            bool loginSuccess = _authenticationService.Login(Email, Password);
            if (loginSuccess) 
            { 
                _navigationService.NavigateTo<PointOfSaleViewModel>();
            } else
            {
                Debug.WriteLine("Login failed");
            }
        }
    }
}
