using Application.Common.Interfaces.Persistence.Data;
using Application.Features.Products.DTOs.Sizes;
using Application.Features.Products.Repositories;
using Domain.Models.Products;

namespace Application.Features.Products.Commands.Sizes.CreateSize
{
    public class CreateSizeHandler : IRequestHandler<CreateSizeCommand, UpsertSizeDto>
    {
        private readonly ISizeRepository _sizeRepository;
        private readonly IMapper _mapper;
        private readonly IApplicationDbContext _context;

        public CreateSizeHandler(ISizeRepository sizeRepository,
            IMapper mapper,
            IApplicationDbContext context)
        {
            _sizeRepository = sizeRepository;
            _mapper = mapper;
            _context = context;
        }

        public async Task<UpsertSizeDto> Handle(CreateSizeCommand request, CancellationToken cancellationToken)
        {
            var nameExists = await _sizeRepository.AnyAsync(s => s.Name == request.Dto.Name, cancellationToken);
            if (nameExists)
                throw new InvalidOperationException("Size with the same name already exists.");

            var entity = _mapper.Map<Size>(request.Dto);

            await _sizeRepository.AddAsync(entity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return _mapper.Map<UpsertSizeDto>(entity);
        }
    }
}
