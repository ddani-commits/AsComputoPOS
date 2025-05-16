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
        public bool IsActive { get; set; } = true;
        //public Role Role {get; set;}  

        public Employee (string firstName, string lastName, string email)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
        }
    }
}