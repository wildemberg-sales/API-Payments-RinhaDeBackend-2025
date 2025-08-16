using ApiPaymentServices.Models.Requests;
using System.Text.Json.Serialization;

namespace ApiPaymentServices.Utils
{
    [JsonSerializable(typeof(PaymentPayloadModel))]
    [JsonSerializable(typeof(PaymentPayloadRequestModel))]
    [JsonSerializable(typeof(PaymentHealthCheckResponse))]
    [JsonSerializable(typeof(PaymentSummaryResponse))]
    [JsonSerializable(typeof(HttpResponseMessage))]
    public partial class ApiJsonSerializerContext : JsonSerializerContext
    {
    }
}
