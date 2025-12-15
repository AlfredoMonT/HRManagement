using HRManagement.Domain.Entities;
using MediatR;
using HRManagement.Application.Interfaces; // Necesario para guardar

namespace HRManagement.Application.Features.Employees.Commands.CreateEmployee
{
    // TUS CORRECCIONES DE TIPOS (ESTO ESTÁ PERFECTO) ✅
    public class CreateEmployeeCommand : IRequest<int>
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }
        public decimal Salary { get; set; }

        public string? PhoneNumber { get; set; }
        public string? Department { get; set; } // Correcto: string
        public int? PositionId { get; set; }    // Correcto: nullable int
    }

    // EL HANDLER REAL (AQUÍ QUITAMOS EL 10 FALSO) ⚠️
    public class CreateEmployeeCommandHandler : IRequestHandler<CreateEmployeeCommand, int>
    {
        private readonly IHrManagementDbContext _context;

        public CreateEmployeeCommandHandler(IHrManagementDbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(CreateEmployeeCommand request, CancellationToken cancellationToken)
        {
            var employee = new Employee
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                Salary = request.Salary,
                PhoneNumber = request.PhoneNumber,
                Department = request.Department, // Ahora sí coinciden los tipos
                PositionId = request.PositionId
            };

            _context.Employees.Add(employee);
            await _context.SaveChangesAsync(cancellationToken);

            return employee.Id; // Devuelve el ID real de la BD
        }
    }
}