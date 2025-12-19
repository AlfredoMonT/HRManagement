using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization; 
using HRManagement.Application.Features.Employees.Commands.CreateEmployee;
using HRManagement.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HRManagement.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class EmployeesController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IHrManagementDbContext _context;

        public EmployeesController(IMediator mediator, IHrManagementDbContext context)
        {
            _mediator = mediator;
            _context = context;
        }

        // 1. GET: api/Employees (Lectura)
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetEmployees()
        {
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
                    e.PositionId
                })
                .ToListAsync();

            return Ok(employees);
        }

        // 2. POST: api/Employees (Creación)
        [HttpPost]
        public async Task<ActionResult<int>> CreateEmployee(CreateEmployeeCommand command)
        {
            var employeeId = await _mediator.Send(command);
            return Ok(new { id = employeeId, message = "Empleado creado con éxito via CQRS" });
        }

        // 3. PUT: api/Employees/5 (Actualización)
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEmployee(int id, [FromBody] CreateEmployeeCommand command)
        {

            // a) Buscamos el empleado existente
            var employee = await _context.Employees.FindAsync(id);

            if (employee == null)
            {
                return NotFound(new { message = $"No se encontró el empleado con ID {id}" });
            }

            // b) Actualizamos los campos
            employee.FirstName = command.FirstName;
            employee.LastName = command.LastName;
            employee.Email = command.Email;
            employee.Salary = command.Salary;
            employee.PhoneNumber = command.PhoneNumber;
            employee.Department = command.Department;
            employee.PositionId = command.PositionId;

            // c) Guardamos cambios en base de datos
            await _context.SaveChangesAsync(default);

            return Ok(new { message = "Empleado actualizado correctamente" });
        }

        // 4. DELETE: api/Employees/5 (Eliminación)
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            // a) Buscamos el empleado
            var employee = await _context.Employees.FindAsync(id);

            if (employee == null)
            {
                return NotFound(new { message = $"No se encontró el empleado con ID {id}" });
            }

            // b) Eliminamos
            _context.Employees.Remove(employee);

            // c) Guardamos cambios
            await _context.SaveChangesAsync(default);

            return Ok(new { message = "Empleado eliminado correctamente" });
        }
    }
}