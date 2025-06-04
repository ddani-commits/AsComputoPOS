using System.Collections.ObjectModel;
using System.Diagnostics;
using UiDesktopApp1.Data;
using UiDesktopApp1.Services;
using Wpf.Ui;

namespace UiDesktopApp1.ViewModels.Pages
{
    public partial class CategoryViewModel : ObservableObject
    {
        [ObservableProperty]
        private Models.Category? selectedCategory;

        [ObservableProperty]
        private string categoryName = string.Empty;

        [ObservableProperty]
        private string parentCategoryName = string.Empty;

        public ObservableCollection<Models.Category> CategoriesList { get; } = new();
        public CategoryViewModel()
        {
            LoadCategories();
        }
        private void LoadCategories()
        {
            using var db = new ApplicationDbContext();
            foreach (var category in db.Categories)
            {
                category.ViewModel = this;
                CategoriesList.Add(category);
            }
        }

        // Añadir
        [RelayCommand]
        public void AddCategory()
        {
            using (var context = new ApplicationDbContext())
            {
                var currentCategory = new Models.Category(CategoryName, ParentCategoryName);
                context.Categories.Add(currentCategory);
                context.SaveChanges();
                CategoriesList.Add(currentCategory);
                ClearFields();
            }
        }
        // Guardar

        [RelayCommand]
        public void SaveCategory()
        {
            using var db = new ApplicationDbContext();
            foreach (var category in CategoriesList)
            {
                db.Categories.Update(category);
            }
            db.SaveChanges();
            Debug.WriteLine("Saved from ViewModel");
        }

        // Elliminar
        [RelayCommand]
        public void DeleteCategory(Models.Category category)
        {
            if (category is null) return;

            using var db = new ApplicationDbContext();
            var categoryToDelete = db.Categories.Find(category.CategoryId);

            if (categoryToDelete != null)
            {
                db.Categories.Remove(categoryToDelete);
                db.SaveChanges();
                CategoriesList.Remove(category);

                if (SelectedCategory?.CategoryId == category.CategoryId)
                    SelectedCategory = null;
            }
            else
            {
                Debug.WriteLine("Category not found.");
            }
        }
        public void ClearFields()
        {
            CategoryName = string.Empty;
            ParentCategoryName = string.Empty;
        }
    }
}
