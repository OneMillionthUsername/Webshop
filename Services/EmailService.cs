using Webshop.Models;

namespace Webshop.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IConfiguration configuration, ILogger<EmailService> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public async Task SendOrderConfirmationAsync(Order order, Customer customer)
        {
            var subject = $"Bestellbestätigung - Bestellung #{order.Id}";
            var body = $@"
                <h1>Vielen Dank für Ihre Bestellung!</h1>
                <p>Hallo {customer.FirstName},</p>
                <p>Ihre Bestellung #{order.Id} wurde erfolgreich aufgegeben.</p>
                <p>Gesamtbetrag: {order.TotalAmount:C}</p>
            ";

            await SendEmailAsync(customer.Email, subject, body);
        }

        public async Task SendOrderShippedAsync(Order order, Customer customer)
        {
            var subject = $"Ihre Bestellung #{order.Id} wurde versandt";
            var body = $@"
                <h1>Ihre Bestellung ist unterwegs!</h1>
                <p>Hallo {customer.FirstName},</p>
                <p>Ihre Bestellung #{order.Id} wurde versandt.</p>
            ";

            await SendEmailAsync(customer.Email, subject, body);
        }

        public async Task SendOrderDeliveredAsync(Order order, Customer customer)
        {
            var subject = $"Ihre Bestellung #{order.Id} wurde zugestellt";
            var body = $@"
                <h1>Ihre Bestellung wurde zugestellt!</h1>
                <p>Hallo {customer.FirstName},</p>
                <p>Ihre Bestellung #{order.Id} wurde erfolgreich zugestellt.</p>
            ";

            await SendEmailAsync(customer.Email, subject, body);
        }

        public async Task SendPaymentConfirmationAsync(Order order, Customer customer, Payment payment)
        {
            var subject = $"Zahlungsbestätigung - Bestellung #{order.Id}";
            var body = $@"
                <h1>Zahlung erhalten</h1>
                <p>Hallo {customer.FirstName},</p>
                <p>Ihre Zahlung über {payment.Amount:C} wurde erfolgreich verarbeitet.</p>
                <p>Zahlungsmethode: {payment.PaymentMethod}</p>
            ";

            await SendEmailAsync(customer.Email, subject, body);
        }

        public async Task SendInvoiceAsync(Order order, Customer customer, string invoicePath)
        {
            var subject = $"Rechnung - Bestellung #{order.Id}";
            var body = $@"
                <h1>Ihre Rechnung</h1>
                <p>Hallo {customer.FirstName},</p>
                <p>Anbei finden Sie die Rechnung für Ihre Bestellung #{order.Id}.</p>
            ";

            // TODO: Anhang hinzufügen
            await SendEmailAsync(customer.Email, subject, body);
        }

        public async Task SendCancellationConfirmationAsync(Order order, Customer customer)
        {
            var subject = $"Bestellung #{order.Id} storniert";
            var body = $@"
                <h1>Bestellung storniert</h1>
                <p>Hallo {customer.FirstName},</p>
                <p>Ihre Bestellung #{order.Id} wurde storniert.</p>
            ";

            await SendEmailAsync(customer.Email, subject, body);
        }

        private async Task SendEmailAsync(string to, string subject, string body)
        {
            // TODO: SMTP-Konfiguration (z.B. mit MailKit oder SendGrid)
            // var smtpHost = _configuration["Email:SmtpHost"];
            // var smtpPort = int.Parse(_configuration["Email:SmtpPort"]);
            // var smtpUser = _configuration["Email:SmtpUser"];
            // var smtpPassword = _configuration["Email:SmtpPassword"];

            _logger.LogInformation($"Email sent to {to}: {subject}");

            // Simulation
            await Task.CompletedTask;
        }
    }
}
