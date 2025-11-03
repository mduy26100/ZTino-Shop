namespace Application.Features.Products.Interfaces.Services.Commands.Categories.DeleteCategory
{
    public interface IDeleteCategoryService
    {
        Task DeleteAsync(int Id, CancellationToken cancellationToken = default);
    }
}
