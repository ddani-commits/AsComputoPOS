using System.Collections.ObjectModel;
using TamoPOS.Models;

namespace TamoPOS.Services
{
    public interface IPOSService
    {
        bool IsSidePanelExpanded { get; set; }
        ObservableCollection<string> PaymentMethods { get; set; }

        ObservableCollection<ProductPurchase> ProductsInStock { get; set; }
        void LoadProductsInStock();

        void ConfirmSale(decimal ChangeDue, decimal CashPayment);
        void AddToCart(CartItem product);
        void ClearCart();
        ObservableCollection<CartItem> Cart { get; set; }
        string PrintTicket();
        List<ProductStockDTO> GetAllProducts();
        decimal Total { get; }
    }
}
