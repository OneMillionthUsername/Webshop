namespace Webshop.Models
{
    public class OrderItem
    {
		public int Id { get; set; }
		public int ProductVariantId { get; set; }
		public int Quantity { get; set; }
		public decimal PriceAtPurchase { get; set; } // Snapshot!

		// Foreign Key
		public int OrderId { get; set; }
		public Order? Order { get; set; }

		// Navigation Property
		public ProductVariant? ProductVariant { get; set; }
	}
}
