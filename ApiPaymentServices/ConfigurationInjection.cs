using ApiPaymentServices.QueueService;
using ApiPaymentServices.Services;
using ApiPaymentServices.Services.Impl;
using Microsoft.EntityFrameworkCore;
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

            return services;
        }
    }
}
