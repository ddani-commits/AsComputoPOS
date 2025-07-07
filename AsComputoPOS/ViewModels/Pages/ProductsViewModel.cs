using CommunityToolkit.Mvvm.Input;
using DocumentFormat.OpenXml.VariantTypes;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Data;
using TamoPOS.Controls;
using TamoPOS.Data;
using TamoPOS.Models;
using TamoPOS.Views.Pages;
using Wpf.Ui;
using static TamoPOS.Models.Product;

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
        private Product? _currentProduct;
        [ObservableProperty]
        private string? _salePrice;
        [ObservableProperty]
        private string? _quantityRemaining;
        public ProductsViewModel(IContentDialogService contentDialogService, INavigationService navigationService, ProductDetailViewModel productDetailViewModel)
        {
            _contentDialogService = contentDialogService;
            _navigationService = navigationService;
            _productDetailViewModel = productDetailViewModel;
            LoadProductsInStock();
        }
        [RelayCommand]
        public void LoadProductsInStock()
        {
            ProductsInStock.Clear();
            var productPurchases = _appDbContext.ProductPurchases
                .Include(pp => pp.Product)
                .Where(pp => pp.QuantityRemaining >= 0)
                .AsEnumerable()
                .GroupBy(pp => pp.ProductId)
                .Select(g =>
                {
                    var oldest = g.OrderBy(pp => pp.PurchaseOrderId).First();
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
                ProductsInStock.Add(productInStock);
            }
            foreach (var productPurchase in productPurchases)
            {
                // Verificar si el producto ya está en ProductsInStock
                var existingProduct = ProductsInStock.FirstOrDefault(pp => pp.ProductId == productPurchase.ProductId);
                if (existingProduct != null)
                {
                    // Actualizar el producto existente con los nuevos valores
                    existingProduct.QuantityRemaining = productPurchase.QuantityRemaining;
                    existingProduct.SalePrice = productPurchase.SalePrice;
                }
                else
                {
                    // Si el producto no existe, agregarlo a la colección
                    ProductsInStock.Add(productPurchase);
                }
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
        public void AddProduct(Product product)
        {
            _appDbContext.Products.Add(product);
            _appDbContext.SaveChanges();
            LoadProductsInStock();
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
            _productDetailViewModel.LoadProductDetails(ProductId);
            _productDetailViewModel.LoadProductPurchases();
            _navigationService.NavigateWithHierarchy(typeof(ProductDetailPage));

        }

        [RelayCommand]
        public void DeleteProduct(object parameter)
        {
            Debug.WriteLine("Producto eliminado correctamente");
            if (parameter is not Product product) return;
            var productToDelete = _appDbContext.Products.Find(product.ProductId);
            if (productToDelete != null)
            {
                var productPurchasesToDelete = _appDbContext.ProductPurchases
                    .Where(pp => pp.ProductId == productToDelete.ProductId).ToList();
                foreach (var purchase in productPurchasesToDelete)
                {
                    _appDbContext.ProductPurchases.Remove(purchase);
                }
                _appDbContext.Products.Remove(productToDelete);
                _appDbContext.SaveChanges();
                var productToRemove = ProductsInStock.FirstOrDefault(p => p.ProductId == product.ProductId); //Si encontramos un producto con el mismo ProductId en ProductsList, lo eliminamos
                if (productToRemove != null)
                {
                    ProductsInStock.Remove(productToRemove);  // Eliminar el producto correctamente de la lista
                }
                LoadProductsInStock();
            }
            else
            {
                Debug.WriteLine("Producto no encontrado");
            }
        }
    }
}
