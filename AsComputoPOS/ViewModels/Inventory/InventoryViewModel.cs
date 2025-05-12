using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AsComputoPOS.Services;

namespace AsComputoPOS.ViewModels.Inventory
{
    public partial class InventoryViewModel: NavigationBarViewModel
    {
        public InventoryViewModel(INavigationService navigation) : base(navigation) { }
    }
}
