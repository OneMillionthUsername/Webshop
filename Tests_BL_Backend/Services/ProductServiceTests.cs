using Moq;
using Webshop.Dtos.Categories;
using Webshop.Dtos.Products;
using Webshop.Models;
using Webshop.Repositories;
using Webshop.Services;

namespace Tests_BL_Backend.Services
{
	public class ProductServiceTests
	{
		private readonly Mock<IProductRepository> _mockProductRepository;
		private readonly Mock<ICategoryRepository> _mockCategoryRepository;
		private readonly ProductService _service;

		public ProductServiceTests()
		{
			_mockProductRepository = new Mock<IProductRepository>();
			_mockCategoryRepository = new Mock<ICategoryRepository>();
			_service = new ProductService(_mockProductRepository.Object, _mockCategoryRepository.Object);
		}

		// -------------------------
		// GetAllProductsAsync
		// -------------------------

		[Fact]
		public async Task GetAllProductsAsync_ReturnsAllProductsAsDtos()
		{
			// Arrange
			var products = new List<Product>
			{
				new Product { Id = 1, Name = "Laptop", Description = "A laptop", BasePrice = 999.99m, CategoryId = 1 },
				new Product { Id = 2, Name = "Mouse", Description = "A mouse", BasePrice = 29.99m, CategoryId = 1 }
			};
			_mockProductRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(products);

			// Act
			var result = await _service.GetAllProductsAsync();

			// Assert
			Assert.Equal(2, result.Count());
			Assert.Equal("Laptop", result.First().Name);
			Assert.Equal(999.99m, result.First().BasePrice);
			_mockProductRepository.Verify(r => r.GetAllAsync(), Times.Once);
		}

		[Fact]
		public async Task GetAllProductsAsync_EmptyRepository_ReturnsEmptyList()
		{
			// Arrange
			_mockProductRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<Product>());

			// Act
			var result = await _service.GetAllProductsAsync();

			// Assert
			Assert.Empty(result);
			_mockProductRepository.Verify(r => r.GetAllAsync(), Times.Once);
		}

		// -------------------------
		// GetProductByIdAsync
		// -------------------------

		[Fact]
		public async Task GetProductByIdAsync_ExistingId_ReturnsProductDto()
		{
			// Arrange
			var product = new Product { Id = 1, Name = "Laptop", Description = "A laptop", BasePrice = 999.99m, CategoryId = 1 };
			_mockProductRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(product);

			// Act
			var result = await _service.GetProductByIdAsync(1);

			// Assert
			Assert.NotNull(result);
			Assert.Equal(1, result.Id);
			Assert.Equal("Laptop", result.Name);
			Assert.Equal(999.99m, result.BasePrice);
			_mockProductRepository.Verify(r => r.GetByIdAsync(1), Times.Once);
		}

		[Fact]
		public async Task GetProductByIdAsync_NonExistingId_ReturnsNull()
		{
			// Arrange
			_mockProductRepository.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((Product?)null);

			// Act
			var result = await _service.GetProductByIdAsync(999);

			// Assert
			Assert.Null(result);
			_mockProductRepository.Verify(r => r.GetByIdAsync(999), Times.Once);
		}

		// -------------------------
		// CreateProductAsync
		// -------------------------

		[Fact]
		public async Task CreateProductAsync_ValidDto_ReturnsCreatedProductDto()
		{
			// Arrange
			var createDto = new CreateProductDto { Name = "Keyboard", Description = "Mechanical", BasePrice = 149.99m, CategoryId = 2 };
			var createdProduct = new Product { Id = 5, Name = "Keyboard", Description = "Mechanical", BasePrice = 149.99m, CategoryId = 2 };
			_mockProductRepository.Setup(r => r.AddAsync(It.IsAny<Product>())).ReturnsAsync(createdProduct);

			// Act
			var result = await _service.CreateProductAsync(createDto);

			// Assert
			Assert.NotNull(result);
			Assert.Equal(5, result.Id);
			Assert.Equal("Keyboard", result.Name);
			Assert.Equal(149.99m, result.BasePrice);
			Assert.Equal(2, result.CategoryId);
			_mockProductRepository.Verify(r => r.AddAsync(It.IsAny<Product>()), Times.Once);
		}

		// -------------------------
		// UpdateProductAsync
		// -------------------------

