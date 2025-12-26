using Application.Common.Interfaces.Persistence.Data;
using Application.Features.Products.Repositories;

namespace Application.Features.Products.Commands.Colors.DeleteColor
{
    public class DeleteColorHandler : IRequestHandler<DeleteColorCommand, Unit>
    {
        private readonly IColorRepository _colorRepository;
        private readonly IProductVariantRepository _productVariantRepository;
        private readonly IApplicationDbContext _context;

        public DeleteColorHandler(IColorRepository colorRepository,
            IProductVariantRepository productVariantRepository,
            IApplicationDbContext context)
        {
            _colorRepository = colorRepository;
            _productVariantRepository = productVariantRepository;
            _context = context;
        }

        public async Task<Unit> Handle(DeleteColorCommand request, CancellationToken cancellationToken)
        {
            var entity = await _colorRepository.GetByIdAsync(request.Id, cancellationToken);
            if (entity == null)
                throw new NotFoundException($"Color with Id {request.Id} not found.");

            bool hasVariants = await _productVariantRepository.AnyAsync(c => c.ColorId == request.Id, cancellationToken);
            if (hasVariants)
                throw new InvalidOperationException("Cannot delete color that is associated with product variants.");

            _colorRepository.Remove(entity);
            await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
