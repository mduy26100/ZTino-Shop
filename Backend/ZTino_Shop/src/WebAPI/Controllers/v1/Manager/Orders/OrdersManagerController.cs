using Application.Features.Orders.v1.Commands.Orders.UpdateOrderStatus;
using Application.Features.Orders.v1.Queries.GetAllOrders;
using Application.Features.Orders.v1.Queries.GetOrderDetail;
using Domain.Consts;

namespace WebAPI.Controllers.v1.Manager.Orders
{
    [ApiController]
    [ApiVersion("1.0")]
    [Authorize(Roles = Roles.Manager)]
    [Route("api/v{version:apiVersion}/admin/orders")]
    public class OrdersManagerController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OrdersManagerController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllOrders(CancellationToken cancellationToken)
        {
            var query = new GetAllOrdersQuery();
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(result);
        }

        [HttpGet("{orderCode}")]
        public async Task<IActionResult> GetOrderDetail(string orderCode, CancellationToken cancellationToken)
        {
            var query = new GetOrderDetailQuery(orderCode);
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(result);
        }

        [HttpPut("{orderId:guid}")]
        public async Task<IActionResult> UpdateOrderStatus(Guid orderId, UpdateOrderStatusCommand command, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return Ok(result);
        }
    }
}