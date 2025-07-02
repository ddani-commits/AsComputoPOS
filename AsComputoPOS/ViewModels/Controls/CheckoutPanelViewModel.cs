using System.Collections.ObjectModel;
using System.ComponentModel;
using TamoPOS.Controls.PointOfSalePanel;
using TamoPOS.Models;
using TamoPOS.Services;

namespace TamoPOS.ViewModels.Controls
{
    public partial class CheckoutPanelViewModel: ViewModel
    {
        public ObservableCollection<string> PaymentMethods => _posService.PaymentMethods;
        public ObservableCollection<CartItem> Cart => _posService.Cart;

        public string Total => $"Cobra {Cart.Sum(item => item.Total).ToString("C2")}";

        private IPOSService _posService;

        public CheckoutPanelViewModel(IPOSService posService)
        {
            _posService = posService;


            // Subscribe to collection changes
            Cart.CollectionChanged += (s, e) =>
            {
                OnPropertyChanged(nameof(Total));
                if (e.NewItems != null)
                {
                    foreach (CartItem item in e.NewItems)
                        item.PropertyChanged += CartItem_PropertyChanged;
                }
                if (e.OldItems != null)
                {
                    foreach (CartItem item in e.OldItems)
                        item.PropertyChanged -= CartItem_PropertyChanged;
                }
            };

            // Subscribe to property changes for existing items
            foreach (var item in Cart)
                item.PropertyChanged += CartItem_PropertyChanged;
        }

        private void CartItem_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(CartItem.Quantity) || e.PropertyName == nameof(CartItem.UnitPrice))
                OnPropertyChanged(nameof(Total));
        }
    }
}
