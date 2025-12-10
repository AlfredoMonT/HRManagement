using Grpc.Core;
using HRManagement.Infrastructure.Persistence; // Tu DbContext
using HRManagement.WebApi.Protos; // El namespace que definimos en el .proto

namespace HRManagement.WebApi.Services
{
    // Heredamos de la clase base que se genera automáticamente del .proto
    public class EmployeeGrpcService : EmployeeGrpc.EmployeeGrpcBase
    {
        private readonly HrManagementDbContext _context;

        public EmployeeGrpcService(HrManagementDbContext context)
        {
            _context = context;
        }

        public override Task<EmployeeReply> GetEmployeeInfo(EmployeeRequest request, ServerCallContext context)
        {
            var employee = _context.Employees.Find(request.Id);

            if (employee == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, $"Empleado con ID {request.Id} no encontrado"));
            }

            // CORRECCIÓN: Usamos FirstName y LastName por separado, tal como está en el .proto
            return Task.FromResult(new EmployeeReply
            {
                Id = employee.Id,
                FirstName = employee.FirstName ?? "",  // Usa FirstName
                LastName = employee.LastName ?? "",    // Usa LastName
                Email = employee.Email ?? ""
                // Borra la línea de IsActive si tu proto no la tiene
            });
        }
    }
}