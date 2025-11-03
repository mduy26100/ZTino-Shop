using Application.Common.Interfaces.Persistence.EFCore;
using Application.Features.Products.Interfaces.Services.Commands.Categories.DeleteCategory;
using Application.Features.Products.Repositories;

namespace Application.Features.Products.Services.Commands.Categories.DeleteCategory
{
    public class DeleteCategoryService : IDeleteCategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IApplicationDbContext _context;

        public DeleteCategoryService(ICategoryRepository categoryRepository,
            IApplicationDbContext context)
        {
            _categoryRepository = categoryRepository;
            _context = context;
        }

        public async Task DeleteAsync(int Id, CancellationToken cancellationToken = default)
        {
            var entity = await _categoryRepository.GetByIdAsync(Id, cancellationToken);
            if (entity == null)
                throw new KeyNotFoundException($"Category with id {Id} not found.");

            _categoryRepository.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
