using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Diagnostics;
using TamoPOS.Data;
using TamoPOS.Models;

namespace TamoPOS.ViewModels.Pages
{
    public partial class ProductDetailViewModel : ViewModel
    {
        private ApplicationDbContext _applicationDbContext = new ApplicationDbContext();
        public ObservableCollection<Product> Products { get; } = new();
        public ObservableCollection<ProductPurchase> ProductPurchases { get; } = new();
        public ObservableCollection<ProductPurchase> ProductsInStock { get; set; } = new();

        [ObservableProperty]
        private string? _idText;
        [ObservableProperty]
        private Product? _currentProduct;
        [ObservableProperty]
        private string? _salePrice;
        [ObservableProperty]
        private string? _quantityRemaining;
        [ObservableProperty]
        private string? _name;
        [ObservableProperty]
        private bool? _isActive;
        [ObservableProperty]
        private string? _currentPurchaseOrder;
        public ProductDetailViewModel() { }
        [RelayCommand]
        public void LoadProductDetails(int productId)
        {
            var product = _applicationDbContext.Products
                .Include(p => p.Category)
                .Include(p => p.ProductPurchase)
                .Single(p => p.ProductId == productId);
                CurrentProduct = product;
            if (product != null)
            {
                var totalRemaining = product.ProductPurchase
                    .Where(pp => pp.QuantityRemaining >= 0)
                    .Sum(pp => pp.QuantityRemaining ?? 0);

                CurrentProduct = product;
                IdText = product.ProductId.ToString();
                Name = product.Name;
                SalePrice = product.ProductPurchase?.FirstOrDefault()?.SalePrice.ToString("C") ?? "0.00";
                QuantityRemaining = totalRemaining.ToString();
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
                ProductPurchases.Clear();
                _applicationDbContext.ProductPurchases
                    .Include(pp => pp.Product)
                    .Where(pp => pp.PurchaseOrderId == purchaseOrderId)
                    .ToList()
                    .ForEach(pp => ProductPurchases.Add(pp));
                foreach (ProductPurchase pr in ProductPurchases)
                {
                    Debug.WriteLine(pr.Id);
                }
            }
        }   
    }
}
