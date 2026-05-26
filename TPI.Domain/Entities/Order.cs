namespace TPI.Domain.Entities
{
    public class Order : BaseEntity
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
