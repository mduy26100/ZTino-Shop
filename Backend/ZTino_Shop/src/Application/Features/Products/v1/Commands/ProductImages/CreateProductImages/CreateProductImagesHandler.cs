using Application.Common.Interfaces.Persistence.Data;
using Application.Common.Interfaces.Services.FileUpLoad;
using Application.Common.Models.Requests;
using Application.Features.Products.v1.DTOs.ProductImages;
using Application.Features.Products.v1.Repositories;
using Domain.Models.Products;

namespace Application.Features.Products.v1.Commands.ProductImages.CreateProductImages
{
    public class CreateProductImagesHandler : IRequestHandler<CreateProductImagesCommand, List<UpsertProductImageDto>>
    {
        private readonly IProductImageRepository _productImageRepository;
        private readonly IProductColorRepository _productColorRepository;
        private readonly IFileUploadService _fileUploadService;
        private readonly IMapper _mapper;
        private readonly IApplicationDbContext _context;

        public CreateProductImagesHandler(
            IProductImageRepository productImageRepository,
            IProductColorRepository productColorRepository,
            IFileUploadService fileUploadService,
            IMapper mapper,
            IApplicationDbContext context)
        {
            _productImageRepository = productImageRepository;
            _productColorRepository = productColorRepository;
            _fileUploadService = fileUploadService;
            _mapper = mapper;
            _context = context;
        }

        public async Task<List<UpsertProductImageDto>> Handle(CreateProductImagesCommand request, CancellationToken cancellationToken)
        {
            if (request.Dtos == null || !request.Dtos.Any())
                return new List<UpsertProductImageDto>();

            var productColorId = request.Dtos[0].ProductColorId;

            var productColor = await _productColorRepository
                .FindOneAsync(v => v.Id == productColorId, false, cancellationToken);

            if (productColor == null)
                throw new NotFoundException($"Product Color with ID {productColorId} does not exist.");

            int currentMaxOrder = await _productImageRepository
                .GetMaxDisplayOrderAsync(productColorId, cancellationToken);

            bool hasExistingImages = currentMaxOrder > 0;

            var uploadTasks = request.Dtos.Select(async dto =>
            {
                if (dto.ImgContent == null || string.IsNullOrWhiteSpace(dto.ImgFileName))
                {
                    return (Dto: dto, Url: dto.ImageUrl);
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
            var entitiesToAdd = new List<ProductImage>();

            foreach (var result in uploadResults)
            {
                if (string.IsNullOrWhiteSpace(result.Url)) continue;

                var entity = _mapper.Map<ProductImage>(result.Dto);

                entity.ImageUrl = result.Url;
                entity.ProductColorId = productColorId;
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

            if (entitiesToAdd.Any())
            {
                await _productImageRepository.AddRangeAsync(entitiesToAdd, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
            }

            return _mapper.Map<List<UpsertProductImageDto>>(entitiesToAdd);
        }
    }
}