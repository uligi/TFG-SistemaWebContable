IF NOT EXISTS (SELECT 1 FROM sys.schemas WHERE name = 'Cuentas')
BEGIN
    EXEC('CREATE SCHEMA Cuentas');
END;
GO

/* =========================================================
   ABONO CUENTA POR PAGAR - LISTAR
   ========================================================= */
CREATE OR ALTER PROCEDURE Cuentas.sp_AbonoCuentaPorPagar_Listar
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        a.IdentificacionProveedor,
        p.RazonSocial AS ProveedorNombre,

        a.NumeroDocumento,
        a.NumeroAbono,
        a.FechaAbono,
        a.MontoAbono,
        a.Observacion,
        a.FechaCreacion,
        a.FechaModificacion,
        a.Activo,

        cxp.FechaEmision,
        cxp.FechaVencimiento,
        cxp.Concepto,
        cxp.MontoOriginal,
        cxp.SaldoActual,
        cxp.Estado AS EstadoCuenta
    FROM dbo.AbonoCuentaPorPagar a
    INNER JOIN dbo.CuentaPorPagar cxp
        ON cxp.IdentificacionProveedor = a.IdentificacionProveedor
       AND cxp.NumeroDocumento = a.NumeroDocumento
    INNER JOIN dbo.Proveedor p
        ON p.IdentificacionProveedor = a.IdentificacionProveedor
    ORDER BY
        a.FechaAbono DESC,
        a.NumeroDocumento DESC,
        a.NumeroAbono DESC;
END;
GO

/* =========================================================
   ABONO CUENTA POR PAGAR - LISTAR POR CUENTA
   ========================================================= */
CREATE OR ALTER PROCEDURE Cuentas.sp_AbonoCuentaPorPagar_ListarPorCuenta
    @IdentificacionProveedor VARCHAR(45),
    @NumeroDocumento VARCHAR(45)
AS
BEGIN
    SET NOCOUNT ON;

    SET @IdentificacionProveedor = LTRIM(RTRIM(ISNULL(@IdentificacionProveedor, '')));
    SET @NumeroDocumento = LTRIM(RTRIM(ISNULL(@NumeroDocumento, '')));

    SELECT
        a.IdentificacionProveedor,
        p.RazonSocial AS ProveedorNombre,

        a.NumeroDocumento,
        a.NumeroAbono,
        a.FechaAbono,
        a.MontoAbono,
        a.Observacion,
        a.FechaCreacion,
        a.FechaModificacion,
        a.Activo,

        cxp.FechaEmision,
        cxp.FechaVencimiento,
        cxp.Concepto,
        cxp.MontoOriginal,
        cxp.SaldoActual,
        cxp.Estado AS EstadoCuenta
    FROM dbo.AbonoCuentaPorPagar a
    INNER JOIN dbo.CuentaPorPagar cxp
        ON cxp.IdentificacionProveedor = a.IdentificacionProveedor
       AND cxp.NumeroDocumento = a.NumeroDocumento
    INNER JOIN dbo.Proveedor p
        ON p.IdentificacionProveedor = a.IdentificacionProveedor
    WHERE a.IdentificacionProveedor = @IdentificacionProveedor
      AND a.NumeroDocumento = @NumeroDocumento
    ORDER BY
        a.NumeroAbono DESC;
END;
GO

/* =========================================================
   ABONO CUENTA POR PAGAR - OBTENER
   ========================================================= */
CREATE OR ALTER PROCEDURE Cuentas.sp_AbonoCuentaPorPagar_Obtener
    @IdentificacionProveedor VARCHAR(45),
    @NumeroDocumento VARCHAR(45),
    @NumeroAbono INT
AS
BEGIN
    SET NOCOUNT ON;

    SET @IdentificacionProveedor = LTRIM(RTRIM(ISNULL(@IdentificacionProveedor, '')));
    SET @NumeroDocumento = LTRIM(RTRIM(ISNULL(@NumeroDocumento, '')));

    SELECT
        a.IdentificacionProveedor,
        p.RazonSocial AS ProveedorNombre,

        a.NumeroDocumento,
        a.NumeroAbono,
        a.FechaAbono,
        a.MontoAbono,
        a.Observacion,
        a.FechaCreacion,
        a.FechaModificacion,
        a.Activo,

        cxp.FechaEmision,
        cxp.FechaVencimiento,
        cxp.Concepto,
        cxp.MontoOriginal,
        cxp.SaldoActual,
        cxp.Estado AS EstadoCuenta
    FROM dbo.AbonoCuentaPorPagar a
    INNER JOIN dbo.CuentaPorPagar cxp
        ON cxp.IdentificacionProveedor = a.IdentificacionProveedor
       AND cxp.NumeroDocumento = a.NumeroDocumento
    INNER JOIN dbo.Proveedor p
        ON p.IdentificacionProveedor = a.IdentificacionProveedor
    WHERE a.IdentificacionProveedor = @IdentificacionProveedor
      AND a.NumeroDocumento = @NumeroDocumento
      AND a.NumeroAbono = @NumeroAbono;
