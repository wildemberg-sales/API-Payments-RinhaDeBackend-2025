using ApiPaymentServices.Models;

namespace ApiPaymentServices.QueueService
{
    public  class PaymentQueueService : IPaymentQueueService
    {
        private static readonly PaymentQueueService _instance = new();
        public static PaymentQueueService Instance => _instance;

        public Queue<Payment> queue = new Queue<Payment>();

        public PaymentQueueService () { }
    }
}
