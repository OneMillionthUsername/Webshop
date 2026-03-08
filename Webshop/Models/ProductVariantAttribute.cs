namespace Webshop.Models
{
    public class ProductVariantAttribute
    {
        public int Id { get; set; }
        public int ProductVariantId { get; set; }
        public string AttributeName { get; set; } = string.Empty; // z.B. "Color", "Size", "Material"
        public string AttributeValue { get; set; } = string.Empty; // z.B. "Red", "XL", "Cotton"

        // Navigation Property
        public ProductVariant ProductVariant { get; set; } = null!;
    }
}
