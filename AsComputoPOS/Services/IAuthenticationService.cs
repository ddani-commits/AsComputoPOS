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
        void Login();
        void Register(int id, string firstName, string lastName, string email);
        void Logout();
    }
}
