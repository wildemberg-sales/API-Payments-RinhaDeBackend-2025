using ApiPaymentServices.Models;
using ApiPaymentServices.Models.Payload;
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

        public async Task<HttpResponseMessage> CreatePaymentAsync(PaymentPayloadModel payment)
        {
            try
            {
                _context.Payments.Add(new Payment
                {
                    CorrelationId = payment.correlationId,
                    Amount = payment.amount,
                    CreatedAt = DateTime.UtcNow
                });

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent("Payment not created")
                };
            }

            _logger.LogInformation("Payment cadastrado com sucesso");

            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("Payment created successfully")
            };
        }
    }
}
