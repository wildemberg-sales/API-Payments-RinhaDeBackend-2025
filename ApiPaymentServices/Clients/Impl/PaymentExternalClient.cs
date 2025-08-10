using ApiPaymentServices.Models;
using ApiPaymentServices.Models.Requests;
using System.Net.Http.Json;

namespace ApiPaymentServices.Clients.Impl
{
    public class PaymentExternalClient : IPaymentExternalClient
    {
        public async Task<(PaymentHealthCheckResponse, PaymentHealthCheckResponse)> GetStatusApiExternal(string urlDefault, string urlFallback)
        {
            HttpClient client = new HttpClient();

            var responseDefault = await client.GetAsync(urlDefault);
            var dataDefault = await responseDefault.Content.ReadFromJsonAsync<PaymentHealthCheckResponse>();

            var responseFallback = await client.GetAsync(urlFallback);
            var dataFallback = await responseDefault.Content.ReadFromJsonAsync<PaymentHealthCheckResponse>();

            return (dataDefault!, dataFallback!);
        }

        public async Task<bool> SendPaymentForExternalService(Payment payment)
        {
            await Task.Delay(1000);
            return true;
        }

    }
}
