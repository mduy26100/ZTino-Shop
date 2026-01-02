using Application.Features.Products.v1.DTOs.Products;
using Application.Features.Products.v1.Repositories;

namespace Application.Features.Products.v1.Queries.Products.GetProductsByCategoryId
{
    public class GetProductsByCategoryIdHandler : IRequestHandler<GetProductsByCategoryIdQuery, List<ProductSummaryDto>>
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public GetProductsByCategoryIdHandler(
            IProductRepository productRepository,
            ICategoryRepository categoryRepository,
            IMapper mapper)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public async Task<List<ProductSummaryDto>> Handle(GetProductsByCategoryIdQuery request, CancellationToken cancellationToken)
        {
            var categories = await _categoryRepository.FindAsync(
                predicate: c => c.Id == request.CategoryId || c.ParentId == request.CategoryId,
                asNoTracking: true,
                cancellationToken: cancellationToken
            );

            var categoryList = categories.ToList();

            if (!categoryList.Any())
            {
                return new List<ProductSummaryDto>();
            }

            var categoryDict = categoryList.ToDictionary(k => k.Id, v => v.Name);

            var targetCategoryIds = categoryList.Select(c => c.Id).ToList();

            var products = await _productRepository.FindAsync(
                predicate: p => targetCategoryIds.Contains(p.CategoryId),
                asNoTracking: true,
                cancellationToken: cancellationToken
            );

            var productDtos = _mapper.Map<List<ProductSummaryDto>>(products);

            foreach (var dto in productDtos)
            {
                if (categoryDict.TryGetValue(dto.CategoryId, out var categoryName))
                {
                    dto.CategoryName = categoryName;
                }
            }

            return productDtos.OrderByDescending(p => p.Id).ToList();
        }
    }
}