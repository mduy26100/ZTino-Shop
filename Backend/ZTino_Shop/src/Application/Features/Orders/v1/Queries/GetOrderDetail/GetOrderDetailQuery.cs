using Application.Features.Orders.v1.DTOs;

namespace Application.Features.Orders.v1.Queries.GetOrderDetail
{
    public record GetOrderDetailQuery(string orderCode) : IRequest<OrderLookupResponseDto?>;
}
