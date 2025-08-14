using ApiPaymentServices.Models;
using ApiPaymentServices.Models.Requests;

namespace ApiPaymentServices.Services
{
    public interface IPaymentService
    {
        Task<bool> CreatePaymentAsync(Payment payment);

        Task UpdatePaymentAsync(Payment payment, bool isFallback, DateTime requestedAt);

        Task<HttpResponseResult<PaymentSummaryResponse>> GetPaymentsSummaryAsync(DateTime? from, DateTime? to);
    }
}
