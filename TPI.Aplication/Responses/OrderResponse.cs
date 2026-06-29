namespace TPI.Aplication.Responses
{
    public class OrderResponse
    {
        public Guid Id { get; set; }
        public string Status { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
        public DateTime PickupETA { get; set; }
        public DateOnly PickupDay { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ConfirmedAt { get; set; }
        public Guid UserId { get; set; }
    }
}