		[Fact]
		public async Task UpdateProductAsync_ExistingId_ReturnsUpdatedProductDto()
		{
			// Arrange
			var existingProduct = new Product { Id = 1, Name = "Old Name", Description = "Old Desc", BasePrice = 50m, CategoryId = 1 };
			var updatedProduct = new Product { Id = 1, Name = "New Name", Description = "New Desc", BasePrice = 75m, CategoryId = 2 };
			var updateDto = new UpdateProductDto { Id = 1, Name = "New Name", Description = "New Desc", BasePrice = 75m, CategoryId = 2 };

			_mockProductRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(existingProduct);
			_mockProductRepository.Setup(r => r.UpdateAsync(It.IsAny<Product>())).ReturnsAsync(updatedProduct);

			// Act
			var result = await _service.UpdateProductAsync(updateDto);

			// Assert
			Assert.NotNull(result);
			Assert.Equal(1, result.Id);
			Assert.Equal("New Name", result.Name);
			Assert.Equal(75m, result.BasePrice);
			_mockProductRepository.Verify(r => r.GetByIdAsync(1), Times.Once);
			_mockProductRepository.Verify(r => r.UpdateAsync(It.IsAny<Product>()), Times.Once);
		}

		[Fact]
		public async Task UpdateProductAsync_NonExistingId_ThrowsKeyNotFoundException()
		{
			// Arrange
			var updateDto = new UpdateProductDto { Id = 999, Name = "X", Description = "X", BasePrice = 1m, CategoryId = 1 };
			_mockProductRepository.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((Product?)null);

			// Act & Assert
			await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.UpdateProductAsync(updateDto));
			_mockProductRepository.Verify(r => r.UpdateAsync(It.IsAny<Product>()), Times.Never);
		}

		// -------------------------
		// DeleteProductAsync
		// -------------------------

		[Fact]
		public async Task DeleteProductAsync_CallsRepositoryDelete()
		{
			// Arrange
			_mockProductRepository.Setup(r => r.DeleteAsync(1)).Returns(Task.CompletedTask);

			// Act
			await _service.DeleteProductAsync(1);

			// Assert
			_mockProductRepository.Verify(r => r.DeleteAsync(1), Times.Once);
		}

		// -------------------------
		// ProductExistsAsync
		// -------------------------

		[Fact]
		public async Task ProductExistsAsync_ExistingId_ReturnsTrue()
		{
			// Arrange
			_mockProductRepository.Setup(r => r.ExistsAsync(1)).ReturnsAsync(true);

			// Act
			var result = await _service.ProductExistsAsync(1);

			// Assert
			Assert.True(result);
			_mockProductRepository.Verify(r => r.ExistsAsync(1), Times.Once);
		}

		[Fact]
		public async Task ProductExistsAsync_NonExistingId_ReturnsFalse()
		{
			// Arrange
			_mockProductRepository.Setup(r => r.ExistsAsync(999)).ReturnsAsync(false);

			// Act
			var result = await _service.ProductExistsAsync(999);

			// Assert
			Assert.False(result);
			_mockProductRepository.Verify(r => r.ExistsAsync(999), Times.Once);
		}

		// -------------------------
		// GetProductsByCategoryAsync
		// -------------------------

		[Fact]
		public async Task GetProductsByCategoryAsync_ExistingCategory_ReturnsFilteredDtos()
		{
			// Arrange
			var products = new List<Product>
			{
				new Product { Id = 1, Name = "Laptop", Description = "A laptop", BasePrice = 999m, CategoryId = 1 },
				new Product { Id = 2, Name = "Desktop", Description = "A desktop", BasePrice = 799m, CategoryId = 1 }
			};
			_mockProductRepository.Setup(r => r.GetByCategoryIdAsync(1)).ReturnsAsync(products);

			// Act
			var result = await _service.GetProductsByCategoryAsync(1);

			// Assert
			Assert.Equal(2, result.Count());
			Assert.All(result, p => Assert.Equal(1, p.CategoryId));
			_mockProductRepository.Verify(r => r.GetByCategoryIdAsync(1), Times.Once);
		}

		[Fact]
		public async Task GetProductsByCategoryAsync_NonExistingCategory_ReturnsEmpty()
		{
			// Arrange
			_mockProductRepository.Setup(r => r.GetByCategoryIdAsync(999)).ReturnsAsync(new List<Product>());

			// Act
			var result = await _service.GetProductsByCategoryAsync(999);

			// Assert
			Assert.Empty(result);
			_mockProductRepository.Verify(r => r.GetByCategoryIdAsync(999), Times.Once);
		}

		// -------------------------
		// SearchProductsAsync
		// -------------------------

		[Fact]
		public async Task SearchProductsAsync_MatchingTerm_ReturnsMatchingDtos()
		{
			// Arrange
			var products = new List<Product>
			{
				new Product { Id = 1, Name = "Gaming Laptop", Description = "Fast", BasePrice = 1500m, CategoryId = 1 }
			};
			_mockProductRepository.Setup(r => r.SearchAsync("Laptop")).ReturnsAsync(products);

			// Act
			var result = await _service.SearchProductsAsync("Laptop");

			// Assert
			Assert.Single(result);
			Assert.Equal("Gaming Laptop", result.First().Name);
			_mockProductRepository.Verify(r => r.SearchAsync("Laptop"), Times.Once);
		}

		[Fact]
		public async Task SearchProductsAsync_NoMatch_ReturnsEmpty()
		{
			// Arrange
			_mockProductRepository.Setup(r => r.SearchAsync("xyz")).ReturnsAsync(new List<Product>());

			// Act
			var result = await _service.SearchProductsAsync("xyz");

			// Assert
			Assert.Empty(result);
			_mockProductRepository.Verify(r => r.SearchAsync("xyz"), Times.Once);
		}

		// -------------------------
		// GetAllCategoriesAsync
		// -------------------------

		[Fact]
		public async Task GetAllCategoriesAsync_ReturnsCategoryDtos()
		{
			// Arrange
			var categories = new List<Category>
			{
				new Category { Id = 1, Name = "Electronics", Description = "Tech" },
				new Category { Id = 2, Name = "Accessories", Description = "Acc" }
			};
			_mockCategoryRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(categories);

			// Act
			var result = await _service.GetAllCategoriesAsync();

			// Assert
			Assert.Equal(2, result.Count());
			Assert.Equal("Electronics", result.First().Name);
			_mockCategoryRepository.Verify(r => r.GetAllAsync(), Times.Once);
		}

		// -------------------------
		// CheckInventoryAsync
		// -------------------------

		[Fact]
		public async Task CheckInventoryAsync_SufficientStock_ReturnsTrue()
		{
			// Arrange
			_mockProductRepository.Setup(r => r.GetAvailableStockAsync(1)).ReturnsAsync(10);

			// Act
			var result = await _service.CheckInventoryAsync(1, 5);

			// Assert
			Assert.True(result);
		}

		[Fact]
		public async Task CheckInventoryAsync_InsufficientStock_ReturnsFalse()
		{
			// Arrange
			_mockProductRepository.Setup(r => r.GetAvailableStockAsync(1)).ReturnsAsync(3);

			// Act
			var result = await _service.CheckInventoryAsync(1, 5);

			// Assert
			Assert.False(result);
		}

		[Fact]
		public async Task CheckInventoryAsync_ExactStock_ReturnsTrue()
		{
			// Arrange
			_mockProductRepository.Setup(r => r.GetAvailableStockAsync(1)).ReturnsAsync(5);

			// Act
			var result = await _service.CheckInventoryAsync(1, 5);

			// Assert
			Assert.True(result);
		}

		// -------------------------
		// GetAvailableStockAsync
		// -------------------------

		[Fact]
		public async Task GetAvailableStockAsync_ReturnsStockFromRepository()
		{
			// Arrange
			_mockProductRepository.Setup(r => r.GetAvailableStockAsync(1)).ReturnsAsync(42);

			// Act
			var result = await _service.GetAvailableStockAsync(1);

			// Assert
			Assert.Equal(42, result);
			_mockProductRepository.Verify(r => r.GetAvailableStockAsync(1), Times.Once);
		}

		// -------------------------
		// GetFeaturedProductsAsync
		// -------------------------

		[Fact]
		public async Task GetFeaturedProductsAsync_MoreThan10Products_ReturnsFirst10()
		{
			// Arrange
			var products = Enumerable.Range(1, 15)
				.Select(i => new Product { Id = i, Name = $"Product {i}", Description = "", BasePrice = i * 10m, CategoryId = 1 })
				.ToList();
			_mockProductRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(products);

			// Act
			var result = await _service.GetFeaturedProductsAsync();

			// Assert
			Assert.Equal(10, result.Count());
			Assert.Equal(1, result.First().Id);
			_mockProductRepository.Verify(r => r.GetAllAsync(), Times.Once);
		}

		[Fact]
		public async Task GetFeaturedProductsAsync_LessThan10Products_ReturnsAll()
		{
			// Arrange
			var products = Enumerable.Range(1, 4)
				.Select(i => new Product { Id = i, Name = $"Product {i}", Description = "", BasePrice = i * 10m, CategoryId = 1 })
				.ToList();
			_mockProductRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(products);

			// Act
			var result = await _service.GetFeaturedProductsAsync();

			// Assert
			Assert.Equal(4, result.Count());
			_mockProductRepository.Verify(r => r.GetAllAsync(), Times.Once);
		}
	}
}
