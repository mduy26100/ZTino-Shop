namespace Domain.Models.Stats
{
    public class DailyRevenueStats
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }

        public int TotalOrders { get; set; }
        public decimal TotalRevenue { get; set; }
        public int NewCustomers { get; set; }
    }
}