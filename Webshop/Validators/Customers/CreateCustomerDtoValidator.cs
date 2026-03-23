using FluentValidation;
using Webshop.Dtos.Customers;

namespace Webshop.Validators.Customers
{
	public class CreateCustomerDtoValidator : AbstractValidator<CreateCustomerDto>
	{
		public CreateCustomerDtoValidator()
		{
			RuleFor(x => x.FirstName).NotNull().Length(2, 50);
			RuleFor(x => x.LastName).NotNull().Length(2, 50);
			RuleFor(x => x.Email).NotEmpty().EmailAddress();
			RuleFor(x => x.PhoneNumber).NotEmpty().MaximumLength(20);
			RuleFor(x => x.Address).NotEmpty().MaximumLength(255);
			RuleFor(x => x.City).NotEmpty().MaximumLength(100);
			RuleFor(x => x.PostalCode).NotEmpty().MaximumLength(10);
		}
	}
}
