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

        public Category( string name, string parentCategory)
        {          
            Name = name;
            ParentCategory = parentCategory;
        }
    }
}
