IF NOT EXISTS (SELECT 1 FROM sys.schemas WHERE name = 'Contabilidad')
BEGIN
    EXEC('CREATE SCHEMA Contabilidad');
END;
GO

/* =========================================================
   DETALLE ASIENTO CONTABLE - LISTAR
   ========================================================= */
CREATE OR ALTER PROCEDURE Contabilidad.sp_DetalleAsientoContable_Listar
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        dac.NumeroAsiento,
        dac.NumeroLinea,
        dac.CodigoCuenta,
        cc.NombreCuenta,
        cc.IdTipoCuentaContable,
        tcc.Nombre AS TipoCuentaContableNombre,
        cc.IdNaturalezaCuentaContable,
        ncc.Naturaleza AS NaturalezaCuentaContableNombre,
        dac.Debe,
        dac.Haber,
        dac.DescripcionLinea,
        dac.Activo
    FROM dbo.DetalleAsientoContable dac
    INNER JOIN dbo.CuentaContable cc
        ON cc.CodigoCuenta = dac.CodigoCuenta
    INNER JOIN dbo.TipoCuentaContable tcc
        ON tcc.IdTipoCuentaContable = cc.IdTipoCuentaContable
    INNER JOIN dbo.NaturalezaCuentaContable ncc
        ON ncc.IdNaturalezaCuentaContable = cc.IdNaturalezaCuentaContable
    ORDER BY
        dac.NumeroAsiento DESC,
        dac.NumeroLinea ASC;
END;
GO

/* =========================================================
   DETALLE ASIENTO CONTABLE - LISTAR POR ASIENTO
   ========================================================= */
CREATE OR ALTER PROCEDURE Contabilidad.sp_DetalleAsientoContable_ListarPorAsiento
    @NumeroAsiento VARCHAR(45)
AS
BEGIN
    SET NOCOUNT ON;

    SET @NumeroAsiento = LTRIM(RTRIM(ISNULL(@NumeroAsiento, '')));

    SELECT
        dac.NumeroAsiento,
        dac.NumeroLinea,
        dac.CodigoCuenta,
        cc.NombreCuenta,
        cc.IdTipoCuentaContable,
        tcc.Nombre AS TipoCuentaContableNombre,
        cc.IdNaturalezaCuentaContable,
        ncc.Naturaleza AS NaturalezaCuentaContableNombre,
        dac.Debe,
        dac.Haber,
        dac.DescripcionLinea,
        dac.Activo
    FROM dbo.DetalleAsientoContable dac
    INNER JOIN dbo.CuentaContable cc
        ON cc.CodigoCuenta = dac.CodigoCuenta
    INNER JOIN dbo.TipoCuentaContable tcc
        ON tcc.IdTipoCuentaContable = cc.IdTipoCuentaContable
    INNER JOIN dbo.NaturalezaCuentaContable ncc
        ON ncc.IdNaturalezaCuentaContable = cc.IdNaturalezaCuentaContable
    WHERE dac.NumeroAsiento = @NumeroAsiento
    ORDER BY
        dac.NumeroLinea ASC;
END;
GO

/* =========================================================
   DETALLE ASIENTO CONTABLE - OBTENER
   ========================================================= */
CREATE OR ALTER PROCEDURE Contabilidad.sp_DetalleAsientoContable_Obtener
    @NumeroAsiento VARCHAR(45),
    @NumeroLinea INT
AS
BEGIN
    SET NOCOUNT ON;

    SET @NumeroAsiento = LTRIM(RTRIM(ISNULL(@NumeroAsiento, '')));

    SELECT
        dac.NumeroAsiento,
        dac.NumeroLinea,
        dac.CodigoCuenta,
        cc.NombreCuenta,
        cc.IdTipoCuentaContable,
        tcc.Nombre AS TipoCuentaContableNombre,
        cc.IdNaturalezaCuentaContable,
        ncc.Naturaleza AS NaturalezaCuentaContableNombre,
        dac.Debe,
        dac.Haber,
        dac.DescripcionLinea,
        dac.Activo
    FROM dbo.DetalleAsientoContable dac
    INNER JOIN dbo.CuentaContable cc
        ON cc.CodigoCuenta = dac.CodigoCuenta
    INNER JOIN dbo.TipoCuentaContable tcc
        ON tcc.IdTipoCuentaContable = cc.IdTipoCuentaContable
    INNER JOIN dbo.NaturalezaCuentaContable ncc
        ON ncc.IdNaturalezaCuentaContable = cc.IdNaturalezaCuentaContable
    WHERE dac.NumeroAsiento = @NumeroAsiento
      AND dac.NumeroLinea = @NumeroLinea;
END;
GO

