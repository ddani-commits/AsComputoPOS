using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Navigation;
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
            DataContext = ViewModel;
            InitializeComponent();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            ViewModel.LoadProductsInStock(); // Llamamos a LoadProductsInStock para recargar los productos cuando la página se cargue
        }
    }
}