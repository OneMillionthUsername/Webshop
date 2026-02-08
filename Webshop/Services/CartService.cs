using Microsoft.EntityFrameworkCore;
using Webshop.Data;
using Webshop.Models;

namespace Webshop.Services
{
    public class CartService : ICartService
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CartService(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task AddToCartAsync(int productId, int quantity, int? variantId = null)
        {
            var variant = await _context.ProductVariants
                .FirstOrDefaultAsync(v => v.ProductId == productId && (variantId == null || v.Id == variantId))
                ?? throw new KeyNotFoundException("Product variant not found.");

            if (variant.StockQuantity < quantity)
                throw new InvalidOperationException("Insufficient stock.");

            var cartItem = new OrderItem
            {
                ProductVariantId = variant.Id,
                Quantity = quantity,
                PriceAtPurchase = await CalculateVariantPriceAsync(variant)
            };

            // Temporärer Warenkorb in Session (vereinfacht)
            // In Produktion: Datenbank mit CartId
            await _context.OrderItems.AddAsync(cartItem);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveFromCartAsync(int cartItemId)
        {
            var cartItem = await _context.OrderItems.FindAsync(cartItemId);
            if (cartItem != null)
            {
                _context.OrderItems.Remove(cartItem);
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdateQuantityAsync(int cartItemId, int quantity)
        {
            var cartItem = await _context.OrderItems.FindAsync(cartItemId);
            if (cartItem != null)
            {
                cartItem.Quantity = quantity;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<OrderItem>> GetCartItemsAsync()
        {
            // Vereinfacht: Alle OrderItems ohne Order (Warenkorb)
            return await _context.OrderItems
                .Where(oi => EF.Property<int?>(oi, "OrderId") == null)
                .ToListAsync();
        }

        public async Task ClearCartAsync()
        {
            var cartItems = await GetCartItemsAsync();
            _context.OrderItems.RemoveRange(cartItems);
            await _context.SaveChangesAsync();
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
                var variant = await _context.ProductVariants.FindAsync(item.ProductVariantId);
                if (variant == null || variant.StockQuantity < item.Quantity)
                    return false;
            }
            return true;
        }

        private async Task<decimal> CalculateVariantPriceAsync(ProductVariant variant)
        {
            var product = await _context.Products.FindAsync(variant.ProductId);
            return product!.BasePrice + variant.PriceAdjustment;
        }
    }
}
