IF NOT EXISTS (SELECT 1 FROM sys.schemas WHERE name = 'Contabilidad')
BEGIN
    EXEC('CREATE SCHEMA Contabilidad');
END;
GO

/* =========================================================
   CUENTA RAÍZ REQUERIDA
   Como CodigoCuentaPadre es NOT NULL y tiene FK a CuentaContable,
   las cuentas principales necesitan apuntar a una raíz.
   ========================================================= */
IF NOT EXISTS (
    SELECT 1
    FROM dbo.CuentaContable
    WHERE CodigoCuenta = '0'
)
BEGIN
    INSERT INTO dbo.CuentaContable
    (
        CodigoCuenta,
        NombreCuenta,
        IdTipoCuentaContable,
        IdNaturalezaCuentaContable,
        AceptaMovimientos,
        Activo,
        CodigoCuentaPadre
    )
    VALUES
    (
        '0',
        'Cuenta raíz',
        1,
        1,
        0,
        1,
        '0'
    );
END;
GO

/* =========================================================
   CUENTA CONTABLE - LISTAR
   ========================================================= */
CREATE OR ALTER PROCEDURE Contabilidad.sp_CuentaContable_Listar
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        cc.CodigoCuenta,
        cc.NombreCuenta,
        cc.IdTipoCuentaContable,
        tcc.Nombre AS TipoCuentaContableNombre,
        cc.IdNaturalezaCuentaContable,
        ncc.Naturaleza AS NaturalezaCuentaContableNombre,
        cc.AceptaMovimientos,
        cc.Activo,
        cc.CodigoCuentaPadre,
        cp.NombreCuenta AS NombreCuentaPadre
    FROM dbo.CuentaContable cc
    INNER JOIN dbo.TipoCuentaContable tcc
        ON tcc.IdTipoCuentaContable = cc.IdTipoCuentaContable
    INNER JOIN dbo.NaturalezaCuentaContable ncc
        ON ncc.IdNaturalezaCuentaContable = cc.IdNaturalezaCuentaContable
    LEFT JOIN dbo.CuentaContable cp
        ON cp.CodigoCuenta = cc.CodigoCuentaPadre
    ORDER BY
        cc.CodigoCuenta;
END;
GO

/* =========================================================
   CUENTA CONTABLE - LISTAR ACTIVAS
   ========================================================= */
CREATE OR ALTER PROCEDURE Contabilidad.sp_CuentaContable_ListarActivas
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        cc.CodigoCuenta,
        cc.NombreCuenta,
        cc.IdTipoCuentaContable,
        tcc.Nombre AS TipoCuentaContableNombre,
        cc.IdNaturalezaCuentaContable,
        ncc.Naturaleza AS NaturalezaCuentaContableNombre,
        cc.AceptaMovimientos,
        cc.Activo,
        cc.CodigoCuentaPadre,
        cp.NombreCuenta AS NombreCuentaPadre
    FROM dbo.CuentaContable cc
    INNER JOIN dbo.TipoCuentaContable tcc
        ON tcc.IdTipoCuentaContable = cc.IdTipoCuentaContable
    INNER JOIN dbo.NaturalezaCuentaContable ncc
        ON ncc.IdNaturalezaCuentaContable = cc.IdNaturalezaCuentaContable
    LEFT JOIN dbo.CuentaContable cp
        ON cp.CodigoCuenta = cc.CodigoCuentaPadre
    WHERE cc.Activo = 1
    ORDER BY
        cc.CodigoCuenta;
END;
GO

/* =========================================================
   CUENTA CONTABLE - LISTAR PARA MOVIMIENTOS
   Solo cuentas activas que aceptan movimientos.
   ========================================================= */
