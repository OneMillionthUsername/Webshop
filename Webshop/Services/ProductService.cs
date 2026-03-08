using AutoMapper;
using Webshop.Dtos.Products;
using Webshop.Models;
using Webshop.Repositories;

namespace Webshop.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public ProductService(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProductDto>> GetAllProductsAsync()
        {
            var products = await _productRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<ProductDto>>(products);
        }

        public async Task<ProductDto?> GetProductByIdAsync(int productId)
        {
            var product = await _productRepository.GetByIdAsync(productId);

            if (product == null)
                return null;

            return _mapper.Map<ProductDto>(product);
        }

        public async Task<ProductDto> CreateProductAsync(CreateProductDto createDto)
        {
            var product = _mapper.Map<Product>(createDto);

            var created = await _productRepository.AddAsync(product);

            return _mapper.Map<ProductDto>(created);
        }

        public async Task<ProductDto> UpdateProductAsync(UpdateProductDto updateDto)
        {
            var product = await _productRepository.GetByIdAsync(updateDto.Id);

            if (product == null)
                throw new KeyNotFoundException($"Product with ID {updateDto.Id} not found.");

            _mapper.Map(updateDto, product);

            var updated = await _productRepository.UpdateAsync(product);

            return _mapper.Map<ProductDto>(updated);
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
            return _mapper.Map<IEnumerable<ProductDto>>(products);
        }

        public async Task<IEnumerable<ProductDto>> SearchProductsAsync(string searchTerm)
        {
            var products = await _productRepository.SearchAsync(searchTerm);
            return _mapper.Map<IEnumerable<ProductDto>>(products);
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
            
            return _mapper.Map<IEnumerable<ProductDto>>(featured);
        }
    }
}
