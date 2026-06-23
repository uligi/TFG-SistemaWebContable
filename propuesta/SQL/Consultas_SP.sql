IF NOT EXISTS (SELECT 1 FROM sys.schemas WHERE name = 'Consultas')
BEGIN
    EXEC('CREATE SCHEMA Consultas');
END;
GO

/* =========================================================
   CONSULTA GENERAL - FACTURAS
   Busca facturas por número, cliente, empleado, estado o fecha.
   CORREGIDO SEGÚN DER REAL
   ========================================================= */
CREATE OR ALTER PROCEDURE Consultas.sp_Consulta_Facturas
    @Filtro VARCHAR(100),
    @FechaInicio DATETIME = NULL,
    @FechaFin DATETIME = NULL,
    @Estado VARCHAR(45) = ''
AS
BEGIN
    SET NOCOUNT ON;

    SET @Filtro = LTRIM(RTRIM(ISNULL(@Filtro, '')));
    SET @Estado = LTRIM(RTRIM(ISNULL(@Estado, '')));

    SELECT
        f.NumeroFactura,
        f.FechaFactura,
        f.IdentificacionCliente,

        LTRIM(RTRIM(
            ISNULL(pc.Nombre, '') + ' ' +
            ISNULL(pc.PrimerApellido, '') + ' ' +
            ISNULL(pc.SegundoApellido, '')
        )) AS ClienteNombre,

        f.IdentificacionEmpleado,

        LTRIM(RTRIM(
            ISNULL(pe.Nombre, '') + ' ' +
            ISNULL(pe.PrimerApellido, '') + ' ' +
            ISNULL(pe.SegundoApellido, '')
        )) AS EmpleadoNombre,

        f.IdTipoFactura,
        tf.TipoFactura AS TipoFacturaNombre,

        f.IdTipoPago,
        tp.Nombre AS TipoPagoNombre,

        f.Subtotal AS SubtotalFactura,
        f.TotalImpuesto AS ImpuestoFactura,
        f.TotalDescuento AS DescuentoFactura,
        f.TotalFactura,

        f.Estado,
        f.Activo
    FROM dbo.Factura f
    INNER JOIN dbo.Cliente c
        ON c.Identificacion = f.IdentificacionCliente
    INNER JOIN dbo.Persona pc
        ON pc.Identificacion = c.Identificacion
    INNER JOIN dbo.Empleado e
        ON e.Identificacion = f.IdentificacionEmpleado
    INNER JOIN dbo.Persona pe
        ON pe.Identificacion = e.Identificacion
    INNER JOIN dbo.TipoFactura tf
        ON tf.IdTipoFactura = f.IdTipoFactura
    INNER JOIN dbo.TipoPago tp
        ON tp.IdTipoPago = f.IdTipoPago
    WHERE
        (
            @Filtro = ''
            OR f.NumeroFactura LIKE '%' + @Filtro + '%'
            OR f.IdentificacionCliente LIKE '%' + @Filtro + '%'
            OR pc.Nombre LIKE '%' + @Filtro + '%'
            OR pc.PrimerApellido LIKE '%' + @Filtro + '%'
            OR pc.SegundoApellido LIKE '%' + @Filtro + '%'
            OR f.IdentificacionEmpleado LIKE '%' + @Filtro + '%'
            OR pe.Nombre LIKE '%' + @Filtro + '%'
            OR pe.PrimerApellido LIKE '%' + @Filtro + '%'
            OR pe.SegundoApellido LIKE '%' + @Filtro + '%'
            OR f.Estado LIKE '%' + @Filtro + '%'
        )
        AND (@Estado = '' OR f.Estado = @Estado)
        AND (@FechaInicio IS NULL OR CAST(f.FechaFactura AS DATE) >= CAST(@FechaInicio AS DATE))
        AND (@FechaFin IS NULL OR CAST(f.FechaFactura AS DATE) <= CAST(@FechaFin AS DATE))
    ORDER BY
        f.FechaFactura DESC,
        f.NumeroFactura DESC;
END;
GO

/* =========================================================
   CONSULTA GENERAL - DETALLE DE FACTURA
   Muestra productos de una factura.
   ========================================================= */
CREATE OR ALTER PROCEDURE Consultas.sp_Consulta_DetalleFactura
    @NumeroFactura VARCHAR(45)
