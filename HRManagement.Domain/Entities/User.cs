namespace HRManagement.Domain.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty; // En producción esto debería ser un Hash, pero para la guía usaremos texto plano por ahora.
        public string Role { get; set; } = "User";
    }
}