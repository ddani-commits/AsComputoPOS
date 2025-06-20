using System.Collections.ObjectModel;
using TamoPOS.Controls;
using TamoPOS.Data;
using TamoPOS.Models;
using Wpf.Ui;

namespace TamoPOS.ViewModels.Pages
{
    public partial class ProductsViewModel : ViewModel
    {
        private readonly IContentDialogService _contentDialogService;
        public ObservableCollection<Product> ProductsList { get; } = new();
        private ApplicationDbContext _appDbContext = new();

        public ProductsViewModel(IContentDialogService contentDialogService)
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

        [RelayCommand]
        private async Task OnShowDialog()
        {
            if (_contentDialogService.GetDialogHost() is not null)
            {   // Example of how to open a content dialog, a dialog must be created. examples are in Controls folder
                var newProductDialog = new NewProductContentDialog(_appDbContext, _contentDialogService.GetDialogHost(), AddProduct);
                _ = await newProductDialog.ShowAsync();
            }
        }

        [RelayCommand]
        public void AddProduct(Product product)
        {
            _appDbContext.Products.Add(product);
            _appDbContext.SaveChanges();
            ProductsList.Add(product);
        }

        [RelayCommand]
        public void SaveProducts()
        {
            foreach (var product in ProductsList)
            {
                _appDbContext.Products.Update(product);
            }
            _appDbContext.SaveChanges();
        }
    }
}
