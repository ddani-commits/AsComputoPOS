using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using TamoPOS.Models;
using Wpf.Ui.Controls;

namespace TamoPOS.Controls
{
    /// <summary>
    /// Lógica de interacción para NewCategoryContentDialog.xaml
    /// </summary>
    public partial class NewCategoryContentDialog : ContentDialog
    {
        private string _categoryName = "";
        private string _parentCategoryName = "";

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

        private readonly Action<Category>? _saveCategories;

        public NewCategoryContentDialog(ContentPresenter? contentPresenter, Action<Category>? saveCategories = null) : base(contentPresenter)
        {
            InitializeComponent();
            _saveCategories = saveCategories;
            DataContext = this;
        }
        protected override void OnButtonClick(ContentDialogButton button)
        {
            if (button == ContentDialogButton.Primary)
            {
                var category = new Category(CategoryNameText, ParentCategoryNameText);
                _saveCategories?.Invoke(category);
                base.OnButtonClick(button);
                Debug.WriteLine("Primary button clickerd");
            }
            else if (button == ContentDialogButton.Close)
            {
                base.OnButtonClick(button);
                Debug.WriteLine("Close button clicked");
            }
        }
        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = null!)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
