using Application.Features.Products.DTOs.Categories;

namespace Application.Features.Products.Interfaces.Services.Commands.Categories.CreateCategory
{
    public interface ICreateCategoryService
    {
        Task<UpsertCategoryDto> CreateAsync(UpsertCategoryDto category, CancellationToken cancellationToken = default);
    }
}
