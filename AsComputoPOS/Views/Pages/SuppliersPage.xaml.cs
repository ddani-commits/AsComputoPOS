using TamoPOS.ViewModels.Pages;
using Wpf.Ui.Abstractions.Controls;

namespace TamoPOS.Views.Pages
{
    public partial class SuppliersPage : INavigableView<SuppliersViewModel>
    {
        public SuppliersViewModel ViewModel { get; }
        public SuppliersPage(SuppliersViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = ViewModel;

            InitializeComponent();           
        }
    }
}