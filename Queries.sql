--Crear Base de datos
CREATE DATABASE EstadoCuentaTarjeta;
--Crear Tabla Tarjetas
CREATE TABLE Tarjetas (
    IdTarjeta INT IDENTITY(1,1) PRIMARY KEY,
    NumeroTarjeta NVARCHAR(16) NOT NULL,
    NombreTitular NVARCHAR(100) NOT NULL,
    SaldoDisponible DECIMAL(18, 2) NOT NULL,
    SaldoUtilizado DECIMAL(18, 2) NOT NULL DEFAULT 0,
    LimiteCredito DECIMAL(18, 2) NOT NULL,
    FechaCreacion DATETIME NOT NULL DEFAULT GETDATE()
);
--Crear Tabla Movimientos
CREATE TABLE Movimientos (
    IdMovimiento INT IDENTITY(1,1) PRIMARY KEY,
    IdTarjeta INT NOT NULL FOREIGN KEY REFERENCES Tarjetas(IdTarjeta),
    FechaMovimiento DATETIME NOT NULL,
    Monto DECIMAL(18, 2) NOT NULL,
    TipoMovimiento NVARCHAR(50) NOT NULL, -- 'Cargo', 'Abono', etc.
    Descripcion NVARCHAR(255) NULL
);

--Ejemplos de Insert
INSERT INTO Tarjetas (NumeroTarjeta, NombreTitular, SaldoDisponible, LimiteCredito)
VALUES 
('1234567890123456', 'Juan Pérez', 1000.00, 2000.00),
('9876543210987654', 'Ana López', 500.00, 1500.00);
INSERT INTO Movimientos (IdTarjeta, FechaMovimiento, Monto, TipoMovimiento, Descripcion)
VALUES 
(1, GETDATE(), 100.00, 'Cargo', 'Compra en supermercado'),
(1, GETDATE(), 50.00, 'Cargo', 'Pago en restaurante'),
(2, GETDATE(), 200.00, 'Abono', 'Pago de tarjeta');

--Agregar clave foránea entre Movimientos y Tarjetas
ALTER TABLE Movimientos
ADD CONSTRAINT FK_Movimientos_Tarjetas
FOREIGN KEY (IdTarjeta) REFERENCES Tarjetas(IdTarjeta);
