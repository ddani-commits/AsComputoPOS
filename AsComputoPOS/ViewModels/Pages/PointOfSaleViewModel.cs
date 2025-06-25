using System.Collections.ObjectModel;
using TamoPOS.Models;
using TamoPOS.Services;
using Wpf.Ui;

namespace TamoPOS.ViewModels.Pages
{
    public partial class PointOfSaleViewModel: ViewModel
    {
        private readonly IContentDialogService _contentDialogService;
        public ObservableCollection<Product> ProductsList { get; } = new();
        private readonly IPoSPanelService _posPanelService;
        public ObservableCollection<ProductPurchase> ProductsInStock => _posPanelService.ProductsInStock;

        public PointOfSaleViewModel(IContentDialogService contentDialogService, IPoSPanelService posPanelService)
        {
            _posPanelService = posPanelService;
            _contentDialogService = contentDialogService;
            _posPanelService.LoadProductsInStock();
        }

        [RelayCommand]
        public void AddProductToCart(Product product)
        {
            if (product == null) return;
            _posPanelService.AddToCart(product);
        }
    }
}
