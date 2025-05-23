using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AsComputoPOS.Services;

namespace AsComputoPOS.ViewModels.PointOfSale
{
    public partial class PointOfSaleViewModel: NavigationBarViewModel
    {
        public PointOfSaleViewModel(INavigationService navigation, IAuthenticationService authenticationService) : base(navigation, authenticationService) { }
    }
}
