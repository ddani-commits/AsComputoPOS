using System.Collections.ObjectModel;
using System.Threading.Tasks;
using TamoPOS.Controls;
using TamoPOS.Data;
using TamoPOS.Models;
using Wpf.Ui;

namespace TamoPOS.ViewModels.Pages
{
    public partial class PurchaseOrderViewModel : ViewModel
    {
        public ObservableCollection<PurchaseOrder> PurchaseOrders { get; } = new();
        private readonly IContentDialogService _contentDialogService;

        public PurchaseOrderViewModel(IContentDialogService contentDialogService)
        {
            _contentDialogService = contentDialogService;
            LoadPurchaseOrders();
        }

        public void LoadPurchaseOrders()
        {
            using var db = new ApplicationDbContext();
            foreach(var purchaseOrder in db.PurchaseOrders)
            {
                PurchaseOrders.Add(purchaseOrder);
            }
        }

        [RelayCommand]
        public async Task ShowNewPurchaseOrderDialog()
        {
            if(_contentDialogService.GetDialogHost() is not null)
            {
                var newPurcharseOrderContentDialog = new NewPurchaseOrderDialog(_contentDialogService.GetDialogHost(), AddPurchaseOrder);
                _ = await newPurcharseOrderContentDialog.ShowAsync();
            }
        }

        [RelayCommand]
        public void AddPurchaseOrder(PurchaseOrder purchaseOrder)
        {
            using (var context = new ApplicationDbContext())
            {
                context.PurchaseOrders.Add(purchaseOrder);
                context.SaveChanges();
                PurchaseOrders.Add(purchaseOrder);
            }
        }

        [RelayCommand]
        public void CreatePurchaseOrder()
        {

        }

    }
}