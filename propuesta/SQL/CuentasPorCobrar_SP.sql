IF NOT EXISTS (SELECT 1 FROM sys.schemas WHERE name = 'Cuentas')
BEGIN
    EXEC('CREATE SCHEMA Cuentas');
END;
GO

/* =========================================================
   CUENTA POR COBRAR - LISTAR
   ========================================================= */
CREATE OR ALTER PROCEDURE Cuentas.sp_CuentaPorCobrar_Listar
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        cxc.IdentificacionCliente,
        cxc.NumeroFactura,
        f.FechaFactura,
        cxc.FechaEmision,
        cxc.FechaVencimiento,
        cxc.MontoOriginal,
        cxc.SaldoActual,
        cxc.Estado,
        cxc.FechaCreacion,
        cxc.FechaModificacion,
        cxc.Activo,

        DATEDIFF(DAY, CAST(GETDATE() AS DATE), CAST(cxc.FechaVencimiento AS DATE)) AS DiasRestantes,

        CASE
            WHEN cxc.SaldoActual <= 0 THEN 'Pagada'
            WHEN cxc.Activo = 0 THEN 'Inactiva'
            WHEN cxc.Estado = 'Anulada' THEN 'Anulada'
            WHEN CAST(cxc.FechaVencimiento AS DATE) < CAST(GETDATE() AS DATE) THEN 'Vencida'
            WHEN DATEDIFF(DAY, CAST(GETDATE() AS DATE), CAST(cxc.FechaVencimiento AS DATE)) <= 3 THEN 'Por vencer'
            ELSE 'Pendiente'
        END AS EstadoCredito,

        LTRIM(RTRIM(
            ISNULL(p.Nombre, '') + ' ' +
            ISNULL(p.PrimerApellido, '') + ' ' +
            ISNULL(p.SegundoApellido, '')
        )) AS ClienteNombre,

        f.TotalFactura,
        f.Estado AS EstadoFactura,
        f.Activo AS FacturaActiva
    FROM dbo.CuentaPorCobrar cxc
    INNER JOIN dbo.Factura f
        ON f.IdentificacionCliente = cxc.IdentificacionCliente
       AND f.NumeroFactura = cxc.NumeroFactura
    INNER JOIN dbo.Cliente c
        ON c.Identificacion = cxc.IdentificacionCliente
    INNER JOIN dbo.Persona p
        ON p.Identificacion = c.Identificacion
    ORDER BY
        cxc.FechaEmision DESC,
        cxc.NumeroFactura DESC;
END;
GO

/* =========================================================
   CUENTA POR COBRAR - LISTAR PENDIENTES
   ========================================================= */
CREATE OR ALTER PROCEDURE Cuentas.sp_CuentaPorCobrar_ListarPendientes
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        cxc.IdentificacionCliente,
        cxc.NumeroFactura,
        f.FechaFactura,
        cxc.FechaEmision,
        cxc.FechaVencimiento,
        cxc.MontoOriginal,
        cxc.SaldoActual,
        cxc.Estado,
        cxc.FechaCreacion,
        cxc.FechaModificacion,
        cxc.Activo,

        DATEDIFF(DAY, CAST(GETDATE() AS DATE), CAST(cxc.FechaVencimiento AS DATE)) AS DiasRestantes,

        CASE
            WHEN cxc.SaldoActual <= 0 THEN 'Pagada'
            WHEN cxc.Activo = 0 THEN 'Inactiva'
            WHEN cxc.Estado = 'Anulada' THEN 'Anulada'
            WHEN CAST(cxc.FechaVencimiento AS DATE) < CAST(GETDATE() AS DATE) THEN 'Vencida'
            WHEN DATEDIFF(DAY, CAST(GETDATE() AS DATE), CAST(cxc.FechaVencimiento AS DATE)) <= 3 THEN 'Por vencer'
            ELSE 'Pendiente'
        END AS EstadoCredito,

        LTRIM(RTRIM(
            ISNULL(p.Nombre, '') + ' ' +
            ISNULL(p.PrimerApellido, '') + ' ' +
            ISNULL(p.SegundoApellido, '')
        )) AS ClienteNombre,

        f.TotalFactura,
        f.Estado AS EstadoFactura,
        f.Activo AS FacturaActiva
    FROM dbo.CuentaPorCobrar cxc
    INNER JOIN dbo.Factura f
        ON f.IdentificacionCliente = cxc.IdentificacionCliente
       AND f.NumeroFactura = cxc.NumeroFactura
    INNER JOIN dbo.Cliente c
        ON c.Identificacion = cxc.IdentificacionCliente
    INNER JOIN dbo.Persona p
        ON p.Identificacion = c.Identificacion
    WHERE cxc.Activo = 1
      AND cxc.SaldoActual > 0
      AND cxc.Estado IN ('Pendiente', 'Parcial')
    ORDER BY
        cxc.FechaVencimiento ASC,
        cxc.NumeroFactura DESC;
