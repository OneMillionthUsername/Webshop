namespace Webshop.Models
{
    public class ProductVariant
    {
		public int Id { get; set; }
		public int ProductId { get; set; }
		public string SKU { get; set; } = string.Empty; // Lagerhaltungsnummer
		public ICollection<ProductVariantAttribute> Attributes { get; set; } = new List<ProductVariantAttribute>();
		public decimal PriceAdjustment { get; set; } // Aufpreis zur Basis
		public int StockQuantity { get; set; }
	}
}
