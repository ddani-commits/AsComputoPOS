using Microsoft.EntityFrameworkCore;
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
            Sales.Clear();
            var sales = _applicationDbContext.Tickets
                    .Include(t => t.Products)
                    .ThenInclude(ci => ci.Product)
                    .Include(t => t.Employee)
                    .ToList();
            foreach (var sale in sales)
            {
                sale.ProductsCount = sale.Products?.Count ?? 0;
                Sales.Add(sale);
            }
        }
    }
}
