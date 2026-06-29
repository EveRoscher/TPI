namespace TPI.Domain.Entities
{
    public enum UserStatus
    {
        Active,
        Suspended,
        Deleted
    }

    public class User : BaseEntity
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public UserStatus Status { get; set; } = UserStatus.Active;
        public bool IsSuspect { get; set; } = false;
        public DateTime? LastOrderAt { get; set; }
        public string Role { get; set; } = "Cliente"; // "Admin" | "Cliente"

        // Navigation
        public List<Order> Orders { get; set; } = new();
    }
}
