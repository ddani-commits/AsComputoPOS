using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;
using TamoPOS.Data;
using TamoPOS.Models;
using TamoPOS.ViewModels.Pages;

namespace TamoPOS.Services
{
    public class POSService : IPOSService
    {
        private IServiceProvider _serviceProvider;
        private IAuthenticationService _authenticationService;
        private SalesHistoryViewModel _salesHistoryViewModel;
        private Lazy<ProductsViewModel> _productsViewModel;
        private readonly ApplicationDbContext _appDbContext = new();

        public ObservableCollection<ProductPurchase> ProductsInStock { get; set; } = new();
        public ObservableCollection<CartItem> Cart { get; set; } = new();
        public ObservableCollection<string> PaymentMethods { get; set; } = new() { "Efectivo", "Debito/Credito" };
        public bool IsSidePanelExpanded { get; set; } = false;
        public decimal Total => Cart.Sum(item => item.Total);

        public POSService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _authenticationService = _serviceProvider.GetRequiredService<IAuthenticationService>();
            _salesHistoryViewModel = _serviceProvider.GetRequiredService<SalesHistoryViewModel>();
            _productsViewModel = new Lazy<ProductsViewModel>(() => _serviceProvider.GetRequiredService<ProductsViewModel>()) ;
        }

        public List<ProductStockDTO> GetAllProducts()
        {
            var products = _appDbContext.Products
                .Include(p => p.ProductPurchase)
                .Include(p => p.Category)
                .ToList();

            return products.Select(p =>
            {
                // This is not correct, it may show prices no longer available. TODO
                var salePrice = p.ProductPurchase.OrderBy(pp => pp.ProductId).FirstOrDefault()?.SalePrice;

                decimal quantityRemaining = p.ProductPurchase
                .Where(pp => pp.GetType().GetProperty("QuantityRemaining") != null)
                .Sum(pp => pp.QuantityRemaining ?? 0);

                return new ProductStockDTO
                {
                    ProductId = p.ProductId,
                    Name = p.Name,
                    SalePrice = salePrice ?? 0,
                    Category = p.Category,
                    CategoryId = p.CategoryId,
                    QuantityRemaining = quantityRemaining,
                };
            }).ToList();
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

            foreach(CartItem cartItem in Cart)
            {
                var oldestProductPurchase = _appDbContext.ProductPurchases
                    .Where(p => p.ProductId == cartItem.ProductId && p.QuantityRemaining > 0)
                    .OrderBy(p => p.Id)
                    .FirstOrDefault();

                if (cartItem is not null && oldestProductPurchase is not null)
                    oldestProductPurchase.QuantityRemaining = oldestProductPurchase.QuantityRemaining - cartItem.Quantity;
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
            _productsViewModel.Value.LoadAllProducts();
        }
        public string PrintTicket() { return "Ticket generated successfully!"; }
        public void AddToCart(CartItem product) { Cart.Add(product); }
        public void ClearCart() { Cart.Clear(); }
    }
}
