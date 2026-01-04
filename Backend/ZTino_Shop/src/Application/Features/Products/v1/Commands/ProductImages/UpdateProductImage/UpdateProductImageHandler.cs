using Application.Features.Products.v1.DTOs.ProductImages;
using Application.Features.Products.v1.Repositories;

namespace Application.Features.Products.v1.Commands.ProductImages.UpdateProductImage
{
    public class UpdateProductImageHandler : IRequestHandler<UpdateProductImageCommand, UpsertProductImageDto>
    {
        private readonly IProductImageRepository _productImageRepository;
        private readonly IFileUploadService _fileService;
        private readonly IMapper _mapper;
        private readonly IApplicationDbContext _context;

        public UpdateProductImageHandler(
            IProductImageRepository productImageRepository,
            IFileUploadService fileService,
            IMapper mapper,
            IApplicationDbContext context)
        {
            _productImageRepository = productImageRepository;
            _fileService = fileService;
            _mapper = mapper;
            _context = context;
        }

        public async Task<UpsertProductImageDto> Handle(UpdateProductImageCommand request, CancellationToken cancellationToken)
        {
            var dto = request.Dto;

            var entity = await _productImageRepository.GetByIdAsync(dto.Id, cancellationToken);
            if (entity == null) throw new NotFoundException($"Product Image {dto.Id} not found.");

            if (dto.ImgContent != null && dto.ImgContent.Length > 0)
            {
                var uploadRequest = new FileUploadRequest
                {
                    Content = dto.ImgContent,
                    FileName = dto.ImgFileName ?? $"img_{Guid.NewGuid()}",
                    ContentType = dto.ImgContentType ?? "image/jpeg"
                };
                entity.ImageUrl = await _fileService.UploadAsync(uploadRequest, cancellationToken);
            }

            if (dto.DisplayOrder != 0)
            {
                entity.DisplayOrder = dto.DisplayOrder;
            }

            if (dto.IsMain != entity.IsMain)
            {
                if (dto.IsMain)
                {
                    var currentMain = await _productImageRepository.FindOneAsync(
                        x => x.ProductColorId == entity.ProductColorId && x.IsMain && x.Id != entity.Id,
                        false,
                        cancellationToken);

                    if (currentMain != null)
                    {
                        currentMain.IsMain = false;
                        _productImageRepository.Update(currentMain);
                        await _context.SaveChangesAsync(cancellationToken);
                    }

                    entity.IsMain = true;
                }
                else
                {
                    var candidate = await _productImageRepository.FindOneAsync(
                        x => x.ProductColorId == entity.ProductColorId && !x.IsMain && x.Id != entity.Id,
                        false,
                        cancellationToken);

                    if (candidate != null)
                    {
                        entity.IsMain = false;
                        _productImageRepository.Update(entity);
                        await _context.SaveChangesAsync(cancellationToken);

                        candidate.IsMain = true;
                        _productImageRepository.Update(candidate);
                    }
                }
            }

            _productImageRepository.Update(entity);
            await _context.SaveChangesAsync(cancellationToken);

            return _mapper.Map<UpsertProductImageDto>(entity);
        }
    }
}


