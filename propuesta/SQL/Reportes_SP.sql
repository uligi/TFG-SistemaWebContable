IF NOT EXISTS (SELECT 1 FROM sys.schemas WHERE name = 'Reportes')
BEGIN
    EXEC('CREATE SCHEMA Reportes');
END;
GO

/* =========================================================
   REPORTE - RESUMEN FINANCIERO
   Conecta ingresos, gastos, CxC y CxP.
   ========================================================= */
CREATE OR ALTER PROCEDURE Reportes.sp_Reporte_ResumenFinanciero
    @FechaInicio DATETIME,
    @FechaFin DATETIME
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @TotalIngresos DECIMAL(18,2);
    DECLARE @TotalGastos DECIMAL(18,2);
    DECLARE @TotalCxC DECIMAL(18,2);
    DECLARE @TotalCxP DECIMAL(18,2);
    DECLARE @UtilidadEstimada DECIMAL(18,2);
    DECLARE @MargenUtilidad DECIMAL(18,2);
    DECLARE @CapitalNetoPendiente DECIMAL(18,2);

    SELECT @TotalIngresos = ISNULL(SUM(i.Monto), 0)
    FROM dbo.Ingreso i
    WHERE i.Activo = 1
      AND CAST(i.FechaIngreso AS DATE) BETWEEN CAST(@FechaInicio AS DATE) AND CAST(@FechaFin AS DATE);

    SELECT @TotalGastos = ISNULL(SUM(g.Monto), 0)
    FROM dbo.Gasto g
    WHERE g.Activo = 1
      AND CAST(g.FechaGasto AS DATE) BETWEEN CAST(@FechaInicio AS DATE) AND CAST(@FechaFin AS DATE);

    SELECT @TotalCxC = ISNULL(SUM(cxc.SaldoActual), 0)
    FROM dbo.CuentaPorCobrar cxc
    WHERE cxc.Activo = 1
      AND cxc.SaldoActual > 0
      AND cxc.Estado IN ('Pendiente', 'Parcial', 'Vencida');

    SELECT @TotalCxP = ISNULL(SUM(cxp.SaldoActual), 0)
    FROM dbo.CuentaPorPagar cxp
    WHERE cxp.Activo = 1
      AND cxp.SaldoActual > 0
      AND cxp.Estado IN ('Pendiente', 'Parcial', 'Vencida');

    SET @UtilidadEstimada = @TotalIngresos - @TotalGastos;

    SET @MargenUtilidad =
        CASE
            WHEN @TotalIngresos = 0 THEN 0
            ELSE ROUND((@UtilidadEstimada / @TotalIngresos) * 100, 2)
        END;

    SET @CapitalNetoPendiente = @TotalCxC - @TotalCxP;

    SELECT
        @TotalIngresos AS TotalIngresos,
        @TotalGastos AS TotalGastos,
        @UtilidadEstimada AS UtilidadEstimada,
        @TotalCxC AS TotalCuentasPorCobrar,
        @TotalCxP AS TotalCuentasPorPagar,
        @MargenUtilidad AS MargenUtilidad,
        @CapitalNetoPendiente AS CapitalNetoPendiente,

        CASE
            WHEN @UtilidadEstimada > 0 THEN 'Rentable'
            WHEN @UtilidadEstimada = 0 THEN 'Equilibrado'
            ELSE 'Pérdida'
        END AS EstadoFinanciero;
END;
GO

/* =========================================================
   REPORTE - INGRESOS
   Usa NumeroIngreso y CodigoCuenta reales.
   ========================================================= */
