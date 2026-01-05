using Application.Features.Orders.v1.DTOs;

namespace Application.Features.Orders.v1.Commands.Orders.CreateOrder
{
    public record CreateOrderCommand(CreateOrderDto Dto) : IRequest<CreateOrderResponseDto>;
}
