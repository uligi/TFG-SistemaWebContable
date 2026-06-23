IF NOT EXISTS (SELECT 1 FROM sys.schemas WHERE name = 'Cuentas')
BEGIN
    EXEC('CREATE SCHEMA Cuentas');
END;
GO

/* =========================================================
   CUENTA POR PAGAR - LISTAR
   ========================================================= */
CREATE OR ALTER PROCEDURE Cuentas.sp_CuentaPorPagar_Listar
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        cxp.IdentificacionProveedor,
        p.RazonSocial AS ProveedorNombre,
        p.DiasCredito,

        cxp.NumeroDocumento,
        cxp.FechaEmision,
        cxp.FechaVencimiento,
        cxp.Concepto,
        cxp.MontoOriginal,
        cxp.SaldoActual,
        cxp.Estado,
        cxp.FechaCreacion,
        cxp.FechaModificacion,
        cxp.Activo,

        DATEDIFF(DAY, CAST(GETDATE() AS DATE), CAST(cxp.FechaVencimiento AS DATE)) AS DiasRestantes,

        CASE
            WHEN cxp.SaldoActual <= 0 THEN 'Pagada'
            WHEN cxp.Activo = 0 THEN 'Inactiva'
            WHEN cxp.Estado = 'Anulada' THEN 'Anulada'
            WHEN CAST(cxp.FechaVencimiento AS DATE) < CAST(GETDATE() AS DATE) THEN 'Vencida'
            WHEN DATEDIFF(DAY, CAST(GETDATE() AS DATE), CAST(cxp.FechaVencimiento AS DATE)) <= 3 THEN 'Por vencer'
            ELSE 'Pendiente'
        END AS EstadoCredito
    FROM dbo.CuentaPorPagar cxp
    INNER JOIN dbo.Proveedor p
        ON p.IdentificacionProveedor = cxp.IdentificacionProveedor
    ORDER BY
        cxp.FechaEmision DESC,
        cxp.NumeroDocumento DESC;
END;
GO

/* =========================================================
   CUENTA POR PAGAR - LISTAR PENDIENTES
   ========================================================= */
CREATE OR ALTER PROCEDURE Cuentas.sp_CuentaPorPagar_ListarPendientes
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        cxp.IdentificacionProveedor,
        p.RazonSocial AS ProveedorNombre,
        p.DiasCredito,

        cxp.NumeroDocumento,
        cxp.FechaEmision,
        cxp.FechaVencimiento,
        cxp.Concepto,
        cxp.MontoOriginal,
        cxp.SaldoActual,
        cxp.Estado,
        cxp.FechaCreacion,
        cxp.FechaModificacion,
        cxp.Activo,

        DATEDIFF(DAY, CAST(GETDATE() AS DATE), CAST(cxp.FechaVencimiento AS DATE)) AS DiasRestantes,

        CASE
            WHEN cxp.SaldoActual <= 0 THEN 'Pagada'
            WHEN cxp.Activo = 0 THEN 'Inactiva'
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
      AND cxp.Estado IN ('Pendiente', 'Parcial')
    ORDER BY
        cxp.FechaVencimiento ASC,
        cxp.NumeroDocumento DESC;
END;
GO

/* =========================================================
   CUENTA POR PAGAR - LISTAR POR PROVEEDOR
   ========================================================= */
CREATE OR ALTER PROCEDURE Cuentas.sp_CuentaPorPagar_ListarPorProveedor
    @IdentificacionProveedor VARCHAR(45)
