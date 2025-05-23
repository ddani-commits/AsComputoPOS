using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AsComputoPOS.Models;

namespace AsComputoPOS.Services
{
    public interface IAuthenticationService
    {
        //event EventHandler AuthenticationStateChanged;
        bool? IsAuthenticated { get; }
        Employee? CurrentEmployee { get; }
        bool Login(string email, string password);
        void Register(string firstName, string lastName, string email);
        bool HasUsers();
        void Logout();
        event EventHandler? AuthenticationStateChanged;
    }
}
