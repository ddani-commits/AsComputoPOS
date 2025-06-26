using System.Collections.ObjectModel;
using TamoPOS.Models;

namespace TamoPOS.Services
{
    public interface IPoSPanelService
    {
        bool IsSidePanelExpanded { get; set; }
        ObservableCollection<ProductPurchase> ProductsInStock { get; set; }
        void LoadProductsInStock();
        void AddToCart(Product product);
    }
}
