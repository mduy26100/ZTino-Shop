using Application.Common.Interfaces.Persistence.Data;
using Application.Features.Products.DTOs.Sizes;
using Application.Features.Products.Repositories;

namespace Application.Features.Products.Commands.Sizes.UpdateSize
{
    public class UpdateSizeHandler : IRequestHandler<UpdateSizeCommand, UpsertSizeDto>
    {
        private readonly ISizeRepository _sizeRepository;
        private readonly IMapper _mapper;
        private readonly IApplicationDbContext _context;

        public UpdateSizeHandler(ISizeRepository sizeRepository,
            IMapper mapper,
            IApplicationDbContext context)
        {
            _sizeRepository = sizeRepository;
            _mapper = mapper;
            _context = context;
        }

        public async Task<UpsertSizeDto> Handle(UpdateSizeCommand request, CancellationToken cancellationToken)
        {
            var dto = request.Dto;

            var entity = await _sizeRepository.GetByIdAsync(dto.Id, cancellationToken);
            if (entity == null)
                throw new KeyNotFoundException($"Size with Id {dto.Id} not found.");

            bool nameExists = await _sizeRepository.AnyAsync(s => s.Name == dto.Name && s.Id != dto.Id, cancellationToken);
            if (nameExists)
                throw new InvalidOperationException("Size with the same name already exists.");

            _mapper.Map(dto, entity);

            _sizeRepository.Update(entity);
            await _context.SaveChangesAsync(cancellationToken);

            return _mapper.Map<UpsertSizeDto>(entity);
        }
    }
}
