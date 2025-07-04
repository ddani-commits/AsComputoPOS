using System.Collections.ObjectModel;
using TamoPOS.Models;

namespace TamoPOS.ViewModels.Pages
{
    public partial class SalesHistoryViewModel: ViewModel
    {
        public ObservableCollection<Ticket> Sales { get; } = new();
    }
}
