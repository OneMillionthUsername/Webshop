using Webshop.Dtos.Categories;
using Webshop.Models;
using Webshop.Repositories;

namespace Webshop.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _repository;

        public CategoryService(ICategoryRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync()
        {
            var categories = await _repository.GetAllAsync();
            return categories.Select(c => new CategoryDto
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description
            });
        }

        public async Task<CategoryDto?> GetCategoryByIdAsync(int categoryId)
        {
            var category = await _repository.GetByIdAsync(categoryId);
            
            if (category == null)
                return null;

            return new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description
            };
        }

        public async Task<CategoryDto> CreateCategoryAsync(CreateCategoryDto createDto)
        {
            var category = new Category
            {
                Name = createDto.Name,
                Description = createDto.Description ?? string.Empty
            };

            var created = await _repository.AddAsync(category);

            return new CategoryDto
            {
                Id = created.Id,
                Name = created.Name,
                Description = created.Description
            };
        }

        public async Task<CategoryDto> UpdateCategoryAsync(UpdateCategoryDto updateDto)
        {
            var category = await _repository.GetByIdAsync(updateDto.Id);
            
            if (category == null)
                throw new KeyNotFoundException($"Category with ID {updateDto.Id} not found.");

            category.Name = updateDto.Name;
            category.Description = updateDto.Description ?? string.Empty;

            var updated = await _repository.UpdateAsync(category);

            return new CategoryDto
            {
                Id = updated.Id,
                Name = updated.Name,
                Description = updated.Description
            };
        }

        public async Task DeleteCategoryAsync(int categoryId)
        {
            await _repository.DeleteAsync(categoryId);
        }

        public async Task<bool> CategoryExistsAsync(int categoryId)
        {
            return await _repository.ExistsAsync(categoryId);
        }
    }
}
