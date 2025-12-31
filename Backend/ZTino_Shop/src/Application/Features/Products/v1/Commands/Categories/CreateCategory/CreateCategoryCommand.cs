using Application.Features.Products.v1.DTOs.Categories;

namespace Application.Features.Products.v1.Commands.Categories.CreateCategory
{
    public record CreateCategoryCommand(UpsertCategoryDto Dto) : IRequest<UpsertCategoryDto>;
}
