using ApiPaymentServices.Clients;
using ApiPaymentServices.Clients.Impl;
using ApiPaymentServices.Models;
using ApiPaymentServices.Services;
using ApiPaymentServices.Singletons.QueueService;

namespace ApiPayment.ApiBackgroundServices
{
    public class ProcessPaymentBackgroundService : BackgroundService
    {
        private readonly ILogger<ProcessPaymentBackgroundService> _logger;
        private readonly IServiceScopeFactory _scopreFactory;

        public ProcessPaymentBackgroundService(ILogger<ProcessPaymentBackgroundService> logger, IServiceScopeFactory scopreFactory)
        {
            _logger = logger;
            _scopreFactory = scopreFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using(var scope = _scopreFactory.CreateScope())
                {
                    var scopeProvider = scope.ServiceProvider;

                    var _queueService = scopeProvider.GetRequiredService<PaymentQueueService>();
                    var _client = scopeProvider.GetRequiredService<IPaymentExternalClient>();
                    var _servicePayment = scopeProvider.GetRequiredService<IPaymentService>();

                    if (_queueService.queue.Count <= 0)
                    {
                        _logger.LogWarning("Empty payment queue");
                        continue;
                    }

                    try
                    {
                        _logger.LogInformation($"Execution of payment through the APIs: {(_queueService.queue.Count() > 0 ? _queueService.queue.Peek().CorrelationId : "Nenhum pagamento na fila" )}");

                        Payment pay = _queueService.queue.Dequeue();

                        var (res, isFallback, requestedAt) = await _client.SendPaymentForExternalService(
                            pay,
                            Environment.GetEnvironmentVariable("URL_DEFAULT")!,
                            Environment.GetEnvironmentVariable("URL_FALLBACK")!
                         );

                        if (!res)
                        {
                            _queueService.queue.Enqueue(pay);
                            _logger.LogError($"Payment not made, add payment to the queue again: {pay.CorrelationId}");
                            continue;
                        }

                        _logger.LogInformation($"Payment made successfully: {pay.CorrelationId}");
                        await _servicePayment.UpdatePaymentAsync(pay, isFallback, requestedAt);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError("Exception in payment processing in the background service");
                    }
                }
            }
        }
    }
}
