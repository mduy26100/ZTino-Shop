using Application.Common.Interfaces.Persistence.Data;
using Application.Common.Interfaces.Services.FileUpLoad;
using Application.Common.Models;
using Application.Features.Products.Commands.ProductImages.UpdateProductImage.Strategies;
using Application.Features.Products.DTOs.ProductImages;
using Application.Features.Products.Repositories;
using Domain.Models.Products;

namespace Application.Features.Products.Commands.ProductImages.UpdateProductImage
{
    public class UpdateProductImageHandler : IRequestHandler<UpdateProductImageCommand, UpsertProductImageDto>
    {
        private readonly IProductImageRepository _repo;
        private readonly IFileUploadService _fileService;
        private readonly IMapper _mapper;
        private readonly IApplicationDbContext _context;

        public UpdateProductImageHandler(
            IProductImageRepository repo,
            IFileUploadService fileService,
            IMapper mapper,
            IApplicationDbContext context)
        {
            _repo = repo;
            _fileService = fileService;
            _mapper = mapper;
            _context = context;
        }

        public async Task<UpsertProductImageDto> Handle(UpdateProductImageCommand request, CancellationToken cancellationToken)
        {
            var dto = request.Dto;

            var entity = await _repo.GetByIdAsync(dto.Id, cancellationToken);
            if (entity == null) throw new KeyNotFoundException($"Product Image {dto.Id} not found.");

            await HandleImageUploadAsync(entity, dto, cancellationToken);

            if (dto.DisplayOrder != 0) entity.DisplayOrder = dto.DisplayOrder;

            IProductImageUpdateStrategy strategy = GetUpdateStrategy(entity.IsMain, dto.IsMain);

            await strategy.ExecuteAsync(entity, _repo, _context, cancellationToken);

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
                _repo.Update(entity);
            }
        }

        private IProductImageUpdateStrategy GetUpdateStrategy(bool currentIsMain, bool requestedIsMain)
        {
            if (requestedIsMain && !currentIsMain)
            {
                return new ClaimMainStrategy();
            }

            if (!requestedIsMain && currentIsMain)
            {
                return new ResignMainStrategy();
            }

            return new DefaultUpdateStrategy();
        }
    }
}