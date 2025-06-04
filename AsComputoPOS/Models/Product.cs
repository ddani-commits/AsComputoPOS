namespace UiDesktopApp1.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public bool IsActive { get; set; }
        public string Barcode { get; set; }
        //public Category Category { get; set; }
        public string SKU { get; set; }

        public Product(string productName, bool isActive, string barcode, string SKU)
        {
            ProductName = productName;
            IsActive = isActive;
            Barcode = barcode;
            //Category = category;
            this.SKU = SKU;
        }
    }
}
