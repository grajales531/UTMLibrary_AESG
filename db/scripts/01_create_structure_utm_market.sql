/*
  =============================================================================
  Script: 01_create_structure_utm_market.sql
  Descripción: Definición de esquema para el sistema UtmMarket.
               Incluye tablas de Producto, Venta y Detalle de Venta.
  Arquitecto: Senior Database Architect / Gemini CLI
  Motor: Microsoft SQL Server 2022 Express
  =============================================================================
*/

-- Configuración de entorno y manejo de errores
SET NOCOUNT ON;
SET XACT_ABORT ON;
GO

-- Asegurar el uso de la base de datos Grajales
IF EXISTS (SELECT name FROM sys.databases WHERE name = N'Grajales')
BEGIN
    USE [Grajales];
END
ELSE
BEGIN
    PRINT 'ERROR: La base de datos [Grajales] no existe.';
    -- Detener ejecución si no existe la base de datos base
    DECLARE @msg NVARCHAR(MAX) = 'La base de datos [Grajales] no existe. Ejecución abortada.';
    RAISERROR(@msg, 16, 1);
    RETURN;
END
GO

/* 
  -----------------------------------------------------------------------------
  1. TABLA: Producto
  Almacena el catálogo de productos con control de SKU único y stock.
  -----------------------------------------------------------------------------
*/
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Producto]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Producto] (
        [ProductoID] INT IDENTITY(1,1) NOT NULL,
        [Nombre]     NVARCHAR(100)  NOT NULL,
        [SKU]        VARCHAR(20)    NOT NULL,
        [Marca]      NVARCHAR(50)   NULL,
        [Precio]     DECIMAL(19,4)  NOT NULL,
        [Stock]      INT            NOT NULL,
        
        CONSTRAINT [PK_Producto] PRIMARY KEY CLUSTERED ([ProductoID]),
        CONSTRAINT [UQ_Producto_SKU] UNIQUE ([SKU]),
        CONSTRAINT [CK_Producto_Precio] CHECK ([Precio] >= 0),
        CONSTRAINT [CK_Producto_Stock] CHECK ([Stock] >= 0)
    );
    PRINT 'INFO: Tabla [Producto] creada exitosamente.';
END
ELSE
BEGIN
    PRINT 'INFO: La tabla [Producto] ya existe.';
END
GO

/* 
  -----------------------------------------------------------------------------
  2. TABLA: Venta
  Encabezado de las transacciones comerciales. 
  Estatus: 1 = Pendiente, 2 = Completada, 3 = Cancelada.
  -----------------------------------------------------------------------------
*/
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Venta]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Venta] (
        [VentaID]        INT IDENTITY(1,1) NOT NULL,
        [Folio]          VARCHAR(20)    NOT NULL,
        [FechaVenta]     DATETIME       NOT NULL DEFAULT (GETDATE()),
        [TotalArticulos] INT            NOT NULL,
        [TotalVenta]     DECIMAL(19,4)  NOT NULL,
        [Estatus]        TINYINT        NOT NULL, -- 1: Pendiente, 2: Completada, 3: Cancelada
        
        CONSTRAINT [PK_Venta] PRIMARY KEY CLUSTERED ([VentaID]),
        CONSTRAINT [UQ_Venta_Folio] UNIQUE ([Folio]),
        CONSTRAINT [CK_Venta_Estatus] CHECK ([Estatus] IN (1, 2, 3)),
        CONSTRAINT [CK_Venta_TotalArticulos] CHECK ([TotalArticulos] >= 0),
        CONSTRAINT [CK_Venta_TotalVenta] CHECK ([TotalVenta] >= 0)
    );
    PRINT 'INFO: Tabla [Venta] creada exitosamente.';
END
ELSE
BEGIN
    PRINT 'INFO: La tabla [Venta] ya existe.';
END
GO

/* 
  -----------------------------------------------------------------------------
  3. TABLA: DetalleVenta
  Desglose de artículos por venta. Relaciona Productos con Ventas (1:N).
  -----------------------------------------------------------------------------
*/
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DetalleVenta]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[DetalleVenta] (
        [DetalleID]      INT IDENTITY(1,1) NOT NULL,
        [VentaID]        INT            NOT NULL,
        [ProductoID]     INT            NOT NULL,
        [PrecioUnitario] DECIMAL(19,4)  NOT NULL,
        [Cantidad]       INT            NOT NULL,
        [TotalDetalle]   DECIMAL(19,4)  NOT NULL,
        
        CONSTRAINT [PK_DetalleVenta] PRIMARY KEY CLUSTERED ([DetalleID]),
        CONSTRAINT [FK_DetalleVenta_Venta] FOREIGN KEY ([VentaID]) 
            REFERENCES [dbo].[Venta] ([VentaID]) ON DELETE CASCADE,
        CONSTRAINT [FK_DetalleVenta_Producto] FOREIGN KEY ([ProductoID]) 
            REFERENCES [dbo].[Producto] ([ProductoID]),
        CONSTRAINT [CK_DetalleVenta_PrecioUnitario] CHECK ([PrecioUnitario] >= 0),
        CONSTRAINT [CK_DetalleVenta_Cantidad] CHECK ([Cantidad] > 0),
        CONSTRAINT [CK_DetalleVenta_TotalDetalle] CHECK ([TotalDetalle] >= 0)
    );
    PRINT 'INFO: Tabla [DetalleVenta] creada exitosamente.';
END
ELSE
BEGIN
    PRINT 'INFO: La tabla [DetalleVenta] ya existe.';
END
GO

PRINT 'Script de arquitectura de base de datos finalizado.';