CREATE OR ALTER PROCEDURE Contabilidad.sp_CuentaContable_ListarParaMovimientos
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        cc.CodigoCuenta,
        cc.NombreCuenta,
        cc.IdTipoCuentaContable,
        tcc.Nombre AS TipoCuentaContableNombre,
        cc.IdNaturalezaCuentaContable,
        ncc.Naturaleza AS NaturalezaCuentaContableNombre,
        cc.AceptaMovimientos,
        cc.Activo,
        cc.CodigoCuentaPadre,
        cp.NombreCuenta AS NombreCuentaPadre
    FROM dbo.CuentaContable cc
    INNER JOIN dbo.TipoCuentaContable tcc
        ON tcc.IdTipoCuentaContable = cc.IdTipoCuentaContable
    INNER JOIN dbo.NaturalezaCuentaContable ncc
        ON ncc.IdNaturalezaCuentaContable = cc.IdNaturalezaCuentaContable
    LEFT JOIN dbo.CuentaContable cp
        ON cp.CodigoCuenta = cc.CodigoCuentaPadre
    WHERE cc.Activo = 1
      AND cc.AceptaMovimientos = 1
      AND cc.CodigoCuenta <> '0'
    ORDER BY
        cc.CodigoCuenta;
END;
GO

/* =========================================================
   CUENTA CONTABLE - OBTENER
   ========================================================= */
CREATE OR ALTER PROCEDURE Contabilidad.sp_CuentaContable_Obtener
    @CodigoCuenta VARCHAR(45)
AS
BEGIN
    SET NOCOUNT ON;

    SET @CodigoCuenta = LTRIM(RTRIM(ISNULL(@CodigoCuenta, '')));

    SELECT
        cc.CodigoCuenta,
        cc.NombreCuenta,
        cc.IdTipoCuentaContable,
        tcc.Nombre AS TipoCuentaContableNombre,
        cc.IdNaturalezaCuentaContable,
        ncc.Naturaleza AS NaturalezaCuentaContableNombre,
        cc.AceptaMovimientos,
        cc.Activo,
        cc.CodigoCuentaPadre,
        cp.NombreCuenta AS NombreCuentaPadre
    FROM dbo.CuentaContable cc
    INNER JOIN dbo.TipoCuentaContable tcc
        ON tcc.IdTipoCuentaContable = cc.IdTipoCuentaContable
    INNER JOIN dbo.NaturalezaCuentaContable ncc
        ON ncc.IdNaturalezaCuentaContable = cc.IdNaturalezaCuentaContable
    LEFT JOIN dbo.CuentaContable cp
        ON cp.CodigoCuenta = cc.CodigoCuentaPadre
    WHERE cc.CodigoCuenta = @CodigoCuenta;
END;
GO

/* =========================================================
   CUENTA CONTABLE - REGISTRAR
   ========================================================= */
