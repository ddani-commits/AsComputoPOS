using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Input;
using TamoPOS.Models;
using TamoPOS.ViewModels.Pages;
using Wpf.Ui.Controls;

namespace TamoPOS.Views.Pages
{
    public partial class ProductDetailPage : Page
    {
        public ProductDetailViewModel ViewModel { get; }
        public ProductDetailPage(ProductDetailViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = ViewModel;
            InitializeComponent();
        }
        private void CategoryBox_TextChanged(object sender, AutoSuggestBoxTextChangedEventArgs e)
        {
            if (e.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                var autoSuggestBox = (AutoSuggestBox)sender;
                    autoSuggestBox.Text = string.Empty;
                    autoSuggestBox.OriginalItemsSource = ViewModel.CategoryList;     
            }
        }
        private void CategoryBox_SuggestionChosen(object sender, AutoSuggestBoxSuggestionChosenEventArgs e)
        {
            if(e.SelectedItem is Category selectedCategory)
            {
                ViewModel.SelectedCategory = selectedCategory;
                ViewModel.UpdateProductDetails(); 
            }
        }
        private void ProductNameTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ViewModel.UpdateProductDetails();
            }
        }
    }
}
 
    

