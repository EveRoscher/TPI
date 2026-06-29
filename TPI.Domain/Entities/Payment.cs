namespace TPI.Domain.Entities
{
    public enum PaymentMethod
    {
        BankTransfer
    }

    public enum PaymentStatus
    {
        PendingReview,
        Accepted,
        Rejected
    }

    public class Payment : BaseEntity
    {
        public Guid OrderId { get; set; }
        public PaymentMethod Method { get; set; } = PaymentMethod.BankTransfer;
        public decimal Amount { get; set; }
        public PaymentStatus Status { get; set; } = PaymentStatus.PendingReview;
        public string? ReceiptUrl { get; set; }
        public DateTime? ReceivedAt { get; set; }

        // Navigation
        public Order Order { get; set; } = null!;
    }
}
