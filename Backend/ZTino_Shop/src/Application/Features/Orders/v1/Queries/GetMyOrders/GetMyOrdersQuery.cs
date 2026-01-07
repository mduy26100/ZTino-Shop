using Application.Features.Orders.v1.DTOs;

namespace Application.Features.Orders.v1.Queries.GetMyOrders
{
    public record GetMyOrdersQuery() : IRequest<IEnumerable<OrderSummaryDto>>;
}