using Microsoft.Extensions.Configuration;
using Webshop.Models;
using Webshop.Repositories;

namespace Webshop.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IConfiguration _configuration;

        public PaymentService(IPaymentRepository paymentRepository, IConfiguration configuration)
        {
            _paymentRepository = paymentRepository;
            _configuration = configuration;
        }

        public async Task<Payment> ProcessStripePaymentAsync(string token, decimal amount, int orderId)
        {
            // TODO: Stripe SDK Integration
            // var stripeApiKey = _configuration["Stripe:SecretKey"];
            // var chargeService = new ChargeService();
            // var charge = await chargeService.CreateAsync(...);

            var payment = new Payment
            {
                OrderId = orderId,
                Amount = amount,
                PaymentMethod = "Stripe",
                Status = "Completed",
                TransactionId = $"stripe_{Guid.NewGuid()}",
                PaymentDate = DateTime.UtcNow
            };

            return await _paymentRepository.AddAsync(payment);
        }

        public async Task<Payment> ProcessPayPalPaymentAsync(string paymentId, decimal amount, int orderId)
        {
            // TODO: PayPal SDK Integration
            // var paypalClientId = _configuration["PayPal:ClientId"];
            // var paypalSecret = _configuration["PayPal:Secret"];

            var payment = new Payment
            {
                OrderId = orderId,
                Amount = amount,
                PaymentMethod = "PayPal",
                Status = "Completed",
                TransactionId = paymentId,
                PaymentDate = DateTime.UtcNow
            };

            return await _paymentRepository.AddAsync(payment);
        }

        public async Task<bool> VerifyPaymentAsync(string paymentReference)
        {
            var payment = await _paymentRepository.GetByTransactionIdAsync(paymentReference);
            return payment?.Status == "Completed";
        }

        public async Task<Payment> GetPaymentByIdAsync(int paymentId)
        {
            return await _paymentRepository.GetByIdWithDetailsAsync(paymentId)
                ?? throw new KeyNotFoundException($"Payment with ID {paymentId} not found.");
        }

        public async Task<bool> RefundPaymentAsync(int paymentId)
        {
            return await _paymentRepository.RefundAsync(paymentId);
        }

        public async Task<IEnumerable<Payment>> GetPaymentsByOrderAsync(int orderId)
        {
            return await _paymentRepository.GetAllByOrderIdAsync(orderId);
        }
    }
}
