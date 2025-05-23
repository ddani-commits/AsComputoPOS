using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using AsComputoPOS.Data;
using AsComputoPOS.Services;
using CommunityToolkit.Mvvm.Input;
namespace AsComputoPOS.ViewModels.Suppliers
{
    public partial class SuppliersViewModel : NavigationBarViewModel
    {
        public ObservableCollection<Models.Supplier> SuppliersList { get; } = new();
        public SuppliersViewModel(INavigationService navigation, IAuthenticationService authenticationService) : base(navigation, authenticationService) // Pass the navigation parameter to the base class constructor
        {
            LoadSuppliers();
        }
        public void LoadSuppliers()
        {
            using var db = new ApplicationDbContext();
            foreach (var supplier in db.Suppliers)
            {
                SuppliersList.Add(supplier);
            }
        }

        public string Name { get; set; } = "";
        public string ContactName { get; set; } = "";
        public string Address { get; set; } = "";
        public string Email { get; set; } = "";
        public string Phone { get; set; } = "";
        public bool IsActive { get; set; } = false;

        // Comando para añadir un proveedor
        public void AddSupplier(string name, string contactName, string address, string email, string phone, bool isActive)
        {
            using var db = new ApplicationDbContext();
            var supplier = new Models.Supplier(name, contactName, address, email, phone, isActive);
            db.Suppliers.Add(supplier);
            db.SaveChanges();
            SuppliersList.Add(supplier);
        }

        [RelayCommand]
        public void SaveSupplier()
        {
            if (string.IsNullOrWhiteSpace(Name) || string.IsNullOrWhiteSpace(ContactName) || string.IsNullOrWhiteSpace(Address) || string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Phone))
            {
                Debug.WriteLine("Please fill in all fields.");
                return;
            }
            AddSupplier(Name, ContactName, Address, Email, Phone, IsActive);
            Name = "";
            ContactName = "";
            Address = "";
            Email = "";
            Phone = "";
            IsActive = true;
            Debug.WriteLine("Supplier added successfully.");

        }

        // Comando para eliminar un proveedor
        

        [RelayCommand]
        public void DeleteSupplier(Models.Supplier supplier)
        {
            if (supplier == null)
            {
                Debug.WriteLine("Supplier not found.");
                return;
            }
            using var db = new ApplicationDbContext();
            db.Suppliers.Attach(supplier);
            db.Suppliers.Remove(supplier);
            db.SaveChanges();
            SuppliersList.Remove(supplier);
        }



    }
}
