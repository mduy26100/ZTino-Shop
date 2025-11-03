using Application.Common.Interfaces.Persistence.EFCore;
using Application.Features.Products.DTOs.Categories;
using Application.Features.Products.Interfaces.Services.Commands.Categories.CreateCategory;
using Application.Features.Products.Repositories;
using Domain.Models.Products;

namespace Application.Features.Products.Services.Commands.Categories.CreateCategory
{
    public class CreateCategoryService : ICreateCategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        private readonly IApplicationDbContext _context;

        public CreateCategoryService(ICategoryRepository categoryRepository,
            IMapper mapper,
            IApplicationDbContext context)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
            _context = context;
        }

        public async Task<UpsertCategoryDto> CreateAsync(UpsertCategoryDto category, CancellationToken cancellationToken = default)
        {
            if (category.ParentId != null)
            {
                bool parentExists = await _categoryRepository.AnyAsync(c => c.Id == category.ParentId);
                if (!parentExists)
                    throw new InvalidOperationException("Parent category does not exist.");
            }

            bool nameExists = await _categoryRepository.AnyAsync(c => c.Name == category.Name);
            if (nameExists)
                throw new InvalidOperationException("Category with the same name already exists.");


            var entity = _mapper.Map<Category>(category);
            await _categoryRepository.AddAsync(entity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return _mapper.Map<UpsertCategoryDto>(entity);
        }
    }
}
