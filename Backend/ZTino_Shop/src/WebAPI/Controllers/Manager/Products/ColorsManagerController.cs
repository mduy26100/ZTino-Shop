using Application.Features.Products.Commands.Colors.CreateColor;

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
    }
}
