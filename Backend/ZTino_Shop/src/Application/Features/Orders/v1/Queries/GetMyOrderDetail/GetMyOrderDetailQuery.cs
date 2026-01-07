using Application.Features.Orders.v1.DTOs;

namespace Application.Features.Orders.v1.Queries.GetMyOrderDetail
{
    public record GetMyOrderDetailQuery(string orderCode) : IRequest<OrderLookupResponseDto?>;
}
