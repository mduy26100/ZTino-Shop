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
            if (entity == null) throw new NotFoundException($"Product Image {dto.Id} not found.");

            var siblings = await _repo.FindAsync(
                x => x.ProductColorId == entity.ProductColorId && x.Id != entity.Id,
                false,
                cancellationToken);

            await HandleImageUploadAsync(entity, dto, cancellationToken);

            if (dto.DisplayOrder != 0)
                entity.DisplayOrder = dto.DisplayOrder;

            HandleMainImageLogic(entity, siblings, dto.IsMain);

            _repo.Update(entity);

            await _context.SaveChangesAsync(cancellationToken);

            return _mapper.Map<UpsertProductImageDto>(entity);
        }

        private void HandleMainImageLogic(ProductImage currentEntity, IEnumerable<ProductImage> siblings, bool requestedIsMain)
        {
            if (requestedIsMain && !currentEntity.IsMain)
            {
                var oldMain = siblings.FirstOrDefault(x => x.IsMain);
                if (oldMain != null)
                {
                    oldMain.IsMain = false;
                    _repo.Update(oldMain);
                }
                currentEntity.IsMain = true;
            }
            else if (!requestedIsMain && currentEntity.IsMain)
            {
                currentEntity.IsMain = false;

                var heir = siblings.OrderBy(x => x.DisplayOrder).FirstOrDefault();

                if (heir != null)
                {
                    heir.IsMain = true;
                    _repo.Update(heir);
                }
                else
                {
                    currentEntity.IsMain = true;
                }
            }
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