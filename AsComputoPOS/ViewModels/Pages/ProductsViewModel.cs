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
        private readonly CategoryViewModel _categoryViewModel;
        private readonly IPOSService _posService;

        public ObservableCollection<ProductStockDTO> AllProducts { get; set; } = new();
        private ApplicationDbContext _appDbContext = new();

        public ProductsViewModel(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _productDetailViewModel = _serviceProvider.GetRequiredService<ProductDetailViewModel>();
            _navigationService = _serviceProvider.GetRequiredService<INavigationService>();
            _contentDialogService = _serviceProvider.GetRequiredService<IContentDialogService>();
            _categoryViewModel = serviceProvider.GetRequiredService<CategoryViewModel>();
            _posService = serviceProvider.GetRequiredService<IPOSService>();
            LoadAllProducts();
        }

        public void LoadAllProducts()
        {
            AllProducts.Clear();
            var products = _posService.GetAllProducts();
            foreach (var product in products)
            {
                AllProducts.Add(product);
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
                var newProductDialog = new NewProductContentDialog(
                    _categoryViewModel.CategoriesList, 
                    _contentDialogService.GetDialogHost(), 
                    AddProduct
                );
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
        public void NavigateToProductDetails(int ProductId)
        {
            _productDetailViewModel.LoadProductDetails(ProductId, _appDbContext);
            _productDetailViewModel.LoadProductPurchases();
            _navigationService.NavigateWithHierarchy(typeof(ProductDetailPage));
        }
    }
}
