using Webshop.Models;

namespace Webshop.Services
{
    public class PricingService : IPricingService
    {
        public decimal CalculateDiscount(Product product, string couponCode)
        {
            if (string.IsNullOrWhiteSpace(couponCode))
                return 0;

            // Beispiel-Logik: Feste Rabatte
            return couponCode.ToUpper() switch
            {
                "SUMMER10" => product.BasePrice * 0.10m,
                "WELCOME20" => product.BasePrice * 0.20m,
                "VIP30" => product.BasePrice * 0.30m,
                _ => 0
            };
        }
    }
}
