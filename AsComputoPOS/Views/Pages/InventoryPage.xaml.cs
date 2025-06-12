using System.Diagnostics;
using TamoPOS.ViewModels.Pages;
using Wpf.Ui.Abstractions.Controls;

namespace TamoPOS.Views.Pages
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
