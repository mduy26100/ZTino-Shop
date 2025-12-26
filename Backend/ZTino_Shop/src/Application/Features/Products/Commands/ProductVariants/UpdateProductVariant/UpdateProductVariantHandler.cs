using Application.Common.Interfaces.Persistence.Data;
using Application.Features.Products.DTOs.ProductVariants;
using Application.Features.Products.Repositories;

namespace Application.Features.Products.Commands.ProductVariants.UpdateProductVariant
{
    public class UpdateProductVariantHandler : IRequestHandler<UpdateProductVariantCommand, UpsertProductVariantDto>
    {
        private readonly IProductVariantRepository _productVariantRepository;
        private readonly IProductRepository _productRepository;
        private readonly ISizeRepository _sizeRepository;
        private readonly IColorRepository _colorRepository;
        private readonly IMapper _mapper;
        private readonly IApplicationDbContext _context;

        public UpdateProductVariantHandler(
            IProductVariantRepository productVariantRepository,
            IProductRepository productRepository,
            ISizeRepository sizeRepository,
            IColorRepository colorRepository,
            IMapper mapper,
            IApplicationDbContext context)
        {
            _productVariantRepository = productVariantRepository;
            _productRepository = productRepository;
            _sizeRepository = sizeRepository;
            _colorRepository = colorRepository;
            _mapper = mapper;
            _context = context;
        }

        public async Task<UpsertProductVariantDto> Handle(UpdateProductVariantCommand request, CancellationToken cancellationToken)
        {
            var entity = await _productVariantRepository.GetByIdAsync(request.Dto.Id, cancellationToken);
            if (entity == null)
                throw new NotFoundException("Product variant not found.");

            var productExists = await _productRepository.AnyAsync(p => p.Id == request.Dto.ProductId, cancellationToken);
            if (!productExists)
                throw new NotFoundException("Product not found.");

            var sizeExists = await _sizeRepository.AnyAsync(s => s.Id == request.Dto.SizeId, cancellationToken);
            if (!sizeExists)
                throw new NotFoundException("Size not found.");

            var colorExists = await _colorRepository.AnyAsync(c => c.Id == request.Dto.ColorId, cancellationToken);
            if (!colorExists)
                throw new NotFoundException("Color not found.");

            var variantExists = await _productVariantRepository.AnyAsync(pv =>
                pv.ProductId == request.Dto.ProductId &&
                pv.SizeId == request.Dto.SizeId &&
                pv.ColorId == request.Dto.ColorId &&
                pv.Id != request.Dto.Id,
                cancellationToken);

            if (variantExists)
                throw new InvalidOperationException("Another product variant with the same product, size, and color already exists.");

            _mapper.Map(request.Dto, entity);

            _productVariantRepository.Update(entity);
            await _context.SaveChangesAsync(cancellationToken);

            return _mapper.Map<UpsertProductVariantDto>(entity);
        }
    }
}
