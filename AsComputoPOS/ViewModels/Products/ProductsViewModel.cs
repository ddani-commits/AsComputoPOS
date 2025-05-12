using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AsComputoPOS.Services;

namespace AsComputoPOS.ViewModels.Products
{
    public partial class ProductsViewModel: NavigationBarViewModel
    {
        public ProductsViewModel(INavigationService navigation) : base(navigation) { }
    }
}
