using AsComputoPOS.ViewModels.Category;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using DialogHostAvalonia;


namespace AsComputoPOS.Views.Category;

public partial class CategoryView : UserControl
{
    public CategoryView()
    {
        InitializeComponent();
    }
    private async void Open(object? sender, RoutedEventArgs e)
    {
        await DialogHost.Show(Resources["Sample2View"]!, "MainDialogHost");
    }

    private void OnDialogClosing(object? sender, DialogClosingEventArgs e)
    {
       if(DataContext is CategoryViewModel vm)
        {
            vm.AddCategory();
            vm.SaveCategory();
           
        }
    }

    private void Button_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
    }
}