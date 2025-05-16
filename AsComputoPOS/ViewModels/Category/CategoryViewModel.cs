using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AsComputoPOS.Services;
using AsComputoPOS.Models;
using AsComputoPOS.Data;

namespace AsComputoPOS.ViewModels.Category
{
    public partial class CategoryViewModel : NavigationBarViewModel
    {
        public ObservableCollection<AsComputoPOS.Models.Category> CategoriesList { get; } = new();
        public CategoryViewModel(INavigationService navigation) : base(navigation)
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
    }
}
   
