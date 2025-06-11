using UiDesktopApp1.Models;

namespace TamoPOS.Models
{
    // Represents the general term of purchase order, which is a request to buy products from a supplier
    public class PurchaseOrder 
    {
        public int Id { get; set; }
        public Supplier Supplier { get; set; }
        public DateTime PurchaseDate { get; set; }
        public Decimal Total {get; set;}
        public Decimal Subtotal {get; set;}
        public ICollection<ProductPurchase> ProductPurchases { get; set; }
    }

    // Product purchase represents each one of the items that are being purchased in a purchase order
    public class ProductPurchase {
        public Product Product {get; set;} // references the product being resupplied
        public Decimal UnitPrice {get; set;} // represents the purchase price from the business perspective
        public Decimal Quantity {get; set;} // represents how many units were bougth in this batch
        public Decimal Subtotal {get; set;} // represents the price before discounts, etc.
        public Decimal Total {get; set;} // represents the price after discounts, etc.
        public Decimal SalePrice {get; set;} // represents the price of the product after adding the markup/profit marginS
        public ICollection<StockBatch> StockBatches { get; set; } // there might be multiple batches for the same product purchase, each with its own stock left
    }

    // what in spanish would be called "lote"
    // allows to track stock of products within the same price, so the user can restock before the stock runs out,
    // and still keep the old pricing
    public class StockBatch {
        public int Id { get; set; }
        public Product Product { get; set; }
        public ProductPurchase ProductPurchase { get; set; }
        public decimal QuantityPurchased { get; set; }
        public decimal QuantityRemaining { get; set; }
        public DateTime ReceivedDate { get; set; }
    }
}