using Application.Features.Carts.v1.DTOs;

namespace Application.Features.Carts.v1.Queries.GetMyCart
{
    public record GetMyCartQuery() : IRequest<CartDto>;
}