END;
GO

/* =========================================================
   CUENTA POR COBRAR - LISTAR POR CLIENTE
   ========================================================= */
CREATE OR ALTER PROCEDURE Cuentas.sp_CuentaPorCobrar_ListarPorCliente
    @IdentificacionCliente VARCHAR(45)
AS
BEGIN
    SET NOCOUNT ON;

    SET @IdentificacionCliente = LTRIM(RTRIM(ISNULL(@IdentificacionCliente, '')));

    SELECT
        cxc.IdentificacionCliente,
        cxc.NumeroFactura,
        f.FechaFactura,
        cxc.FechaEmision,
        cxc.FechaVencimiento,
        cxc.MontoOriginal,
        cxc.SaldoActual,
        cxc.Estado,
        cxc.FechaCreacion,
        cxc.FechaModificacion,
        cxc.Activo,

        DATEDIFF(DAY, CAST(GETDATE() AS DATE), CAST(cxc.FechaVencimiento AS DATE)) AS DiasRestantes,

        CASE
            WHEN cxc.SaldoActual <= 0 THEN 'Pagada'
            WHEN cxc.Activo = 0 THEN 'Inactiva'
            WHEN cxc.Estado = 'Anulada' THEN 'Anulada'
            WHEN CAST(cxc.FechaVencimiento AS DATE) < CAST(GETDATE() AS DATE) THEN 'Vencida'
            WHEN DATEDIFF(DAY, CAST(GETDATE() AS DATE), CAST(cxc.FechaVencimiento AS DATE)) <= 3 THEN 'Por vencer'
            ELSE 'Pendiente'
        END AS EstadoCredito,

        LTRIM(RTRIM(
            ISNULL(p.Nombre, '') + ' ' +
            ISNULL(p.PrimerApellido, '') + ' ' +
            ISNULL(p.SegundoApellido, '')
        )) AS ClienteNombre,

        f.TotalFactura,
        f.Estado AS EstadoFactura,
        f.Activo AS FacturaActiva
    FROM dbo.CuentaPorCobrar cxc
    INNER JOIN dbo.Factura f
        ON f.IdentificacionCliente = cxc.IdentificacionCliente
       AND f.NumeroFactura = cxc.NumeroFactura
    INNER JOIN dbo.Cliente c
        ON c.Identificacion = cxc.IdentificacionCliente
    INNER JOIN dbo.Persona p
        ON p.Identificacion = c.Identificacion
    WHERE cxc.IdentificacionCliente = @IdentificacionCliente
    ORDER BY
        cxc.FechaEmision DESC,
        cxc.NumeroFactura DESC;
END;
GO

/* =========================================================
   CUENTA POR COBRAR - OBTENER
   ========================================================= */
CREATE OR ALTER PROCEDURE Cuentas.sp_CuentaPorCobrar_Obtener
    @IdentificacionCliente VARCHAR(45),
    @NumeroFactura VARCHAR(45)
