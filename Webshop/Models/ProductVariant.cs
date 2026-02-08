namespace Webshop.Models
{
    public class ProductVariant
    {
		public int Id { get; set; }
		public int ProductId { get; set; }
		public string SKU { get; set; } = string.Empty; // Lagerhaltungsnummer
		public string Attributes { get; set; } // z.B. JSON "{"Color": "Red", "Size": "XL"}"
		public decimal PriceAdjustment { get; set; } // Aufpreis zur Basis
		public int StockQuantity { get; set; }
	}
}
