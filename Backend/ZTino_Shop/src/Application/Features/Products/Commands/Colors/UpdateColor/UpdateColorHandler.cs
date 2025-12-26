using Application.Common.Interfaces.Persistence.Data;
using Application.Features.Products.DTOs.Colors;
using Application.Features.Products.Repositories;

namespace Application.Features.Products.Commands.Colors.UpdateColor
{
    public class UpdateColorHandler : IRequestHandler<UpdateColorCommand, UpsertColorDto>
    {
        private readonly IColorRepository _colorRepository;
        private readonly IMapper _mapper;
        private readonly IApplicationDbContext _context;

        public UpdateColorHandler(IColorRepository colorRepository,
            IMapper mapper,
            IApplicationDbContext context)
        {
            _colorRepository = colorRepository;
            _mapper = mapper;
            _context = context;
        }

        public async Task<UpsertColorDto> Handle(UpdateColorCommand request, CancellationToken cancellationToken)
        {
            var dto = request.Dto;

            var entity = await _colorRepository.GetByIdAsync(dto.Id, cancellationToken);

            if (entity == null)
                throw new KeyNotFoundException($"Color with Id {dto.Id} not found.");

            bool nameExists = await _colorRepository.AnyAsync(
                c => c.Name == dto.Name && c.Id != dto.Id,
                cancellationToken);
            if (nameExists)
                throw new InvalidOperationException("Color with the same name already exists.");

            _mapper.Map(dto, entity);

            _colorRepository.Update(entity);
            await _context.SaveChangesAsync(cancellationToken);

            return _mapper.Map<UpsertColorDto>(entity);
        }
    }
}
