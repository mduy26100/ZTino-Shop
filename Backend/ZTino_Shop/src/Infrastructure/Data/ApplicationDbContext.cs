using Infrastructure.Auth.Models;
using Infrastructure.Data.Configurations.Auth;

namespace Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
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
        }


        //Auth
        public DbSet<RefreshToken> RefreshTokens { get; set; }
    }
}
