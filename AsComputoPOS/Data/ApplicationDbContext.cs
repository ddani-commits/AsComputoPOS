using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AsComputoPOS.Models;
using Microsoft.EntityFrameworkCore;

namespace AsComputoPOS.Data
{
    public class ApplicationDbContext: DbContext
    {
        public DbSet<Employee> Employees { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=app.db"); 
        }
    }
}
