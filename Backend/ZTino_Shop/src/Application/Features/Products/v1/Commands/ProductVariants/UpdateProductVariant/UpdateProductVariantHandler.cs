using Application.Features.Products.v1.DTOs.ProductVariants;
using Application.Features.Products.v1.Repositories;

namespace Application.Features.Products.v1.Commands.ProductVariants.UpdateProductVariant
{
    public class UpdateProductVariantHandler : IRequestHandler<UpdateProductVariantCommand, UpsertProductVariantDto>
    {
        private readonly IProductVariantRepository _productVariantRepository;
        private readonly IProductColorRepository _productColorRepository;
        private readonly ISizeRepository _sizeRepository;
        private readonly IMapper _mapper;
        private readonly IApplicationDbContext _context;

        public UpdateProductVariantHandler(
            IProductVariantRepository productVariantRepository,
            IProductColorRepository productColorRepository,
            ISizeRepository sizeRepository,
            IMapper mapper,
            IApplicationDbContext context)
        {
            _productVariantRepository = productVariantRepository;
            _sizeRepository = sizeRepository;
            _productColorRepository = productColorRepository;
            _mapper = mapper;
            _context = context;
        }

        public async Task<UpsertProductVariantDto> Handle(UpdateProductVariantCommand request, CancellationToken cancellationToken)
        {
            var dto = request.Dto;

            var entity = await _productVariantRepository.GetByIdAsync(dto.Id, cancellationToken);
            if (entity == null)
                throw new NotFoundException("Product variant not found.");

            var productExists = await _productColorRepository.AnyAsync(p => p.Id == dto.ProductColorId, cancellationToken);
            if (!productExists)
                throw new NotFoundException("Product color not found.");

            var sizeExists = await _sizeRepository.AnyAsync(s => s.Id == dto.SizeId, cancellationToken);
            if (!sizeExists)
                throw new NotFoundException("Size not found.");

            var variantConflict = await _productVariantRepository.AnyAsync(pv =>
                pv.ProductColorId == dto.ProductColorId &&
                pv.SizeId == dto.SizeId &&
                pv.Id != dto.Id,
                cancellationToken);

            if (variantConflict)
                throw new ConflictException("Another product variant with the same size and color already exists for this product.");

            _mapper.Map(dto, entity);

            _productVariantRepository.Update(entity);
            await _context.SaveChangesAsync(cancellationToken);

            return _mapper.Map<UpsertProductVariantDto>(entity);
        }
    }
}
