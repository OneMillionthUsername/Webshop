using Microsoft.AspNetCore.Http;
using Moq;
using Webshop.Models;
using Webshop.Repositories;
using Webshop.Services;

namespace Tests_BL_Backend.Services
{
    public class CartServiceTests
    {
        private readonly Mock<IOrderItemRepository> _mockOrderItemRepository;
        private readonly Mock<IProductVariantRepository> _mockProductVariantRepository;
        private readonly Mock<IProductRepository> _mockProductRepository;
        private readonly CartService _service;

        public CartServiceTests()
        {
            _mockOrderItemRepository = new Mock<IOrderItemRepository>();
            _mockProductVariantRepository = new Mock<IProductVariantRepository>();
            _mockProductRepository = new Mock<IProductRepository>();

            _service = new CartService(
                _mockOrderItemRepository.Object,
                _mockProductVariantRepository.Object,
                _mockProductRepository.Object,
                Mock.Of<IHttpContextAccessor>());
        }

        // -------------------------
        // AddToCartAsync
        // -------------------------

        [Fact]
        public async Task AddToCartAsync_ValidProductAndVariant_CallsAddAsync()
        {
            // Arrange
            var variant = new ProductVariant { Id = 1, ProductId = 1, StockQuantity = 10, PriceAdjustment = 0m };
            var product = new Product { Id = 1, BasePrice = 1000m };
            _mockProductVariantRepository.Setup(r => r.GetByProductIdAndVariantIdAsync(1, 1)).ReturnsAsync(variant);
            _mockProductRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(product);
            _mockOrderItemRepository.Setup(r => r.AddAsync(It.IsAny<OrderItem>())).ReturnsAsync((OrderItem oi) => oi);

            // Act
            await _service.AddToCartAsync(1, 2, 1);

            // Assert
            _mockOrderItemRepository.Verify(r => r.AddAsync(It.Is<OrderItem>(oi =>
                oi.ProductVariantId == 1 && oi.Quantity == 2 && oi.PriceAtPurchase == 1000m)), Times.Once);
        }

        [Fact]
        public async Task AddToCartAsync_VariantWithPriceAdjustment_OrderItemHasCorrectPrice()
        {
            // Arrange – BasePrice 1000m + PriceAdjustment 200m = 1200m
            var variant = new ProductVariant { Id = 1, ProductId = 1, StockQuantity = 10, PriceAdjustment = 200m };
            var product = new Product { Id = 1, BasePrice = 1000m };
            _mockProductVariantRepository.Setup(r => r.GetByProductIdAndVariantIdAsync(1, 1)).ReturnsAsync(variant);
            _mockProductRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(product);
            _mockOrderItemRepository.Setup(r => r.AddAsync(It.IsAny<OrderItem>())).ReturnsAsync((OrderItem oi) => oi);

            // Act
            await _service.AddToCartAsync(1, 1, 1);

            // Assert
            _mockOrderItemRepository.Verify(r => r.AddAsync(It.Is<OrderItem>(oi => oi.PriceAtPurchase == 1200m)), Times.Once);
        }

        [Fact]
        public async Task AddToCartAsync_VariantNotFound_ThrowsKeyNotFoundException()
        {
            // Arrange
            _mockProductVariantRepository.Setup(r => r.GetByProductIdAndVariantIdAsync(999, null))
                .ReturnsAsync((ProductVariant?)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.AddToCartAsync(999, 1));
        }

        [Fact]
        public async Task AddToCartAsync_InsufficientStock_ThrowsInvalidOperationException()
        {
            // Arrange
            var variant = new ProductVariant { Id = 1, ProductId = 1, StockQuantity = 3 };
            _mockProductVariantRepository.Setup(r => r.GetByProductIdAndVariantIdAsync(1, 1)).ReturnsAsync(variant);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _service.AddToCartAsync(1, 5, 1));
        }

        // -------------------------
        // RemoveFromCartAsync
        // -------------------------

        [Fact]
        public async Task RemoveFromCartAsync_ExistingItem_CallsDeleteAsync()
        {
            // Arrange
            _mockOrderItemRepository.Setup(r => r.DeleteAsync(1)).ReturnsAsync(true);

            // Act
            await _service.RemoveFromCartAsync(1);

            // Assert
            _mockOrderItemRepository.Verify(r => r.DeleteAsync(1), Times.Once);
        }

        [Fact]
        public async Task RemoveFromCartAsync_NonExistingItem_CallsDeleteAsync()
        {
            // Arrange
            _mockOrderItemRepository.Setup(r => r.DeleteAsync(999)).ReturnsAsync(false);

            // Act
            await _service.RemoveFromCartAsync(999);

            // Assert
            _mockOrderItemRepository.Verify(r => r.DeleteAsync(999), Times.Once);
        }

        // -------------------------
        // UpdateQuantityAsync
        // -------------------------

        [Fact]
        public async Task UpdateQuantityAsync_ExistingItem_UpdatesQuantity()
        {
            // Arrange
            var item = new OrderItem { Id = 1, Quantity = 2 };
            _mockOrderItemRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(item);
            _mockOrderItemRepository.Setup(r => r.UpdateAsync(It.IsAny<OrderItem>())).ReturnsAsync((OrderItem oi) => oi);

            // Act
            await _service.UpdateQuantityAsync(1, 5);

            // Assert
            _mockOrderItemRepository.Verify(r => r.UpdateAsync(It.Is<OrderItem>(oi => oi.Quantity == 5)), Times.Once);
        }

