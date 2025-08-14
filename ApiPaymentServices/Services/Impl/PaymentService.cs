using ApiPaymentServices.Channels;
using ApiPaymentServices.Models;
using ApiPaymentServices.Models.Requests;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Net;

namespace ApiPaymentServices.Services.Impl
{
    public sealed class PaymentService : IPaymentService
    {
        private readonly ApiDbContext _context;
        private readonly ILogger<PaymentService> _logger;
        private readonly QueuePaymentDatabaseChannel _channel;

        public PaymentService(ApiDbContext context, ILogger<PaymentService> logger, QueuePaymentDatabaseChannel channel)
        {
            _context = context;
            _logger = logger;
            _channel = channel;
        }

        public async Task<bool> CreatePaymentAsync(Payment payment)
        {
            try
            {
                _context.Payments.Add(payment);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in creation: {ex.Message}");
                return false;
            }

            return true;
        }

        public async Task UpdatePaymentAsync(Payment payment, bool isFallback, DateTime requestedAt)
        {
            try
            {
                payment.Update(isProcessed: true, isFallback: isFallback, createdAt: requestedAt);
                _context.Update(payment);
                await _context.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                _logger.LogError($"Update Error payment -> {payment} -> With error: {ex.Message}");
            }

            _logger.LogInformation($"Payment -> {payment} -> Updated Successfully");
        }

        public async Task<HttpResponseResult<PaymentSummaryResponse>> GetPaymentsSummaryAsync(DateTime? from = null, DateTime? to = null)
        {
            var query = _context.Payments.Where(p => p.IsProcessed).AsQueryable();

            if(from != null)
                query = query.Where(p => p.CreatedAt >= from);
            
            if(to != null)
                query = query.Where(p => p.CreatedAt <= to);

            var res = await query.ToListAsync();

            if (res == null)
            {
                _logger.LogError("Not payments found in database");
                return HttpResponseResult<PaymentSummaryResponse>.Fail("Not Found Items", HttpStatusCode.NotFound);
            }

            PaymentSummaryResponse response = PaymentSummaryResponse.Create(
                defaultTotalRequest: res.Count(p => !p.IsFallback),
                defaultTotalAmount: (float)Math.Round(res.Where(p => !p.IsFallback).Sum(p => p.Amount), 2),
                fallbackTotalRequest: res.Count(p => p.IsFallback),
                fallbackTotalAmount: (float)Math.Round(res.Where(p => p.IsFallback).Sum(p => p.Amount), 2)
            );

            return HttpResponseResult<PaymentSummaryResponse>.Ok(response);
        }
    }
}
