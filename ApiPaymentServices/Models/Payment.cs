namespace ApiPaymentServices.Models
{
    public class Payment
    {
        public Guid CorrelationId { get; set; }
        public float Amount { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
