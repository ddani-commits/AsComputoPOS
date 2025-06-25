using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using System.Windows.Media;
using TamoPOS.Data;
using TamoPOS.Models;
using Wpf.Ui.Controls;

namespace TamoPOS.Controls
{
    public partial class NewCategoryContentDialog : ContentDialog
    {
        private string _categoryName = "";
        private string _parentCategoryName = "";
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly ContentPresenter? _contentPresenter;
        public List<string> CategoryList = new();
        private readonly Action<Category>? _saveCategories;
        private Category? _selectedCategory;    
        
        public string CategoryNameText
        {
            get => _categoryName;
            set { _categoryName = value; OnPropertyChanged(); }
        }
        public string ParentCategoryNameText
        {
            get => _parentCategoryName;
            set { _parentCategoryName = value; OnPropertyChanged(); }
        }
        public Category? SelectedCategory
        {
            get => _selectedCategory;
            set
            {
                _selectedCategory = value;
                ParentCategoryNameText = _selectedCategory?.CategoryName ?? string.Empty;
                OnPropertyChanged();
            }
        }   
        
        public NewCategoryContentDialog(
            ContentPresenter? contentPresenter,
            ApplicationDbContext dbContext,
            Action<Category>? saveCategories = null
        ) : base(contentPresenter)
        {
            InitializeComponent();
            _contentPresenter = contentPresenter;
            _applicationDbContext = dbContext;
            _saveCategories = saveCategories;
            DataContext = this;
        }    
        
        protected override void OnButtonClick(ContentDialogButton button)
        {
            if (button == ContentDialogButton.Primary)
            {
                if (string.IsNullOrWhiteSpace(CategoryNameText))
                {
                    ErrorsMessageTextBlock.Text = "El nombre de la categoría no puede estar vacío.";
                    ErrorsMessageTextBlock.Visibility = Visibility.Visible;
                    return;
                }
                Category category = new Category
                {
                    CategoryName = CategoryNameText,
                    ParentCategoryId = SelectedCategory?.CategoryId
                };
                _saveCategories?.Invoke(category);
                base.OnButtonClick(button);
            }
            else if (button == ContentDialogButton.Close)
            {
                base.OnButtonClick(button);
            }
        }
        private void CategoryAutoSuggestBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                var categories = _applicationDbContext.Categories
                      .Where(c => c.CategoryName.Contains(sender.Text))
                      .ToList();
                CategoryAutoSuggestBox.OriginalItemsSource = categories;
            }
        }
        public event PropertyChangedEventHandler? PropertyChanged;
        private void CategoryAutoSuggestBox_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            if (args.SelectedItem is Category) _selectedCategory = args.SelectedItem as Category;
        }
        private void OnPropertyChanged([CallerMemberName] string propertyName = null!)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}