AS
BEGIN
    SET NOCOUNT ON;

    SET @NumeroFactura = LTRIM(RTRIM(ISNULL(@NumeroFactura, '')));

    SELECT
        df.NumeroFactura,
        df.CodigoProducto,
        p.NombreProducto,
        df.DescripcionItem,
        df.Cantidad,
        df.PrecioUnitario,
        df.PorcentajeImpuesto,
        df.PorcentajeDescuento,
        df.SubtotalLinea,
        df.TotalLinea,
        df.Activo
    FROM dbo.DetalleFactura df
    INNER JOIN dbo.Producto p
        ON p.CodigoProducto = df.CodigoProducto
    WHERE df.NumeroFactura = @NumeroFactura
    ORDER BY
        df.CodigoProducto;
END;
GO

/* =========================================================
   CONSULTA GENERAL - CUENTAS POR COBRAR
   Busca saldos pendientes de clientes.
   ========================================================= */
CREATE OR ALTER PROCEDURE Consultas.sp_Consulta_CuentasPorCobrar
    @Filtro VARCHAR(100),
    @Estado VARCHAR(45) = '',
    @EstadoCredito VARCHAR(45) = ''
AS
BEGIN
    SET NOCOUNT ON;

    SET @Filtro = LTRIM(RTRIM(ISNULL(@Filtro, '')));
    SET @Estado = LTRIM(RTRIM(ISNULL(@Estado, '')));
    SET @EstadoCredito = LTRIM(RTRIM(ISNULL(@EstadoCredito, '')));

    ;WITH BaseCxC AS
    (
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
    )
    SELECT *
    FROM BaseCxC
    WHERE
        (
            @Filtro = ''
            OR IdentificacionCliente LIKE '%' + @Filtro + '%'
            OR NumeroFactura LIKE '%' + @Filtro + '%'
            OR ClienteNombre LIKE '%' + @Filtro + '%'
        )
        AND (@Estado = '' OR Estado = @Estado)
        AND (@EstadoCredito = '' OR EstadoCredito = @EstadoCredito)
    ORDER BY
        FechaVencimiento ASC,
        NumeroFactura ASC;
END;
GO

/* =========================================================
   CONSULTA GENERAL - CUENTAS POR PAGAR
   Busca documentos pendientes de proveedores.
   ========================================================= */
CREATE OR ALTER PROCEDURE Consultas.sp_Consulta_CuentasPorPagar
    @Filtro VARCHAR(100),
    @Estado VARCHAR(45) = '',
    @EstadoCredito VARCHAR(45) = ''
AS
BEGIN
    SET NOCOUNT ON;

    SET @Filtro = LTRIM(RTRIM(ISNULL(@Filtro, '')));
    SET @Estado = LTRIM(RTRIM(ISNULL(@Estado, '')));
    SET @EstadoCredito = LTRIM(RTRIM(ISNULL(@EstadoCredito, '')));

    ;WITH BaseCxP AS
    (
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
    )
    SELECT *
    FROM BaseCxP
    WHERE
        (
            @Filtro = ''
            OR IdentificacionProveedor LIKE '%' + @Filtro + '%'
            OR ProveedorNombre LIKE '%' + @Filtro + '%'
            OR NumeroDocumento LIKE '%' + @Filtro + '%'
            OR Concepto LIKE '%' + @Filtro + '%'
        )
        AND (@Estado = '' OR Estado = @Estado)
        AND (@EstadoCredito = '' OR EstadoCredito = @EstadoCredito)
    ORDER BY
        FechaVencimiento ASC,
        NumeroDocumento ASC;
END;
GO

/* =========================================================
   CONSULTA GENERAL - PRODUCTOS
   Búsqueda operativa de inventario.
   CORREGIDO SEGÚN DER REAL
   ========================================================= */
CREATE OR ALTER PROCEDURE Consultas.sp_Consulta_Productos
    @Filtro VARCHAR(100),
    @SoloBajoStock BIT = 0,
    @SoloSinStock BIT = 0,
    @SoloActivos BIT = 1
