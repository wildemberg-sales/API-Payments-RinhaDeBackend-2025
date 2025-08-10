using ApiPaymentServices.Clients.Impl;
using ApiPaymentServices.Models;
using ApiPaymentServices.Services;
using ApiPaymentServices.Singletons.QueueService;

namespace ApiPayment.ApiBackgroundServices
{
    public class ProcessPaymentBackgroundService : BackgroundService
    {
        private readonly ILogger<ProcessPaymentBackgroundService> _logger;
        private readonly PaymentQueueService _queueService;
        private readonly PaymentExternalClient _client;
        private readonly IPaymentService _servicePayment;

        public ProcessPaymentBackgroundService(ILogger<ProcessPaymentBackgroundService> logger,
                                                PaymentQueueService queueservice,
                                                PaymentExternalClient client,
                                                IPaymentService servicePayment)
        {
            _logger = logger;
            _queueService = queueservice;
            _client = client;
            _servicePayment = servicePayment;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                if (_queueService.queue.Count <= 0)
                {
                    _logger.LogWarning("Fila de pagementos vazia");
                    continue;
                }

                try
                {
                    _logger.LogInformation($"Execução da pagamento pelas apis: {(_queueService.queue.Count() > 0 ? _queueService.queue.Peek().CorrelationId : "Nenhum pagamento na fila" )}");

                    Payment pay = _queueService.queue.Dequeue();

                    var (res, isFallback) = await _client.SendPaymentForExternalService(
                        pay,
                        Environment.GetEnvironmentVariable("URL_DEFAULT")!,
                        Environment.GetEnvironmentVariable("URL_FALLBACK")!
                     );

                    if (!res)
                    {
                        _queueService.queue.Enqueue(pay);
                        _logger.LogError($"Pagemento nao efetuado: {pay.CorrelationId}");
                        continue;
                    }

                    _logger.LogInformation($"Pagemento efetuado com sucesso: {pay.CorrelationId}");

                    await _servicePayment.UpdatePaymentAsync(pay, isFallback);
                }
                catch (Exception ex)
                {
                    _logger.LogError("erro ao processar pagamento");
                }
            }
        }
    }
}
