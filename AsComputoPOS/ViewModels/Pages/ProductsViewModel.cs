using System.Collections.ObjectModel;
using System.Diagnostics;
using TamoPOS.Controls;
using TamoPOS.Data;
using TamoPOS.Models;
using Wpf.Ui;

namespace TamoPOS.ViewModels.Pages
{
    public partial class ProductsViewModel: ViewModel
    {
        [ObservableProperty]
        private string _productName = "";
        [ObservableProperty]
        private bool _isActive = true;
        [ObservableProperty]
        private string _barcode = "";
        [ObservableProperty]
        private string _SKU = "";
        
        private readonly IContentDialogService _contentDialogService;
        public ObservableCollection<Product> ProductsList { get; } = new();
        
        public ProductsViewModel(IContentDialogService contentDialogService)
        {
            _contentDialogService = contentDialogService;
            LoadProducts();
        }

        private void LoadProducts()
        {
            using var db = new ApplicationDbContext();
            foreach (var product in db.Products)
            {
                ProductsList.Add(product);
            }
        }

        [RelayCommand]
        private async Task OnShowDialog()
        {
            Debug.WriteLine("Show dialog button Clicked");
            if (_contentDialogService.GetDialogHost() is not null)
            {   // Example of how to open a content dialog, a dialog must be created. examples are in Controls folder
                var newProductDialog = new NewProductContentDialog(_contentDialogService.GetDialogHost(), AddProduct);
                _ = await newProductDialog.ShowAsync();
            }
        }

        [RelayCommand]
        public void AddProduct(Product product)
        {
            using (var context = new ApplicationDbContext())
            {
                context.Products.Add(product);
                context.SaveChanges();
                ProductsList.Add(product);
            }
            Console.WriteLine("Add Product command executed.");
        }

        [RelayCommand]
        public void SaveProducts()
        {
            using (var context = new ApplicationDbContext())
            {
                foreach (var product in ProductsList)
                {
                    context.Products.Update(product);
                }
                context.SaveChanges();
            }
        }
    }
}
