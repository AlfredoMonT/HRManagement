using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HRManagement.Application.Features.Employees.Commands.CreateEmployee;
using HRManagement.Infrastructure.Persistence;
using HRManagement.Domain.Entities;
using Microsoft.AspNetCore.Authorization; // <--- ¡NUEVO! Necesario para la seguridad

namespace HRManagement.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // <--- ¡CANDADO PUESTO! Ahora todo este controlador requiere Login
    public class EmployeesController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly HrManagementDbContext _context;

        // Inyectamos AMBOS: Mediator para CQRS y Context para consultas rápidas
        public EmployeesController(IMediator mediator, HrManagementDbContext context)
        {
            _mediator = mediator;
            _context = context;
        }

        // --- PARTE CQRS (Cumple con el requisito 6.4 de la Guía) ---

        // POST: api/Employees
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateEmployeeCommand command)
        {
            // Aquí usamos el patrón CQRS para escribir
            var employeeId = await _mediator.Send(command);
            return Ok(new { Id = employeeId, Message = "Empleado creado con éxito via CQRS" });
        }

        // --- PARTE CLÁSICA (Mantenla para que tu app siga funcionando) ---

        // GET: api/Employees
        [HttpGet]
        public IActionResult GetEmployees()
        {
            // Seguimos leyendo directamente para no tener que programar todos los Queries hoy
            var employees = _context.Employees.Include(e => e.Position).ToList();
            return Ok(employees);
        }

        // GET: api/Employees/5
        [HttpGet("{id}")]
        public IActionResult GetEmployeeById(int id)
        {
            var employee = _context.Employees.Include(e => e.Position).FirstOrDefault(e => e.Id == id);
            if (employee == null) return NotFound();
            return Ok(employee);
        }

        // DELETE: api/Employees/5
        [HttpDelete("{id}")]
        public IActionResult DeleteEmployee(int id)
        {
            var employee = _context.Employees.Find(id);
            if (employee == null) return NotFound();

            _context.Employees.Remove(employee);
            _context.SaveChanges();
            return Ok($"Empleado {id} eliminado.");
        }
    }
}