CREATE OR ALTER PROCEDURE Contabilidad.sp_CuentaContable_Registrar
    @CodigoCuenta VARCHAR(45),
    @NombreCuenta VARCHAR(100),
    @IdTipoCuentaContable INT,
    @IdNaturalezaCuentaContable INT,
    @AceptaMovimientos BIT,
    @CodigoCuentaPadre VARCHAR(45),
    @Resultado INT OUTPUT,
    @Mensaje VARCHAR(500) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    SET @Resultado = 0;
    SET @Mensaje = '';

    SET @CodigoCuenta = LTRIM(RTRIM(ISNULL(@CodigoCuenta, '')));
    SET @NombreCuenta = LTRIM(RTRIM(ISNULL(@NombreCuenta, '')));
    SET @CodigoCuentaPadre = LTRIM(RTRIM(ISNULL(@CodigoCuentaPadre, '')));

    IF (@CodigoCuenta = '')
    BEGIN
        SET @Mensaje = 'El código de cuenta es obligatorio.';
        RETURN;
    END;

    IF (LEN(@CodigoCuenta) > 45)
    BEGIN
        SET @Mensaje = 'El código de cuenta no puede superar los 45 caracteres.';
        RETURN;
    END;

    IF (@CodigoCuenta = '0')
    BEGIN
        SET @Mensaje = 'No se puede registrar otra cuenta con el código raíz 0.';
        RETURN;
    END;

    IF (@NombreCuenta = '')
    BEGIN
        SET @Mensaje = 'El nombre de la cuenta es obligatorio.';
        RETURN;
    END;

    IF (LEN(@NombreCuenta) > 100)
    BEGIN
        SET @Mensaje = 'El nombre de la cuenta no puede superar los 100 caracteres.';
        RETURN;
    END;

    IF (@IdTipoCuentaContable <= 0)
    BEGIN
        SET @Mensaje = 'Debe seleccionar un tipo de cuenta contable.';
        RETURN;
    END;

    IF (@IdNaturalezaCuentaContable <= 0)
    BEGIN
        SET @Mensaje = 'Debe seleccionar una naturaleza de cuenta contable.';
        RETURN;
    END;

    IF (@CodigoCuentaPadre = '')
    BEGIN
        SET @CodigoCuentaPadre = '0';
    END;

    IF EXISTS (
        SELECT 1
        FROM dbo.CuentaContable
        WHERE CodigoCuenta = @CodigoCuenta
    )
    BEGIN
        SET @Mensaje = 'Ya existe una cuenta contable con ese código.';
        RETURN;
    END;

    IF EXISTS (
        SELECT 1
        FROM dbo.CuentaContable
        WHERE UPPER(LTRIM(RTRIM(NombreCuenta))) = UPPER(@NombreCuenta)
    )
    BEGIN
        SET @Mensaje = 'Ya existe una cuenta contable con ese nombre.';
        RETURN;
    END;

    IF NOT EXISTS (
        SELECT 1
        FROM dbo.TipoCuentaContable
        WHERE IdTipoCuentaContable = @IdTipoCuentaContable
          AND Activo = 1
    )
    BEGIN
        SET @Mensaje = 'El tipo de cuenta contable seleccionado no existe o está inactivo.';
        RETURN;
    END;

    IF NOT EXISTS (
        SELECT 1
        FROM dbo.NaturalezaCuentaContable
        WHERE IdNaturalezaCuentaContable = @IdNaturalezaCuentaContable
          AND Activo = 1
    )
    BEGIN
        SET @Mensaje = 'La naturaleza de cuenta contable seleccionada no existe o está inactiva.';
        RETURN;
    END;

    IF NOT EXISTS (
        SELECT 1
        FROM dbo.CuentaContable
        WHERE CodigoCuenta = @CodigoCuentaPadre
          AND Activo = 1
    )
    BEGIN
        SET @Mensaje = 'La cuenta padre seleccionada no existe o está inactiva.';
        RETURN;
    END;

    IF (@CodigoCuentaPadre <> '0')
    BEGIN
        IF EXISTS (
            SELECT 1
            FROM dbo.CuentaContable
            WHERE CodigoCuenta = @CodigoCuentaPadre
              AND AceptaMovimientos = 1
        )
        BEGIN
            SET @Mensaje = 'La cuenta padre no puede aceptar movimientos si tendrá cuentas hijas.';
            RETURN;
        END;
    END;

    BEGIN TRY
        INSERT INTO dbo.CuentaContable
        (
            CodigoCuenta,
            NombreCuenta,
            IdTipoCuentaContable,
            IdNaturalezaCuentaContable,
            AceptaMovimientos,
            Activo,
            CodigoCuentaPadre
        )
        VALUES
        (
            @CodigoCuenta,
            @NombreCuenta,
            @IdTipoCuentaContable,
            @IdNaturalezaCuentaContable,
            @AceptaMovimientos,
            1,
            @CodigoCuentaPadre
        );

        SET @Resultado = 1;
        SET @Mensaje = 'Cuenta contable registrada correctamente.';
    END TRY
    BEGIN CATCH
        SET @Resultado = 0;
        SET @Mensaje = ERROR_MESSAGE();
    END CATCH;
END;
GO

/* =========================================================
   CUENTA CONTABLE - EDITAR
   Nota:
   CodigoCuenta es PK. No se debe cambiar.
   ========================================================= */
