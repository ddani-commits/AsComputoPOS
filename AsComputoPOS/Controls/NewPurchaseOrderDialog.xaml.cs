using System.Diagnostics;
using System.Windows.Controls;
using TamoPOS.Data;
using TamoPOS.Models;
using Wpf.Ui.Controls;

namespace TamoPOS.Controls
{
    public partial class NewPurchaseOrderDialog : ContentDialog
    {
        private readonly Action<PurchaseOrder>? _savePurchaseOrder;
        public List<string> SuppliersList = new();
        private readonly ApplicationDbContext _dbContext;

        public NewPurchaseOrderDialog(
            ApplicationDbContext dbContext,
            ContentPresenter? contentPresenter, 
            Action<PurchaseOrder>? savePurchaseOrder = null
          ) : base(contentPresenter)
        {
            InitializeComponent();
            _savePurchaseOrder = savePurchaseOrder;
            _dbContext = dbContext;
            DatePickerField.SelectedDate = DateTime.Now;
            DataContext = this;
        }

        protected override void OnButtonClick(ContentDialogButton button)
        {
            if (button == ContentDialogButton.Primary)
            {
                Supplier supplier = _dbContext.Suppliers.Where(s => s.Name == SupplierBox.Text).First();
                if (supplier == null)
                {
                    Debug.WriteLine("Supplier not found");
                    return;
                }
                Debug.WriteLine($"Selected Supplier: {supplier.Name}");

                var purchaseOrder = new PurchaseOrder(supplier, DatePickerField.SelectedDate.Value);
                _savePurchaseOrder?.Invoke(purchaseOrder);
                base.OnButtonClick(button);
                Debug.WriteLine("Primary button clicked");
            }
            else if (button == ContentDialogButton.Close)
            {
                base.OnButtonClick(button);
                Debug.WriteLine("Close button clicked");
            }
        }

        private void SupplierBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                var suppliers = _dbContext
                    .Suppliers
                    .Where(s => s.Name.Contains(sender.Text))
                    .ToList();

                foreach (Supplier supplier in suppliers)
                {
                    if (!SuppliersList.Contains(supplier.Name))
                    {
                        SuppliersList.Add(supplier.Name);
                    }
                }
                SupplierBox.OriginalItemsSource = SuppliersList;
            }
        }
        private void SelectedDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e) { }
    }
}