CREATE OR ALTER PROCEDURE Reportes.sp_Reporte_Ingresos
    @FechaInicio DATETIME,
    @FechaFin DATETIME
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        i.NumeroIngreso,
        i.FechaIngreso,
        ti.Nombre AS TipoIngreso,
        i.OrigenIngreso,
        i.NumeroFactura,
        i.CodigoCuenta,
        cc.NombreCuenta,
        i.Descripcion,
        i.Monto,
        i.Activo
    FROM dbo.Ingreso i
    INNER JOIN dbo.TipoIngreso ti
        ON ti.IdTipoIngreso = i.IdTipoIngreso
    INNER JOIN dbo.CuentaContable cc
        ON cc.CodigoCuenta = i.CodigoCuenta
    WHERE i.Activo = 1
      AND CAST(i.FechaIngreso AS DATE) BETWEEN CAST(@FechaInicio AS DATE) AND CAST(@FechaFin AS DATE)
    ORDER BY
        i.FechaIngreso DESC,
        i.NumeroIngreso DESC;
END;
GO

/* =========================================================
   REPORTE - GASTOS
   Usa NumeroGasto y CodigoCuenta reales.
   ========================================================= */
CREATE OR ALTER PROCEDURE Reportes.sp_Reporte_Gastos
    @FechaInicio DATETIME,
    @FechaFin DATETIME
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        g.NumeroGasto,
        g.FechaGasto,
        tg.Nombre AS TipoGasto,
        g.CodigoCuenta,
        cc.NombreCuenta,

        g.IdentificacionProveedor,
        p.RazonSocial AS ProveedorNombre,
        g.NumeroDocumento,

        g.Descripcion,
        g.Monto,

        g.NumeroComprobante,
        g.NombreArchivoComprobante,
        g.RutaComprobante,

        g.Activo
    FROM dbo.Gasto g
    INNER JOIN dbo.TipoGasto tg
        ON tg.IdTipoGasto = g.IdTipoGasto
    INNER JOIN dbo.CuentaContable cc
        ON cc.CodigoCuenta = g.CodigoCuenta
    INNER JOIN dbo.Proveedor p
        ON p.IdentificacionProveedor = g.IdentificacionProveedor
    WHERE g.Activo = 1
      AND CAST(g.FechaGasto AS DATE) BETWEEN CAST(@FechaInicio AS DATE) AND CAST(@FechaFin AS DATE)
    ORDER BY
        g.FechaGasto DESC,
        g.NumeroGasto DESC;
END;
GO

/* =========================================================
   REPORTE - CUENTAS POR COBRAR
   Usa PK real: IdentificacionCliente + NumeroFactura.
   ========================================================= */
CREATE OR ALTER PROCEDURE Reportes.sp_Reporte_CuentasPorCobrar
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        cxc.IdentificacionCliente,
        cxc.NumeroFactura,

        LTRIM(RTRIM(
            ISNULL(p.Nombre, '') + ' ' +
            ISNULL(p.PrimerApellido, '') + ' ' +
            ISNULL(p.SegundoApellido, '')
        )) AS ClienteNombre,

        cxc.FechaEmision,
        cxc.FechaVencimiento,
        cxc.MontoOriginal,
        cxc.SaldoActual,
        cxc.Estado,
        cxc.Activo,

        DATEDIFF(DAY, CAST(GETDATE() AS DATE), CAST(cxc.FechaVencimiento AS DATE)) AS DiasRestantes,

        CASE
            WHEN cxc.SaldoActual <= 0 THEN 'Pagada'
            WHEN cxc.Estado = 'Anulada' THEN 'Anulada'
            WHEN CAST(cxc.FechaVencimiento AS DATE) < CAST(GETDATE() AS DATE) THEN 'Vencida'
            WHEN DATEDIFF(DAY, CAST(GETDATE() AS DATE), CAST(cxc.FechaVencimiento AS DATE)) <= 3 THEN 'Por vencer'
            ELSE 'Pendiente'
        END AS EstadoCredito
    FROM dbo.CuentaPorCobrar cxc
    INNER JOIN dbo.Cliente c
        ON c.Identificacion = cxc.IdentificacionCliente
    INNER JOIN dbo.Persona p
        ON p.Identificacion = c.Identificacion
    WHERE cxc.Activo = 1
      AND cxc.SaldoActual > 0
      AND cxc.Estado IN ('Pendiente', 'Parcial', 'Vencida')
    ORDER BY
        cxc.FechaVencimiento ASC,
        cxc.NumeroFactura ASC;
