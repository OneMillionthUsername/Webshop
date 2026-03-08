using AutoMapper;
using Webshop.Dtos.Categories;
using Webshop.Models;

namespace Webshop.Mapping
{
	public class MappingProfile : Profile
	{
		public MappingProfile() {
			CreateMap<Category, CategoryDto>();
			CreateMap<CreateCategoryDto, Category>();
			CreateMap<UpdateCategoryDto, Category>();
		}
	}
}
