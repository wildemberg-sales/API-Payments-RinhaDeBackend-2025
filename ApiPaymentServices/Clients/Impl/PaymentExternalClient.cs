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
                var dataFallback = await responseFallback.Content.ReadFromJsonAsync<PaymentHealthCheckResponse>();

                _logger.LogInformation("Successfully in health check requisiton");

                return (dataDefault!, dataFallback!);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Fail in health check requisition: {ex.Message}");
                return (null, null);
            }
            
        }

        public async Task<(bool, bool)> SendPaymentForExternalService(Payment payment, string urlDefault, string urlFallback)
        {
            HttpClient client = new HttpClient();

            _logger.LogWarning("Initializing Payment External Requisition");

            if (_state.ExternalDefaultPaymentUp)
            {
                _logger.LogWarning("Default Route Payment");
                try
                {
                    var response = await client.PostAsJsonAsync($"{urlDefault}/payments", new
                    {
                        correlationId = payment.CorrelationId,
                        amount = payment.Amount,
                        requestedAt = DateTime.UtcNow,
                    });

                    if (!response.IsSuccessStatusCode)
                    {
                        var responseBody = await response.Content.ReadAsStringAsync();
                        _logger.LogError("Fail Payment. Status: {StatusCode} - {ReasonPhrase}. Body: {ResponseBody}",
                            (int)response.StatusCode,
                            response.ReasonPhrase,
                            responseBody);
                        return (false, false);
                    }

                    _logger.LogInformation($"Default Payment Successfully: {payment.CorrelationId}");
                    return (true, false);
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Exception In Payment Requisiton Default: {ex.Message}");
                    return (false, false);
                }
            }
            else if (_state.ExternalFallbackPaymentUp)
            {
                _logger.LogWarning("Fallback Route Payment");
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
                    _logger.LogError($"Exception In Payment Requisition Fallback: {ex.Message}");
                    return (false, false);
                }
            }

            _logger.LogWarning("No API External Avaliable");
            return (false, false);
        }
    }
}
