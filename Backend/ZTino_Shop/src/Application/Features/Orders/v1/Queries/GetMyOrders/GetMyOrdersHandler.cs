using Application.Features.Orders.v1.DTOs;
using Application.Features.Orders.v1.Services;

namespace Application.Features.Orders.v1.Queries.GetMyOrders
{
    public class GetMyOrdersHandler : IRequestHandler<GetMyOrdersQuery, IEnumerable<OrderSummaryDto>>
    {
        private readonly IOrderQueryService _orderQueryService;
        private readonly ICurrentUser _currentUser;

        public GetMyOrdersHandler(IOrderQueryService orderQueryService, ICurrentUser currentUser)
        {
            _orderQueryService = orderQueryService;
            _currentUser = currentUser;
        }

        public async Task<IEnumerable<OrderSummaryDto>> Handle(GetMyOrdersQuery request, CancellationToken cancellationToken)
        {
            var userId = _currentUser.UserId;

            if (userId == Guid.Empty)
            {
                throw new UnauthorizedAccessException("User is not authenticated.");
            }

            var orders = await _orderQueryService.GetMyOrdersAsync(userId, cancellationToken);

            return orders;
        }
    }
}