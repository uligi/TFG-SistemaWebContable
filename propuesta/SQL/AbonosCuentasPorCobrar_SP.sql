IF NOT EXISTS (SELECT 1 FROM sys.schemas WHERE name = 'Cuentas')
BEGIN
    EXEC('CREATE SCHEMA Cuentas');
END;
GO

/* =========================================================
   ABONO CUENTA POR COBRAR - LISTAR
   ========================================================= */
CREATE OR ALTER PROCEDURE Cuentas.sp_AbonoCuentaPorCobrar_Listar
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        a.IdentificacionCliente,
        a.NumeroFactura,
        a.NumeroAbono,
        a.FechaAbono,
        a.MontoAbono,
        a.Observacion,
        a.FechaCreacion,
        a.FechaModificacion,
        a.Activo,
        cxc.FechaEmision,
        cxc.FechaVencimiento,
        cxc.MontoOriginal,
        cxc.SaldoActual,
        cxc.Estado AS EstadoCuenta,
        LTRIM(RTRIM(ISNULL(p.Nombre, '') + ' ' + ISNULL(p.PrimerApellido, '') + ' ' + ISNULL(p.SegundoApellido, ''))) AS ClienteNombre
    FROM dbo.AbonoCuentaPorCobrar a
    INNER JOIN dbo.CuentaPorCobrar cxc
        ON cxc.IdentificacionCliente = a.IdentificacionCliente
       AND cxc.NumeroFactura = a.NumeroFactura
    INNER JOIN dbo.Cliente c
        ON c.Identificacion = a.IdentificacionCliente
    INNER JOIN dbo.Persona p
        ON p.Identificacion = c.Identificacion
    ORDER BY a.FechaAbono DESC, a.NumeroFactura DESC, a.NumeroAbono DESC;
END;
GO

/* =========================================================
   ABONO CUENTA POR COBRAR - LISTAR POR CUENTA
   ========================================================= */
CREATE OR ALTER PROCEDURE Cuentas.sp_AbonoCuentaPorCobrar_ListarPorCuenta
    @IdentificacionCliente VARCHAR(45),
    @NumeroFactura VARCHAR(45)
AS
BEGIN
    SET NOCOUNT ON;

    SET @IdentificacionCliente = LTRIM(RTRIM(ISNULL(@IdentificacionCliente, '')));
    SET @NumeroFactura = LTRIM(RTRIM(ISNULL(@NumeroFactura, '')));

    SELECT
        a.IdentificacionCliente,
        a.NumeroFactura,
        a.NumeroAbono,
        a.FechaAbono,
        a.MontoAbono,
        a.Observacion,
        a.FechaCreacion,
        a.FechaModificacion,
        a.Activo,
        cxc.FechaEmision,
        cxc.FechaVencimiento,
        cxc.MontoOriginal,
        cxc.SaldoActual,
        cxc.Estado AS EstadoCuenta,
        LTRIM(RTRIM(ISNULL(p.Nombre, '') + ' ' + ISNULL(p.PrimerApellido, '') + ' ' + ISNULL(p.SegundoApellido, ''))) AS ClienteNombre
    FROM dbo.AbonoCuentaPorCobrar a
    INNER JOIN dbo.CuentaPorCobrar cxc
        ON cxc.IdentificacionCliente = a.IdentificacionCliente
       AND cxc.NumeroFactura = a.NumeroFactura
    INNER JOIN dbo.Cliente c
        ON c.Identificacion = a.IdentificacionCliente
    INNER JOIN dbo.Persona p
        ON p.Identificacion = c.Identificacion
    WHERE a.IdentificacionCliente = @IdentificacionCliente
      AND a.NumeroFactura = @NumeroFactura
    ORDER BY a.NumeroAbono ASC;
END;
GO

/* =========================================================
   ABONO CUENTA POR COBRAR - OBTENER
   ========================================================= */
CREATE OR ALTER PROCEDURE Cuentas.sp_AbonoCuentaPorCobrar_Obtener
    @IdentificacionCliente VARCHAR(45),
    @NumeroFactura VARCHAR(45),
    @NumeroAbono INT
