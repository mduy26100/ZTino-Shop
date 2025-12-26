using Application.Common.Interfaces.Persistence.Data;
using Application.Common.Interfaces.Services.FileUpLoad;
using Application.Common.Models;
using Application.Features.Products.DTOs.ProductImages;
using Application.Features.Products.Repositories;
using Domain.Models.Products;

namespace Application.Features.Products.Commands.ProductImages.CreateProductImage
{
    public class CreateProductImagesHandler : IRequestHandler<CreateProductImagesCommand, List<UpsertProductImageDto>>
    {
        private readonly IProductImageRepository _productImageRepository;
        private readonly IProductVariantRepository _productVariantRepository;
        private readonly IFileUploadService _fileUploadService;
        private readonly IMapper _mapper;
        private readonly IApplicationDbContext _context;

        public CreateProductImagesHandler(
            IProductImageRepository productImageRepository,
            IProductVariantRepository productVariantRepository,
            IFileUploadService fileUploadService,
            IMapper mapper,
            IApplicationDbContext context)
        {
            _productImageRepository = productImageRepository;
            _productVariantRepository = productVariantRepository;
            _fileUploadService = fileUploadService;
            _mapper = mapper;
            _context = context;
        }

        public async Task<List<UpsertProductImageDto>> Handle(CreateProductImagesCommand request, CancellationToken cancellationToken)
        {
            if (request.Dtos is null || request.Dtos.Count == 0)
                return new List<UpsertProductImageDto>();

            var variantId = request.Dtos[0].ProductVariantId;

            bool variantExists = await _productVariantRepository
                .AnyAsync(v => v.Id == variantId, cancellationToken);

            if (!variantExists)
                throw new KeyNotFoundException($"Product Variant with ID {variantId} does not exist.");

            int currentMaxOrder = await _productImageRepository
                .GetMaxDisplayOrderAsync(variantId, cancellationToken);

            bool hasExistingImages = currentMaxOrder > 0;

            var uploadTasks = request.Dtos.Select(async dto =>
            {
                if (dto.ImgContent is null || string.IsNullOrWhiteSpace(dto.ImgFileName))
                {
                    return (Dto: dto, Url: dto.ImageUrl ?? string.Empty);
                }

                var uploadRequest = new FileUploadRequest
                {
                    Content = dto.ImgContent,
                    FileName = dto.ImgFileName,
                    ContentType = dto.ImgContentType ?? "image/jpeg"
                };

                string url = await _fileUploadService.UploadAsync(uploadRequest, cancellationToken);
                return (Dto: dto, Url: url);
            });

            var uploadResults = await Task.WhenAll(uploadTasks);

            var entitiesToAdd = new List<ProductImage>(uploadResults.Length);

            foreach (var (dto, url) in uploadResults)
            {
                if (string.IsNullOrWhiteSpace(url)) continue;

                var entity = _mapper.Map<ProductImage>(dto);

                entity.ImageUrl = url;
                entity.DisplayOrder = ++currentMaxOrder;

                if (!hasExistingImages)
                {
                    entity.IsMain = true;
                    hasExistingImages = true;
                }
                else
                {
                    entity.IsMain = false;
                }

                entitiesToAdd.Add(entity);
            }

            if (entitiesToAdd.Count > 0)
            {
                await _productImageRepository.AddRangeAsync(entitiesToAdd, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
            }

            return _mapper.Map<List<UpsertProductImageDto>>(entitiesToAdd);
        }
    }
}