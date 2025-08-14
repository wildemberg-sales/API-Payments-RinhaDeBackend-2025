using ApiPaymentServices.Channels;
using ApiPaymentServices.Clients;
using ApiPaymentServices.Clients.Impl;
using ApiPaymentServices.Services;
using ApiPaymentServices.Services.Impl;
using ApiPaymentServices.Singletons.State;
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
            
            services.AddDbContext<ApiDbContext>();
            
            services.AddSingleton<QueuePaymentDatabaseChannel>();
            services.AddSingleton<QueuePaymentRequisitionChannel>();
            services.AddSingleton<ExternalPaymentServiceState>();

            services.AddHttpClient("PaymentsExternal", client =>
            {
                client.Timeout = TimeSpan.FromMilliseconds(100);
            });

            return services;
        }
    }
}
