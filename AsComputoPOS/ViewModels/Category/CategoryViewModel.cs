using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AsComputoPOS.Services;
using AsComputoPOS.Models;
using AsComputoPOS.Data;
using CommunityToolkit.Mvvm.Input;
using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;

namespace AsComputoPOS.ViewModels.Category
{
    public partial class CategoryViewModel : NavigationBarViewModel
    {
        // Base de datos
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
                CategoriesList.Add(category);
            }
        }

        // Popup

        [ObservableProperty]
        private bool isPopupOpen = false;

        [RelayCommand]
        private void ShowPopup()
        {
            IsPopupOpen = true;
        }

        [RelayCommand]
        private void ClosePopup()
        {
            IsPopupOpen = false;
        }

        // Método para agregar una categoría
        public void AddCategory(string name, string parentCategory)
        {
            using var db = new ApplicationDbContext();
            var category = new Models.Category(name, parentCategory);
            db.Categories.Add(category);
            db.SaveChanges();
            CategoriesList.Add(category);
        }

        public string Name { get; set; } = "";
        public string ParentCategory { get; set; } = "";


        // Comando para añadar una categoría
        [RelayCommand]
        public void SaveCategory()
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                Debug.WriteLine("Please enter a category name.");
                return;
            }
            AddCategory(Name, ParentCategory);
            Name = string.Empty;
            ParentCategory = string.Empty;
        }

        // Comando para eliminar una categoría
        [RelayCommand]
        public void DeleteCategory(Models.Category category)
        {
            using var db = new ApplicationDbContext();
            db.Categories.Remove(category);
            db.SaveChanges();
            CategoriesList.Remove(category);



        }
    }
}
   