AS
BEGIN
    SET NOCOUNT ON;

    SET @IdentificacionCliente = LTRIM(RTRIM(ISNULL(@IdentificacionCliente, '')));
    SET @NumeroFactura = LTRIM(RTRIM(ISNULL(@NumeroFactura, '')));

    SELECT
        a.IdentificacionCliente,
        a.NumeroFactura,
        a.NumeroAbono,
        a.FechaAbono,
        a.MontoAbono,
        a.Observacion,
        a.FechaCreacion,
        a.FechaModificacion,
        a.Activo,
        cxc.FechaEmision,
        cxc.FechaVencimiento,
        cxc.MontoOriginal,
        cxc.SaldoActual,
        cxc.Estado AS EstadoCuenta,
        LTRIM(RTRIM(ISNULL(p.Nombre, '') + ' ' + ISNULL(p.PrimerApellido, '') + ' ' + ISNULL(p.SegundoApellido, ''))) AS ClienteNombre
    FROM dbo.AbonoCuentaPorCobrar a
    INNER JOIN dbo.CuentaPorCobrar cxc
        ON cxc.IdentificacionCliente = a.IdentificacionCliente
       AND cxc.NumeroFactura = a.NumeroFactura
    INNER JOIN dbo.Cliente c
        ON c.Identificacion = a.IdentificacionCliente
    INNER JOIN dbo.Persona p
        ON p.Identificacion = c.Identificacion
    WHERE a.IdentificacionCliente = @IdentificacionCliente
      AND a.NumeroFactura = @NumeroFactura
      AND a.NumeroAbono = @NumeroAbono;
END;
GO

/* =========================================================
   ABONO CUENTA POR COBRAR - GENERAR NUMERO
   ========================================================= */
CREATE OR ALTER PROCEDURE Cuentas.sp_AbonoCuentaPorCobrar_GenerarNumero
    @IdentificacionCliente VARCHAR(45),
    @NumeroFactura VARCHAR(45),
    @NumeroAbono INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    SET @IdentificacionCliente = LTRIM(RTRIM(ISNULL(@IdentificacionCliente, '')));
    SET @NumeroFactura = LTRIM(RTRIM(ISNULL(@NumeroFactura, '')));

    SELECT @NumeroAbono = ISNULL(MAX(NumeroAbono), 0) + 1
    FROM dbo.AbonoCuentaPorCobrar
    WHERE IdentificacionCliente = @IdentificacionCliente
      AND NumeroFactura = @NumeroFactura;
END;
GO

/* =========================================================
   ABONO CUENTA POR COBRAR - REGISTRAR
   Registra el abono, recalcula saldo y crea ingreso automático.
   ========================================================= */
