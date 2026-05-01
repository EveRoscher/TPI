namespace TPI.Domain.Entities
{
    public class User
    {
        //atributos
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty; //ver tema encriptado
        public string? Phone { get; set; } //dato opcional, por defecto null
        public enum Status 
        {
            active,
            suspended,
            deleted
        }
        
       



    }
}
