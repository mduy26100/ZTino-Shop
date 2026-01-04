using Domain.Models.Finances;

namespace Infrastructure.Persistence.Configurations.Finances
{
    internal class InvoiceConfiguration : IEntityTypeConfiguration<Invoice>
    {
        public void Configure(EntityTypeBuilder<Invoice> builder)
        {
            builder.ToTable("Invoices");

            builder.HasKey(i => i.Id);

            builder.Property(i => i.Id)
                .ValueGeneratedOnAdd();

            builder.Property(i => i.OrderId)
                .IsRequired();

            builder.Property(i => i.InvoiceNumber)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(i => i.IssuedDate)
                .IsRequired()
                .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(i => i.DueDate)
                .IsRequired(false);

            builder.Property(i => i.CustomerName)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(i => i.TaxCode)
                .IsRequired(false)
                .HasMaxLength(20);

            builder.Property(i => i.CompanyAddress)
                .IsRequired(false)
                .HasMaxLength(500);

            builder.Property(i => i.TotalAmount)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(i => i.TaxAmount)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(i => i.InvoicePdfUrl)
                .IsRequired(false)
                .HasMaxLength(500);

            builder.Property(i => i.Status)
                .IsRequired()
                .HasMaxLength(50);

            builder.HasOne(i => i.Order)
                .WithOne(o => o.Invoice)
                .HasForeignKey<Invoice>(i => i.OrderId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(i => i.InvoiceNumber)
                .IsUnique()
                .HasDatabaseName("IX_Invoices_InvoiceNumber");

            builder.HasIndex(i => i.IssuedDate)
                .HasDatabaseName("IX_Invoices_IssuedDate");

            builder.HasIndex(i => i.Status)
                .HasDatabaseName("IX_Invoices_Status");
        }
    }
}
