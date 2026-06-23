IF NOT EXISTS (SELECT 1 FROM sys.schemas WHERE name = 'Contabilidad')
BEGIN
    EXEC('CREATE SCHEMA Contabilidad');
END;
GO

/* =========================================================
   CONFIGURACIONES BASE
   Nota:
   Si existe alguna cuenta activa que acepta movimientos, se usa como
   cuenta temporal para crear configuraciones iniciales.
   Después el usuario las cambia desde la vista.
   ========================================================= */
DECLARE @CodigoCuentaDefault VARCHAR(45);

SELECT TOP 1
    @CodigoCuentaDefault = CodigoCuenta
FROM dbo.CuentaContable
WHERE Activo = 1
  AND AceptaMovimientos = 1
  AND CodigoCuenta <> '0'
ORDER BY CodigoCuenta;

IF (@CodigoCuentaDefault IS NOT NULL)
BEGIN
    IF NOT EXISTS (
        SELECT 1
        FROM dbo.ConfiguracionCuentaContable
        WHERE CodigoOperacion = 'VENTA_CONTADO'
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
            'VENTA_CONTADO',
            'Venta de contado',
            'Cuenta contable utilizada para ingresos generados por facturas de contado.',
            @CodigoCuentaDefault,
            GETDATE(),
            GETDATE(),
            1
        );
    END;

    IF NOT EXISTS (
        SELECT 1
        FROM dbo.ConfiguracionCuentaContable
        WHERE CodigoOperacion = 'VENTA_CREDITO'
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
            'VENTA_CREDITO',
            'Venta a crédito',
            'Cuenta contable utilizada para registrar ventas a crédito.',
            @CodigoCuentaDefault,
            GETDATE(),
            GETDATE(),
            1
        );
    END;

    IF NOT EXISTS (
        SELECT 1
        FROM dbo.ConfiguracionCuentaContable
        WHERE CodigoOperacion = 'ABONO_CXC'
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
            'ABONO_CXC',
            'Abono cuenta por cobrar',
            'Cuenta contable utilizada para ingresos generados por abonos de cuentas por cobrar.',
            @CodigoCuentaDefault,
            GETDATE(),
            GETDATE(),
            1
        );
    END;

    IF NOT EXISTS (
        SELECT 1
        FROM dbo.ConfiguracionCuentaContable
        WHERE CodigoOperacion = 'GASTO_CONTADO'
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
            'GASTO_CONTADO',
            'Gasto de contado',
            'Cuenta contable utilizada para gastos pagados de contado.',
            @CodigoCuentaDefault,
            GETDATE(),
            GETDATE(),
            1
        );
    END;

    IF NOT EXISTS (
        SELECT 1
        FROM dbo.ConfiguracionCuentaContable
        WHERE CodigoOperacion = 'GASTO_CREDITO'
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
            'GASTO_CREDITO',
            'Gasto a crédito',
            'Cuenta contable utilizada para registrar gastos a crédito.',
            @CodigoCuentaDefault,
            GETDATE(),
            GETDATE(),
            1
        );
    END;

    IF NOT EXISTS (
        SELECT 1
        FROM dbo.ConfiguracionCuentaContable
        WHERE CodigoOperacion = 'PAGO_CXP'
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
            'PAGO_CXP',
            'Pago cuenta por pagar',
            'Cuenta contable utilizada para registrar pagos de cuentas por pagar.',
            @CodigoCuentaDefault,
            GETDATE(),
            GETDATE(),
            1
        );
    END;
END;
GO

/* =========================================================
   CONFIGURACION CUENTA CONTABLE - LISTAR
   ========================================================= */
CREATE OR ALTER PROCEDURE Contabilidad.sp_ConfiguracionCuentaContable_Listar
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        ccc.CodigoOperacion,
        ccc.NombreOperacion,
        ccc.Descripcion,
        ccc.CodigoCuenta,
        cc.NombreCuenta,
        cc.AceptaMovimientos,
        cc.Activo AS CuentaActiva,
        ccc.FechaCreacion,
        ccc.FechaModificacion,
        ccc.Activo
    FROM dbo.ConfiguracionCuentaContable ccc
    INNER JOIN dbo.CuentaContable cc
        ON cc.CodigoCuenta = ccc.CodigoCuenta
    ORDER BY
        ccc.NombreOperacion;
END;
GO

/* =========================================================
   CONFIGURACION CUENTA CONTABLE - OBTENER POR OPERACION
   ========================================================= */
