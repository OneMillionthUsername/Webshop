namespace Webshop.Models
{
    public abstract class PaymentDetail
    {
        public int Id { get; set; }
        public int PaymentId { get; set; }
        public Payment? Payment { get; set; }
    }

    public class CreditCardPaymentDetail : PaymentDetail
    {
        public string CardBrand { get; set; } = string.Empty;
        public string Last4 { get; set; } = string.Empty;
        public int ExpiryMonth { get; set; }
        public int ExpiryYear { get; set; }
        public string Token { get; set; } = string.Empty;
    }

    public class EpsPaymentDetail : PaymentDetail
    {
        public string BankName { get; set; } = string.Empty;
        public string Reference { get; set; } = string.Empty;
        public string ProviderPaymentId { get; set; } = string.Empty;
    }

    public class PayPalPaymentDetail : PaymentDetail
    {
        public string PayerEmail { get; set; } = string.Empty;
        public string ProviderPaymentId { get; set; } = string.Empty;
    }
}
