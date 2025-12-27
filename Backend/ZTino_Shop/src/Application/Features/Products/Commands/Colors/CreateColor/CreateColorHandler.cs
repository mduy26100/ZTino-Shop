using Application.Common.Interfaces.Persistence.Data;
using Application.Features.Products.DTOs.Colors;
using Application.Features.Products.Repositories;
using Domain.Models.Products;

namespace Application.Features.Products.Commands.Colors.CreateColor
{
    public class CreateColorHandler : IRequestHandler<CreateColorCommand, UpsertColorDto>
    {
        private readonly IColorRepository _colorRepository;
        private readonly IMapper _mapper;
        private readonly IApplicationDbContext _context;

        public CreateColorHandler(IColorRepository colorRepository,
            IMapper mapper,
            IApplicationDbContext context)
        {
            _colorRepository = colorRepository;
            _mapper = mapper;
            _context = context;
        }

        public async Task<UpsertColorDto> Handle(CreateColorCommand request, CancellationToken cancellationToken)
        {
            var dto = request.Dto;

            bool nameExists = await _colorRepository.AnyAsync(c => c.Name == dto.Name, cancellationToken);
            if (nameExists)
                throw new ConflictException("Color with the same name already exists.");

            var entity = _mapper.Map<Color>(dto);
            await _colorRepository.AddAsync(entity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return _mapper.Map<UpsertColorDto>(entity);
        }
    }
}
