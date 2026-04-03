namespace TPI.Domain.Entities
{
    public class Order
    {
        public Guid Id { get; set; }
        public enum Status
        {
            created,
            awaiting_payment,
            paid,
            canceled,
            ready,
            delivered
        }
        public decimal TotalAmount { get; set; }

    }
}
