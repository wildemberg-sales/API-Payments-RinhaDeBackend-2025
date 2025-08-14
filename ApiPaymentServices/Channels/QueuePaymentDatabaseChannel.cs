using ApiPaymentServices.Models;
using ApiPaymentServices.Models.Requests;
using Microsoft.Extensions.Logging;
using System.Threading.Channels;

namespace ApiPaymentServices.Channels
{
    public class QueuePaymentDatabaseChannel
    {
        private readonly Channel<PaymentPayloadModel> _channelDatabase = Channel.CreateUnbounded<PaymentPayloadModel>();

        private readonly ILogger<QueuePaymentDatabaseChannel> _logger;

        public QueuePaymentDatabaseChannel(ILogger<QueuePaymentDatabaseChannel> logger)
        {
            _logger = logger;
        }

        public async Task AddPaymentDatabseAsync(PaymentPayloadModel payment)
        {
            _logger.LogInformation($"Add payment in database channel queue: {payment.correlationId}");
            await _channelDatabase.Writer.WriteAsync(payment);
        }

        public IAsyncEnumerable<PaymentPayloadModel> AllPaymentsDatabaseQueueAsync(CancellationToken token)
        {
            return _channelDatabase.Reader.ReadAllAsync(token);
        }
    }
}
