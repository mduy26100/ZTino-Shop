using Application.Features.Products.v1.DTOs.ProductColor;
using Application.Features.Products.v1.Repositories;

namespace Application.Features.Products.v1.Commands.ProductColor.UpdateProductColor
{
    public class UpdateProductColorHandler : IRequestHandler<UpdateProductColorCommand, UpsertProductColorDto>
    {
        private readonly IProductColorRepository _productColorRepository;
        private readonly IProductRepository _productRepository;
        private readonly IColorRepository _colorRepository;
        private readonly IMapper _mapper;
        private readonly IApplicationDbContext _context;

        public UpdateProductColorHandler(IProductColorRepository productColorRepository,
            IProductRepository productRepository,
            IColorRepository colorRepository,
            IMapper mapper,
            IApplicationDbContext context)
        {
            _productColorRepository = productColorRepository;
            _productRepository = productRepository;
            _colorRepository = colorRepository;
            _mapper = mapper;
            _context = context;
        }

        public async Task<UpsertProductColorDto> Handle(UpdateProductColorCommand request, CancellationToken cancellationToken)
        {
            var dto = request.Dto;

            var entity = await _productColorRepository.GetByIdAsync(dto.Id, cancellationToken);
            if (entity == null)
                throw new NotFoundException($"Product Color with Id {dto.Id} not found.");

            var productExists = await _productRepository.AnyAsync(p => p.Id == dto.ProductId, cancellationToken);
            if (!productExists)
                throw new NotFoundException($"Product with Id {dto.ProductId} not found.");

            var colorExists = await _colorRepository.AnyAsync(c => c.Id == dto.ColorId, cancellationToken);
            if (!colorExists)
                throw new NotFoundException($"Color with Id {dto.ColorId} not found.");

            var productColorExists = await _productColorRepository.AnyAsync(pc =>
                pc.ProductId == dto.ProductId &&
                pc.ColorId == dto.ColorId &&
                pc.Id != dto.Id, cancellationToken);
            if (productColorExists)
                throw new ConflictException($"Product Color with ProductId {dto.ProductId} and ColorId {dto.ColorId} already exists.");

            _mapper.Map(dto, entity);

            _productColorRepository.Update(entity);
            await _context.SaveChangesAsync(cancellationToken);
            return _mapper.Map<UpsertProductColorDto>(entity);
        }
    }
}

