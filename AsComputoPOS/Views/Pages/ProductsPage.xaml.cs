using System.ComponentModel;
using System.Diagnostics;
using TamoPOS.Models;
using TamoPOS.ViewModels.Pages;
using Wpf.Ui.Abstractions.Controls;

namespace TamoPOS.Views.Pages
{
    public partial class ProductsPage : INavigableView<ProductsViewModel>, INotifyPropertyChanged
    {
        public ProductsViewModel ViewModel { get; }
        public ProductsPage(ProductsViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = this;
            InitializeComponent();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class Product
    {
        public Product()
        {
            ProductPurchase = new List<ProductPurchase>();
        }

        public List<ProductPurchase> ProductPurchase { get; set; }
    }
}
