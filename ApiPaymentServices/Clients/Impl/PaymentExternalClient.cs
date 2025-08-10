using ApiPaymentServices.Models;
using ApiPaymentServices.Models.Requests;
using ApiPaymentServices.Singletons.State;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;

namespace ApiPaymentServices.Clients.Impl
{
    public class PaymentExternalClient : IPaymentExternalClient
    {
        private readonly ILogger<PaymentExternalClient> _logger;
        private readonly ExternalPaymentServiceState _state;

        public PaymentExternalClient(ILogger<PaymentExternalClient> logger, ExternalPaymentServiceState state) 
        {
            _logger = logger;
            _state = state;
        }

        public async Task<(PaymentHealthCheckResponse, PaymentHealthCheckResponse)> GetStatusApiExternal(string urlDefault, string urlFallback)
        {
            HttpClient client = new HttpClient();

            try
            {
                var responseDefault = await client.GetAsync($"{urlDefault}/payments/service-health");
                var dataDefault = await responseDefault.Content.ReadFromJsonAsync<PaymentHealthCheckResponse>();

                var responseFallback = await client.GetAsync($"{urlFallback}/payments/service-health");
                var dataFallback = await responseDefault.Content.ReadFromJsonAsync<PaymentHealthCheckResponse>();

                return (dataDefault!, dataFallback!);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Falha durante requisição de health check: {ex.Message}");
                return (null, null);
            }
            
        }

        public async Task<(bool, bool)> SendPaymentForExternalService(Payment payment, string urlDefault, string urlFallback)
        {
            HttpClient client = new HttpClient();

            if (_state.ExternalDefaultPaymentUp)
            {
                try
                {
                    var response = await client.PostAsJsonAsync($"{urlDefault}/payments", new
                    {
                        correlationId = payment.CorrelationId,
                        amount = payment.Amount,
                        requestedAt = DateTime.UtcNow,
                    });

                    if(response.IsSuccessStatusCode)
                        return (true, false);

                    return (false, false);
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Falha durante requisição de pagamento: {ex.Message}");
                    return (false, false);
                }
            }
            else if (_state.ExternalFallbackPaymentUp)
            {
                try
                {
                    var response = await client.PostAsJsonAsync($"{urlFallback}/payments", new
                    {
                        correlationId = payment.CorrelationId,
                        amount = payment.Amount,
                        requestedAt = DateTime.UtcNow,
                    });

                    if (response.IsSuccessStatusCode)
                        return (true, true);

                    return (false, true);
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Falha durante requisição de pagamento: {ex.Message}");
                    return (false, false);
                }
            }

            return (false, false);
        }

    }
}
