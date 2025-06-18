namespace TamoPOS.Models
{
    // Represents the general term of purchase order, which is a request to buy products from a supplier
    public class PurchaseOrder 
    {
        public int Id { get; set; }
        public int SupplierId { get; set; }
        public Supplier Supplier { get; set; }
        public DateTime PurchaseDate { get; set; }
        public decimal? Total {get; set;}
        public decimal? Subtotal {get; set;}
        public ICollection<ProductPurchase>? ProductPurchases { get; set; }
        public PurchaseOrder() { }
        public PurchaseOrder(Supplier supplier, DateTime purchaseDate) 
        {
            Supplier = supplier;
            PurchaseDate = purchaseDate;
        }
    }

    // Product purchase represents each one of the items that are being purchased in a purchase order
    public class ProductPurchase {
        public int Id { get; set; }

        public int ProductId { get; set; }
        public Product Product {get; set;} // references the product being resupplied
        
        public int PurchaseOrderId { get; set; }
        public PurchaseOrder PurchaseOrder { get; set; }
        
        public decimal UnitPrice {get; set;} // represents the purchase price from the business perspective
        public decimal Quantity {get; set;} // represents how many units were bougth in this batch
        public decimal Subtotal {get; set;} // represents the price before discounts, etc.
        public decimal Total {get; set;} // represents the price after discounts, etc.
        public decimal? FlatProfitMargin { get; set; }    // e.g. add $5 to the unit price
        public decimal? PercentProfitMargin { get; set; } // e.g. add 10% to the unit price
        public decimal SalePrice {get; set;} // represents the price of the product after adding the markup/profit marginS
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