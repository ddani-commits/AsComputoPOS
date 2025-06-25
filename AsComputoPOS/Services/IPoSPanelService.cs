using System.Collections.ObjectModel;
using TamoPOS.Models;

namespace TamoPOS.Services
{
    public interface IPoSPanelService
    {
        void CollapseSidePanel(IServiceProvider serviceProvider);
        void ExpandSidePanel(IServiceProvider serviceProvider);
        bool IsSidePanelExpanded { get; set; }
        ObservableCollection<Product> ProductsInStock { get; set; }
        void LoadProductsInStock();
    }
}
