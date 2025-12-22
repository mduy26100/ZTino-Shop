using Application.Features.Products.DTOs.Sizes;
using Application.Features.Products.Repositories;

namespace Application.Features.Products.Queries.Sizes.GetAllSizes
{
    public class GetAllSizesHandler : IRequestHandler<GetAllSizesQuery, IEnumerable<SizeDto>>
    {
        private readonly ISizeRepository _sizeRepository;
        private readonly IMapper _mapper;

        public GetAllSizesHandler(ISizeRepository sizeRepository,
            IMapper mapper)
        {
            _sizeRepository = sizeRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<SizeDto>> Handle(GetAllSizesQuery request, CancellationToken cancellationToken)
        {
            var list = await _sizeRepository.GetAllAsync(cancellationToken);
            return _mapper.Map<IEnumerable<SizeDto>>(list);
        }
    }
}
