namespace ApiPaymentServices.Models.Requests
{
    public sealed record PaymentPayloadModel(Guid correlationId, float amount);
}
