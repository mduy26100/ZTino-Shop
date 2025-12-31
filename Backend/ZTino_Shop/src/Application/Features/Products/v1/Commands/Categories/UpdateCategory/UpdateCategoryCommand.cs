using Application.Features.Products.v1.DTOs.Categories;

namespace Application.Features.Products.v1.Commands.Categories.UpdateCategory
{
    public record UpdateCategoryCommand(UpsertCategoryDto Dto) : IRequest<UpsertCategoryDto>;
}
