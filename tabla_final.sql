USE HRManagementDb;
GO

-- 1. Borramos la tabla para hacerla de nuevo con TODAS las columnas
IF OBJECT_ID('dbo.Employees', 'U') IS NOT NULL
DROP TABLE dbo.Employees;
GO

-- 2. Creamos la tabla con TODO lo que tu código podría pedir
CREATE TABLE Employees (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    FirstName NVARCHAR(100) NOT NULL,
    LastName NVARCHAR(100) NOT NULL,
    Email NVARCHAR(150) NOT NULL,
    Salary DECIMAL(18,2) NOT NULL,
    PhoneNumber NVARCHAR(50) NULL,
    PositionId INT NULL,
    Department NVARCHAR(100) NULL  -- <--- ESTA FALTABA
);
GO

-- 3. Insertamos un empleado de prueba
INSERT INTO Employees (FirstName, LastName, Email, Salary, PhoneNumber, PositionId, Department)
VALUES ('Juan', 'Perez', 'juan@test.com', 1500.00, '999-888-777', NULL, 'IT');
GO
