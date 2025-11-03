using Application.Features.Products.DTOs.Categories;

namespace Application.Features.Products.Commands.Categories.UpdateCategory
{
    public record UpdateCategoryCommand(UpsertCategoryDto Dto) : IRequest<UpsertCategoryDto>;
}
