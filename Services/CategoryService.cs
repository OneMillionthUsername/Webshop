using Microsoft.EntityFrameworkCore;
using Webshop.Data;
using Webshop.Dtos.Categories;
using Webshop.Models;

namespace Webshop.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ApplicationDbContext _context;
        
        public CategoryService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync()
        {
            return await _context.Categories
                .Select(c => new CategoryDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description
                })
                .ToListAsync();
        }

        public async Task<CategoryDto?> GetCategoryByIdAsync(int categoryId)
        {
            var category = await _context.Categories.FindAsync(categoryId);
            
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
                Description = createDto.Description
            };

            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            return new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description
            };
        }

        public async Task<CategoryDto> UpdateCategoryAsync(UpdateCategoryDto updateDto)
        {
            var category = await _context.Categories.FindAsync(updateDto.Id);
            
            if (category == null)
                throw new KeyNotFoundException($"Category with ID {updateDto.Id} not found.");

            category.Name = updateDto.Name;
            category.Description = updateDto.Description ?? string.Empty;

            _context.Categories.Update(category);
            await _context.SaveChangesAsync();

            return new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description
            };
        }

        public async Task DeleteCategoryAsync(int categoryId)
        {
            var category = await _context.Categories.FindAsync(categoryId);
            
            if (category != null)
            {
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> CategoryExistsAsync(int categoryId)
        {
            return await _context.Categories.AnyAsync(e => e.Id == categoryId);
        }
    }
}
