using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Webshop.Controllers;
using Webshop.Data;
using Webshop.Models;
using Webshop.Dtos.Categories;
using Webshop.Services;
using Microsoft.EntityFrameworkCore.InMemory;
using Microsoft.Identity.Client;
using Moq;

namespace Tests_BL_Backend.Controllers
{
    public class CategoriesControllerTests
    {
        private readonly Mock<ICategoryService> _mockCategoryService;
        private readonly CategoriesController _controller;
        
        public CategoriesControllerTests()
        {
            _mockCategoryService = new Mock<ICategoryService>();
            _controller = new CategoriesController(_mockCategoryService.Object);
        }
        
        public static IEnumerable<object[]> GetInvalidCategoryIds()
        {
            // Äquivalenzklasse: Alle Werte < 1
            yield return new object[] { -100 };
            yield return new object[] { -1 };
            yield return new object[] { int.MinValue };
            yield return new object[] { 0 };

            // Äquivalenzklasse: Null
            yield return new object[] { null };

            // Äquivalenzklasse: Nicht existente Werte
            yield return new object[] { 999 };
            yield return new object[] { 500000 };
            yield return new object[] { int.MaxValue };
        }
        
        [Fact]
        public async Task Index_ReturnsViewResult_WithListOfCategories()
        {
            // Arrange
            var categories = new List<CategoryDto>
            {
                new CategoryDto { Id = 1, Name = "Electronics" },
                new CategoryDto { Id = 2, Name = "Books" },
                new CategoryDto { Id = 3, Name = "Shoes", Description = "Eine Vielzahl an Schuhe." },
                new CategoryDto { Id = 4, Name = "Clothing", Description = "Eine Vielzahl an Kleidung." }
            };
            _mockCategoryService.Setup(s => s.GetAllCategoriesAsync())
                .ReturnsAsync(categories);
            
            // Act
            var result = await _controller.Index();
            
            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<CategoryDto>>(viewResult.Model);
            Assert.Equal(4, model.Count());
            _mockCategoryService.Verify(s => s.GetAllCategoriesAsync(), Times.Once);
        }
        
        [Fact]
        public async Task Details_ReturnViewResult_WithValidId()
        {
            // Arrange
            int testCategoryId = 1;
            var category = new CategoryDto { Id = 1, Name = "Electronics" };
            _mockCategoryService.Setup(s => s.GetCategoryByIdAsync(testCategoryId))
                .ReturnsAsync(category);
            
            // Act
            var result = await _controller.Details(testCategoryId);
            
            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<CategoryDto>(viewResult.Model);
            Assert.Equal(testCategoryId, model.Id);
            _mockCategoryService.Verify(s => s.GetCategoryByIdAsync(testCategoryId), Times.Once);
        }
        
