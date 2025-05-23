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
using AsComputoPOS.ViewModels.Inventory;
using CommunityToolkit.Mvvm.Input;
using AsComputoPOS.ViewModels.Category;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Diagnostics;

namespace AsComputoPOS.ViewModels
{
    public partial class NavigationBarViewModel : ViewModelBase
    {
        private readonly INavigationService _navigation;
        private readonly IAuthenticationService _authenticationService;
        [ObservableProperty] // This observable property will return the current user in
                             // AuthenticationService if available, or "Debug User" if not available 
        private string? _currentUser;
        public NavigationBarViewModel(INavigationService navigation, IAuthenticationService authenticationService) {
            _navigation = navigation;
            _authenticationService = authenticationService;
            _authenticationService.AuthenticationStateChanged += OnAuthenticationStateChanged;
            CurrentUser = _authenticationService.CurrentEmployee?.FirstName ?? "Debug User";
        }
        private void OnAuthenticationStateChanged(object? sender, EventArgs e)
        {
            CurrentUser = _authenticationService.CurrentEmployee?.FirstName ?? "Debug User";
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
        [RelayCommand]
        public void NavigateToInventory()
        {
            _navigation.NavigateTo<InventoryViewModel>();
        }
        [RelayCommand]
        public void NavigateToCategory()
        {
            _navigation.NavigateTo<CategoryViewModel>();
        }
    }
}