CREATE OR ALTER PROCEDURE Cuentas.sp_AbonoCuentaPorCobrar_Registrar
    @IdentificacionCliente VARCHAR(45),
    @NumeroFactura VARCHAR(45),
    @FechaAbono DATETIME,
    @MontoAbono DECIMAL(18,2),
    @Observacion VARCHAR(150),
    @NumeroAbonoGenerado INT OUTPUT,
    @Resultado INT OUTPUT,
    @Mensaje VARCHAR(500) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @NumeroAbono INT;
    DECLARE @SaldoActual DECIMAL(18,2);
    DECLARE @ResultadoRecalculo BIT;
    DECLARE @MensajeRecalculo VARCHAR(500);
    DECLARE @ResultadoIngreso BIT;
    DECLARE @MensajeIngreso VARCHAR(500);

    SET @Resultado = 0;
    SET @Mensaje = '';
    SET @NumeroAbonoGenerado = 0;

    SET @IdentificacionCliente = LTRIM(RTRIM(ISNULL(@IdentificacionCliente, '')));
    SET @NumeroFactura = LTRIM(RTRIM(ISNULL(@NumeroFactura, '')));
    SET @Observacion = LTRIM(RTRIM(ISNULL(@Observacion, '')));

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
        FROM dbo.CuentaPorCobrar
        WHERE IdentificacionCliente = @IdentificacionCliente
          AND NumeroFactura = @NumeroFactura
          AND Activo = 1
          AND Estado IN ('Pendiente', 'Parcial')
    )
    BEGIN
        SET @Mensaje = 'La cuenta por cobrar no existe, está inactiva o ya se encuentra pagada/anulada.';
        RETURN;
    END;

    SELECT @SaldoActual = SaldoActual
    FROM dbo.CuentaPorCobrar
    WHERE IdentificacionCliente = @IdentificacionCliente
      AND NumeroFactura = @NumeroFactura;

    IF (@FechaAbono IS NULL)
    BEGIN
        SET @Mensaje = 'Debe ingresar la fecha del abono.';
        RETURN;
    END;

    IF (@MontoAbono <= 0)
    BEGIN
        SET @Mensaje = 'El monto del abono debe ser mayor a cero.';
        RETURN;
    END;

    IF (@MontoAbono > @SaldoActual)
    BEGIN
        SET @Mensaje = 'El monto del abono no puede ser mayor que el saldo actual.';
        RETURN;
    END;

    IF (LEN(@Observacion) > 150)
    BEGIN
        SET @Mensaje = 'La observación no puede superar los 150 caracteres.';
        RETURN;
    END;

    BEGIN TRY
        BEGIN TRANSACTION;

        EXEC Cuentas.sp_AbonoCuentaPorCobrar_GenerarNumero
            @IdentificacionCliente = @IdentificacionCliente,
            @NumeroFactura = @NumeroFactura,
            @NumeroAbono = @NumeroAbono OUTPUT;

        INSERT INTO dbo.AbonoCuentaPorCobrar
        (
            IdentificacionCliente,
            NumeroFactura,
            NumeroAbono,
            FechaAbono,
            MontoAbono,
            Observacion,
            FechaCreacion,
            FechaModificacion,
            Activo
        )
        VALUES
        (
            @IdentificacionCliente,
            @NumeroFactura,
            @NumeroAbono,
            @FechaAbono,
            @MontoAbono,
            @Observacion,
            GETDATE(),
            GETDATE(),
            1
        );

        EXEC Cuentas.sp_CuentaPorCobrar_RecalcularSaldo
            @IdentificacionCliente = @IdentificacionCliente,
            @NumeroFactura = @NumeroFactura,
            @Resultado = @ResultadoRecalculo OUTPUT,
            @Mensaje = @MensajeRecalculo OUTPUT;

        IF (@ResultadoRecalculo = 0)
        BEGIN
            ROLLBACK TRANSACTION;
            SET @Resultado = 0;
            SET @NumeroAbonoGenerado = 0;
            SET @Mensaje = @MensajeRecalculo;
            RETURN;
        END;

        EXEC Movimientos.sp_Ingreso_RegistrarAutomatico
            @FechaIngreso = @FechaAbono,
            @Descripcion = 'Ingreso automático por abono de cuenta por cobrar',
            @OrigenIngreso = 'Abono CxC',
            @Monto = @MontoAbono,
            @IdentificacionCliente = '000000000',
            @NumeroFactura = 'FAC-GENERAL-000000',
            @IdentificacionClienteAbono = @IdentificacionCliente,
            @NumeroFacturaAbono = @NumeroFactura,
            @NumeroAbonoCuentaPorCobrar = @NumeroAbono,
            @Resultado = @ResultadoIngreso OUTPUT,
            @Mensaje = @MensajeIngreso OUTPUT;

        IF (@ResultadoIngreso = 0)
        BEGIN
            ROLLBACK TRANSACTION;
            SET @Resultado = 0;
            SET @NumeroAbonoGenerado = 0;
            SET @Mensaje = @MensajeIngreso;
            RETURN;
        END;

        COMMIT TRANSACTION;

        SET @Resultado = 1;
        SET @NumeroAbonoGenerado = @NumeroAbono;
        SET @Mensaje = 'Abono registrado correctamente e ingreso automático generado.';
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;

        SET @Resultado = 0;
        SET @NumeroAbonoGenerado = 0;
        SET @Mensaje = ERROR_MESSAGE();
    END CATCH;
END;
GO

/* =========================================================
   ABONO CUENTA POR COBRAR - EDITAR
   Actualiza el ingreso automático asociado.
   ========================================================= */
