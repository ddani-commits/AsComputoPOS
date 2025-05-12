using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AsComputoPOS.Services;
using AsComputoPOS.ViewModels.Suppliers;
using CommunityToolkit.Mvvm.Input;

namespace AsComputoPOS.ViewModels
{
    public partial class NavigationBarViewModel : ViewModelBase
    {
        private readonly INavigationService _navigation;
        public NavigationBarViewModel(INavigationService navigation) {

            _navigation = navigation;
        
        }

        [RelayCommand]
        public void NavigateToSuppliers()
        {
            _navigation.NavigateTo<SuppliersViewModel>();
        }
    }
}
