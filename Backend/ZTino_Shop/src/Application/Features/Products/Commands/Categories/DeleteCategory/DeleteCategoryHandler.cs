using Application.Common.Interfaces.Persistence.Data;
using Application.Features.Products.Repositories;

namespace Application.Features.Products.Commands.Categories.DeleteCategory
{
    public class DeleteCategoryHandler : IRequestHandler<DeleteCategoryCommand, Unit>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IProductRepository _productRepository;
        private readonly IApplicationDbContext _context;

        public DeleteCategoryHandler(
            ICategoryRepository categoryRepository,
            IProductRepository productRepository,
            IApplicationDbContext context)
        {
            _categoryRepository = categoryRepository;
            _productRepository = productRepository;
            _context = context;
        }

        public async Task<Unit> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            var entity = await _categoryRepository.GetByIdAsync(request.Id, cancellationToken);

            if (entity == null)
                throw new NotFoundException($"Category with id {request.Id} not found.");

            bool hasProducts = await _productRepository.AnyAsync(x => x.CategoryId == request.Id, cancellationToken);
            if (hasProducts)
                throw new InvalidOperationException("Cannot delete category because it has associated products.");

            _categoryRepository.Remove(entity);
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
