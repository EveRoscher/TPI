namespace TPI.Domain.Entities
{
    public class Email
    {
        public string ToEmail { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public enum SentStatus
        {
            Queued,
            Sent,
            Failed
        }


    }
}
