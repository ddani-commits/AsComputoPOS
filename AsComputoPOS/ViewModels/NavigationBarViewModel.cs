using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AsComputoPOS.Services;
using AsComputoPOS.ViewModels.PointOfSale;
using AsComputoPOS.ViewModels.Products;
using AsComputoPOS.ViewModels.SalesHistory;
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
        public void NavigateToPointOfSale()
        {
            _navigation.NavigateTo<PointOfSaleViewModel>();
        }

        [RelayCommand]
        public void NavigateToSalesHistory()
        {
            _navigation.NavigateTo<SalesHistoryViewModel>();
        }

        [RelayCommand]
        public void NavigateToProducts()
        {
            _navigation.NavigateTo<ProductsViewModel>();
        }

        [RelayCommand]
        public void NavigateToSuppliers()
        {
            _navigation.NavigateTo<SuppliersViewModel>();
        }

        [RelayCommand]
        public void NavigateToEmployees()
        {
            _navigation.NavigateTo<EmployeesViewModel>();
        }
    }
}
