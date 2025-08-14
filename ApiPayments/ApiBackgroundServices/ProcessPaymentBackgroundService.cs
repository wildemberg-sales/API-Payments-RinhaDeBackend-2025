using ApiPaymentServices.Channels;
using ApiPaymentServices.Clients;
using ApiPaymentServices.Models;
using ApiPaymentServices.Models.Requests;
using ApiPaymentServices.Services;
using ApiPaymentServices.Services.Impl;

namespace ApiPayment.ApiBackgroundServices
{
    public class ProcessPaymentBackgroundService : BackgroundService
    {
        private readonly ILogger<ProcessPaymentBackgroundService> _logger;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly QueuePaymentDatabaseChannel _channel;
        private readonly QueuePaymentRequisitionChannel _channelReq;

        public ProcessPaymentBackgroundService(
            ILogger<ProcessPaymentBackgroundService> logger,
            IServiceScopeFactory scopeFactory,
            QueuePaymentDatabaseChannel channel,
            QueuePaymentRequisitionChannel channelReq)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;
            _channel = channel;
            _channelReq = channelReq;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            int numberOfConsumers = 4;
            _logger.LogInformation("Iniciando {NumberOfConsumers} consumidores para o processamento de pagamentos de banco de dados.", numberOfConsumers);

            var tasks = Enumerable.Range(0, numberOfConsumers)
           .Select(_ => Task.Run(() => ConsumerLoop(stoppingToken), stoppingToken));

            await Task.WhenAll(tasks);

            _logger.LogInformation("Todos os consumidores de pagamento foram encerrados.");
        }

        private async Task ConsumerLoop(CancellationToken stoppingToken)
        {
            try
            {
                _logger.LogWarning("Initialize Process payments background");
                await foreach (var pay in _channel.AllPaymentsDatabaseQueueAsync(stoppingToken))
                {
                    await ProcessPaymentDatabaseAsync(pay, stoppingToken);
                }
            }
            catch (OperationCanceledException)
            {
                _logger.LogWarning("Operação cancelada. Consumidor de pagamentos encerrando.");
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Erro fatal no loop do consumidor de pagamentos do banco de dados. Thread: {ThreadId}", Environment.CurrentManagedThreadId);
            }
        }

        private async Task ProcessPaymentDatabaseAsync(PaymentPayloadModel pay, CancellationToken stoppingToken)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var scopeProvider = scope.ServiceProvider;
                var _paymentService = scopeProvider.GetRequiredService<IPaymentService>();

                Payment payment = Payment.Create(pay.correlationId, pay.amount);

                bool inserted = await _paymentService.CreatePaymentAsync(payment);

                if (inserted)
                {
                    await _channelReq.AddPaymentRequisitionAsync(payment);
                }
                else
                {
                    _logger.LogError("Payment not inserted in database", pay.correlationId);
                    await _channel.AddPaymentDatabseAsync(pay);
                }
                
            }
        }
    }
}
