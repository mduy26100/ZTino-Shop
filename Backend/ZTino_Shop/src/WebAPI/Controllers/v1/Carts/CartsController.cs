using Application.Features.Carts.v1.Commands.Carts.CreateCart;
using Application.Features.Carts.v1.Commands.Carts.DeleteCart;
using Application.Features.Carts.v1.Commands.Carts.UpdateCart;
using Application.Features.Carts.v1.Queries.GetCartById;
using Application.Features.Carts.v1.Queries.GetMyCart;

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

        [HttpPut("{cartId:guid}")]
        public async Task<IActionResult> UpdateCart(Guid cartId, UpdateCartCommand command, CancellationToken cancellationToken)
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

        [HttpGet("{cartId:guid}")]
        public async Task<IActionResult> GetCartById(Guid cartId, CancellationToken cancellationToken)
        {
            var query = new GetCartByIdQuery(cartId);
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetMyCart(CancellationToken cancellationToken)
        {
            var query = new GetMyCartQuery();
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(result);
        }
    }
}
