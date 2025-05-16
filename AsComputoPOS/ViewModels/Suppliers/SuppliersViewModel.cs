using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AsComputoPOS.Data;
using AsComputoPOS.Services;
namespace AsComputoPOS.ViewModels.Suppliers
{
    public partial class SuppliersViewModel : NavigationBarViewModel
    {
        public ObservableCollection<Models.Supplier> SuppliersList { get; } = new();
        public SuppliersViewModel(INavigationService navigation) : base(navigation) // Pass the navigation parameter to the base class constructor
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
    }
}
