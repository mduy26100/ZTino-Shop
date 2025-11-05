using Application.Features.Products.DTOs.Categories;

namespace Application.Features.Products.Queries.Categories.GetAllCategories
{
    public record GetAllCategoriesQuery() : IRequest<List<CategoryTreeDto>>;
}
