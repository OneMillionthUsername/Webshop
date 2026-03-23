using FluentValidation;
using Webshop.Dtos.Orders;

namespace Webshop.Validators.Orders
{
	public class CreateOrderItemDtoValidator : AbstractValidator<CreateOrderItemDto>
	{
		public CreateOrderItemDtoValidator()
		{
			RuleFor(x => x.ProductVariantId).GreaterThan(0);
			RuleFor(x => x.Quantity).GreaterThan(0).LessThan(10000);
		}
	}
}
