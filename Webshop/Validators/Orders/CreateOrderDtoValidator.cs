using FluentValidation;
using Webshop.Dtos.Orders;

namespace Webshop.Validators.Orders
{
	public class CreateOrderDtoValidator : AbstractValidator<CreateOrderDto>
	{
		public CreateOrderDtoValidator()
		{
			RuleFor(x => x.CustomerId).GreaterThan(0);
			RuleFor(x => x.Items).NotEmpty();
			RuleForEach(x => x.Items).SetValidator(new CreateOrderItemDtoValidator());
		}
	}
}
