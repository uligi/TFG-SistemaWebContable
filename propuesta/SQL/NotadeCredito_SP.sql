IF NOT EXISTS (SELECT 1 FROM sys.schemas WHERE name = 'Facturacion')
BEGIN
    EXEC('CREATE SCHEMA Facturacion');
END;
GO

/* =========================================================
   NOTA CRÉDITO - LISTAR
   ========================================================= */
CREATE OR ALTER PROCEDURE Facturacion.sp_NotaCredito_Listar
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        nc.NumeroNotaCredito,
        nc.NumeroFactura,

        f.IdentificacionCliente,
        LTRIM(RTRIM(pc.Nombre + ' ' + pc.PrimerApellido + ' ' + pc.SegundoApellido)) AS ClienteNombre,

        nc.IdentificacionEmpleado,
        LTRIM(RTRIM(pe.Nombre + ' ' + pe.PrimerApellido + ' ' + pe.SegundoApellido)) AS EmpleadoNombre,

        nc.FechaNotaCredito,
        nc.Motivo,
        nc.Subtotal,
        nc.TotalImpuesto,
        nc.TotalNotaCredito,
        nc.Estado,
        nc.FechaCreacion,
        nc.FechaModificacion,
        nc.Activo,

        f.TotalFactura,
        f.Estado AS EstadoFactura,
        f.Activo AS FacturaActiva
    FROM dbo.NotaCredito nc
    INNER JOIN dbo.Factura f
        ON f.NumeroFactura = nc.NumeroFactura
    INNER JOIN dbo.Cliente c
        ON c.Identificacion = f.IdentificacionCliente
    INNER JOIN dbo.Persona pc
        ON pc.Identificacion = c.Identificacion
    INNER JOIN dbo.Empleado e
        ON e.Identificacion = nc.IdentificacionEmpleado
    INNER JOIN dbo.Persona pe
        ON pe.Identificacion = e.Identificacion
    ORDER BY
        nc.FechaNotaCredito DESC,
        nc.NumeroNotaCredito DESC;
END;
GO

/* =========================================================
   NOTA CRÉDITO - LISTAR ACTIVAS
   ========================================================= */
CREATE OR ALTER PROCEDURE Facturacion.sp_NotaCredito_ListarActivas
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        nc.NumeroNotaCredito,
        nc.NumeroFactura,

        f.IdentificacionCliente,
        LTRIM(RTRIM(pc.Nombre + ' ' + pc.PrimerApellido + ' ' + pc.SegundoApellido)) AS ClienteNombre,

        nc.IdentificacionEmpleado,
        LTRIM(RTRIM(pe.Nombre + ' ' + pe.PrimerApellido + ' ' + pe.SegundoApellido)) AS EmpleadoNombre,

        nc.FechaNotaCredito,
        nc.Motivo,
        nc.Subtotal,
        nc.TotalImpuesto,
        nc.TotalNotaCredito,
        nc.Estado,
        nc.FechaCreacion,
        nc.FechaModificacion,
        nc.Activo,

        f.TotalFactura,
        f.Estado AS EstadoFactura,
        f.Activo AS FacturaActiva
    FROM dbo.NotaCredito nc
    INNER JOIN dbo.Factura f
        ON f.NumeroFactura = nc.NumeroFactura
    INNER JOIN dbo.Cliente c
        ON c.Identificacion = f.IdentificacionCliente
    INNER JOIN dbo.Persona pc
        ON pc.Identificacion = c.Identificacion
    INNER JOIN dbo.Empleado e
        ON e.Identificacion = nc.IdentificacionEmpleado
    INNER JOIN dbo.Persona pe
        ON pe.Identificacion = e.Identificacion
    WHERE nc.Activo = 1
    ORDER BY
        nc.FechaNotaCredito DESC,
        nc.NumeroNotaCredito DESC;
END;
GO

/* =========================================================
   NOTA CRÉDITO - LISTAR POR FACTURA
   ========================================================= */
CREATE OR ALTER PROCEDURE Facturacion.sp_NotaCredito_ListarPorFactura
    @NumeroFactura VARCHAR(45)
