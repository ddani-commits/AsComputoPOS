using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AsComputoPOS.Models;

namespace AsComputoPOS.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        public bool? IsAuthenticated { get; set; }
        public Employee? CurrentEmployee { get; set; }
        public void Login()
        {
            IsAuthenticated = true;
            CurrentEmployee = new Employee(1, "John", "Doe", "myemail@gmail.com");
        }
        public void Register(int id, string firstName, string lastName, string email)
        {
            IsAuthenticated = true;
            CurrentEmployee = new Employee(id, firstName, lastName, email);
        }
        public void Logout()
        {
            IsAuthenticated = false;
            CurrentEmployee = null;
        }
    }
}
