using System.Collections.ObjectModel;
using TamoPOS.Controls.PointOfSalePanel;
using TamoPOS.Models;
using TamoPOS.Services;

namespace TamoPOS.ViewModels.Controls
{
    public partial class CheckoutPanelViewModel: ViewModel
    {
        public ObservableCollection<string> PaymentMethods => _posService.PaymentMethods;
        public ObservableCollection<ProductPurchase> Cart => _posService.Cart;

        private IPOSService _posService;

        public CheckoutPanelViewModel(IPOSService posService)
        {
            _posService = posService;
        }
    }
}
