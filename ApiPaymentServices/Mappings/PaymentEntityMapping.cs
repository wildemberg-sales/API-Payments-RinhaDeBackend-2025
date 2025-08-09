using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ApiPaymentServices.Models;

namespace ApiPaymentServices.Mappings
{
    internal sealed class PaymentEntityMapping : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            builder
                .ToTable("Payments")
                .HasKey(p => p.CorrelationId);

            builder
                .Property(p => p.Amount)
                .IsRequired();

            builder
                .Property(p => p.CreatedAt)
                .IsRequired();
        }
    }
}
