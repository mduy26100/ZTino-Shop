using Application.Features.Products.DTOs.Categories;

namespace Application.Features.Products.Commands.Categories.CreateCategory
{
    public record CreateCategoryCommand(UpsertCategoryDto Dto) : IRequest<UpsertCategoryDto>;
}
