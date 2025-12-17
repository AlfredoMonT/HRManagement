USE HRManagementDb;
GO

-- 1. Borramos la tabla vieja para hacerla bien de nuevo
IF OBJECT_ID('dbo.Employees', 'U') IS NOT NULL
DROP TABLE dbo.Employees;
GO

-- 2. Creamos la tabla con TODOS los campos que pide tu código C#
CREATE TABLE Employees (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    FirstName NVARCHAR(100) NOT NULL,
    LastName NVARCHAR(100) NOT NULL,
    Email NVARCHAR(150) NOT NULL,
    Salary DECIMAL(18,2) NOT NULL,
    -- 👇 Estas son las columnas que faltaban y daban error
    PhoneNumber NVARCHAR(50) NULL,
    PositionId INT NULL
);
GO

-- 3. Insertamos un empleado de prueba completo
INSERT INTO Employees (FirstName, LastName, Email, Salary, PhoneNumber, PositionId)
VALUES ('Juan', 'Perez', 'juan@test.com', 1500.00, '999-888-777', NULL);
GO
