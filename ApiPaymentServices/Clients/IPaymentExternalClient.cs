using ApiPaymentServices.Models;
using ApiPaymentServices.Models.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiPaymentServices.Clients
{
    public interface IPaymentExternalClient
    {
        Task<bool> SendPaymentForExternalService(Payment payment);
        Task<(PaymentHealthCheckResponse, PaymentHealthCheckResponse)> GetStatusApiExternal(string urlDefault, string urlFallback);
    }
}
