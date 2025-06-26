using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TamoPOS.ViewModels.Controls
{
    public partial class CheckoutPanelViewModel: ViewModel
    {
        public List<string> Colors { get; } = new() { "Efectivo", "Debito/Credito" };
    }
}
