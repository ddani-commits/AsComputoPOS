using UiDesktopApp1.Models;

namespace UiDesktopApp1.Services
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
