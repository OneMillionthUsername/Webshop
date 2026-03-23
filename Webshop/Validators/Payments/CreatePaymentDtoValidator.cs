using FluentValidation;
using Webshop.Dtos.Payments;

namespace Webshop.Validators.Payments
{
	public class CreatePaymentDtoValidator : AbstractValidator<CreatePaymentDto>
	{
		public CreatePaymentDtoValidator()
		{
			RuleFor(x => x.OrderId).GreaterThan(0);
			RuleFor(x => x.Amount).GreaterThan(0).LessThanOrEqualTo(9_999_999_999_999_999.99m);
			RuleFor(x => x.PaymentMethod).NotEmpty().MaximumLength(50);
			RuleFor(x => x.Token).NotEmpty();
		}
	}
}
