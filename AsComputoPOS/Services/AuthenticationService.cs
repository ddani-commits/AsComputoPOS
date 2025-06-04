using UiDesktopApp1.Data;
using UiDesktopApp1.Models;

namespace UiDesktopApp1.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        public event EventHandler? AuthenticationStateChanged;
        public bool? IsAuthenticated { get; set; }
        public Employee? CurrentEmployee { get; set; }
        private void OnAuthenticationStateChanged()
        {
            AuthenticationStateChanged?.Invoke(this, EventArgs.Empty);
        }
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
                OnAuthenticationStateChanged();
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
            OnAuthenticationStateChanged();
        }
        public void Logout()
        {
            IsAuthenticated = false;
            CurrentEmployee = null;
            OnAuthenticationStateChanged();
        }
    }
}
