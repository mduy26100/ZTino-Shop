using Application.Features.Products.DTOs.Categories;
using Application.Features.Products.Interfaces.Services.Commands.Categories.CreateCategory;

namespace Application.Features.Products.Commands.Categories.CreateCategory
{
    public class CreateCategoryHandler : IRequestHandler<CreateCategoryCommand, UpsertCategoryDto>
    {
        private readonly ICreateCategoryService _createCategoryService;

        public CreateCategoryHandler(ICreateCategoryService createCategoryService)
        {
            _createCategoryService = createCategoryService;
        }

        public async Task<UpsertCategoryDto> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            var result = await _createCategoryService.CreateAsync(request.Dto, cancellationToken);
            return result;
        }
    }
}
