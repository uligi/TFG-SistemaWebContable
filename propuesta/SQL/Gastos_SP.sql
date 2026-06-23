IF NOT EXISTS (SELECT 1 FROM sys.schemas WHERE name = 'Movimientos')
BEGIN
    EXEC('CREATE SCHEMA Movimientos');
END;
GO

/* =========================================================
   GASTO - LISTAR
   ========================================================= */
CREATE OR ALTER PROCEDURE Movimientos.sp_Gasto_Listar
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        g.NumeroGasto,
        g.FechaGasto,
        g.Descripcion,
        g.IdTipoGasto,
        tg.Nombre AS TipoGastoNombre,

        g.CodigoCuenta,
        cc.NombreCuenta,

        g.Monto,

        g.IdentificacionProveedor,
        g.NumeroDocumento,
        p.RazonSocial AS ProveedorNombre,

        cxp.Concepto AS ConceptoCuentaPorPagar,
        cxp.MontoOriginal,
        cxp.SaldoActual,
        cxp.Estado AS EstadoCuentaPorPagar,

        g.IdentificacionProveedorAbono,
        g.NumeroDocumentoAbono,
        g.NumeroAbonoCuentaPorPagar,

        a.FechaAbono,
        a.MontoAbono,

        g.NumeroComprobante,
        g.NombreArchivoComprobante,
        g.RutaComprobante,

        g.FechaCreacion,
        g.FechaModificacion,
        g.Activo
    FROM dbo.Gasto g
    INNER JOIN dbo.TipoGasto tg
        ON tg.IdTipoGasto = g.IdTipoGasto
    INNER JOIN dbo.CuentaContable cc
        ON cc.CodigoCuenta = g.CodigoCuenta
    INNER JOIN dbo.Proveedor p
        ON p.IdentificacionProveedor = g.IdentificacionProveedor
    INNER JOIN dbo.CuentaPorPagar cxp
        ON cxp.IdentificacionProveedor = g.IdentificacionProveedor
       AND cxp.NumeroDocumento = g.NumeroDocumento
    INNER JOIN dbo.AbonoCuentaPorPagar a
        ON a.IdentificacionProveedor = g.IdentificacionProveedorAbono
       AND a.NumeroDocumento = g.NumeroDocumentoAbono
       AND a.NumeroAbono = g.NumeroAbonoCuentaPorPagar
    ORDER BY
        g.FechaGasto DESC,
        g.NumeroGasto DESC;
END;
GO

/* =========================================================
   GASTO - LISTAR ACTIVOS
   ========================================================= */
CREATE OR ALTER PROCEDURE Movimientos.sp_Gasto_ListarActivos
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        g.NumeroGasto,
        g.FechaGasto,
        g.Descripcion,
        g.IdTipoGasto,
        tg.Nombre AS TipoGastoNombre,

        g.CodigoCuenta,
        cc.NombreCuenta,

        g.Monto,

        g.IdentificacionProveedor,
        g.NumeroDocumento,
        p.RazonSocial AS ProveedorNombre,

        cxp.Concepto AS ConceptoCuentaPorPagar,
        cxp.MontoOriginal,
        cxp.SaldoActual,
        cxp.Estado AS EstadoCuentaPorPagar,

        g.IdentificacionProveedorAbono,
        g.NumeroDocumentoAbono,
        g.NumeroAbonoCuentaPorPagar,

        a.FechaAbono,
        a.MontoAbono,

        g.NumeroComprobante,
        g.NombreArchivoComprobante,
        g.RutaComprobante,

        g.FechaCreacion,
        g.FechaModificacion,
        g.Activo
    FROM dbo.Gasto g
    INNER JOIN dbo.TipoGasto tg
        ON tg.IdTipoGasto = g.IdTipoGasto
    INNER JOIN dbo.CuentaContable cc
        ON cc.CodigoCuenta = g.CodigoCuenta
    INNER JOIN dbo.Proveedor p
        ON p.IdentificacionProveedor = g.IdentificacionProveedor
    INNER JOIN dbo.CuentaPorPagar cxp
        ON cxp.IdentificacionProveedor = g.IdentificacionProveedor
       AND cxp.NumeroDocumento = g.NumeroDocumento
    INNER JOIN dbo.AbonoCuentaPorPagar a
        ON a.IdentificacionProveedor = g.IdentificacionProveedorAbono
       AND a.NumeroDocumento = g.NumeroDocumentoAbono
       AND a.NumeroAbono = g.NumeroAbonoCuentaPorPagar
    WHERE g.Activo = 1
    ORDER BY
        g.FechaGasto DESC,
        g.NumeroGasto DESC;