AS
BEGIN
    SET NOCOUNT ON;

    SET @IdentificacionProveedor = LTRIM(RTRIM(ISNULL(@IdentificacionProveedor, '')));

    SELECT
        cxp.IdentificacionProveedor,
        p.RazonSocial AS ProveedorNombre,
        p.DiasCredito,

        cxp.NumeroDocumento,
        cxp.FechaEmision,
        cxp.FechaVencimiento,
        cxp.Concepto,
        cxp.MontoOriginal,
        cxp.SaldoActual,
        cxp.Estado,
        cxp.FechaCreacion,
        cxp.FechaModificacion,
        cxp.Activo,

        DATEDIFF(DAY, CAST(GETDATE() AS DATE), CAST(cxp.FechaVencimiento AS DATE)) AS DiasRestantes,

        CASE
            WHEN cxp.SaldoActual <= 0 THEN 'Pagada'
            WHEN cxp.Activo = 0 THEN 'Inactiva'
            WHEN cxp.Estado = 'Anulada' THEN 'Anulada'
            WHEN CAST(cxp.FechaVencimiento AS DATE) < CAST(GETDATE() AS DATE) THEN 'Vencida'
            WHEN DATEDIFF(DAY, CAST(GETDATE() AS DATE), CAST(cxp.FechaVencimiento AS DATE)) <= 3 THEN 'Por vencer'
            ELSE 'Pendiente'
        END AS EstadoCredito
    FROM dbo.CuentaPorPagar cxp
    INNER JOIN dbo.Proveedor p
        ON p.IdentificacionProveedor = cxp.IdentificacionProveedor
    WHERE cxp.IdentificacionProveedor = @IdentificacionProveedor
    ORDER BY
        cxp.FechaEmision DESC,
        cxp.NumeroDocumento DESC;
END;
GO

/* =========================================================
   CUENTA POR PAGAR - OBTENER
   ========================================================= */
CREATE OR ALTER PROCEDURE Cuentas.sp_CuentaPorPagar_Obtener
    @IdentificacionProveedor VARCHAR(45),
    @NumeroDocumento VARCHAR(45)
AS
BEGIN
    SET NOCOUNT ON;

    SET @IdentificacionProveedor = LTRIM(RTRIM(ISNULL(@IdentificacionProveedor, '')));
    SET @NumeroDocumento = LTRIM(RTRIM(ISNULL(@NumeroDocumento, '')));

    SELECT
        cxp.IdentificacionProveedor,
        p.RazonSocial AS ProveedorNombre,
        p.DiasCredito,

        cxp.NumeroDocumento,
        cxp.FechaEmision,
        cxp.FechaVencimiento,
        cxp.Concepto,
        cxp.MontoOriginal,
        cxp.SaldoActual,
        cxp.Estado,
        cxp.FechaCreacion,
        cxp.FechaModificacion,
        cxp.Activo,

        DATEDIFF(DAY, CAST(GETDATE() AS DATE), CAST(cxp.FechaVencimiento AS DATE)) AS DiasRestantes,

        CASE
            WHEN cxp.SaldoActual <= 0 THEN 'Pagada'
            WHEN cxp.Activo = 0 THEN 'Inactiva'
            WHEN cxp.Estado = 'Anulada' THEN 'Anulada'
            WHEN CAST(cxp.FechaVencimiento AS DATE) < CAST(GETDATE() AS DATE) THEN 'Vencida'
            WHEN DATEDIFF(DAY, CAST(GETDATE() AS DATE), CAST(cxp.FechaVencimiento AS DATE)) <= 3 THEN 'Por vencer'
            ELSE 'Pendiente'
        END AS EstadoCredito
    FROM dbo.CuentaPorPagar cxp
    INNER JOIN dbo.Proveedor p
        ON p.IdentificacionProveedor = cxp.IdentificacionProveedor
    WHERE cxp.IdentificacionProveedor = @IdentificacionProveedor
      AND cxp.NumeroDocumento = @NumeroDocumento;
END;
GO

/* =========================================================
   CUENTA POR PAGAR - REGISTRAR
   Genera NumeroDocumento automático si viene vacío.
   ========================================================= */
