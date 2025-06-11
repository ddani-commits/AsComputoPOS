using UiDesktopApp1.Controls;
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
            IsAuthenticated = false;
            using (var context = new ApplicationDbContext())
            {
                CurrentEmployee = context.Employees.FirstOrDefault(employee => employee.Email == email);

                if (CurrentEmployee != null && CurrentEmployee.VerifyPassword(password)) // Verificar el hash de la contraseña
                {
                    IsAuthenticated = true;
                    OnAuthenticationStateChanged();
                    return true;
                }
            }

            OnAuthenticationStateChanged();
            return false;
        }

        public void Register(string firstName, string lastName, string email, string password)
        {
            using (var context = new ApplicationDbContext())
            {
                CurrentEmployee = new Employee(firstName, lastName, email);
                CurrentEmployee.SetPassword(password);
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
