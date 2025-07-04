using DocumentFormat.OpenXml.Vml;
using Microsoft.Win32;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using TamoPOS.Data;
using TamoPOS.Models;
using Wpf.Ui.Controls;

namespace TamoPOS.Controls
{
    public partial class NewProductContentDialog : ContentDialog
    {
        private string _productName = string.Empty;
        public string ProductName
        {
            get => _productName;
            set { _productName = value; OnPropertyChanged(); }
        }

        private bool _isActive = true;
        public bool IsActive
        {
            get => _isActive;
            set { _isActive = value; OnPropertyChanged(); }
        }

        private string _barcode = string.Empty;
        public string Barcode
        {
            get => _barcode;
            set { _barcode = value; OnPropertyChanged(); }
        }

        private string _SKU = string.Empty;
        public string SKU
        {
            get => _SKU;
            set { _SKU = value; OnPropertyChanged(); }
        }
        public Category SelectedCategory;

        private string _imagePath = string.Empty;
        public byte[]? ImageBytes;
        private readonly Action<Product>? _createProduct;
        public List<string> CategoryList = new();
        private Category? _selectedCategory;

        public NewProductContentDialog(
            ApplicationDbContext dbContext, 
            ContentPresenter? contentPresenter, 
            Action<Product>? createProduct = null
        ) : base(contentPresenter)
        {
            _createProduct = createProduct;
            DataContext = this;
            Title = "Crear un producto";
            InitializeComponent();
        }

        public void OnOpenPicture()
        {
            //OpenedPicturePathVisibility = Visibility.Collapsed;
            OpenFileDialog openFileDialog = new()
            {
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures),
                Filter = "Image files (*.bmp;*.jpg;*.jpeg;*.png)|*.bmp;*.jpg;*.jpeg;*.png|All files (*.*)|*.*",
            };

            if (openFileDialog.ShowDialog() != true)
            {
                return;
            }

            //if (!File.Exists(openFileDialog.FileName))
            //{
            //    return;
            //}

            //OpenedPicturePath = openFileDialog.FileName;
            //OpenedPicturePathVisibility = Visibility.Visible;
        }

        protected override void OnButtonClick(ContentDialogButton button)
        {
            if (button == ContentDialogButton.Primary)
            {
                var product = new Product()
                {
                    Name = ProductName,
                    IsActive = IsActive,
                    Barcode = Barcode,
                    Category = SelectedCategory,
                    SKU = SKU,
                    ImageData = ImageBytes,
                };
                _createProduct?.Invoke(product);
                base.OnButtonClick(button);
                Debug.WriteLine("primary button clicked");
            }
            else if (button == ContentDialogButton.Secondary)
            {
                Debug.WriteLine("Secondary button clicked");
            }
            else if (button == ContentDialogButton.Close)
            {
                // Close dialog without saving
                base.OnButtonClick(button);
                Debug.WriteLine("Cancel button clicked");
            }
        }
        
        public event PropertyChangedEventHandler? PropertyChanged;

        // Notify property changes for data binding
        private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
