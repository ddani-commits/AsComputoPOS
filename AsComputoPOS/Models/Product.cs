using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace TamoPOS.Models
{
    public class Product : INotifyPropertyChanged
    {
        public int ProductId { get; set; }
        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged(nameof(Name));
                }
            }
        }
        public bool? IsActive { get; set; }
        public string? Barcode { get; set; }
        private Category _category;
        private int _categoryId;
        public int CategoryId 
        {
            get => _categoryId;
            set
            {
                if (_categoryId != value)
                {
                    _categoryId = value;
                    OnPropertyChanged(nameof(CategoryId));
                    OnPropertyChanged(nameof(Category));
                }
            }
        }
        public Category Category
        {
            get => _category;
            set
            {
                if (_category != value)
                {
                    _category = value;
                    OnPropertyChanged(nameof(Category));
                }
            }
        }
        public string? SKU { get; set; }    
        public byte[]? ImageData { get; set; }
        public ICollection<ProductPurchase> ProductPurchase { get; set; }
        public Product() { }
        public Product(string productName, bool isActive, string barcode, string SKU, byte[]? imageData, Category category)
        {
            Name = productName;
            IsActive = isActive;
            Barcode = barcode;
            ImageData = imageData;
            Category = category;
            this.SKU = SKU;
        }

        public override string ToString()
        {
            return Name;
        }
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
