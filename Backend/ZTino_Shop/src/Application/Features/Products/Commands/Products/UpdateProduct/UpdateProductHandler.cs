using Application.Common.Interfaces.Persistence.Data;
using Application.Common.Interfaces.Services.FileUpLoad;
using Application.Common.Models.Requests;
using Application.Features.Products.DTOs.Products;
using Application.Features.Products.Repositories;

namespace Application.Features.Products.Commands.Products.UpdateProduct
{
    public class UpdateProductHandler : IRequestHandler<UpdateProductCommand, UpsertProductDto>
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IFileUploadService _fileUploadService;
        private readonly IMapper _mapper;
        private readonly IApplicationDbContext _context;

        public UpdateProductHandler(IProductRepository productRepository,
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

        public async Task<UpsertProductDto> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var dto = request.Dto;

            var entity = await _productRepository.GetByIdAsync(dto.Id, cancellationToken);
            if (entity == null)
                throw new NotFoundException($"Product with Id {dto.Id} not found.");

            var categoryExists = await _categoryRepository.GetByIdAsync(dto.CategoryId, cancellationToken);
            if (categoryExists == null)
                throw new NotFoundException($"Category with ID {dto.CategoryId} does not exist.");

            if (categoryExists.ParentId == null)
                throw new InvalidOperationException("Product must be assigned to a sub-category, not a root category.");

            bool nameExists = await _productRepository.AnyAsync(p => p.Name == dto.Name && p.Id != dto.Id, cancellationToken);
            if (nameExists)
                throw new InvalidOperationException("Product with the same name already exists.");

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

            _mapper.Map(dto, entity);

            entity.MainImageUrl = !string.IsNullOrWhiteSpace(imgUrl)
                ? imgUrl
                : entity.MainImageUrl ?? string.Empty;

            _productRepository.Update(entity);
            await _context.SaveChangesAsync(cancellationToken);

            return _mapper.Map<UpsertProductDto>(entity);
        }
    }
}
