namespace Webshop.Dtos.Products
{
    public class ProductVariantDto
    {
        public int Id { get; set; }
        public string SKU { get; set; } = string.Empty;
        public string Attributes { get; set; } = string.Empty;
        public decimal PriceAdjustment { get; set; }
        public int StockQuantity { get; set; }
    }
}
