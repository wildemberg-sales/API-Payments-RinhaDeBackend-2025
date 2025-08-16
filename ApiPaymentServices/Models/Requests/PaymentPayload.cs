namespace ApiPaymentServices.Models.Requests
{
    public sealed record PaymentPayloadModel(Guid correlationId, float amount);
    public sealed record PaymentPayloadRequestModel
    {
        public Guid correlationId { get; set; } 
        public float amount { get; set; }
        public DateTime requestedAt { get; set; }
    }
}
