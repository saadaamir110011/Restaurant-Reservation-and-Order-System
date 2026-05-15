-- =============================================
-- RESTAURANT RESERVATION SYSTEM - COMPLETE DB
-- Run this entire script in SSMS
-- =============================================

-- Step 1: Create Database
IF EXISTS (SELECT name FROM sys.databases WHERE name = 'RestaurantDB')
BEGIN
    ALTER DATABASE RestaurantDB SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE RestaurantDB;
END
GO

CREATE DATABASE RestaurantDB;
GO
USE RestaurantDB;
GO

-- =============================================
-- TABLES
-- =============================================

CREATE TABLE Users (
    UserID   INT PRIMARY KEY IDENTITY(1,1),
    Username VARCHAR(50)  UNIQUE NOT NULL,
    Password VARCHAR(100) NOT NULL
);

CREATE TABLE Reservations (
    ReservationID   INT PRIMARY KEY IDENTITY(1,1),
    GuestName       VARCHAR(100),
    Contact         VARCHAR(50),
    ReservationDate DATE,
    Time            VARCHAR(20),
    TableNumber     VARCHAR(20),
    Status          VARCHAR(20) DEFAULT 'Confirmed'
);

CREATE TABLE Orders (
    OrderID   INT PRIMARY KEY IDENTITY(1,1),
    ItemName  VARCHAR(100),
    Price     FLOAT,
    Quantity  INT,
    SubTotal  FLOAT,
    OrderDate DATETIME DEFAULT GETDATE()
);

CREATE TABLE Sales (
    SaleID  INT PRIMARY KEY IDENTITY(1,1),
    DayName VARCHAR(10),
    Amount  FLOAT
);
GO

-- =============================================
-- DEFAULT DATA
-- =============================================

-- Login: admin / saad7890
INSERT INTO Users (Username, Password) VALUES ('admin', 'saad7890');

-- Weekly Sales for Dashboard Chart
INSERT INTO Sales (DayName, Amount) VALUES
('Mon', 2000),
('Tue', 1500),
('Wed', 2500),
('Thu', 1800),
('Fri', 3500),
('Sat', 4000),
('Sun', 3200);
GO

-- =============================================
-- STORED PROCEDURES
-- =============================================

-- LOGIN
CREATE PROCEDURE sp_Login
    @Username VARCHAR(50),
    @Password VARCHAR(100)
AS
BEGIN
    SELECT COUNT(*)
    FROM Users
    WHERE Username = @Username AND Password = @Password
END;
GO

-- ADD RESERVATION
CREATE PROCEDURE sp_AddReservation
    @GuestName      VARCHAR(100),
    @Contact        VARCHAR(50),
    @ReservationDate DATE,
    @Time           VARCHAR(20),
    @TableNumber    VARCHAR(20)
AS
BEGIN
    INSERT INTO Reservations (GuestName, Contact, ReservationDate, Time, TableNumber, Status)
    VALUES (@GuestName, @Contact, @ReservationDate, @Time, @TableNumber, 'Confirmed')
END;
GO

-- GET ALL RESERVATIONS
CREATE PROCEDURE sp_GetReservations
AS
BEGIN
    SELECT
        ReservationID AS [Res ID],
        GuestName     AS [Customer Name],
        ReservationDate AS [Date],
        Time,
        TableNumber   AS [Table],
        Status
    FROM Reservations
    ORDER BY ReservationID DESC
END;
GO

-- ADD ORDER
CREATE PROCEDURE sp_AddOrder
    @ItemName VARCHAR(100),
    @Price    FLOAT,
    @Quantity INT
AS
BEGIN
    INSERT INTO Orders (ItemName, Price, Quantity, SubTotal)
    VALUES (@ItemName, @Price, @Quantity, @Price * @Quantity)
END;
GO

-- GET TOTAL BILL
CREATE PROCEDURE sp_GetTotalBill
AS
BEGIN
    SELECT ISNULL(SUM(SubTotal), 0) AS Total FROM Orders
END;
GO

-- =============================================
-- FUNCTIONS
-- =============================================

CREATE FUNCTION dbo.fn_CalcTotal(@price FLOAT, @qty INT)
RETURNS FLOAT
AS
BEGIN
    RETURN @price * @qty;
END;
GO

CREATE FUNCTION dbo.fn_TotalReservations()
RETURNS INT
AS
BEGIN
    DECLARE @count INT;
    SELECT @count = COUNT(*) FROM Reservations;
    RETURN @count;
END;
GO

-- =============================================
-- TRIGGERS
-- =============================================

-- After Order inserted → update Sales table for that day
CREATE TRIGGER trg_UpdateSales
ON Orders
AFTER INSERT
AS
BEGIN
    DECLARE @day    VARCHAR(10) = LEFT(DATENAME(WEEKDAY, GETDATE()), 3);
    DECLARE @amount FLOAT;
    SELECT @amount = SubTotal FROM inserted;

    IF EXISTS (SELECT 1 FROM Sales WHERE DayName = @day)
        UPDATE Sales SET Amount = Amount + @amount WHERE DayName = @day;
    ELSE
        INSERT INTO Sales (DayName, Amount) VALUES (@day, @amount);
END;
GO

-- After Reservation deleted → log message
CREATE TRIGGER trg_DeleteReservation
ON Reservations
AFTER DELETE
AS
BEGIN
    PRINT 'Reservation deleted successfully';
END;
GO

-- =============================================
-- VERIFY: Test these after running the script
-- =============================================
-- EXEC sp_Login 'admin', 'saad7890'           --> Should return 1
-- EXEC sp_AddReservation 'Ali','0300','2026-05-01','8PM','T1'
-- EXEC sp_AddOrder 'Burger', 500, 2
-- SELECT * FROM Reservations
-- SELECT * FROM Orders
-- SELECT * FROM Sales
-- SELECT dbo.fn_TotalReservations()
-- SELECT dbo.fn_CalcTotal(100, 3)
