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
        Task<(bool, bool)> SendPaymentForExternalService(Payment payment, string urlDefault, string urlFallback);
        Task<(PaymentHealthCheckResponse, PaymentHealthCheckResponse)> GetStatusApiExternal(string urlDefault, string urlFallback);
    }
}