END;
GO

/* =========================================================
   ABONO CUENTA POR PAGAR - REGISTRAR
   Registra abono, recalcula saldo y genera gasto automático.
   ========================================================= */
CREATE OR ALTER PROCEDURE Cuentas.sp_AbonoCuentaPorPagar_Registrar
    @IdentificacionProveedor VARCHAR(45),
    @NumeroDocumento VARCHAR(45),
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
    DECLARE @MontoOriginal DECIMAL(18,2);
    DECLARE @SaldoActual DECIMAL(18,2);
    DECLARE @NuevoSaldo DECIMAL(18,2);
    DECLARE @NuevoEstado VARCHAR(45);

    DECLARE @ResultadoGasto BIT;
    DECLARE @MensajeGasto VARCHAR(500);

    SET @Resultado = 0;
    SET @Mensaje = '';
    SET @NumeroAbonoGenerado = 0;

    SET @IdentificacionProveedor = LTRIM(RTRIM(ISNULL(@IdentificacionProveedor, '')));
    SET @NumeroDocumento = LTRIM(RTRIM(ISNULL(@NumeroDocumento, '')));
    SET @Observacion = LTRIM(RTRIM(ISNULL(@Observacion, '')));

    IF (@IdentificacionProveedor = '')
    BEGIN
        SET @Mensaje = 'Debe seleccionar un proveedor.';
        RETURN;
    END;

    IF (@NumeroDocumento = '')
    BEGIN
        SET @Mensaje = 'Debe seleccionar un número de documento.';
        RETURN;
    END;

    IF NOT EXISTS (
        SELECT 1
        FROM dbo.CuentaPorPagar
        WHERE IdentificacionProveedor = @IdentificacionProveedor
          AND NumeroDocumento = @NumeroDocumento
          AND Activo = 1
    )
    BEGIN
        SET @Mensaje = 'La cuenta por pagar no existe o está inactiva.';
        RETURN;
    END;

    SELECT
        @MontoOriginal = MontoOriginal,
        @SaldoActual = SaldoActual
    FROM dbo.CuentaPorPagar
    WHERE IdentificacionProveedor = @IdentificacionProveedor
      AND NumeroDocumento = @NumeroDocumento;

    IF (@SaldoActual <= 0)
    BEGIN
        SET @Mensaje = 'La cuenta por pagar ya se encuentra pagada.';
        RETURN;
    END;

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
        SET @Mensaje = 'El monto del abono no puede ser mayor al saldo actual.';
        RETURN;
    END;

    IF (LEN(@Observacion) > 150)
    BEGIN
        SET @Mensaje = 'La observación no puede superar los 150 caracteres.';
        RETURN;
    END;

    BEGIN TRY
        BEGIN TRANSACTION;

        SELECT @NumeroAbono = ISNULL(MAX(NumeroAbono), 0) + 1
        FROM dbo.AbonoCuentaPorPagar
        WHERE IdentificacionProveedor = @IdentificacionProveedor
          AND NumeroDocumento = @NumeroDocumento;

        INSERT INTO dbo.AbonoCuentaPorPagar
        (
            IdentificacionProveedor,
            NumeroDocumento,
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
            @IdentificacionProveedor,
            @NumeroDocumento,
            @NumeroAbono,
            @FechaAbono,
            @MontoAbono,
            @Observacion,
            GETDATE(),
            GETDATE(),
            1
        );

        SET @NuevoSaldo = @SaldoActual - @MontoAbono;

        IF (@NuevoSaldo < 0)
        BEGIN
            SET @NuevoSaldo = 0;
        END;

        SET @NuevoEstado =
            CASE
                WHEN @NuevoSaldo = 0 THEN 'Pagada'
                WHEN @NuevoSaldo < @MontoOriginal THEN 'Parcial'
                ELSE 'Pendiente'
            END;

        UPDATE dbo.CuentaPorPagar
        SET
            SaldoActual = @NuevoSaldo,
            Estado = @NuevoEstado,
            FechaModificacion = GETDATE()
        WHERE IdentificacionProveedor = @IdentificacionProveedor
          AND NumeroDocumento = @NumeroDocumento;

        EXEC Movimientos.sp_Gasto_RegistrarAutomatico
            @FechaGasto = @FechaAbono,
            @Descripcion = 'Gasto automático por abono de cuenta por pagar',
            @Monto = @MontoAbono,

            @IdentificacionProveedor = @IdentificacionProveedor,
            @NumeroDocumento = @NumeroDocumento,

            @IdentificacionProveedorAbono = @IdentificacionProveedor,
            @NumeroDocumentoAbono = @NumeroDocumento,
            @NumeroAbonoCuentaPorPagar = @NumeroAbono,

            @Resultado = @ResultadoGasto OUTPUT,
            @Mensaje = @MensajeGasto OUTPUT;

        IF (@ResultadoGasto = 0)
        BEGIN
            ROLLBACK TRANSACTION;

            SET @Resultado = 0;
            SET @NumeroAbonoGenerado = 0;
            SET @Mensaje = @MensajeGasto;
            RETURN;
        END;

        COMMIT TRANSACTION;

        SET @Resultado = 1;
        SET @NumeroAbonoGenerado = @NumeroAbono;
        SET @Mensaje = 'Abono de cuenta por pagar registrado correctamente y gasto automático generado.';
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
   ABONO CUENTA POR PAGAR - EDITAR
   Edita abono, recalcula saldo y actualiza gasto automático.
   ========================================================= */