        [Fact]
        public async Task UpdateQuantityAsync_NonExistingItem_DoesNotCallUpdate()
        {
            // Arrange
            _mockOrderItemRepository.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((OrderItem?)null);

            // Act
            await _service.UpdateQuantityAsync(999, 3);

            // Assert
            _mockOrderItemRepository.Verify(r => r.UpdateAsync(It.IsAny<OrderItem>()), Times.Never);
        }

        // -------------------------
        // GetCartItemsAsync
        // -------------------------

        [Fact]
        public async Task GetCartItemsAsync_ReturnsCartItems()
        {
            // Arrange
            var items = new List<OrderItem>
            {
                new OrderItem { Id = 1, ProductVariantId = 1, Quantity = 2 },
                new OrderItem { Id = 2, ProductVariantId = 2, Quantity = 1 }
            };
            _mockOrderItemRepository.Setup(r => r.GetCartItemsAsync()).ReturnsAsync(items);

            // Act
            var result = await _service.GetCartItemsAsync();

            // Assert
            Assert.Equal(2, result.Count());
            _mockOrderItemRepository.Verify(r => r.GetCartItemsAsync(), Times.Once);
        }

        [Fact]
        public async Task GetCartItemsAsync_EmptyCart_ReturnsEmptyList()
        {
            // Arrange
            _mockOrderItemRepository.Setup(r => r.GetCartItemsAsync()).ReturnsAsync(new List<OrderItem>());

            // Act
            var result = await _service.GetCartItemsAsync();

            // Assert
            Assert.Empty(result);
        }

        // -------------------------
        // CalculateCartTotalAsync
        // -------------------------

        [Fact]
        public async Task CalculateCartTotalAsync_WithItems_ReturnsCorrectTotal()
        {
            // Arrange – 100*2 + 50*3 = 350
            var items = new List<OrderItem>
            {
                new OrderItem { PriceAtPurchase = 100m, Quantity = 2 },
                new OrderItem { PriceAtPurchase = 50m,  Quantity = 3 }
            };
            _mockOrderItemRepository.Setup(r => r.GetCartItemsAsync()).ReturnsAsync(items);

            // Act
            var result = await _service.CalculateCartTotalAsync();

            // Assert
            Assert.Equal(350m, result);
        }

        [Fact]
        public async Task CalculateCartTotalAsync_EmptyCart_ReturnsZero()
        {
            // Arrange
            _mockOrderItemRepository.Setup(r => r.GetCartItemsAsync()).ReturnsAsync(new List<OrderItem>());

            // Act
            var result = await _service.CalculateCartTotalAsync();

            // Assert
            Assert.Equal(0m, result);
        }

        // -------------------------
        // ValidateCartAsync
        // -------------------------

        [Fact]
        public async Task ValidateCartAsync_AllItemsInStock_ReturnsTrue()
        {
            // Arrange
            var items = new List<OrderItem>
            {
                new OrderItem { ProductVariantId = 1, Quantity = 2 },
                new OrderItem { ProductVariantId = 2, Quantity = 1 }
            };
            _mockOrderItemRepository.Setup(r => r.GetCartItemsAsync()).ReturnsAsync(items);
            _mockProductVariantRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(new ProductVariant { StockQuantity = 10 });
            _mockProductVariantRepository.Setup(r => r.GetByIdAsync(2)).ReturnsAsync(new ProductVariant { StockQuantity = 5 });

            // Act
            var result = await _service.ValidateCartAsync();

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task ValidateCartAsync_InsufficientStock_ReturnsFalse()
        {
            // Arrange
            var items = new List<OrderItem>
            {
                new OrderItem { ProductVariantId = 1, Quantity = 10 }
            };
            _mockOrderItemRepository.Setup(r => r.GetCartItemsAsync()).ReturnsAsync(items);
            _mockProductVariantRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(new ProductVariant { StockQuantity = 3 });

            // Act
            var result = await _service.ValidateCartAsync();

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task ValidateCartAsync_EmptyCart_ReturnsTrue()
        {
            // Arrange
            _mockOrderItemRepository.Setup(r => r.GetCartItemsAsync()).ReturnsAsync(new List<OrderItem>());

            // Act
            var result = await _service.ValidateCartAsync();

            // Assert
            Assert.True(result);
        }

        // -------------------------
        // ClearCartAsync
        // -------------------------

        [Fact]
        public async Task ClearCartAsync_WithItems_DeletesAllItems()
        {
            // Arrange
            var items = new List<OrderItem>
            {
                new OrderItem { Id = 1 },
                new OrderItem { Id = 2 }
            };
            _mockOrderItemRepository.Setup(r => r.GetCartItemsAsync()).ReturnsAsync(items);
            _mockOrderItemRepository.Setup(r => r.DeleteAsync(It.IsAny<int>())).ReturnsAsync(true);

            // Act
            await _service.ClearCartAsync();

            // Assert
            _mockOrderItemRepository.Verify(r => r.DeleteAsync(1), Times.Once);
            _mockOrderItemRepository.Verify(r => r.DeleteAsync(2), Times.Once);
        }

        [Fact]
        public async Task ClearCartAsync_EmptyCart_DoesNotCallDelete()
        {
            // Arrange
            _mockOrderItemRepository.Setup(r => r.GetCartItemsAsync()).ReturnsAsync(new List<OrderItem>());

            // Act
            await _service.ClearCartAsync();

            // Assert
            _mockOrderItemRepository.Verify(r => r.DeleteAsync(It.IsAny<int>()), Times.Never);
        }
    }
}
