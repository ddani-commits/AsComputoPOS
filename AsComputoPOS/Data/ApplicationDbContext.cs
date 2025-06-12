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

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=app.db"); 
        }
    }
}