AS
BEGIN
    SET NOCOUNT ON;

    SET @IdentificacionCliente = LTRIM(RTRIM(ISNULL(@IdentificacionCliente, '')));
    SET @NumeroFactura = LTRIM(RTRIM(ISNULL(@NumeroFactura, '')));

    SELECT
        cxc.IdentificacionCliente,
        cxc.NumeroFactura,
        f.FechaFactura,
        cxc.FechaEmision,
        cxc.FechaVencimiento,
        cxc.MontoOriginal,
        cxc.SaldoActual,
        cxc.Estado,
        cxc.FechaCreacion,
        cxc.FechaModificacion,
        cxc.Activo,

        DATEDIFF(DAY, CAST(GETDATE() AS DATE), CAST(cxc.FechaVencimiento AS DATE)) AS DiasRestantes,

        CASE
            WHEN cxc.SaldoActual <= 0 THEN 'Pagada'
            WHEN cxc.Activo = 0 THEN 'Inactiva'
            WHEN cxc.Estado = 'Anulada' THEN 'Anulada'
            WHEN CAST(cxc.FechaVencimiento AS DATE) < CAST(GETDATE() AS DATE) THEN 'Vencida'
            WHEN DATEDIFF(DAY, CAST(GETDATE() AS DATE), CAST(cxc.FechaVencimiento AS DATE)) <= 3 THEN 'Por vencer'
            ELSE 'Pendiente'
        END AS EstadoCredito,

        LTRIM(RTRIM(
            ISNULL(p.Nombre, '') + ' ' +
            ISNULL(p.PrimerApellido, '') + ' ' +
            ISNULL(p.SegundoApellido, '')
        )) AS ClienteNombre,

        f.TotalFactura,
        f.Estado AS EstadoFactura,
        f.Activo AS FacturaActiva
    FROM dbo.CuentaPorCobrar cxc
    INNER JOIN dbo.Factura f
        ON f.IdentificacionCliente = cxc.IdentificacionCliente
       AND f.NumeroFactura = cxc.NumeroFactura
    INNER JOIN dbo.Cliente c
        ON c.Identificacion = cxc.IdentificacionCliente
    INNER JOIN dbo.Persona p
        ON p.Identificacion = c.Identificacion
    WHERE cxc.IdentificacionCliente = @IdentificacionCliente
      AND cxc.NumeroFactura = @NumeroFactura;
END;
GO

/* =========================================================
   CUENTA POR COBRAR - REGISTRAR
   ========================================================= */
