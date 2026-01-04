using Application.Common.Interfaces.Identity;
using Application.Features.Carts.v1.DTOs;
using Application.Features.Carts.v1.Repositories;
using Application.Features.Carts.v1.Services;

namespace Application.Features.Carts.v1.Queries.GetCartById
{
    public class GetCartByIdHandler : IRequestHandler<GetCartByIdQuery, CartDto>
    {
        private readonly ICartRepository _cartRepository;
        private readonly ICartQueryService _cartQueryService;
        private readonly ICurrentUser _currentUser;

        public GetCartByIdHandler(
            ICartRepository cartRepository,
            ICartQueryService cartQueryService,
            ICurrentUser currentUser)
        {
            _cartRepository = cartRepository;
            _cartQueryService = cartQueryService;
            _currentUser = currentUser;
        }

        public async Task<CartDto> Handle(GetCartByIdQuery request, CancellationToken cancellationToken)
        {
            var userId = GetCurrentUserId();

            CartDto? cart;

            if (userId.HasValue)
            {
                cart = await GetAuthenticatedUserCartAsync(userId.Value, request.CartId, cancellationToken);
            }
            else
            {
                cart = await GetGuestCartAsync(request.CartId, cancellationToken);
            }

            return cart ?? CreateEmptyCart();
        }

        private Guid? GetCurrentUserId()
        {
            var userId = _currentUser.UserId;
            return userId == Guid.Empty ? null : userId;
        }

        private async Task<CartDto?> GetAuthenticatedUserCartAsync(
            Guid userId,
            Guid? requestedCartId,
            CancellationToken cancellationToken)
        {
            if (requestedCartId.HasValue && requestedCartId.Value != Guid.Empty)
            {
                await ValidateCartOwnershipAsync(requestedCartId.Value, userId, cancellationToken);
                return await _cartQueryService.GetCartByIdAsync(requestedCartId.Value, cancellationToken);
            }

            return await _cartQueryService.GetCartByUserIdAsync(userId, cancellationToken);
        }

        private async Task<CartDto?> GetGuestCartAsync(Guid? cartId, CancellationToken cancellationToken)
        {
            if (!cartId.HasValue || cartId.Value == Guid.Empty)
            {
                return null;
            }

            await ValidateGuestCartAccessAsync(cartId.Value, cancellationToken);

            return await _cartQueryService.GetCartByIdAsync(cartId.Value, cancellationToken);
        }

        private async Task ValidateCartOwnershipAsync(Guid cartId, Guid userId, CancellationToken cancellationToken)
        {
            var cart = await _cartRepository.FindOneAsync(
                c => c.Id == cartId && c.IsActive,
                asNoTracking: true,
                cancellationToken);

            if (cart is null)
            {
                throw new NotFoundException($"Cart with ID {cartId} not found.");
            }

            if (cart.UserId.HasValue && cart.UserId.Value != userId)
            {
                throw new ForbiddenException("You do not have permission to view this cart.");
            }
        }

        private async Task ValidateGuestCartAccessAsync(Guid cartId, CancellationToken cancellationToken)
        {
            var cart = await _cartRepository.FindOneAsync(
                c => c.Id == cartId && c.IsActive,
                asNoTracking: true,
                cancellationToken);

            if (cart is null)
            {
                throw new NotFoundException($"Cart with ID {cartId} not found.");
            }

            if (cart.UserId.HasValue)
            {
                throw new ForbiddenException("Authentication required to view this cart.");
            }
        }

        private static CartDto CreateEmptyCart()
        {
            return new CartDto
            {
                CartId = Guid.Empty,
                Items = new List<CartItemDto>(),
                TotalPrice = 0,
                TotalItems = 0
            };
        }
    }
}