CREATE OR ALTER PROCEDURE Cuentas.sp_CuentaPorPagar_Registrar
    @IdentificacionProveedor VARCHAR(45),
    @NumeroDocumento VARCHAR(45),
    @FechaEmision DATETIME,
    @FechaVencimiento DATETIME,
    @Concepto VARCHAR(100),
    @MontoOriginal DECIMAL(18,2),
    @Estado VARCHAR(45),
    @NumeroDocumentoGenerado VARCHAR(45) OUTPUT,
    @Resultado INT OUTPUT,
    @Mensaje VARCHAR(500) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @DiasCredito INT;
    DECLARE @Anio INT;

    SET @Resultado = 0;
    SET @Mensaje = '';
    SET @NumeroDocumentoGenerado = '';

    SET @IdentificacionProveedor = LTRIM(RTRIM(ISNULL(@IdentificacionProveedor, '')));
    SET @NumeroDocumento = LTRIM(RTRIM(ISNULL(@NumeroDocumento, '')));
    SET @Concepto = LTRIM(RTRIM(ISNULL(@Concepto, '')));
    SET @Estado = LTRIM(RTRIM(ISNULL(@Estado, '')));

    IF (@IdentificacionProveedor = '')
    BEGIN
        SET @Mensaje = 'Debe seleccionar un proveedor.';
        RETURN;
    END;

    IF NOT EXISTS (
        SELECT 1
        FROM dbo.Proveedor
        WHERE IdentificacionProveedor = @IdentificacionProveedor
          AND Activo = 1
    )
    BEGIN
        SET @Mensaje = 'El proveedor seleccionado no existe o está inactivo.';
        RETURN;
    END;

    IF (@FechaEmision IS NULL)
    BEGIN
        SET @Mensaje = 'Debe ingresar la fecha de emisión.';
        RETURN;
    END;

    IF (@NumeroDocumento = '')
    BEGIN
        SET @Anio = YEAR(@FechaEmision);

        EXEC Cuentas.sp_CuentaPorPagar_GenerarNumeroDocumento
            @Anio = @Anio,
            @NumeroDocumento = @NumeroDocumento OUTPUT;
    END;

    IF (@NumeroDocumento = '')
    BEGIN
        SET @Mensaje = 'No se pudo generar el número de documento.';
        RETURN;
    END;

    IF (LEN(@NumeroDocumento) > 45)
    BEGIN
        SET @Mensaje = 'El número de documento no puede superar los 45 caracteres.';
        RETURN;
    END;

    IF EXISTS (
        SELECT 1
        FROM dbo.CuentaPorPagar
        WHERE IdentificacionProveedor = @IdentificacionProveedor
          AND NumeroDocumento = @NumeroDocumento
    )
    BEGIN
        SET @Mensaje = 'Ya existe una cuenta por pagar para este proveedor y número de documento.';
        RETURN;
    END;

    IF (@FechaVencimiento IS NULL)
    BEGIN
        SELECT @DiasCredito = ISNULL(DiasCredito, 0)
        FROM dbo.Proveedor
        WHERE IdentificacionProveedor = @IdentificacionProveedor;

        SET @FechaVencimiento = DATEADD(DAY, ISNULL(@DiasCredito, 0), @FechaEmision);
    END;

    IF (@FechaVencimiento < @FechaEmision)
    BEGIN
        SET @Mensaje = 'La fecha de vencimiento no puede ser menor que la fecha de emisión.';
        RETURN;
    END;

    IF (@Concepto = '')
    BEGIN
        SET @Mensaje = 'Debe ingresar el concepto de la cuenta por pagar.';
        RETURN;
    END;

    IF (LEN(@Concepto) > 100)
    BEGIN
        SET @Mensaje = 'El concepto no puede superar los 100 caracteres.';
        RETURN;
    END;

    IF (@MontoOriginal <= 0)
    BEGIN
        SET @Mensaje = 'El monto original debe ser mayor a cero.';
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
        INSERT INTO dbo.CuentaPorPagar
        (
            IdentificacionProveedor,
            NumeroDocumento,
            FechaEmision,
            FechaVencimiento,
            Concepto,
            MontoOriginal,
            SaldoActual,
            Estado,
            FechaCreacion,
            FechaModificacion,
            Activo
        )
        VALUES
        (
            @IdentificacionProveedor,
            @NumeroDocumento,
            @FechaEmision,
            @FechaVencimiento,
            @Concepto,
            @MontoOriginal,
            @MontoOriginal,
            @Estado,
            GETDATE(),
            GETDATE(),
            1
        );

        SET @Resultado = 1;
        SET @NumeroDocumentoGenerado = @NumeroDocumento;
        SET @Mensaje = 'Cuenta por pagar registrada correctamente.';
    END TRY
    BEGIN CATCH
        SET @Resultado = 0;
        SET @NumeroDocumentoGenerado = '';
        SET @Mensaje = ERROR_MESSAGE();
    END CATCH;
END;
GO

