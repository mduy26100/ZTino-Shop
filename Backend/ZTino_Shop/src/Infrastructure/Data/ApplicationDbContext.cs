using Application.Common.Interfaces.Persistence.Data;
using Domain.Models.Carts;
using Domain.Models.Finances;
using Domain.Models.Orders;
using Domain.Models.Products;
using Domain.Models.Stats;
using Infrastructure.Auth.Models;
using Infrastructure.Data.Configurations.Auth;
using Infrastructure.Data.Configurations.Carts;
using Infrastructure.Data.Configurations.Finances;
using Infrastructure.Data.Configurations.Orders;
using Infrastructure.Data.Configurations.Products;
using Infrastructure.Data.Configurations.Stats;

namespace Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>, IApplicationDbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        protected ApplicationDbContext()
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Auth
            builder.ApplyConfiguration(new RefreshTokenConfiguration());
            builder.Entity<ApplicationUser>().ToTable("AppUsers");
            builder.Entity<ApplicationRole>().ToTable("AppRoles");

            // Products
            builder.ApplyConfiguration(new CategoryConfiguration());
            builder.ApplyConfiguration(new ColorConfiguration());
            builder.ApplyConfiguration(new ProductColorConfiguration());
            builder.ApplyConfiguration(new ProductConfiguration());
            builder.ApplyConfiguration(new ProductImageConfiguration());
            builder.ApplyConfiguration(new ProductVariantConfiguration());
            builder.ApplyConfiguration(new SizeConfiguration());

            // Carts
            builder.ApplyConfiguration(new CartConfiguration());
            builder.ApplyConfiguration(new CartItemConfiguration());

            // Orders
            builder.ApplyConfiguration(new OrderConfiguration());
            builder.ApplyConfiguration(new OrderAddressConfiguration());
            builder.ApplyConfiguration(new OrderItemConfiguration());
            builder.ApplyConfiguration(new OrderPaymentConfiguration());
            builder.ApplyConfiguration(new OrderStatusHistoryConfiguration());

            // Finances
            builder.ApplyConfiguration(new InvoiceConfiguration());

            // Stats
            builder.ApplyConfiguration(new DailyRevenueStatsConfiguration());
            builder.ApplyConfiguration(new ProductSalesStatsConfiguration());
        }


        // Auth
        public DbSet<RefreshToken> RefreshTokens { get; set; }


        // Products
        public DbSet<Category> Categories { get; set; }
        public DbSet<Color> Colors { get; set; }
        public DbSet<ProductColor> ProductColors { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<ProductVariant> ProductVariants { get; set; }
        public DbSet<Size> Sizes { get; set; }


        // Carts
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }


        // Orders
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderAddress> OrderAddresses { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<OrderPayment> OrderPayments { get; set; }
        public DbSet<OrderStatusHistory> OrderStatusHistories { get; set; }


        // Finances
        public DbSet<Invoice> Invoices { get; set; }


        // Stats
        public DbSet<DailyRevenueStats> DailyRevenueStats { get; set; }
        public DbSet<ProductSalesStats> ProductSalesStats { get; set; }
    }
}