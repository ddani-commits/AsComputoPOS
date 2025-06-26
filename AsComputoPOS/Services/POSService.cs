using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;
using System.Diagnostics;
using TamoPOS.Data;
using TamoPOS.Models;
using TamoPOS.Views.Windows;
using Wpf.Ui;

namespace TamoPOS.Services
{
    public class POSService : IPOSService
    {
        private INavigationWindow? _navigationWindow;
        private MainWindow? _mainWindow;
        public ObservableCollection<ProductPurchase> ProductsInStock { get; set; } = new();
        private readonly ApplicationDbContext _appDbContext = new();
        private ObservableCollection<Product> Cart { get; set; } = new();

        public bool _isSidePanelExpanded = false;
        public bool IsSidePanelExpanded
        {
            get => _isSidePanelExpanded;
            set
            {
                _isSidePanelExpanded = value;
            }
        }
        public POSService(){}
        public void AddToCart(Product product)
        {
            Cart.Add(product);
        }

        public void LoadProductsInStock()
        {
            ProductsInStock.Clear();
            var productPurchases = _appDbContext.ProductPurchases
                .Include(pp => pp.Product)
                .Where(productPurchase => productPurchase.QuantityRemaining > 0)
                .GroupBy(productPurchase => productPurchase.ProductId)
                .Select(g => g.OrderBy(pp => (double)pp.QuantityRemaining!).First())
                .ToList();

            foreach (var productPurchase in productPurchases)
            {
                ProductsInStock.Add(productPurchase);
            }
        }
    }
}