CREATE OR ALTER PROCEDURE Contabilidad.sp_CuentaContable_Editar
    @CodigoCuenta VARCHAR(45),
    @NombreCuenta VARCHAR(100),
    @IdTipoCuentaContable INT,
    @IdNaturalezaCuentaContable INT,
    @AceptaMovimientos BIT,
    @Activo BIT,
    @CodigoCuentaPadre VARCHAR(45),
    @Resultado BIT OUTPUT,
    @Mensaje VARCHAR(500) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    SET @Resultado = 0;
    SET @Mensaje = '';

    SET @CodigoCuenta = LTRIM(RTRIM(ISNULL(@CodigoCuenta, '')));
    SET @NombreCuenta = LTRIM(RTRIM(ISNULL(@NombreCuenta, '')));
    SET @CodigoCuentaPadre = LTRIM(RTRIM(ISNULL(@CodigoCuentaPadre, '')));

    IF (@CodigoCuenta = '')
    BEGIN
        SET @Mensaje = 'Debe seleccionar una cuenta contable válida.';
        RETURN;
    END;

    IF (@CodigoCuenta = '0')
    BEGIN
        SET @Mensaje = 'No se puede editar la cuenta raíz del sistema.';
        RETURN;
    END;

    IF NOT EXISTS (
        SELECT 1
        FROM dbo.CuentaContable
        WHERE CodigoCuenta = @CodigoCuenta
    )
    BEGIN
        SET @Mensaje = 'La cuenta contable no existe.';
        RETURN;
    END;

    IF (@NombreCuenta = '')
    BEGIN
        SET @Mensaje = 'El nombre de la cuenta es obligatorio.';
        RETURN;
    END;

    IF (LEN(@NombreCuenta) > 100)
    BEGIN
        SET @Mensaje = 'El nombre de la cuenta no puede superar los 100 caracteres.';
        RETURN;
    END;

    IF (@IdTipoCuentaContable <= 0)
    BEGIN
        SET @Mensaje = 'Debe seleccionar un tipo de cuenta contable.';
        RETURN;
    END;

    IF (@IdNaturalezaCuentaContable <= 0)
    BEGIN
        SET @Mensaje = 'Debe seleccionar una naturaleza de cuenta contable.';
        RETURN;
    END;

    IF (@CodigoCuentaPadre = '')
    BEGIN
        SET @CodigoCuentaPadre = '0';
    END;

    IF (@CodigoCuentaPadre = @CodigoCuenta)
    BEGIN
        SET @Mensaje = 'Una cuenta no puede ser su propia cuenta padre.';
        RETURN;
    END;

    IF EXISTS (
        SELECT 1
        FROM dbo.CuentaContable
        WHERE UPPER(LTRIM(RTRIM(NombreCuenta))) = UPPER(@NombreCuenta)
          AND CodigoCuenta <> @CodigoCuenta
    )
    BEGIN
        SET @Mensaje = 'Ya existe otra cuenta contable con ese nombre.';
        RETURN;
    END;

    IF NOT EXISTS (
        SELECT 1
        FROM dbo.TipoCuentaContable
        WHERE IdTipoCuentaContable = @IdTipoCuentaContable
          AND Activo = 1
    )
    BEGIN
        SET @Mensaje = 'El tipo de cuenta contable seleccionado no existe o está inactivo.';
        RETURN;
    END;

    IF NOT EXISTS (
        SELECT 1
        FROM dbo.NaturalezaCuentaContable
        WHERE IdNaturalezaCuentaContable = @IdNaturalezaCuentaContable
          AND Activo = 1
    )
    BEGIN
        SET @Mensaje = 'La naturaleza de cuenta contable seleccionada no existe o está inactiva.';
        RETURN;
    END;

    IF NOT EXISTS (
        SELECT 1
        FROM dbo.CuentaContable
        WHERE CodigoCuenta = @CodigoCuentaPadre
          AND Activo = 1
    )
    BEGIN
        SET @Mensaje = 'La cuenta padre seleccionada no existe o está inactiva.';
        RETURN;
    END;

    IF (@CodigoCuentaPadre <> '0')
    BEGIN
        IF EXISTS (
            SELECT 1
            FROM dbo.CuentaContable
            WHERE CodigoCuenta = @CodigoCuentaPadre
              AND AceptaMovimientos = 1
        )
        BEGIN
            SET @Mensaje = 'La cuenta padre no puede aceptar movimientos si tendrá cuentas hijas.';
            RETURN;
        END;
    END;

    IF (@AceptaMovimientos = 1)
    BEGIN
        IF EXISTS (
            SELECT 1
            FROM dbo.CuentaContable
            WHERE CodigoCuentaPadre = @CodigoCuenta
              AND Activo = 1
        )
        BEGIN
            SET @Mensaje = 'La cuenta no puede aceptar movimientos porque tiene cuentas hijas activas.';
            RETURN;
        END;
    END;

    IF (@Activo = 0)
    BEGIN
        IF EXISTS (
            SELECT 1
            FROM dbo.CuentaContable
            WHERE CodigoCuentaPadre = @CodigoCuenta
              AND Activo = 1
        )
        BEGIN
            SET @Mensaje = 'No se puede inactivar la cuenta porque tiene cuentas hijas activas.';
            RETURN;
        END;

        IF EXISTS (
            SELECT 1
            FROM dbo.DetalleAsientoContable
            WHERE CodigoCuenta = @CodigoCuenta
              AND Activo = 1
        )
        BEGIN
            SET @Mensaje = 'No se puede inactivar la cuenta porque tiene movimientos contables activos.';
            RETURN;
        END;

        IF EXISTS (
            SELECT 1
            FROM dbo.Ingreso
            WHERE CodigoCuenta = @CodigoCuenta
              AND Activo = 1
        )
        BEGIN
            SET @Mensaje = 'No se puede inactivar la cuenta porque está asociada a ingresos activos.';
            RETURN;
        END;

        IF EXISTS (
            SELECT 1
            FROM dbo.Gasto
            WHERE CodigoCuenta = @CodigoCuenta
              AND Activo = 1
        )
        BEGIN
            SET @Mensaje = 'No se puede inactivar la cuenta porque está asociada a gastos activos.';
            RETURN;
        END;
    END;

    BEGIN TRY
        UPDATE dbo.CuentaContable
        SET
            NombreCuenta = @NombreCuenta,
            IdTipoCuentaContable = @IdTipoCuentaContable,
            IdNaturalezaCuentaContable = @IdNaturalezaCuentaContable,
            AceptaMovimientos = @AceptaMovimientos,
            Activo = @Activo,
            CodigoCuentaPadre = @CodigoCuentaPadre
        WHERE CodigoCuenta = @CodigoCuenta;

        SET @Resultado = 1;
        SET @Mensaje = 'Cuenta contable actualizada correctamente.';
    END TRY
    BEGIN CATCH
        SET @Resultado = 0;
        SET @Mensaje = ERROR_MESSAGE();
    END CATCH;
