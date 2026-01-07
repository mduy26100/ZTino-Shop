using Application.Features.Orders.v1.DTOs;
using Application.Features.Orders.v1.Expressions;
using Application.Features.Orders.v1.Services;
using Infrastructure.Persistence;

namespace Infrastructure.Orders.Services
{
    public class OrderQueryService : IOrderQueryService
    {
        private readonly ApplicationDbContext _context;

        public OrderQueryService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<OrderLookupResponseDto?> GetGuestOrderByCodeAndPhoneAsync(string orderCode, string phoneNumber, CancellationToken cancellationToken = default)
        {
            return await _context.Orders
                .AsNoTracking()
                .Where(x => x.OrderCode == orderCode
                            && x.CustomerPhone == phoneNumber
                            && x.UserId == null)
                .Select(OrderExpressions.OrderLookupProjection)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<IEnumerable<OrderSummaryDto>> GetMyOrdersAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            return await _context.Orders
                .AsNoTracking()
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.CreatedAt)
                .Select(OrderExpressions.SummaryProjection)
                .ToListAsync(cancellationToken);
        }
    }
}