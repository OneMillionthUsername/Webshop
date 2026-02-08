namespace Webshop.Dtos.Payments
{
    public class CreatePaymentDto
    {
        public int OrderId { get; set; }
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
    }
}
