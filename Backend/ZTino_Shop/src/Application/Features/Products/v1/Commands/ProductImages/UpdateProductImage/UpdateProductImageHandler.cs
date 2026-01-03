using Application.Common.Interfaces.Persistence.Data;
using Application.Common.Interfaces.Services.FileUpLoad;
using Application.Common.Models.Requests;
using Application.Features.Products.v1.DTOs.ProductImages;
using Application.Features.Products.v1.Repositories;
using Domain.Models.Products;

namespace Application.Features.Products.v1.Commands.ProductImages.UpdateProductImage
{
    public class UpdateProductImageHandler : IRequestHandler<UpdateProductImageCommand, UpsertProductImageDto>
    {
        private readonly IProductImageRepository _productImageRepository;
        private readonly IProductColorRepository _productColorRepository;
        private readonly IFileUploadService _fileService;
        private readonly IMapper _mapper;
        private readonly IApplicationDbContext _context;

        public UpdateProductImageHandler(
            IProductImageRepository productImageRepository,
            IProductColorRepository productColorRepository,
            IFileUploadService fileService,
            IMapper mapper,
            IApplicationDbContext context)
        {
            _productImageRepository = productImageRepository;
            _productColorRepository = productColorRepository;
            _fileService = fileService;
            _mapper = mapper;
            _context = context;
        }

        public async Task<UpsertProductImageDto> Handle(UpdateProductImageCommand request, CancellationToken cancellationToken)
        {
            var dto = request.Dto;

            var entity = await _productImageRepository.GetByIdAsync(dto.Id, cancellationToken);
            if (entity == null) throw new NotFoundException($"Product Image {dto.Id} not found.");

            var productColor = await _productColorRepository
                .FindOneAsync(v => v.Id == entity.ProductColorId, false, cancellationToken);
            if (productColor == null)
                throw new NotFoundException($"Product Color with ID {entity.ProductColorId} does not exist.");

            await HandleImageUploadAsync(entity, dto, cancellationToken);

            if (dto.DisplayOrder != 0)
                entity.DisplayOrder = dto.DisplayOrder;

            if (dto.IsMain != entity.IsMain)
            {
                if (dto.IsMain)
                {
                    var currentMain = await _productImageRepository.FindOneAsync(
                        x => x.ProductColorId == entity.ProductColorId && x.IsMain && x.Id != entity.Id,
                        true,
                        cancellationToken);

                    if (currentMain != null)
                    {
                        currentMain.IsMain = false;
                        _productImageRepository.Update(currentMain);
                    }
                    entity.IsMain = true;
                }
                else
                {
                    var alternativeMain = await _productImageRepository.FindOneAsync(
                        x => x.ProductColorId == entity.ProductColorId && x.Id != entity.Id,
                        true,
                        cancellationToken);

                    if (alternativeMain != null)
                    {
                        alternativeMain.IsMain = true;
                        _productImageRepository.Update(alternativeMain);
                        entity.IsMain = false;
                    }
                    else
                    {
                        entity.IsMain = true;
                    }
                }
            }

            _productImageRepository.Update(entity);
            await _context.SaveChangesAsync(cancellationToken);

            return _mapper.Map<UpsertProductImageDto>(entity);
        }

        private async Task HandleImageUploadAsync(ProductImage entity, UpsertProductImageDto dto, CancellationToken ct)
        {
            if (dto.ImgContent != null && dto.ImgContent.Length > 0)
            {
                var uploadRequest = new FileUploadRequest
                {
                    Content = dto.ImgContent,
                    FileName = dto.ImgFileName ?? $"img_{Guid.NewGuid()}",
                    ContentType = dto.ImgContentType ?? "image/jpeg"
                };

                entity.ImageUrl = await _fileService.UploadAsync(uploadRequest, ct);
            }
        }
    }
}