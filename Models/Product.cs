namespace Webshop.Models
{
    public class Product
    {
		public int Id { get; set; }
		public string Name { get; set; } = string.Empty;
		public string Description { get; set; } = string.Empty;
		public decimal BasePrice { get; set; }

		// Foreign Key
		public int CategoryId { get; set; }
		public Category? Category { get; set; }

		// Navigation Property
		public List<ProductVariant> Variants { get; set; } = new();
	}
}
