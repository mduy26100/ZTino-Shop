using Application.Features.Products.DTOs.Categories;

namespace Application.Features.Products.Interfaces.Services.Commands.Categories.UpdateCategory
{
    public interface IUpdateCategoryService
    {
        Task<UpsertCategoryDto> UpdateAsync(UpsertCategoryDto category, CancellationToken cancellationToken = default);
    }
}
