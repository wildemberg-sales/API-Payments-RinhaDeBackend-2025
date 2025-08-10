using ApiPaymentServices.Clients;
using ApiPaymentServices.Clients.Impl;
using ApiPaymentServices.Services;
using ApiPaymentServices.Services.Impl;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ApiPaymentServices
{
    public static class ConfigurationInjection
    {
        public static IServiceCollection AddPaymentServices(this IServiceCollection services,
            IConfiguration configuration)
        {

            services.AddScoped<IPaymentService, PaymentService>();
            services.AddScoped<IPaymentExternalClient, PaymentExternalClient>();

            return services;
        }
    }
}
