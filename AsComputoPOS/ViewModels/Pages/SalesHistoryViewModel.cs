using System.Collections.ObjectModel;
using TamoPOS.Data;
using TamoPOS.Models;

namespace TamoPOS.ViewModels.Pages
{
    public partial class SalesHistoryViewModel: ViewModel
    {
        private ApplicationDbContext _applicationDbContext { get; set; } = new ApplicationDbContext();
        public ObservableCollection<Ticket> Sales { get; } = new();
        
        public SalesHistoryViewModel()
        {
            LoadSalesHistoryAsync();
        }

        public void LoadSalesHistoryAsync()
        {
            var sales = _applicationDbContext.Tickets.ToList();
            foreach (var sale in sales)
            {
                Sales.Add(sale);
            }
        }
    }
}
