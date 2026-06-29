namespace TPI.Domain.Entities
{
    public abstract class BaseEntity
    {
        public Guid Id { get; set; }
        public bool IsDeleted { get; set; } = false;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedDateTime { get; set; }
        public DateTime DeletedDateTime { get; set; }
    }
}
