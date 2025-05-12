using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AsComputoPOS.Services;
namespace AsComputoPOS.ViewModels.Suppliers
{
    public partial class SuppliersViewModel : NavigationBarViewModel
    {
        public SuppliersViewModel(INavigationService navigation) : base(navigation) // Pass the navigation parameter to the base class constructor
        {
        }
    }
}
