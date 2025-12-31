using Application.Features.Products.v1.Commands.Colors.DeleteColor;
using Application.Features.Products.v1.Commands.Colors.CreateColor;
using Application.Features.Products.v1.Commands.Colors.UpdateColor;
using Domain.Consts;

namespace WebAPI.Controllers.v1.Manager.Products
{
    [ApiController]
    [ApiVersion("1.0")]
    [Authorize(Roles = Roles.Manager)]
    [Route("api/v{version:apiVersion}/admin/colors")]
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

        [HttpPut("{Id:int}")]
        public async Task<IActionResult> UpdateColor(int Id, UpdateColorCommand command, CancellationToken cancellationToken)
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
