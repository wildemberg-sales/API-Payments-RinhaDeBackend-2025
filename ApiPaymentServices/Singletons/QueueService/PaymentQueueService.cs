using ApiPaymentServices.Models;

namespace ApiPaymentServices.Singletons.QueueService
{
    public  class PaymentQueueService
    {
        private static readonly PaymentQueueService _instance = new();
        public static PaymentQueueService Instance => _instance;

        public Queue<Payment> queue = new Queue<Payment>();

        public PaymentQueueService () { }
    }
}
