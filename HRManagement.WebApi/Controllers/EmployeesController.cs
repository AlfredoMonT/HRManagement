using Microsoft.AspNetCore.Mvc;
using HRManagement.Application.Features.Employees.Commands.CreateEmployee;
using HRManagement.Application.Interfaces; // Para usar el DbContext directamente en el GET simple
using MediatR;
using Microsoft.EntityFrameworkCore; // Necesario para ToListAsync

namespace HRManagement.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IHrManagementDbContext _context; // Inyectamos el contexto para consultas rápidas

        public EmployeesController(IMediator mediator, IHrManagementDbContext context)
        {
            _mediator = mediator;
            _context = context;
        }

        // GET: api/Employees
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetEmployees()
        {
            // ERROR CS1061 ARREGLADO:
            // En lugar de incluir "Position", simplemente traemos los datos planos del empleado.
            var employees = await _context.Employees
                .Select(e => new
                {
                    e.Id,
                    e.FirstName,
                    e.LastName,
                    e.Email,
                    e.Salary,
                    e.PhoneNumber,
                    e.Department,
                    e.PositionId // Usamos solo el ID, no el objeto completo .Position
                })
                .ToListAsync();

            return Ok(employees);
        }

        // POST: api/Employees
        [HttpPost]
        public async Task<ActionResult<int>> CreateEmployee(CreateEmployeeCommand command)
        {
            var employeeId = await _mediator.Send(command);
            // Devolvemos un objeto JSON bonito en lugar de solo el número
            return Ok(new { id = employeeId, message = "Empleado creado con éxito via CQRS" });
        }
    }
}