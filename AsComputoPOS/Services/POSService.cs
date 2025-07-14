using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;
using System.Diagnostics;
using TamoPOS.Data;
using TamoPOS.Models;
using TamoPOS.ViewModels.Pages;

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
        private IServiceProvider _serviceProvider;
        private SalesHistoryViewModel _salesHistoryViewModel;
        private IAuthenticationService _authenticationService;

        public POSService(IServiceProvider serviceProvider) 
        {
            _serviceProvider = serviceProvider;
            _authenticationService = _serviceProvider.GetRequiredService<IAuthenticationService>();
            _salesHistoryViewModel = _serviceProvider.GetRequiredService<SalesHistoryViewModel>();
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

        public void ConfirmSale(decimal ChangeDue, decimal CashPayment)
        {
            Ticket ticket = new Ticket()
            {
                Date = DateTime.Now,
                Products = Cart,
                Total = Total,
                ChangeDue = ChangeDue,
                Paid = CashPayment,
                EmployeeId = _authenticationService.CurrentEmployee.EmployeeId
            };

            foreach(ProductPurchase pp in _appDbContext.ProductPurchases.ToList())
            {
                var cartItem = Cart.FirstOrDefault(p => p.Product.ProductId == pp.ProductId);

                if(cartItem is not null)
                    pp.QuantityRemaining = pp.QuantityRemaining - cartItem.Quantity;
            }

            _appDbContext.Add(ticket);
            _appDbContext.SaveChanges();

            // Without this, when creating multiple tickets in the same session, the CartItems
            // from the last session would be deleted, presumably entity framework would
            // just update the variables instead of creating new instances
            _appDbContext.Entry(ticket).State = EntityState.Detached;

            Cart.Clear();
            LoadProductsInStock(); // Not ideal but working
            _salesHistoryViewModel.LoadSalesHistoryAsync();
        }
        public string PrintTicket() { return "Ticket generated successfully!"; }
        public void AddToCart(CartItem product) {Cart.Add(product);}
        public void ClearCart() {Cart.Clear();}
    }
}
