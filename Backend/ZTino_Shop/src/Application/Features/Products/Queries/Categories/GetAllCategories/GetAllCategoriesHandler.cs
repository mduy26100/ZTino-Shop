using Application.Features.Products.DTOs.Categories;
using Application.Features.Products.Repositories;

namespace Application.Features.Products.Queries.Categories.GetAllCategories
{
    public class GetAllCategoriesHandler : IRequestHandler<GetAllCategoriesQuery, List<CategoryTreeDto>>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public GetAllCategoriesHandler(ICategoryRepository categoryRepository,
            IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public async Task<List<CategoryTreeDto>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
        {
            var categories = await _categoryRepository.GetAllAsync(cancellationToken);

            var categoryDtos = _mapper.Map<List<CategoryTreeDto>>(categories);

            var lookup = categoryDtos.ToDictionary(c => c.Id);
            var roots = new List<CategoryTreeDto>();

            foreach (var category in categoryDtos)
            {
                if (category.ParentId.HasValue && lookup.ContainsKey(category.ParentId.Value))
                {
                    lookup[category.ParentId.Value].Children.Add(category);
                }
                else
                {
                    roots.Add(category);
                }
            }

            return roots;
        }
    }
}
