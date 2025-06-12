using TamoPOS.ViewModels.Pages;
using Wpf.Ui.Abstractions.Controls;

namespace TamoPOS.Views.Pages
{
    public partial class CategoryPage : INavigableView<CategoryViewModel>
    {
        public CategoryViewModel ViewModel { get; }

        public CategoryPage(CategoryViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = ViewModel;

            InitializeComponent();
        }
    }
}