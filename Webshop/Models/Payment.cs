namespace Webshop.Models
{
    public class Payment
    {
        public int Id { get; set; }

        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; } = string.Empty; // "Stripe", "PayPal"
        public string Status { get; set; } = "Pending"; // Pending, Completed, Failed, Refunded
        public string TransactionId { get; set; } = string.Empty;
        public DateTime PaymentDate { get; set; } = DateTime.UtcNow;
        public string? ErrorMessage { get; set; }

        // FK
		public int OrderId { get; set; } 

		// Navigation properties
		public Order? Order { get; set; } 
        public PaymentDetail? Details { get; set; }
    }
}
