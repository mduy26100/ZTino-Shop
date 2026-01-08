using Application.Features.Orders.v1.Queries.GetAllOrders;
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
    }
}