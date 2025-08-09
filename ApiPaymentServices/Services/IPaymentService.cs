using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiPaymentServices.Services
{
    public interface IPaymentService
    {
        Task<HttpResponseMessage> CreatePaymentAsync(string paymentDetails);
    }
}
