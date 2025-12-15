using Grpc.Core;
using MediatR;
using HRManagement.WebApi.Protos; // Aquí viven las clases generadas por el Proto
using HRManagement.Application.Features.Employees.Commands.CreateEmployee;
// Si tienes una Query para GetEmployee, agrégala aquí también, por ahora usaremos datos dummy o simulados

namespace HRManagement.WebApi.Services
{
    // Heredamos de la base generada por el nuevo Proto
    public class EmployeeGrpcService : EmployeeGrpc.EmployeeGrpcBase
    {
        private readonly IMediator _mediator;

        public EmployeeGrpcService(IMediator mediator)
        {
            _mediator = mediator;
        }

        // 1. Implementación de CreateEmployee (Tal como lo definimos en el Proto)
        public override async Task<CreateEmployeeResponse> CreateEmployee(CreateEmployeeRequest request, ServerCallContext context)
        {
            // Convertimos el mensaje de Proto (request) al Comando de MediatR
            var command = new CreateEmployeeCommand
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                Salary = (decimal)request.Salary, // Convertimos double a decimal
                PhoneNumber = request.PhoneNumber,
                Department = request.Department,
                PositionId = request.PositionId == 0 ? null : request.PositionId // Manejo de nulos
            };

            // Enviamos a la Capa Application
            var newEmployeeId = await _mediator.Send(command);

            // Devolvemos la respuesta que pide el Proto
            return new CreateEmployeeResponse
            {
                Success = true,
                NewId = newEmployeeId,
                Message = "Empleado creado exitosamente desde gRPC"
            };
        }

        // 2. Implementación de GetEmployeeInfo
        public override Task<EmployeeModel> GetEmployeeInfo(GetEmployeeRequest request, ServerCallContext context)
        {
            // NOTA: Aquí deberías llamar a una Query de MediatR (GetEmployeeByIdQuery).
            // Como no estoy seguro si ya creaste esa Query, pondré un dato simulado para que compile.
            // Cuando tengas la Query, cambias esto.

            var employeeModel = new EmployeeModel
            {
                Id = request.Id,
                FirstName = "Simulado",
                LastName = "Desde gRPC",
                Email = "test@grpc.com",
                Salary = 2000.50,
                Department = "IT",
                PhoneNumber = "123456",
                PositionId = 1
            };

            return Task.FromResult(employeeModel);
        }
    }
}