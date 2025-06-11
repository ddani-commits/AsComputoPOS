using Microsoft.AspNetCore.Identity;

namespace UiDesktopApp1.Models
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
        
        public string PasswordHash { get; set; } = string.Empty;

        public Employee (string firstName, string lastName, string email)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
        }

        public void SetPassword(string password)
        {
            var paswordHasher = new PasswordHasher<Employee>();
            PasswordHash = paswordHasher.HashPassword(this, password);
        }

        public bool VerifyPassword(string password)
        {
            var passwordHasher = new PasswordHasher<Employee>();
            var result = passwordHasher.VerifyHashedPassword(this, PasswordHash, password);
            return result == PasswordVerificationResult.Success;
        }
    }
}