using FluentValidation;
using Webshop.Dtos.Products;

namespace Webshop.Validators.Products
{
	public class CreateProductVariantDtoValidator : AbstractValidator<CreateProductVariantDto>
	{
		public CreateProductVariantDtoValidator()
		{
			RuleFor(x => x.ProductId).GreaterThan(0);
			RuleFor(x => x.SKU).NotEmpty().MaximumLength(100);
			RuleFor(x => x.Attributes).NotEmpty().MaximumLength(500);
			RuleFor(x => x.PriceAdjustment).LessThanOrEqualTo(9_999_999_999_999_999.99m);
			RuleFor(x => x.StockQuantity).GreaterThanOrEqualTo(0);
		}
	}
}
