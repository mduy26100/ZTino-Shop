using Application.Common.Interfaces.Persistence.Data;
using Application.Features.Products.v1.DTOs.ProductVariants;
using Application.Features.Products.v1.Repositories;
using Domain.Models.Products;

namespace Application.Features.Products.v1.Commands.ProductVariants.CreateProductVariant
{
    public class CreateProductVariantHandler : IRequestHandler<CreateProductVariantCommand, UpsertProductVariantDto>
    {
        private readonly IProductVariantRepository _productVariantRepository;
        private readonly IProductRepository _productRepository;
        private readonly ISizeRepository _sizeRepository;
        private readonly IColorRepository _colorRepository;
        private readonly IMapper _mapper;
        private readonly IApplicationDbContext _context;

        public CreateProductVariantHandler(IProductVariantRepository productVariantRepository,
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

        public async Task<UpsertProductVariantDto> Handle(CreateProductVariantCommand request, CancellationToken cancellationToken)
        {
            var productExists = await _productRepository.AnyAsync(p => p.Id == request.Dto.ProductId, cancellationToken);
            if (!productExists)
                throw new NotFoundException("Product not found.");

            var sizeExists = await _sizeRepository.AnyAsync(s => s.Id == request.Dto.SizeId, cancellationToken);
            if (!sizeExists)
                throw new NotFoundException("Size not found.");

            var colorExists = await _colorRepository.AnyAsync(c => c.Id == request.Dto.ColorId, cancellationToken);
            if (!colorExists)
                throw new NotFoundException("Color not found.");

            var variantExists = await _productVariantRepository.AnyAsync(pv => pv.ProductId == request.Dto.ProductId &&
                                                                             pv.SizeId == request.Dto.SizeId &&
                                                                             pv.ColorId == request.Dto.ColorId,
                                                                             cancellationToken);
            if (variantExists)
                throw new ConflictException("Product variant with the same product, size, and color already exists.");

            var entity = _mapper.Map<ProductVariant>(request.Dto);

            await _productVariantRepository.AddAsync(entity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return _mapper.Map<UpsertProductVariantDto>(entity);
        }
    }
}