/* =========================================================
   DETALLE ASIENTO CONTABLE - GENERAR LINEA
   ========================================================= */
CREATE OR ALTER PROCEDURE Contabilidad.sp_DetalleAsientoContable_GenerarLinea
    @NumeroAsiento VARCHAR(45),
    @NumeroLinea INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    SET @NumeroAsiento = LTRIM(RTRIM(ISNULL(@NumeroAsiento, '')));

    SELECT
        @NumeroLinea = ISNULL(MAX(NumeroLinea), 0) + 1
    FROM dbo.DetalleAsientoContable
    WHERE NumeroAsiento = @NumeroAsiento;
END;
GO

/* =========================================================
   DETALLE ASIENTO CONTABLE - REGISTRAR
   ========================================================= */
CREATE OR ALTER PROCEDURE Contabilidad.sp_DetalleAsientoContable_Registrar
    @NumeroAsiento VARCHAR(45),
    @CodigoCuenta VARCHAR(45),
    @Debe DECIMAL(18,2),
    @Haber DECIMAL(18,2),
    @DescripcionLinea VARCHAR(150),
    @NumeroLineaGenerada INT OUTPUT,
    @Resultado INT OUTPUT,
    @Mensaje VARCHAR(500) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @NumeroLinea INT;
    DECLARE @Anio INT;
    DECLARE @Mes INT;

    SET @Resultado = 0;
    SET @Mensaje = '';
    SET @NumeroLineaGenerada = 0;

    SET @NumeroAsiento = LTRIM(RTRIM(ISNULL(@NumeroAsiento, '')));
    SET @CodigoCuenta = LTRIM(RTRIM(ISNULL(@CodigoCuenta, '')));
    SET @DescripcionLinea = LTRIM(RTRIM(ISNULL(@DescripcionLinea, '')));

    SET @Debe = ISNULL(@Debe, 0);
    SET @Haber = ISNULL(@Haber, 0);

    IF (@NumeroAsiento = '')
    BEGIN
        SET @Mensaje = 'Debe seleccionar un asiento contable válido.';
        RETURN;
    END;

    SELECT
        @Anio = Anio,
        @Mes = Mes
    FROM dbo.AsientoContable
    WHERE NumeroAsiento = @NumeroAsiento
      AND Activo = 1;

    IF (@Anio IS NULL OR @Mes IS NULL)
    BEGIN
        SET @Mensaje = 'El asiento contable no existe o está inactivo.';
        RETURN;
    END;

    IF NOT EXISTS (
        SELECT 1
        FROM dbo.PeriodoContable
        WHERE Anio = @Anio
          AND Mes = @Mes
          AND Estado = 'Abierto'
          AND Activo = 1
    )
    BEGIN
        SET @Mensaje = 'No se pueden agregar líneas a un asiento de un período cerrado o inactivo.';
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
          AND CodigoCuenta <> '0'
    )
    BEGIN
        SET @Mensaje = 'La cuenta contable seleccionada no existe, está inactiva o no acepta movimientos.';
        RETURN;
    END;

    IF (@Debe < 0 OR @Haber < 0)
    BEGIN
        SET @Mensaje = 'Los montos de debe y haber no pueden ser negativos.';
        RETURN;
    END;

    IF (@Debe = 0 AND @Haber = 0)
    BEGIN
        SET @Mensaje = 'Debe ingresar un monto en debe o en haber.';
        RETURN;
    END;

    IF (@Debe > 0 AND @Haber > 0)
    BEGIN
        SET @Mensaje = 'No puede ingresar monto en debe y haber al mismo tiempo.';
        RETURN;
    END;

    IF (LEN(@DescripcionLinea) > 150)
    BEGIN
        SET @Mensaje = 'La descripción de la línea no puede superar los 150 caracteres.';
        RETURN;
    END;

    BEGIN TRY
        BEGIN TRANSACTION;

        EXEC Contabilidad.sp_DetalleAsientoContable_GenerarLinea
            @NumeroAsiento = @NumeroAsiento,
            @NumeroLinea = @NumeroLinea OUTPUT;

        INSERT INTO dbo.DetalleAsientoContable
        (
            NumeroAsiento,
            NumeroLinea,
            CodigoCuenta,
            Debe,
            Haber,
            DescripcionLinea,
            Activo
        )
        VALUES
        (
            @NumeroAsiento,
            @NumeroLinea,
            @CodigoCuenta,
            @Debe,
            @Haber,
            @DescripcionLinea,
            1
        );

        EXEC Contabilidad.sp_AsientoContable_RecalcularTotales
            @NumeroAsiento = @NumeroAsiento;

        COMMIT TRANSACTION;

        SET @Resultado = 1;
        SET @NumeroLineaGenerada = @NumeroLinea;
        SET @Mensaje = 'Detalle de asiento registrado correctamente.';
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;

        SET @Resultado = 0;
        SET @NumeroLineaGenerada = 0;
        SET @Mensaje = ERROR_MESSAGE();
    END CATCH;
