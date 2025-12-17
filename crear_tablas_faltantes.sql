USE HRManagementDb;
GO

-- 1. ¡IMPORTANTE! Borramos la tabla vieja si existe para evitar conflictos
IF OBJECT_ID('dbo.Employees', 'U') IS NOT NULL
DROP TABLE dbo.Employees;
GO

-- 2. Creamos la tabla nueva con la estructura EXACTA de tu JSON
CREATE TABLE Employees (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    FirstName NVARCHAR(100) NOT NULL,
    LastName NVARCHAR(100) NOT NULL,
    Email NVARCHAR(150) NOT NULL,
    Salary DECIMAL(18,2) NOT NULL -- Ahora sí se creará esta columna
);
GO

-- 3. Insertar dato de prueba
INSERT INTO Employees (FirstName, LastName, Email, Salary)
VALUES ('Juan', 'Perez', 'juan@test.com', 100.00);
GO