END;
GO

/* =========================================================
   CUENTA CONTABLE - INACTIVAR
   ========================================================= */
CREATE OR ALTER PROCEDURE Contabilidad.sp_CuentaContable_Inactivar
    @CodigoCuenta VARCHAR(45),
    @Resultado BIT OUTPUT,
    @Mensaje VARCHAR(500) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    SET @Resultado = 0;
    SET @Mensaje = '';

    SET @CodigoCuenta = LTRIM(RTRIM(ISNULL(@CodigoCuenta, '')));

    IF (@CodigoCuenta = '')
    BEGIN
        SET @Mensaje = 'Debe seleccionar una cuenta contable válida.';
        RETURN;
    END;

    IF (@CodigoCuenta = '0')
    BEGIN
        SET @Mensaje = 'No se puede inactivar la cuenta raíz del sistema.';
        RETURN;
    END;

    IF NOT EXISTS (
        SELECT 1
        FROM dbo.CuentaContable
        WHERE CodigoCuenta = @CodigoCuenta
    )
    BEGIN
        SET @Mensaje = 'La cuenta contable no existe.';
        RETURN;
    END;

    IF EXISTS (
        SELECT 1
        FROM dbo.CuentaContable
        WHERE CodigoCuentaPadre = @CodigoCuenta
          AND Activo = 1
    )
    BEGIN
        SET @Mensaje = 'No se puede inactivar la cuenta porque tiene cuentas hijas activas.';
        RETURN;
    END;

    IF EXISTS (
        SELECT 1
        FROM dbo.DetalleAsientoContable
        WHERE CodigoCuenta = @CodigoCuenta
          AND Activo = 1
    )
    BEGIN
        SET @Mensaje = 'No se puede inactivar la cuenta porque tiene movimientos contables activos.';
        RETURN;
    END;

    IF EXISTS (
        SELECT 1
        FROM dbo.Ingreso
        WHERE CodigoCuenta = @CodigoCuenta
          AND Activo = 1
    )
    BEGIN
        SET @Mensaje = 'No se puede inactivar la cuenta porque está asociada a ingresos activos.';
        RETURN;
    END;

    IF EXISTS (
        SELECT 1
        FROM dbo.Gasto
        WHERE CodigoCuenta = @CodigoCuenta
          AND Activo = 1
    )
    BEGIN
        SET @Mensaje = 'No se puede inactivar la cuenta porque está asociada a gastos activos.';
        RETURN;
    END;

    BEGIN TRY
        UPDATE dbo.CuentaContable
        SET
            Activo = 0,
            AceptaMovimientos = 0
        WHERE CodigoCuenta = @CodigoCuenta;

        SET @Resultado = 1;
        SET @Mensaje = 'Cuenta contable inactivada correctamente.';
    END TRY
    BEGIN CATCH
        SET @Resultado = 0;
        SET @Mensaje = ERROR_MESSAGE();
    END CATCH;
