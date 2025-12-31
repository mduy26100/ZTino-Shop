using Application.Features.Products.v1.DTOs.Categories;

namespace Application.Features.Products.v1.Queries.Categories.GetAllCategories
{
    public record GetAllCategoriesQuery() : IRequest<List<CategoryTreeDto>>;
}
