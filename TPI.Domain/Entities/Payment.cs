namespace TPI.Domain.Entities
{
    public class Payment : BaseEntity
    {
        public int Id { get; set; }
        public enum Method 
        {
            BankTransfer
        }

        public decimal Amount { get; set; }

        public enum Status
        {
            Pending,
            Accepted,
            Rejected
        }
    }


    }