AS
BEGIN
    SET NOCOUNT ON;

    SET @Filtro = LTRIM(RTRIM(ISNULL(@Filtro, '')));

    SELECT
        p.CodigoProducto,
        p.NombreProducto,
        tp.Nombre AS TipoProducto,

        CAST(0 AS DECIMAL(18,2)) AS PrecioCompra,
        p.PrecioVenta,

        p.StockActual,
        CAST(5 AS DECIMAL(18,2)) AS StockMinimo,

        i.Porcentaje AS PorcentajeImpuesto,
        i.Nombre AS ImpuestoNombre,

        p.Activo,

        CASE
            WHEN p.StockActual <= 0 THEN 'Sin stock'
            WHEN p.StockActual <= 5 THEN 'Bajo stock'
            ELSE 'Stock disponible'
        END AS EstadoInventario
    FROM dbo.Producto p
    INNER JOIN dbo.TipoProducto tp
        ON tp.IdTipoProducto = p.IdTipoProducto
    INNER JOIN dbo.Impuesto i
        ON i.IdImpuesto = p.IdImpuesto
    WHERE
        (
            @Filtro = ''
            OR p.CodigoProducto LIKE '%' + @Filtro + '%'
            OR p.NombreProducto LIKE '%' + @Filtro + '%'
            OR tp.Nombre LIKE '%' + @Filtro + '%'
            OR i.Nombre LIKE '%' + @Filtro + '%'
        )
        AND (@SoloActivos = 0 OR p.Activo = 1)
        AND (@SoloBajoStock = 0 OR (p.StockActual > 0 AND p.StockActual <= 5))
        AND (@SoloSinStock = 0 OR p.StockActual <= 0)
    ORDER BY
        p.NombreProducto ASC;
END;
GO

/* =========================================================
   CONSULTA GENERAL - CLIENTES
   Búsqueda operativa de clientes y saldo pendiente.
   ========================================================= */
CREATE OR ALTER PROCEDURE Consultas.sp_Consulta_Clientes
    @Filtro VARCHAR(100),
    @SoloConSaldo BIT = 0,
    @SoloActivos BIT = 1
AS
BEGIN
    SET NOCOUNT ON;

    SET @Filtro = LTRIM(RTRIM(ISNULL(@Filtro, '')));

    SELECT
        c.Identificacion,

        LTRIM(RTRIM(
            ISNULL(p.Nombre, '') + ' ' +
            ISNULL(p.PrimerApellido, '') + ' ' +
            ISNULL(p.SegundoApellido, '')
        )) AS ClienteNombre,

        c.LimiteCredito,
        c.DiasCredito,
        c.Activo,

        ISNULL((
            SELECT SUM(cxc.SaldoActual)
            FROM dbo.CuentaPorCobrar cxc
            WHERE cxc.IdentificacionCliente = c.Identificacion
              AND cxc.Activo = 1
              AND cxc.SaldoActual > 0
        ), 0) AS SaldoPendiente,

        ISNULL((
            SELECT COUNT(1)
            FROM dbo.CuentaPorCobrar cxc
            WHERE cxc.IdentificacionCliente = c.Identificacion
              AND cxc.Activo = 1
              AND cxc.SaldoActual > 0
        ), 0) AS CantidadFacturasPendientes
    FROM dbo.Cliente c
    INNER JOIN dbo.Persona p
        ON p.Identificacion = c.Identificacion
    WHERE
        (
            @Filtro = ''
            OR c.Identificacion LIKE '%' + @Filtro + '%'
            OR p.Nombre LIKE '%' + @Filtro + '%'
            OR p.PrimerApellido LIKE '%' + @Filtro + '%'
            OR p.SegundoApellido LIKE '%' + @Filtro + '%'
        )
        AND (@SoloActivos = 0 OR c.Activo = 1)
        AND (
            @SoloConSaldo = 0
            OR EXISTS (
                SELECT 1
                FROM dbo.CuentaPorCobrar cxc
                WHERE cxc.IdentificacionCliente = c.Identificacion
                  AND cxc.Activo = 1
                  AND cxc.SaldoActual > 0
            )
        )
    ORDER BY
        ClienteNombre ASC;
END;
GO

/* =========================================================
   CONSULTA GENERAL - PROVEEDORES
   Búsqueda operativa de proveedores y saldo pendiente.
   ========================================================= */
CREATE OR ALTER PROCEDURE Consultas.sp_Consulta_Proveedores
    @Filtro VARCHAR(100),
    @SoloConSaldo BIT = 0,
    @SoloActivos BIT = 1
