using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using TamoPOS.ViewModels.Pages;

namespace TamoPOS.Models
{
    public class Supplier
    {
        public int SupplierId { get; set; }
        public string Name { get; set; }
        public string ContactName { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public bool _isActive { get; set; } = true;
        public bool IsActive
        {
            get => _isActive;
            set
            {
                if( _isActive != value )
                {
                    _isActive = value;
                  OnPropertyChanged(nameof(IsActive));
                }
            }
        }

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

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}