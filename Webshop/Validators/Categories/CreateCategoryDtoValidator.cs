using FluentValidation;
using Webshop.Dtos.Categories;

namespace Webshop.Validators.Categories
{
	public class CreateCategoryDtoValidator : AbstractValidator<CreateCategoryDto>
	{
		public CreateCategoryDtoValidator()
		{
			RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
			RuleFor(x => x.Description).MaximumLength(500);
		}
	}
}