END;
GO

/* =========================================================
   GASTO - OBTENER
   ========================================================= */
CREATE OR ALTER PROCEDURE Movimientos.sp_Gasto_Obtener
    @NumeroGasto VARCHAR(45)
AS
BEGIN
    SET NOCOUNT ON;

    SET @NumeroGasto = LTRIM(RTRIM(ISNULL(@NumeroGasto, '')));

    SELECT
        g.NumeroGasto,
        g.FechaGasto,
        g.Descripcion,
        g.IdTipoGasto,
        tg.Nombre AS TipoGastoNombre,

        g.CodigoCuenta,
        cc.NombreCuenta,

        g.Monto,

        g.IdentificacionProveedor,
        g.NumeroDocumento,
        p.RazonSocial AS ProveedorNombre,

        cxp.Concepto AS ConceptoCuentaPorPagar,
        cxp.MontoOriginal,
        cxp.SaldoActual,
        cxp.Estado AS EstadoCuentaPorPagar,

        g.IdentificacionProveedorAbono,
        g.NumeroDocumentoAbono,
        g.NumeroAbonoCuentaPorPagar,

        a.FechaAbono,
        a.MontoAbono,

        g.NumeroComprobante,
        g.NombreArchivoComprobante,
        g.RutaComprobante,

        g.FechaCreacion,
        g.FechaModificacion,
        g.Activo
    FROM dbo.Gasto g
    INNER JOIN dbo.TipoGasto tg
        ON tg.IdTipoGasto = g.IdTipoGasto
    INNER JOIN dbo.CuentaContable cc
        ON cc.CodigoCuenta = g.CodigoCuenta
    INNER JOIN dbo.Proveedor p
        ON p.IdentificacionProveedor = g.IdentificacionProveedor
    INNER JOIN dbo.CuentaPorPagar cxp
        ON cxp.IdentificacionProveedor = g.IdentificacionProveedor
       AND cxp.NumeroDocumento = g.NumeroDocumento
    INNER JOIN dbo.AbonoCuentaPorPagar a
        ON a.IdentificacionProveedor = g.IdentificacionProveedorAbono
       AND a.NumeroDocumento = g.NumeroDocumentoAbono
       AND a.NumeroAbono = g.NumeroAbonoCuentaPorPagar
    WHERE g.NumeroGasto = @NumeroGasto;
END;
GO

/* =========================================================
   GASTO - GENERAR NÚMERO
   Formato: GAS-2026-000001
   ========================================================= */
CREATE OR ALTER PROCEDURE Movimientos.sp_Gasto_GenerarNumero
    @Anio INT,
    @NumeroGasto VARCHAR(45) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @Consecutivo INT;

    SET @NumeroGasto = '';

    IF (@Anio < 2000 OR @Anio > 2100)
    BEGIN
        SET @Anio = YEAR(GETDATE());
    END;

    SELECT @Consecutivo =
        ISNULL(MAX(TRY_CAST(RIGHT(NumeroGasto, 6) AS INT)), 0) + 1
    FROM dbo.Gasto
    WHERE NumeroGasto LIKE 'GAS-' + CAST(@Anio AS VARCHAR(4)) + '-%';

    SET @NumeroGasto =
        'GAS-' + CAST(@Anio AS VARCHAR(4)) + '-' + RIGHT('000000' + CAST(@Consecutivo AS VARCHAR(6)), 6);
END;
GO

/* =========================================================
   GASTO - REGISTRAR
   ========================================================= */
