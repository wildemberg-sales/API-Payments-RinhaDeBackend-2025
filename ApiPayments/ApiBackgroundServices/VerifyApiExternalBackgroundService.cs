using ApiPaymentServices.Clients;
using ApiPaymentServices.Clients.Impl;
using ApiPaymentServices.Singletons.State;

namespace ApiPayments.ApiBackgroundServices
{
    public class VerifyApiExternalBackgroundService : BackgroundService
    {
        private readonly ILogger<VerifyApiExternalBackgroundService> _logger;
        private readonly IServiceScopeFactory _scopeFactory;

        public VerifyApiExternalBackgroundService(ILogger<VerifyApiExternalBackgroundService> logger, IServiceScopeFactory scopeFactory)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    _logger.LogInformation("Execução da verificação de estado das apis externas");

                    using(var scope = _scopeFactory.CreateScope())
                    {
                        var serviceProvider = scope.ServiceProvider;

                        var client = serviceProvider.GetRequiredService<IPaymentExternalClient>();
                        var state = serviceProvider.GetRequiredService<ExternalPaymentServiceState>();

                        var (defaultStatus, fallbackStatus) = await client.GetStatusApiExternal(
                                                                Environment.GetEnvironmentVariable("URL_DEFAULT")!,
                                                                Environment.GetEnvironmentVariable("URL_FALLBACK")!
                                                             );

                        if (defaultStatus == null || fallbackStatus == null)
                        {
                            _logger.LogError($"Erro ao verificar estado da api externa - retorno null\n default-status: {defaultStatus} \n fallback-status: {fallbackStatus}");
                            throw new HttpRequestException();
                        }

                        state.ExternalDefaultPaymentUp = defaultStatus.failing;
                        state.TimeExternalDefaultPayment = defaultStatus.minResponseTime;
                        state.ExternalFallbackPaymentUp = fallbackStatus.failing;
                        state.TimeExternalFallbackPayment = fallbackStatus.minResponseTime;
                    }

                }
                catch(Exception ex)
                {
                    _logger.LogError($"Erro ao verificar estado da api externa: {ex.Message}");
                }

                await Task.Delay(5000);
            }
        }
    }
}
