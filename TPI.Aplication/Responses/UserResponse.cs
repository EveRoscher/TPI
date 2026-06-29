namespace TPI.Aplication.Responses
{
    public class UserResponse
    {
        public Guid Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public string Status { get; set; } = string.Empty;
        public bool IsSuspect { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? LastOrderAt { get; set; }
        public string Role { get; set; } = string.Empty;
    }
}