CREATE OR ALTER PROCEDURE Contabilidad.sp_ConfiguracionCuentaContable_ObtenerPorOperacion
    @CodigoOperacion VARCHAR(45)
AS
BEGIN
    SET NOCOUNT ON;

    SET @CodigoOperacion = LTRIM(RTRIM(ISNULL(@CodigoOperacion, '')));

    SELECT TOP 1
        ccc.CodigoOperacion,
        ccc.NombreOperacion,
        ccc.Descripcion,
        ccc.CodigoCuenta,
        cc.NombreCuenta,
        cc.AceptaMovimientos,
        cc.Activo AS CuentaActiva,
        ccc.FechaCreacion,
        ccc.FechaModificacion,
        ccc.Activo
    FROM dbo.ConfiguracionCuentaContable ccc
    INNER JOIN dbo.CuentaContable cc
        ON cc.CodigoCuenta = ccc.CodigoCuenta
    WHERE ccc.CodigoOperacion = @CodigoOperacion;
END;
GO

/* =========================================================
   CONFIGURACION CUENTA CONTABLE - OBTENER ACTIVA POR OPERACION
   Para procesos automáticos.
   ========================================================= */
CREATE OR ALTER PROCEDURE Contabilidad.sp_ConfiguracionCuentaContable_ObtenerActivaPorOperacion
    @CodigoOperacion VARCHAR(45)
AS
BEGIN
    SET NOCOUNT ON;

    SET @CodigoOperacion = LTRIM(RTRIM(ISNULL(@CodigoOperacion, '')));

    SELECT TOP 1
        ccc.CodigoOperacion,
        ccc.NombreOperacion,
        ccc.Descripcion,
        ccc.CodigoCuenta,
        cc.NombreCuenta,
        cc.AceptaMovimientos,
        cc.Activo AS CuentaActiva,
        ccc.FechaCreacion,
        ccc.FechaModificacion,
        ccc.Activo
    FROM dbo.ConfiguracionCuentaContable ccc
    INNER JOIN dbo.CuentaContable cc
        ON cc.CodigoCuenta = ccc.CodigoCuenta
    WHERE ccc.CodigoOperacion = @CodigoOperacion
      AND ccc.Activo = 1
      AND cc.Activo = 1
      AND cc.AceptaMovimientos = 1;
END;
GO

/* =========================================================
   CONFIGURACION CUENTA CONTABLE - REGISTRAR
   ========================================================= */
CREATE OR ALTER PROCEDURE Contabilidad.sp_ConfiguracionCuentaContable_Registrar
    @CodigoOperacion VARCHAR(45),
    @NombreOperacion VARCHAR(100),
    @Descripcion VARCHAR(200),
    @CodigoCuenta VARCHAR(45),
    @Resultado INT OUTPUT,
    @Mensaje VARCHAR(500) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    SET @Resultado = 0;
    SET @Mensaje = '';

    SET @CodigoOperacion = UPPER(LTRIM(RTRIM(ISNULL(@CodigoOperacion, ''))));
    SET @NombreOperacion = LTRIM(RTRIM(ISNULL(@NombreOperacion, '')));
    SET @Descripcion = LTRIM(RTRIM(ISNULL(@Descripcion, '')));
    SET @CodigoCuenta = LTRIM(RTRIM(ISNULL(@CodigoCuenta, '')));

    IF (@CodigoOperacion = '')
    BEGIN
        SET @Mensaje = 'El código de operación es obligatorio.';
        RETURN;
    END;

    IF (LEN(@CodigoOperacion) > 45)
    BEGIN
        SET @Mensaje = 'El código de operación no puede superar los 45 caracteres.';
        RETURN;
    END;

    IF (@NombreOperacion = '')
    BEGIN
        SET @Mensaje = 'El nombre de operación es obligatorio.';
        RETURN;
    END;

    IF (LEN(@NombreOperacion) > 100)
    BEGIN
        SET @Mensaje = 'El nombre de operación no puede superar los 100 caracteres.';
        RETURN;
    END;

    IF (LEN(@Descripcion) > 200)
    BEGIN
        SET @Mensaje = 'La descripción no puede superar los 200 caracteres.';
        RETURN;
    END;

    IF (@CodigoCuenta = '')
    BEGIN
        SET @Mensaje = 'Debe seleccionar una cuenta contable.';
        RETURN;
    END;

    IF EXISTS (
        SELECT 1
        FROM dbo.ConfiguracionCuentaContable
        WHERE CodigoOperacion = @CodigoOperacion
    )
    BEGIN
        SET @Mensaje = 'Ya existe una configuración para ese código de operación.';
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

    BEGIN TRY
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
            @CodigoOperacion,
            @NombreOperacion,
            @Descripcion,
            @CodigoCuenta,
            GETDATE(),
            GETDATE(),
            1
        );

        SET @Resultado = 1;
        SET @Mensaje = 'Configuración contable registrada correctamente.';
    END TRY
    BEGIN CATCH
        SET @Resultado = 0;
        SET @Mensaje = ERROR_MESSAGE();
    END CATCH;
