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

        void ConfirmSale();
        void AddToCart(ProductPurchase product);
        ObservableCollection<ProductPurchase> Cart { get; set; }
    }
}
