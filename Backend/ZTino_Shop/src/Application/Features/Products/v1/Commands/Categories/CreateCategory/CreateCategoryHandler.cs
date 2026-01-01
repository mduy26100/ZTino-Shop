using Application.Common.Interfaces.Persistence.Data;
using Application.Common.Interfaces.Services.FileUpLoad;
using Application.Common.Models.Requests;
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

                if (dto.ImageUrl != null)
                    throw new BusinessRuleException("Child categories cannot have an image.");
            }

            bool nameExists = await _categoryRepository.AnyAsync(c => c.Name == dto.Name, cancellationToken);
            if (nameExists)
                throw new ConflictException("Category with the same name already exists.");

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