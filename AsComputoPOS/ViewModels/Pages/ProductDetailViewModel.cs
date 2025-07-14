using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using TamoPOS.Data;
using TamoPOS.Models;
using Wpf.Ui.Controls;

namespace TamoPOS.ViewModels.Pages
{
    public partial class ProductDetailViewModel : ViewModel
    {
        private ApplicationDbContext _applicationDbContext = new ApplicationDbContext();
        public ObservableCollection<Product> Products { get; } = new();
        public ObservableCollection<ProductPurchase> ProductPurchases { get; } = new();
        public ObservableCollection<ProductPurchase> ProductsInStock { get; set; } = new();
        public ObservableCollection<Category> _categoryList;
        public ObservableCollection<Category> CategoryList 
        { 
            get => _categoryList ??= new ObservableCollection<Category>(_applicationDbContext.Categories.ToList());
            set
            {
                if (_categoryList != value)
                {
                    _categoryList = value;
                    OnPropertyChanged(nameof(CategoryList));
                }
            }
        }
        [ObservableProperty]
        private Category? _selectedCategory;
        public byte[]? ImageBytes;
        [ObservableProperty]
        private string? _idText;
        [ObservableProperty]
        private Product? _currentProduct;
        [ObservableProperty]
        private string? _salePrice;
        [ObservableProperty]
        private string? _quantityRemaining;
        [ObservableProperty]
        private string? _name;
        [ObservableProperty]
        private bool? _isActive;
        [ObservableProperty]
        private string? _currentPurchaseOrder;
        public ProductDetailViewModel() { }
        [RelayCommand]
        public void LoadProductDetails(int productId)
        {
            var product = _applicationDbContext.Products
                .Include(p => p.Category)
                .Include(p => p.ProductPurchase)
                .Single(p => p.ProductId == productId);
            CurrentProduct = product;
            if (product != null)
            {
                SelectedCategory = null;
                var totalRemaining = product.ProductPurchase
                    .Where(pp => pp.QuantityRemaining >= 0)
                    .Sum(pp => pp.QuantityRemaining ?? 0);
                IdText = product.ProductId.ToString();
                Name = product.Name;
                SalePrice = product.ProductPurchase?.FirstOrDefault()?.SalePrice.ToString("C") ?? "0.00";
                QuantityRemaining = totalRemaining.ToString();
                IsActive = product.IsActive;
            }
        }
        [RelayCommand]
        public void LoadProductPurchases()
        {
            if (int.TryParse(CurrentPurchaseOrder, out int purchaseOrderId))
            {
                ProductPurchases.Clear();
                _applicationDbContext.ProductPurchases
                    .Include(pp => pp.Product)
                    .Where(pp => pp.PurchaseOrderId == purchaseOrderId)
                    .ToList()
                    .ForEach(pp => ProductPurchases.Add(pp));
                foreach (ProductPurchase pr in ProductPurchases)
                {
                    Debug.WriteLine(pr.Id);
                }
            }
        }
        public void UpdateProductCategory()
        {
            if (SelectedCategory != null && CurrentProduct != null)
            {
                CurrentProduct.CategoryId = SelectedCategory.CategoryId;
                CurrentProduct.Category = SelectedCategory;
                _applicationDbContext.Products.Update(CurrentProduct);
                _applicationDbContext.SaveChanges();
            }
        }

        [RelayCommand]
        public void SaveProductName()
        {
            if (CurrentProduct != null && !string.IsNullOrWhiteSpace(Name))
            {
                CurrentProduct.Name = Name;
                _applicationDbContext.Products.Update(CurrentProduct);
                _applicationDbContext.SaveChanges();
                Application.Current.Dispatcher.Invoke(() =>
                {
                    OnPropertyChanged(nameof(CurrentProduct));
                    OnPropertyChanged(nameof(Name));
                });
               
                Debug.WriteLine($"Se cambió el nombre correctamente a {CurrentProduct.Name}");
            }
        }

        public void OnOpenPicture()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures),
                Filter = "Image files (*.bmp;*.jpg;*.jpeg;*.png)|*.bmp;*.jpg;*.jpeg;*.png|All files (*.*)|*.*",
            };

            if (openFileDialog.ShowDialog() == true)
            {
                try
                {

                    byte[] imageBytes = File.ReadAllBytes(openFileDialog.FileName);
                    ImageBytes = imageBytes;


                    if (CurrentProduct != null)
                    {
                        CurrentProduct.ImageData = imageBytes;
                        _applicationDbContext.Products.Update(CurrentProduct);
                        _applicationDbContext.SaveChanges();
                        OnPropertyChanged(nameof(CurrentProduct));
                    }

                    Debug.WriteLine("Image read and updated successfully.");
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error reading image file: {ex.Message}");
                    ImageBytes = null;
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