/* =========================================================
   CUENTA POR PAGAR - EDITAR
   ========================================================= */
CREATE OR ALTER PROCEDURE Cuentas.sp_CuentaPorPagar_Editar
    @IdentificacionProveedor VARCHAR(45),
    @NumeroDocumento VARCHAR(45),
    @FechaEmision DATETIME,
    @FechaVencimiento DATETIME,
    @Concepto VARCHAR(100),
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

    SET @IdentificacionProveedor = LTRIM(RTRIM(ISNULL(@IdentificacionProveedor, '')));
    SET @NumeroDocumento = LTRIM(RTRIM(ISNULL(@NumeroDocumento, '')));
    SET @Concepto = LTRIM(RTRIM(ISNULL(@Concepto, '')));
    SET @Estado = LTRIM(RTRIM(ISNULL(@Estado, '')));

    IF (@IdentificacionProveedor = '' OR @NumeroDocumento = '')
    BEGIN
        SET @Mensaje = 'Debe seleccionar una cuenta por pagar válida.';
        RETURN;
    END;

    IF NOT EXISTS (
        SELECT 1
        FROM dbo.CuentaPorPagar
        WHERE IdentificacionProveedor = @IdentificacionProveedor
          AND NumeroDocumento = @NumeroDocumento
    )
    BEGIN
        SET @Mensaje = 'La cuenta por pagar no existe.';
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

    IF (@Concepto = '')
    BEGIN
        SET @Mensaje = 'Debe ingresar el concepto.';
        RETURN;
    END;

    IF (LEN(@Concepto) > 100)
    BEGIN
        SET @Mensaje = 'El concepto no puede superar los 100 caracteres.';
        RETURN;
    END;

    IF (@MontoOriginal < 0 OR @SaldoActual < 0)
    BEGIN
        SET @Mensaje = 'Los montos no pueden ser negativos.';
        RETURN;
    END;

    SELECT @TotalAbonos = ISNULL(SUM(MontoAbono), 0)
    FROM dbo.AbonoCuentaPorPagar
    WHERE IdentificacionProveedor = @IdentificacionProveedor
      AND NumeroDocumento = @NumeroDocumento
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

    UPDATE dbo.CuentaPorPagar
    SET
        FechaEmision = @FechaEmision,
        FechaVencimiento = @FechaVencimiento,
        Concepto = @Concepto,
        MontoOriginal = @MontoOriginal,
        SaldoActual = @SaldoActual,
        Estado = @Estado,
        FechaModificacion = GETDATE(),
        Activo = @Activo
    WHERE IdentificacionProveedor = @IdentificacionProveedor
      AND NumeroDocumento = @NumeroDocumento;

    SET @Resultado = 1;
    SET @Mensaje = 'Cuenta por pagar actualizada correctamente.';
END;
GO

/* =========================================================
   CUENTA POR PAGAR - RECALCULAR SALDO
   ========================================================= */
CREATE OR ALTER PROCEDURE Cuentas.sp_CuentaPorPagar_RecalcularSaldo
    @IdentificacionProveedor VARCHAR(45),
    @NumeroDocumento VARCHAR(45),
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

    SET @IdentificacionProveedor = LTRIM(RTRIM(ISNULL(@IdentificacionProveedor, '')));
    SET @NumeroDocumento = LTRIM(RTRIM(ISNULL(@NumeroDocumento, '')));

    IF NOT EXISTS (
        SELECT 1
        FROM dbo.CuentaPorPagar
        WHERE IdentificacionProveedor = @IdentificacionProveedor
          AND NumeroDocumento = @NumeroDocumento
    )
    BEGIN
        SET @Mensaje = 'La cuenta por pagar no existe.';
        RETURN;
    END;

    SELECT @MontoOriginal = MontoOriginal
    FROM dbo.CuentaPorPagar
    WHERE IdentificacionProveedor = @IdentificacionProveedor
      AND NumeroDocumento = @NumeroDocumento;

    SELECT @TotalAbonos = ISNULL(SUM(MontoAbono), 0)
    FROM dbo.AbonoCuentaPorPagar
    WHERE IdentificacionProveedor = @IdentificacionProveedor
      AND NumeroDocumento = @NumeroDocumento
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

    UPDATE dbo.CuentaPorPagar
    SET
        SaldoActual = @SaldoActual,
        Estado = @Estado,
        FechaModificacion = GETDATE()
    WHERE IdentificacionProveedor = @IdentificacionProveedor
      AND NumeroDocumento = @NumeroDocumento;

    SET @Resultado = 1;
    SET @Mensaje = 'Saldo de cuenta por pagar recalculado correctamente.';
