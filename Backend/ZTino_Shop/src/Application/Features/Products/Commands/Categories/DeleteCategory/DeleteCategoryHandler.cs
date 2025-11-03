using Application.Features.Products.Interfaces.Services.Commands.Categories.DeleteCategory;

namespace Application.Features.Products.Commands.Categories.DeleteCategory
{
    public class DeleteCategoryHandler : IRequestHandler<DeleteCategoryCommand, Unit>
    {
        private readonly IDeleteCategoryService _deleteCategoryService;

        public DeleteCategoryHandler(IDeleteCategoryService deleteCategoryService)
        {
            _deleteCategoryService = deleteCategoryService;
        }

        public async Task<Unit> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            await _deleteCategoryService.DeleteAsync(request.Id, cancellationToken);

            return Unit.Value;
        }
    }
}