END;
GO

/* =========================================================
   CONFIGURACION CUENTA CONTABLE - EDITAR
   Nota:
   CodigoOperacion es PK. No se cambia.
   ========================================================= */
CREATE OR ALTER PROCEDURE Contabilidad.sp_ConfiguracionCuentaContable_Editar
    @CodigoOperacion VARCHAR(45),
    @NombreOperacion VARCHAR(100),
    @Descripcion VARCHAR(200),
    @CodigoCuenta VARCHAR(45),
    @Activo BIT,
    @Resultado BIT OUTPUT,
    @Mensaje VARCHAR(500) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    SET @Resultado = 0;
    SET @Mensaje = '';

    SET @CodigoOperacion = UPPER(LTRIM(RTRIM(ISNULL(@CodigoOperacion, ''))));
    SET @NombreOperacion = LTRIM(RTRIM(ISNULL(@NombreOperacion, '')));
    SET @Descripcion = LTRIM(RTRIM(ISNULL(@Descripcion, '')));
    SET @CodigoCuenta = LTRIM(RTRIM(ISNULL(@CodigoCuenta, '')));

    IF (@CodigoOperacion = '')
    BEGIN
        SET @Mensaje = 'Debe seleccionar una configuración válida.';
        RETURN;
    END;

    IF NOT EXISTS (
        SELECT 1
        FROM dbo.ConfiguracionCuentaContable
        WHERE CodigoOperacion = @CodigoOperacion
    )
    BEGIN
        SET @Mensaje = 'La configuración contable no existe.';
        RETURN;
    END;

    IF (@NombreOperacion = '')
    BEGIN
        SET @Mensaje = 'El nombre de operación es obligatorio.';
        RETURN;
    END;

    IF (LEN(@NombreOperacion) > 100)
    BEGIN
        SET @Mensaje = 'El nombre de operación no puede superar los 100 caracteres.';
        RETURN;
    END;

    IF (LEN(@Descripcion) > 200)
    BEGIN
        SET @Mensaje = 'La descripción no puede superar los 200 caracteres.';
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

    UPDATE dbo.ConfiguracionCuentaContable
    SET
        NombreOperacion = @NombreOperacion,
        Descripcion = @Descripcion,
        CodigoCuenta = @CodigoCuenta,
        FechaModificacion = GETDATE(),
        Activo = @Activo
    WHERE CodigoOperacion = @CodigoOperacion;

    SET @Resultado = 1;
    SET @Mensaje = 'Configuración contable actualizada correctamente.';
END;
GO

/* =========================================================
   CONFIGURACION CUENTA CONTABLE - INACTIVAR
   ========================================================= */
CREATE OR ALTER PROCEDURE Contabilidad.sp_ConfiguracionCuentaContable_Inactivar
    @CodigoOperacion VARCHAR(45),
    @Resultado BIT OUTPUT,
    @Mensaje VARCHAR(500) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    SET @Resultado = 0;
    SET @Mensaje = '';

    SET @CodigoOperacion = UPPER(LTRIM(RTRIM(ISNULL(@CodigoOperacion, ''))));

    IF (@CodigoOperacion = '')
    BEGIN
        SET @Mensaje = 'Debe seleccionar una configuración válida.';
        RETURN;
    END;

    IF NOT EXISTS (
        SELECT 1
        FROM dbo.ConfiguracionCuentaContable
        WHERE CodigoOperacion = @CodigoOperacion
    )
    BEGIN
        SET @Mensaje = 'La configuración contable no existe.';
        RETURN;
    END;

    UPDATE dbo.ConfiguracionCuentaContable
    SET
        Activo = 0,
        FechaModificacion = GETDATE()
    WHERE CodigoOperacion = @CodigoOperacion;

    SET @Resultado = 1;
    SET @Mensaje = 'Configuración contable inactivada correctamente.';
