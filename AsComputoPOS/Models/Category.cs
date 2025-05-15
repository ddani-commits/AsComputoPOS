using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsComputoPOS.Models
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public string ParentCategory { get; set; }

        public Category(int categoryId, string name, string parentCategory)
        {
            CategoryId = categoryId;
            Name = name;
            ParentCategory = parentCategory;
        }
    }
}
