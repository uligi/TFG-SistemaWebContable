IF NOT EXISTS (SELECT 1 FROM sys.schemas WHERE name = 'Contabilidad')
BEGIN
    EXEC('CREATE SCHEMA Contabilidad');
END;
GO

/* =========================================================
   ASIENTO CONTABLE - LISTAR
   ========================================================= */
CREATE OR ALTER PROCEDURE Contabilidad.sp_AsientoContable_Listar
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        ac.NumeroAsiento,
        ac.Anio,
        ac.Mes,
        pc.FechaInicio,
        pc.FechaFin,
        pc.Estado AS EstadoPeriodo,
        ac.FechaAsiento,
        ac.TipoAsiento,
        ac.Concepto,
        ac.TotalDebe,
        ac.TotalHaber,
        ac.Activo
    FROM dbo.AsientoContable ac
    INNER JOIN dbo.PeriodoContable pc
        ON pc.Anio = ac.Anio
       AND pc.Mes = ac.Mes
    ORDER BY
        ac.FechaAsiento DESC,
        ac.NumeroAsiento DESC;
END;
GO

/* =========================================================
   ASIENTO CONTABLE - OBTENER
   ========================================================= */
CREATE OR ALTER PROCEDURE Contabilidad.sp_AsientoContable_Obtener
    @NumeroAsiento VARCHAR(45)
AS
BEGIN
    SET NOCOUNT ON;

    SET @NumeroAsiento = LTRIM(RTRIM(ISNULL(@NumeroAsiento, '')));

    SELECT
        ac.NumeroAsiento,
        ac.Anio,
        ac.Mes,
        pc.FechaInicio,
        pc.FechaFin,
        pc.Estado AS EstadoPeriodo,
        ac.FechaAsiento,
        ac.TipoAsiento,
        ac.Concepto,
        ac.TotalDebe,
        ac.TotalHaber,
        ac.Activo
    FROM dbo.AsientoContable ac
    INNER JOIN dbo.PeriodoContable pc
        ON pc.Anio = ac.Anio
       AND pc.Mes = ac.Mes
    WHERE ac.NumeroAsiento = @NumeroAsiento;
END;
GO

/* =========================================================
   ASIENTO CONTABLE - LISTAR POR PERIODO
   ========================================================= */
CREATE OR ALTER PROCEDURE Contabilidad.sp_AsientoContable_ListarPorPeriodo
    @Anio INT,
    @Mes INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        ac.NumeroAsiento,
        ac.Anio,
        ac.Mes,
        pc.FechaInicio,
        pc.FechaFin,
        pc.Estado AS EstadoPeriodo,
        ac.FechaAsiento,
        ac.TipoAsiento,
        ac.Concepto,
        ac.TotalDebe,
        ac.TotalHaber,
        ac.Activo
    FROM dbo.AsientoContable ac
    INNER JOIN dbo.PeriodoContable pc
        ON pc.Anio = ac.Anio
       AND pc.Mes = ac.Mes
    WHERE ac.Anio = @Anio
      AND ac.Mes = @Mes
    ORDER BY
        ac.FechaAsiento DESC,
        ac.NumeroAsiento DESC;
END;
GO

/* =========================================================
   ASIENTO CONTABLE - GENERAR NUMERO
   Formato: AS-2026-000001
   ========================================================= */
CREATE OR ALTER PROCEDURE Contabilidad.sp_AsientoContable_GenerarNumero
    @Anio INT,
    @NumeroAsiento VARCHAR(45) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @Consecutivo INT;

    SET @NumeroAsiento = '';

    IF (@Anio < 2000 OR @Anio > 2100)
    BEGIN
        SET @Anio = YEAR(GETDATE());
    END;

    SELECT @Consecutivo =
        ISNULL(MAX(
            TRY_CAST(RIGHT(NumeroAsiento, 6) AS INT)
        ), 0) + 1
    FROM dbo.AsientoContable
    WHERE NumeroAsiento LIKE 'AS-' + CAST(@Anio AS VARCHAR(4)) + '-%';

    SET @NumeroAsiento =
        'AS-' + CAST(@Anio AS VARCHAR(4)) + '-' + RIGHT('000000' + CAST(@Consecutivo AS VARCHAR(6)), 6);
