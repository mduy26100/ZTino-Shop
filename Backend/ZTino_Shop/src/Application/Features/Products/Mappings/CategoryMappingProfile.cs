using Application.Features.Products.DTOs.Categories;
using Domain.Models.Products;

namespace Application.Features.Products.Mappings
{
    public class CategoryMappingProfile : Profile
    {
        public CategoryMappingProfile()
        {
            CreateMap<Category, UpsertCategoryDto>().ReverseMap();
        }
    }
}
