using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text.Json;
using TamoPOS.Controls;
using TamoPOS.Data;
using TamoPOS.Models;
using TamoPOS.Services;
using TamoPOS.Views.Pages;
using Wpf.Ui;

namespace TamoPOS.ViewModels.Pages
{
    public partial class ProductsViewModel : ViewModel
    {
        private readonly IContentDialogService _contentDialogService;
        private readonly INavigationService _navigationService;
        private readonly IServiceProvider _serviceProvider;
        private readonly ProductDetailViewModel _productDetailViewModel; 

        public ObservableCollection<Product> ProductsList { get; } = new();
        private ApplicationDbContext _appDbContext = new();
        public ObservableCollection<ProductStockDTO> ProductsInStock { get; set; } = new();

        public ProductsViewModel(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _productDetailViewModel = _serviceProvider.GetRequiredService<ProductDetailViewModel>();
            _navigationService = _serviceProvider.GetRequiredService<INavigationService>();
            _contentDialogService = _serviceProvider.GetRequiredService<IContentDialogService>();
            LoadAllProducts();
        }

        public void LoadAllProducts()
        {
            ProductsInStock.Clear();
            var products = _appDbContext.Products
                .Include(p => p.ProductPurchase)
                .Include(p => p.Category)
                .ToList();

            var productsInStock = products.Select( p =>
            {
                // Ok this is not actually correct because is not considering if there is still products in 
                // stock in that price but for now is good enough
                var salePrice = p.ProductPurchase.OrderBy(pp => pp.ProductId).FirstOrDefault()?.SalePrice;

                decimal quantityRemaining = p.ProductPurchase
                .Where(pp => pp.GetType().GetProperty("QuantityRemaining") != null)
                .Sum(pp => pp.QuantityRemaining ?? 0);

                return new ProductStockDTO
                {
                    ProductId = p.ProductId,
                    Name = p.Name,
                    SalePrice = salePrice ?? 0,
                    Category = p.Category,
                    CategoryId = p.CategoryId,
                    QuantityRemaining = quantityRemaining,
                };
            } ).ToList();

            foreach( ProductStockDTO p in productsInStock)
            {
                ProductsInStock.Add(p);
            }
        }

        public JsonSerializerOptions options = new JsonSerializerOptions
        {
            WriteIndented = true, // makes output readable
            ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles // avoids circular references
        };

        [RelayCommand]
        private async Task OnShowDialog()
        {
            if (_contentDialogService.GetDialogHost() is not null)
            {
                var newProductDialog = new NewProductContentDialog(_appDbContext, _contentDialogService.GetDialogHost(), AddProduct);
                _ = await newProductDialog.ShowAsync();
            }
        }

        [RelayCommand]
        public void AddProduct(Product product)
        {
            _appDbContext.Products.Add(product);
            _appDbContext.SaveChanges();
            LoadAllProducts(); 
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

        [RelayCommand]
        public void NavigateToProductDetails(int ProductId)
        {
            _productDetailViewModel.LoadProductDetails(ProductId, _appDbContext);
            _productDetailViewModel.LoadProductPurchases();
            _navigationService.NavigateWithHierarchy(typeof(ProductDetailPage));
        }
    }
}
