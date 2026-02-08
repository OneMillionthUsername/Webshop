namespace Webshop.Dtos.Products
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal BasePrice { get; set; }
        public int CategoryId { get; set; }
        public List<ProductVariantDto> Variants { get; set; } = new();
    }
}
