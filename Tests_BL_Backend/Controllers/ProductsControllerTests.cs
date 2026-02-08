using Microsoft.AspNetCore.Mvc;
using Moq;
using Webshop.Controllers;
using Webshop.Dtos.Categories;
using Webshop.Dtos.Products;
using Webshop.Services;

namespace Tests_BL_Backend.Controllers
{
    public class ProductsControllerTests
    {
        private readonly Mock<IProductService> _mockProductService;
        private readonly ProductsController _controller;

        public ProductsControllerTests()
        {
            _mockProductService = new Mock<IProductService>();
            _controller = new ProductsController(_mockProductService.Object);
        }

        [Fact]
        public async Task Index_ReturnsViewWithProducts()
        {
            // Arrange
            var products = new List<ProductDto>
            {
                new ProductDto { Id = 1, Name = "Product 1", Description = "Desc 1", BasePrice = 10.00m, CategoryId = 1 },
                new ProductDto { Id = 2, Name = "Product 2", Description = "Desc 2", BasePrice = 20.00m, CategoryId = 1 }
            };
            _mockProductService.Setup(s => s.GetAllProductsAsync())
                .ReturnsAsync(products);

            // Act
            var result = await _controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<ProductDto>>(viewResult.Model);
            Assert.Equal(2, model.Count());
            _mockProductService.Verify(s => s.GetAllProductsAsync(), Times.Once);
        }

        [Fact]
        public async Task Details_WithValidId_ReturnsProduct()
        {
            // Arrange
            var product = new ProductDto { Id = 1, Name = "Product 1", Description = "Desc 1", BasePrice = 10.00m, CategoryId = 1 };
            _mockProductService.Setup(s => s.GetProductByIdAsync(1))
                .ReturnsAsync(product);

            // Act
            var result = await _controller.Details(1);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<ProductDto>(viewResult.Model);
            Assert.Equal(1, model.Id);
            Assert.Equal("Product 1", model.Name);
            _mockProductService.Verify(s => s.GetProductByIdAsync(1), Times.Once);
        }

