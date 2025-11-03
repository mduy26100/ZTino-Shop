using Application.Common.Interfaces.Persistence.EFCore;
using Application.Features.Products.DTOs.Categories;
using Application.Features.Products.Interfaces.Services.Commands.Categories.UpdateCategory;
using Application.Features.Products.Repositories;

namespace Application.Features.Products.Services.Commands.Categories.UpdateCategory
{
    public class UpdateCategoryService : IUpdateCategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        private readonly IApplicationDbContext _context;

        public UpdateCategoryService(ICategoryRepository categoryRepository,
            IMapper mapper,
            IApplicationDbContext context)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
            _context = context;
        }

        public async Task<UpsertCategoryDto> UpdateAsync(UpsertCategoryDto category, CancellationToken cancellationToken = default)
        {
            var entity = await _categoryRepository.GetByIdAsync(category.Id, cancellationToken);

            if (entity == null)
                throw new KeyNotFoundException($"Category with Id {category.Id} not found.");

            bool nameExists = await _categoryRepository.AnyAsync(c => c.Name == category.Name && c.Id != category.Id);
            if (nameExists)
                throw new InvalidOperationException("Category with the same name already exists.");

            if (category.ParentId != null)
            {
                bool parentExists = await _categoryRepository.AnyAsync(c => c.Id == category.ParentId);
                if (!parentExists)
                    throw new InvalidOperationException("Parent category does not exist.");
            }

            _mapper.Map(category, entity);

            _categoryRepository.Update(entity);
            await _context.SaveChangesAsync(cancellationToken);

            return _mapper.Map<UpsertCategoryDto>(entity);
        }
    }
}
