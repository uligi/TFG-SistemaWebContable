IF NOT EXISTS (SELECT 1 FROM sys.schemas WHERE name = 'Catalogos')
BEGIN
    EXEC('CREATE SCHEMA Catalogos');
END;
GO

CREATE OR ALTER PROCEDURE Catalogos.sp_TipoCorreo_Listar
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        IdTipoCorreo,
        Nombre,
        Activo
    FROM dbo.TipoCorreo
    ORDER BY Nombre;
END;
GO

CREATE OR ALTER PROCEDURE Catalogos.sp_TipoCorreo_Registrar
    @Nombre VARCHAR(45),
    @Resultado INT OUTPUT,
    @Mensaje VARCHAR(500) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    SET @Resultado = 0;
    SET @Mensaje = '';

    SET @Nombre = LTRIM(RTRIM(@Nombre));

    IF (@Nombre = '')
    BEGIN
        SET @Mensaje = 'El nombre del tipo de correo es obligatorio.';
        RETURN;
    END;

    IF EXISTS (
        SELECT 1
        FROM dbo.TipoCorreo
        WHERE UPPER(LTRIM(RTRIM(Nombre))) = UPPER(@Nombre)
    )
    BEGIN
        SET @Mensaje = 'Ya existe un tipo de correo con ese nombre.';
        RETURN;
    END;

    INSERT INTO dbo.TipoCorreo
    (
        Nombre,
        Activo
    )
    VALUES
    (
        @Nombre,
        1
    );

    SET @Resultado = SCOPE_IDENTITY();
    SET @Mensaje = 'Tipo de correo registrado correctamente.';
END;
GO

CREATE OR ALTER PROCEDURE Catalogos.sp_TipoCorreo_Editar
    @IdTipoCorreo INT,
    @Nombre VARCHAR(45),
    @Activo BIT,
    @Resultado BIT OUTPUT,
    @Mensaje VARCHAR(500) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    SET @Resultado = 0;
    SET @Mensaje = '';

    SET @Nombre = LTRIM(RTRIM(@Nombre));

    IF (@IdTipoCorreo <= 0)
    BEGIN
        SET @Mensaje = 'Debe seleccionar un tipo de correo válido.';
        RETURN;
    END;

    IF (@Nombre = '')
    BEGIN
        SET @Mensaje = 'El nombre del tipo de correo es obligatorio.';
        RETURN;
    END;

    IF NOT EXISTS (
        SELECT 1
        FROM dbo.TipoCorreo
        WHERE IdTipoCorreo = @IdTipoCorreo
    )
    BEGIN
        SET @Mensaje = 'El tipo de correo no existe.';
        RETURN;
    END;

    IF EXISTS (
        SELECT 1
        FROM dbo.TipoCorreo
        WHERE UPPER(LTRIM(RTRIM(Nombre))) = UPPER(@Nombre)
          AND IdTipoCorreo <> @IdTipoCorreo
    )
    BEGIN
        SET @Mensaje = 'Ya existe otro tipo de correo con ese nombre.';
        RETURN;
    END;

    UPDATE dbo.TipoCorreo
    SET
        Nombre = @Nombre,
        Activo = @Activo
    WHERE IdTipoCorreo = @IdTipoCorreo;

    SET @Resultado = 1;
    SET @Mensaje = 'Tipo de correo actualizado correctamente.';
END;
GO

CREATE OR ALTER PROCEDURE Catalogos.sp_TipoCorreo_Inactivar
    @IdTipoCorreo INT,
    @Resultado BIT OUTPUT,
    @Mensaje VARCHAR(500) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    SET @Resultado = 0;
    SET @Mensaje = '';

    IF (@IdTipoCorreo <= 0)
    BEGIN
        SET @Mensaje = 'Debe seleccionar un tipo de correo válido.';
        RETURN;
    END;

    IF NOT EXISTS (
        SELECT 1
        FROM dbo.TipoCorreo
        WHERE IdTipoCorreo = @IdTipoCorreo
    )
    BEGIN
        SET @Mensaje = 'El tipo de correo no existe.';
        RETURN;
    END;

    IF EXISTS (
        SELECT 1
        FROM dbo.Correo
        WHERE IdTipoCorreo = @IdTipoCorreo
          AND Activo = 1
    )
    BEGIN
        SET @Mensaje = 'No se puede inactivar el tipo de correo porque tiene correos de personas activos asociados.';
        RETURN;
    END;

    IF EXISTS (
        SELECT 1
        FROM dbo.CorreoProveedor
        WHERE IdTipoCorreo = @IdTipoCorreo
          AND Activo = 1
    )
    BEGIN
        SET @Mensaje = 'No se puede inactivar el tipo de correo porque tiene correos de proveedores activos asociados.';
        RETURN;
    END;

    UPDATE dbo.TipoCorreo
    SET Activo = 0
    WHERE IdTipoCorreo = @IdTipoCorreo;

    SET @Resultado = 1;
    SET @Mensaje = 'Tipo de correo inactivado correctamente.';
END;
GO