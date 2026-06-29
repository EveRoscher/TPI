namespace TPI.Aplication.Requests
{
    public class UserRequest
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public string Role { get; set; } = "Cliente";
    }
}