END;
GO

/* =========================================================
   ASIENTO CONTABLE - REGISTRAR
   ========================================================= */
CREATE OR ALTER PROCEDURE Contabilidad.sp_AsientoContable_Registrar
    @Anio INT,
    @Mes INT,
    @FechaAsiento DATETIME,
    @TipoAsiento VARCHAR(45),
    @Concepto VARCHAR(150),
    @NumeroAsientoGenerado VARCHAR(45) OUTPUT,
    @Resultado INT OUTPUT,
    @Mensaje VARCHAR(500) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @NumeroAsiento VARCHAR(45);

    SET @Resultado = 0;
    SET @Mensaje = '';
    SET @NumeroAsientoGenerado = '';

    SET @TipoAsiento = LTRIM(RTRIM(ISNULL(@TipoAsiento, '')));
    SET @Concepto = LTRIM(RTRIM(ISNULL(@Concepto, '')));

    IF (@Anio < 2000 OR @Anio > 2100)
    BEGIN
        SET @Mensaje = 'Debe seleccionar un año válido.';
        RETURN;
    END;

    IF (@Mes < 1 OR @Mes > 12)
    BEGIN
        SET @Mensaje = 'Debe seleccionar un mes válido.';
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
        SET @Mensaje = 'El período contable seleccionado no existe, está cerrado o está inactivo.';
        RETURN;
    END;

    IF (@FechaAsiento IS NULL)
    BEGIN
        SET @Mensaje = 'Debe ingresar la fecha del asiento.';
        RETURN;
    END;

    IF NOT EXISTS (
        SELECT 1
        FROM dbo.PeriodoContable
        WHERE Anio = @Anio
          AND Mes = @Mes
          AND @FechaAsiento BETWEEN FechaInicio AND FechaFin
    )
    BEGIN
        SET @Mensaje = 'La fecha del asiento debe estar dentro del período contable seleccionado.';
        RETURN;
    END;

    IF (@TipoAsiento = '')
    BEGIN
        SET @Mensaje = 'Debe ingresar el tipo de asiento.';
        RETURN;
    END;

    IF (LEN(@TipoAsiento) > 45)
    BEGIN
        SET @Mensaje = 'El tipo de asiento no puede superar los 45 caracteres.';
        RETURN;
    END;

    IF (@Concepto = '')
    BEGIN
        SET @Mensaje = 'Debe ingresar el concepto del asiento.';
        RETURN;
    END;

    IF (LEN(@Concepto) > 150)
    BEGIN
        SET @Mensaje = 'El concepto no puede superar los 150 caracteres.';
        RETURN;
    END;

    BEGIN TRY
        BEGIN TRANSACTION;

        EXEC Contabilidad.sp_AsientoContable_GenerarNumero
            @Anio = @Anio,
            @NumeroAsiento = @NumeroAsiento OUTPUT;

        INSERT INTO dbo.AsientoContable
        (
            NumeroAsiento,
            Anio,
            Mes,
            FechaAsiento,
            TipoAsiento,
            Concepto,
            TotalDebe,
            TotalHaber,
            Activo
        )
        VALUES
        (
            @NumeroAsiento,
            @Anio,
            @Mes,
            @FechaAsiento,
            @TipoAsiento,
            @Concepto,
            0,
            0,
            1
        );

        COMMIT TRANSACTION;

        SET @Resultado = 1;
        SET @NumeroAsientoGenerado = @NumeroAsiento;
        SET @Mensaje = 'Asiento contable registrado correctamente.';
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;

        SET @Resultado = 0;
        SET @NumeroAsientoGenerado = '';
        SET @Mensaje = ERROR_MESSAGE();
    END CATCH;
