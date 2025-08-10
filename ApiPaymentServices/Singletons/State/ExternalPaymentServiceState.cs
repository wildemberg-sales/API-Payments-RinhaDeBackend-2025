namespace ApiPaymentServices.Singletons.State
{
    public class ExternalPaymentServiceState
    {
        private static readonly ExternalPaymentServiceState _instance = new();
        public static ExternalPaymentServiceState Instance => _instance;

        public bool ExternalDefaultPaymentUp { get; set; }
        public int TimeExternalDefaultPayment { get; set; }
        public bool ExternalFallbackPaymentUp { get; set; }
        public int TimeExternalFallbackPayment { get; set; }

        public ExternalPaymentServiceState() { }
    }
}
