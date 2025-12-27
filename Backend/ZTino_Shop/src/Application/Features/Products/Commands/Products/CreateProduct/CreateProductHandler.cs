using Application.Common.Interfaces.Persistence.Data;
using Application.Common.Interfaces.Services.FileUpLoad;
using Application.Common.Models.Requests;
using Application.Features.Products.DTOs.Products;
using Application.Features.Products.Repositories;
using Domain.Models.Products;

namespace Application.Features.Products.Commands.Products.CreateProduct
{
    public class CreateProductHandler : IRequestHandler<CreateProductCommand, UpsertProductDto>
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IFileUploadService _fileUploadService;
        private readonly IMapper _mapper;
        private readonly IApplicationDbContext _context;

        public CreateProductHandler(IProductRepository productRepository,
            ICategoryRepository categoryRepository,
            IFileUploadService fileUploadService,
            IMapper mapper,
            IApplicationDbContext context)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _fileUploadService = fileUploadService;
            _mapper = mapper;
            _context = context;
        }

        public async Task<UpsertProductDto> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var dto = request.Dto;

            var categoryExists = await _categoryRepository.GetByIdAsync(dto.CategoryId, cancellationToken);
            if (categoryExists == null)
                throw new NotFoundException($"Category with ID {dto.CategoryId} does not exist.");

            if(categoryExists.ParentId == null)
                throw new BusinessRuleException("Product must be assigned to a sub-category, not a root category.");

            bool nameExists = await _productRepository.AnyAsync(p => p.Name == dto.Name, cancellationToken);
            if (nameExists)
                throw new ConflictException("Product with the same name already exists.");

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

            var entity = _mapper.Map<Product>(dto);

            entity.MainImageUrl = string.IsNullOrWhiteSpace(imgUrl)
                ? dto.MainImageUrl
                : imgUrl;

            await _productRepository.AddAsync(entity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return _mapper.Map<UpsertProductDto>(entity);
        }
    }
}
