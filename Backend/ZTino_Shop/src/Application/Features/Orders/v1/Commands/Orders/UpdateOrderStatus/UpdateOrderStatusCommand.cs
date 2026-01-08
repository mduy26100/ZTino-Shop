using Application.Features.Orders.v1.DTOs;

namespace Application.Features.Orders.v1.Commands.Orders.UpdateOrderStatus
{
    public record UpdateOrderStatusCommand(UpdateOrderDto Dto) : IRequest<UpdateOrderResponseDto>;
}
