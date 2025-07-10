namespace TamoPOS.Models
{
    // Product purchase represents each one of the items that are being purchased in a purchase order
    public class ProductPurchase
    {
        public int Id { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; } // references the product being resupplied

        public int PurchaseOrderId { get; set; }
        public PurchaseOrder PurchaseOrder { get; set; }

        public decimal UnitPrice { get; set; } // represents the purchase price from the business perspective
        public decimal Quantity { get; set; } // represents how many units were bougth in this batch
        public decimal Subtotal { get; set; } // represents the price before discounts, etc.
        public decimal Total { get; set; } // represents the price after discounts, etc.
        public decimal? FlatProfitMargin { get; set; }    // e.g. add $5 to the unit price
        public decimal? PercentProfitMargin { get; set; } // e.g. add 10% to the unit price
        public decimal SalePrice { get; set; } // represents the price of the product after adding the markup/profit marginS
        public decimal? QuantityRemaining { get; set; }
    }
}
