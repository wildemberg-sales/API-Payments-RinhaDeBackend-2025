using ApiPaymentServices.Models.Payload;
using ApiPaymentServices.Services;
using Microsoft.AspNetCore.Mvc;

namespace ApiPayments.Controllers
{
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        [HttpPost("payments")]
        public async Task<HttpResponseMessage> CreatePayment
        (
            [FromServices] IPaymentService service,
            [FromBody] PaymentPayloadModel payload
        )
        {
            return await service.CreatePaymentAsync(payload);
        }


        [HttpGet("payments-summary")]
        public object GetAll()
        {
            return new
            {
                doido = "sou mesmo"
            };
        }
    }

}
