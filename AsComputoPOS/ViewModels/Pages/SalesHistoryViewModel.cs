using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text.Json;
using TamoPOS.Data;
using TamoPOS.Models;

namespace TamoPOS.ViewModels.Pages
{
    public partial class SalesHistoryViewModel: ViewModel
    {
        private ApplicationDbContext _applicationDbContext { get; set; } = new ApplicationDbContext();
        public ObservableCollection<Ticket> Sales { get; } = new();

        public JsonSerializerOptions options = new JsonSerializerOptions
        {
            WriteIndented = true, // makes output readable
            ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles // avoids circular references
        };

        public SalesHistoryViewModel()
        {
            LoadSalesHistoryAsync();
        }

        [RelayCommand]
        public void PrintDetails(int ticketId)
        {
            var cartItems = _applicationDbContext.CartItems.Select(c => new { c.TicketId, c.ProductId, c.Quantity}).Where(c => c.TicketId == ticketId);
            var json = JsonSerializer.Serialize(cartItems, options);
            Debug.WriteLine(json);
        }

        public void LoadSalesHistoryAsync()
        {
            Sales.Clear();
            var sales = _applicationDbContext.Tickets
                    .Include(t => t.Products)
                    //.ThenInclude(ci => ci.Product)
                    .Include(t => t.Employee)
                    .ToList();
            foreach (var sale in sales)
            {
                Debug.WriteLine(sale.Products.Count);
                Debug.WriteLine(sale.Employee.FirstName);
                sale.ProductsCount = sale.Products?.Count ?? 0;
                Sales.Add(sale);
            }


            //var json = JsonSerializer.Serialize(Sales, options);
            //Debug.WriteLine(json);
        }
    }
}
