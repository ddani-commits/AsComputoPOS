using System.ComponentModel;

namespace TamoPOS.Models
{
    public class ProductStockDTO : INotifyPropertyChanged
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public decimal SalePrice { get; set; }
        public Category? Category { get; set; }
        public int? CategoryId { get; set; }
        public decimal QuantityRemaining { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}