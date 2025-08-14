using ApiPaymentServices.Models;
using ApiPaymentServices.Models.Requests;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace ApiPaymentServices.Channels
{
    public class QueuePaymentRequisitionChannel
    {
        private readonly Channel<Payment> _channelRequisiton = Channel.CreateUnbounded<Payment>();

        private readonly ILogger<QueuePaymentDatabaseChannel> _logger;

        public QueuePaymentRequisitionChannel(ILogger<QueuePaymentDatabaseChannel> logger)
        {
            _logger = logger;
        }

        public async Task AddPaymentRequisitionAsync(Payment payment)
        {
            _logger.LogInformation($"Add payment in requisition channel queue: {payment.CorrelationId}");
            await _channelRequisiton.Writer.WriteAsync(payment);
        }

        public IAsyncEnumerable<Payment> AllPaymentsRequisitionQueueAsync(CancellationToken token)
        {
            return _channelRequisiton.Reader.ReadAllAsync(token);
        }
    }
}