CREATE OR ALTER PROCEDURE Movimientos.sp_Gasto_Registrar
    @FechaGasto DATETIME,
    @Descripcion VARCHAR(150),
    @IdTipoGasto INT,
    @CodigoCuenta VARCHAR(45),
    @Monto DECIMAL(18,2),

    @IdentificacionProveedor VARCHAR(45),
    @NumeroDocumento VARCHAR(45),

    @IdentificacionProveedorAbono VARCHAR(45),
    @NumeroDocumentoAbono VARCHAR(45),
    @NumeroAbonoCuentaPorPagar INT,

    @NumeroComprobante VARCHAR(45),
    @NombreArchivoComprobante VARCHAR(150),
    @RutaComprobante VARCHAR(300),

    @NumeroGastoGenerado VARCHAR(45) OUTPUT,
    @Resultado INT OUTPUT,
    @Mensaje VARCHAR(500) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @NumeroGasto VARCHAR(45);
    DECLARE @Anio INT;

    SET @Resultado = 0;
    SET @Mensaje = '';
    SET @NumeroGastoGenerado = '';

    SET @Descripcion = LTRIM(RTRIM(ISNULL(@Descripcion, '')));
    SET @CodigoCuenta = LTRIM(RTRIM(ISNULL(@CodigoCuenta, '')));

    SET @IdentificacionProveedor = LTRIM(RTRIM(ISNULL(@IdentificacionProveedor, '')));
    SET @NumeroDocumento = LTRIM(RTRIM(ISNULL(@NumeroDocumento, '')));

    SET @IdentificacionProveedorAbono = LTRIM(RTRIM(ISNULL(@IdentificacionProveedorAbono, '')));
    SET @NumeroDocumentoAbono = LTRIM(RTRIM(ISNULL(@NumeroDocumentoAbono, '')));

    SET @NumeroComprobante = LTRIM(RTRIM(ISNULL(@NumeroComprobante, '')));
    SET @NombreArchivoComprobante = LTRIM(RTRIM(ISNULL(@NombreArchivoComprobante, '')));
    SET @RutaComprobante = LTRIM(RTRIM(ISNULL(@RutaComprobante, '')));

    IF (@IdentificacionProveedor = '')
        SET @IdentificacionProveedor = '000000000';

    IF (@NumeroDocumento = '')
        SET @NumeroDocumento = 'CXP-GENERAL-000000';

    IF (@IdentificacionProveedorAbono = '')
        SET @IdentificacionProveedorAbono = '000000000';

    IF (@NumeroDocumentoAbono = '')
        SET @NumeroDocumentoAbono = 'CXP-GENERAL-000000';

    IF (@NumeroAbonoCuentaPorPagar < 0)
        SET @NumeroAbonoCuentaPorPagar = 0;

    IF (@FechaGasto IS NULL)
    BEGIN
        SET @Mensaje = 'Debe ingresar la fecha del gasto.';
        RETURN;
    END;

    IF (@Descripcion = '')
    BEGIN
        SET @Mensaje = 'Debe ingresar una descripción.';
        RETURN;
    END;

    IF (LEN(@Descripcion) > 150)
    BEGIN
        SET @Mensaje = 'La descripción no puede superar los 150 caracteres.';
        RETURN;
    END;

    IF (@IdTipoGasto <= 0)
    BEGIN
        SET @Mensaje = 'Debe seleccionar un tipo de gasto.';
        RETURN;
    END;

    IF NOT EXISTS (
        SELECT 1
        FROM dbo.TipoGasto
        WHERE IdTipoGasto = @IdTipoGasto
          AND Activo = 1
    )
    BEGIN
        SET @Mensaje = 'El tipo de gasto seleccionado no existe o está inactivo.';
        RETURN;
    END;

    IF (@CodigoCuenta = '')
    BEGIN
        SET @Mensaje = 'Debe seleccionar una cuenta contable.';
        RETURN;
    END;

    IF NOT EXISTS (
        SELECT 1
        FROM dbo.CuentaContable
        WHERE CodigoCuenta = @CodigoCuenta
          AND Activo = 1
          AND AceptaMovimientos = 1
    )
    BEGIN
        SET @Mensaje = 'La cuenta contable seleccionada no existe, está inactiva o no acepta movimientos.';
        RETURN;
    END;

    IF (@Monto <= 0)
    BEGIN
        SET @Mensaje = 'El monto del gasto debe ser mayor a cero.';
        RETURN;
    END;

    IF NOT EXISTS (
        SELECT 1
        FROM dbo.Proveedor
        WHERE IdentificacionProveedor = @IdentificacionProveedor
    )
    BEGIN
        SET @Mensaje = 'El proveedor asociado al gasto no existe.';
        RETURN;
    END;

    IF NOT EXISTS (
        SELECT 1
        FROM dbo.CuentaPorPagar
        WHERE IdentificacionProveedor = @IdentificacionProveedor
          AND NumeroDocumento = @NumeroDocumento
    )
    BEGIN
        SET @Mensaje = 'La cuenta por pagar asociada al gasto no existe.';
        RETURN;
    END;

    IF NOT EXISTS (
        SELECT 1
        FROM dbo.AbonoCuentaPorPagar
        WHERE IdentificacionProveedor = @IdentificacionProveedorAbono
          AND NumeroDocumento = @NumeroDocumentoAbono
          AND NumeroAbono = @NumeroAbonoCuentaPorPagar
    )
    BEGIN
        SET @Mensaje = 'El abono asociado al gasto no existe.';
        RETURN;
    END;

    IF (LEN(@NumeroComprobante) > 45)
    BEGIN
        SET @Mensaje = 'El número de comprobante no puede superar los 45 caracteres.';
        RETURN;
    END;

    IF (LEN(@NombreArchivoComprobante) > 150)
    BEGIN
        SET @Mensaje = 'El nombre del archivo no puede superar los 150 caracteres.';
        RETURN;
    END;

    IF (LEN(@RutaComprobante) > 300)
    BEGIN
        SET @Mensaje = 'La ruta del comprobante no puede superar los 300 caracteres.';
        RETURN;
    END;

    BEGIN TRY
        SET @Anio = YEAR(@FechaGasto);

        EXEC Movimientos.sp_Gasto_GenerarNumero
            @Anio = @Anio,
            @NumeroGasto = @NumeroGasto OUTPUT;

        INSERT INTO dbo.Gasto
        (
            NumeroGasto,
            FechaGasto,
            Descripcion,
            IdTipoGasto,
            CodigoCuenta,
            Monto,
            IdentificacionProveedor,
            NumeroDocumento,
            IdentificacionProveedorAbono,
            NumeroDocumentoAbono,
            NumeroAbonoCuentaPorPagar,
            NumeroComprobante,
            NombreArchivoComprobante,
            RutaComprobante,
            FechaCreacion,
            FechaModificacion,
            Activo
        )
        VALUES
        (
            @NumeroGasto,
            @FechaGasto,
            @Descripcion,
            @IdTipoGasto,
            @CodigoCuenta,
            @Monto,
            @IdentificacionProveedor,
            @NumeroDocumento,
            @IdentificacionProveedorAbono,
            @NumeroDocumentoAbono,
            @NumeroAbonoCuentaPorPagar,
            @NumeroComprobante,
            @NombreArchivoComprobante,
            @RutaComprobante,
            GETDATE(),
            GETDATE(),
            1
        );

        SET @Resultado = 1;
        SET @NumeroGastoGenerado = @NumeroGasto;
        SET @Mensaje = 'Gasto registrado correctamente.';
    END TRY
    BEGIN CATCH
        SET @Resultado = 0;
        SET @NumeroGastoGenerado = '';
        SET @Mensaje = ERROR_MESSAGE();
    END CATCH;
