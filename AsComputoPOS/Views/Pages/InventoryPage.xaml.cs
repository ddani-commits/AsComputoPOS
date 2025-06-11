using System.Diagnostics;
using UiDesktopApp1.ViewModels.Pages;
using Wpf.Ui.Abstractions.Controls;

namespace UiDesktopApp1.Views.Pages
{
    public partial class InventoryPage : INavigableView<InventoryViewModel>
    {
        public InventoryViewModel ViewModel { get; }
        public InventoryPage(InventoryViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = ViewModel;
            InitializeComponent();
        }
    }
}
