using ApiPayment.ApiBackgroundServices;
using ApiPayments.ApiBackgroundServices;
using ApiPaymentServices;
using ApiPaymentServices.QueueService;
using ApiPaymentServices.State;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApiDbContext>();

//Services
builder.Services.AddPaymentServices(builder.Configuration);

//Singletons
builder.Services.AddSingleton<PaymentQueueService>();
builder.Services.AddSingleton<ExternalPaymentServiceState>();

//Background Services
builder.Services.AddHostedService<VerifyApiExternalBackgroundService>();
builder.Services.AddHostedService<ProcessPaymentBackgroundService>();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
