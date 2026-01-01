using Application.Features.Products.v1.Commands.Categories.DeleteCategory;
using Application.Features.Products.v1.Commands.Categories.CreateCategory;
using Application.Features.Products.v1.Commands.Categories.UpdateCategory;
using Domain.Consts;
using WebAPI.Requests.Products.Product;

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
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> CreateCategory([FromForm]UpsertCategoryForm form, CancellationToken cancellationToken)
        {
            var command = new CreateCategoryCommand(form.ToDto());
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
