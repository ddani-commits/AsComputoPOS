using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AsComputoPOS.Data;
using AsComputoPOS.Models;

namespace AsComputoPOS.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        public bool? IsAuthenticated { get; set; }
        public Employee? CurrentEmployee { get; set; }
        public bool HasUsers()
        {
            using (var context = new ApplicationDbContext())
            {
                return context.Employees.Any();
            }
        }
        public bool Login(string email, string password)
        {
            IsAuthenticated = true;
            using (var context = new ApplicationDbContext())
            {
                CurrentEmployee = context.Employees.FirstOrDefault(employee => employee.Email == email);
                return CurrentEmployee != null;
            }
        }
        public void Register(string firstName, string lastName, string email)
        {
            using (var context = new ApplicationDbContext())
            {
                CurrentEmployee = new Employee(firstName, lastName, email);
                context.Employees.Add(CurrentEmployee);
                context.SaveChanges();
            }
            IsAuthenticated = true;
        }
        public void Logout()
        {
            IsAuthenticated = false;
            CurrentEmployee = null;
        }
    }
}
