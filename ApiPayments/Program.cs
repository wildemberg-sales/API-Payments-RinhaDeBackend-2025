using ApiPayment.ApiBackgroundServices;
using ApiPayments.ApiBackgroundServices;
using ApiPaymentServices;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        });
});

//Services
builder.Services.AddPaymentServices(builder.Configuration);

//Background Services
builder.Services.AddHostedService<VerifyApiExternalBackgroundService>();
builder.Services.AddHostedService<ProcessPaymentBackgroundService>();


builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

app.UseCors("AllowAll");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
