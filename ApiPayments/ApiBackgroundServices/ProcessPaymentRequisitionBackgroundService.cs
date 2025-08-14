using ApiPayment.ApiBackgroundServices;
using ApiPaymentServices.Channels;
using ApiPaymentServices.Clients;
using ApiPaymentServices.Models;
using ApiPaymentServices.Services;

namespace ApiPayments.ApiBackgroundServices
{
    public class ProcessPaymentRequisitionBackgroundService : BackgroundService
    {
        private readonly ILogger<ProcessPaymentBackgroundService> _logger;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly QueuePaymentRequisitionChannel _channel;

        public ProcessPaymentRequisitionBackgroundService(
            ILogger<ProcessPaymentBackgroundService> logger,
            IServiceScopeFactory scopeFactory,
            QueuePaymentRequisitionChannel channel)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;
            _channel = channel;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            int numberOfConsumers = 4;
            _logger.LogInformation("Iniciando {NumberOfConsumers} consumidores para o processamento de pagamentos de requisição.", numberOfConsumers);

            var tasks = Enumerable.Range(0, numberOfConsumers)
            .Select(_ => Task.Run(() => ConsumerLoop(stoppingToken), stoppingToken));

            await Task.WhenAll(tasks);

            _logger.LogInformation("Todos os consumidores de pagamento foram encerrados.");
        }

        private async Task ConsumerLoop(CancellationToken stoppingToken)
        {
            try
            {
                await foreach (var payment in _channel.AllPaymentsRequisitionQueueAsync(stoppingToken))
                {
                    await ProcessPaymentRequisitionsAsync(payment, stoppingToken);
                }
            }
            catch (OperationCanceledException)
            {
                _logger.LogWarning("Operação cancelada. Consumidor de pagamentos encerrando.");
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Erro fatal no loop do consumidor de pagamentos. Thread: {ThreadId}", Environment.CurrentManagedThreadId);
            }
        }

        private async Task ProcessPaymentRequisitionsAsync(Payment payment, CancellationToken stoppingToken)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var scopeProvider = scope.ServiceProvider;
                var _paymentExternalClient = scopeProvider.GetRequiredService<IPaymentExternalClient>();
                var _paymentService = scopeProvider.GetRequiredService<IPaymentService>();

                _logger.LogWarning("Initialize Process payments background");
               
                try
                {
                    var (res, isFallback, requestedAt) = await _paymentExternalClient.SendPaymentForExternalService(
                                payment,
                                Environment.GetEnvironmentVariable("URL_DEFAULT")!,
                                Environment.GetEnvironmentVariable("URL_FALLBACK")!
                    );

                    if (!res)
                    {
                        await _channel.AddPaymentRequisitionAsync(payment);
                        _logger.LogError($"Payment not made, add payment to the queue again: {payment.CorrelationId}");
                    }
                    else
                    {
                        _logger.LogInformation($"Payment made successfully: {payment.CorrelationId}");
                        await _paymentService.UpdatePaymentAsync(payment, isFallback, requestedAt);
                    }

                }
                catch (Exception ex)
                {
                    _logger.LogError($"Exception in payment processing Channel in the background service: {ex.Message}");
                    await _channel.AddPaymentRequisitionAsync(payment);
                }
                
            }
        }

    }
}
