using TamoPOS.Models;
using Microsoft.EntityFrameworkCore;

namespace TamoPOS.Data
{
    public class ApplicationDbContext: DbContext
    {
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<PurchaseOrder> PurchaseOrders { get; set; }
        public DbSet<ProductPurchase> ProductPurchases { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<CartItem> CartItems { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=app.db"); 
        }
    }
}
