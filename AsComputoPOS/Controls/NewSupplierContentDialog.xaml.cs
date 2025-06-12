using DocumentFormat.OpenXml.EMMA;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using TamoPOS.Models;
using Wpf.Ui.Controls;

namespace TamoPOS.Controls
{
    /// <summary>
    /// Lógica de interacción para NewSupplierContentDialog.xaml
    /// </summary>
    public partial class NewSupplierContentDialog : ContentDialog
    {
        private string _name = string.Empty;
        private string _contactName = string.Empty;
        private string _address = string.Empty;
        private string _email = string.Empty;
        private string _phone = string.Empty;
        

        public string NameText
        {
            get => _name;
            set { _name = value; OnPropertyChanged(); }
        }
        public string ContactNameText
        {
            get => _contactName;
            set { _contactName = value; OnPropertyChanged(); }
        }
        public string AddressText
        {
            get => _address;
            set { _address = value; OnPropertyChanged(); }
        }
        public string EmailText
        {
            get => _email;
            set { _email = value; OnPropertyChanged(); }
        }
        public string PhoneText
        {
            get => _phone;
            set { _phone = value; OnPropertyChanged(); }
        }

        private readonly Action<Supplier>? _saveSupplier;

        public NewSupplierContentDialog(ContentPresenter? contentPresenter, Action<Supplier>? saveSupplier = null) : base(contentPresenter)
        {
            InitializeComponent();
            _saveSupplier = saveSupplier;
            DataContext = this;

        }

        protected override void OnButtonClick(ContentDialogButton button)
        {
            if (button == ContentDialogButton.Primary)
            {
                var supplier = new Supplier(NameText, ContactNameText, AddressText, EmailText, PhoneText);
                _saveSupplier?.Invoke(supplier);
                base.OnButtonClick(button);
                Debug.WriteLine("Supplier saved from ContentDialog");
            }
            else if (button == ContentDialogButton.Close)
            {
                // Handle the close button click if needed
                base.OnButtonClick(button);
            }

        }
        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = null!)
        {
                       PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
      
    }
}
