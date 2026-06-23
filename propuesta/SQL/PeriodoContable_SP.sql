IF NOT EXISTS (SELECT 1 FROM sys.schemas WHERE name = 'Contabilidad')
BEGIN
    EXEC('CREATE SCHEMA Contabilidad');
END;
GO

/* =========================================================
   PERIODO CONTABLE - LISTAR
   ========================================================= */
CREATE OR ALTER PROCEDURE Contabilidad.sp_PeriodoContable_Listar
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        Anio,
        Mes,
        FechaInicio,
        FechaFin,
        Estado,
        Activo
    FROM dbo.PeriodoContable
    ORDER BY
        Anio DESC,
        Mes DESC;
END;
GO

/* =========================================================
   PERIODO CONTABLE - LISTAR ACTIVOS
   ========================================================= */
CREATE OR ALTER PROCEDURE Contabilidad.sp_PeriodoContable_ListarActivos
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        Anio,
        Mes,
        FechaInicio,
        FechaFin,
        Estado,
        Activo
    FROM dbo.PeriodoContable
    WHERE Activo = 1
    ORDER BY
        Anio DESC,
        Mes DESC;
END;
GO

/* =========================================================
   PERIODO CONTABLE - LISTAR ABIERTOS
   ========================================================= */
CREATE OR ALTER PROCEDURE Contabilidad.sp_PeriodoContable_ListarAbiertos
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        Anio,
        Mes,
        FechaInicio,
        FechaFin,
        Estado,
        Activo
    FROM dbo.PeriodoContable
    WHERE Activo = 1
      AND Estado = 'Abierto'
    ORDER BY
        Anio DESC,
        Mes DESC;
END;
GO

/* =========================================================
   PERIODO CONTABLE - OBTENER POR ANIO Y MES
   ========================================================= */
CREATE OR ALTER PROCEDURE Contabilidad.sp_PeriodoContable_Obtener
    @Anio INT,
    @Mes INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        Anio,
        Mes,
        FechaInicio,
        FechaFin,
        Estado,
        Activo
    FROM dbo.PeriodoContable
    WHERE Anio = @Anio
      AND Mes = @Mes;
END;
GO

/* =========================================================
   PERIODO CONTABLE - REGISTRAR
   ========================================================= */
CREATE OR ALTER PROCEDURE Contabilidad.sp_PeriodoContable_Registrar
    @Anio INT,
    @Mes INT,
    @FechaInicio DATETIME,
    @FechaFin DATETIME,
    @Estado VARCHAR(45),
    @Resultado INT OUTPUT,
    @Mensaje VARCHAR(500) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    SET @Resultado = 0;
    SET @Mensaje = '';

    SET @Estado = LTRIM(RTRIM(ISNULL(@Estado, '')));

    IF (@Anio < 2000 OR @Anio > 2100)
    BEGIN
        SET @Mensaje = 'Debe ingresar un año válido.';
        RETURN;
    END;

    IF (@Mes < 1 OR @Mes > 12)
    BEGIN
        SET @Mensaje = 'Debe ingresar un mes válido.';
        RETURN;
    END;

    IF (@FechaInicio IS NULL OR @FechaFin IS NULL)
    BEGIN
        SET @Mensaje = 'Debe ingresar la fecha de inicio y la fecha final.';
        RETURN;
    END;

    IF (@FechaInicio > @FechaFin)
    BEGIN
        SET @Mensaje = 'La fecha de inicio no puede ser mayor que la fecha final.';
        RETURN;
    END;

    IF (@Estado NOT IN ('Abierto', 'Cerrado'))
    BEGIN
        SET @Mensaje = 'El estado debe ser Abierto o Cerrado.';
        RETURN;
    END;

    IF EXISTS (
        SELECT 1
        FROM dbo.PeriodoContable
        WHERE Anio = @Anio
          AND Mes = @Mes
    )
    BEGIN
        SET @Mensaje = 'Ya existe un período contable para ese año y mes.';
        RETURN;
    END;

    IF (@Estado = 'Abierto')
    BEGIN
        IF EXISTS (
            SELECT 1
            FROM dbo.PeriodoContable
            WHERE Estado = 'Abierto'
              AND Activo = 1
        )
        BEGIN
            SET @Mensaje = 'Ya existe un período contable abierto. Debe cerrarlo antes de abrir otro.';
            RETURN;
        END;
    END;

    BEGIN TRY
        INSERT INTO dbo.PeriodoContable
        (
            Anio,
            Mes,
            FechaInicio,
            FechaFin,
            Estado,
            Activo
        )
        VALUES
        (
            @Anio,
            @Mes,
            @FechaInicio,
            @FechaFin,
            @Estado,
            1
        );

        SET @Resultado = 1;
        SET @Mensaje = 'Período contable registrado correctamente.';
    END TRY
    BEGIN CATCH
        SET @Resultado = 0;
        SET @Mensaje = ERROR_MESSAGE();
    END CATCH;
END;
GO

/* =========================================================
   PERIODO CONTABLE - EDITAR
   Nota:
   Como Anio + Mes es PK, no conviene permitir cambiar Anio/Mes.
   Solo se editan fechas, estado y activo.
   ========================================================= */