END;
GO

/* =========================================================
   GASTO - REGISTRAR AUTOMÁTICO
   Usado por abonos de cuentas por pagar.
   ========================================================= */
CREATE OR ALTER PROCEDURE Movimientos.sp_Gasto_RegistrarAutomatico
    @FechaGasto DATETIME,
    @Descripcion VARCHAR(150),
    @Monto DECIMAL(18,2),

    @IdentificacionProveedor VARCHAR(45),
    @NumeroDocumento VARCHAR(45),

    @IdentificacionProveedorAbono VARCHAR(45),
    @NumeroDocumentoAbono VARCHAR(45),
    @NumeroAbonoCuentaPorPagar INT,

    @Resultado BIT OUTPUT,
    @Mensaje VARCHAR(500) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @IdTipoGasto INT;
    DECLARE @CodigoCuenta VARCHAR(45);
    DECLARE @NumeroGasto VARCHAR(45);
    DECLARE @Anio INT;

    SET @Resultado = 0;
    SET @Mensaje = '';

    SET @Descripcion = LTRIM(RTRIM(ISNULL(@Descripcion, '')));
    SET @IdentificacionProveedor = LTRIM(RTRIM(ISNULL(@IdentificacionProveedor, '')));
    SET @NumeroDocumento = LTRIM(RTRIM(ISNULL(@NumeroDocumento, '')));
    SET @IdentificacionProveedorAbono = LTRIM(RTRIM(ISNULL(@IdentificacionProveedorAbono, '')));
    SET @NumeroDocumentoAbono = LTRIM(RTRIM(ISNULL(@NumeroDocumentoAbono, '')));

    IF (@FechaGasto IS NULL)
        SET @FechaGasto = GETDATE();

    IF (@Descripcion = '')
        SET @Descripcion = 'Gasto automático por abono de cuenta por pagar';

    IF (@Monto <= 0)
    BEGIN
        SET @Mensaje = 'El monto del gasto automático debe ser mayor a cero.';
        RETURN;
    END;

    IF (@IdentificacionProveedor = '')
        SET @IdentificacionProveedor = '000000000';

    IF (@NumeroDocumento = '')
        SET @NumeroDocumento = 'CXP-GENERAL-000000';

    IF (@IdentificacionProveedorAbono = '')
        SET @IdentificacionProveedorAbono = '000000000';

    IF (@NumeroDocumentoAbono = '')
        SET @NumeroDocumentoAbono = 'CXP-GENERAL-000000';

    IF (@NumeroAbonoCuentaPorPagar < 0)
        SET @NumeroAbonoCuentaPorPagar = 0;

    IF NOT EXISTS (
        SELECT 1
        FROM dbo.Proveedor
        WHERE IdentificacionProveedor = @IdentificacionProveedor
    )
    BEGIN
        SET @Mensaje = 'El proveedor asociado al gasto automático no existe.';
        RETURN;
    END;

    IF NOT EXISTS (
        SELECT 1
        FROM dbo.CuentaPorPagar
        WHERE IdentificacionProveedor = @IdentificacionProveedor
          AND NumeroDocumento = @NumeroDocumento
    )
    BEGIN
        SET @Mensaje = 'La cuenta por pagar asociada al gasto automático no existe.';
        RETURN;
    END;

    IF NOT EXISTS (
        SELECT 1
        FROM dbo.AbonoCuentaPorPagar
        WHERE IdentificacionProveedor = @IdentificacionProveedorAbono
          AND NumeroDocumento = @NumeroDocumentoAbono
          AND NumeroAbono = @NumeroAbonoCuentaPorPagar
    )
    BEGIN
        SET @Mensaje = 'El abono asociado al gasto automático no existe.';
        RETURN;
    END;

    IF EXISTS (
        SELECT 1
        FROM dbo.Gasto
        WHERE IdentificacionProveedorAbono = @IdentificacionProveedorAbono
          AND NumeroDocumentoAbono = @NumeroDocumentoAbono
          AND NumeroAbonoCuentaPorPagar = @NumeroAbonoCuentaPorPagar
          AND Activo = 1
    )
    BEGIN
        SET @Resultado = 1;
        SET @Mensaje = 'El gasto automático del abono ya existe.';
        RETURN;
    END;

    /* Tipo de gasto automático */
    SELECT TOP 1 @IdTipoGasto = IdTipoGasto
    FROM dbo.TipoGasto
    WHERE Activo = 1
      AND (
            Nombre COLLATE Latin1_General_CI_AI LIKE '%proveedor%'
         OR Nombre COLLATE Latin1_General_CI_AI LIKE '%compra%'
         OR Nombre COLLATE Latin1_General_CI_AI LIKE '%gasto%'
         OR Nombre COLLATE Latin1_General_CI_AI LIKE '%pago%'
      )
    ORDER BY IdTipoGasto;

    IF (@IdTipoGasto IS NULL)
    BEGIN
        SELECT TOP 1 @IdTipoGasto = IdTipoGasto
        FROM dbo.TipoGasto
        WHERE Activo = 1
        ORDER BY IdTipoGasto;
    END;

    IF (@IdTipoGasto IS NULL)
    BEGIN
        SET @Mensaje = 'No existe un tipo de gasto activo para registrar el gasto automático.';
        RETURN;
    END;

    /* Cuenta contable automática para salida de dinero */
    SELECT TOP 1 @CodigoCuenta = CodigoCuenta
    FROM dbo.CuentaContable
    WHERE Activo = 1
      AND AceptaMovimientos = 1
      AND (
            NombreCuenta COLLATE Latin1_General_CI_AI LIKE '%caja%'
         OR NombreCuenta COLLATE Latin1_General_CI_AI LIKE '%banco%'
         OR NombreCuenta COLLATE Latin1_General_CI_AI LIKE '%efectivo%'
         OR NombreCuenta COLLATE Latin1_General_CI_AI LIKE '%cuenta bancaria%'
      )
    ORDER BY CodigoCuenta;

    IF (@CodigoCuenta IS NULL)
    BEGIN
        SELECT TOP 1 @CodigoCuenta = CodigoCuenta
        FROM dbo.CuentaContable
        WHERE Activo = 1
          AND AceptaMovimientos = 1
        ORDER BY CodigoCuenta;
    END;

    IF (@CodigoCuenta IS NULL)
    BEGIN
        SET @Mensaje = 'No existe una cuenta contable activa que acepte movimientos para registrar el gasto automático.';
        RETURN;
    END;

    BEGIN TRY
        SET @Anio = YEAR(@FechaGasto);

        EXEC Movimientos.sp_Gasto_GenerarNumero
            @Anio = @Anio,
            @NumeroGasto = @NumeroGasto OUTPUT;

        INSERT INTO dbo.Gasto
        (
            NumeroGasto,
            FechaGasto,
            Descripcion,
            IdTipoGasto,
            CodigoCuenta,
            Monto,
            IdentificacionProveedor,
            NumeroDocumento,
            IdentificacionProveedorAbono,
            NumeroDocumentoAbono,
            NumeroAbonoCuentaPorPagar,
            NumeroComprobante,
            NombreArchivoComprobante,
            RutaComprobante,
            FechaCreacion,
            FechaModificacion,
            Activo
        )
        VALUES
        (
            @NumeroGasto,
            @FechaGasto,
            @Descripcion,
            @IdTipoGasto,
            @CodigoCuenta,
            @Monto,
            @IdentificacionProveedor,
            @NumeroDocumento,
            @IdentificacionProveedorAbono,
            @NumeroDocumentoAbono,
            @NumeroAbonoCuentaPorPagar,
            '',
            '',
            '',
            GETDATE(),
            GETDATE(),
            1
        );

        SET @Resultado = 1;
        SET @Mensaje = 'Gasto automático registrado correctamente.';
    END TRY
    BEGIN CATCH
        SET @Resultado = 0;
        SET @Mensaje = ERROR_MESSAGE();
    END CATCH;
