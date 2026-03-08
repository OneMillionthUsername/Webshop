using Moq;
using Webshop.Models;
using Webshop.Repositories;
using Webshop.Services;

namespace Tests_BL_Backend.Services
{
    public class OrderServiceTests
    {
        private readonly Mock<IOrderRepository> _mockOrderRepository;
        private readonly OrderService _service;

        public OrderServiceTests()
        {
            _mockOrderRepository = new Mock<IOrderRepository>();
            _service = new OrderService(_mockOrderRepository.Object);
        }

        // -------------------------
        // CreateOrderAsync
        // -------------------------

        [Fact]
        public async Task CreateOrderAsync_ValidInput_ReturnsOrderWithCorrectData()
        {
            // Arrange
            var customer = new Customer { Id = 1, FirstName = "Max" };
            var items = new List<OrderItem> { new OrderItem { ProductVariantId = 1, Quantity = 2, PriceAtPurchase = 50m } };
            _mockOrderRepository.Setup(r => r.AddAsync(It.IsAny<Order>()))
                .ReturnsAsync((Order o) => o);

            // Act
            var result = await _service.CreateOrderAsync(customer, items, 100m);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.CustomerId);
            Assert.Equal(100m, result.TotalAmount);
            Assert.Single(result.Items);
            _mockOrderRepository.Verify(r => r.AddAsync(It.IsAny<Order>()), Times.Once);
        }

        [Fact]
        public async Task CreateOrderAsync_ValidInput_SetsOrderDateToUtcNow()
        {
            // Arrange
            var customer = new Customer { Id = 1 };
            _mockOrderRepository.Setup(r => r.AddAsync(It.IsAny<Order>()))
                .ReturnsAsync((Order o) => o);

            // Act
            var before = DateTime.UtcNow;
            var result = await _service.CreateOrderAsync(customer, new List<OrderItem>(), 0m);
            var after = DateTime.UtcNow;

            // Assert
            Assert.InRange(result.OrderDate, before, after);
        }

        // -------------------------
        // GetOrderByIdAsync
        // -------------------------

        [Fact]
        public async Task GetOrderByIdAsync_ExistingId_ReturnsOrder()
        {
            // Arrange
            var order = new Order { Id = 1, CustomerId = 1, TotalAmount = 100m };
            _mockOrderRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(order);

            // Act
            var result = await _service.GetOrderByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal(100m, result.TotalAmount);
            _mockOrderRepository.Verify(r => r.GetByIdAsync(1), Times.Once);
        }

        [Fact]
        public async Task GetOrderByIdAsync_NonExistingId_ThrowsKeyNotFoundException()
        {
            // Arrange
            _mockOrderRepository.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((Order?)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.GetOrderByIdAsync(999));
        }

        // -------------------------
        // GetOrdersByCustomerAsync
        // -------------------------

        [Fact]
        public async Task GetOrdersByCustomerAsync_ExistingCustomer_ReturnsOrders()
        {
            // Arrange
            var orders = new List<Order>
            {
                new Order { Id = 1, CustomerId = 1, OrderDate = DateTime.Parse("2024-01-01") },
                new Order { Id = 2, CustomerId = 1, OrderDate = DateTime.Parse("2024-03-01") },
                new Order { Id = 3, CustomerId = 1, OrderDate = DateTime.Parse("2024-02-01") }
            };
            _mockOrderRepository.Setup(r => r.GetByCustomerIdAsync(1)).ReturnsAsync(orders);

            // Act
            var result = (await _service.GetOrdersByCustomerAsync(1)).ToList();

            // Assert
            Assert.Equal(3, result.Count);
            _mockOrderRepository.Verify(r => r.GetByCustomerIdAsync(1), Times.Once);
        }

        [Fact]
        public async Task GetOrdersByCustomerAsync_ReturnsOrdersOrderedByDateDescending()
        {
            // Arrange
            var orders = new List<Order>
            {
                new Order { Id = 1, CustomerId = 1, OrderDate = DateTime.Parse("2024-01-01") },
                new Order { Id = 2, CustomerId = 1, OrderDate = DateTime.Parse("2024-03-01") },
                new Order { Id = 3, CustomerId = 1, OrderDate = DateTime.Parse("2024-02-01") }
            };
            _mockOrderRepository.Setup(r => r.GetByCustomerIdAsync(1)).ReturnsAsync(orders);

            // Act
            var result = (await _service.GetOrdersByCustomerAsync(1)).ToList();

            // Assert
            Assert.Equal(2, result[0].Id); // März zuerst
            Assert.Equal(3, result[1].Id); // Februar zweite
            Assert.Equal(1, result[2].Id); // Januar dritte
        }