END;
GO

/* =========================================================
   ASIENTO CONTABLE - EDITAR
   Nota:
   NumeroAsiento es PK, no se modifica.
   ========================================================= */
CREATE OR ALTER PROCEDURE Contabilidad.sp_AsientoContable_Editar
    @NumeroAsiento VARCHAR(45),
    @Anio INT,
    @Mes INT,
    @FechaAsiento DATETIME,
    @TipoAsiento VARCHAR(45),
    @Concepto VARCHAR(150),
    @Activo BIT,
    @Resultado BIT OUTPUT,
    @Mensaje VARCHAR(500) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    SET @Resultado = 0;
    SET @Mensaje = '';

    SET @NumeroAsiento = LTRIM(RTRIM(ISNULL(@NumeroAsiento, '')));
    SET @TipoAsiento = LTRIM(RTRIM(ISNULL(@TipoAsiento, '')));
    SET @Concepto = LTRIM(RTRIM(ISNULL(@Concepto, '')));

    IF (@NumeroAsiento = '')
    BEGIN
        SET @Mensaje = 'Debe seleccionar un asiento contable válido.';
        RETURN;
    END;

    IF NOT EXISTS (
        SELECT 1
        FROM dbo.AsientoContable
        WHERE NumeroAsiento = @NumeroAsiento
    )
    BEGIN
        SET @Mensaje = 'El asiento contable no existe.';
        RETURN;
    END;

    IF (@Anio < 2000 OR @Anio > 2100)
    BEGIN
        SET @Mensaje = 'Debe seleccionar un año válido.';
        RETURN;
    END;

    IF (@Mes < 1 OR @Mes > 12)
    BEGIN
        SET @Mensaje = 'Debe seleccionar un mes válido.';
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
        SET @Mensaje = 'El período contable seleccionado no existe, está cerrado o está inactivo.';
        RETURN;
    END;

    IF (@FechaAsiento IS NULL)
    BEGIN
        SET @Mensaje = 'Debe ingresar la fecha del asiento.';
        RETURN;
    END;

    IF NOT EXISTS (
        SELECT 1
        FROM dbo.PeriodoContable
        WHERE Anio = @Anio
          AND Mes = @Mes
          AND @FechaAsiento BETWEEN FechaInicio AND FechaFin
    )
    BEGIN
        SET @Mensaje = 'La fecha del asiento debe estar dentro del período contable seleccionado.';
        RETURN;
    END;

    IF (@TipoAsiento = '')
    BEGIN
        SET @Mensaje = 'Debe ingresar el tipo de asiento.';
        RETURN;
    END;

    IF (LEN(@TipoAsiento) > 45)
    BEGIN
        SET @Mensaje = 'El tipo de asiento no puede superar los 45 caracteres.';
        RETURN;
    END;

    IF (@Concepto = '')
    BEGIN
        SET @Mensaje = 'Debe ingresar el concepto del asiento.';
        RETURN;
    END;

    IF (LEN(@Concepto) > 150)
    BEGIN
        SET @Mensaje = 'El concepto no puede superar los 150 caracteres.';
        RETURN;
    END;

    BEGIN TRY
        UPDATE dbo.AsientoContable
        SET
            Anio = @Anio,
            Mes = @Mes,
            FechaAsiento = @FechaAsiento,
            TipoAsiento = @TipoAsiento,
            Concepto = @Concepto,
            Activo = @Activo
        WHERE NumeroAsiento = @NumeroAsiento;

        SET @Resultado = 1;
        SET @Mensaje = 'Asiento contable actualizado correctamente.';
    END TRY
    BEGIN CATCH
        SET @Resultado = 0;
        SET @Mensaje = ERROR_MESSAGE();
    END CATCH;
END;
GO

/* =========================================================
   ASIENTO CONTABLE - RECALCULAR TOTALES
   Se ejecuta desde DetalleAsientoContable.
   ========================================================= */
