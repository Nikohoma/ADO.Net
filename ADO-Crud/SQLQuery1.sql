-- 1) Create database
IF DB_ID('TrainingDB') IS NULL
BEGIN
  CREATE DATABASE TrainingDB;
END
GO

USE TrainingDB;
GO

-- 2) Table
IF OBJECT_ID('dbo.Employees', 'U') IS NULL
BEGIN
  CREATE TABLE dbo.Employees (
    EmployeeId INT IDENTITY(1,1) PRIMARY KEY,
    FullName   NVARCHAR(100) NOT NULL,
    Department NVARCHAR(60)  NOT NULL,
    Salary     DECIMAL(12,2) NOT NULL
  );
END
GO

-- 3) Sample data (optional)
IF NOT EXISTS (SELECT 1 FROM dbo.Employees)
BEGIN
  INSERT INTO dbo.Employees(FullName, Department, Salary) VALUES
  ('Asha Kumar', 'IT', 65000),
  ('Ravi Sharma', 'HR', 45000),
  ('Meera Iyer', 'Finance', 70000);
END
GO

-- 4) Stored procedure: add employee (returns new EmployeeId)
CREATE OR ALTER PROCEDURE dbo.sp_AddEmployee
  @FullName NVARCHAR(100),
  @Department NVARCHAR(60),
  @Salary DECIMAL(12,2),
  @NewEmployeeId INT OUTPUT
AS
BEGIN
  SET NOCOUNT ON;

  INSERT INTO dbo.Employees(FullName, Department, Salary)
  VALUES (@FullName, @Department, @Salary);

  SET @NewEmployeeId = SCOPE_IDENTITY();
END
GO

-- 5) Stored procedure: get employee by id
CREATE OR ALTER PROCEDURE dbo.sp_GetEmployeeById
  @EmployeeId INT
AS
BEGIN
  SET NOCOUNT ON;

  SELECT EmployeeId, FullName, Department, Salary
  FROM dbo.Employees
  WHERE EmployeeId = @EmployeeId;
END
GO

-- 6) Stored procedure: update salary
CREATE OR ALTER PROCEDURE dbo.sp_UpdateSalary
  @EmployeeId INT,
  @Salary DECIMAL(12,2)
AS
BEGIN
  SET NOCOUNT ON;

  UPDATE dbo.Employees
  SET Salary = @Salary
  WHERE EmployeeId = @EmployeeId;
END
GO

-- 7) Stored procedure: count employees (Output)
CREATE OR ALTER PROCEDURE dbo.sp_CountEmployees
  @Total INT OUTPUT
AS
BEGIN
  SET NOCOUNT ON;

  SELECT @Total = COUNT(*) FROM dbo.Employees;
END
GOs