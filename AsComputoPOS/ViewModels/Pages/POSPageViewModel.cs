using System.Collections.ObjectModel;
using System.Diagnostics;
using TamoPOS.Models;
using TamoPOS.Services;
using Wpf.Ui;

namespace TamoPOS.ViewModels.Pages
{
    public partial class POSPageViewModel: ViewModel
    {
        private readonly IContentDialogService _contentDialogService;
        public ObservableCollection<Product> ProductsList { get; } = new();
        private readonly IPOSService _posPanelService;
        private ObservableCollection<ProductPurchase> _productsInStock;
        public ObservableCollection<ProductPurchase> ProductsInStock
        {
            get => _productsInStock;
            set
            {
                if (_productsInStock != value)
                {
                    _productsInStock = value;
                    OnPropertyChanged(nameof(ProductsInStock));
                }
            }
        }
        public POSPageViewModel(IContentDialogService contentDialogService, IPOSService posPanelService)
        {
            _posPanelService = posPanelService;
            _contentDialogService = contentDialogService;
            ProductsInStock = _posPanelService.ProductsInStock; // doesnt work
            _posPanelService.LoadProductsInStock();
        }

        [RelayCommand]
        public void AddProductToCart(ProductPurchase product)
        {
            if (product == null) return;
            _posPanelService.AddToCart(product);
        }

        [RelayCommand]
        public void SearchProductByName(string searchText)
        {
            // 1. Should only search in products inside the ProductsInStock variable
            Debug.WriteLine(searchText);
            if (string.IsNullOrWhiteSpace(searchText))
            {
                ProductsInStock = _posPanelService.ProductsInStock;
            }

            //var filtered = ProductsInStock
                //.Where(p => p.Product.Name.Contains(searchText, StringComparison.OrdinalIgnoreCase));
                //.Select(p => p.Product);
            //ProductsInStock.Clear();
            //foreach(var product in filtered)
            //{
                //ProductsInStock.Add(product);
                //Debug.WriteLine(ProductsList.Count);
            //}
        }
    }
}