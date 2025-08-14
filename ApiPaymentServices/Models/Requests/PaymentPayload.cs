namespace ApiPaymentServices.Models.Requests
{
    public sealed record PaymentPayloadModel(Guid correlationId, float amount);
    public sealed record PaymentPayloadRequestModel(Guid correlationId, float amoutn, DateTime requestedAt);
}
