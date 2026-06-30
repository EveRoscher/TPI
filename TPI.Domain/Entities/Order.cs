namespace TPI.Domain.Entities
{
    public enum OrderStatus
    {
        Created,            //0
        AwaitingPayment,    //1
        Paid,               //2
        Canceled,           //3
        Ready,              //4
        Delivered           //5
    }

    public class Order : BaseEntity
    {
        public Guid UserId { get; set; }
        public OrderStatus Status { get; set; } = OrderStatus.AwaitingPayment;
        public decimal TotalAmount { get; set; }
        public DateTime PickupETA { get; set; }
        public DateOnly PickupDay { get; set; }
        public DateTime? ConfirmedAt { get; set; }

        // 
        public User User { get; set; } = null!;
        public List<OrderItem> OrderItems { get; set; } = new();
        public Payment? Payment { get; set; }
    }
}
