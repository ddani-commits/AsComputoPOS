using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using TamoPOS.Controls;
using TamoPOS.Data;
using TamoPOS.Models;
using TamoPOS.Views.Pages;
using Wpf.Ui;
using static TamoPOS.Models.Product;
using CommunityToolkit.Mvvm.Input;

namespace TamoPOS.ViewModels.Pages
{
    public partial class ProductsViewModel : ViewModel
    {
        private readonly IContentDialogService _contentDialogService;
        private readonly INavigationService _navigationService;
        public ObservableCollection<Models.Product> ProductsList { get; } = new();
        private ApplicationDbContext _appDbContext = new();
        private readonly ProductDetailViewModel _productDetailViewModel;

        public ProductsViewModel(IContentDialogService contentDialogService, INavigationService navigationService, ProductDetailViewModel productDetailViewModel)
        {
            _contentDialogService = contentDialogService;
            _navigationService = navigationService;
            _productDetailViewModel = productDetailViewModel;
            LoadProducts();
        }

        private void LoadProducts()
        {
            var products = _appDbContext.Products.ToList(); 
            ProductsList.Clear();
            foreach (var product in products)
            { 
                product.ProductPurchase = _appDbContext.Set<ProductPurchase>().ToList();
                ProductsList.Add(product);
            }
        }

        [RelayCommand]
        private async Task OnShowDialog()
        {
            if (_contentDialogService.GetDialogHost() is not null)
            {   // Example of how to open a content dialog, a dialog must be created. examples are in Controls folder
                var newProductDialog = new NewProductContentDialog(_appDbContext, _contentDialogService.GetDialogHost(), AddProduct);
                _ = await newProductDialog.ShowAsync();
            }
        }

        [RelayCommand]
        public void AddProduct(Models.Product product)
        {
            _appDbContext.Products.Add(product);
            _appDbContext.SaveChanges();
            ProductsList.Add(product);
        }

        [RelayCommand]
        public void SaveProducts()
        {
            foreach (var product in ProductsList)
            {
                _appDbContext.Products.Update(product);
            }
            _appDbContext.SaveChanges();
        }

        public void LoadUnits()
        {
            var productsUnit = _appDbContext.Products
                .Include(p => p.ProductPurchase)
                .ToList();
            if (productsUnit != null)
            {

                ProductsList.Clear();
                foreach (var product in productsUnit)
                {
                    ProductsList.Add(product);
                }
            }
        }
        [RelayCommand]
        public void NavigateToProductDetails(int ProductId)
        {
            _productDetailViewModel.LoadProductDetails(ProductId);
            _productDetailViewModel.LoadProductPurchases();
            _navigationService.NavigateWithHierarchy(typeof(ProductDetailPage));

        }
    }
}
