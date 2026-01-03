using Application.Common.Interfaces.Persistence.Data;
using Application.Features.Products.v1.DTOs.ProductVariants;
using Application.Features.Products.v1.Repositories;
using Domain.Models.Products;

namespace Application.Features.Products.v1.Commands.ProductVariants.CreateProductVariant
{
    public class CreateProductVariantHandler : IRequestHandler<CreateProductVariantCommand, UpsertProductVariantDto>
    {
        private readonly IProductVariantRepository _productVariantRepository;
        private readonly IProductColorRepository _productColorRepository;
        private readonly ISizeRepository _sizeRepository;
        private readonly IMapper _mapper;
        private readonly IApplicationDbContext _context;

        public CreateProductVariantHandler(
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

        public async Task<UpsertProductVariantDto> Handle(CreateProductVariantCommand request, CancellationToken cancellationToken)
        {
            var dto = request.Dto;

            var productExists = await _productColorRepository.AnyAsync(p => p.Id == dto.ProductColorId, cancellationToken);
            if (!productExists) throw new NotFoundException("Product Color Id not found.");

            var sizeExists = await _sizeRepository.AnyAsync(s => s.Id == dto.SizeId, cancellationToken);
            if (!sizeExists) throw new NotFoundException("Size not found.");

            var variantExists = await _productVariantRepository.AnyAsync(pv =>
                pv.ProductColorId == dto.ProductColorId &&
                pv.SizeId == dto.SizeId,
                cancellationToken);

            if (variantExists)
                throw new ConflictException("This size already exists for the selected color of this product.");

            var entity = _mapper.Map<ProductVariant>(dto);

            await _productVariantRepository.AddAsync(entity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return _mapper.Map<UpsertProductVariantDto>(entity);
        }
    }
}