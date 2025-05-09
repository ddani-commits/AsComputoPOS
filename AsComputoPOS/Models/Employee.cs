using System;
using System.Collections.Generic;

namespace AsComputoPOS.Models
{
    // EF Base model for Employee
    public class Employee
    {

        public int EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        //public Role Role {get; set;}  

        public Employee (int employeeId, string firstName, string lastName, string email)
        {
            EmployeeId = employeeId;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
        }

    }
}
