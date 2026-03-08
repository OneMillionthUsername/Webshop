using AutoMapper;
using Webshop.Dtos.Categories;
using Webshop.Models;
using Webshop.Repositories;

namespace Webshop.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _repository;
        private readonly IMapper _mapper;

        public CategoryService(ICategoryRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync()
        {
            var categories = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<CategoryDto>>(categories);
        }

        public async Task<CategoryDto?> GetCategoryByIdAsync(int categoryId)
        {
            var category = await _repository.GetByIdAsync(categoryId);
            
            if (category == null)
                return null;

            return _mapper.Map<CategoryDto>(category);
        }

        public async Task<CategoryDto> CreateCategoryAsync(CreateCategoryDto createDto)
        {
            var category = _mapper.Map<Category>(createDto);

            var created = await _repository.AddAsync(category);

            return _mapper.Map<CategoryDto>(created);
        }

        public async Task<CategoryDto> UpdateCategoryAsync(UpdateCategoryDto updateDto)
        {
            var category = await _repository.GetByIdAsync(updateDto.Id);
            
            if (category == null)
                throw new KeyNotFoundException($"Category with ID {updateDto.Id} not found.");

            _mapper.Map(updateDto, category);

            var updated = await _repository.UpdateAsync(category);

            return _mapper.Map<CategoryDto>(updated);
        }

        public async Task<bool> DeleteCategoryAsync(int categoryId)
        {
            return await _repository.DeleteAsync(categoryId);
        }

        public async Task<bool> CategoryExistsAsync(int categoryId)
        {
            return await _repository.ExistsAsync(categoryId);
        }
    }
}
