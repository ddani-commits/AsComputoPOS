using System.Collections.ObjectModel;
using TamoPOS.Data;
using TamoPOS.Models;
using Wpf.Ui;

namespace TamoPOS.ViewModels.Pages
{
    public partial class PurchaseOrderViewModel : ViewModel
    {
        public ObservableCollection<PurchaseOrder> PurchaseOrders { get; } = new();
        private readonly IContentDialogService _contentDialogService;

        public void LoadPurchaseOrders()
        {
            using var db = new ApplicationDbContext();
            foreach(var purchaseOrder in db.PurchaseOrders)
            {
                PurchaseOrders.Add(purchaseOrder);
            }
        }

    }
}
