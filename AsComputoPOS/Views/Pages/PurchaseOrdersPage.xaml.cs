using System.Windows.Controls;
using TamoPOS.ViewModels.Pages;
using Wpf.Ui.Abstractions.Controls;

namespace TamoPOS.Views.Pages
{
    public partial class PurchaseOrdersPage : INavigableView<PurchaseOrdersViewModel>
    {
        public PurchaseOrdersViewModel ViewModel { get; }
        public PurchaseOrdersPage(PurchaseOrdersViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = ViewModel;

            InitializeComponent();
        }
    }
}
