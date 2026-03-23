using FluentValidation;
using Webshop.Dtos.Products;

namespace Webshop.Validators.Products
{
	public class UpdateProductDtoValidator : AbstractValidator<UpdateProductDto>
	{
		public UpdateProductDtoValidator()
		{
			RuleFor(x => x.Id).GreaterThan(0);
			RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
			RuleFor(x => x.Description).MaximumLength(500);
			RuleFor(x => x.BasePrice).GreaterThan(0).LessThanOrEqualTo(9_999_999_999_999_999.99m);
			RuleFor(x => x.CategoryId).GreaterThan(0);
		}
	}
}
