using Application.Features.Orders.v1.DTOs;

namespace Application.Features.Orders.v1.Services
{
    public interface IOrderQueryService
    {
        Task<IEnumerable<OrderSummaryDto>> GetAllOrdersAsync(CancellationToken cancellationToken = default);
        Task<OrderLookupResponseDto?> GetGuestOrderByCodeAndPhoneAsync(string orderCode, string phoneNumber, CancellationToken cancellationToken = default);
        Task<OrderLookupResponseDto?> GetMyOrderDetail(string orderCode, Guid userId, CancellationToken cancellationToken = default);
        Task<IEnumerable<OrderSummaryDto>> GetMyOrdersAsync(Guid userId, CancellationToken cancellationToken = default);
        Task<OrderLookupResponseDto?> GetOrderDetail(string orderCode, CancellationToken cancellationToken = default);
    }
}