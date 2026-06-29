namespace TPI.Aplication.Responses
{
    public class PaymentResponse
    {
        public Guid Id { get; set; }
        public string Method { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string Status { get; set; } = string.Empty;
        public string? ReceiptUrl { get; set; }
        public DateTime? ReceivedAt { get; set; }
        public Guid OrderId { get; set; }
    }
}
