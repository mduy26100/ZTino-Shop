using Application.Features.Products.v1.Repositories;

namespace Application.Features.Products.v1.Commands.Products.DeleteProduct
{
    public class DeleteProductHandler : IRequestHandler<DeleteProductCommand, Unit>
    {
        private readonly IProductRepository _productRepository;
        private readonly IApplicationDbContext _context;

        public DeleteProductHandler(IProductRepository productRepository,
            IApplicationDbContext context)
        {
            _productRepository = productRepository;
            _context = context;
        }

        public async Task<Unit> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            var entity = await _productRepository.GetByIdAsync(request.Id, cancellationToken);
            if (entity == null)
                throw new NotFoundException($"Product with id {request.Id} not found.");

            _productRepository.Remove(entity);
            await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}

