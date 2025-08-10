using ApiPaymentServices.Models;
using ApiPaymentServices.Models.Requests;

namespace ApiPaymentServices.Services
{
    public interface IPaymentService
    {
        Task<HttpResponseResult<Payment>> CreatePaymentAsync(PaymentPayloadModel payment);
    }
}