CREATE OR ALTER PROCEDURE Cuentas.sp_AbonoCuentaPorPagar_Editar
    @IdentificacionProveedor VARCHAR(45),
    @NumeroDocumento VARCHAR(45),
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

    DECLARE @MontoOriginal DECIMAL(18,2);
    DECLARE @TotalAbonos DECIMAL(18,2);
    DECLARE @NuevoSaldo DECIMAL(18,2);
    DECLARE @NuevoEstado VARCHAR(45);

    SET @Resultado = 0;
    SET @Mensaje = '';

    SET @IdentificacionProveedor = LTRIM(RTRIM(ISNULL(@IdentificacionProveedor, '')));
    SET @NumeroDocumento = LTRIM(RTRIM(ISNULL(@NumeroDocumento, '')));
    SET @Observacion = LTRIM(RTRIM(ISNULL(@Observacion, '')));

    IF (@IdentificacionProveedor = '' OR @NumeroDocumento = '' OR @NumeroAbono <= 0)
    BEGIN
        SET @Mensaje = 'Debe seleccionar un abono válido.';
        RETURN;
    END;

    IF NOT EXISTS (
        SELECT 1
        FROM dbo.AbonoCuentaPorPagar
        WHERE IdentificacionProveedor = @IdentificacionProveedor
          AND NumeroDocumento = @NumeroDocumento
          AND NumeroAbono = @NumeroAbono
    )
    BEGIN
        SET @Mensaje = 'El abono seleccionado no existe.';
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

    BEGIN TRY
        BEGIN TRANSACTION;

        UPDATE dbo.AbonoCuentaPorPagar
        SET
            FechaAbono = @FechaAbono,
            MontoAbono = @MontoAbono,
            Observacion = @Observacion,
            FechaModificacion = GETDATE(),
            Activo = @Activo
        WHERE IdentificacionProveedor = @IdentificacionProveedor
          AND NumeroDocumento = @NumeroDocumento
          AND NumeroAbono = @NumeroAbono;

        SELECT @MontoOriginal = MontoOriginal
        FROM dbo.CuentaPorPagar
        WHERE IdentificacionProveedor = @IdentificacionProveedor
          AND NumeroDocumento = @NumeroDocumento;

        SELECT @TotalAbonos = ISNULL(SUM(MontoAbono), 0)
        FROM dbo.AbonoCuentaPorPagar
        WHERE IdentificacionProveedor = @IdentificacionProveedor
          AND NumeroDocumento = @NumeroDocumento
          AND Activo = 1;

        SET @NuevoSaldo = @MontoOriginal - @TotalAbonos;

        IF (@NuevoSaldo < 0)
        BEGIN
            ROLLBACK TRANSACTION;
            SET @Mensaje = 'La suma de abonos activos no puede superar el monto original.';
            RETURN;
        END;

        SET @NuevoEstado =
            CASE
                WHEN @NuevoSaldo = 0 THEN 'Pagada'
                WHEN @NuevoSaldo < @MontoOriginal THEN 'Parcial'
                ELSE 'Pendiente'
            END;

        UPDATE dbo.CuentaPorPagar
        SET
            SaldoActual = @NuevoSaldo,
            Estado = @NuevoEstado,
            FechaModificacion = GETDATE()
        WHERE IdentificacionProveedor = @IdentificacionProveedor
          AND NumeroDocumento = @NumeroDocumento;

        UPDATE dbo.Gasto
        SET
            FechaGasto = @FechaAbono,
            Descripcion = 'Gasto automático por abono de cuenta por pagar',
            Monto = @MontoAbono,
            FechaModificacion = GETDATE(),
            Activo = @Activo
        WHERE IdentificacionProveedorAbono = @IdentificacionProveedor
          AND NumeroDocumentoAbono = @NumeroDocumento
          AND NumeroAbonoCuentaPorPagar = @NumeroAbono;

        COMMIT TRANSACTION;

        SET @Resultado = 1;
        SET @Mensaje = 'Abono de cuenta por pagar actualizado correctamente y gasto automático actualizado.';
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
   ABONO CUENTA POR PAGAR - INACTIVAR
   Inactiva abono, recalcula saldo e inactiva gasto automático.
   ========================================================= */
