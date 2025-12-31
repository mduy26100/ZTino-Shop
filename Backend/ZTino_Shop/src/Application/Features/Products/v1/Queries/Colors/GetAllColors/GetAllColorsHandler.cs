using Application.Features.Products.v1.DTOs.Colors;
using Application.Features.Products.v1.Repositories;

namespace Application.Features.Products.v1.Queries.Colors.GetAllColors
{
    public class GetAllColorsHandler : IRequestHandler<GetAllColorsQuery, IEnumerable<ColorDto>>
    {
        private readonly IColorRepository _colorRepository;
        private readonly IMapper _mapper;

        public GetAllColorsHandler(IColorRepository colorRepository,
            IMapper mapper)
        {
            _colorRepository = colorRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ColorDto>> Handle(GetAllColorsQuery request, CancellationToken cancellationToken)
        {
            var list = await _colorRepository.GetAllAsync(cancellationToken);
            return _mapper.Map<IEnumerable<ColorDto>>(list);
        }
    }
}
