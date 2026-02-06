using Webshop.Dtos.Categories;
using Webshop.Dtos.Products;
using Webshop.Models;
using Webshop.Repositories;

namespace Webshop.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;

        public ProductService(IProductRepository productRepository, ICategoryRepository categoryRepository)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
        }

        public async Task<IEnumerable<ProductDto>> GetAllProductsAsync()
        {
            var products = await _productRepository.GetAllAsync();
            return products.Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                BasePrice = p.BasePrice,
                CategoryId = p.CategoryId
            });
        }

        public async Task<ProductDto?> GetProductByIdAsync(int productId)
        {
            var product = await _productRepository.GetByIdAsync(productId);

            if (product == null)
                return null;

            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                BasePrice = product.BasePrice,
                CategoryId = product.CategoryId
            };
        }

        public async Task<ProductDto> CreateProductAsync(CreateProductDto createDto)
        {
            var product = new Product
            {
                Name = createDto.Name,
                Description = createDto.Description,
                BasePrice = createDto.BasePrice,
                CategoryId = createDto.CategoryId
            };

            var created = await _productRepository.AddAsync(product);

            return new ProductDto
            {
                Id = created.Id,
                Name = created.Name,
                Description = created.Description,
                BasePrice = created.BasePrice,
                CategoryId = created.CategoryId
            };
        }

        public async Task<ProductDto> UpdateProductAsync(UpdateProductDto updateDto)
        {
            var product = await _productRepository.GetByIdAsync(updateDto.Id);

            if (product == null)
                throw new KeyNotFoundException($"Product with ID {updateDto.Id} not found.");

            product.Name = updateDto.Name;
            product.Description = updateDto.Description;
            product.BasePrice = updateDto.BasePrice;
            product.CategoryId = updateDto.CategoryId;

            var updated = await _productRepository.UpdateAsync(product);

            return new ProductDto
            {
                Id = updated.Id,
                Name = updated.Name,
                Description = updated.Description,
                BasePrice = updated.BasePrice,
                CategoryId = updated.CategoryId
            };
        }

        public async Task DeleteProductAsync(int productId)
        {
            await _productRepository.DeleteAsync(productId);
        }

        public async Task<bool> ProductExistsAsync(int productId)
        {
            return await _productRepository.ExistsAsync(productId);
        }

        public async Task<IEnumerable<ProductDto>> GetProductsByCategoryAsync(int categoryId)
        {
            var products = await _productRepository.GetByCategoryIdAsync(categoryId);
            return products.Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                BasePrice = p.BasePrice,
                CategoryId = p.CategoryId
            });
        }

        public async Task<IEnumerable<ProductDto>> SearchProductsAsync(string searchTerm)
        {
            var products = await _productRepository.SearchAsync(searchTerm);
            return products.Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                BasePrice = p.BasePrice,
                CategoryId = p.CategoryId
            });
        }

        public async Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync()
        {
            var categories = await _categoryRepository.GetAllAsync();
            return categories.Select(c => new CategoryDto
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description
            });
        }

        public async Task<bool> CheckInventoryAsync(int productId, int quantity)
        {
            var availableStock = await GetAvailableStockAsync(productId);
            return availableStock >= quantity;
        }

        public async Task<int> GetAvailableStockAsync(int productId)
        {
            return await _productRepository.GetAvailableStockAsync(productId);
        }

        public async Task<IEnumerable<ProductDto>> GetFeaturedProductsAsync()
        {
            var allProducts = await _productRepository.GetAllAsync();
            var featured = allProducts.Take(10);
            
            return featured.Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                BasePrice = p.BasePrice,
                CategoryId = p.CategoryId
            });
        }
    }
}
