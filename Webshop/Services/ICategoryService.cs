using Webshop.Dtos.Categories;
using Webshop.Models;

namespace Webshop.Services
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync();
        Task<CategoryDto?> GetCategoryByIdAsync(int categoryId);
        Task<CategoryDto> CreateCategoryAsync(CreateCategoryDto createDto);
        Task<CategoryDto> UpdateCategoryAsync(UpdateCategoryDto updateDto);
        Task<bool> DeleteCategoryAsync(int categoryId);
        Task<bool> CategoryExistsAsync(int categoryId);
    }
}