END;
GO

/* =========================================================
   CUENTA CONTABLE - GENERAR CÓDIGO
   ========================================================= */
CREATE OR ALTER PROCEDURE Contabilidad.sp_CuentaContable_GenerarCodigo
    @CodigoCuentaPadre VARCHAR(45),
    @CodigoGenerado VARCHAR(45) OUTPUT,
    @Resultado BIT OUTPUT,
    @Mensaje VARCHAR(500) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @UltimoNumero INT;

    SET @CodigoGenerado = '';
    SET @Resultado = 0;
    SET @Mensaje = '';

    SET @CodigoCuentaPadre = LTRIM(RTRIM(ISNULL(@CodigoCuentaPadre, '')));

    IF (@CodigoCuentaPadre = '')
    BEGIN
        SET @CodigoCuentaPadre = '0';
    END;

    IF NOT EXISTS (
        SELECT 1
        FROM dbo.CuentaContable
        WHERE CodigoCuenta = @CodigoCuentaPadre
          AND Activo = 1
    )
    BEGIN
        SET @Mensaje = 'La cuenta padre seleccionada no existe o está inactiva.';
        RETURN;
    END;

    IF (@CodigoCuentaPadre = '0')
    BEGIN
        SELECT @UltimoNumero =
            ISNULL(MAX(
                TRY_CAST(LEFT(CodigoCuenta, CHARINDEX('.', CodigoCuenta + '.') - 1) AS INT)
            ), 0) + 1
        FROM dbo.CuentaContable
        WHERE CodigoCuenta <> '0'
          AND CodigoCuentaPadre = '0';

        SET @CodigoGenerado = CAST(@UltimoNumero AS VARCHAR(10));
    END
    ELSE
    BEGIN
        SELECT @UltimoNumero =
            ISNULL(MAX(
                TRY_CAST(
                    RIGHT(CodigoCuenta, LEN(CodigoCuenta) - LEN(@CodigoCuentaPadre) - 1)
                    AS INT
                )
            ), 0) + 1
        FROM dbo.CuentaContable
        WHERE CodigoCuentaPadre = @CodigoCuentaPadre
          AND CodigoCuenta LIKE @CodigoCuentaPadre + '.%';

        SET @CodigoGenerado =
            @CodigoCuentaPadre + '.' + RIGHT('00' + CAST(@UltimoNumero AS VARCHAR(10)), 2);
    END;

    SET @Resultado = 1;
    SET @Mensaje = 'Código generado correctamente.';
END;
GO