USE HRManagementDb;
GO

-- =============================================
-- PARTE 1: ASEGURAR USUARIOS (Para el Login)
-- =============================================

-- 1. Crear tabla Users si no existe
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Users' AND xtype='U')
BEGIN
    CREATE TABLE Users (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Username NVARCHAR(100) NOT NULL,
        Password NVARCHAR(100) NOT NULL,
        Role NVARCHAR(50) NOT NULL
    );
END
GO

-- 2. Insertar Admin si no existe (Para que no pierdas acceso)
IF NOT EXISTS (SELECT * FROM Users WHERE Username = 'admin')
BEGIN
    INSERT INTO Users (Username, Password, Role)
    VALUES ('admin', 'admin', 'Administrator');
END
GO

-- 3. Insertar Empleado de prueba si no existe
IF NOT EXISTS (SELECT * FROM Users WHERE Username = 'alfredo')
BEGIN
    INSERT INTO Users (Username, Password, Role)
    VALUES ('alfredo', '123456', 'Employee');
END
GO

-- =============================================
-- PARTE 2: ARREGLAR TABLA EMPLEADOS (Para el Error 500)
-- =============================================

-- 4. Borramos la tabla Employees vieja (la que le faltan columnas)
IF OBJECT_ID('dbo.Employees', 'U') IS NOT NULL
DROP TABLE dbo.Employees;
GO

-- 5. Creamos la tabla Employees DEFINITIVA (Con todas las columnas que pide C#)
CREATE TABLE Employees (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    FirstName NVARCHAR(100) NOT NULL,
    LastName NVARCHAR(100) NOT NULL,
    Email NVARCHAR(150) NOT NULL,
    Salary DECIMAL(18,2) NOT NULL,
    PhoneNumber NVARCHAR(50) NULL,    -- C# lo pide
    PositionId INT NULL,              -- C# lo pide
    Department NVARCHAR(100) NULL     -- C# lo pide (era el error del GET)
);
GO

-- 6. Insertamos un dato de prueba para verificar
INSERT INTO Employees (FirstName, LastName, Email, Salary, PhoneNumber, Department)
VALUES ('Juan', 'Perez', 'juan@test.com', 1500.00, '999-000-111', 'IT');
GO
