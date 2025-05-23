using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using DialogHostAvalonia;
using AsComputoPOS.ViewModels.Suppliers;
using System.Diagnostics;

namespace AsComputoPOS.Views.Suppliers;

public partial class SuppliersView : UserControl
{
    public SuppliersView()
    {
        InitializeComponent();
    }

    private async void Open(object? sender, RoutedEventArgs e)
    {
        await DialogHost.Show(Resources["Sample2View"]!, "MainDialogHost");
    }

    private void OnDialogClosing(object? sender, DialogClosingEventArgs e)
    {
        
        Debug.WriteLine("Dialog Closing");
        if (DataContext is SuppliersViewModel vm)
        {
            Debug.WriteLine("Saving data");
            vm.AddSupplier();
            vm.SaveSupplier();
        }
    }
}