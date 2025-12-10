using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using HRManagement.WebApi.Controllers;
using MediatR;
using HRManagement.Infrastructure.Persistence;
using HRManagement.Application.Features.Employees.Commands.CreateEmployee;
using System.Threading;
using System.Threading.Tasks;

namespace HRManagement.Tests
{
    public class EmployeesControllerTests
    {
        // Definimos los Mocks (Los "dobles" de acción)
        private readonly Mock<IMediator> _mockMediator;
        private readonly Mock<HrManagementDbContext> _mockContext;

        public EmployeesControllerTests()
        {
            // Inicializamos los simuladores
            _mockMediator = new Mock<IMediator>();
            _mockContext = new Mock<HrManagementDbContext>(); // No lo usaremos en el Create, así que puede estar vacío
        }

        [Fact]
        public async Task Create_ShouldReturnOk_WhenCommandIsValid()
        {
            // 1. ARRANGE (Preparar)
            // Configuramos el simulador: "Cuando te envíen cualquier comando, responde con el ID 99"
            _mockMediator.Setup(m => m.Send(It.IsAny<CreateEmployeeCommand>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(99);

            // Instanciamos el controlador inyectándole los objetos falsos
            var controller = new EmployeesController(_mockMediator.Object, null);
            // Nota: Pasamos 'null' al DbContext porque el método Create (CQRS) no lo usa, usa Mediator.

            var command = new CreateEmployeeCommand
            {
                FirstName = "Test",
                LastName = "User",
                Email = "test@mail.com",
                Salary = 1000
            };

            // 2. ACT (Actuar)
            var result = await controller.Create(command);

            // 3. ASSERT (Verificar)
            // Verificamos que la respuesta sea un "200 OK"
            var okResult = Assert.IsType<OkObjectResult>(result);
            // Verificamos que el Mediator haya sido llamado exactamente 1 vez
            _mockMediator.Verify(x => x.Send(It.IsAny<CreateEmployeeCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}