AS
BEGIN
    SET NOCOUNT ON;

    SET @Filtro = LTRIM(RTRIM(ISNULL(@Filtro, '')));

    SELECT
        p.IdentificacionProveedor,
        p.RazonSocial,
        p.NombreContacto,
        p.DiasCredito,
        p.Activo,

        ISNULL((
            SELECT SUM(cxp.SaldoActual)
            FROM dbo.CuentaPorPagar cxp
            WHERE cxp.IdentificacionProveedor = p.IdentificacionProveedor
              AND cxp.Activo = 1
              AND cxp.SaldoActual > 0
        ), 0) AS SaldoPendiente,

        ISNULL((
            SELECT COUNT(1)
            FROM dbo.CuentaPorPagar cxp
            WHERE cxp.IdentificacionProveedor = p.IdentificacionProveedor
              AND cxp.Activo = 1
              AND cxp.SaldoActual > 0
        ), 0) AS CantidadDocumentosPendientes
    FROM dbo.Proveedor p
    WHERE
        (
            @Filtro = ''
            OR p.IdentificacionProveedor LIKE '%' + @Filtro + '%'
            OR p.RazonSocial LIKE '%' + @Filtro + '%'
            OR p.NombreContacto LIKE '%' + @Filtro + '%'
        )
        AND (@SoloActivos = 0 OR p.Activo = 1)
        AND (
            @SoloConSaldo = 0
            OR EXISTS (
                SELECT 1
                FROM dbo.CuentaPorPagar cxp
                WHERE cxp.IdentificacionProveedor = p.IdentificacionProveedor
                  AND cxp.Activo = 1
                  AND cxp.SaldoActual > 0
            )
        )
    ORDER BY
        p.RazonSocial ASC;
END;
GO

/* =========================================================
   CONSULTA GENERAL - MOVIMIENTOS CONTABLES
   Une ingresos y gastos por cuenta contable.
   ========================================================= */
CREATE OR ALTER PROCEDURE Consultas.sp_Consulta_MovimientosContables
    @Filtro VARCHAR(100),
    @FechaInicio DATETIME = NULL,
    @FechaFin DATETIME = NULL,
    @TipoMovimiento VARCHAR(45) = ''
AS
BEGIN
    SET NOCOUNT ON;

    SET @Filtro = LTRIM(RTRIM(ISNULL(@Filtro, '')));
    SET @TipoMovimiento = LTRIM(RTRIM(ISNULL(@TipoMovimiento, '')));

    ;WITH Movimientos AS
    (
        SELECT
            CAST('Ingreso' AS VARCHAR(45)) AS TipoMovimiento,
            i.NumeroIngreso AS NumeroMovimiento,
            i.FechaIngreso AS FechaMovimiento,
            i.CodigoCuenta,
            cc.NombreCuenta,
            i.Descripcion,
            i.Monto,
            i.OrigenIngreso AS Origen,
            i.NumeroFactura AS Referencia,
            i.Activo
        FROM dbo.Ingreso i
        INNER JOIN dbo.CuentaContable cc
            ON cc.CodigoCuenta = i.CodigoCuenta
        WHERE i.Activo = 1

        UNION ALL

        SELECT
            CAST('Gasto' AS VARCHAR(45)) AS TipoMovimiento,
            g.NumeroGasto AS NumeroMovimiento,
            g.FechaGasto AS FechaMovimiento,
            g.CodigoCuenta,
            cc.NombreCuenta,
            g.Descripcion,
            g.Monto,
            tg.Nombre AS Origen,
            g.NumeroDocumento AS Referencia,
            g.Activo
        FROM dbo.Gasto g
        INNER JOIN dbo.CuentaContable cc
            ON cc.CodigoCuenta = g.CodigoCuenta
        INNER JOIN dbo.TipoGasto tg
            ON tg.IdTipoGasto = g.IdTipoGasto
        WHERE g.Activo = 1
    )
    SELECT *
    FROM Movimientos
    WHERE
        (
            @Filtro = ''
            OR NumeroMovimiento LIKE '%' + @Filtro + '%'
            OR CodigoCuenta LIKE '%' + @Filtro + '%'
            OR NombreCuenta LIKE '%' + @Filtro + '%'
            OR Descripcion LIKE '%' + @Filtro + '%'
            OR Origen LIKE '%' + @Filtro + '%'
            OR Referencia LIKE '%' + @Filtro + '%'
        )
        AND (@TipoMovimiento = '' OR TipoMovimiento = @TipoMovimiento)
        AND (@FechaInicio IS NULL OR CAST(FechaMovimiento AS DATE) >= CAST(@FechaInicio AS DATE))
        AND (@FechaFin IS NULL OR CAST(FechaMovimiento AS DATE) <= CAST(@FechaFin AS DATE))
    ORDER BY
        FechaMovimiento DESC,
        NumeroMovimiento DESC;
END;
GO