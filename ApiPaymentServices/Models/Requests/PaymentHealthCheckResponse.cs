namespace ApiPaymentServices.Models.Requests
{
    public class PaymentHealthCheckResponse
    {
        public bool failing { get; set; }   
        public int minResponseTime { get; set; }
    }
}
