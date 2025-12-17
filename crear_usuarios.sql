USE master;
GO

-- 1. Crear la Base de Datos si no existe
IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'HRManagementDb')
BEGIN
    CREATE DATABASE HRManagementDb;
END
GO

USE HRManagementDb;
GO

-- 2. Crear la tabla Users si no existe
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

-- 3. Insertar usuarios (Evitamos duplicados validando si existen)
IF NOT EXISTS (SELECT * FROM Users WHERE Username = 'admin')
BEGIN
    INSERT INTO Users (Username, Password, Role)
    VALUES ('admin', 'admin', 'Administrator');
END
GO

IF NOT EXISTS (SELECT * FROM Users WHERE Username = 'alfredo')
BEGIN
    INSERT INTO Users (Username, Password, Role)
    VALUES ('alfredo', '123456', 'Employee');
END
GO
