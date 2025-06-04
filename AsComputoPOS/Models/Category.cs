using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UiDesktopApp1.ViewModels.Pages;
//using UiDesktopApp1.ViewModels.Category;

namespace UiDesktopApp1.Models
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string ParentCategoryName { get; set; }

      
        public Category( string categoryName, string parentCategoryName)
        {          
            CategoryName = categoryName;
            ParentCategoryName = parentCategoryName;
        }


        [NotMapped]
        public CategoryViewModel ViewModel { get; internal set; }

    }
}
