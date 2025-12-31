using Application.Common.Interfaces.Persistence.Data;
using Application.Features.Products.v1.DTOs.Categories;
using Application.Features.Products.v1.Repositories;

namespace Application.Features.Products.v1.Commands.Categories.UpdateCategory
{
    public class UpdateCategoryHandler : IRequestHandler<UpdateCategoryCommand, UpsertCategoryDto>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        private readonly IApplicationDbContext _context;

        public UpdateCategoryHandler(
            ICategoryRepository categoryRepository,
            IMapper mapper,
            IApplicationDbContext context)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
            _context = context;
        }

        public async Task<UpsertCategoryDto> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            var dto = request.Dto;

            var entity = await _categoryRepository.GetByIdAsync(dto.Id, cancellationToken);
            if (entity == null)
                throw new NotFoundException($"Category with Id {dto.Id} not found.");

            bool nameExists = await _categoryRepository.AnyAsync(
                c => c.Name == dto.Name && c.Id != dto.Id,
                cancellationToken);
            if (nameExists)
                throw new ConflictException("Category with the same name already exists.");

            if (dto.ParentId != null)
            {
                var parentCategory = await _categoryRepository.GetByIdAsync(dto.ParentId.Value, cancellationToken);
                if (parentCategory == null)
                    throw new NotFoundException("Parent category does not exist.");

                if (parentCategory.ParentId != null)
                    throw new BusinessRuleException("Cannot assign a parent that is already a child. Only 1 level of hierarchy allowed.");
            }

            _mapper.Map(dto, entity);

            _categoryRepository.Update(entity);
            await _context.SaveChangesAsync(cancellationToken);

            return _mapper.Map<UpsertCategoryDto>(entity);
        }
    }
}
