using TamoPOS.ViewModels.Pages;
using Wpf.Ui.Abstractions.Controls;

namespace TamoPOS.Views.Pages
{
    public partial class SalesHistoryPage : INavigableView<SalesHistoryViewModel>
    {
        public SalesHistoryViewModel ViewModel { get; }
        public SalesHistoryPage(SalesHistoryViewModel viewModel)   
        {
            ViewModel = viewModel;
            DataContext = ViewModel;
            InitializeComponent();
        }

    }
}
