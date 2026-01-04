using Application.Features.Products.v1.Repositories;

namespace Application.Features.Products.v1.Commands.Sizes.DeleteSize
{
    public class DeleteSizeHandler : IRequestHandler<DeleteSizeCommand, Unit>
    {
        private readonly ISizeRepository _sizeRepository;
        private readonly IProductVariantRepository _productVariantRepository;
        private readonly IApplicationDbContext _context;

        public DeleteSizeHandler(ISizeRepository sizeRepository,
            IProductVariantRepository productVariantRepository,
            IApplicationDbContext context)
        {
            _sizeRepository = sizeRepository;
            _productVariantRepository = productVariantRepository;
            _context = context;
        }

        public async Task<Unit> Handle(DeleteSizeCommand request, CancellationToken cancellationToken)
        {
            var entity = await _sizeRepository.GetByIdAsync(request.Id, cancellationToken);
            if (entity == null)
                throw new NotFoundException($"Size with Id {request.Id} not found.");

            var isSizeInUse = await _productVariantRepository.AnyAsync(pv => pv.SizeId == request.Id, cancellationToken);
            if (isSizeInUse)
                throw new BusinessRuleException("Cannot delete size as it is associated with existing product variants.");

            _sizeRepository.Remove(entity);
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}