END;
GO

/* =========================================================
   DETALLE ASIENTO CONTABLE - EDITAR
   ========================================================= */
CREATE OR ALTER PROCEDURE Contabilidad.sp_DetalleAsientoContable_Editar
    @NumeroAsiento VARCHAR(45),
    @NumeroLinea INT,
    @CodigoCuenta VARCHAR(45),
    @Debe DECIMAL(18,2),
    @Haber DECIMAL(18,2),
    @DescripcionLinea VARCHAR(150),
    @Activo BIT,
    @Resultado BIT OUTPUT,
    @Mensaje VARCHAR(500) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @Anio INT;
    DECLARE @Mes INT;

    SET @Resultado = 0;
    SET @Mensaje = '';

    SET @NumeroAsiento = LTRIM(RTRIM(ISNULL(@NumeroAsiento, '')));
    SET @CodigoCuenta = LTRIM(RTRIM(ISNULL(@CodigoCuenta, '')));
    SET @DescripcionLinea = LTRIM(RTRIM(ISNULL(@DescripcionLinea, '')));

    SET @Debe = ISNULL(@Debe, 0);
    SET @Haber = ISNULL(@Haber, 0);

    IF (@NumeroAsiento = '')
    BEGIN
        SET @Mensaje = 'Debe seleccionar un asiento contable válido.';
        RETURN;
    END;

    IF (@NumeroLinea <= 0)
    BEGIN
        SET @Mensaje = 'Debe seleccionar una línea válida.';
        RETURN;
    END;

    IF NOT EXISTS (
        SELECT 1
        FROM dbo.DetalleAsientoContable
        WHERE NumeroAsiento = @NumeroAsiento
          AND NumeroLinea = @NumeroLinea
    )
    BEGIN
        SET @Mensaje = 'La línea de asiento no existe.';
        RETURN;
    END;

    SELECT
        @Anio = Anio,
        @Mes = Mes
    FROM dbo.AsientoContable
    WHERE NumeroAsiento = @NumeroAsiento
      AND Activo = 1;

    IF (@Anio IS NULL OR @Mes IS NULL)
    BEGIN
        SET @Mensaje = 'El asiento contable no existe o está inactivo.';
        RETURN;
    END;

    IF NOT EXISTS (
        SELECT 1
        FROM dbo.PeriodoContable
        WHERE Anio = @Anio
          AND Mes = @Mes
          AND Estado = 'Abierto'
          AND Activo = 1
    )
    BEGIN
        SET @Mensaje = 'No se puede editar una línea de un período cerrado o inactivo.';
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
          AND CodigoCuenta <> '0'
    )
    BEGIN
        SET @Mensaje = 'La cuenta contable seleccionada no existe, está inactiva o no acepta movimientos.';
        RETURN;
    END;

    IF (@Debe < 0 OR @Haber < 0)
    BEGIN
        SET @Mensaje = 'Los montos de debe y haber no pueden ser negativos.';
        RETURN;
    END;

    IF (@Debe = 0 AND @Haber = 0)
    BEGIN
        SET @Mensaje = 'Debe ingresar un monto en debe o en haber.';
        RETURN;
    END;

    IF (@Debe > 0 AND @Haber > 0)
    BEGIN
        SET @Mensaje = 'No puede ingresar monto en debe y haber al mismo tiempo.';
        RETURN;
    END;

    IF (LEN(@DescripcionLinea) > 150)
    BEGIN
        SET @Mensaje = 'La descripción de la línea no puede superar los 150 caracteres.';
        RETURN;
    END;

    BEGIN TRY
        BEGIN TRANSACTION;

        UPDATE dbo.DetalleAsientoContable
        SET
            CodigoCuenta = @CodigoCuenta,
            Debe = @Debe,
            Haber = @Haber,
            DescripcionLinea = @DescripcionLinea,
            Activo = @Activo
        WHERE NumeroAsiento = @NumeroAsiento
          AND NumeroLinea = @NumeroLinea;

        EXEC Contabilidad.sp_AsientoContable_RecalcularTotales
            @NumeroAsiento = @NumeroAsiento;

        COMMIT TRANSACTION;

        SET @Resultado = 1;
        SET @Mensaje = 'Detalle de asiento actualizado correctamente.';
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
   DETALLE ASIENTO CONTABLE - INACTIVAR
   ========================================================= */
