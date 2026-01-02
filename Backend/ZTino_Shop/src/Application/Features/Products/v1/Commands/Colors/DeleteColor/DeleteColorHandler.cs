using Application.Common.Interfaces.Persistence.Data;
using Application.Features.Products.v1.Repositories;

namespace Application.Features.Products.v1.Commands.Colors.DeleteColor
{
    public class DeleteColorHandler : IRequestHandler<DeleteColorCommand, Unit>
    {
        private readonly IColorRepository _colorRepository;
        private readonly IProductColorRepository _productColorRepository;
        private readonly IApplicationDbContext _context;

        public DeleteColorHandler(
            IColorRepository colorRepository,
            IProductColorRepository productColorRepository,
            IApplicationDbContext context)
        {
            _colorRepository = colorRepository;
            _productColorRepository = productColorRepository;
            _context = context;
        }

        public async Task<Unit> Handle(DeleteColorCommand request, CancellationToken cancellationToken)
        {
            var entity = await _colorRepository.GetByIdAsync(request.Id, cancellationToken);
            if (entity == null)
                throw new NotFoundException($"Color with Id {request.Id} not found.");

            bool isUsedInProducts = await _productColorRepository.AnyAsync(pc => pc.ColorId == request.Id, cancellationToken);
            if (isUsedInProducts)
                throw new BusinessRuleException("Cannot delete color that is associated with products.");

            _colorRepository.Remove(entity);
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}