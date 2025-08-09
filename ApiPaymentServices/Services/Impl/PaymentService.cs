using ApiPaymentServices.Models;
using System.Net;

namespace ApiPaymentServices.Services.Impl
{
    internal sealed class PaymentService : IPaymentService
    {
        private readonly ApiDbContext _context;

        public PaymentService(ApiDbContext context)
        {
            _context = context;
        }

        public async Task<HttpResponseMessage> CreatePaymentAsync(string paymentDetails)
        {
            _context.Payments.Add(new Payment
            {
                Id = Guid.NewGuid(),
                Value = 1000
            });

            await _context.SaveChangesAsync();

            var code = new HttpStatusCode();
            Console.WriteLine("Payment created successfully");
            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("Payment created successfully")
            };
        }
    }
}
