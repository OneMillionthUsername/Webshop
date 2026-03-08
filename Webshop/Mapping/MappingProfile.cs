using AutoMapper;
using Webshop.Dtos.Categories;
using Webshop.Dtos.Products;
using Webshop.Models;

namespace Webshop.Mapping
{
	public class MappingProfile : Profile
	{
		public MappingProfile() {
			// Category mappings
			CreateMap<Category, CategoryDto>();
			CreateMap<CreateCategoryDto, Category>();
			CreateMap<UpdateCategoryDto, Category>();

			// Product mappings
			CreateMap<ProductVariant, ProductVariantDto>()
				.ForMember(
				dest => dest.Attributes,
				opt => opt.MapFrom(
				src => string.Join(", ", src.Attributes.Select(
					a => $"{a.AttributeName}: {a.AttributeValue}"))));
			CreateMap<Product, ProductDto>();
			CreateMap<CreateProductDto, Product>();
			CreateMap<UpdateProductDto, Product>();
		}
	}
}
