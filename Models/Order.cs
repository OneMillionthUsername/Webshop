namespace Webshop.Models
{
    public class Order
    {
		public int Id { get; set; }
		public DateTime OrderDate { get; set; } = DateTime.UtcNow;
		public string UserId { get; set; } = string.Empty;
		public decimal TotalAmount { get; set; }

		// Foreign Key
		public int CustomerId { get; set; }
		public Customer? Customer { get; set; }

		// Navigation Properties
		public List<OrderItem> Items { get; set; } = new();
		public List<Payment> Payments { get; set; } = new();
		public List<Discount> Discounts { get; set; } = new();
	}
}
