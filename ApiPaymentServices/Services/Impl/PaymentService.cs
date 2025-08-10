using ApiPaymentServices.Models;
using ApiPaymentServices.Models.Requests;
using Microsoft.Extensions.Logging;
using System.Net;

namespace ApiPaymentServices.Services.Impl
{
    internal sealed class PaymentService : IPaymentService
    {
        private readonly ApiDbContext _context;
        private readonly ILogger<PaymentService> _logger;

        public PaymentService(ApiDbContext context, ILogger<PaymentService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<HttpResponseResult<Payment>> CreatePaymentAsync(PaymentPayloadModel payment)
        {
            Payment pay = new Payment
            {
                CorrelationId = payment.correlationId,
                Amount = payment.amount,
                CreatedAt = DateTime.UtcNow
            };

            try
            {
                _context.Payments.Add(pay);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return HttpResponseResult<Payment>.Fail("Payment Not Created", HttpStatusCode.BadRequest);
            }

            _logger.LogInformation("Payment cadastrado com sucesso");

            return HttpResponseResult<Payment>.Created(pay, "Payment Created Successfully");
        }
    }
}
