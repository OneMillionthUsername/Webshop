using Moq;
using Webshop.Dtos.Categories;
using Webshop.Models;
using Webshop.Repositories;
using Webshop.Services;

namespace Tests_BL_Backend.Services
{
    public class CategoryServiceTests
    {
        private readonly Mock<ICategoryRepository> _mockRepository;
        private readonly CategoryService _service;

        public CategoryServiceTests()
        {
            _mockRepository = new Mock<ICategoryRepository>();
            _service = new CategoryService(_mockRepository.Object);
        }

        [Fact]
        public async Task GetAllCategoriesAsync_ReturnsAllCategories()
        {
            // Arrange
            var categories = new List<Category>
            {
                new Category { Id = 1, Name = "Electronics", Description = "Tech products" },
                new Category { Id = 2, Name = "Books", Description = "Reading materials" }
            };
            _mockRepository.Setup(r => r.GetAllAsync())
                .ReturnsAsync(categories);

            // Act
            var result = await _service.GetAllCategoriesAsync();

            // Assert
            Assert.Equal(2, result.Count());
            Assert.Equal("Electronics", result.First().Name);
            _mockRepository.Verify(r => r.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task GetCategoryByIdAsync_WithValidId_ReturnsCategory()
        {
            // Arrange
            var category = new Category { Id = 1, Name = "Electronics", Description = "Tech" };
            _mockRepository.Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(category);

            // Act
            var result = await _service.GetCategoryByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("Electronics", result.Name);
            _mockRepository.Verify(r => r.GetByIdAsync(1), Times.Once);
        }

        [Fact]
        public async Task GetCategoryByIdAsync_WithInvalidId_ReturnsNull()
        {
            // Arrange
            _mockRepository.Setup(r => r.GetByIdAsync(999))
                .ReturnsAsync((Category?)null);

            // Act
            var result = await _service.GetCategoryByIdAsync(999);

            // Assert
            Assert.Null(result);
            _mockRepository.Verify(r => r.GetByIdAsync(999), Times.Once);
        }

        [Fact]
        public async Task CreateCategoryAsync_CreatesAndReturnsCategory()
        {
            // Arrange
            var createDto = new CreateCategoryDto
            {
                Name = "New Category",
                Description = "New Description"
            };
            var createdCategory = new Category
            {
                Id = 5,
                Name = "New Category",
                Description = "New Description"
            };
            _mockRepository.Setup(r => r.AddAsync(It.IsAny<Category>()))
                .ReturnsAsync(createdCategory);

            // Act
            var result = await _service.CreateCategoryAsync(createDto);

            // Assert
            Assert.Equal(5, result.Id);
            Assert.Equal("New Category", result.Name);
            Assert.Equal("New Description", result.Description);
            _mockRepository.Verify(r => r.AddAsync(It.Is<Category>(c =>
                c.Name == "New Category" &&
                c.Description == "New Description")), Times.Once);
        }

        [Fact]
        public async Task UpdateCategoryAsync_WithValidId_UpdatesCategory()
        {
            // Arrange
            var updateDto = new UpdateCategoryDto
            {
                Id = 1,
                Name = "Updated Name",
                Description = "Updated Description"
            };
            var existingCategory = new Category { Id = 1, Name = "Old Name", Description = "Old Desc" };
            var updatedCategory = new Category { Id = 1, Name = "Updated Name", Description = "Updated Description" };

            _mockRepository.Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(existingCategory);
            _mockRepository.Setup(r => r.UpdateAsync(It.IsAny<Category>()))
                .ReturnsAsync(updatedCategory);

            // Act
            var result = await _service.UpdateCategoryAsync(updateDto);

            // Assert
            Assert.Equal(1, result.Id);
            Assert.Equal("Updated Name", result.Name);
            Assert.Equal("Updated Description", result.Description);
            _mockRepository.Verify(r => r.GetByIdAsync(1), Times.Once);
            _mockRepository.Verify(r => r.UpdateAsync(It.Is<Category>(c =>
                c.Id == 1 &&
                c.Name == "Updated Name" &&
                c.Description == "Updated Description")), Times.Once);
        }

        [Fact]
        public async Task UpdateCategoryAsync_WithInvalidId_ThrowsKeyNotFoundException()
        {
            // Arrange
            var updateDto = new UpdateCategoryDto { Id = 999, Name = "Test" };
            _mockRepository.Setup(r => r.GetByIdAsync(999))
                .ReturnsAsync((Category?)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(async () =>
                await _service.UpdateCategoryAsync(updateDto));
            _mockRepository.Verify(r => r.UpdateAsync(It.IsAny<Category>()), Times.Never);
        }

        [Fact]
        public async Task DeleteCategoryAsync_CallsRepositoryDelete()
        {
            // Arrange
            _mockRepository.Setup(r => r.DeleteAsync(1))
                .ReturnsAsync(true);

            // Act
            await _service.DeleteCategoryAsync(1);

            // Assert
            _mockRepository.Verify(r => r.DeleteAsync(1), Times.Once);
        }

        [Fact]
        public async Task CategoryExistsAsync_WithExistingId_ReturnsTrue()
        {
            // Arrange
            _mockRepository.Setup(r => r.ExistsAsync(1))
                .ReturnsAsync(true);

            // Act
            var result = await _service.CategoryExistsAsync(1);

            // Assert
            Assert.True(result);
            _mockRepository.Verify(r => r.ExistsAsync(1), Times.Once);
        }

        [Fact]
        public async Task CategoryExistsAsync_WithNonExistentId_ReturnsFalse()
        {
            // Arrange
            _mockRepository.Setup(r => r.ExistsAsync(999))
                .ReturnsAsync(false);

            // Act
            var result = await _service.CategoryExistsAsync(999);

            // Assert
            Assert.False(result);
            _mockRepository.Verify(r => r.ExistsAsync(999), Times.Once);
        }
    }
}
