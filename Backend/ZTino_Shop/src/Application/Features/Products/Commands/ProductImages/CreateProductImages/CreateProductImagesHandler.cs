using Application.Common.Interfaces.Persistence.EFCore;
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
            if (request.Dtos is null || !request.Dtos.Any())
                return new List<UpsertProductImageDto>();

            var variantId = request.Dtos.First().ProductVariantId;
            if (request.Dtos.Any(x => x.ProductVariantId != variantId))
            {
                throw new ArgumentException("All images must belong to the same Product Variant.");
            }

            bool variantExists = await _productVariantRepository
                .AnyAsync(v => v.Id == variantId, cancellationToken);

            if (!variantExists)
                throw new KeyNotFoundException($"Product Variant with ID {variantId} does not exist.");

            int currentMaxOrder = await _productImageRepository
                .GetMaxDisplayOrderAsync(variantId, cancellationToken);
            
            bool hasExistingImages = currentMaxOrder > 0;

            var uploadTasks = request.Dtos.Select(async dto =>
            {
                string imgUrl = string.Empty;
                if (dto.ImgContent != null && !string.IsNullOrWhiteSpace(dto.ImgFileName))
                {
                    var uploadRequest = new FileUploadRequest
                    {
                        Content = dto.ImgContent,
                        FileName = dto.ImgFileName!,
                        ContentType = dto.ImgContentType ?? "image/jpeg"
                    };

                    imgUrl = await _fileUploadService.UploadAsync(uploadRequest, cancellationToken);
                }
                
                return new { Dto = dto, Url = imgUrl };
            });

            var uploadResults = await Task.WhenAll(uploadTasks);

            var entitiesToAdd = new List<ProductImage>();
            
            foreach (var item in uploadResults)
            {
                var entity = _mapper.Map<ProductImage>(item.Dto);
                
                entity.ImageUrl = item.Url;
                entity.DisplayOrder = ++currentMaxOrder;

                if (!hasExistingImages && entity.DisplayOrder == 1)
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

            await _productImageRepository.AddRangeAsync(entitiesToAdd, cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);

            return _mapper.Map<List<UpsertProductImageDto>>(entitiesToAdd);
        }
    }
}