using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AsComputoPOS.Services;

namespace AsComputoPOS.ViewModels.Category
{
    public partial class CategoryViewModel(INavigationService navigation) : NavigationBarViewModel(navigation)
    {

        // Add properties and methods specific to CategoryViewModel here
        private string _categoryName;
        public string CategoryName
        {
            get => _categoryName;
            set
            {
                if (_categoryName != value)
                {
                    _categoryName = value;
                    OnPropertyChanged(nameof(CategoryName));
                }
            }
        }
        private string _parentCategory;
        public string ParentCategory
        {
            get => _parentCategory;
            set
            {
                if (_parentCategory != value)
                {
                    _parentCategory = value;
                    OnPropertyChanged(nameof(ParentCategory));
                }
            }
        }
        private int _categoryId;
        public int CategoryId
        {
            get => _categoryId;
            set
            {
                if (_categoryId != value)
                {
                    _categoryId = value;
                    OnPropertyChanged(nameof(CategoryId));
                }
            }
        }
        private List<string> _categories;
        public List<string> Categories
        {
            get => _categories;
            set
            {
                if (_categories != value)
                {
                    _categories = value;
                    OnPropertyChanged(nameof(Categories));
                }
            }
        }

        //Simulación de una base de datos
        public CategoryViewModel(): 
        {
            // Initialize the list of categories with some sample data
            Categories = new List<string>
            {
                "Electronics",
                "Clothing",
                "Books",
                "Home & Kitchen"
            };
        }
       
        public void AddCategory()
        {
            // Logic to add a new category
            // This could involve calling a service or updating a database
            // For now, we'll just simulate adding a category
            Categories.Add(CategoryName);
            CategoryName = string.Empty; // Clear the input after adding
        }
        public void DeleteCategory(string categoryName)
        {
            // Logic to delete a category
            // This could involve calling a service or updating a database
            // For now, we'll just simulate deleting a category
            Categories.Remove(categoryName);
        }
        public void EditCategory(string oldCategoryName, string newCategoryName)
        {
            // Logic to edit a category
            // This could involve calling a service or updating a database
            // For now, we'll just simulate editing a category
            int index = Categories.IndexOf(oldCategoryName);
            if (index != -1)
            {
                Categories[index] = newCategoryName;
            }
        }
    }

} 

   
