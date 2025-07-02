namespace TamoPOS.Models
{
    // Represents the general term of purchase order, which is a request to buy products from a supplier
    public class PurchaseOrder
    {
        public int Id { get; set; }
        public int SupplierId { get; set; }
        public Supplier Supplier { get; set; }
        public DateTime PurchaseDate { get; set; }
        public decimal? Total { get; set; }
        public decimal? Subtotal { get; set; }
        public ICollection<ProductPurchase>? ProductPurchases { get; set; }
        public PurchaseOrder() { }
        public PurchaseOrder(Supplier supplier, DateTime purchaseDate)
        {
            Supplier = supplier;
            PurchaseDate = purchaseDate;
        }
    }
}