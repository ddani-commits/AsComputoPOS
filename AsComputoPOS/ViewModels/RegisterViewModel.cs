using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AsComputoPOS.Services;
using CommunityToolkit.Mvvm.Input;

namespace AsComputoPOS.ViewModels
{
    public partial class RegisterViewModel: ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IAuthenticationService _authenticationService; 

        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string Email { get; set; } = "";
        public string ErrorUsername { get; set; } = "";
        public string Password { get; set; } = "";
        public string ErrorPassword { get; set; } = "";
        public string ConfirmPassword { get; set; } = "";
        public string ErrorConfirmPassword { get; set; } = "";
        public RegisterViewModel(INavigationService navigationService, IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
            _navigationService = navigationService;
        }
        [RelayCommand]
        public void Register()
        {
            Debug.WriteLine("Username: " + FirstName);
            Debug.WriteLine("Password: " + Password);
            Debug.WriteLine("ConfirmPassword: " + ConfirmPassword);
            if (string.IsNullOrEmpty(FirstName) || string.IsNullOrEmpty(Password) || string.IsNullOrEmpty(ConfirmPassword))
            {   
                Debug.WriteLine("Please fill in all fields.");
                return;
            }
            if (Password != ConfirmPassword)
            {
                Debug.WriteLine(Password, "!=", ConfirmPassword);
                return;
            }
            _authenticationService.Register(FirstName, LastName, Email);
            _navigationService.NavigateTo<AddEmployeesViewModel>();
        }
    }
}
