IF NOT EXISTS (SELECT 1 FROM sys.schemas WHERE name = 'Catalogos')
BEGIN
    EXEC('CREATE SCHEMA Catalogos');
END;
GO

CREATE OR ALTER PROCEDURE Catalogos.sp_TipoIngreso_Listar
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        IdTipoIngreso,
        Nombre,
        Descripcion,
        Activo
    FROM dbo.TipoIngreso
    ORDER BY Nombre;
END;
GO

CREATE OR ALTER PROCEDURE Catalogos.sp_TipoIngreso_ListarActivos
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        IdTipoIngreso,
        Nombre,
        Descripcion,
        Activo
    FROM dbo.TipoIngreso
    WHERE Activo = 1
    ORDER BY Nombre;
END;
GO

CREATE OR ALTER PROCEDURE Catalogos.sp_TipoIngreso_Registrar
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
        SET @Mensaje = 'El nombre del tipo de ingreso es obligatorio.';
        RETURN;
    END;

    IF (@Descripcion = '')
    BEGIN
        SET @Mensaje = 'La descripción es obligatoria.';
        RETURN;
    END;

    IF EXISTS (
        SELECT 1
        FROM dbo.TipoIngreso
        WHERE UPPER(LTRIM(RTRIM(Nombre))) = UPPER(@Nombre)
    )
    BEGIN
        SET @Mensaje = 'Ya existe un tipo de ingreso con ese nombre.';
        RETURN;
    END;

    INSERT INTO dbo.TipoIngreso
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
    SET @Mensaje = 'Tipo de ingreso registrado correctamente.';
END;
GO

CREATE OR ALTER PROCEDURE Catalogos.sp_TipoIngreso_Editar
    @IdTipoIngreso INT,
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

    IF (@IdTipoIngreso <= 0)
    BEGIN
        SET @Mensaje = 'Debe seleccionar un tipo de ingreso válido.';
        RETURN;
    END;

    IF (@Nombre = '')
    BEGIN
        SET @Mensaje = 'El nombre del tipo de ingreso es obligatorio.';
        RETURN;
    END;

    IF (@Descripcion = '')
    BEGIN
        SET @Mensaje = 'La descripción es obligatoria.';
        RETURN;
    END;

    IF NOT EXISTS (
        SELECT 1
        FROM dbo.TipoIngreso
        WHERE IdTipoIngreso = @IdTipoIngreso
    )
    BEGIN
        SET @Mensaje = 'El tipo de ingreso no existe.';
        RETURN;
    END;

    IF EXISTS (
        SELECT 1
        FROM dbo.TipoIngreso
        WHERE UPPER(LTRIM(RTRIM(Nombre))) = UPPER(@Nombre)
          AND IdTipoIngreso <> @IdTipoIngreso
    )
    BEGIN
        SET @Mensaje = 'Ya existe otro tipo de ingreso con ese nombre.';
        RETURN;
    END;

    UPDATE dbo.TipoIngreso
    SET
        Nombre = @Nombre,
        Descripcion = @Descripcion,
        Activo = @Activo
    WHERE IdTipoIngreso = @IdTipoIngreso;

    SET @Resultado = 1;
    SET @Mensaje = 'Tipo de ingreso actualizado correctamente.';
END;
GO

CREATE OR ALTER PROCEDURE Catalogos.sp_TipoIngreso_Inactivar
    @IdTipoIngreso INT,
    @Resultado BIT OUTPUT,
    @Mensaje VARCHAR(500) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    SET @Resultado = 0;
    SET @Mensaje = '';

    IF (@IdTipoIngreso <= 0)
    BEGIN
        SET @Mensaje = 'Debe seleccionar un tipo de ingreso válido.';
        RETURN;
    END;

    IF NOT EXISTS (
        SELECT 1
        FROM dbo.TipoIngreso
        WHERE IdTipoIngreso = @IdTipoIngreso
    )
    BEGIN
        SET @Mensaje = 'El tipo de ingreso no existe.';
        RETURN;
    END;

    IF EXISTS (
        SELECT 1
        FROM dbo.Ingreso
        WHERE IdTipoIngreso = @IdTipoIngreso
          AND Activo = 1
    )
    BEGIN
        SET @Mensaje = 'No se puede inactivar el tipo de ingreso porque está asociado a ingresos activos.';
        RETURN;
    END;

    UPDATE dbo.TipoIngreso
    SET Activo = 0
    WHERE IdTipoIngreso = @IdTipoIngreso;

    SET @Resultado = 1;
    SET @Mensaje = 'Tipo de ingreso inactivado correctamente.';
END;
GO