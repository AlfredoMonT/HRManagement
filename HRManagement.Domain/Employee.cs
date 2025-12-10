namespace HRManagement.Domain.Entities
{
    public class Employee
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }

        // ¡AGREGA ESTA LÍNEA!
        public decimal Salary { get; set; }

        public int PositionId { get; set; }
        public Position? Position { get; set; }
    }
}