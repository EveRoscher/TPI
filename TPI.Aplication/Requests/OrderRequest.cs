namespace TPI.Aplication.Requests
{
    public class OrderRequest
    {
        public Guid UserId { get; set; }
        public DateTime PickupETA { get; set; }
        public DateOnly PickupDay { get; set; }
    }
}
