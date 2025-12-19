using Grpc.Core;
using MediatR;
using HRManagement.WebApi.Protos;
using HRManagement.Application.Features.Employees.Commands.CreateEmployee;
using HRManagement.Application.Interfaces; 
using Microsoft.EntityFrameworkCore;       

namespace HRManagement.WebApi.Services
{
    public class EmployeeGrpcService : EmployeeGrpc.EmployeeGrpcBase
    {
        private readonly IMediator _mediator;
        private readonly IHrManagementDbContext _context; 

        // Actualizamos el constructor
        public EmployeeGrpcService(IMediator mediator, IHrManagementDbContext context)
        {
            _mediator = mediator;
            _context = context;
        }

        // 1. CREATE 
        public override async Task<CreateEmployeeResponse> CreateEmployee(CreateEmployeeRequest request, ServerCallContext context)
        {
            try
            {
                var command = new CreateEmployeeCommand
                {
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    Email = request.Email,
                    Salary = (decimal)request.Salary,
                    PhoneNumber = request.PhoneNumber,
                    Department = request.Department,
                    PositionId = request.PositionId == 0 ? null : request.PositionId
                };

                var newEmployeeId = await _mediator.Send(command);

                return new CreateEmployeeResponse
                {
                    Success = true,
                    NewId = newEmployeeId,
                    Message = "Empleado creado exitosamente desde gRPC"
                };
            }
            catch (Exception ex)
            {
                return new CreateEmployeeResponse { Success = false, Message = ex.Message };
            }
        }

        // 2. GET (Ahora con DATOS REALES)
        public override async Task<EmployeeModel> GetEmployeeInfo(GetEmployeeRequest request, ServerCallContext context)
        {
            // Buscamos en la BD real
            var employee = await _context.Employees.FindAsync(request.Id);

            if (employee == null)
            {
                // Lanzamos error estándar de gRPC si no existe
                throw new RpcException(new Status(StatusCode.NotFound, $"Empleado ID {request.Id} no encontrado"));
            }

            return new EmployeeModel
            {
                Id = employee.Id,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Email = employee.Email,
                Salary = (double)employee.Salary,
                Department = employee.Department ?? "",
                PhoneNumber = employee.PhoneNumber ?? "",
                PositionId = employee.PositionId ?? 0
            };
        }
    }
}