END;
GO

/* =========================================================
   CUENTA POR PAGAR - INACTIVAR
   ========================================================= */
CREATE OR ALTER PROCEDURE Cuentas.sp_CuentaPorPagar_Inactivar
    @IdentificacionProveedor VARCHAR(45),
    @NumeroDocumento VARCHAR(45),
    @Resultado BIT OUTPUT,
    @Mensaje VARCHAR(500) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    SET @Resultado = 0;
    SET @Mensaje = '';

    SET @IdentificacionProveedor = LTRIM(RTRIM(ISNULL(@IdentificacionProveedor, '')));
    SET @NumeroDocumento = LTRIM(RTRIM(ISNULL(@NumeroDocumento, '')));

    IF (@IdentificacionProveedor = '' OR @NumeroDocumento = '')
    BEGIN
        SET @Mensaje = 'Debe seleccionar una cuenta por pagar válida.';
        RETURN;
    END;

    IF NOT EXISTS (
        SELECT 1
        FROM dbo.CuentaPorPagar
        WHERE IdentificacionProveedor = @IdentificacionProveedor
          AND NumeroDocumento = @NumeroDocumento
    )
    BEGIN
        SET @Mensaje = 'La cuenta por pagar no existe.';
        RETURN;
    END;

    IF EXISTS (
        SELECT 1
        FROM dbo.AbonoCuentaPorPagar
        WHERE IdentificacionProveedor = @IdentificacionProveedor
          AND NumeroDocumento = @NumeroDocumento
          AND Activo = 1
    )
    BEGIN
        SET @Mensaje = 'No se puede inactivar la cuenta por pagar porque tiene abonos activos registrados.';
        RETURN;
    END;

    UPDATE dbo.CuentaPorPagar
    SET
        Activo = 0,
        Estado = 'Anulada',
        FechaModificacion = GETDATE()
    WHERE IdentificacionProveedor = @IdentificacionProveedor
      AND NumeroDocumento = @NumeroDocumento;

    SET @Resultado = 1;
    SET @Mensaje = 'Cuenta por pagar inactivada correctamente.';
END;
GO

/* =========================================================
   CUENTA POR PAGAR - GENERAR NÚMERO DOCUMENTO
   Formato: CXP-2026-000001
   ========================================================= */
CREATE OR ALTER PROCEDURE Cuentas.sp_CuentaPorPagar_GenerarNumeroDocumento
    @Anio INT,
    @NumeroDocumento VARCHAR(45) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @Consecutivo INT;

    SET @NumeroDocumento = '';

    IF (@Anio < 2000 OR @Anio > 2100)
    BEGIN
        SET @Anio = YEAR(GETDATE());
    END;

    SELECT @Consecutivo =
        ISNULL(MAX(TRY_CAST(RIGHT(NumeroDocumento, 6) AS INT)), 0) + 1
    FROM dbo.CuentaPorPagar
    WHERE NumeroDocumento LIKE 'CXP-' + CAST(@Anio AS VARCHAR(4)) + '-%';

    SET @NumeroDocumento =
        'CXP-' + CAST(@Anio AS VARCHAR(4)) + '-' + RIGHT('000000' + CAST(@Consecutivo AS VARCHAR(6)), 6);
END;
GO


/* =========================================================
   CUENTA POR PAGAR - datos
   ========================================================= */

   IF NOT EXISTS (
    SELECT 1
    FROM dbo.ConfiguracionCuentaContable
    WHERE CodigoOperacion = 'CUENTAS_POR_PAGAR'
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
        'CUENTAS_POR_PAGAR',
        'Cuentas por pagar',
        'Cuenta contable utilizada para registrar obligaciones y abonos pendientes de pago a proveedores.',
        '2.01',
        GETDATE(),
        GETDATE(),
        1
    );
END;
GO