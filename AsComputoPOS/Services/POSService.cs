using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Diagnostics;
using TamoPOS.Data;
using TamoPOS.Models;

namespace TamoPOS.Services
{
    public class POSService : IPOSService
    {
        public ObservableCollection<ProductPurchase> ProductsInStock { get; set; } = new();
        private readonly ApplicationDbContext _appDbContext = new();
        public ObservableCollection<CartItem> Cart { get; set; } = new();
        public ObservableCollection<string> PaymentMethods { get; set; } = new () { "Efectivo", "Debito/Credito" };
        public bool IsSidePanelExpanded { get; set; } = false;

        public decimal Total => Cart.Sum(item => item.Total);

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

        public void ConfirmSale() 
        {
            Ticket ticket = new Ticket()
            {
                Date = DateTime.Now,
                Products = Cart,
                Total = Total
            };

            _appDbContext.Add(ticket);
            _appDbContext.SaveChanges();
        }
        public string PrintTicket() { return "Ticket generated successfully!"; }
    }
}
