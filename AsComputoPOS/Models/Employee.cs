
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
namespace TamoPOS.Models
{
    // EF Base model for Employee
    public class Employee
    {
        public int EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; } = true;

        [NotMapped]
        public string FullName => $"{FirstName} {LastName}";
        public string PasswordHash { get; set; } = string.Empty;
        public bool IsAdmin { get; set; } = false;

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