using Application.Features.Carts.v1.DTOs;

namespace Application.Features.Carts.v1.Queries.GetCartById
{
    public record GetCartByIdQuery(Guid? CartId = null) : IRequest<CartDto>;
}