CREATE OR ALTER PROCEDURE Cuentas.sp_AbonoCuentaPorCobrar_Editar
    @IdentificacionCliente VARCHAR(45),
    @NumeroFactura VARCHAR(45),
    @NumeroAbono INT,
    @FechaAbono DATETIME,
    @MontoAbono DECIMAL(18,2),
    @Observacion VARCHAR(150),
    @Activo BIT,
    @Resultado BIT OUTPUT,
    @Mensaje VARCHAR(500) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @MontoOriginalCuenta DECIMAL(18,2);
    DECLARE @TotalOtrosAbonos DECIMAL(18,2);
    DECLARE @SaldoCalculado DECIMAL(18,2);
    DECLARE @ResultadoRecalculo BIT;
    DECLARE @MensajeRecalculo VARCHAR(500);

    SET @Resultado = 0;
    SET @Mensaje = '';

    SET @IdentificacionCliente = LTRIM(RTRIM(ISNULL(@IdentificacionCliente, '')));
    SET @NumeroFactura = LTRIM(RTRIM(ISNULL(@NumeroFactura, '')));
    SET @Observacion = LTRIM(RTRIM(ISNULL(@Observacion, '')));

    IF (@IdentificacionCliente = '' OR @NumeroFactura = '')
    BEGIN
        SET @Mensaje = 'Debe seleccionar una cuenta por cobrar válida.';
        RETURN;
    END;

    IF (@NumeroAbono <= 0)
    BEGIN
        SET @Mensaje = 'Debe seleccionar un número de abono válido.';
        RETURN;
    END;

    IF NOT EXISTS (
        SELECT 1
        FROM dbo.AbonoCuentaPorCobrar
        WHERE IdentificacionCliente = @IdentificacionCliente
          AND NumeroFactura = @NumeroFactura
          AND NumeroAbono = @NumeroAbono
    )
    BEGIN
        SET @Mensaje = 'El abono no existe.';
        RETURN;
    END;

    IF NOT EXISTS (
        SELECT 1
        FROM dbo.CuentaPorCobrar
        WHERE IdentificacionCliente = @IdentificacionCliente
          AND NumeroFactura = @NumeroFactura
          AND Activo = 1
    )
    BEGIN
        SET @Mensaje = 'La cuenta por cobrar no existe o está inactiva.';
        RETURN;
    END;

    IF (@FechaAbono IS NULL)
    BEGIN
        SET @Mensaje = 'Debe ingresar la fecha del abono.';
        RETURN;
    END;

    IF (@MontoAbono <= 0 AND @Activo = 1)
    BEGIN
        SET @Mensaje = 'El monto del abono debe ser mayor a cero.';
        RETURN;
    END;

    IF (LEN(@Observacion) > 150)
    BEGIN
        SET @Mensaje = 'La observación no puede superar los 150 caracteres.';
        RETURN;
    END;

    SELECT @MontoOriginalCuenta = MontoOriginal
    FROM dbo.CuentaPorCobrar
    WHERE IdentificacionCliente = @IdentificacionCliente
      AND NumeroFactura = @NumeroFactura;

    SELECT @TotalOtrosAbonos = ISNULL(SUM(MontoAbono), 0)
    FROM dbo.AbonoCuentaPorCobrar
    WHERE IdentificacionCliente = @IdentificacionCliente
      AND NumeroFactura = @NumeroFactura
      AND NumeroAbono <> @NumeroAbono
      AND Activo = 1;

    IF (@Activo = 1)
        SET @SaldoCalculado = @MontoOriginalCuenta - (@TotalOtrosAbonos + @MontoAbono);
    ELSE
        SET @SaldoCalculado = @MontoOriginalCuenta - @TotalOtrosAbonos;

    IF (@SaldoCalculado < 0)
    BEGIN
        SET @Mensaje = 'El monto del abono excede el saldo disponible de la cuenta.';
        RETURN;
    END;

    BEGIN TRY
        BEGIN TRANSACTION;

        UPDATE dbo.AbonoCuentaPorCobrar
        SET FechaAbono = @FechaAbono,
            MontoAbono = @MontoAbono,
            Observacion = @Observacion,
            FechaModificacion = GETDATE(),
            Activo = @Activo
        WHERE IdentificacionCliente = @IdentificacionCliente
          AND NumeroFactura = @NumeroFactura
          AND NumeroAbono = @NumeroAbono;

        UPDATE dbo.Ingreso
        SET FechaIngreso = @FechaAbono,
            Descripcion = 'Ingreso automático por abono de cuenta por cobrar',
            Monto = @MontoAbono,
            FechaModificacion = GETDATE(),
            Activo = @Activo
        WHERE IdentificacionClienteAbono = @IdentificacionCliente
          AND NumeroFacturaAbono = @NumeroFactura
          AND NumeroAbonoCuentaPorCobrar = @NumeroAbono
          AND OrigenIngreso = 'Abono CxC';

        EXEC Cuentas.sp_CuentaPorCobrar_RecalcularSaldo
            @IdentificacionCliente = @IdentificacionCliente,
            @NumeroFactura = @NumeroFactura,
            @Resultado = @ResultadoRecalculo OUTPUT,
            @Mensaje = @MensajeRecalculo OUTPUT;

        IF (@ResultadoRecalculo = 0)
        BEGIN
            ROLLBACK TRANSACTION;
            SET @Resultado = 0;
            SET @Mensaje = @MensajeRecalculo;
            RETURN;
        END;

        COMMIT TRANSACTION;

        SET @Resultado = 1;
        SET @Mensaje = 'Abono actualizado correctamente e ingreso automático asociado actualizado.';
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;

        SET @Resultado = 0;
        SET @Mensaje = ERROR_MESSAGE();
    END CATCH;
