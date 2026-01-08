using Application.Features.Orders.v1.DTOs;
using Application.Features.Orders.v1.Services;

namespace Application.Features.Orders.v1.Queries.GetAllOrders
{
    public class GetAllOrdersHandler : IRequestHandler<GetAllOrdersQuery, IEnumerable<OrderSummaryDto>>
    {
        private readonly IOrderQueryService _orderQueryService;

        public GetAllOrdersHandler(IOrderQueryService orderQueryService)
        {
            _orderQueryService = orderQueryService;
        }

        public async Task<IEnumerable<OrderSummaryDto>> Handle(GetAllOrdersQuery request, CancellationToken cancellationToken)
        {
            return await _orderQueryService.GetAllOrdersAsync(cancellationToken);
        }
    }
}
