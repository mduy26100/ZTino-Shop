using Application.Features.Orders.v1.DTOs;
using Application.Features.Orders.v1.Services;

namespace Application.Features.Orders.v1.Queries.GetOrderDetail
{
    public class GetOrderDetailHandler : IRequestHandler<GetOrderDetailQuery, OrderLookupResponseDto?>
    {
        private readonly IOrderQueryService _orderQueryService;

        public GetOrderDetailHandler(IOrderQueryService orderQueryService)
        {
            _orderQueryService = orderQueryService;
        }

        public async Task<OrderLookupResponseDto?> Handle(GetOrderDetailQuery request, CancellationToken cancellationToken)
        {
            return await _orderQueryService.GetOrderDetail(request.orderCode, cancellationToken);
        }
    }
}
