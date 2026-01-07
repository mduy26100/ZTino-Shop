using Application.Features.Orders.v1.DTOs;

namespace Application.Features.Orders.v1.Queries.GetOrderLookup
{
    public record GetOrderLookupQuery(GetOrderLookupRequestDto Dto) : IRequest<OrderLookupResponseDto?>;
}