IF NOT EXISTS (SELECT 1 FROM sys.schemas WHERE name = 'Catalogos')
BEGIN
    EXEC('CREATE SCHEMA Catalogos');
END;
GO

CREATE OR ALTER PROCEDURE Catalogos.sp_TipoDeObservacion_Listar
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        IdTipoDeObservacion,
        TipoDeObservacion,
        Descripcion,
        Activo
    FROM dbo.TipoDeObservacion
    ORDER BY TipoDeObservacion;
END;
GO

CREATE OR ALTER PROCEDURE Catalogos.sp_TipoDeObservacion_Registrar
    @TipoDeObservacion VARCHAR(45),
    @Descripcion VARCHAR(150),
    @Resultado INT OUTPUT,
    @Mensaje VARCHAR(500) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    SET @Resultado = 0;
    SET @Mensaje = '';

    SET @TipoDeObservacion = LTRIM(RTRIM(@TipoDeObservacion));
    SET @Descripcion = LTRIM(RTRIM(ISNULL(@Descripcion, '')));

    IF (@TipoDeObservacion = '')
    BEGIN
        SET @Mensaje = 'El nombre del tipo de observación es obligatorio.';
        RETURN;
    END;

    IF EXISTS (
        SELECT 1
        FROM dbo.TipoDeObservacion
        WHERE UPPER(LTRIM(RTRIM(TipoDeObservacion))) = UPPER(@TipoDeObservacion)
    )
    BEGIN
        SET @Mensaje = 'Ya existe un tipo de observación con ese nombre.';
        RETURN;
    END;

    INSERT INTO dbo.TipoDeObservacion
    (
        TipoDeObservacion,
        Descripcion,
        Activo
    )
    VALUES
    (
        @TipoDeObservacion,
        @Descripcion,
        1
    );

    SET @Resultado = SCOPE_IDENTITY();
    SET @Mensaje = 'Tipo de observación registrado correctamente.';
END;
GO

CREATE OR ALTER PROCEDURE Catalogos.sp_TipoDeObservacion_Editar
    @IdTipoDeObservacion INT,
    @TipoDeObservacion VARCHAR(45),
    @Descripcion VARCHAR(150),
    @Activo BIT,
    @Resultado BIT OUTPUT,
    @Mensaje VARCHAR(500) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    SET @Resultado = 0;
    SET @Mensaje = '';

    SET @TipoDeObservacion = LTRIM(RTRIM(@TipoDeObservacion));
    SET @Descripcion = LTRIM(RTRIM(ISNULL(@Descripcion, '')));

    IF (@IdTipoDeObservacion <= 0)
    BEGIN
        SET @Mensaje = 'Debe seleccionar un tipo de observación válido.';
        RETURN;
    END;

    IF (@TipoDeObservacion = '')
    BEGIN
        SET @Mensaje = 'El nombre del tipo de observación es obligatorio.';
        RETURN;
    END;

    IF NOT EXISTS (
        SELECT 1
        FROM dbo.TipoDeObservacion
        WHERE IdTipoDeObservacion = @IdTipoDeObservacion
    )
    BEGIN
        SET @Mensaje = 'El tipo de observación no existe.';
        RETURN;
    END;

    IF EXISTS (
        SELECT 1
        FROM dbo.TipoDeObservacion
        WHERE UPPER(LTRIM(RTRIM(TipoDeObservacion))) = UPPER(@TipoDeObservacion)
          AND IdTipoDeObservacion <> @IdTipoDeObservacion
    )
    BEGIN
        SET @Mensaje = 'Ya existe otro tipo de observación con ese nombre.';
        RETURN;
    END;

    UPDATE dbo.TipoDeObservacion
    SET
        TipoDeObservacion = @TipoDeObservacion,
        Descripcion = @Descripcion,
        Activo = @Activo
    WHERE IdTipoDeObservacion = @IdTipoDeObservacion;

    SET @Resultado = 1;
    SET @Mensaje = 'Tipo de observación actualizado correctamente.';
END;
GO

CREATE OR ALTER PROCEDURE Catalogos.sp_TipoDeObservacion_Inactivar
    @IdTipoDeObservacion INT,
    @Resultado BIT OUTPUT,
    @Mensaje VARCHAR(500) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    SET @Resultado = 0;
    SET @Mensaje = '';

    IF (@IdTipoDeObservacion <= 0)
    BEGIN
        SET @Mensaje = 'Debe seleccionar un tipo de observación válido.';
        RETURN;
    END;

    IF NOT EXISTS (
        SELECT 1
        FROM dbo.TipoDeObservacion
        WHERE IdTipoDeObservacion = @IdTipoDeObservacion
    )
    BEGIN
        SET @Mensaje = 'El tipo de observación no existe.';
        RETURN;
    END;

    IF EXISTS (
        SELECT 1
        FROM dbo.Observacion
        WHERE IdTipoDeObservacion = @IdTipoDeObservacion
          AND Activo = 1
    )
    BEGIN
        SET @Mensaje = 'No se puede inactivar el tipo de observación porque tiene observaciones activas asociadas.';
        RETURN;
    END;

    UPDATE dbo.TipoDeObservacion
    SET Activo = 0
    WHERE IdTipoDeObservacion = @IdTipoDeObservacion;

    SET @Resultado = 1;
    SET @Mensaje = 'Tipo de observación inactivado correctamente.';
END;
GO