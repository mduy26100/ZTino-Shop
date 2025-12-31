using Application.Common.Interfaces.Persistence.Data;
using Application.Features.Products.v1.Repositories;

namespace Application.Features.Products.v1.Commands.ProductImages.DeleteProductImage
{
    public class DeleteProductImageHandler : IRequestHandler<DeleteProductImageCommand, Unit>
    {
        private readonly IProductImageRepository _productImageRepository;
        private readonly IApplicationDbContext _context;

        public DeleteProductImageHandler(
            IProductImageRepository productImageRepository,
            IApplicationDbContext context)
        {
            _productImageRepository = productImageRepository;
            _context = context;
        }

        public async Task<Unit> Handle(DeleteProductImageCommand request, CancellationToken cancellationToken)
        {
            var entity = await _productImageRepository.GetByIdAsync(request.Id, cancellationToken);

            if (entity == null)
            {
                throw new NotFoundException($"Product Image with Id {request.Id} not found.");
            }

            if (entity.IsMain)
            {
                var siblings = await _productImageRepository.FindAsync(
                    x => x.ProductVariantId == entity.ProductVariantId && x.Id != entity.Id,
                    true,
                    cancellationToken);

                var heir = siblings.FirstOrDefault();

                _productImageRepository.Remove(entity);

                await _context.SaveChangesAsync(cancellationToken);

                if (heir != null)
                {
                    heir.IsMain = true;
                    _productImageRepository.Update(heir);

                    await _context.SaveChangesAsync(cancellationToken);
                }
            }
            else
            {
                _productImageRepository.Remove(entity);
                await _context.SaveChangesAsync(cancellationToken);
            }

            return Unit.Value;
        }
    }
}