using Application.Features.Products.v1.DTOs.Categories;
using Application.Features.Products.v1.Repositories;
using Domain.Models.Products;

namespace Application.Features.Products.v1.Commands.Categories.CreateCategory
{
    public class CreateCategoryHandler : IRequestHandler<CreateCategoryCommand, UpsertCategoryDto>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        private readonly IFileUploadService _fileUploadService;
        private readonly IApplicationDbContext _context;

        public CreateCategoryHandler(
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

        public async Task<UpsertCategoryDto> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            var dto = request.Dto;

            if (dto.ParentId != null)
            {
                var parentCategory = await _categoryRepository.GetByIdAsync(dto.ParentId.Value, cancellationToken);
                if (parentCategory == null)
                    throw new NotFoundException("Parent category does not exist.");

                if (parentCategory.ParentId != null)
                    throw new BusinessRuleException("Cannot assign a parent that is already a child. Only 1 level of hierarchy allowed.");

                bool hasImageUrl = !string.IsNullOrWhiteSpace(dto.ImageUrl);
                bool hasImageFile = dto.ImgContent != null && dto.ImgContent.Length > 0;

                if (hasImageUrl || hasImageFile)
                {
                    throw new BusinessRuleException("Child categories cannot have an image.");
                }
            }

            var duplicate = await _categoryRepository.FindOneAsync(c =>
                c.Name == dto.Name || c.Slug == dto.Slug,
                asNoTracking: true,
                cancellationToken: cancellationToken);

            if (duplicate != null)
            {
                if (duplicate.Name == dto.Name)
                    throw new ConflictException($"Category name '{dto.Name}' already exists.");

                if (duplicate.Slug == dto.Slug)
                    throw new ConflictException($"Slug '{dto.Slug}' is already in use.");
            }

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

            var entity = _mapper.Map<Category>(dto);

            entity.ImageUrl = string.IsNullOrWhiteSpace(imgUrl)
                ? dto.ImageUrl
                : imgUrl;

            await _categoryRepository.AddAsync(entity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return _mapper.Map<UpsertCategoryDto>(entity);
        }
    }
}