CREATE OR ALTER PROCEDURE Contabilidad.sp_AsientoContable_RecalcularTotales
    @NumeroAsiento VARCHAR(45)
AS
BEGIN
    SET NOCOUNT ON;

    SET @NumeroAsiento = LTRIM(RTRIM(ISNULL(@NumeroAsiento, '')));

    UPDATE dbo.AsientoContable
    SET
        TotalDebe = ISNULL((
            SELECT SUM(Debe)
            FROM dbo.DetalleAsientoContable
            WHERE NumeroAsiento = @NumeroAsiento
              AND Activo = 1
        ), 0),
        TotalHaber = ISNULL((
            SELECT SUM(Haber)
            FROM dbo.DetalleAsientoContable
            WHERE NumeroAsiento = @NumeroAsiento
              AND Activo = 1
        ), 0)
    WHERE NumeroAsiento = @NumeroAsiento;
END;
GO

/* =========================================================
   ASIENTO CONTABLE - VALIDAR CUADRE
   ========================================================= */
CREATE OR ALTER PROCEDURE Contabilidad.sp_AsientoContable_ValidarCuadre
    @NumeroAsiento VARCHAR(45),
    @Resultado BIT OUTPUT,
    @Mensaje VARCHAR(500) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @TotalDebe DECIMAL(18,2);
    DECLARE @TotalHaber DECIMAL(18,2);
    DECLARE @CantidadLineas INT;

    SET @Resultado = 0;
    SET @Mensaje = '';

    SET @NumeroAsiento = LTRIM(RTRIM(ISNULL(@NumeroAsiento, '')));

    IF (@NumeroAsiento = '')
    BEGIN
        SET @Mensaje = 'Debe seleccionar un asiento contable válido.';
        RETURN;
    END;

    IF NOT EXISTS (
        SELECT 1
        FROM dbo.AsientoContable
        WHERE NumeroAsiento = @NumeroAsiento
          AND Activo = 1
    )
    BEGIN
        SET @Mensaje = 'El asiento contable no existe o está inactivo.';
        RETURN;
    END;

    EXEC Contabilidad.sp_AsientoContable_RecalcularTotales
        @NumeroAsiento = @NumeroAsiento;

    SELECT
        @TotalDebe = TotalDebe,
        @TotalHaber = TotalHaber
    FROM dbo.AsientoContable
    WHERE NumeroAsiento = @NumeroAsiento;

    SELECT @CantidadLineas = COUNT(1)
    FROM dbo.DetalleAsientoContable
    WHERE NumeroAsiento = @NumeroAsiento
      AND Activo = 1;

    IF (@CantidadLineas < 2)
    BEGIN
        SET @Mensaje = 'El asiento debe tener al menos dos líneas activas.';
        RETURN;
    END;

    IF (@TotalDebe <= 0 OR @TotalHaber <= 0)
    BEGIN
        SET @Mensaje = 'El asiento debe tener montos en debe y haber.';
        RETURN;
    END;

    IF (@TotalDebe <> @TotalHaber)
    BEGIN
        SET @Mensaje = 'El asiento no está cuadrado. Debe y Haber deben ser iguales.';
        RETURN;
    END;

    SET @Resultado = 1;
    SET @Mensaje = 'El asiento contable está cuadrado.';
END;
GO

/* =========================================================
   ASIENTO CONTABLE - INACTIVAR
   ========================================================= */
CREATE OR ALTER PROCEDURE Contabilidad.sp_AsientoContable_Inactivar
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
    WHERE NumeroAsiento = @NumeroAsiento;

    IF (@Anio IS NULL OR @Mes IS NULL)
    BEGIN
        SET @Mensaje = 'El asiento contable no existe.';
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
        SET @Mensaje = 'No se puede inactivar un asiento de un período cerrado o inactivo.';
        RETURN;
    END;

    BEGIN TRY
        BEGIN TRANSACTION;

        UPDATE dbo.AsientoContable
        SET Activo = 0
        WHERE NumeroAsiento = @NumeroAsiento;

        UPDATE dbo.DetalleAsientoContable
        SET Activo = 0
        WHERE NumeroAsiento = @NumeroAsiento;

        EXEC Contabilidad.sp_AsientoContable_RecalcularTotales
            @NumeroAsiento = @NumeroAsiento;

        COMMIT TRANSACTION;

        SET @Resultado = 1;
        SET @Mensaje = 'Asiento contable inactivado correctamente.';
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
   ASIENTO CONTABLE Automatico - Validar
   ========================================================= */


   CREATE OR ALTER PROCEDURE Contabilidad.sp_ReferenciaAsientoContable_Existe
