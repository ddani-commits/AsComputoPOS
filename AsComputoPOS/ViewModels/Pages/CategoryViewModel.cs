using System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using TamoPOS.Controls;
using TamoPOS.Data;
using TamoPOS.Models;
using TamoPOS.Services;
using Wpf.Ui;

namespace TamoPOS.ViewModels.Pages
{
    public partial class CategoryViewModel : ViewModel
    {
        [ObservableProperty]
        private Category? selectedCategory;

        [ObservableProperty]
        private string categoryName = string.Empty;

        [ObservableProperty]
        private string parentCategoryName = string.Empty;

        public ObservableCollection<Category> CategoriesList { get; } = new();
        private readonly IContentDialogService _contentDialogService;
        public CategoryViewModel(IContentDialogService contentDialogService)
        {
            _contentDialogService = contentDialogService;
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
        [RelayCommand]
        private async Task ShowSignInContentDialog()
        {
            if (_contentDialogService.GetDialogHost() is not null)
            {
                // Example of how to open a content dialog, a dialog must be created. examples are in Controls folder
                var newCategoryContentDialog = new NewCategoryContentDialog(_contentDialogService.GetDialogHost(), AddCategory);
                _ = await newCategoryContentDialog.ShowAsync();
            }
            Debug.WriteLine("Show SignIn Content Dialog Command Executed");
        }
        // Añadir
        [RelayCommand]
        public void AddCategory(Category CurrentCategory)
        {
            using (var context = new ApplicationDbContext())
            {
                context.Categories.Add(CurrentCategory);
                context.SaveChanges();
                CategoriesList.Add(CurrentCategory);

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
        public void DeleteCategory(Category category)
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

        [RelayCommand]
        public void ExportToExcel()
        {
            var dt = new DataTable("Category");
            dt.Columns.Add("ID", typeof(int));
            dt.Columns.Add("Nombre de Categoría", typeof(string));
            dt.Columns.Add("Nombre de Categoría Padre", typeof(string));

            foreach (var category in CategoriesList)
            {
                dt.Rows.Add(category.CategoryId, category.CategoryName, category.ParentCategoryName);
            }
            string dowloadpaths = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");
            string filePath = Path.Combine(dowloadpaths, "Categorias.xlsx");
            using (var wb = new ClosedXML.Excel.XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                wb.SaveAs(filePath);
            }
        }
    }
}
