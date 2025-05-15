using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using AsComputoPOS.Services;
using AsComputoPOS.Models;
using AsComputoPOS.Data;

namespace AsComputoPOS.ViewModels
{
    public partial class EmployeesViewModel: NavigationBarViewModel
    {   
        public ObservableCollection<Employee> EmployeesList { get;  } = new();
        public EmployeesViewModel(INavigationService navigation) : base(navigation)
        {
            LoadEmployees();
        }

        private void LoadEmployees()
        {
            using var db = new ApplicationDbContext();
            foreach(var employee in db.Employees)
            {
                EmployeesList.Add(employee);
            }
        }
    }
}