CREATE OR ALTER PROCEDURE Cuentas.sp_CuentaPorCobrar_Registrar
    @IdentificacionCliente VARCHAR(45),
    @NumeroFactura VARCHAR(45),
    @FechaEmision DATETIME,
    @FechaVencimiento DATETIME,
    @MontoOriginal DECIMAL(18,2),
    @Estado VARCHAR(45),
    @Resultado INT OUTPUT,
    @Mensaje VARCHAR(500) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @TotalFactura DECIMAL(18,2);

    SET @Resultado = 0;
    SET @Mensaje = '';

    SET @IdentificacionCliente = LTRIM(RTRIM(ISNULL(@IdentificacionCliente, '')));
    SET @NumeroFactura = LTRIM(RTRIM(ISNULL(@NumeroFactura, '')));
    SET @Estado = LTRIM(RTRIM(ISNULL(@Estado, '')));

    IF (@IdentificacionCliente = '')
    BEGIN
        SET @Mensaje = 'Debe seleccionar un cliente.';
        RETURN;
    END;

    IF (@NumeroFactura = '')
    BEGIN
        SET @Mensaje = 'Debe seleccionar una factura.';
        RETURN;
    END;

    IF NOT EXISTS (
        SELECT 1
        FROM dbo.Cliente
        WHERE Identificacion = @IdentificacionCliente
          AND Activo = 1
    )
    BEGIN
        SET @Mensaje = 'El cliente seleccionado no existe o está inactivo.';
        RETURN;
    END;

    SELECT @TotalFactura = TotalFactura
    FROM dbo.Factura
    WHERE IdentificacionCliente = @IdentificacionCliente
      AND NumeroFactura = @NumeroFactura
      AND Activo = 1
      AND Estado NOT IN ('Anulada', 'Cancelada');

    IF (@TotalFactura IS NULL)
    BEGIN
        SET @Mensaje = 'La factura no existe, no pertenece al cliente, está inactiva o está anulada.';
        RETURN;
    END;

    IF EXISTS (
        SELECT 1
        FROM dbo.CuentaPorCobrar
        WHERE IdentificacionCliente = @IdentificacionCliente
          AND NumeroFactura = @NumeroFactura
    )
    BEGIN
        SET @Mensaje = 'Ya existe una cuenta por cobrar para esta factura.';
        RETURN;
    END;

    IF (@FechaEmision IS NULL)
    BEGIN
        SET @Mensaje = 'Debe ingresar la fecha de emisión.';
        RETURN;
    END;

    IF (@FechaVencimiento IS NULL)
    BEGIN
        SET @Mensaje = 'Debe ingresar la fecha de vencimiento.';
        RETURN;
    END;

    IF (@FechaVencimiento < @FechaEmision)
    BEGIN
        SET @Mensaje = 'La fecha de vencimiento no puede ser menor que la fecha de emisión.';
        RETURN;
    END;

    IF (@MontoOriginal <= 0)
    BEGIN
        SET @Mensaje = 'El monto original debe ser mayor a cero.';
        RETURN;
    END;

    IF (@MontoOriginal > @TotalFactura)
    BEGIN
        SET @Mensaje = 'El monto original no puede ser mayor que el total de la factura.';
        RETURN;
    END;

    IF (@Estado = '')
    BEGIN
        SET @Estado = 'Pendiente';
    END;

    IF (LEN(@Estado) > 45)
    BEGIN
        SET @Mensaje = 'El estado no puede superar los 45 caracteres.';
        RETURN;
    END;

    BEGIN TRY
        INSERT INTO dbo.CuentaPorCobrar
        (
            IdentificacionCliente,
            NumeroFactura,
            FechaEmision,
            FechaVencimiento,
            MontoOriginal,
            SaldoActual,
            Estado,
            FechaCreacion,
            FechaModificacion,
            Activo
        )
        VALUES
        (
            @IdentificacionCliente,
            @NumeroFactura,
            @FechaEmision,
            @FechaVencimiento,
            @MontoOriginal,
            @MontoOriginal,
            @Estado,
            GETDATE(),
            GETDATE(),
            1
        );

        SET @Resultado = 1;
        SET @Mensaje = 'Cuenta por cobrar registrada correctamente.';
    END TRY
    BEGIN CATCH
        SET @Resultado = 0;
        SET @Mensaje = ERROR_MESSAGE();
    END CATCH;
END;
GO

/* =========================================================
   CUENTA POR COBRAR - EDITAR
   ========================================================= */
