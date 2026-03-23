using FluentValidation;
using Webshop.Dtos.Categories;

namespace Webshop.Validators.Categories
{
	public class UpdateCategoryDtoValidator : AbstractValidator<UpdateCategoryDto>
	{
		public UpdateCategoryDtoValidator()
		{
			RuleFor(x => x.Id).GreaterThan(0);
			RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
			RuleFor(x => x.Description).MaximumLength(500);
		}
	}
}
