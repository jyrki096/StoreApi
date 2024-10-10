namespace Api.Models
{
    public class PaymentResponse
    {
        public string IntentId { get; set; }
        public bool Success { get; set; } = true;
        public string Secret { get; set; }
        public string ErrorMessage { get; set; }
    }
}
