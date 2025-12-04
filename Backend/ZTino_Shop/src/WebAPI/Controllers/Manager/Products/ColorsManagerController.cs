using Application.Features.Products.Commands.Colors.CreateColor;
using Application.Features.Products.Commands.Colors.DeleteColor;
using Application.Features.Products.Commands.Colors.UpdateColor;

namespace WebAPI.Controllers.Manager.Products
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ColorsManagerController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ColorsManagerController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateColor(CreateColorCommand command, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateColor(UpdateColorCommand command, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return Ok(result);
        }

        [HttpDelete("{Id:int}")]
        public async Task<IActionResult> DeleteColor(int Id, CancellationToken cancellationToken)
        {
            var command = new DeleteColorCommand(Id);
            var result = await _mediator.Send(command, cancellationToken);
            return Ok(result);
        }
    }
}
