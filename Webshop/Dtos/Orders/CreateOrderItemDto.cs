namespace Webshop.Dtos.Orders
{
    public class CreateOrderItemDto
    {
        public int ProductVariantId { get; set; }
        public int Quantity { get; set; }
    }
}