CREATE OR ALTER PROCEDURE Cuentas.sp_CuentaPorCobrar_Editar
    @IdentificacionCliente VARCHAR(45),
    @NumeroFactura VARCHAR(45),
    @FechaEmision DATETIME,
    @FechaVencimiento DATETIME,
    @MontoOriginal DECIMAL(18,2),
    @SaldoActual DECIMAL(18,2),
    @Estado VARCHAR(45),
    @Activo BIT,
    @Resultado BIT OUTPUT,
    @Mensaje VARCHAR(500) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @TotalAbonos DECIMAL(18,2);

    SET @Resultado = 0;
    SET @Mensaje = '';

    SET @IdentificacionCliente = LTRIM(RTRIM(ISNULL(@IdentificacionCliente, '')));
    SET @NumeroFactura = LTRIM(RTRIM(ISNULL(@NumeroFactura, '')));
    SET @Estado = LTRIM(RTRIM(ISNULL(@Estado, '')));

    IF (@IdentificacionCliente = '' OR @NumeroFactura = '')
    BEGIN
        SET @Mensaje = 'Debe seleccionar una cuenta por cobrar válida.';
        RETURN;
    END;

    IF NOT EXISTS (
        SELECT 1
        FROM dbo.CuentaPorCobrar
        WHERE IdentificacionCliente = @IdentificacionCliente
          AND NumeroFactura = @NumeroFactura
    )
    BEGIN
        SET @Mensaje = 'La cuenta por cobrar no existe.';
        RETURN;
    END;

    IF (@FechaEmision IS NULL)
    BEGIN
        SET @Mensaje = 'Debe ingresar la fecha de emisión.';
        RETURN;
    END;

    IF (@FechaVencimiento IS NULL)
    BEGIN
        SET @Mensaje = 'Debe ingresar la fecha de vencimiento.';
        RETURN;
    END;

    IF (@FechaVencimiento < @FechaEmision)
    BEGIN
        SET @Mensaje = 'La fecha de vencimiento no puede ser menor que la fecha de emisión.';
        RETURN;
    END;

    IF (@MontoOriginal < 0 OR @SaldoActual < 0)
    BEGIN
        SET @Mensaje = 'Los montos no pueden ser negativos.';
        RETURN;
    END;

    SELECT @TotalAbonos = ISNULL(SUM(MontoAbono), 0)
    FROM dbo.AbonoCuentaPorCobrar
    WHERE IdentificacionCliente = @IdentificacionCliente
      AND NumeroFactura = @NumeroFactura
      AND Activo = 1;

    IF (@SaldoActual <> (@MontoOriginal - @TotalAbonos))
    BEGIN
        SET @Mensaje = 'El saldo actual no coincide con el monto original menos los abonos activos.';
        RETURN;
    END;

    IF (@Estado = '')
    BEGIN
        SET @Mensaje = 'Debe ingresar el estado.';
        RETURN;
    END;

    IF (LEN(@Estado) > 45)
    BEGIN
        SET @Mensaje = 'El estado no puede superar los 45 caracteres.';
        RETURN;
    END;

    UPDATE dbo.CuentaPorCobrar
    SET
        FechaEmision = @FechaEmision,
        FechaVencimiento = @FechaVencimiento,
        MontoOriginal = @MontoOriginal,
        SaldoActual = @SaldoActual,
        Estado = @Estado,
        FechaModificacion = GETDATE(),
        Activo = @Activo
    WHERE IdentificacionCliente = @IdentificacionCliente
      AND NumeroFactura = @NumeroFactura;

    SET @Resultado = 1;
    SET @Mensaje = 'Cuenta por cobrar actualizada correctamente.';
END;
GO

/* =========================================================
   CUENTA POR COBRAR - RECALCULAR SALDO
   ========================================================= */
CREATE OR ALTER PROCEDURE Cuentas.sp_CuentaPorCobrar_RecalcularSaldo
    @IdentificacionCliente VARCHAR(45),
    @NumeroFactura VARCHAR(45),
    @Resultado BIT OUTPUT,
    @Mensaje VARCHAR(500) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @MontoOriginal DECIMAL(18,2);
    DECLARE @TotalAbonos DECIMAL(18,2);
    DECLARE @SaldoActual DECIMAL(18,2);
    DECLARE @Estado VARCHAR(45);

    SET @Resultado = 0;
    SET @Mensaje = '';

    SET @IdentificacionCliente = LTRIM(RTRIM(ISNULL(@IdentificacionCliente, '')));
    SET @NumeroFactura = LTRIM(RTRIM(ISNULL(@NumeroFactura, '')));

    IF NOT EXISTS (
        SELECT 1
        FROM dbo.CuentaPorCobrar
        WHERE IdentificacionCliente = @IdentificacionCliente
          AND NumeroFactura = @NumeroFactura
    )
    BEGIN
        SET @Mensaje = 'La cuenta por cobrar no existe.';
        RETURN;
    END;

    SELECT @MontoOriginal = MontoOriginal
    FROM dbo.CuentaPorCobrar
    WHERE IdentificacionCliente = @IdentificacionCliente
      AND NumeroFactura = @NumeroFactura;

    SELECT @TotalAbonos = ISNULL(SUM(MontoAbono), 0)
    FROM dbo.AbonoCuentaPorCobrar
    WHERE IdentificacionCliente = @IdentificacionCliente
      AND NumeroFactura = @NumeroFactura
      AND Activo = 1;

    SET @SaldoActual = @MontoOriginal - @TotalAbonos;

    IF (@SaldoActual < 0)
    BEGIN
        SET @SaldoActual = 0;
    END;

    SET @Estado =
        CASE
            WHEN @SaldoActual = 0 THEN 'Pagada'
            WHEN @SaldoActual < @MontoOriginal THEN 'Parcial'
            ELSE 'Pendiente'
        END;

    UPDATE dbo.CuentaPorCobrar
    SET
        SaldoActual = @SaldoActual,
        Estado = @Estado,
        FechaModificacion = GETDATE()
    WHERE IdentificacionCliente = @IdentificacionCliente
      AND NumeroFactura = @NumeroFactura;

    SET @Resultado = 1;
    SET @Mensaje = 'Saldo de cuenta por cobrar recalculado correctamente.';
