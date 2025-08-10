
using ApiPaymentServices.State;

namespace ApiPayments.ApiBackgroundServices
{
    public class VerifyApiExternalBackgroundService : BackgroundService
    {
        private readonly ILogger<VerifyApiExternalBackgroundService> _logger;
        private readonly ExternalPaymentServiceState _state;

        public VerifyApiExternalBackgroundService(ILogger<VerifyApiExternalBackgroundService> logger, ExternalPaymentServiceState state)
        {
            _logger = logger;
            _state = state;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    _logger.LogInformation("Execução da verificação de estado das apis externas");
                }
                catch(Exception ex)
                {
                    _logger.LogError("Erro ao verificar estado da api externa");
                }

                await Task.Delay(300500);
            }
        }
    }
}
