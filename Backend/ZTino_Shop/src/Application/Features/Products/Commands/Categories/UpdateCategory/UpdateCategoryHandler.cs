using Application.Features.Products.DTOs.Categories;
using Application.Features.Products.Interfaces.Services.Commands.Categories.UpdateCategory;

namespace Application.Features.Products.Commands.Categories.UpdateCategory
{
    public class UpdateCategoryHandler : IRequestHandler<UpdateCategoryCommand, UpsertCategoryDto>
    {
        private readonly IUpdateCategoryService _updateCategoryService;

        public UpdateCategoryHandler(IUpdateCategoryService updateCategoryService)
        {
            _updateCategoryService = updateCategoryService;
        }

        public async Task<UpsertCategoryDto> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            var result = await _updateCategoryService.UpdateAsync(request.Dto, cancellationToken);
            return result;
        }
    }
}
