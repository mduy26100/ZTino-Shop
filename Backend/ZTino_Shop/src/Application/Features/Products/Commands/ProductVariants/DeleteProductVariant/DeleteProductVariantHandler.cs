using Application.Common.Interfaces.Persistence.Data;
using Application.Features.Products.Repositories;

namespace Application.Features.Products.Commands.ProductVariants.DeleteProductVariant
{
    public class DeleteProductVariantHandler : IRequestHandler<DeleteProductVariantCommand, Unit>
    {
        private readonly IProductVariantRepository _productVariantRepository;
        private readonly IApplicationDbContext _context;

        public DeleteProductVariantHandler(IProductVariantRepository productVariantRepository,
            IApplicationDbContext context)
        {
            _productVariantRepository = productVariantRepository;
            _context = context;
        }

        public async Task<Unit> Handle(DeleteProductVariantCommand request, CancellationToken cancellationToken)
        {
            var entity = await _productVariantRepository.GetByIdAsync(request.Id, cancellationToken);
            if (entity == null)
                throw new NotFoundException($"Product variant with Id {request.Id} not found.");

            _productVariantRepository.Remove(entity);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
