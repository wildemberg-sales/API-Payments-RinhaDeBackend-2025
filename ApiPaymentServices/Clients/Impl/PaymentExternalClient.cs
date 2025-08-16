using ApiPaymentServices.Models;
using ApiPaymentServices.Models.Requests;
using ApiPaymentServices.Singletons.State;
using ApiPaymentServices.Utils;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;

namespace ApiPaymentServices.Clients.Impl
{
    public class PaymentExternalClient : IPaymentExternalClient
    {
        private readonly ILogger<PaymentExternalClient> _logger;
        private readonly ExternalPaymentServiceState _state;
        private readonly IHttpClientFactory _httpClient;

        public PaymentExternalClient(ILogger<PaymentExternalClient> logger, ExternalPaymentServiceState state, IHttpClientFactory httpClient) 
        {
            _logger = logger;
            _state = state;
            _httpClient = httpClient;
        }

        public async Task<(PaymentHealthCheckResponse, PaymentHealthCheckResponse)> GetStatusApiExternal(string urlDefault, string urlFallback)
        {
            HttpClient client = _httpClient.CreateClient();

            try
            {
                var responseDefault = await client.GetAsync($"{urlDefault}/payments/service-health");
                var dataDefault = await responseDefault.Content.ReadFromJsonAsync(ApiJsonSerializerContext.Default.PaymentHealthCheckResponse);

                var responseFallback = await client.GetAsync($"{urlFallback}/payments/service-health");
                var dataFallback = await responseFallback.Content.ReadFromJsonAsync(ApiJsonSerializerContext.Default.PaymentHealthCheckResponse);

                _logger.LogInformation("Successfully in health check requisiton");

                return (dataDefault!, dataFallback!);
            }
            catch (Exception ex)
            {
                _logger.LogError("Fail in health check requisition: ", ex.Message);
                return (null, null);
            }
            
        }

        public async Task<(bool, bool, DateTime)> SendPaymentForExternalService(Payment payment, string urlDefault, string urlFallback)
        {
            _logger.LogWarning("Initializing Payment External Requisition");

            HttpClient client = _httpClient.CreateClient("PaymentsExternal");
            DateTime requestedAt = DateTime.UtcNow;

            if (_state.ExternalDefaultPaymentUp)
            {
                _logger.LogWarning("Default Route Payment");
                try
                {
                    var response = await client.PostAsJsonAsync($"{urlDefault}/payments", 
                    new PaymentPayloadRequestModel
                    {
                        correlationId = payment.CorrelationId,
                        amount = payment.Amount,
                        requestedAt=requestedAt,
                    },
                    ApiJsonSerializerContext.Default.PaymentPayloadRequestModel);

                    if (!response.IsSuccessStatusCode)
                    {
                        var responseBody = await response.Content.ReadAsStringAsync();
                        _logger.LogError("Fail Payment: ", payment.CorrelationId);
                        return (false, false, requestedAt);
                    }

                    _logger.LogInformation("Default Payment Successfully: ", payment.CorrelationId);
                    return (true, false, requestedAt);
                }
                catch (Exception ex)
                {
                    _logger.LogError("Exception In Payment Requisiton Default: ", ex.Message);
                    return (false, false, requestedAt);
                }
            }
            else if (_state.ExternalFallbackPaymentUp)
            {
                _logger.LogWarning("Fallback Route Payment");
                try
                {
                    var response = await client.PostAsJsonAsync($"{urlFallback}/payments",
                    new PaymentPayloadRequestModel
                    {
                        correlationId = payment.CorrelationId,
                        amount = payment.Amount,
                        requestedAt = requestedAt,
                    },
                    ApiJsonSerializerContext.Default.PaymentPayloadRequestModel);


                    if (!response.IsSuccessStatusCode)
                    {
                        var responseBody = await response.Content.ReadAsStringAsync();
                        _logger.LogError("Fail Payment: ", payment.CorrelationId);
                        return (false, false, requestedAt);
                    }

                    return (true, true, requestedAt);
                }
                catch (Exception ex)
                {
                    _logger.LogError("Exception In Payment Requisition Fallback: ", ex.Message);
                    return (false, false, requestedAt);
                }
            }

            _logger.LogWarning("No API External Avaliable");
            return (false, false, requestedAt);
        }
    }
}
