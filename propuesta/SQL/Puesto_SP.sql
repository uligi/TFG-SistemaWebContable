IF NOT EXISTS (SELECT 1 FROM sys.schemas WHERE name = 'Catalogos')
BEGIN
    EXEC('CREATE SCHEMA Catalogos');
END;
GO

CREATE OR ALTER PROCEDURE Catalogos.sp_Puesto_Listar
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        IdPuesto,
        Nombre,
        Descripcion,
        Activo
    FROM dbo.Puesto
    ORDER BY Nombre;
END;
GO

CREATE OR ALTER PROCEDURE Catalogos.sp_Puesto_ListarActivos
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        IdPuesto,
        Nombre,
        Descripcion,
        Activo
    FROM dbo.Puesto
    WHERE Activo = 1
    ORDER BY Nombre;
END;
GO

CREATE OR ALTER PROCEDURE Catalogos.sp_Puesto_Registrar
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
        SET @Mensaje = 'El nombre del puesto es obligatorio.';
        RETURN;
    END;

    IF (@Descripcion = '')
    BEGIN
        SET @Mensaje = 'La descripción es obligatoria.';
        RETURN;
    END;

    IF EXISTS (
        SELECT 1
        FROM dbo.Puesto
        WHERE UPPER(LTRIM(RTRIM(Nombre))) = UPPER(@Nombre)
    )
    BEGIN
        SET @Mensaje = 'Ya existe un puesto con ese nombre.';
        RETURN;
    END;

    INSERT INTO dbo.Puesto
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
    SET @Mensaje = 'Puesto registrado correctamente.';
END;
GO

CREATE OR ALTER PROCEDURE Catalogos.sp_Puesto_Editar
    @IdPuesto INT,
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

    IF (@IdPuesto <= 0)
    BEGIN
        SET @Mensaje = 'Debe seleccionar un puesto válido.';
        RETURN;
    END;

    IF (@Nombre = '')
    BEGIN
        SET @Mensaje = 'El nombre del puesto es obligatorio.';
        RETURN;
    END;

    IF (@Descripcion = '')
    BEGIN
        SET @Mensaje = 'La descripción es obligatoria.';
        RETURN;
    END;

    IF NOT EXISTS (
        SELECT 1
        FROM dbo.Puesto
        WHERE IdPuesto = @IdPuesto
    )
    BEGIN
        SET @Mensaje = 'El puesto no existe.';
        RETURN;
    END;

    IF EXISTS (
        SELECT 1
        FROM dbo.Puesto
        WHERE UPPER(LTRIM(RTRIM(Nombre))) = UPPER(@Nombre)
          AND IdPuesto <> @IdPuesto
    )
    BEGIN
        SET @Mensaje = 'Ya existe otro puesto con ese nombre.';
        RETURN;
    END;

    UPDATE dbo.Puesto
    SET
        Nombre = @Nombre,
        Descripcion = @Descripcion,
        Activo = @Activo
    WHERE IdPuesto = @IdPuesto;

    SET @Resultado = 1;
    SET @Mensaje = 'Puesto actualizado correctamente.';
END;
GO

CREATE OR ALTER PROCEDURE Catalogos.sp_Puesto_Inactivar
    @IdPuesto INT,
    @Resultado BIT OUTPUT,
    @Mensaje VARCHAR(500) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    SET @Resultado = 0;
    SET @Mensaje = '';

    IF (@IdPuesto <= 0)
    BEGIN
        SET @Mensaje = 'Debe seleccionar un puesto válido.';
        RETURN;
    END;

    IF NOT EXISTS (
        SELECT 1
        FROM dbo.Puesto
        WHERE IdPuesto = @IdPuesto
    )
    BEGIN
        SET @Mensaje = 'El puesto no existe.';
        RETURN;
    END;

    IF EXISTS (
        SELECT 1
        FROM dbo.Empleado
        WHERE IdPuesto = @IdPuesto
          AND Activo = 1
    )
    BEGIN
        SET @Mensaje = 'No se puede inactivar el puesto porque está asociado a empleados activos.';
        RETURN;
    END;

    UPDATE dbo.Puesto
    SET Activo = 0
    WHERE IdPuesto = @IdPuesto;

    SET @Resultado = 1;
    SET @Mensaje = 'Puesto inactivado correctamente.';
END;
GO