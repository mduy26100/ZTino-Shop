using Application.Features.Orders.v1.Commands.Orders.CreateOrder;
using Application.Features.Orders.v1.Commands.Orders.UpdateMyOrderStatus;
using Application.Features.Orders.v1.DTOs;
using Application.Features.Orders.v1.Queries.GetMyOrderDetail;
using Application.Features.Orders.v1.Queries.GetMyOrders;
using Application.Features.Orders.v1.Queries.GetOrderLookup;

namespace WebAPI.Controllers.v1.Orders
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/orders")]
    public class OrdersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OrdersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder(CreateOrderCommand command, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return Ok(result);
        }

        [HttpPost("lookup")]
        public async Task<IActionResult> GetOrderById(GetOrderLookupRequestDto dto, CancellationToken cancellationToken)
        {
            var query = new GetOrderLookupQuery(dto);
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(result);
        }

        [HttpGet("my-orders")]
        [Authorize]
        public async Task<IActionResult> GetMyOrders(CancellationToken cancellationToken)
        {
            var query = new GetMyOrdersQuery();
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(result);
        }

        [HttpGet("{orderCode}")]
        [Authorize]
        public async Task<IActionResult> GetMyOrderDetail(string orderCode, CancellationToken cancellationToken)
        {
            var query = new GetMyOrderDetailQuery(orderCode);
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(result);
        }

        [HttpPatch("{orderId:guid}/status")]
        [Authorize]
        public async Task<IActionResult> UpdateMyOrderStatus(
            Guid orderId,
            UpdateMyOrderStatusCommand command,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return Ok(result);
        }
    }
}