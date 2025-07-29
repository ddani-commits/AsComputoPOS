using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TamoPOS.ViewModels.Pages;
//using TamoPOS.ViewModels.Category;

namespace TamoPOS.Models
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public int? ParentCategoryId { get; set; }
        public Category? ParentCategory { get; set; }
        public override string ToString()
        {
            return CategoryName;
        }

        [NotMapped]
        public string? ParentCategoryName
        {
            get
            {
                return ParentCategory != null ? ParentCategory.CategoryName : null;
            }
        }
    }
}
