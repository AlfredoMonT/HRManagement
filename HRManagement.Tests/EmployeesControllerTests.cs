using Xunit;
using Moq;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using HRManagement.WebApi.Controllers;
using HRManagement.Application.Features.Employees.Commands.CreateEmployee;
using HRManagement.Application.Interfaces; // Necesario para el mock del Context
using System.Threading;
using System.Threading.Tasks;

namespace HRManagement.Tests
{
    public class EmployeesControllerTests
    {
        private readonly Mock<IMediator> _mockMediator;
        private readonly Mock<IHrManagementDbContext> _mockContext; // 1. Nuevo Mock necesario
        private readonly EmployeesController _controller;

        public EmployeesControllerTests()
        {
            _mockMediator = new Mock<IMediator>();
            _mockContext = new Mock<IHrManagementDbContext>(); // 2. Inicializamos el mock

            // 3. Pasamos AMBOS mocks al constructor (antes solo pasábamos mediator)
            _controller = new EmployeesController(_mockMediator.Object, _mockContext.Object);
        }

        [Fact]
        public async Task CreateEmployee_ReturnsOkResult()
        {
            // Arrange
            var command = new CreateEmployeeCommand
            {
                FirstName = "Test",
                LastName = "User",
                Email = "test@example.com",
                Salary = 1000,
                // Agregamos los campos requeridos para evitar warnings de null
                PhoneNumber = "123456789",
                Department = "IT",
                PositionId = 1
            };

            // Simulamos que MediatR devuelve el ID 1 cuando se le llama
            _mockMediator.Setup(m => m.Send(It.IsAny<CreateEmployeeCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            // Act
            // 4. CORRECCIÓN CLAVE: Llamamos a 'CreateEmployee', no a 'Create'
            var result = await _controller.CreateEmployee(command);

            // Assert
            var actionResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(200, actionResult.StatusCode);
        }
    }
} 