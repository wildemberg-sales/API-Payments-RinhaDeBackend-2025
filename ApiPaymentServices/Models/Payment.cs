namespace ApiPaymentServices.Models
{
    public class Payment
    {
        public Guid CorrelationId { get; set; }
        public float Amount { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsProcessed { get; set; } = false;
        public bool IsFallback { get; set; } = false;

        public Payment() { }

        public static Payment Create(Guid correlationId, float amount)
        {
            return new()
            {
                CorrelationId = correlationId,
                Amount = amount,
                CreatedAt = DateTime.UtcNow,
                IsProcessed = false,
                IsFallback = false
            };
        }

        public void Update(Guid? correlationId = null, float? amount = null, DateTime? createdAt = null, bool? isProcessed = null, bool? isFallback = null)
        {
            CorrelationId = correlationId ?? CorrelationId;
            Amount = amount ?? Amount;
            CreatedAt = createdAt ?? CreatedAt;
            IsProcessed = isProcessed ?? IsProcessed;
            IsFallback = isFallback ?? IsFallback;
        }
    }
}
