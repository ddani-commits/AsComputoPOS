using AsComputoPOS.ViewModels;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using DialogHostAvalonia;

namespace AsComputoPOS.Views;

public partial class EmployeesView : UserControl
{
    public EmployeesView()
    {
        InitializeComponent();
    }

    private async void Open(object? sender, RoutedEventArgs e)
    {
        await DialogHost.Show(Resources["Sample2View"]!, "MainDialogHost");
    }

    private void OnDialogClosing(object? sender, DialogClosingEventArgs e)
    {
        if(DataContext is EmployeesViewModel vm)
        {
            vm.AddEmployee();
            vm.SaveEmployees();
        }
    }
}