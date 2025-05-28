using AsComputoPOS.ViewModels.Products;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using DialogHostAvalonia;
using System.Diagnostics;

namespace AsComputoPOS.Views.Products;

public partial class ProductsView : UserControl
{
    public ProductsView()
    {
        InitializeComponent();
    }

    private async void Open(object? sender, RoutedEventArgs e)
    {
        await DialogHost.Show(Resources["ProductForm"], "ProductDialogHost");
    }

    private void OnDialogClosing(object? sender, DialogClosingEventArgs e)
    {
        if (DataContext is ProductsViewModel vm)
        {
            vm.AddProduct();
            Debug.WriteLine("Creating new product...");
        }
    }
}