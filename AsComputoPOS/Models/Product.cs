using System.ComponentModel.DataAnnotations.Schema;

namespace TamoPOS.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        public string? Name { get; set; }
        public bool? IsActive { get; set; }
        public string? Barcode { get; set; }
        public Category? Category { get; set; }
        public int? CategoryId { get; set; }
        public string? SKU { get; set; }    
        public byte[]? ImageData { get; set; }
        public ICollection<ProductPurchase> ProductPurchase { get; set; }

        public Product() { }
        public Product(string productName, bool isActive, string barcode, string SKU, byte[]? imageData)
        {
            Name = productName;
            IsActive = isActive;
            Barcode = barcode;
            ImageData = imageData;
            //Category = category;
            this.SKU = SKU;
        }

        public override string ToString()
        {
            return Name;
        }
        [NotMapped]
        public (decimal TotalStockQuantity, decimal TotalStockAvalable) TotalStockResumen
        {
            get
            {
                decimal TotalStockQuantity = ProductPurchase?.Sum(pp => pp.Quantity) ?? 0;
                decimal totalStockAvalable = ProductPurchase?.Sum(pp => pp.QuantityRemaining) ?? 0;
                return (TotalStockQuantity, totalStockAvalable);
            }
        }

    }
}
