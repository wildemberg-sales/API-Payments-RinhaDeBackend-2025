namespace ApiPaymentServices.State
{
    public class ExternalPaymentServiceState : IExternalPaymentServiceState
    {
        private static readonly ExternalPaymentServiceState _instance = new();
        public static ExternalPaymentServiceState Instance => _instance;

        public bool ExternalDefaultPaymentUp { get; set; }
        public bool ExternalFallbackPaymentUp { get; set; }

        public ExternalPaymentServiceState() { }
    }
}
