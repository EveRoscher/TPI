using TPI.Domain.Entities;

namespace TPI.Aplication.Requests
{
    public class PaymentRequest
    {
        public Guid OrderId { get; set; }
        public PaymentMethod Method { get; set; }
        public decimal Amount { get; set; }
        public string? ReceiptUrl { get; set; }
    }
}
