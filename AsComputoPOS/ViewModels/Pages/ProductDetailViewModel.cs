using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TamoPOS.Data;
using TamoPOS.Models;

namespace TamoPOS.ViewModels.Pages
{
    public partial class ProductDetailViewModel : ViewModel
    {
        private ApplicationDbContext _applicationDbContext = new ApplicationDbContext();
        public ObservableCollection<Product> Products { get; } = new();
        public ObservableCollection<ProductPurchase> ProductPurchases { get; } = new();
        public ObservableCollection<StockBatch> StockBatches { get; } = new();

        [ObservableProperty]
        private string? _idText;
        [ObservableProperty]
        private Product? _currentProduct;
        [ObservableProperty]
        private string? _price;
        [ObservableProperty]
        private string? _stock;
        [ObservableProperty]
        private string? name;
        [ObservableProperty]
        private bool? _isActive;

        [RelayCommand]
        public void LoadProductDetails(int productId)
        {
            var product = _applicationDbContext.Products
                .Where(p => p.ProductId == productId)
                .Include(p => p.ProductPurchase)
                .FirstOrDefault();

            if (product != null)
            {
                CurrentProduct = product;
                IdText = product.ProductId.ToString();
                Name = product.Name;
                Price = product.ProductPurchase?.FirstOrDefault()?.SalePrice.ToString("C") ?? "0.00";
                Stock = product.ProductPurchase?.FirstOrDefault()?.QuantityRemaining?.ToString("C") ?? "0.00";
                IsActive = product.IsActive;
            }
        }

   
    }
}
