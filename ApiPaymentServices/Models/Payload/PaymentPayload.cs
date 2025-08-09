using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiPaymentServices.Models.Payload
{
    public sealed record PaymentPayloadModel(Guid correlationId, float amount);
}
