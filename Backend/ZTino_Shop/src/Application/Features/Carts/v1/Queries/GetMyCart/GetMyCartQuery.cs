using Application.Features.Carts.v1.DTOs;

namespace Application.Features.Carts.v1.Queries.GetMyCart
{
    /// <summary>
    /// Query to retrieve the authenticated user's cart.
    /// Uses UserId from the authentication token.
    /// </summary>
    public record GetMyCartQuery() : IRequest<CartDto>;
}
