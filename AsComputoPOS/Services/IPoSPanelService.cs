using System.Collections.ObjectModel;
using TamoPOS.Models;

namespace TamoPOS.Services
{
    public interface IPoSPanelService
    {
        void CollapseSidePanel(IServiceProvider serviceProvider);
        void ExpandSidePanel(IServiceProvider serviceProvider);
        bool IsSidePanelExpanded { get; set; }
        ObservableCollection<ProductPurchase> ProductsInStock { get; set; }
        void LoadProductsInStock();
    }
}
