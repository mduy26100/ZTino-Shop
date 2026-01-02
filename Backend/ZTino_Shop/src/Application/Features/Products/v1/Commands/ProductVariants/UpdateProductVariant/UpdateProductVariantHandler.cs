using Application.Common.Interfaces.Persistence.Data;
using Application.Features.Products.v1.DTOs.ProductVariants;
using Application.Features.Products.v1.Repositories;
using Domain.Models.Products;

namespace Application.Features.Products.v1.Commands.ProductVariants.UpdateProductVariant
{
    public class UpdateProductVariantHandler : IRequestHandler<UpdateProductVariantCommand, UpsertProductVariantDto>
    {
        private readonly IProductVariantRepository _productVariantRepository;
        private readonly IProductRepository _productRepository;
        private readonly ISizeRepository _sizeRepository;
        private readonly IColorRepository _colorRepository;
        private readonly IProductColorRepository _productColorRepository;
        private readonly IMapper _mapper;
        private readonly IApplicationDbContext _context;

        public UpdateProductVariantHandler(
            IProductVariantRepository productVariantRepository,
            IProductRepository productRepository,
            ISizeRepository sizeRepository,
            IColorRepository colorRepository,
            IProductColorRepository productColorRepository,
            IMapper mapper,
            IApplicationDbContext context)
        {
            _productVariantRepository = productVariantRepository;
            _productRepository = productRepository;
            _sizeRepository = sizeRepository;
            _colorRepository = colorRepository;
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

            var productExists = await _productRepository.AnyAsync(p => p.Id == dto.ProductId, cancellationToken);
            if (!productExists)
                throw new NotFoundException("Product not found.");

            var sizeExists = await _sizeRepository.AnyAsync(s => s.Id == dto.SizeId, cancellationToken);
            if (!sizeExists)
                throw new NotFoundException("Size not found.");

            var colorExists = await _colorRepository.AnyAsync(c => c.Id == dto.ColorId, cancellationToken);
            if (!colorExists)
                throw new NotFoundException("Color not found.");

            var productColor = await _productColorRepository.FindOneAsync(
                pc => pc.ProductId == dto.ProductId && pc.ColorId == dto.ColorId,
                false,
                cancellationToken);

            if (productColor == null)
            {
                productColor = new ProductColor
                {
                    ProductId = dto.ProductId,
                    ColorId = dto.ColorId
                };
                await _productColorRepository.AddAsync(productColor, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
            }

            var variantConflict = await _productVariantRepository.AnyAsync(pv =>
                pv.ProductColorId == productColor.Id &&
                pv.SizeId == dto.SizeId &&
                pv.Id != dto.Id,
                cancellationToken);

            if (variantConflict)
                throw new ConflictException("Another product variant with the same size and color already exists for this product.");

            _mapper.Map(dto, entity);
            entity.ProductColorId = productColor.Id;

            _productVariantRepository.Update(entity);
            await _context.SaveChangesAsync(cancellationToken);

            return _mapper.Map<UpsertProductVariantDto>(entity);
        }
    }
}