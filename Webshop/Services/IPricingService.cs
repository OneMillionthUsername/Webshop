using Webshop.Models;

namespace Webshop.Services
{
    public interface IPricingService
    {
		decimal CalculateDiscount(Product product, string couponCode);
	}
}
