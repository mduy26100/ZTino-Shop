using Application.Common.Interfaces.Persistence.Data;
using Application.Features.Products.DTOs.Categories;
using Application.Features.Products.Repositories;
using Domain.Models.Products;

namespace Application.Features.Products.Commands.Categories.CreateCategory
{
    public class CreateCategoryHandler : IRequestHandler<CreateCategoryCommand, UpsertCategoryDto>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        private readonly IApplicationDbContext _context;

        public CreateCategoryHandler(
            ICategoryRepository categoryRepository,
            IMapper mapper,
            IApplicationDbContext context)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
            _context = context;
        }

        public async Task<UpsertCategoryDto> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            var dto = request.Dto;

            if (dto.ParentId != null)
            {
                var parentCategory = await _categoryRepository.GetByIdAsync(dto.ParentId.Value, cancellationToken);
                if (parentCategory == null)
                    throw new InvalidOperationException("Parent category does not exist.");

                if (parentCategory.ParentId != null)
                    throw new InvalidOperationException("Cannot assign a parent that is already a child. Only 1 level of hierarchy allowed.");
            }

            bool nameExists = await _categoryRepository.AnyAsync(c => c.Name == dto.Name, cancellationToken);
            if (nameExists)
                throw new InvalidOperationException("Category with the same name already exists.");

            var entity = _mapper.Map<Category>(dto);
            await _categoryRepository.AddAsync(entity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return _mapper.Map<UpsertCategoryDto>(entity);
        }
    }
}