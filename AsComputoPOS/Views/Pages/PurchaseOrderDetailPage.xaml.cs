using TamoPOS.ViewModels.Pages;
using Wpf.Ui.Abstractions.Controls;

namespace TamoPOS.Views.Pages
{
    public partial class PurchaseOrderDetailPage : INavigableView<PurchaseOrderDetailViewModel>
    {
        public PurchaseOrderDetailViewModel ViewModel { get; }
        public PurchaseOrderDetailPage(PurchaseOrderDetailViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = ViewModel;
            InitializeComponent();
        }
    }
}
