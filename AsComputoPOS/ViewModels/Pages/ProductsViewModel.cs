using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using TamoPOS.Controls;
using TamoPOS.Data;
using TamoPOS.Models;
using TamoPOS.Views.Pages;
using Wpf.Ui;

namespace TamoPOS.ViewModels.Pages
{
    public partial class ProductsViewModel : ViewModel
    {
        [ObservableProperty]
        private Product selectedProduct;
        private readonly IContentDialogService _contentDialogService;
        private readonly INavigationService _navigationService;
        public ObservableCollection<Product> ProductsList { get; } = new();
        private ApplicationDbContext _appDbContext = new();
        private readonly ProductDetailViewModel _productDetailViewModel; 
        public ObservableCollection<ProductPurchase> ProductsInStock { get; set; } = new();

        [ObservableProperty]
        private string? _name;
        [ObservableProperty]
        private Product? _currentProduct;
        [ObservableProperty]
        private string? _salePrice;
        [ObservableProperty]
        private string? _quantityRemaining;
        [ObservableProperty]
        private Category? _category;
        public ProductsViewModel(IContentDialogService contentDialogService, INavigationService navigationService, ProductDetailViewModel productDetailViewModel)
        {
            _productDetailViewModel = productDetailViewModel;
            _navigationService = navigationService;
            _contentDialogService = contentDialogService;
            LoadAllProducts();
        }

        public void LoadAllProducts()
        {
            var productPurchases = _appDbContext.ProductPurchases
                .Include(pp => pp.Product)
                .Include(p => p.Product.Category)
                .Where(pp => pp.QuantityRemaining >= 0)
                .AsEnumerable()
                .GroupBy(pp => pp.ProductId)
                .Select(g =>
                {
                    var oldest = g.OrderBy(pp => pp.PurchaseOrderId).LastOrDefault();
                    var totalRemaining = g.Sum(pp => pp.QuantityRemaining ?? 0);
                    var salePrice = oldest?.SalePrice ?? 0;
                    return new ProductPurchase
                    {
                        ProductId = oldest.ProductId,
                        Product = oldest.Product,
                        SalePrice = salePrice,
                        QuantityRemaining = totalRemaining
                    };
                }).ToList();
            var allProductsIds = productPurchases.Select(pp => pp.ProductId).ToList();
            var allProducts = _appDbContext.Products
                .Include(p => p.Category)
                .Where(p => !allProductsIds.Contains(p.ProductId)).ToList();
            ProductsInStock.Clear();
            foreach (var product in allProducts)
            {
                var productInStock = new ProductPurchase
                {
                    ProductId = product.ProductId,
                    Product = product,
                    SalePrice = 0,
                    QuantityRemaining = 0
                };
                if (product.Category != null)
                {
                    productInStock.Product.Name = product.Name;
                    productInStock.Product.Category = product.Category;
                }
                ProductsInStock.Add(productInStock);
            }
            foreach (var productPurchase in productPurchases)
            {
                var existingProduct = ProductsInStock.FirstOrDefault(pp => pp.ProductId == productPurchase.ProductId);
                if (existingProduct != null)
                {
                    existingProduct.QuantityRemaining = productPurchase.QuantityRemaining;
                    existingProduct.SalePrice = productPurchase.SalePrice;
                }
                else
                {
                    ProductsInStock.Add(productPurchase);
                }
            }
        }

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
