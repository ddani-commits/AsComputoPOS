using ClosedXML.Excel;
using System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Xml.Linq;
using TamoPOS.Controls;
using UiDesktopApp1.Data;
using UiDesktopApp1.Models;
using UiDesktopApp1.Services;
using Wpf.Ui;


namespace UiDesktopApp1.ViewModels.Pages
{
    public partial class SuppliersViewModel: ViewModel
    {
        [ObservableProperty]
        private Models.Supplier? selectedSupplier;
        [ObservableProperty]
        private string name = string.Empty;
        [ObservableProperty]
        private string contactName = string.Empty;
        [ObservableProperty]
        private string address = string.Empty;
        [ObservableProperty]
        private string email = string.Empty;
        [ObservableProperty]
        private string phone = string.Empty;
        public ObservableCollection<Models.Supplier> SuppliersList { get; } = new();
        private readonly IContentDialogService _contentDialogService;
        public SuppliersViewModel(IContentDialogService contentDialogService)
        {
            _contentDialogService = contentDialogService;
            LoadSuppliers();
        }
        public void LoadSuppliers()
        {
            using var db = new ApplicationDbContext();
            foreach (var supplier in db.Suppliers)
            {
                supplier.ViewModel = this; // Esto permite acceder al comando desde XAML
                SuppliersList.Add(supplier);

            }
        }

        [RelayCommand]
        private async Task ShowSignInContentDialog()
        {
            if(_contentDialogService.GetDialogHost() is not null)
            {
                var NewSupplierContentDialog = new NewSupplierContentDialog(_contentDialogService.GetDialogHost(), AddSupplier);
                _ = await NewSupplierContentDialog.ShowAsync();
            }
        }

        //Añadir
        [RelayCommand]
        public void AddSupplier(Supplier CurrentSupplier)
        {
            using (var context = new ApplicationDbContext())
            {
                context.Suppliers.Add(CurrentSupplier);
                context.SaveChanges();
                SuppliersList.Add(CurrentSupplier);
            }
        }

        // Guardar
        [RelayCommand]
        public void SaveSupplier()
        {
            using var db = new ApplicationDbContext();
            foreach (var supplier in SuppliersList)
            {
                db.Suppliers.Update(supplier);
            }
            db.SaveChanges();
            Debug.WriteLine("Saved from ViewModel");
        }

        // Eliminar
        [RelayCommand]
        public void DeleteSupplier(Models.Supplier supplier)
        {
            if (supplier is null) return;

            using var db = new ApplicationDbContext();
            var supplierToDelete = db.Suppliers.Find(supplier.SupplierId);

            if (supplierToDelete != null)
            {
                db.Suppliers.Remove(supplierToDelete);
                db.SaveChanges();
                SuppliersList.Remove(supplier);

                if (SelectedSupplier?.SupplierId == supplier.SupplierId)
                    SelectedSupplier = null;
            }
            else
            {
                Debug.WriteLine("Supplier not found in database.");
            }
        }

        [RelayCommand]
        private void ExportToExcel()
        {

            var dt = new DataTable("Suppliers");
            dt.Columns.Add("SupplierId", typeof(int));
            dt.Columns.Add("Name", typeof(string));
            dt.Columns.Add("ContactName", typeof(string));
            dt.Columns.Add("Address", typeof(string));
            dt.Columns.Add("Email", typeof(string));
            dt.Columns.Add("Phone", typeof(string));
            dt.Columns.Add("IsActive", typeof(bool));

            foreach (var supplier in SuppliersList)
            {
                dt.Rows.Add(supplier.SupplierId, supplier.Name, supplier.ContactName, supplier.Address, supplier.Email, supplier.Phone, supplier.IsActive);
            }


            string downloadsPath = Path.Combine(
                                   Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                                   "Downloads"
             );
            string filePath = Path.Combine(downloadsPath, "Suppliers.xlsx");


            using (var wb = new XLWorkbook())
            {
                var ws = wb.Worksheets.Add(dt, "Suppliers");
                ws.Columns().AdjustToContents();
                wb.SaveAs(filePath);
                Debug.WriteLine("Se descargó correctamente");
            }
        }

    }
}