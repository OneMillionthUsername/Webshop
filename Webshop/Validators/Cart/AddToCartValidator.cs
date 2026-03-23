using FluentValidation;
using Webshop.Dtos.Cart;

namespace Webshop.Validators.Cart
{
	public class AddToCartValidator : AbstractValidator<AddToCartDto>
	{
		public AddToCartValidator()
		{
			RuleFor(x => x.ProductVariantId).GreaterThan(0);
			RuleFor(x => x.Quantity).GreaterThan(0).LessThan(10000);
		}
	}
}
