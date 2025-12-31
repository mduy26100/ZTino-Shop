using Application.Features.Products.v1.Commands.Sizes.DeleteSize;
using Application.Features.Products.v1.Commands.Sizes.CreateSize;
using Application.Features.Products.v1.Commands.Sizes.UpdateSize;
using Domain.Consts;

namespace WebAPI.Controllers.v1.Manager.Products
{
    [ApiController]
    [ApiVersion("1.0")]
    [Authorize(Roles = Roles.Manager)]
    [Route("api/v{version:apiVersion}/admin/sizes")]
    public class SizesManagerController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SizesManagerController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateSize(CreateSizeCommand command, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return Ok(result);
        }

        [HttpPut("{Id:int}")]
        public async Task<IActionResult> UpdateSize(int Id, UpdateSizeCommand command, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return Ok(result);
        }

        [HttpDelete("{Id:int}")]
        public async Task<IActionResult> DeleteSize(int Id, CancellationToken cancellationToken)
        {
            var command = new DeleteSizeCommand(Id);
            var result = await _mediator.Send(command, cancellationToken);
            return Ok(result);
        }
    }
}
