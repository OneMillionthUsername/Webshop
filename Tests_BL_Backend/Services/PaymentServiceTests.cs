using Microsoft.Extensions.Configuration;
using Moq;
using Webshop.Models;
using Webshop.Repositories;
using Webshop.Services;

namespace Tests_BL_Backend.Services
{
    public class PaymentServiceTests
    {
        private readonly Mock<IPaymentRepository> _mockPaymentRepository;
        private readonly PaymentService _service;

        public PaymentServiceTests()
        {
            _mockPaymentRepository = new Mock<IPaymentRepository>();
            _service = new PaymentService(_mockPaymentRepository.Object, Mock.Of<IConfiguration>());
        }

        // -------------------------
        // ProcessStripePaymentAsync
        // -------------------------

        [Fact]
        public async Task ProcessStripePaymentAsync_ValidInput_ReturnsPaymentWithCorrectData()
        {
            // Arrange
            _mockPaymentRepository.Setup(r => r.AddAsync(It.IsAny<Payment>()))
                .ReturnsAsync((Payment p) => p);

            // Act
            var result = await _service.ProcessStripePaymentAsync("tok_test", 100m, 1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Stripe", result.PaymentMethod);
            Assert.Equal(100m, result.Amount);
            Assert.Equal(1, result.OrderId);
            _mockPaymentRepository.Verify(r => r.AddAsync(It.IsAny<Payment>()), Times.Once);
        }

        [Fact]
        public async Task ProcessStripePaymentAsync_ValidInput_SetsStatusToCompleted()
        {
            // Arrange
            _mockPaymentRepository.Setup(r => r.AddAsync(It.IsAny<Payment>()))
                .ReturnsAsync((Payment p) => p);

            // Act
            var result = await _service.ProcessStripePaymentAsync("tok_test", 100m, 1);

            // Assert
            Assert.Equal("Completed", result.Status);
        }

        [Fact]
        public async Task ProcessStripePaymentAsync_ValidInput_TransactionIdStartsWithStripe()
        {
            // Arrange
            _mockPaymentRepository.Setup(r => r.AddAsync(It.IsAny<Payment>()))
                .ReturnsAsync((Payment p) => p);

            // Act
            var result = await _service.ProcessStripePaymentAsync("tok_test", 100m, 1);

            // Assert
            Assert.StartsWith("stripe_", result.TransactionId);
        }

        [Fact]
        public async Task ProcessStripePaymentAsync_MultipleCallsProduceDifferentTransactionIds()
        {
            // Arrange
            _mockPaymentRepository.Setup(r => r.AddAsync(It.IsAny<Payment>()))
                .ReturnsAsync((Payment p) => p);

            // Act
            var result1 = await _service.ProcessStripePaymentAsync("tok_1", 100m, 1);
            var result2 = await _service.ProcessStripePaymentAsync("tok_2", 100m, 1);

            // Assert
            Assert.NotEqual(result1.TransactionId, result2.TransactionId);
        }

        // -------------------------
        // ProcessPayPalPaymentAsync
        // -------------------------

        [Fact]
        public async Task ProcessPayPalPaymentAsync_ValidInput_ReturnsPaymentWithCorrectData()
        {
            // Arrange
            _mockPaymentRepository.Setup(r => r.AddAsync(It.IsAny<Payment>()))
                .ReturnsAsync((Payment p) => p);

            // Act
            var result = await _service.ProcessPayPalPaymentAsync("PAY-123", 200m, 2);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("PayPal", result.PaymentMethod);
            Assert.Equal(200m, result.Amount);
            Assert.Equal(2, result.OrderId);
            _mockPaymentRepository.Verify(r => r.AddAsync(It.IsAny<Payment>()), Times.Once);
        }

        [Fact]
        public async Task ProcessPayPalPaymentAsync_ValidInput_SetsStatusToCompleted()
        {
            // Arrange
            _mockPaymentRepository.Setup(r => r.AddAsync(It.IsAny<Payment>()))
                .ReturnsAsync((Payment p) => p);

            // Act
            var result = await _service.ProcessPayPalPaymentAsync("PAY-123", 200m, 2);

            // Assert
            Assert.Equal("Completed", result.Status);
        }

        [Fact]
        public async Task ProcessPayPalPaymentAsync_ValidInput_UsesProvidedPaymentIdAsTransactionId()
        {
            // Arrange
            _mockPaymentRepository.Setup(r => r.AddAsync(It.IsAny<Payment>()))
                .ReturnsAsync((Payment p) => p);

            // Act
            var result = await _service.ProcessPayPalPaymentAsync("PAY-UNIQUE-456", 200m, 2);

            // Assert
            Assert.Equal("PAY-UNIQUE-456", result.TransactionId);
        }

        // -------------------------
        // VerifyPaymentAsync
        // -------------------------

        [Fact]
        public async Task VerifyPaymentAsync_CompletedTransaction_ReturnsTrue()
        {
            // Arrange
            _mockPaymentRepository.Setup(r => r.GetByTransactionIdAsync("stripe_completed"))
                .ReturnsAsync(new Payment { Status = "Completed" });

            // Act
            var result = await _service.VerifyPaymentAsync("stripe_completed");

            // Assert
            Assert.True(result);
            _mockPaymentRepository.Verify(r => r.GetByTransactionIdAsync("stripe_completed"), Times.Once);
        }

        [Fact]
        public async Task VerifyPaymentAsync_PendingTransaction_ReturnsFalse()
        {
            // Arrange
            _mockPaymentRepository.Setup(r => r.GetByTransactionIdAsync("paypal_pending"))
                .ReturnsAsync(new Payment { Status = "Pending" });

            // Act
            var result = await _service.VerifyPaymentAsync("paypal_pending");

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task VerifyPaymentAsync_RefundedTransaction_ReturnsFalse()
        {
            // Arrange
            _mockPaymentRepository.Setup(r => r.GetByTransactionIdAsync("stripe_refunded"))
                .ReturnsAsync(new Payment { Status = "Refunded" });

            // Act
            var result = await _service.VerifyPaymentAsync("stripe_refunded");

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task VerifyPaymentAsync_UnknownTransactionId_ReturnsFalse()
        {
            // Arrange
            _mockPaymentRepository.Setup(r => r.GetByTransactionIdAsync("non_existent"))
                .ReturnsAsync((Payment?)null);

            // Act
            var result = await _service.VerifyPaymentAsync("non_existent");

            // Assert
            Assert.False(result);
        }

        // -------------------------
        // GetPaymentByIdAsync
        // -------------------------

        [Fact]
        public async Task GetPaymentByIdAsync_ExistingId_ReturnsCorrectPayment()
        {
            // Arrange
            var payment = new Payment { Id = 1, Amount = 99.99m, Order = new Order { Id = 1 } };
            _mockPaymentRepository.Setup(r => r.GetByIdWithDetailsAsync(1)).ReturnsAsync(payment);

            // Act
            var result = await _service.GetPaymentByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal(99.99m, result.Amount);
            _mockPaymentRepository.Verify(r => r.GetByIdWithDetailsAsync(1), Times.Once);
        }

        [Fact]
        public async Task GetPaymentByIdAsync_ExistingId_IncludesOrder()
        {
            // Arrange
            var payment = new Payment { Id = 1, Order = new Order { Id = 1 } };
            _mockPaymentRepository.Setup(r => r.GetByIdWithDetailsAsync(1)).ReturnsAsync(payment);

            // Act
            var result = await _service.GetPaymentByIdAsync(1);

            // Assert
            Assert.NotNull(result.Order);
            Assert.Equal(1, result.Order!.Id);
        }

        [Fact]
        public async Task GetPaymentByIdAsync_NonExistingId_ThrowsKeyNotFoundException()
        {
            // Arrange
            _mockPaymentRepository.Setup(r => r.GetByIdWithDetailsAsync(999))
                .ReturnsAsync((Payment?)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.GetPaymentByIdAsync(999));
        }

        // -------------------------
        // RefundPaymentAsync
        // -------------------------

        [Fact]
        public async Task RefundPaymentAsync_CompletedPayment_ReturnsTrue()
        {
            // Arrange
            _mockPaymentRepository.Setup(r => r.RefundAsync(1)).ReturnsAsync(true);

            // Act
            var result = await _service.RefundPaymentAsync(1);

            // Assert
            Assert.True(result);
            _mockPaymentRepository.Verify(r => r.RefundAsync(1), Times.Once);
        }

        [Fact]
        public async Task RefundPaymentAsync_AlreadyRefundedPayment_ReturnsFalse()
        {
            // Arrange
            _mockPaymentRepository.Setup(r => r.RefundAsync(3)).ReturnsAsync(false);

            // Act
            var result = await _service.RefundPaymentAsync(3);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task RefundPaymentAsync_NonExistingPayment_ReturnsFalse()
        {
            // Arrange
            _mockPaymentRepository.Setup(r => r.RefundAsync(999)).ReturnsAsync(false);

            // Act
            var result = await _service.RefundPaymentAsync(999);

            // Assert
            Assert.False(result);
        }

        // -------------------------
        // GetPaymentsByOrderAsync
        // -------------------------

        [Fact]
        public async Task GetPaymentsByOrderAsync_OrderWithMultiplePayments_ReturnsAllPayments()
        {
            // Arrange
            var payments = new List<Payment>
            {
                new Payment { Id = 1, OrderId = 1 },
                new Payment { Id = 2, OrderId = 1 }
            };
            _mockPaymentRepository.Setup(r => r.GetAllByOrderIdAsync(1)).ReturnsAsync(payments);

            // Act
            var result = await _service.GetPaymentsByOrderAsync(1);

            // Assert
            Assert.Equal(2, result.Count());
            _mockPaymentRepository.Verify(r => r.GetAllByOrderIdAsync(1), Times.Once);
        }

        [Fact]
        public async Task GetPaymentsByOrderAsync_ExistingOrder_ReturnsOnlyPaymentsForThatOrder()
        {
            // Arrange
            var payments = new List<Payment>
            {
                new Payment { Id = 3, OrderId = 2 },
                new Payment { Id = 4, OrderId = 2 }
            };
            _mockPaymentRepository.Setup(r => r.GetAllByOrderIdAsync(2)).ReturnsAsync(payments);

            // Act
            var result = await _service.GetPaymentsByOrderAsync(2);

            // Assert
            Assert.All(result, p => Assert.Equal(2, p.OrderId));
        }

        [Fact]
        public async Task GetPaymentsByOrderAsync_OrderWithNoPayments_ReturnsEmptyList()
        {
            // Arrange
            _mockPaymentRepository.Setup(r => r.GetAllByOrderIdAsync(99))
                .ReturnsAsync(new List<Payment>());

            // Act
            var result = await _service.GetPaymentsByOrderAsync(99);

            // Assert
            Assert.Empty(result);
        }
    }
}
