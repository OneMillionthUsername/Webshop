namespace Webshop.Dtos.Orders
{
    public class CreateOrderDto
    {
        public string UserId { get; set; } = string.Empty;
        public List<CreateOrderItemDto> Items { get; set; } = new();
    }
}
