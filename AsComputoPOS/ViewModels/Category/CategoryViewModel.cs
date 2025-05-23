using System.Collections.ObjectModel;
using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using AsComputoPOS.Models;
using AsComputoPOS.Data;
using AsComputoPOS.Services;

namespace AsComputoPOS.ViewModels.Category
{
    public partial class CategoryViewModel : NavigationBarViewModel
    {
        [ObservableProperty]
        private Models.Category? selectedCategory;

        [ObservableProperty]
        private string categoryName = string.Empty;

        [ObservableProperty]
        private string parentCategoryName = string.Empty;

        public ObservableCollection<Models.Category> CategoriesList { get; } = new();
        public CategoryViewModel(INavigationService navigation, IAuthenticationService authenticationService) : base(navigation, authenticationService)
        {
            LoadCategories();
        }
        private void LoadCategories()
        {
            using var db = new ApplicationDbContext();
            foreach (var category in db.Categories)
            {
                category.ViewModel = this; // Esto permite acceder al comando desde XAML
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
