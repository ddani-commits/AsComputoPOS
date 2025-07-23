using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;
using System.Diagnostics;
using TamoPOS.Controls;
using TamoPOS.Data;
using TamoPOS.Models;
using TamoPOS.Services;
using Wpf.Ui;

namespace TamoPOS.ViewModels.Pages
{
    public partial class PurchaseOrderDetailViewModel : ViewModel
    {
        private ApplicationDbContext _applicationDbContext = new ApplicationDbContext();
        private IContentDialogService _contentDialogService;
        public ObservableCollection<ProductPurchase> ProductPurchases { get; } = new();
        private readonly IPOSService _posPanelService;
        private readonly IServiceProvider _serviceProvider;
        private readonly ProductsViewModel _productsViewModel;

        [ObservableProperty]
        private string? _idText;

        [ObservableProperty]
        private PurchaseOrder? _currentPurchaseOrder;

        [ObservableProperty]
        private string? _total;

        [ObservableProperty]
        private string? _subtotal;

        public PurchaseOrderDetailViewModel(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _posPanelService = _serviceProvider.GetRequiredService<IPOSService>();
            _contentDialogService = _serviceProvider.GetRequiredService<IContentDialogService>();
            _productsViewModel = _serviceProvider.GetRequiredService<ProductsViewModel>();
        }

        [RelayCommand]
        public async Task ShowNewProductPurchaseDialog()
        {
            if (_contentDialogService.GetDialogHost() is not null)
            {
                var newProductPurchaseContentDialog = new NewProductPurchaseContentDialog(
                    _applicationDbContext,
                    _contentDialogService.GetDialogHost(),
                    AddProductPurchase,
                    _posPanelService
                 );
                _ = await newProductPurchaseContentDialog.ShowAsync();
            }
        }

        [RelayCommand]
        public void LoadProductPurchases()
        {
            ProductPurchases.Clear();
            _applicationDbContext.ProductPurchases
                .Include(pp => pp.Product)
                .Where(pp => pp.PurchaseOrderId == CurrentPurchaseOrder!.Id)
                .ToList()
                .ForEach(pp => ProductPurchases.Add(pp));
        }

        [RelayCommand]
        public void AddProductPurchase(ProductPurchase productPurchase)
        {
            productPurchase.PurchaseOrderId = CurrentPurchaseOrder!.Id;

            _applicationDbContext.ProductPurchases.Add(productPurchase);
            
            CurrentPurchaseOrder.Total = CurrentPurchaseOrder.Total + productPurchase.Total;
            CurrentPurchaseOrder.Subtotal = CurrentPurchaseOrder.Subtotal + productPurchase.Total;
            
            _applicationDbContext.PurchaseOrders.Update(CurrentPurchaseOrder);

            _applicationDbContext.SaveChanges();
            
            ProductPurchases.Add(productPurchase);

            // just update the whole list, might hurt performance on long product lists
            if (productPurchase.QuantityRemaining > 0) _posPanelService.LoadProductsInStock();
            LoadDetails(CurrentPurchaseOrder.Id);
            _productsViewModel.LoadAllProducts();
        }

        public void LoadDetails(int Id)
        {
            CurrentPurchaseOrder = _applicationDbContext.PurchaseOrders
                .Include(po => po.Supplier)
                .Single(p => p.Id == Id);

            IdText = $"#{CurrentPurchaseOrder.Id}";
            Subtotal = $"${CurrentPurchaseOrder.Subtotal.ToString()}";
            Total = $"${CurrentPurchaseOrder.Total.ToString()}";
            Debug.WriteLine(CurrentPurchaseOrder.Total);
        }
    }
}