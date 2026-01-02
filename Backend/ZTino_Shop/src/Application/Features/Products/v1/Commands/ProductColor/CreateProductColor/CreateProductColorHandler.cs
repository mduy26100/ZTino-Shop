using Application.Common.Interfaces.Persistence.Data;
using Application.Features.Products.v1.DTOs.ProductColor;
using Application.Features.Products.v1.Repositories;
using ProductColorEntity = Domain.Models.Products.ProductColor;

namespace Application.Features.Products.v1.Commands.ProductColor.CreateProductColor
{
    public class CreateProductColorHandler : IRequestHandler<CreateProductColorCommand, UpsertProductColorDto>
    {
        private readonly IProductColorRepository _productColorRepository;
        private readonly IProductRepository _productRepository;
        private readonly IColorRepository _colorRepository;
        private readonly IMapper _mapper;
        private readonly IApplicationDbContext _context;

        public CreateProductColorHandler(IProductColorRepository productColorRepository,
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

        public async Task<UpsertProductColorDto> Handle(CreateProductColorCommand request, CancellationToken cancellationToken)
        {
            var productExists = await _productRepository.GetByIdAsync(request.Dto.ProductId);
            if (productExists is null)
            {
                throw new NotFoundException($"Product with ID {request.Dto.ProductId} not found.");
            }

            var colorExists = await _colorRepository.GetByIdAsync(request.Dto.ColorId);
            if (colorExists is null)
            {
                throw new NotFoundException($"Color with ID {request.Dto.ColorId} not found.");
            }

            var productColorExists = await _productColorRepository.AnyAsync(pc =>
                pc.ProductId == request.Dto.ProductId && pc.ColorId == request.Dto.ColorId, cancellationToken);
            if (productColorExists)
            {
                throw new ConflictException("ProductColor with the same ProductId and ColorId already exists.");
            }

            var productColorEntity = _mapper.Map<ProductColorEntity>(request.Dto);

            await _productColorRepository.AddAsync(productColorEntity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return _mapper.Map<UpsertProductColorDto>(productColorEntity);
        }
    }
}