AS
BEGIN
    SET NOCOUNT ON;

    SET @NumeroFactura = LTRIM(RTRIM(ISNULL(@NumeroFactura, '')));

    SELECT
        nc.NumeroNotaCredito,
        nc.NumeroFactura,

        f.IdentificacionCliente,
        LTRIM(RTRIM(pc.Nombre + ' ' + pc.PrimerApellido + ' ' + pc.SegundoApellido)) AS ClienteNombre,

        nc.IdentificacionEmpleado,
        LTRIM(RTRIM(pe.Nombre + ' ' + pe.PrimerApellido + ' ' + pe.SegundoApellido)) AS EmpleadoNombre,

        nc.FechaNotaCredito,
        nc.Motivo,
        nc.Subtotal,
        nc.TotalImpuesto,
        nc.TotalNotaCredito,
        nc.Estado,
        nc.FechaCreacion,
        nc.FechaModificacion,
        nc.Activo,

        f.TotalFactura,
        f.Estado AS EstadoFactura,
        f.Activo AS FacturaActiva
    FROM dbo.NotaCredito nc
    INNER JOIN dbo.Factura f
        ON f.NumeroFactura = nc.NumeroFactura
    INNER JOIN dbo.Cliente c
        ON c.Identificacion = f.IdentificacionCliente
    INNER JOIN dbo.Persona pc
        ON pc.Identificacion = c.Identificacion
    INNER JOIN dbo.Empleado e
        ON e.Identificacion = nc.IdentificacionEmpleado
    INNER JOIN dbo.Persona pe
        ON pe.Identificacion = e.Identificacion
    WHERE nc.NumeroFactura = @NumeroFactura
    ORDER BY
        nc.FechaNotaCredito DESC,
        nc.NumeroNotaCredito DESC;
END;
GO

/* =========================================================
   NOTA CRÉDITO - OBTENER
   ========================================================= */
CREATE OR ALTER PROCEDURE Facturacion.sp_NotaCredito_Obtener
    @NumeroNotaCredito VARCHAR(45)
AS
BEGIN
    SET NOCOUNT ON;

    SET @NumeroNotaCredito = LTRIM(RTRIM(ISNULL(@NumeroNotaCredito, '')));

    SELECT
        nc.NumeroNotaCredito,
        nc.NumeroFactura,

        f.IdentificacionCliente,
        LTRIM(RTRIM(pc.Nombre + ' ' + pc.PrimerApellido + ' ' + pc.SegundoApellido)) AS ClienteNombre,

        nc.IdentificacionEmpleado,
        LTRIM(RTRIM(pe.Nombre + ' ' + pe.PrimerApellido + ' ' + pe.SegundoApellido)) AS EmpleadoNombre,

        nc.FechaNotaCredito,
        nc.Motivo,
        nc.Subtotal,
        nc.TotalImpuesto,
        nc.TotalNotaCredito,
        nc.Estado,
        nc.FechaCreacion,
        nc.FechaModificacion,
        nc.Activo,

        f.TotalFactura,
        f.Estado AS EstadoFactura,
        f.Activo AS FacturaActiva
    FROM dbo.NotaCredito nc
    INNER JOIN dbo.Factura f
        ON f.NumeroFactura = nc.NumeroFactura
    INNER JOIN dbo.Cliente c
        ON c.Identificacion = f.IdentificacionCliente
    INNER JOIN dbo.Persona pc
        ON pc.Identificacion = c.Identificacion
    INNER JOIN dbo.Empleado e
        ON e.Identificacion = nc.IdentificacionEmpleado
    INNER JOIN dbo.Persona pe
        ON pe.Identificacion = e.Identificacion
    WHERE nc.NumeroNotaCredito = @NumeroNotaCredito;
END;
GO

/* =========================================================
   NOTA CRÉDITO - GENERAR NÚMERO
   Formato: NC-2026-000001
   ========================================================= */
CREATE OR ALTER PROCEDURE Facturacion.sp_NotaCredito_GenerarNumero
    @Anio INT,
    @NumeroNotaCredito VARCHAR(45) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @Consecutivo INT;

    SET @NumeroNotaCredito = '';

    IF (@Anio < 2000 OR @Anio > 2100)
    BEGIN
        SET @Anio = YEAR(GETDATE());
    END;

    SELECT @Consecutivo =
        ISNULL(MAX(TRY_CAST(RIGHT(NumeroNotaCredito, 6) AS INT)), 0) + 1
    FROM dbo.NotaCredito
    WHERE NumeroNotaCredito LIKE 'NC-' + CAST(@Anio AS VARCHAR(4)) + '-%';

    SET @NumeroNotaCredito =
        'NC-' + CAST(@Anio AS VARCHAR(4)) + '-' + RIGHT('000000' + CAST(@Consecutivo AS VARCHAR(6)), 6);
END;
GO

/* =========================================================
   NOTA CRÉDITO - REGISTRAR
   ========================================================= */