(
    @ModuloOrigen VARCHAR(45),
    @DocumentoOrigen VARCHAR(45),
    @TipoMovimiento VARCHAR(45),
    @Existe BIT OUTPUT,
    @NumeroAsiento VARCHAR(45) OUTPUT
)
AS
BEGIN
    SET NOCOUNT ON;

    SET @Existe = 0;
    SET @NumeroAsiento = '';

    SELECT TOP 1
        @Existe = 1,
        @NumeroAsiento = NumeroAsiento
    FROM dbo.ReferenciaAsientoContable
    WHERE ModuloOrigen = @ModuloOrigen
      AND DocumentoOrigen = @DocumentoOrigen
      AND TipoMovimiento = @TipoMovimiento
      AND Activo = 1;
END;
GO


/* =========================================================
   ASIENTO CONTABLE Automatico - Registrar
   ========================================================= */


   CREATE OR ALTER PROCEDURE Contabilidad.sp_ReferenciaAsientoContable_Registrar
(
    @NumeroAsiento VARCHAR(45),
    @ModuloOrigen VARCHAR(45),
    @DocumentoOrigen VARCHAR(45),
    @TipoMovimiento VARCHAR(45),
    @Resultado INT OUTPUT,
    @Mensaje VARCHAR(500) OUTPUT
)
AS
BEGIN
    SET NOCOUNT ON;

    SET @Resultado = 0;
    SET @Mensaje = '';

    IF NOT EXISTS (
        SELECT 1
        FROM dbo.AsientoContable
        WHERE NumeroAsiento = @NumeroAsiento
    )
    BEGIN
        SET @Mensaje = 'El asiento contable indicado no existe.';
        RETURN;
    END;

    IF EXISTS (
        SELECT 1
        FROM dbo.ReferenciaAsientoContable
        WHERE ModuloOrigen = @ModuloOrigen
          AND DocumentoOrigen = @DocumentoOrigen
          AND TipoMovimiento = @TipoMovimiento
          AND Activo = 1
    )
    BEGIN
        SET @Mensaje = 'Ya existe un asiento contable para este movimiento.';
        RETURN;
    END;

    INSERT INTO dbo.ReferenciaAsientoContable
    (
        NumeroAsiento,
        ModuloOrigen,
        DocumentoOrigen,
        TipoMovimiento,
        FechaCreacion,
        Activo
    )
    VALUES
    (
        @NumeroAsiento,
        @ModuloOrigen,
        @DocumentoOrigen,
        @TipoMovimiento,
        GETDATE(),
        1
    );

    SET @Resultado = SCOPE_IDENTITY();
    SET @Mensaje = 'Referencia de asiento contable registrada correctamente.';
END;
GO

/* =========================================================
   ASIENTO CONTABLE Automatico - Listar activos
   ========================================================= */

CREATE OR ALTER PROCEDURE Contabilidad.sp_CuentaContable_ListarIngresosActivos
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        CodigoCuenta,
        NombreCuenta,
        IdTipoCuentaContable,
        IdNaturalezaCuentaContable,
        CodigoCuentaPadre,
        AceptaMovimientos,
        Activo
    FROM dbo.CuentaContable
    WHERE Activo = 1
      AND AceptaMovimientos = 1
      AND IdTipoCuentaContable = 4
    ORDER BY CodigoCuenta;
END;
GO