CREATE OR ALTER PROCEDURE Cuentas.sp_AbonoCuentaPorPagar_Inactivar
    @IdentificacionProveedor VARCHAR(45),
    @NumeroDocumento VARCHAR(45),
    @NumeroAbono INT,
    @Resultado BIT OUTPUT,
    @Mensaje VARCHAR(500) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @MontoOriginal DECIMAL(18,2);
    DECLARE @TotalAbonos DECIMAL(18,2);
    DECLARE @NuevoSaldo DECIMAL(18,2);
    DECLARE @NuevoEstado VARCHAR(45);

    SET @Resultado = 0;
    SET @Mensaje = '';

    SET @IdentificacionProveedor = LTRIM(RTRIM(ISNULL(@IdentificacionProveedor, '')));
    SET @NumeroDocumento = LTRIM(RTRIM(ISNULL(@NumeroDocumento, '')));

    IF (@IdentificacionProveedor = '' OR @NumeroDocumento = '' OR @NumeroAbono <= 0)
    BEGIN
        SET @Mensaje = 'Debe seleccionar un abono válido.';
        RETURN;
    END;

    IF NOT EXISTS (
        SELECT 1
        FROM dbo.AbonoCuentaPorPagar
        WHERE IdentificacionProveedor = @IdentificacionProveedor
          AND NumeroDocumento = @NumeroDocumento
          AND NumeroAbono = @NumeroAbono
          AND Activo = 1
    )
    BEGIN
        SET @Mensaje = 'El abono no existe o ya está inactivo.';
        RETURN;
    END;

    BEGIN TRY
        BEGIN TRANSACTION;

        UPDATE dbo.AbonoCuentaPorPagar
        SET
            Activo = 0,
            FechaModificacion = GETDATE()
        WHERE IdentificacionProveedor = @IdentificacionProveedor
          AND NumeroDocumento = @NumeroDocumento
          AND NumeroAbono = @NumeroAbono;

        UPDATE dbo.Gasto
        SET
            Activo = 0,
            FechaModificacion = GETDATE()
        WHERE IdentificacionProveedorAbono = @IdentificacionProveedor
          AND NumeroDocumentoAbono = @NumeroDocumento
          AND NumeroAbonoCuentaPorPagar = @NumeroAbono
          AND Activo = 1;

        SELECT @MontoOriginal = MontoOriginal
        FROM dbo.CuentaPorPagar
        WHERE IdentificacionProveedor = @IdentificacionProveedor
          AND NumeroDocumento = @NumeroDocumento;

        SELECT @TotalAbonos = ISNULL(SUM(MontoAbono), 0)
        FROM dbo.AbonoCuentaPorPagar
        WHERE IdentificacionProveedor = @IdentificacionProveedor
          AND NumeroDocumento = @NumeroDocumento
          AND Activo = 1;

        SET @NuevoSaldo = @MontoOriginal - @TotalAbonos;

        IF (@NuevoSaldo < 0)
        BEGIN
            SET @NuevoSaldo = 0;
        END;

        SET @NuevoEstado =
            CASE
                WHEN @NuevoSaldo = 0 THEN 'Pagada'
                WHEN @NuevoSaldo < @MontoOriginal THEN 'Parcial'
                ELSE 'Pendiente'
            END;

        UPDATE dbo.CuentaPorPagar
        SET
            SaldoActual = @NuevoSaldo,
            Estado = @NuevoEstado,
            FechaModificacion = GETDATE()
        WHERE IdentificacionProveedor = @IdentificacionProveedor
          AND NumeroDocumento = @NumeroDocumento;

        COMMIT TRANSACTION;

        SET @Resultado = 1;
        SET @Mensaje = 'Abono de cuenta por pagar inactivado correctamente y gasto automático inactivado.';
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;

        SET @Resultado = 0;
        SET @Mensaje = ERROR_MESSAGE();
    END CATCH;
END;
GO