using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Diagnostics;
using TamoPOS.Controls;
using TamoPOS.Data;
using TamoPOS.Models;
using Wpf.Ui;

namespace TamoPOS.ViewModels.Pages
{
    public partial class PurchaseOrderDetailViewModel: ViewModel
    {
        [ObservableProperty]
        private string? _idText;

        [ObservableProperty]
        private PurchaseOrder? _currentPurchaseOrder;

        [ObservableProperty]
        private string? _total;

        [ObservableProperty]
        private string? _subtotal;

        private IContentDialogService _contentDialogService;

        public ObservableCollection<ProductPurchase> PurchaseOrders { get; } = new ObservableCollection<ProductPurchase>()
        {
            new ProductPurchase()
            {
                Id = 0,
                FlatProfitMargin = 5,
                PercentProfitMargin = 19,
                Product = new Product("Sabritas", true, "24949", "24u842"),
                ProductId = 4,
                Quantity = 10,
                SalePrice = 294,
                Subtotal = 249,
                Total = 249 * 10,
                UnitPrice = 848
            },
            new ProductPurchase()
            {
                Id = 1,
                FlatProfitMargin = 34,
                PercentProfitMargin = 1,
                Product = new Product("Coca Cola", true, "99258", "67193"),
                ProductId = 5,
                Quantity = 20,
                SalePrice = 20 + 34,
                Subtotal = 249,
                Total = 249 * 10,
                UnitPrice = 15
            }
        };

        public PurchaseOrderDetailViewModel(IContentDialogService contentDialogService) 
        {
            _contentDialogService = contentDialogService;
        }

        [RelayCommand]
        public async Task ShowNewProductPurchaseDialog()
        {
            if (_contentDialogService.GetDialogHost() is not null)
            {
                var newProductPurchaseContentDialog = new NewProductPurchaseContentDialog(_contentDialogService.GetDialogHost(), AddProductPurchase);
                _ = await newProductPurchaseContentDialog.ShowAsync();
            }
        }

        [RelayCommand]
        public void AddProductPurchase(ProductPurchase productPurchase)
        {
            PurchaseOrders.Add(productPurchase);
        }

        public void LoadDetails(int Id)
        {
            using (var appDbContext = new ApplicationDbContext())
            {
                var purchaseOrder = appDbContext.PurchaseOrders
                    .Include(po => po.Supplier)
                    .Single(p => p.Id == Id);
                CurrentPurchaseOrder = purchaseOrder;
            }
            IdText = $"#{CurrentPurchaseOrder.Id}";
            Subtotal = $"${CurrentPurchaseOrder.Subtotal.ToString()}"; 
            Total = $"${CurrentPurchaseOrder.Total.ToString()}";
        }
    }
}