END;
GO

/* =========================================================
   ABONO CUENTA POR COBRAR - INACTIVAR
   Inactiva el ingreso automático asociado.
   ========================================================= */
CREATE OR ALTER PROCEDURE Cuentas.sp_AbonoCuentaPorCobrar_Inactivar
    @IdentificacionCliente VARCHAR(45),
    @NumeroFactura VARCHAR(45),
    @NumeroAbono INT,
    @Resultado BIT OUTPUT,
    @Mensaje VARCHAR(500) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @ResultadoRecalculo BIT;
    DECLARE @MensajeRecalculo VARCHAR(500);

    SET @Resultado = 0;
    SET @Mensaje = '';

    SET @IdentificacionCliente = LTRIM(RTRIM(ISNULL(@IdentificacionCliente, '')));
    SET @NumeroFactura = LTRIM(RTRIM(ISNULL(@NumeroFactura, '')));

    IF (@IdentificacionCliente = '' OR @NumeroFactura = '')
    BEGIN
        SET @Mensaje = 'Debe seleccionar una cuenta por cobrar válida.';
        RETURN;
    END;

    IF (@NumeroAbono <= 0)
    BEGIN
        SET @Mensaje = 'Debe seleccionar un número de abono válido.';
        RETURN;
    END;

    IF NOT EXISTS (
        SELECT 1
        FROM dbo.AbonoCuentaPorCobrar
        WHERE IdentificacionCliente = @IdentificacionCliente
          AND NumeroFactura = @NumeroFactura
          AND NumeroAbono = @NumeroAbono
          AND Activo = 1
    )
    BEGIN
        SET @Mensaje = 'El abono no existe o ya está inactivo.';
        RETURN;
    END;

    BEGIN TRY
        BEGIN TRANSACTION;

        UPDATE dbo.AbonoCuentaPorCobrar
        SET Activo = 0,
            FechaModificacion = GETDATE()
        WHERE IdentificacionCliente = @IdentificacionCliente
          AND NumeroFactura = @NumeroFactura
          AND NumeroAbono = @NumeroAbono;

        UPDATE dbo.Ingreso
        SET Activo = 0,
            FechaModificacion = GETDATE()
        WHERE IdentificacionClienteAbono = @IdentificacionCliente
          AND NumeroFacturaAbono = @NumeroFactura
          AND NumeroAbonoCuentaPorCobrar = @NumeroAbono
          AND OrigenIngreso = 'Abono CxC'
          AND Activo = 1;

        EXEC Cuentas.sp_CuentaPorCobrar_RecalcularSaldo
            @IdentificacionCliente = @IdentificacionCliente,
            @NumeroFactura = @NumeroFactura,
            @Resultado = @ResultadoRecalculo OUTPUT,
            @Mensaje = @MensajeRecalculo OUTPUT;

        IF (@ResultadoRecalculo = 0)
        BEGIN
            ROLLBACK TRANSACTION;
            SET @Resultado = 0;
            SET @Mensaje = @MensajeRecalculo;
            RETURN;
        END;

        COMMIT TRANSACTION;

        SET @Resultado = 1;
        SET @Mensaje = 'Abono inactivado correctamente e ingreso automático asociado inactivado.';
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;

        SET @Resultado = 0;
        SET @Mensaje = ERROR_MESSAGE();
    END CATCH;
END;
GO
