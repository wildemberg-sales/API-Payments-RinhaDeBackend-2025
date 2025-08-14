using ApiPaymentServices.Channels;
using ApiPaymentServices.Models.Requests;
using ApiPaymentServices.Services;
using Microsoft.AspNetCore.Mvc;

namespace ApiPayments.Controllers
{
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        [HttpPost("payments")]
        public async Task<IActionResult> CreatePayment
        (
            [FromServices] QueuePaymentDatabaseChannel _channel,
            [FromBody] PaymentPayloadModel payload
        )
        {

            await _channel.AddPaymentDatabseAsync(payload);

            return Created();
        }

        [HttpGet("payments-summary")]
        public async Task<IActionResult> GetPaymentsSummaryAsync(
            [FromServices] IPaymentService service,
            [FromQuery] DateTime? from,
            [FromQuery] DateTime? to
        )
        {
            var res = await service.GetPaymentsSummaryAsync(from, to);
            return StatusCode((int)res.StatusCode, res.Data);
        }
    }

}
