using Application.Features.Orders.v1.DTOs;
using Application.Features.Orders.v1.Services;

namespace Application.Features.Orders.v1.Queries.GetMyOrderDetail
{
    public class GetMyOrderDetailHandler : IRequestHandler<GetMyOrderDetailQuery, OrderLookupResponseDto?>
    {
        private readonly IOrderQueryService _orderQueryService;
        private readonly ICurrentUser _currentUser;

        public GetMyOrderDetailHandler(IOrderQueryService orderQueryService, ICurrentUser currentUser)
        {
            _orderQueryService = orderQueryService;
            _currentUser = currentUser;
        }

        public async Task<OrderLookupResponseDto?> Handle(GetMyOrderDetailQuery request, CancellationToken cancellationToken)
        {
            Guid userId = _currentUser.UserId;
            if (userId == Guid.Empty)
            {
                throw new UnauthorizedAccessException("User is not authenticated.");
            }

            var orderDetail = await _orderQueryService.GetMyOrderDetail(request.orderCode, userId, cancellationToken);

            return orderDetail;
        }
    }
}
