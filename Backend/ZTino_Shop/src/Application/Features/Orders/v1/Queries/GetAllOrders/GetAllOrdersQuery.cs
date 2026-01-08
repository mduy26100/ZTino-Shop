using Application.Features.Orders.v1.DTOs;

namespace Application.Features.Orders.v1.Queries.GetAllOrders
{
    public record GetAllOrdersQuery() : IRequest<IEnumerable<OrderSummaryDto>>;
}
