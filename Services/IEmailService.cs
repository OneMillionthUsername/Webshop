using Webshop.Models;

namespace Webshop.Services
{
    public interface IEmailService
    {
        Task SendOrderConfirmationAsync(Order order, Customer customer);
        Task SendOrderShippedAsync(Order order, Customer customer);
        Task SendOrderDeliveredAsync(Order order, Customer customer);
        Task SendPaymentConfirmationAsync(Order order, Customer customer, Payment payment);
        Task SendInvoiceAsync(Order order, Customer customer, string invoicePath);
        Task SendCancellationConfirmationAsync(Order order, Customer customer);
    }
}