CREATE OR ALTER PROCEDURE Facturacion.sp_NotaCredito_Registrar
    @NumeroFactura VARCHAR(45),
    @IdentificacionEmpleado VARCHAR(45),
    @FechaNotaCredito DATETIME,
    @Motivo VARCHAR(250),
    @Subtotal DECIMAL(18,2),
    @TotalImpuesto DECIMAL(18,2),
    @TotalNotaCredito DECIMAL(18,2),
    @Estado VARCHAR(45),
    @NumeroNotaCreditoGenerado VARCHAR(45) OUTPUT,
    @Resultado INT OUTPUT,
    @Mensaje VARCHAR(500) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @NumeroNotaCredito VARCHAR(45);
    DECLARE @Anio INT;

    DECLARE @IdentificacionCliente VARCHAR(45);
    DECLARE @TotalFactura DECIMAL(18,2);
    DECLARE @SaldoActual DECIMAL(18,2);
    DECLARE @NuevoSaldo DECIMAL(18,2);
    DECLARE @NuevoEstadoCuenta VARCHAR(45);

    SET @Resultado = 0;
    SET @Mensaje = '';
    SET @NumeroNotaCreditoGenerado = '';

    SET @NumeroFactura = LTRIM(RTRIM(ISNULL(@NumeroFactura, '')));
    SET @IdentificacionEmpleado = LTRIM(RTRIM(ISNULL(@IdentificacionEmpleado, '')));
    SET @Motivo = LTRIM(RTRIM(ISNULL(@Motivo, '')));
    SET @Estado = LTRIM(RTRIM(ISNULL(@Estado, '')));

    IF (@NumeroFactura = '')
    BEGIN
        SET @Mensaje = 'Debe seleccionar una factura.';
        RETURN;
    END;

   SELECT
    @IdentificacionCliente = IdentificacionCliente,
    @TotalFactura = TotalFactura
FROM dbo.Factura
WHERE NumeroFactura = @NumeroFactura
  AND Activo = 1
  AND Estado = 'Emitida'
  AND TotalFactura > 0;

    IF (@IdentificacionCliente IS NULL)
    BEGIN
        SET @Mensaje = 'La nota de crédito solo puede aplicarse a facturas emitidas y activas.';RETURN;
    END;

    IF (@IdentificacionEmpleado = '')
    BEGIN
        SET @Mensaje = 'Debe seleccionar un empleado.';
        RETURN;
    END;

    IF NOT EXISTS (
        SELECT 1
        FROM dbo.Empleado
        WHERE Identificacion = @IdentificacionEmpleado
          AND Activo = 1
    )
    BEGIN
        SET @Mensaje = 'El empleado seleccionado no existe o está inactivo.';
        RETURN;
    END;

    IF (@FechaNotaCredito IS NULL)
    BEGIN
        SET @Mensaje = 'Debe ingresar la fecha de la nota de crédito.';
        RETURN;
    END;

    IF (@Motivo = '')
    BEGIN
        SET @Mensaje = 'Debe ingresar el motivo de la nota de crédito.';
        RETURN;
    END;

    IF (LEN(@Motivo) > 250)
    BEGIN
        SET @Mensaje = 'El motivo no puede superar los 250 caracteres.';
        RETURN;
    END;

    IF (@Subtotal < 0 OR @TotalImpuesto < 0 OR @TotalNotaCredito < 0)
    BEGIN
        SET @Mensaje = 'Los montos de la nota de crédito no pueden ser negativos.';
        RETURN;
    END;

    IF (@TotalNotaCredito <= 0)
    BEGIN
        SET @Mensaje = 'El total de la nota de crédito debe ser mayor a cero.';
        RETURN;
    END;

    IF (@TotalNotaCredito > @TotalFactura)
    BEGIN
        SET @Mensaje = 'El total de la nota de crédito no puede superar el total de la factura.';
        RETURN;
    END;

    IF (@Estado = '')
    BEGIN
        SET @Estado = 'Aplicada';
    END;

    IF (LEN(@Estado) > 45)
    BEGIN
        SET @Mensaje = 'El estado no puede superar los 45 caracteres.';
        RETURN;
    END;

    BEGIN TRY
        BEGIN TRANSACTION;

        SET @Anio = YEAR(@FechaNotaCredito);

        EXEC Facturacion.sp_NotaCredito_GenerarNumero
            @Anio = @Anio,
            @NumeroNotaCredito = @NumeroNotaCredito OUTPUT;

        INSERT INTO dbo.NotaCredito
        (
            NumeroNotaCredito,
            NumeroFactura,
            IdentificacionEmpleado,
            FechaNotaCredito,
            Motivo,
            Subtotal,
            TotalImpuesto,
            TotalNotaCredito,
            Estado,
            FechaCreacion,
            FechaModificacion,
            Activo
        )
        VALUES
        (
            @NumeroNotaCredito,
            @NumeroFactura,
            @IdentificacionEmpleado,
            @FechaNotaCredito,
            @Motivo,
            @Subtotal,
            @TotalImpuesto,
            @TotalNotaCredito,
            @Estado,
            GETDATE(),
            GETDATE(),
            1
        );

        /* Si existe cuenta por cobrar activa, se reduce el saldo */
        IF EXISTS (
            SELECT 1
            FROM dbo.CuentaPorCobrar
            WHERE IdentificacionCliente = @IdentificacionCliente
              AND NumeroFactura = @NumeroFactura
              AND Activo = 1
              AND Estado NOT IN ('Pagada', 'Anulada')
        )
        BEGIN
            SELECT @SaldoActual = SaldoActual
            FROM dbo.CuentaPorCobrar
            WHERE IdentificacionCliente = @IdentificacionCliente
              AND NumeroFactura = @NumeroFactura;

            SET @NuevoSaldo = @SaldoActual - @TotalNotaCredito;

            IF (@NuevoSaldo < 0)
            BEGIN
                SET @NuevoSaldo = 0;
            END;

            SET @NuevoEstadoCuenta =
                CASE
                    WHEN @NuevoSaldo = 0 THEN 'Pagada'
                    WHEN @NuevoSaldo < @SaldoActual THEN 'Parcial'
                    ELSE 'Pendiente'
                END;

            UPDATE dbo.CuentaPorCobrar
            SET
                SaldoActual = @NuevoSaldo,
                Estado = @NuevoEstadoCuenta,
                FechaModificacion = GETDATE()
            WHERE IdentificacionCliente = @IdentificacionCliente
              AND NumeroFactura = @NumeroFactura;
        END;

        COMMIT TRANSACTION;

        SET @Resultado = 1;
        SET @NumeroNotaCreditoGenerado = @NumeroNotaCredito;
        SET @Mensaje = 'Nota de crédito registrada correctamente.';
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;

        SET @Resultado = 0;
        SET @NumeroNotaCreditoGenerado = '';
        SET @Mensaje = ERROR_MESSAGE();
    END CATCH;
