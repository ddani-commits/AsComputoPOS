using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TamoPOS.Controls;
using TamoPOS.Data;
using TamoPOS.Models;

namespace TamoPOS.ViewModels.Pages
{
    public partial class ProductDetailViewModel : ViewModel
    {
        private ApplicationDbContext _applicationDbContext = new ApplicationDbContext();
        public ObservableCollection<Product> Products { get; } = new();
        public ObservableCollection<ProductPurchase> ProductPurchase { get; } = new();


        [ObservableProperty]
        private string? _idText;
        [ObservableProperty]
        private Product? _currentProduct;
        [ObservableProperty]
        private string? _salePrice;
        [ObservableProperty]
        private string? _stock;
        [ObservableProperty]
        private string? _name;
        [ObservableProperty]
        private bool? _isActive;
        [ObservableProperty]
        private string? _currentPurchaseOrder;
        public decimal? FirstSalePrice => CurrentProduct?.ProductPurchase?.FirstOrDefault()?.SalePrice;
        public ProductDetailViewModel() { }
        [RelayCommand]
        public void LoadProductDetails(int productId)
        {
            var product = _applicationDbContext.Products
                .Include(p => p.ProductPurchase)
                .Single(p => p.ProductId == productId);
                CurrentProduct = product;
            if (product != null)
            {
                CurrentProduct = product;
                IdText = product.ProductId.ToString();
                Name = product.Name;
                SalePrice = product.ProductPurchase?.FirstOrDefault()?.SalePrice.ToString("C") ?? "0.00";
               // Stock = product.ProductPurchase?.FirstOrDefault()?.QuantityRemaining?.ToString("C") ?? "0";
                IsActive = product.IsActive;
                Debug.WriteLine($"Product ID: {product.ProductId}");
            }
            else
            {
                Debug.WriteLine($"Producto no encontrado ");
            }
        }
        [RelayCommand]
        public void LoadProductPurchases()
        {
            if (int.TryParse(CurrentPurchaseOrder, out int purchaseOrderId))
            {
                ProductPurchase.Clear();
                _applicationDbContext.ProductPurchases
                    .Include(pp => pp.Product)
                    .Where(pp => pp.PurchaseOrderId == purchaseOrderId)
                    .ToList()
                    .ForEach(pp => ProductPurchase.Add(pp));
                foreach (ProductPurchase pr in ProductPurchase)
                {
                    Debug.WriteLine(pr.Id);
                }
            }
        }
    }
}