        [Theory]
        [MemberData(nameof(GetInvalidCategoryIds))]
        public async Task Details_ReturnNotFoundResult_WithInvalidId(int? invalidCategoryId)
        {
            // Arrange
            _mockCategoryService.Setup(s => s.GetCategoryByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((CategoryDto?)null);
            
            // Act
            var result = await _controller.Details(invalidCategoryId);
            
            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
        
        [Fact]
        public async Task Create_Post_ValidCategory_RedirectsToIndex()
        {
            // Arrange
            var createDto = new CreateCategoryDto
            {
                Name = "New Category",
                Description = "Description of new category"
            };
            var createdCategory = new CategoryDto
            {
                Id = 5,
                Name = "New Category",
                Description = "Description of new category"
            };
            _mockCategoryService.Setup(s => s.CreateCategoryAsync(createDto))
                .ReturnsAsync(createdCategory);
            
            // Act
            var result = await _controller.Create(createDto);
            
            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
            _mockCategoryService.Verify(s => s.CreateCategoryAsync(createDto), Times.Once);
        }
        
        [Theory]
        [InlineData("", "Description of category")]
        [InlineData(null, "Description of category")]
        public async Task Create_Post_InvalidCategory_ReturnsView(string name, string description)
        {
            // Arrange
            var createDto = new CreateCategoryDto
            {
                Name = name,
                Description = description
            };
            _controller.ModelState.AddModelError("Name", "Name is required");
            
            // Act
            var result = await _controller.Create(createDto);
            
            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<CreateCategoryDto>(viewResult.Model);
            Assert.Equal(name, model.Name);
            Assert.Equal(description, model.Description);
            _mockCategoryService.Verify(s => s.CreateCategoryAsync(It.IsAny<CreateCategoryDto>()), Times.Never);
        }
        
        [Fact]
        public async Task Create_DoesNotSaveWithInvalidModelState()
        {
            // Arrange
            var createDto = new CreateCategoryDto { Name = "Test" };
            _controller.ModelState.AddModelError("Name", "Invalid name");
            
            // Act
            await _controller.Create(createDto);
            
            // Assert
            _mockCategoryService.Verify(s => s.CreateCategoryAsync(It.IsAny<CreateCategoryDto>()), Times.Never);
        }
        
        [Fact]
        public async Task Edit_Get_WithValidId_ReturnsUpdateDto()
        {
            // Arrange
            var category = new CategoryDto { Id = 1, Name = "Electronics", Description = "" };
            _mockCategoryService.Setup(s => s.GetCategoryByIdAsync(1))
                .ReturnsAsync(category);
            
            // Act
            var result = await _controller.Edit(1);
            
            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<UpdateCategoryDto>(viewResult.Model);
            Assert.Equal(1, model.Id);
            Assert.Equal("Electronics", model.Name);
        }
        
        [Fact]
        public async Task Edit_Get_WithInvalidId_ReturnsNotFound()
        {
            // Arrange
            _mockCategoryService.Setup(s => s.GetCategoryByIdAsync(999))
                .ReturnsAsync((CategoryDto?)null);
            
            // Act
            var result = await _controller.Edit(999);
            
            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
        
        [Fact]
        public async Task Edit_Post_WithValidData_UpdatesCategory()
        {
            // Arrange
            var updateDto = new UpdateCategoryDto
            {
                Id = 1,
                Name = "Updated Electronics",
                Description = "Updated Description"
            };
            var updatedCategory = new CategoryDto
            {
                Id = 1,
                Name = "Updated Electronics",
                Description = "Updated Description"
            };
            _mockCategoryService.Setup(s => s.UpdateCategoryAsync(updateDto))
                .ReturnsAsync(updatedCategory);
            
            // Act
            var result = await _controller.Edit(1, updateDto);
            
            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);
            _mockCategoryService.Verify(s => s.UpdateCategoryAsync(updateDto), Times.Once);
        }
        
        [Fact]
        public async Task Edit_Post_WithMismatchedIds_ReturnsNotFound()
        {
            // Arrange
            var updateDto = new UpdateCategoryDto { Id = 1, Name = "Test" };
            
            // Act
            var result = await _controller.Edit(2, updateDto);
            
            // Assert
            Assert.IsType<NotFoundResult>(result);
            _mockCategoryService.Verify(s => s.UpdateCategoryAsync(It.IsAny<UpdateCategoryDto>()), Times.Never);
        }
        
        [Fact]
        public async Task Edit_Post_WithInvalidModelState_ReturnsView()
        {
            // Arrange
            var updateDto = new UpdateCategoryDto { Id = 1, Name = "" };
            _controller.ModelState.AddModelError("Name", "Required");
            
            // Act
            var result = await _controller.Edit(1, updateDto);
            
            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            _mockCategoryService.Verify(s => s.UpdateCategoryAsync(It.IsAny<UpdateCategoryDto>()), Times.Never);
        }
        
        [Fact]
        public async Task Delete_Get_WithValidId_ReturnsCategory()
        {
            // Arrange
            var category = new CategoryDto { Id = 1, Name = "Electronics" };
            _mockCategoryService.Setup(s => s.GetCategoryByIdAsync(1))
                .ReturnsAsync(category);
            
            // Act
            var result = await _controller.Delete(1);
            
            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<CategoryDto>(viewResult.Model);
            Assert.Equal(1, model.Id);
        }
        
        [Fact]
        public async Task Delete_Get_WithInvalidId_ReturnsNotFound()
        {
            // Arrange
            _mockCategoryService.Setup(s => s.GetCategoryByIdAsync(999))
                .ReturnsAsync((CategoryDto?)null);
            
            // Act
            var result = await _controller.Delete(999);
            
            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
        
        [Fact]
        public async Task DeleteConfirmed_RemovesCategory()
        {
            // Arrange
            _mockCategoryService.Setup(s => s.DeleteCategoryAsync(1))
                .ReturnsAsync(true);
            
            // Act
            var result = await _controller.DeleteConfirmed(1);
            
            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);
            _mockCategoryService.Verify(s => s.DeleteCategoryAsync(1), Times.Once);
        }
    }
}
