IF NOT EXISTS (SELECT 1 FROM sys.schemas WHERE name = 'Catalogos')
BEGIN
    EXEC('CREATE SCHEMA Catalogos');
END;
GO

CREATE OR ALTER PROCEDURE Catalogos.sp_TipoGasto_Listar
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        IdTipoGasto,
        Nombre,
        Descripcion,
        Activo
    FROM dbo.TipoGasto
    ORDER BY Nombre;
END;
GO

CREATE OR ALTER PROCEDURE Catalogos.sp_TipoGasto_ListarActivos
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        IdTipoGasto,
        Nombre,
        Descripcion,
        Activo
    FROM dbo.TipoGasto
    WHERE Activo = 1
    ORDER BY Nombre;
END;
GO

CREATE OR ALTER PROCEDURE Catalogos.sp_TipoGasto_Registrar
    @Nombre VARCHAR(45),
    @Descripcion VARCHAR(255),
    @Resultado INT OUTPUT,
    @Mensaje VARCHAR(500) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    SET @Resultado = 0;
    SET @Mensaje = '';

    SET @Nombre = LTRIM(RTRIM(@Nombre));
    SET @Descripcion = LTRIM(RTRIM(ISNULL(@Descripcion, '')));

    IF (@Nombre = '')
    BEGIN
        SET @Mensaje = 'El nombre del tipo de gasto es obligatorio.';
        RETURN;
    END;

    IF (@Descripcion = '')
    BEGIN
        SET @Mensaje = 'La descripción es obligatoria.';
        RETURN;
    END;

    IF EXISTS (
        SELECT 1
        FROM dbo.TipoGasto
        WHERE UPPER(LTRIM(RTRIM(Nombre))) = UPPER(@Nombre)
    )
    BEGIN
        SET @Mensaje = 'Ya existe un tipo de gasto con ese nombre.';
        RETURN;
    END;

    INSERT INTO dbo.TipoGasto
    (
        Nombre,
        Descripcion,
        Activo
    )
    VALUES
    (
        @Nombre,
        @Descripcion,
        1
    );

    SET @Resultado = SCOPE_IDENTITY();
    SET @Mensaje = 'Tipo de gasto registrado correctamente.';
END;
GO

CREATE OR ALTER PROCEDURE Catalogos.sp_TipoGasto_Editar
    @IdTipoGasto INT,
    @Nombre VARCHAR(45),
    @Descripcion VARCHAR(255),
    @Activo BIT,
    @Resultado BIT OUTPUT,
    @Mensaje VARCHAR(500) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    SET @Resultado = 0;
    SET @Mensaje = '';

    SET @Nombre = LTRIM(RTRIM(@Nombre));
    SET @Descripcion = LTRIM(RTRIM(ISNULL(@Descripcion, '')));

    IF (@IdTipoGasto <= 0)
    BEGIN
        SET @Mensaje = 'Debe seleccionar un tipo de gasto válido.';
        RETURN;
    END;

    IF (@Nombre = '')
    BEGIN
        SET @Mensaje = 'El nombre del tipo de gasto es obligatorio.';
        RETURN;
    END;

    IF (@Descripcion = '')
    BEGIN
        SET @Mensaje = 'La descripción es obligatoria.';
        RETURN;
    END;

    IF NOT EXISTS (
        SELECT 1
        FROM dbo.TipoGasto
        WHERE IdTipoGasto = @IdTipoGasto
    )
    BEGIN
        SET @Mensaje = 'El tipo de gasto no existe.';
        RETURN;
    END;

    IF EXISTS (
        SELECT 1
        FROM dbo.TipoGasto
        WHERE UPPER(LTRIM(RTRIM(Nombre))) = UPPER(@Nombre)
          AND IdTipoGasto <> @IdTipoGasto
    )
    BEGIN
        SET @Mensaje = 'Ya existe otro tipo de gasto con ese nombre.';
        RETURN;
    END;

    UPDATE dbo.TipoGasto
    SET
        Nombre = @Nombre,
        Descripcion = @Descripcion,
        Activo = @Activo
    WHERE IdTipoGasto = @IdTipoGasto;

    SET @Resultado = 1;
    SET @Mensaje = 'Tipo de gasto actualizado correctamente.';
END;
GO

CREATE OR ALTER PROCEDURE Catalogos.sp_TipoGasto_Inactivar
    @IdTipoGasto INT,
    @Resultado BIT OUTPUT,
    @Mensaje VARCHAR(500) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    SET @Resultado = 0;
    SET @Mensaje = '';

    IF (@IdTipoGasto <= 0)
    BEGIN
        SET @Mensaje = 'Debe seleccionar un tipo de gasto válido.';
        RETURN;
    END;

    IF NOT EXISTS (
        SELECT 1
        FROM dbo.TipoGasto
        WHERE IdTipoGasto = @IdTipoGasto
    )
    BEGIN
        SET @Mensaje = 'El tipo de gasto no existe.';
        RETURN;
    END;

    IF EXISTS (
        SELECT 1
        FROM dbo.Gasto
        WHERE IdTipoGasto = @IdTipoGasto
          AND Activo = 1
    )
    BEGIN
        SET @Mensaje = 'No se puede inactivar el tipo de gasto porque está asociado a gastos activos.';
        RETURN;
    END;

    UPDATE dbo.TipoGasto
    SET Activo = 0
    WHERE IdTipoGasto = @IdTipoGasto;

    SET @Resultado = 1;
    SET @Mensaje = 'Tipo de gasto inactivado correctamente.';
END;
GO