END;
GO

/* =========================================================
   GASTO - EDITAR
   ========================================================= */
CREATE OR ALTER PROCEDURE Movimientos.sp_Gasto_Editar
    @NumeroGasto VARCHAR(45),
    @FechaGasto DATETIME,
    @Descripcion VARCHAR(150),
    @IdTipoGasto INT,
    @CodigoCuenta VARCHAR(45),
    @Monto DECIMAL(18,2),
    @NumeroComprobante VARCHAR(45),
    @NombreArchivoComprobante VARCHAR(150),
    @RutaComprobante VARCHAR(300),
    @Activo BIT,
    @Resultado BIT OUTPUT,
    @Mensaje VARCHAR(500) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    SET @Resultado = 0;
    SET @Mensaje = '';

    SET @NumeroGasto = LTRIM(RTRIM(ISNULL(@NumeroGasto, '')));
    SET @Descripcion = LTRIM(RTRIM(ISNULL(@Descripcion, '')));
    SET @CodigoCuenta = LTRIM(RTRIM(ISNULL(@CodigoCuenta, '')));
    SET @NumeroComprobante = LTRIM(RTRIM(ISNULL(@NumeroComprobante, '')));
    SET @NombreArchivoComprobante = LTRIM(RTRIM(ISNULL(@NombreArchivoComprobante, '')));
    SET @RutaComprobante = LTRIM(RTRIM(ISNULL(@RutaComprobante, '')));

    IF (@NumeroGasto = '')
    BEGIN
        SET @Mensaje = 'Debe seleccionar un gasto válido.';
        RETURN;
    END;

    IF NOT EXISTS (
        SELECT 1
        FROM dbo.Gasto
        WHERE NumeroGasto = @NumeroGasto
    )
    BEGIN
        SET @Mensaje = 'El gasto seleccionado no existe.';
        RETURN;
    END;

    IF (@FechaGasto IS NULL)
    BEGIN
        SET @Mensaje = 'Debe ingresar la fecha del gasto.';
        RETURN;
    END;

    IF (@Descripcion = '')
    BEGIN
        SET @Mensaje = 'Debe ingresar una descripción.';
        RETURN;
    END;

    IF (LEN(@Descripcion) > 150)
    BEGIN
        SET @Mensaje = 'La descripción no puede superar los 150 caracteres.';
        RETURN;
    END;

    IF (@IdTipoGasto <= 0)
    BEGIN
        SET @Mensaje = 'Debe seleccionar un tipo de gasto.';
        RETURN;
    END;

    IF NOT EXISTS (
        SELECT 1
        FROM dbo.TipoGasto
        WHERE IdTipoGasto = @IdTipoGasto
          AND Activo = 1
    )
    BEGIN
        SET @Mensaje = 'El tipo de gasto seleccionado no existe o está inactivo.';
        RETURN;
    END;

    IF (@CodigoCuenta = '')
    BEGIN
        SET @Mensaje = 'Debe seleccionar una cuenta contable.';
        RETURN;
    END;

    IF NOT EXISTS (
        SELECT 1
        FROM dbo.CuentaContable
        WHERE CodigoCuenta = @CodigoCuenta
          AND Activo = 1
          AND AceptaMovimientos = 1
    )
    BEGIN
        SET @Mensaje = 'La cuenta contable seleccionada no existe, está inactiva o no acepta movimientos.';
        RETURN;
    END;

    IF (@Monto <= 0 AND @Activo = 1)
    BEGIN
        SET @Mensaje = 'El monto del gasto debe ser mayor a cero.';
        RETURN;
    END;

    IF (LEN(@NumeroComprobante) > 45)
    BEGIN
        SET @Mensaje = 'El número de comprobante no puede superar los 45 caracteres.';
        RETURN;
    END;

    IF (LEN(@NombreArchivoComprobante) > 150)
    BEGIN
        SET @Mensaje = 'El nombre del archivo no puede superar los 150 caracteres.';
        RETURN;
    END;

    IF (LEN(@RutaComprobante) > 300)
    BEGIN
        SET @Mensaje = 'La ruta del comprobante no puede superar los 300 caracteres.';
        RETURN;
    END;

    UPDATE dbo.Gasto
    SET
        FechaGasto = @FechaGasto,
        Descripcion = @Descripcion,
        IdTipoGasto = @IdTipoGasto,
        CodigoCuenta = @CodigoCuenta,
        Monto = @Monto,
        NumeroComprobante = @NumeroComprobante,
        NombreArchivoComprobante = @NombreArchivoComprobante,
        RutaComprobante = @RutaComprobante,
        FechaModificacion = GETDATE(),
        Activo = @Activo
    WHERE NumeroGasto = @NumeroGasto;

    SET @Resultado = 1;
    SET @Mensaje = 'Gasto actualizado correctamente.';
END;
GO

/* =========================================================
   GASTO - INACTIVAR
   ========================================================= */
CREATE OR ALTER PROCEDURE Movimientos.sp_Gasto_Inactivar
    @NumeroGasto VARCHAR(45),
    @Resultado BIT OUTPUT,
    @Mensaje VARCHAR(500) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    SET @Resultado = 0;
    SET @Mensaje = '';

    SET @NumeroGasto = LTRIM(RTRIM(ISNULL(@NumeroGasto, '')));

    IF (@NumeroGasto = '')
    BEGIN
        SET @Mensaje = 'Debe seleccionar un gasto válido.';
        RETURN;
    END;

    IF NOT EXISTS (
        SELECT 1
        FROM dbo.Gasto
        WHERE NumeroGasto = @NumeroGasto
          AND Activo = 1
    )
    BEGIN
        SET @Mensaje = 'El gasto no existe o ya está inactivo.';
        RETURN;
    END;

    UPDATE dbo.Gasto
    SET
        Activo = 0,
        FechaModificacion = GETDATE()
    WHERE NumeroGasto = @NumeroGasto;

    SET @Resultado = 1;
    SET @Mensaje = 'Gasto inactivado correctamente.';
END;
GO