using Application.Features.Orders.v1.DTOs;
using Application.Features.Orders.v1.Services;

namespace Application.Features.Orders.v1.Queries.GetOrderLookup
{
    public class GetOrderLookupHandler : IRequestHandler<GetOrderLookupQuery, OrderLookupResponseDto?>
    {
        private readonly IOrderQueryService _orderQueryService;

        public GetOrderLookupHandler(IOrderQueryService orderQueryService)
        {
            _orderQueryService = orderQueryService;
        }

        public async Task<OrderLookupResponseDto?> Handle(GetOrderLookupQuery request, CancellationToken cancellationToken)
        {
            return await _orderQueryService.GetGuestOrderByCodeAndPhoneAsync(
                request.Dto.OrderCode,
                request.Dto.CustomerPhone,
                cancellationToken);
        }
    }
}