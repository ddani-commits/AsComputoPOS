using System.Collections.ObjectModel;
using TamoPOS.Models;

namespace TamoPOS.Services
{
    public interface IPOSService
    {
        bool IsSidePanelExpanded { get; set; }
        ObservableCollection<ProductPurchase> ProductsInStock { get; set; }
        void LoadProductsInStock();
        void AddToCart(Product product);
    }
}