        [Fact]
        public async Task Details_WithNullId_ReturnsNotFound()
        {
            // Act
            var result = await _controller.Details(null);

            // Assert
            Assert.IsType<NotFoundResult>(result);
            _mockProductService.Verify(s => s.GetProductByIdAsync(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task Details_WithInvalidId_ReturnsNotFound()
        {
            // Arrange
            _mockProductService.Setup(s => s.GetProductByIdAsync(999))
                .ReturnsAsync((ProductDto?)null);

            // Act
            var result = await _controller.Details(999);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Create_Get_ReturnsViewWithCategories()
        {
            // Arrange
            var categories = new List<CategoryDto>
            {
                new CategoryDto { Id = 1, Name = "Category 1" },
                new CategoryDto { Id = 2, Name = "Category 2" }
            };
            _mockProductService.Setup(s => s.GetAllCategoriesAsync())
                .ReturnsAsync(categories);

            // Act
            var result = await _controller.Create();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.NotNull(viewResult.ViewData["CategoryId"]);
            _mockProductService.Verify(s => s.GetAllCategoriesAsync(), Times.Once);
        }

        [Fact]
        public async Task Create_WithValidProduct_RedirectsToIndex()
        {
            // Arrange
            var createDto = new CreateProductDto 
            { 
                Name = "New Product", 
                Description = "New Desc", 
                BasePrice = 15.00m, 
                CategoryId = 1 
            };
            var createdProduct = new ProductDto
            {
                Id = 3,
                Name = "New Product",
                Description = "New Desc",
                BasePrice = 15.00m,
                CategoryId = 1
            };
            _mockProductService.Setup(s => s.CreateProductAsync(createDto))
                .ReturnsAsync(createdProduct);

            // Act
            var result = await _controller.Create(createDto);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal(nameof(_controller.Index), redirectResult.ActionName);
            _mockProductService.Verify(s => s.CreateProductAsync(createDto), Times.Once);
        }

        [Fact]
        public async Task Create_WithInvalidModelState_ReturnsView()
        {
            // Arrange
            var createDto = new CreateProductDto { Name = "Test" };
            _controller.ModelState.AddModelError("BasePrice", "Required");
            var categories = new List<CategoryDto>
            {
                new CategoryDto { Id = 1, Name = "Category 1" }
            };
            _mockProductService.Setup(s => s.GetAllCategoriesAsync())
                .ReturnsAsync(categories);

            // Act
            var result = await _controller.Create(createDto);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsAssignableFrom<CreateProductDto>(viewResult.Model);
            _mockProductService.Verify(s => s.CreateProductAsync(It.IsAny<CreateProductDto>()), Times.Never);
        }

        [Fact]
        public async Task Edit_Get_WithValidId_ReturnsUpdateDto()
        {
            // Arrange
            var product = new ProductDto 
            { 
                Id = 1, 
                Name = "Product 1", 
                Description = "Desc 1", 
                BasePrice = 10.00m, 
                CategoryId = 1 
            };
            var categories = new List<CategoryDto>
            {
                new CategoryDto { Id = 1, Name = "Category 1" }
            };
            _mockProductService.Setup(s => s.GetProductByIdAsync(1))
                .ReturnsAsync(product);
            _mockProductService.Setup(s => s.GetAllCategoriesAsync())
                .ReturnsAsync(categories);

            // Act
            var result = await _controller.Edit(1);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<UpdateProductDto>(viewResult.Model);
            Assert.Equal(1, model.Id);
            Assert.Equal("Product 1", model.Name);
        }

        [Fact]
        public async Task Edit_Get_WithInvalidId_ReturnsNotFound()
        {
            // Arrange
            _mockProductService.Setup(s => s.GetProductByIdAsync(999))
                .ReturnsAsync((ProductDto?)null);

            // Act
            var result = await _controller.Edit(999);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Edit_Post_WithValidData_UpdatesProduct()
        {
            // Arrange
            var updateDto = new UpdateProductDto
            {
                Id = 1,
                Name = "Updated Product",
                Description = "Updated Desc",
                BasePrice = 25.00m,
                CategoryId = 1
            };
            var updatedProduct = new ProductDto
            {
                Id = 1,
                Name = "Updated Product",
                Description = "Updated Desc",
                BasePrice = 25.00m,
                CategoryId = 1
            };
            _mockProductService.Setup(s => s.UpdateProductAsync(updateDto))
                .ReturnsAsync(updatedProduct);

            // Act
            var result = await _controller.Edit(1, updateDto);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal(nameof(_controller.Index), redirectResult.ActionName);
            _mockProductService.Verify(s => s.UpdateProductAsync(updateDto), Times.Once);
        }

        [Fact]
        public async Task Edit_Post_WithMismatchedIds_ReturnsNotFound()
        {
            // Arrange
            var updateDto = new UpdateProductDto { Id = 1, Name = "Test", BasePrice = 10m, CategoryId = 1 };

            // Act
            var result = await _controller.Edit(2, updateDto);

            // Assert
            Assert.IsType<NotFoundResult>(result);
            _mockProductService.Verify(s => s.UpdateProductAsync(It.IsAny<UpdateProductDto>()), Times.Never);
        }

        [Fact]
        public async Task Delete_WithValidId_RemovesProduct()
        {
            // Arrange
            _mockProductService.Setup(s => s.DeleteProductAsync(1))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.DeleteConfirmed(1);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal(nameof(_controller.Index), redirectResult.ActionName);
            _mockProductService.Verify(s => s.DeleteProductAsync(1), Times.Once);
        }

        [Fact]
        public async Task ProductExists_WithExistingId_ReturnsTrue()
        {
            // Arrange
            _mockProductService.Setup(s => s.ProductExistsAsync(1))
                .ReturnsAsync(true);

            // Act & Assert
            var exists = await _mockProductService.Object.ProductExistsAsync(1);
            Assert.True(exists);
        }

        [Fact]
        public async Task ProductExists_WithNonExistentId_ReturnsFalse()
        {
            // Arrange
            _mockProductService.Setup(s => s.ProductExistsAsync(999))
                .ReturnsAsync(false);

            // Act & Assert
            var exists = await _mockProductService.Object.ProductExistsAsync(999);
            Assert.False(exists);
        }
    }
}
