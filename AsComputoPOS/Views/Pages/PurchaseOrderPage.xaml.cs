using System.Windows.Controls;
using TamoPOS.ViewModels.Pages;
using Wpf.Ui.Abstractions.Controls;

namespace TamoPOS.Views.Pages
{
    public partial class PurchaseOrderPage : INavigableView<PurchaseOrderViewModel>
    {
        public PurchaseOrderViewModel ViewModel { get; }
        public PurchaseOrderPage(PurchaseOrderViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = ViewModel;

            InitializeComponent();
        }
    }
}
