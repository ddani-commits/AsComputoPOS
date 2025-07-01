using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Data;
using TamoPOS.Models;
using TamoPOS.Services;
using Wpf.Ui;

namespace TamoPOS.ViewModels.Pages
{
    public partial class POSPageViewModel : ViewModel
    {
        private readonly IContentDialogService _contentDialogService;
        public ObservableCollection<Product> ProductsList { get; } = new();
        private readonly IPOSService _posPanelService;
        public ICollectionView DisplayProducts { get; }

        public POSPageViewModel(IContentDialogService contentDialogService, IPOSService posPanelService)
        {
            _posPanelService = posPanelService;
            _contentDialogService = contentDialogService;
            _posPanelService.LoadProductsInStock();
            DisplayProducts = CollectionViewSource.GetDefaultView(_posPanelService.ProductsInStock);
        }

        [RelayCommand]
        public void AddProductToCart(ProductPurchase product)
        {
            if (product == null) return;
            var ps = _posPanelService.Cart.FirstOrDefault(x => x.ProductId == product.ProductId);

            if (ps == null)
            {
                ps = new CartItem()
                {
                    Product = product.Product,
                    ProductId = product.ProductId,
                    Quantity = 1,
                    UnitPrice = product.SalePrice,
                };
               _posPanelService.AddToCart(ps);
            }
            else
            {
                if(ps.Quantity >= product.QuantityRemaining) return;
                ps.Quantity++;
                Debug.WriteLine(ps.Quantity);
            }

        }

        [RelayCommand]
        public void Filter(string searchText)
        {
            if (string.IsNullOrWhiteSpace(searchText))
            {
                DisplayProducts.Filter = null;
                DisplayProducts.Refresh();
            }

            DisplayProducts.Filter = item =>
            {
                if (item is ProductPurchase productPurchase)
                {
                    return productPurchase.Product.Name.Contains(searchText, StringComparison.OrdinalIgnoreCase)
                   || productPurchase.Product.Barcode.Contains(searchText, StringComparison.OrdinalIgnoreCase);
                }
                return false;
            };
        }
    }
}