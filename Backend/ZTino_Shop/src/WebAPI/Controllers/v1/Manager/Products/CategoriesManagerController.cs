using Application.Features.Products.v1.Commands.Categories.DeleteCategory;
using Application.Features.Products.v1.Commands.Categories.CreateCategory;
using Application.Features.Products.v1.Commands.Categories.UpdateCategory;
using Domain.Consts;

namespace WebAPI.Controllers.v1.Manager.Products
{
    [ApiController]
    [ApiVersion("1.0")]
    [Authorize(Roles = Roles.Manager)]
    [Route("api/v{version:apiVersion}/admin/categories")]
    public class CategoriesManagerController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CategoriesManagerController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory(CreateCategoryCommand command, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return Ok(result);
        }

        [HttpDelete("{Id:int}")]
        public async Task<IActionResult> DeleteCategory(int Id, CancellationToken cancellationToken)
        {
            var command = new DeleteCategoryCommand(Id);
            var result = await _mediator.Send(command, cancellationToken);
            return NoContent();
        }

        [HttpPut("{Id:int}")]
        public async Task<IActionResult> UpdateCategory(int Id, UpdateCategoryCommand command, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return Ok(result);
        }
    }
}