CREATE OR ALTER PROCEDURE Contabilidad.sp_PeriodoContable_Editar
    @Anio INT,
    @Mes INT,
    @FechaInicio DATETIME,
    @FechaFin DATETIME,
    @Estado VARCHAR(45),
    @Activo BIT,
    @Resultado BIT OUTPUT,
    @Mensaje VARCHAR(500) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    SET @Resultado = 0;
    SET @Mensaje = '';

    SET @Estado = LTRIM(RTRIM(ISNULL(@Estado, '')));

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
    )
    BEGIN
        SET @Mensaje = 'El período contable no existe.';
        RETURN;
    END;

    IF (@FechaInicio IS NULL OR @FechaFin IS NULL)
    BEGIN
        SET @Mensaje = 'Debe ingresar la fecha de inicio y la fecha final.';
        RETURN;
    END;

    IF (@FechaInicio > @FechaFin)
    BEGIN
        SET @Mensaje = 'La fecha de inicio no puede ser mayor que la fecha final.';
        RETURN;
    END;

    IF (@Estado NOT IN ('Abierto', 'Cerrado'))
    BEGIN
        SET @Mensaje = 'El estado debe ser Abierto o Cerrado.';
        RETURN;
    END;

    IF (@Estado = 'Abierto' AND @Activo = 1)
    BEGIN
        IF EXISTS (
            SELECT 1
            FROM dbo.PeriodoContable
            WHERE Estado = 'Abierto'
              AND Activo = 1
              AND NOT (Anio = @Anio AND Mes = @Mes)
        )
        BEGIN
            SET @Mensaje = 'Ya existe otro período contable abierto.';
            RETURN;
        END;
    END;

    IF EXISTS (
        SELECT 1
        FROM dbo.AsientoContable
        WHERE Anio = @Anio
          AND Mes = @Mes
          AND Activo = 1
    )
    BEGIN
        IF (@Activo = 0)
        BEGIN
            SET @Mensaje = 'No se puede inactivar el período porque tiene asientos contables activos.';
            RETURN;
        END;
    END;

    BEGIN TRY
        UPDATE dbo.PeriodoContable
        SET
            FechaInicio = @FechaInicio,
            FechaFin = @FechaFin,
            Estado = @Estado,
            Activo = @Activo
        WHERE Anio = @Anio
          AND Mes = @Mes;

        SET @Resultado = 1;
        SET @Mensaje = 'Período contable actualizado correctamente.';
    END TRY
    BEGIN CATCH
        SET @Resultado = 0;
        SET @Mensaje = ERROR_MESSAGE();
    END CATCH;
END;
GO

/* =========================================================
   PERIODO CONTABLE - CERRAR
   ========================================================= */
CREATE OR ALTER PROCEDURE Contabilidad.sp_PeriodoContable_Cerrar
    @Anio INT,
    @Mes INT,
    @Resultado BIT OUTPUT,
    @Mensaje VARCHAR(500) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    SET @Resultado = 0;
    SET @Mensaje = '';

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
          AND Activo = 1
    )
    BEGIN
        SET @Mensaje = 'El período contable no existe o está inactivo.';
        RETURN;
    END;

    IF EXISTS (
        SELECT 1
        FROM dbo.PeriodoContable
        WHERE Anio = @Anio
          AND Mes = @Mes
          AND Estado = 'Cerrado'
    )
    BEGIN
        SET @Mensaje = 'El período contable ya se encuentra cerrado.';
        RETURN;
    END;

    BEGIN TRY
        UPDATE dbo.PeriodoContable
        SET Estado = 'Cerrado'
        WHERE Anio = @Anio
          AND Mes = @Mes;

        SET @Resultado = 1;
        SET @Mensaje = 'Período contable cerrado correctamente.';
    END TRY
    BEGIN CATCH
        SET @Resultado = 0;
        SET @Mensaje = ERROR_MESSAGE();
    END CATCH;
END;
GO

/* =========================================================
   PERIODO CONTABLE - INACTIVAR
   ========================================================= */
CREATE OR ALTER PROCEDURE Contabilidad.sp_PeriodoContable_Inactivar
    @Anio INT,
    @Mes INT,
    @Resultado BIT OUTPUT,
    @Mensaje VARCHAR(500) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    SET @Resultado = 0;
    SET @Mensaje = '';

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
    )
    BEGIN
        SET @Mensaje = 'El período contable no existe.';
        RETURN;
    END;

    IF EXISTS (
        SELECT 1
        FROM dbo.PeriodoContable
        WHERE Anio = @Anio
          AND Mes = @Mes
          AND Estado = 'Abierto'
          AND Activo = 1
    )
    BEGIN
        SET @Mensaje = 'No se puede inactivar un período abierto. Primero debe cerrarlo.';
        RETURN;
    END;

    IF EXISTS (
        SELECT 1
        FROM dbo.AsientoContable
        WHERE Anio = @Anio
          AND Mes = @Mes
          AND Activo = 1
    )
    BEGIN
        SET @Mensaje = 'No se puede inactivar el período porque tiene asientos contables activos.';
        RETURN;
    END;

    BEGIN TRY
        UPDATE dbo.PeriodoContable
        SET Activo = 0
        WHERE Anio = @Anio
          AND Mes = @Mes;

        SET @Resultado = 1;
        SET @Mensaje = 'Período contable inactivado correctamente.';
    END TRY
    BEGIN CATCH
        SET @Resultado = 0;
        SET @Mensaje = ERROR_MESSAGE();
    END CATCH;
END;
GO