using Application.Features.Carts.v1.Commands.Carts.CreateCart;
using Application.Features.Carts.v1.Commands.Carts.DeleteCart;
using Application.Features.Carts.v1.Commands.Carts.UpdateCart;

namespace WebAPI.Controllers.v1.Carts
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/carts")]
    public class CartsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CartsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateCart(CreateCartCommand command, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCart(UpdateCartCommand command, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return Ok(result);
        }

        [HttpDelete("{cartItemId}")]
        public async Task<IActionResult> DeleteCartItem(int cartItemId, CancellationToken cancellationToken)
        {
            var command = new DeleteCartCommand(cartItemId);
            await _mediator.Send(command, cancellationToken);
            return NoContent();
        }
    }
}
