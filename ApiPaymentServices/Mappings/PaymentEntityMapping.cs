using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ApiPaymentServices.Models;

namespace ApiPaymentServices.Mappings
{
    internal sealed class PaymentEntityMapping : IEntityTypeConfiguration<Payment>
    {

        public void Configure(EntityTypeBuilder<Payment> builder)
        {

        }
    }
}
