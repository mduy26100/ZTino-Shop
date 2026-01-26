using Domain.Models.Stats;
using Infrastructure.Persistence.Constants;

namespace Infrastructure.Persistence.Configurations.Stats
{
    internal class DailyRevenueStatsConfiguration : IEntityTypeConfiguration<DailyRevenueStats>
    {
        public void Configure(EntityTypeBuilder<DailyRevenueStats> builder)
        {
            builder.ToTable("DailyRevenueStats", SchemaNames.Analytics);

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
