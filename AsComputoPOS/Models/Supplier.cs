using System.ComponentModel.DataAnnotations.Schema;
using UiDesktopApp1.ViewModels.Pages;

namespace UiDesktopApp1.Models
{
    public class Supplier
    {
        public int SupplierId { get; set; }
        public string Name { get; set; }
        public string ContactName { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public bool IsActive { get; set; } = true;

        public Supplier(string name, string contactName, string address, string email, string phone )
        {
            Name = name;
            ContactName = contactName;
            Address = address;
            Email = email;
            Phone = phone;
            
        }
        [NotMapped]
        public SuppliersViewModel ViewModel { get; internal set; }
    }
}