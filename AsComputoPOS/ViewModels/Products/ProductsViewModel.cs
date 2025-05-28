using AsComputoPOS.Data;
using AsComputoPOS.Models;
using AsComputoPOS.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsComputoPOS.ViewModels.Products
{
    public partial class ProductsViewModel: NavigationBarViewModel
    {
        [ObservableProperty]
        private string _productName = "";
        [ObservableProperty]
        private bool _isActive = true;
        [ObservableProperty]
        private string _barcode = "";
        [ObservableProperty]
        private string _SKU = "";
        public ObservableCollection<Product> ProductsList { get; } = new();
        public ProductsViewModel(INavigationService navigation, IAuthenticationService authenticationService) : base(navigation, authenticationService) 
        {
            LoadProducts();
        }
        
        private void LoadProducts()
        {
            using var db = new ApplicationDbContext();
            foreach (var product in db.Products)
            {
                ProductsList.Add(product);
            }
        }

        [RelayCommand]
        public void AddProduct()
        {
            using (var context = new ApplicationDbContext())
            {
                var CurrentProduct = new Product(ProductName, IsActive, Barcode, SKU);
                context.Products.Add(CurrentProduct);
                context.SaveChanges();
                ProductsList.Add(CurrentProduct);
            }
            Console.WriteLine("Add Product command executed.");
            ClearFields();
        }

        private void ClearFields()
        {
            ProductName = string.Empty;
            IsActive = true;
            Barcode = string.Empty;
            SKU = string.Empty;
        }
    }
}
