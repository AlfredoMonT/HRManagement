using HRManagement.Domain;
using HRManagement.Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace HRManagement.Application.Features.Employees.Commands.CreateEmployee
{
    // 1. EL COMANDO (Los datos que vienen del cliente)
    public class CreateEmployeeCommand : IRequest<int>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public decimal Salary { get; set; }
    }

    // 2. EL MANEJADOR (La lógica que guarda en BD)
    public class CreateEmployeeCommandHandler : IRequestHandler<CreateEmployeeCommand, int>
    {


        public CreateEmployeeCommandHandler(/* IEmployeeRepository repository */)
        {
            // _repository = repository;
        }

        public Task<int> Handle(CreateEmployeeCommand request, CancellationToken cancellationToken)
        {
            // LÓGICA SIMULADA (Para cumplir la guía rápido):
            // 1. Convertimos el comando a una entidad de dominio
            var employee = new Employee
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                Salary = request.Salary
            };

            // 2. Aquí guardarías en base de datos:
            // _repository.Add(employee);

            // 3. Retornamos un ID falso (ej. 10) para confirmar que funcionó
            return Task.FromResult(10);
        }
    }
}