END;
GO




/* =========================================================
   NOTA CRÉDITO - EDITAR
   Nota:
   Solo permite editar datos generales si la nota sigue activa.
   No recalcula saldo porque ya fue aplicada.
   ========================================================= */
CREATE OR ALTER PROCEDURE Facturacion.sp_NotaCredito_Editar
    @NumeroNotaCredito VARCHAR(45),
    @IdentificacionEmpleado VARCHAR(45),
    @FechaNotaCredito DATETIME,
    @Motivo VARCHAR(250),
    @Estado VARCHAR(45),
    @Activo BIT,
    @Resultado BIT OUTPUT,
    @Mensaje VARCHAR(500) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    SET @Resultado = 0;
    SET @Mensaje = '';

    SET @NumeroNotaCredito = LTRIM(RTRIM(ISNULL(@NumeroNotaCredito, '')));
    SET @IdentificacionEmpleado = LTRIM(RTRIM(ISNULL(@IdentificacionEmpleado, '')));
    SET @Motivo = LTRIM(RTRIM(ISNULL(@Motivo, '')));
    SET @Estado = LTRIM(RTRIM(ISNULL(@Estado, '')));

    IF (@NumeroNotaCredito = '')
    BEGIN
        SET @Mensaje = 'Debe seleccionar una nota de crédito válida.';
        RETURN;
    END;

    IF NOT EXISTS (
        SELECT 1
        FROM dbo.NotaCredito
        WHERE NumeroNotaCredito = @NumeroNotaCredito
          AND Activo = 1
    )
    BEGIN
        SET @Mensaje = 'La nota de crédito no existe o está inactiva.';
        RETURN;
    END;

    IF (@IdentificacionEmpleado = '')
    BEGIN
        SET @Mensaje = 'Debe seleccionar un empleado.';
        RETURN;
    END;

    IF NOT EXISTS (
        SELECT 1
        FROM dbo.Empleado
        WHERE Identificacion = @IdentificacionEmpleado
          AND Activo = 1
    )
    BEGIN
        SET @Mensaje = 'El empleado seleccionado no existe o está inactivo.';
        RETURN;
    END;

    IF (@FechaNotaCredito IS NULL)
    BEGIN
        SET @Mensaje = 'Debe ingresar la fecha de la nota de crédito.';
        RETURN;
    END;

    IF (@Motivo = '')
    BEGIN
        SET @Mensaje = 'Debe ingresar el motivo.';
        RETURN;
    END;

    IF (LEN(@Motivo) > 250)
    BEGIN
        SET @Mensaje = 'El motivo no puede superar los 250 caracteres.';
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

    UPDATE dbo.NotaCredito
    SET
        IdentificacionEmpleado = @IdentificacionEmpleado,
        FechaNotaCredito = @FechaNotaCredito,
        Motivo = @Motivo,
        Estado = @Estado,
        Activo = @Activo,
        FechaModificacion = GETDATE()
    WHERE NumeroNotaCredito = @NumeroNotaCredito;

    SET @Resultado = 1;
    SET @Mensaje = 'Nota de crédito actualizada correctamente.';
