using Application.Features.Orders.v1.DTOs;

namespace Application.Features.Orders.v1.Commands.Orders.UpdateMyOrderStatus
{
    public record UpdateMyOrderStatusCommand(UpdateOrderDto Dto) : IRequest<UpdateOrderResponseDto>;
}
