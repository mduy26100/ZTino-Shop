using Domain.Models.AppSettings;
using Infrastructure.Persistence.Constants;

namespace Infrastructure.Persistence.Configurations.AppSettings
{
    internal class AppSettingConfiguration : IEntityTypeConfiguration<AppSetting>
    {
        public void Configure(EntityTypeBuilder<AppSetting> builder)
        {
            builder.ToTable("AppSettings", SchemaNames.System);

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Group)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(x => x.Key)
                .HasMaxLength(100)
                .IsRequired();

            builder.HasIndex(x => new { x.Group, x.Key })
                .IsUnique();

            builder.Property(x => x.Value)
                .IsRequired();

            builder.Property(x => x.Description)
                .HasMaxLength(500)
                .IsRequired(false);

            builder.Property(x => x.IsActive)
                .HasDefaultValue(true);
        }
    }
}