END;
GO

/* =========================================================
   CUENTA POR COBRAR - INACTIVAR
   ========================================================= */
CREATE OR ALTER PROCEDURE Cuentas.sp_CuentaPorCobrar_Inactivar
    @IdentificacionCliente VARCHAR(45),
    @NumeroFactura VARCHAR(45),
    @Resultado BIT OUTPUT,
    @Mensaje VARCHAR(500) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    SET @Resultado = 0;
    SET @Mensaje = '';

    SET @IdentificacionCliente = LTRIM(RTRIM(ISNULL(@IdentificacionCliente, '')));
    SET @NumeroFactura = LTRIM(RTRIM(ISNULL(@NumeroFactura, '')));

    IF (@IdentificacionCliente = '' OR @NumeroFactura = '')
    BEGIN
        SET @Mensaje = 'Debe seleccionar una cuenta por cobrar válida.';
        RETURN;
    END;

    IF NOT EXISTS (
        SELECT 1
        FROM dbo.CuentaPorCobrar
        WHERE IdentificacionCliente = @IdentificacionCliente
          AND NumeroFactura = @NumeroFactura
    )
    BEGIN
        SET @Mensaje = 'La cuenta por cobrar no existe.';
        RETURN;
    END;

    IF EXISTS (
        SELECT 1
        FROM dbo.AbonoCuentaPorCobrar
        WHERE IdentificacionCliente = @IdentificacionCliente
          AND NumeroFactura = @NumeroFactura
          AND Activo = 1
    )
    BEGIN
        SET @Mensaje = 'No se puede inactivar la cuenta por cobrar porque tiene abonos activos registrados.';
        RETURN;
    END;

    UPDATE dbo.CuentaPorCobrar
    SET
        Activo = 0,
        Estado = 'Anulada',
        FechaModificacion = GETDATE()
    WHERE IdentificacionCliente = @IdentificacionCliente
      AND NumeroFactura = @NumeroFactura;

    SET @Resultado = 1;
    SET @Mensaje = 'Cuenta por cobrar inactivada correctamente.';
END;
GO


/* =========================================================
   CUENTA POR COBRAR - Datos
   ========================================================= */

IF NOT EXISTS (
    SELECT 1
    FROM dbo.ConfiguracionCuentaContable
    WHERE CodigoOperacion = 'CUENTAS_POR_COBRAR'
)
BEGIN
    INSERT INTO dbo.ConfiguracionCuentaContable
    (
        CodigoOperacion,
        NombreOperacion,
        Descripcion,
        CodigoCuenta,
        FechaCreacion,
        FechaModificacion,
        Activo
    )
    VALUES
    (
        'CUENTAS_POR_COBRAR',
        'Cuentas por cobrar',
        'Cuenta contable utilizada para registrar saldos y abonos de clientes pendientes de cobro.',
        '1.03',
        GETDATE(),
        GETDATE(),
        1
    );
END;
GO