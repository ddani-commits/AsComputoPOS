using ExcelDataReader;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using TamoPOS.Controls;
using TamoPOS.Data;
using TamoPOS.Models;
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
        private string? selectedFolderPath;
        private readonly ApplicationDbContext _dbContext = new ApplicationDbContext();
        public int? ParentCategoryId { get; set; }
        public ObservableCollection<Category> CategoriesList { get; } = new();
        private readonly IContentDialogService _contentDialogService;
        public CategoryViewModel(IContentDialogService contentDialogService)
        {
            _contentDialogService = contentDialogService;
            LoadCategories();
        } 
        private void LoadCategories()
        {
            CategoriesList.Clear();
            foreach(var category in _dbContext.Categories)
            {
                CategoriesList.Add(category);
            }
        }
        
        [RelayCommand]
        private async Task ShowSignInContentDialog()
        {
            if (_contentDialogService.GetDialogHost() is not null)
            {
                var newCategoryContentDialog = new NewCategoryContentDialog(
                    _contentDialogService.GetDialogHost(),
                    _dbContext,
                    AddCategory
                );
                _ = await newCategoryContentDialog.ShowAsync();
            }
        }

        [RelayCommand]
        public void AddCategory(Category CurrentCategory) // think this is wrong, variables should be lowercase
        {
            _dbContext.Categories.Add(CurrentCategory);
            Debug.WriteLine(CurrentCategory.ParentCategory is null);
            _dbContext.SaveChanges();
            CategoriesList.Add(CurrentCategory); 
        }
        [RelayCommand]
        public void SaveCategory()
        {   
            foreach (var category in CategoriesList)
            {
                _dbContext.Categories.Update(category);
            }
            _dbContext.SaveChanges();
            LoadCategories();
            Debug.WriteLine("Saved from ViewModel");
        }

        [RelayCommand]
        public void DeleteCategory(object parameter)
        {
            if (parameter is not Category category) return;
            var categoryToDelete = _dbContext.Categories.Find(category.CategoryId);

            if (categoryToDelete != null)
            {
                _dbContext.Categories.Remove(categoryToDelete);
                _dbContext.SaveChanges();
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
        public void SelectFolder()
        {
#if NET8_0_OR_GREATER
            OpenFolderDialog openFolderDialog = new()
            {
                Multiselect = false,
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
            };
            if (openFolderDialog.ShowDialog() != true || openFolderDialog.FolderNames.Length == 0)
            {
                return;
            }
            string selectedFolder = openFolderDialog.FolderNames[0];
            SelectedFolderPath = selectedFolder;

            try
            {
                ExportToExcel(selectedFolder);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error exporting to Excel: {ex.Message}");
            }
#else
            Debug.WriteLine("Esta función requiere .NET 8 o superior.");
#endif
        }
        [RelayCommand]
        public void ExportToExcel(string folderpath)
        {
            var dt = new DataTable("Category");
            dt.Columns.Add("ID", typeof(int));
            dt.Columns.Add("Nombre de Categoría", typeof(string));
            dt.Columns.Add("Nombre de la Categoría Padre", typeof(string));

            foreach (var category in CategoriesList)
            {
                dt.Rows.Add(category.CategoryId, category.CategoryName, category.ParentCategoryName);
            }
            string filePath = Path.Combine(folderpath, "Categories.xlsx");

            using (var wb = new ClosedXML.Excel.XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                wb.SaveAs(filePath);
            }
        }
        [RelayCommand]
        public void ImportFormExcel()
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "Excel Files|*.xlsx;*xls",
                Title = "Importar categorías desde Excel"
            };
            if (openFileDialog.ShowDialog() != true)
                return;
            try
            {
                using var stream = File.Open(openFileDialog.FileName, FileMode.Open, FileAccess.Read);
                using var reader = ExcelReaderFactory.CreateReader(stream);
                var dataSet = reader.AsDataSet(new ExcelDataSetConfiguration
                {
                    ConfigureDataTable = _ => new ExcelDataTableConfiguration
                    {
                        UseHeaderRow = true
                    }
                });
                var dataTable = dataSet.Tables[0];
                CategoriesList.Clear();
                foreach (DataRow row in dataTable.Rows)
                {
                    var idRaw = row["ID"]?.ToString();
                    var nombre = row["Nombre de Categoría"]?.ToString() ?? string.Empty;
                    var padreName = row["Nombre de la categoría padre"]?.ToString();
                    int? parentId = null;
                    if (!string.IsNullOrWhiteSpace(padreName))
                    {
                        var parent = _dbContext.Categories.FirstOrDefault(c => c.CategoryName == padreName);
                        parentId = parent?.CategoryId;
                    }
                    Category? category = null;
                    if (int.TryParse(idRaw, out int id) && id > 0)
                    {
                        category = _dbContext.Categories.FirstOrDefault(c => c.CategoryId == id);
                        if (category != null)
                        {
                            category.CategoryName = nombre;
                            category.ParentCategoryId = parentId;
                            _dbContext.Categories.Update(category);
                        }
                    }
                    if (category == null)
                    {
                        category = new Category
                        {
                            CategoryName = nombre,
                            ParentCategoryId = parentId,
                        };
                        _dbContext.Categories.Add(category);
                    }
                    CategoriesList.Add(category);
                }
                _dbContext.SaveChanges();
                CategoriesList.Clear();
                foreach(var cat in _dbContext.Categories.Include(c => c.ParentCategory))
                {
                    CategoriesList.Add(cat);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error al importar: {ex.Message}");
            }
        }
    }
}
