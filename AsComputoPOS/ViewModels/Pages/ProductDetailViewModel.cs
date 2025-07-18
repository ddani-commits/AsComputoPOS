using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using TamoPOS.Data;
using TamoPOS.Models;
using TamoPOS.Services;

namespace TamoPOS.ViewModels.Pages
{
    public partial class ProductDetailViewModel : ViewModel
    {
        private ApplicationDbContext? _applicationDbContext;
        public ObservableCollection<Product> Products { get; } = new();
        public ObservableCollection<ProductPurchase> ProductPurchases { get; } = new();
        public ObservableCollection<ProductPurchase> ProductsInStock { get; set; } = new();
        public ObservableCollection<Ticket> Sales { get; } = new();
        public ObservableCollection<Category>? _categoryList;
        public ObservableCollection<Category>? CategoryList
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

        private IServiceProvider _serviceProvider;
        public ProductDetailViewModel(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public void LoadProductDetails(int productId, ApplicationDbContext appDbContext)
        {
            _applicationDbContext = appDbContext;
            CurrentProduct = _applicationDbContext.Products
                .Include(p => p.Category)
                .Include(p => p.ProductPurchase)
                .Single(p => p.ProductId == productId);
            
                var totalRemaining = CurrentProduct.ProductPurchase
                    .Where(pp => pp.QuantityRemaining >= 0)
                    .Sum(pp => pp.QuantityRemaining ?? 0);
                IdText = CurrentProduct.ProductId.ToString();
                Name = CurrentProduct.Name;
                SelectedCategory = CurrentProduct.Category;
                SalePrice = CurrentProduct.ProductPurchase?.FirstOrDefault()?.SalePrice.ToString("C") ?? "0.00";
                QuantityRemaining = totalRemaining.ToString();
                IsActive = CurrentProduct.IsActive;
            Debug.WriteLine(CurrentProduct.Name + CurrentProduct.Category);
            
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
            }
        }
        public void UpdateProductDetails()
        {

            if (SelectedCategory != null && CurrentProduct != null)
            {
                CurrentProduct.CategoryId = SelectedCategory.CategoryId;
                CurrentProduct.Category = SelectedCategory;
            }

            if (!string.IsNullOrWhiteSpace(Name) && CurrentProduct != null)
            {
                CurrentProduct.Name = Name;
            }
            if (CurrentProduct != null)
            {
                _applicationDbContext.Products.Update(CurrentProduct);
                _applicationDbContext.SaveChanges();
                OnPropertyChanged(nameof(CurrentProduct));
                Debug.WriteLine("Producto actualizado correctamente.");
            }

            ProductsViewModel productsViewModel = _serviceProvider.GetRequiredService<ProductsViewModel>();
            productsViewModel.LoadAllProducts();
            IPOSService posService = _serviceProvider.GetRequiredService<IPOSService>();
            posService.LoadProductsInStock();
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

        public void LoadSalesHistoryAsync(int productId)
        {
            Sales.Clear();

            // Filtrar los tickets que contengan al menos un producto con el productId especificado
            var sales = _applicationDbContext.Tickets
                .Include(t => t.Products)
                    .ThenInclude(ci => ci.Product)
                .Include(t => t.Employee)
                .Where(t => t.Products.Any(p => p.ProductId == productId))  // Filtro por ProductId
                .ToList();

            foreach (var sale in sales)
            {
                sale.ProductsCount = sale.Products?.Count ?? 0;
                Sales.Add(sale);
            }
        }


        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
