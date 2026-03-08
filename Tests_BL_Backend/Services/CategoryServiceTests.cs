using AutoMapper;
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
        private readonly Mock<IMapper> _mockMapper;
        private readonly CategoryService _service;

        public CategoryServiceTests()
        {
            _mockMapper = new Mock<IMapper>();
            _mockRepository = new Mock<ICategoryRepository>();
            _service = new CategoryService(_mockRepository.Object, _mockMapper.Object);
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
            var categoryDtos = new List<CategoryDto>
            {
                new CategoryDto { Id = 1, Name = "Electronics" },
                new CategoryDto { Id = 2, Name = "Books" }
            };
            _mockRepository.Setup(r => r.GetAllAsync())
                .ReturnsAsync(categories);
            _mockMapper.Setup(m => m.Map<IEnumerable<CategoryDto>>(It.IsAny<object>()))
                .Returns(categoryDtos);

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
            var categoryDto = new CategoryDto { Id = 1, Name = "Electronics" };
            _mockRepository.Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(category);
            _mockMapper.Setup(m => m.Map<CategoryDto>(It.IsAny<object>()))
                .Returns(categoryDto);

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
            var mappedCategory = new Category { Name = "New Category", Description = "New Description" };
            var createdCategory = new Category { Id = 5, Name = "New Category", Description = "New Description" };
            var createdDto = new CategoryDto { Id = 5, Name = "New Category", Description = "New Description" };

            _mockMapper.Setup(m => m.Map<Category>(It.IsAny<CreateCategoryDto>()))
                .Returns(mappedCategory);
            _mockRepository.Setup(r => r.AddAsync(It.IsAny<Category>()))
                .ReturnsAsync(createdCategory);
            _mockMapper.Setup(m => m.Map<CategoryDto>(It.IsAny<object>()))
                .Returns(createdDto);

            // Act
            var result = await _service.CreateCategoryAsync(createDto);

            // Assert
            Assert.Equal(5, result.Id);
            Assert.Equal("New Category", result.Name);
            Assert.Equal("New Description", result.Description);
            _mockRepository.Verify(r => r.AddAsync(It.IsAny<Category>()), Times.Once);
        }

        [Fact]
        public async Task UpdateCategoryAsync_WithValidId_UpdatesCategory()
        {
            // Arrange
            var updateDto = new UpdateCategoryDto { Id = 1, Name = "Updated Name", Description = "Updated Description" };
            var existingCategory = new Category { Id = 1, Name = "Old Name", Description = "Old Desc" };
            var updatedCategory = new Category { Id = 1, Name = "Updated Name", Description = "Updated Description" };
            var updatedDto = new CategoryDto { Id = 1, Name = "Updated Name", Description = "Updated Description" };

            _mockRepository.Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(existingCategory);
            _mockMapper.Setup(m => m.Map(It.IsAny<UpdateCategoryDto>(), It.IsAny<Category>()))
                .Returns(existingCategory);
            _mockRepository.Setup(r => r.UpdateAsync(It.IsAny<Category>()))
                .ReturnsAsync(updatedCategory);
            _mockMapper.Setup(m => m.Map<CategoryDto>(It.IsAny<object>()))
                .Returns(updatedDto);

            // Act
            var result = await _service.UpdateCategoryAsync(updateDto);

            // Assert
            Assert.Equal(1, result.Id);
            Assert.Equal("Updated Name", result.Name);
            Assert.Equal("Updated Description", result.Description);
            _mockRepository.Verify(r => r.GetByIdAsync(1), Times.Once);
            _mockRepository.Verify(r => r.UpdateAsync(It.IsAny<Category>()), Times.Once);
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
