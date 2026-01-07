using Application.Features.Products.v1.DTOs.Categories;
using Application.Features.Products.v1.Repositories;

namespace Application.Features.Products.v1.Commands.Categories.UpdateCategory
{
    public class UpdateCategoryHandler : IRequestHandler<UpdateCategoryCommand, UpsertCategoryDto>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        private readonly IFileUploadService _fileUploadService;
        private readonly IApplicationDbContext _context;

        public UpdateCategoryHandler(
            ICategoryRepository categoryRepository,
            IMapper mapper,
            IFileUploadService fileUploadService,
            IApplicationDbContext context)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
            _fileUploadService = fileUploadService;
            _context = context;
        }

        public async Task<UpsertCategoryDto> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            var dto = request.Dto;

            var entity = await _categoryRepository
                .GetByIdAsync(dto.Id, cancellationToken);

            if (entity is null)
                throw new NotFoundException($"Category with Id {dto.Id} not found.");

            var duplicate = await _categoryRepository.FindOneAsync(c =>
                c.Id != dto.Id && (c.Name == dto.Name || c.Slug == dto.Slug),
                asNoTracking: true,
                cancellationToken: cancellationToken);

            if (duplicate != null)
            {
                if (duplicate.Name.Equals(dto.Name, StringComparison.OrdinalIgnoreCase))
                    throw new ConflictException($"Category name '{dto.Name}' already exists.");

                if (duplicate.Slug.Equals(dto.Slug, StringComparison.OrdinalIgnoreCase))
                    throw new ConflictException($"Slug '{dto.Slug}' is already in use.");
            }

            if (dto.ParentId.HasValue)
            {
                var parent = await _categoryRepository
                    .GetByIdAsync(dto.ParentId.Value, cancellationToken);

                if (parent is null)
                    throw new NotFoundException("Parent category does not exist.");

                if (parent.ParentId.HasValue)
                    throw new BusinessRuleException(
                        "Only one level of category hierarchy is allowed.");
            }

            var isChildCategory = dto.ParentId.HasValue;
            var isUploadingImage =
                dto.ImgContent is not null &&
                !string.IsNullOrWhiteSpace(dto.ImgFileName);

            if (isChildCategory && isUploadingImage)
                throw new BusinessRuleException(
                    "Only root categories can have images.");

            string? uploadedImageUrl = null;

            if (isUploadingImage)
            {
                var uploadRequest = new FileUploadRequest
                {
                    Content = dto.ImgContent!,
                    FileName = dto.ImgFileName!,
                    ContentType = dto.ImgContentType ?? "image/jpeg"
                };

                uploadedImageUrl = await _fileUploadService
                    .UploadAsync(uploadRequest, cancellationToken);
            }

            var currentImageUrl = entity.ImageUrl;

            _mapper.Map(dto, entity);

            if (!string.IsNullOrWhiteSpace(uploadedImageUrl))
            {
                entity.ImageUrl = uploadedImageUrl;
            }
            else
            {
                entity.ImageUrl = currentImageUrl;
            }

            _categoryRepository.Update(entity);
            await _context.SaveChangesAsync(cancellationToken);

            return _mapper.Map<UpsertCategoryDto>(entity);
        }
    }
}