END;
GO


/* =========================================================
   CONFIGURACIONES CONTABLES MÍNIMAS PARA ASIENTOS AUTOMÁTICOS
   Versión segura: usa cuentas reales existentes.
   ========================================================= */

DECLARE @CuentaCaja VARCHAR(45);
DECLARE @CuentaIngreso VARCHAR(45);
DECLARE @CuentaGasto VARCHAR(45);

/* Caja / bancos */
SELECT TOP 1
    @CuentaCaja = CodigoCuenta
FROM dbo.CuentaContable
WHERE Activo = 1
  AND AceptaMovimientos = 1
  AND (
        NombreCuenta LIKE '%Caja%'
        OR NombreCuenta LIKE '%Banco%'
        OR NombreCuenta LIKE '%Bancos%'
      )
ORDER BY CodigoCuenta;

/* Ingresos / ventas */
SELECT TOP 1
    @CuentaIngreso = CodigoCuenta
FROM dbo.CuentaContable
WHERE Activo = 1
  AND AceptaMovimientos = 1
  AND (
        NombreCuenta LIKE '%Ingreso%'
        OR NombreCuenta LIKE '%Ingresos%'
        OR NombreCuenta LIKE '%Venta%'
        OR NombreCuenta LIKE '%Ventas%'
      )
ORDER BY CodigoCuenta;

/* Gastos */
SELECT TOP 1
    @CuentaGasto = CodigoCuenta
FROM dbo.CuentaContable
WHERE Activo = 1
  AND AceptaMovimientos = 1
  AND (
        NombreCuenta LIKE '%Gasto%'
        OR NombreCuenta LIKE '%Gastos%'
      )
ORDER BY CodigoCuenta;

/* Respaldo: si no encuentra una cuenta por nombre, usa cualquier cuenta activa que acepte movimientos */
IF @CuentaCaja IS NULL
BEGIN
    SELECT TOP 1 @CuentaCaja = CodigoCuenta
    FROM dbo.CuentaContable
    WHERE Activo = 1
      AND AceptaMovimientos = 1
    ORDER BY CodigoCuenta;
END;

IF @CuentaIngreso IS NULL
BEGIN
    SELECT TOP 1 @CuentaIngreso = CodigoCuenta
    FROM dbo.CuentaContable
    WHERE Activo = 1
      AND AceptaMovimientos = 1
    ORDER BY CodigoCuenta;
END;

IF @CuentaGasto IS NULL
BEGIN
    SELECT TOP 1 @CuentaGasto = CodigoCuenta
    FROM dbo.CuentaContable
    WHERE Activo = 1
      AND AceptaMovimientos = 1
    ORDER BY CodigoCuenta;
END;

/* Validación */
IF @CuentaCaja IS NULL OR @CuentaIngreso IS NULL OR @CuentaGasto IS NULL
BEGIN
    RAISERROR('No existen cuentas contables activas que acepten movimientos. Cree al menos una cuenta contable válida antes de insertar configuraciones.', 16, 1);
    RETURN;
END;

/* CAJA_BANCOS */
IF NOT EXISTS (
    SELECT 1
    FROM dbo.ConfiguracionCuentaContable
    WHERE CodigoOperacion = 'CAJA_BANCOS'
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
        'CAJA_BANCOS',
        'Caja o bancos',
        'Cuenta usada para entradas y salidas de efectivo.',
        @CuentaCaja,
        GETDATE(),
        GETDATE(),
        1
    );
END;

/* INGRESOS_VARIOS */
IF NOT EXISTS (
    SELECT 1
    FROM dbo.ConfiguracionCuentaContable
    WHERE CodigoOperacion = 'INGRESOS_VARIOS'
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
        'INGRESOS_VARIOS',
        'Ingresos varios',
        'Cuenta contable genérica para ingresos manuales.',
        @CuentaIngreso,
        GETDATE(),
        GETDATE(),
        1
    );
END;

/* GASTOS_OPERATIVOS */
IF NOT EXISTS (
    SELECT 1
    FROM dbo.ConfiguracionCuentaContable
    WHERE CodigoOperacion = 'GASTOS_OPERATIVOS'
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
        'GASTOS_OPERATIVOS',
        'Gastos operativos',
        'Cuenta contable genérica para gastos del negocio.',
        @CuentaGasto,
        GETDATE(),
        GETDATE(),
        1
    );
END;
GO