CREATE OR ALTER PROCEDURE Contabilidad.sp_DetalleAsientoContable_Inactivar
    @NumeroAsiento VARCHAR(45),
    @NumeroLinea INT,
    @Resultado BIT OUTPUT,
    @Mensaje VARCHAR(500) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @Anio INT;
    DECLARE @Mes INT;

    SET @Resultado = 0;
    SET @Mensaje = '';

    SET @NumeroAsiento = LTRIM(RTRIM(ISNULL(@NumeroAsiento, '')));

    IF (@NumeroAsiento = '')
    BEGIN
        SET @Mensaje = 'Debe seleccionar un asiento contable válido.';
        RETURN;
    END;

    IF (@NumeroLinea <= 0)
    BEGIN
        SET @Mensaje = 'Debe seleccionar una línea válida.';
        RETURN;
    END;

    IF NOT EXISTS (
        SELECT 1
        FROM dbo.DetalleAsientoContable
        WHERE NumeroAsiento = @NumeroAsiento
          AND NumeroLinea = @NumeroLinea
    )
    BEGIN
        SET @Mensaje = 'La línea de asiento no existe.';
        RETURN;
    END;

    SELECT
        @Anio = Anio,
        @Mes = Mes
    FROM dbo.AsientoContable
    WHERE NumeroAsiento = @NumeroAsiento
      AND Activo = 1;

    IF (@Anio IS NULL OR @Mes IS NULL)
    BEGIN
        SET @Mensaje = 'El asiento contable no existe o está inactivo.';
        RETURN;
    END;

    IF NOT EXISTS (
        SELECT 1
        FROM dbo.PeriodoContable
        WHERE Anio = @Anio
          AND Mes = @Mes
          AND Estado = 'Abierto'
          AND Activo = 1
    )
    BEGIN
        SET @Mensaje = 'No se puede inactivar una línea de un período cerrado o inactivo.';
        RETURN;
    END;

    BEGIN TRY
        BEGIN TRANSACTION;

        UPDATE dbo.DetalleAsientoContable
        SET Activo = 0
        WHERE NumeroAsiento = @NumeroAsiento
          AND NumeroLinea = @NumeroLinea;

        EXEC Contabilidad.sp_AsientoContable_RecalcularTotales
            @NumeroAsiento = @NumeroAsiento;

        COMMIT TRANSACTION;

        SET @Resultado = 1;
        SET @Mensaje = 'Detalle de asiento inactivado correctamente.';
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
   DETALLE ASIENTO CONTABLE - INACTIVAR POR ASIENTO
   Uso interno para regenerar detalles automáticos.
   ========================================================= */
CREATE OR ALTER PROCEDURE Contabilidad.sp_DetalleAsientoContable_InactivarPorAsiento
    @NumeroAsiento VARCHAR(45),
    @Resultado BIT OUTPUT,
    @Mensaje VARCHAR(500) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @Anio INT;
    DECLARE @Mes INT;

    SET @Resultado = 0;
    SET @Mensaje = '';

    SET @NumeroAsiento = LTRIM(RTRIM(ISNULL(@NumeroAsiento, '')));

    IF (@NumeroAsiento = '')
    BEGIN
        SET @Mensaje = 'Debe seleccionar un asiento contable válido.';
        RETURN;
    END;

    SELECT
        @Anio = Anio,
        @Mes = Mes
    FROM dbo.AsientoContable
    WHERE NumeroAsiento = @NumeroAsiento
      AND Activo = 1;

    IF (@Anio IS NULL OR @Mes IS NULL)
    BEGIN
        SET @Mensaje = 'El asiento contable no existe o está inactivo.';
        RETURN;
    END;

    IF NOT EXISTS (
        SELECT 1
        FROM dbo.PeriodoContable
        WHERE Anio = @Anio
          AND Mes = @Mes
          AND Estado = 'Abierto'
          AND Activo = 1
    )
    BEGIN
        SET @Mensaje = 'No se pueden inactivar líneas de un período cerrado o inactivo.';
        RETURN;
    END;

    BEGIN TRY
        BEGIN TRANSACTION;

        UPDATE dbo.DetalleAsientoContable
        SET Activo = 0
        WHERE NumeroAsiento = @NumeroAsiento;

        EXEC Contabilidad.sp_AsientoContable_RecalcularTotales
            @NumeroAsiento = @NumeroAsiento;

        COMMIT TRANSACTION;

        SET @Resultado = 1;
        SET @Mensaje = 'Líneas del asiento inactivadas correctamente.';
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;

        SET @Resultado = 0;
        SET @Mensaje = ERROR_MESSAGE();
    END CATCH;
END;
GO