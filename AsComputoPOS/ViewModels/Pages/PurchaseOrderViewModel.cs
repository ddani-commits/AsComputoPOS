using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using TamoPOS.Controls;
using TamoPOS.Data;
using TamoPOS.Models;
using Wpf.Ui;

namespace TamoPOS.ViewModels.Pages
{
    public partial class PurchaseOrdersViewModel : ViewModel
    {
        public ObservableCollection<PurchaseOrder> PurchaseOrders { get; } = new();
        private readonly IContentDialogService _contentDialogService;
        private readonly ApplicationDbContext _dbContext = new ApplicationDbContext();

        public PurchaseOrdersViewModel(IContentDialogService contentDialogService)
        {
            _contentDialogService = contentDialogService;
            LoadPurchaseOrders();
        }

        public void LoadPurchaseOrders()
        {
            var orders = _dbContext.PurchaseOrders
                                   .Include(po => po.Supplier) // Include nested object Supplier
                                   .ToList();

            foreach (var purchaseOrder in orders)
            {
                PurchaseOrders.Add(purchaseOrder);
            }
        }


        [RelayCommand]
        public async Task ShowNewPurchaseOrderDialog()
        {
            if(_contentDialogService.GetDialogHost() is not null)
            {
                var newPurchaseOrderContentDialog = new NewPurchaseOrderDialog(_dbContext, _contentDialogService.GetDialogHost(), AddPurchaseOrder);
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