using Domain.Models.Stats;

namespace Infrastructure.Data.Configurations.Stats
{
    internal class DailyRevenueStatsConfiguration : IEntityTypeConfiguration<DailyRevenueStats>
    {
        public void Configure(EntityTypeBuilder<DailyRevenueStats> builder)
        {
            builder.ToTable("DailyRevenueStats");

            builder.HasKey(s => s.Id);

            builder.Property(s => s.Id)
                .ValueGeneratedOnAdd();

            builder.Property(s => s.Date)
                .IsRequired()
                .HasColumnType("date");

            builder.Property(s => s.TotalOrders)
                .IsRequired();

            builder.Property(s => s.TotalRevenue)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(s => s.NewCustomers)
                .IsRequired();

            builder.HasIndex(s => s.Date)
                .IsUnique()
                .HasDatabaseName("IX_DailyRevenueStats_Date");
        }
    }
}
