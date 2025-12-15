namespace HRManagement.Domain.Entities
{
    public class Employee
    {
        public int Id { get; set; }

        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public decimal Salary { get; set; }


        public string? PhoneNumber { get; set; }


        public string? Department { get; set; }


        public int? PositionId { get; set; }
    }
}