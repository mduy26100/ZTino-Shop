using Application.Features.Products.v1.DTOs.Categories;
using Domain.Models.Products;

namespace Application.Features.Products.v1.Mappings
{
    public class CategoryMappingProfile : Profile
    {
        public CategoryMappingProfile()
        {
            CreateMap<Category, UpsertCategoryDto>().ReverseMap();
            CreateMap<Category, CategoryTreeDto>().ReverseMap();
        }
    }
}
