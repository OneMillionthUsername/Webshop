namespace Webshop.Models
{
    public class Discount
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal DiscountAmount { get; set; }
        public decimal DiscountPercentage { get; set; }
        public string Type { get; set; } = "Percentage"; // Percentage or Fixed
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public string Reason { get; set; } = string.Empty; // z.B. "Bulk Order", "Loyalty", "Promotion"

        // Navigation Property
        public Order? Order { get; set; }
    }
}
