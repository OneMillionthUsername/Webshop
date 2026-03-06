using Webshop.Models;
using Webshop.Repositories;

namespace Webshop.Services
{
    public class CartService : ICartService
    {
        private readonly IOrderItemRepository _orderItemRepository;
        private readonly IProductVariantRepository _productVariantRepository;
        private readonly IProductRepository _productRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CartService(
            IOrderItemRepository orderItemRepository,
            IProductVariantRepository productVariantRepository,
            IProductRepository productRepository,
            IHttpContextAccessor httpContextAccessor)
        {
            _orderItemRepository = orderItemRepository;
            _productVariantRepository = productVariantRepository;
            _productRepository = productRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task AddToCartAsync(int productId, int quantity, int? variantId = null)
        {
            var variant = await _productVariantRepository.GetByProductIdAndVariantIdAsync(productId, variantId)
                ?? throw new KeyNotFoundException("Product variant not found.");

            if (variant.StockQuantity < quantity)
                throw new InvalidOperationException("Insufficient stock.");

            var cartItem = new OrderItem
            {
                ProductVariantId = variant.Id,
                Quantity = quantity,
                PriceAtPurchase = await CalculateVariantPriceAsync(variant)
            };

            await _orderItemRepository.AddAsync(cartItem);
        }

        public async Task RemoveFromCartAsync(int cartItemId)
        {
            await _orderItemRepository.DeleteAsync(cartItemId);
        }

        public async Task UpdateQuantityAsync(int cartItemId, int quantity)
        {
            var cartItem = await _orderItemRepository.GetByIdAsync(cartItemId);
            if (cartItem != null)
            {
                cartItem.Quantity = quantity;
                await _orderItemRepository.UpdateAsync(cartItem);
            }
        }

        public async Task<IEnumerable<OrderItem>> GetCartItemsAsync()
        {
            return await _orderItemRepository.GetCartItemsAsync();
        }

        public async Task ClearCartAsync()
        {
            var cartItems = await GetCartItemsAsync();
            foreach (var item in cartItems)
                await _orderItemRepository.DeleteAsync(item.Id);
        }

        public async Task<decimal> CalculateCartTotalAsync()
        {
            var cartItems = await GetCartItemsAsync();
            return cartItems.Sum(item => item.PriceAtPurchase * item.Quantity);
        }

        public async Task<bool> ValidateCartAsync()
        {
            var cartItems = await GetCartItemsAsync();
            foreach (var item in cartItems)
            {
                var variant = await _productVariantRepository.GetByIdAsync(item.ProductVariantId);
                if (variant == null || variant.StockQuantity < item.Quantity)
                    return false;
            }
            return true;
        }

        private async Task<decimal> CalculateVariantPriceAsync(ProductVariant variant)
        {
            var product = await _productRepository.GetByIdAsync(variant.ProductId);
            return product!.BasePrice + variant.PriceAdjustment;
        }
    }
}
