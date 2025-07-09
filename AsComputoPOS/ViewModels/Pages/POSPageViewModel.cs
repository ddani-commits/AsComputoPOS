using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;
using TamoPOS.Models;
using TamoPOS.Services;

namespace TamoPOS.ViewModels.Pages
{
    public partial class POSPageViewModel : ViewModel
    {
        public ObservableCollection<Product> ProductsList { get; } = new();
        private readonly IPOSService _posPanelService;
        public ICollectionView DisplayProducts { get; }

        public POSPageViewModel(IPOSService posPanelService)
        {
            _posPanelService = posPanelService;
            _posPanelService.LoadProductsInStock();
            DisplayProducts = CollectionViewSource.GetDefaultView(_posPanelService.ProductsInStock);
        }

        [RelayCommand]
        public void AddProductToCart(ProductPurchase product)
        {
            if (product == null) return;

            var cartItem = _posPanelService.Cart.FirstOrDefault(x => x.ProductId == product.ProductId);
            if (cartItem == null)
            {
                cartItem = new CartItem()
                {   
                    Product = product.Product,
                    ProductId = product.ProductId,
                    Quantity = 1,
                    UnitPrice = product.SalePrice,
                };
               _posPanelService.AddToCart(cartItem);
            }
            else
            {
                if(product.QuantityRemaining == 0) return;
                cartItem.Quantity++;
            }
            product.QuantityRemaining--;
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