END;
GO

/* =========================================================
   NOTA CRÉDITO - INACTIVAR / ANULAR
   Revierte el efecto en cuenta por cobrar si existía.
   ========================================================= */
CREATE OR ALTER PROCEDURE Facturacion.sp_NotaCredito_Inactivar
    @NumeroNotaCredito VARCHAR(45),
    @Resultado BIT OUTPUT,
    @Mensaje VARCHAR(500) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @NumeroFactura VARCHAR(45);
    DECLARE @IdentificacionCliente VARCHAR(45);
    DECLARE @TotalNotaCredito DECIMAL(18,2);
    DECLARE @MontoOriginal DECIMAL(18,2);
    DECLARE @SaldoActual DECIMAL(18,2);
    DECLARE @NuevoSaldo DECIMAL(18,2);
    DECLARE @NuevoEstadoCuenta VARCHAR(45);

    SET @Resultado = 0;
    SET @Mensaje = '';

    SET @NumeroNotaCredito = LTRIM(RTRIM(ISNULL(@NumeroNotaCredito, '')));

    IF (@NumeroNotaCredito = '')
    BEGIN
        SET @Mensaje = 'Debe seleccionar una nota de crédito válida.';
        RETURN;
    END;

    SELECT
        @NumeroFactura = nc.NumeroFactura,
        @TotalNotaCredito = nc.TotalNotaCredito,
        @IdentificacionCliente = f.IdentificacionCliente
    FROM dbo.NotaCredito nc
    INNER JOIN dbo.Factura f
        ON f.NumeroFactura = nc.NumeroFactura
    WHERE nc.NumeroNotaCredito = @NumeroNotaCredito
      AND nc.Activo = 1;

    IF (@NumeroFactura IS NULL)
    BEGIN
        SET @Mensaje = 'La nota de crédito no existe o ya está inactiva.';
        RETURN;
    END;

    BEGIN TRY
        BEGIN TRANSACTION;

        UPDATE dbo.NotaCredito
        SET
            Activo = 0,
            Estado = 'Anulada',
            FechaModificacion = GETDATE()
        WHERE NumeroNotaCredito = @NumeroNotaCredito;

        /* Si existe cuenta por cobrar, se devuelve el saldo */
        IF EXISTS (
            SELECT 1
            FROM dbo.CuentaPorCobrar
            WHERE IdentificacionCliente = @IdentificacionCliente
              AND NumeroFactura = @NumeroFactura
              AND Activo = 1
        )
        BEGIN
            SELECT
                @MontoOriginal = MontoOriginal,
                @SaldoActual = SaldoActual
            FROM dbo.CuentaPorCobrar
            WHERE IdentificacionCliente = @IdentificacionCliente
              AND NumeroFactura = @NumeroFactura;

            SET @NuevoSaldo = @SaldoActual + @TotalNotaCredito;

            IF (@NuevoSaldo > @MontoOriginal)
            BEGIN
                SET @NuevoSaldo = @MontoOriginal;
            END;

            SET @NuevoEstadoCuenta =
                CASE
                    WHEN @NuevoSaldo = 0 THEN 'Pagada'
                    WHEN @NuevoSaldo < @MontoOriginal THEN 'Parcial'
                    ELSE 'Pendiente'
                END;

            UPDATE dbo.CuentaPorCobrar
            SET
                SaldoActual = @NuevoSaldo,
                Estado = @NuevoEstadoCuenta,
                FechaModificacion = GETDATE()
            WHERE IdentificacionCliente = @IdentificacionCliente
              AND NumeroFactura = @NumeroFactura;
        END;

        COMMIT TRANSACTION;

        SET @Resultado = 1;
        SET @Mensaje = 'Nota de crédito anulada correctamente.';
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;

        SET @Resultado = 0;
        SET @Mensaje = ERROR_MESSAGE();
    END CATCH;
END;
GO