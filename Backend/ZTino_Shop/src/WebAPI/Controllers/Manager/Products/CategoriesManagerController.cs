using Application.Features.Products.Commands.Categories.CreateCategory;
using Domain.Consts;
using Microsoft.AspNetCore.Authorization;

namespace WebAPI.Controllers.Manager.Products
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = Roles.Manager)]
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
    }
}
