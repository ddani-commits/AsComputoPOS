using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsComputoPOS.ViewModels
{
    public class AddEmployeesViewModel: ViewModelBase
    {
    public NavigationBarViewModel Navbar { get; }
        public AddEmployeesViewModel(NavigationBarViewModel navbar)
        {
            Navbar = navbar;
        }
    }
}