END;
GO

/* =========================================================
   REPORTE - CUENTAS POR PAGAR
   Usa PK real: IdentificacionProveedor + NumeroDocumento.
   ========================================================= */
CREATE OR ALTER PROCEDURE Reportes.sp_Reporte_CuentasPorPagar
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        cxp.IdentificacionProveedor,
        p.RazonSocial AS ProveedorNombre,
        cxp.NumeroDocumento,
        cxp.FechaEmision,
        cxp.FechaVencimiento,
        cxp.Concepto,
        cxp.MontoOriginal,
        cxp.SaldoActual,
        cxp.Estado,
        cxp.Activo,

        DATEDIFF(DAY, CAST(GETDATE() AS DATE), CAST(cxp.FechaVencimiento AS DATE)) AS DiasRestantes,

        CASE
            WHEN cxp.SaldoActual <= 0 THEN 'Pagada'
            WHEN cxp.Estado = 'Anulada' THEN 'Anulada'
            WHEN CAST(cxp.FechaVencimiento AS DATE) < CAST(GETDATE() AS DATE) THEN 'Vencida'
            WHEN DATEDIFF(DAY, CAST(GETDATE() AS DATE), CAST(cxp.FechaVencimiento AS DATE)) <= 3 THEN 'Por vencer'
            ELSE 'Pendiente'
        END AS EstadoCredito
    FROM dbo.CuentaPorPagar cxp
    INNER JOIN dbo.Proveedor p
        ON p.IdentificacionProveedor = cxp.IdentificacionProveedor
    WHERE cxp.Activo = 1
      AND cxp.SaldoActual > 0
      AND cxp.Estado IN ('Pendiente', 'Parcial', 'Vencida')
    ORDER BY
        cxp.FechaVencimiento ASC,
        cxp.NumeroDocumento ASC;
END;
GO

/* =========================================================
   REPORTE - PRODUCTOS MÁS VENDIDOS
   Ayuda a saber qué productos impulsan el negocio.
   ========================================================= */
CREATE OR ALTER PROCEDURE Reportes.sp_Reporte_ProductosMasVendidos
    @FechaInicio DATETIME,
    @FechaFin DATETIME
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        df.CodigoProducto,
        p.NombreProducto,
        tp.Nombre AS TipoProducto,

        SUM(df.Cantidad) AS CantidadVendida,
        SUM(df.TotalLinea) AS TotalVendido,
        COUNT(DISTINCT df.NumeroFactura) AS VecesFacturado,

        p.StockActual,

        CASE
            WHEN p.StockActual <= 0 THEN 'Sin stock'
            WHEN p.StockActual <= SUM(df.Cantidad) THEN 'Revisar inventario'
            ELSE 'Stock disponible'
        END AS Recomendacion
    FROM dbo.DetalleFactura df
    INNER JOIN dbo.Factura f
        ON f.NumeroFactura = df.NumeroFactura
    INNER JOIN dbo.Producto p
        ON p.CodigoProducto = df.CodigoProducto
    INNER JOIN dbo.TipoProducto tp
        ON tp.IdTipoProducto = p.IdTipoProducto
    WHERE df.Activo = 1
      AND f.Activo = 1
      AND f.Estado = 'Emitida'
      AND CAST(f.FechaFactura AS DATE) BETWEEN CAST(@FechaInicio AS DATE) AND CAST(@FechaFin AS DATE)
    GROUP BY
        df.CodigoProducto,
        p.NombreProducto,
        tp.Nombre,
        p.StockActual
    ORDER BY
        TotalVendido DESC,
        CantidadVendida DESC;
END;
GO

/* =========================================================
   REPORTE - CLIENTES CON MÁS VENTAS
   Ayuda a detectar clientes importantes.
   ========================================================= */
