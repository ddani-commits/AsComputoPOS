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
        public ObservableCollection<CartItem> Cart { get; set; } = new();
        public ObservableCollection<string> PaymentMethods { get; set; } = new () { "Efectivo", "Debito/Credito" };

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
        public void AddToCart(CartItem product)
        {
            Cart.Add(product);
        }

        // Todo: Create a class specific for POS Product Display
        public void LoadProductsInStock()
        {
            ProductsInStock.Clear();
            var productPurchases = _appDbContext.ProductPurchases
                .Include(pp => pp.Product)
                .Where(pp => pp.QuantityRemaining > 0)
                .AsEnumerable()
                .GroupBy(productPurchase => productPurchase.ProductId)
                .Select(g =>
                {
                    var oldest = g.OrderBy(pp => pp.PurchaseOrderId).First(); // find the oldest by using the smallest id
                    var totalRemaining = g.Sum(pp => pp.QuantityRemaining ?? 0); // sum every one's Quantity Remaining

                    // Return new instance with the needed data
                    return new ProductPurchase
                    {
                        Id = oldest.Id,
                        ProductId = oldest.ProductId,
                        Product = oldest.Product,
                        SalePrice = oldest.SalePrice,
                        QuantityRemaining = totalRemaining
                    };
                })
                .ToList();

            foreach (var productPurchase in productPurchases)
            {
                ProductsInStock.Add(productPurchase);
            }
        }

        public void ConfirmSale() { }
        public string PrintTicket() { return "Ticket generated successfully!"; }
    }
}
