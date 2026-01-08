using Application.Features.Orders.v1.Repositories;
using Application.Features.Stats.v1.Repositories;
using Application.Features.Stats.v1.Services;
using Domain.Models.Orders;
using Domain.Models.Stats;

namespace Infrastructure.Stats.Services
{
    public class SalesStatsService : ISalesStatsService
    {
        private readonly IDailyRevenueStatsRepository _dailyRevenueStatsRepository;
        private readonly IProductSalesStatsRepository _productSalesStatsRepository;
        private readonly IOrderRepository _orderRepository;

        public SalesStatsService(
            IDailyRevenueStatsRepository dailyRevenueStatsRepository,
            IProductSalesStatsRepository productSalesStatsRepository,
            IOrderRepository orderRepository)
        {
            _dailyRevenueStatsRepository = dailyRevenueStatsRepository;
            _productSalesStatsRepository = productSalesStatsRepository;
            _orderRepository = orderRepository;
        }

        public async Task UpdateDailyRevenueStatsAsync(Order order, CancellationToken cancellationToken = default)
        {
            var today = DateTime.UtcNow.Date;

            var dailyStats = await _dailyRevenueStatsRepository
                .FindOneAsync(s => s.Date == today, asNoTracking: false, cancellationToken);

            if (dailyStats == null)
            {
                dailyStats = new DailyRevenueStats
                {
                    Date = today,
                    TotalOrders = 1,
                    TotalRevenue = order.TotalAmount,
                    NewCustomers = await IsNewCustomerAsync(order.UserId, order.Id, cancellationToken) ? 1 : 0
                };

                await _dailyRevenueStatsRepository.AddAsync(dailyStats, cancellationToken);
            }
            else
            {
                dailyStats.TotalOrders += 1;
                dailyStats.TotalRevenue += order.TotalAmount;

                if (await IsNewCustomerAsync(order.UserId, order.Id, cancellationToken))
                {
                    dailyStats.NewCustomers += 1;
                }
            }
        }

        public async Task UpdateProductSalesStatsAsync(ICollection<OrderItem> orderItems, CancellationToken cancellationToken = default)
        {
            var groupedItems = orderItems
                .GroupBy(item => item.ProductId)
                .Select(group => new
                {
                    ProductId = group.Key,
                    ProductName = group.First().ProductName,
                    Sku = group.First().Sku,
                    TotalQuantity = group.Sum(x => x.Quantity),
                    TotalRevenue = group.Sum(x => x.TotalLineAmount)
                })
                .ToList();

            foreach (var item in groupedItems)
            {
                var productStats = await _productSalesStatsRepository
                    .FindOneAsync(s => s.ProductId == item.ProductId, asNoTracking: false, cancellationToken);

                if (productStats == null)
                {
                    var newStats = new ProductSalesStats
                    {
                        ProductId = item.ProductId,
                        ProductName = item.ProductName,
                        Sku = item.Sku,
                        TotalSoldQuantity = item.TotalQuantity,
                        TotalRevenue = item.TotalRevenue,
                        LastSoldAt = DateTime.UtcNow
                    };

                    await _productSalesStatsRepository.AddAsync(newStats, cancellationToken);
                }
                else
                {
                    productStats.TotalSoldQuantity += item.TotalQuantity;
                    productStats.TotalRevenue += item.TotalRevenue;
                    productStats.LastSoldAt = DateTime.UtcNow;
                }
            }
        }

        private async Task<bool> IsNewCustomerAsync(Guid? userId, Guid currentOrderId, CancellationToken cancellationToken)
        {
            if (!userId.HasValue)
            {
                return true;
            }

            var hasPreviousOrders = await _orderRepository
                .HasPreviousDeliveredOrdersAsync(userId, currentOrderId, cancellationToken);

            return !hasPreviousOrders;
        }
    }
}
