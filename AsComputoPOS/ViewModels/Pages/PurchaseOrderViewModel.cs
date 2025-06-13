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
        private readonly ApplicationDbContext _dbContext = new ApplicationDbContext();

        public PurchaseOrderViewModel(IContentDialogService contentDialogService)
        {
            _contentDialogService = contentDialogService;
            LoadPurchaseOrders();
        }

        public void LoadPurchaseOrders()
        {
            foreach(var purchaseOrder in _dbContext.PurchaseOrders)
            {
                PurchaseOrders.Add(purchaseOrder);
            }
        }

        [RelayCommand]
        public async Task ShowNewPurchaseOrderDialog()
        {
            if(_contentDialogService.GetDialogHost() is not null)
            {
                var newPurchaseOrderContentDialog = new NewPurchaseOrderDialog(_contentDialogService.GetDialogHost(), AddPurchaseOrder);
                _ = await newPurchaseOrderContentDialog.ShowAsync();
            }
        }

        [RelayCommand]
        public void AddPurchaseOrder(PurchaseOrder purchaseOrder)
        {
            _dbContext.PurchaseOrders.Add(purchaseOrder);
            _dbContext.SaveChanges();
            PurchaseOrders.Add(purchaseOrder);
        }

        [RelayCommand]
        public void CreatePurchaseOrder()
        {

        }

    }
}