using Application.Common.Interfaces.Persistence.Data;
using Application.Features.Products.v1.Repositories;

namespace Application.Features.Products.v1.Commands.ProductColor.DeleteProductColor
{
    public class DeleteProductColorHandler : IRequestHandler<DeleteProductColorCommand, Unit>
    {
        private readonly IProductColorRepository _productColorRepository;
        private readonly IProductImageRepository _productImageRepository;
        private readonly IProductVariantRepository _productVariantRepository;
        private readonly IApplicationDbContext _context;

        public DeleteProductColorHandler(IProductColorRepository productColorRepository,
            IProductImageRepository productImageRepository,
            IProductVariantRepository productVariantRepository,
            IApplicationDbContext context)
        {
            _productColorRepository = productColorRepository;
            _productImageRepository = productImageRepository;
            _productVariantRepository = productVariantRepository;
            _context = context;
        }

        public async Task<Unit> Handle(DeleteProductColorCommand request, CancellationToken cancellationToken)
        {
            var entity = await _productColorRepository.GetByIdAsync(request.Id, cancellationToken);
            if (entity == null)
            {
                throw new NotFoundException($"Product Color with Id {request.Id} not found.");
            }

            var hasVariants = await _productVariantRepository.AnyAsync(pv => pv.ProductColorId == request.Id, cancellationToken);
            if (hasVariants)
            {
                throw new BusinessRuleException("Cannot delete Product Color as it is associated with existing Product Variants.");
            }

            var images = await _productImageRepository.AnyAsync(pi => pi.ProductColorId == request.Id, cancellationToken);
            if (images)
            {
                throw new BusinessRuleException("Cannot delete Product Color as it is associated with existing Product Images.");
            }

            _productColorRepository.Remove(entity);
            await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
