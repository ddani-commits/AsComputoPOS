using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AsComputoPOS.Services;

namespace AsComputoPOS.ViewModels.SalesHistory
{
    public partial class SalesHistoryViewModel: NavigationBarViewModel
    {
        public SalesHistoryViewModel(INavigationService navigation, IAuthenticationService authenticationService) : base(navigation, authenticationService) { }
    }
}
