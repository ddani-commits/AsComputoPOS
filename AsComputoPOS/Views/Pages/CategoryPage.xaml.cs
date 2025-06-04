using UiDesktopApp1.ViewModels.Pages;
using Wpf.Ui.Abstractions.Controls;

namespace UiDesktopApp1.Views.Pages
{
    public partial class CategoryPage : INavigableView<CategoryViewModel>
    {
        public CategoryViewModel ViewModel { get; }

        public CategoryPage(CategoryViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = viewModel;

            InitializeComponent();
        }
    }
}