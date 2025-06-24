using System.Collections.ObjectModel;
using TamoPOS.Data;
using TamoPOS.Models;
using Wpf.Ui;

namespace TamoPOS.ViewModels.Pages
{
    public partial class PointOfSaleViewModel: ViewModel
    {
        private readonly IContentDialogService _contentDialogService;
        public ObservableCollection<Product> ProductsList { get; } = new();
        private ApplicationDbContext _appDbContext = new();

        public PointOfSaleViewModel(IContentDialogService contentDialogService)
        {
            _contentDialogService = contentDialogService;
            LoadProducts();
        }

        private void LoadProducts()
        {
            // Only show products with stock
            var productPurchases = _appDbContext.ProductPurchases
                .Where(productPurchase => productPurchase.QuantityRemaining > 0)
                .GroupBy(productPurchase => productPurchase.ProductId)
                .Select(g => g.OrderBy(pp => (double)pp.QuantityRemaining!).First().Product)
                .ToList();

            foreach (var productPurchase in productPurchases)
            {
                ProductsList.Add(productPurchase);
            }
        }
    }
}