CREATE OR ALTER PROCEDURE Reportes.sp_Reporte_ClientesMasVentas
    @FechaInicio DATETIME,
    @FechaFin DATETIME
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        f.IdentificacionCliente,

        LTRIM(RTRIM(
            ISNULL(p.Nombre, '') + ' ' +
            ISNULL(p.PrimerApellido, '') + ' ' +
            ISNULL(p.SegundoApellido, '')
        )) AS ClienteNombre,

        COUNT(f.NumeroFactura) AS CantidadFacturas,
        SUM(f.TotalFactura) AS TotalComprado,
        AVG(f.TotalFactura) AS PromedioCompra,
        MAX(f.FechaFactura) AS UltimaCompra,

        c.LimiteCredito,
        c.DiasCredito,

        CASE
            WHEN COUNT(f.NumeroFactura) >= 5 THEN 'Cliente frecuente'
            WHEN SUM(f.TotalFactura) >= 100000 THEN 'Cliente de alto valor'
            ELSE 'Cliente ocasional'
        END AS ClasificacionCliente
    FROM dbo.Factura f
    INNER JOIN dbo.Cliente c
        ON c.Identificacion = f.IdentificacionCliente
    INNER JOIN dbo.Persona p
        ON p.Identificacion = c.Identificacion
    WHERE f.Activo = 1
      AND f.Estado = 'Emitida'
      AND CAST(f.FechaFactura AS DATE) BETWEEN CAST(@FechaInicio AS DATE) AND CAST(@FechaFin AS DATE)
    GROUP BY
        f.IdentificacionCliente,
        p.Nombre,
        p.PrimerApellido,
        p.SegundoApellido,
        c.LimiteCredito,
        c.DiasCredito
    ORDER BY
        TotalComprado DESC,
        CantidadFacturas DESC;
END;
GO

/* =========================================================
   REPORTE - FLUJO DIARIO
   Ayuda a ver días fuertes y días débiles.
   ========================================================= */
CREATE OR ALTER PROCEDURE Reportes.sp_Reporte_FlujoDiario
    @FechaInicio DATETIME,
    @FechaFin DATETIME
AS
BEGIN
    SET NOCOUNT ON;

    ;WITH Ingresos AS
    (
        SELECT
            CAST(i.FechaIngreso AS DATE) AS Fecha,
            SUM(i.Monto) AS TotalIngresos
        FROM dbo.Ingreso i
        WHERE i.Activo = 1
          AND CAST(i.FechaIngreso AS DATE) BETWEEN CAST(@FechaInicio AS DATE) AND CAST(@FechaFin AS DATE)
        GROUP BY CAST(i.FechaIngreso AS DATE)
    ),
    Gastos AS
    (
        SELECT
            CAST(g.FechaGasto AS DATE) AS Fecha,
            SUM(g.Monto) AS TotalGastos
        FROM dbo.Gasto g
        WHERE g.Activo = 1
          AND CAST(g.FechaGasto AS DATE) BETWEEN CAST(@FechaInicio AS DATE) AND CAST(@FechaFin AS DATE)
        GROUP BY CAST(g.FechaGasto AS DATE)
    ),
    Fechas AS
    (
        SELECT Fecha FROM Ingresos
        UNION
        SELECT Fecha FROM Gastos
    )
    SELECT
        f.Fecha,
        ISNULL(i.TotalIngresos, 0) AS TotalIngresos,
        ISNULL(g.TotalGastos, 0) AS TotalGastos,
        ISNULL(i.TotalIngresos, 0) - ISNULL(g.TotalGastos, 0) AS ResultadoDiario,

        CASE
            WHEN ISNULL(i.TotalIngresos, 0) - ISNULL(g.TotalGastos, 0) > 0 THEN 'Positivo'
            WHEN ISNULL(i.TotalIngresos, 0) - ISNULL(g.TotalGastos, 0) = 0 THEN 'Equilibrado'
            ELSE 'Negativo'
        END AS EstadoDia
    FROM Fechas f
    LEFT JOIN Ingresos i
        ON i.Fecha = f.Fecha
    LEFT JOIN Gastos g
        ON g.Fecha = f.Fecha
    ORDER BY
        f.Fecha ASC;
END;
GO