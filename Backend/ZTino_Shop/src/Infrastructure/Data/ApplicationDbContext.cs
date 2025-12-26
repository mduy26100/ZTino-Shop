using Application.Common.Interfaces.Persistence.Data;
using Domain.Models.Products;
using Infrastructure.Auth.Models;
using Infrastructure.Data.Configurations.Auth;
using Infrastructure.Data.Configurations.Products;

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

            //Auth
            builder.ApplyConfiguration(new RefreshTokenConfiguration());
            builder.Entity<ApplicationUser>().ToTable("AppUsers");
            builder.Entity<ApplicationRole>().ToTable("AppRoles");


            //Products
            builder.ApplyConfiguration(new CategoryConfiguration());
            builder.ApplyConfiguration(new ColorConfiguration());
            builder.ApplyConfiguration(new ProductConfiguration());
            builder.ApplyConfiguration(new ProductImageConfiguration());
            builder.ApplyConfiguration(new ProductVariantConfiguration());
            builder.ApplyConfiguration(new SizeConfiguration());
        }


        //Auth
        public DbSet<RefreshToken> RefreshTokens { get; set; }


        //Products
        public DbSet<Category> Categories { get; set; }
        public DbSet<Color> Colors { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<ProductVariant> ProductVariants { get; set; }
        public DbSet<Size> Sizes { get; set; }
    }
}