        [Fact]
        public async Task GetOrdersByCustomerAsync_NoOrders_ReturnsEmptyList()
        {
            // Arrange
            _mockOrderRepository.Setup(r => r.GetByCustomerIdAsync(99)).ReturnsAsync(new List<Order>());

            // Act
            var result = await _service.GetOrdersByCustomerAsync(99);

            // Assert
            Assert.Empty(result);
        }

        // -------------------------
        // UpdateOrderStatusAsync
        // -------------------------

        [Fact]
        public async Task UpdateOrderStatusAsync_ValidInput_CallsRepository()
        {
            // Arrange
            _mockOrderRepository.Setup(r => r.UpdateStatusAsync(1, "Shipped")).Returns(Task.CompletedTask);

            // Act
            await _service.UpdateOrderStatusAsync(1, "Shipped");

            // Assert
            _mockOrderRepository.Verify(r => r.UpdateStatusAsync(1, "Shipped"), Times.Once);
        }

        // -------------------------
        // CancelOrderAsync
        // -------------------------

        [Fact]
        public async Task CancelOrderAsync_ExistingOrder_ReturnsTrue()
        {
            // Arrange
            _mockOrderRepository.Setup(r => r.ExistsAsync(1)).ReturnsAsync(true);
            _mockOrderRepository.Setup(r => r.UpdateStatusAsync(1, "Cancelled")).Returns(Task.CompletedTask);

            // Act
            var result = await _service.CancelOrderAsync(1);

            // Assert
            Assert.True(result);
            _mockOrderRepository.Verify(r => r.UpdateStatusAsync(1, "Cancelled"), Times.Once);
        }

        [Fact]
        public async Task CancelOrderAsync_NonExistingOrder_ReturnsFalse()
        {
            // Arrange
            _mockOrderRepository.Setup(r => r.ExistsAsync(999)).ReturnsAsync(false);

            // Act
            var result = await _service.CancelOrderAsync(999);

            // Assert
            Assert.False(result);
            _mockOrderRepository.Verify(r => r.UpdateStatusAsync(It.IsAny<int>(), It.IsAny<string>()), Times.Never);
        }

        // -------------------------
        // GenerateInvoiceAsync
        // -------------------------

        [Fact]
        public async Task GenerateInvoiceAsync_ExistingOrder_ReturnsInvoicePath()
        {
            // Arrange
            _mockOrderRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(new Order { Id = 1 });

            // Act
            var result = await _service.GenerateInvoiceAsync(1);

            // Assert
            Assert.StartsWith("Invoices/Invoice_1_", result);
            Assert.EndsWith(".pdf", result);
        }

        [Fact]
        public async Task GenerateInvoiceAsync_NonExistingOrder_ThrowsKeyNotFoundException()
        {
            // Arrange
            _mockOrderRepository.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((Order?)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.GenerateInvoiceAsync(999));
        }

        // -------------------------
        // GetAllOrdersAsync
        // -------------------------

        [Fact]
        public async Task GetAllOrdersAsync_ReturnsAllOrdersOrderedByDateDescending()
        {
            // Arrange
            var orders = new List<Order>
            {
                new Order { Id = 1, OrderDate = DateTime.Parse("2024-01-01") },
                new Order { Id = 2, OrderDate = DateTime.Parse("2024-03-01") },
                new Order { Id = 3, OrderDate = DateTime.Parse("2024-02-01") }
            };
            _mockOrderRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(orders);

            // Act
            var result = (await _service.GetAllOrdersAsync()).ToList();

            // Assert
            Assert.Equal(3, result.Count);
            Assert.Equal(2, result[0].Id); // März zuerst
            Assert.Equal(3, result[1].Id); // Februar zweite
            Assert.Equal(1, result[2].Id); // Januar dritte
            _mockOrderRepository.Verify(r => r.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task GetAllOrdersAsync_EmptyRepository_ReturnsEmptyList()
        {
            // Arrange
            _mockOrderRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<Order>());

            // Act
            var result = await _service.GetAllOrdersAsync();

            // Assert
            Assert.Empty(result);
        }
    }
}
