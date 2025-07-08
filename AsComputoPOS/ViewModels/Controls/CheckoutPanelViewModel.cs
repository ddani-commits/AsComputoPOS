using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using TamoPOS.Controls.PointOfSalePanel;
using TamoPOS.Models;
using TamoPOS.Services;
using Wpf.Ui;

namespace TamoPOS.ViewModels.Controls
{
    public partial class CheckoutPanelViewModel: ViewModel
    {
        private IPOSService _posService;
        private IContentDialogService? _contentDialogService;
        public ObservableCollection<string> PaymentMethods => _posService.PaymentMethods;
        public ObservableCollection<CartItem> Cart => _posService.Cart;
        public decimal Total => _posService.Total;
        public string CheckoutButtonText => Total == 0 ? "Carrito Vacío" : $"Cobrar {Total:C2}";

        public CheckoutPanelViewModel(IPOSService posService)
        {
            _posService = posService;

            // Subscribe to collection changes
            Cart.CollectionChanged += (s, e) =>
            {
                OnPropertyChanged(nameof(Total));
                OnPropertyChanged(nameof(CheckoutButtonText));

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

        public void SetContentDialogService(IContentDialogService contentDialogService)
        {
            _contentDialogService = contentDialogService;
        }

        private void CartItem_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(CartItem.Quantity) || e.PropertyName == nameof(CartItem.UnitPrice))
                OnPropertyChanged(nameof(Total));
                OnPropertyChanged(nameof(CheckoutButtonText));
        }

        [RelayCommand]
        private async Task OnShowCheckoutContentDialog()
        {
            if (_contentDialogService.GetDialogHost() is not null)
            {
                var paymentContentDialog = new CheckoutContentDialog(_contentDialogService.GetDialogHost(), _posService);
                _ = await paymentContentDialog.ShowAsync();
            } else
            {
                Debug.WriteLine("Dialog host error");
            }
        }
    }
}
