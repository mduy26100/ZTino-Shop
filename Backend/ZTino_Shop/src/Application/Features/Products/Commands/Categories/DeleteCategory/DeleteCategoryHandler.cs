using Application.Common.Interfaces.Persistence.EFCore;
using Application.Features.Products.Repositories;

namespace Application.Features.Products.Commands.Categories.DeleteCategory
{
    public class DeleteCategoryHandler : IRequestHandler<DeleteCategoryCommand, Unit>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IApplicationDbContext _context;

        public DeleteCategoryHandler(
            ICategoryRepository categoryRepository,
            IApplicationDbContext context)
        {
            _categoryRepository = categoryRepository;
            _context = context;
        }

        public async Task<Unit> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            var entity = await _categoryRepository.GetByIdAsync(request.Id, cancellationToken);

            if (entity == null)
                throw new KeyNotFoundException($"Category with id {request.Id} not found.");

            _categoryRepository.Remove(entity);
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
