using ApiPaymentServices.Singletons.QueueService;

namespace ApiPayment.ApiBackgroundServices
{
    public class ProcessPaymentBackgroundService : BackgroundService
    {
        private readonly ILogger<ProcessPaymentBackgroundService> _logger;
        private readonly PaymentQueueService _queueService;

        public ProcessPaymentBackgroundService(ILogger<ProcessPaymentBackgroundService> logger, PaymentQueueService queueservice)
        {
            _logger = logger;
            _queueService = queueservice;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            int count = 0;
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    _logger.LogInformation($"Execução da pagamento pelas apis: {count++}");
                }
                catch (Exception ex)
                {
                    _logger.LogError("erro ao processar pagamento");
                }
                await Task.Delay(1000);
            }
        }
    }
}
