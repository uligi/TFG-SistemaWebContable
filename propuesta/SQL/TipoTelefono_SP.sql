IF NOT EXISTS (SELECT 1 FROM sys.schemas WHERE name = 'Catalogos')
BEGIN
    EXEC('CREATE SCHEMA Catalogos');
END;
GO

CREATE OR ALTER PROCEDURE Catalogos.sp_TipoTelefono_Listar
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        IdTipoTelefono,
        Nombre,
        Activo
    FROM dbo.TipoTelefono
    ORDER BY Nombre;
END;
GO

CREATE OR ALTER PROCEDURE Catalogos.sp_TipoTelefono_Registrar
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
        SET @Mensaje = 'El nombre del tipo de teléfono es obligatorio.';
        RETURN;
    END;

    IF EXISTS (
        SELECT 1
        FROM dbo.TipoTelefono
        WHERE UPPER(LTRIM(RTRIM(Nombre))) = UPPER(@Nombre)
    )
    BEGIN
        SET @Mensaje = 'Ya existe un tipo de teléfono con ese nombre.';
        RETURN;
    END;

    INSERT INTO dbo.TipoTelefono
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
    SET @Mensaje = 'Tipo de teléfono registrado correctamente.';
END;
GO

CREATE OR ALTER PROCEDURE Catalogos.sp_TipoTelefono_Editar
    @IdTipoTelefono INT,
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

    IF (@IdTipoTelefono <= 0)
    BEGIN
        SET @Mensaje = 'Debe seleccionar un tipo de teléfono válido.';
        RETURN;
    END;

    IF (@Nombre = '')
    BEGIN
        SET @Mensaje = 'El nombre del tipo de teléfono es obligatorio.';
        RETURN;
    END;

    IF NOT EXISTS (
        SELECT 1
        FROM dbo.TipoTelefono
        WHERE IdTipoTelefono = @IdTipoTelefono
    )
    BEGIN
        SET @Mensaje = 'El tipo de teléfono no existe.';
        RETURN;
    END;

    IF EXISTS (
        SELECT 1
        FROM dbo.TipoTelefono
        WHERE UPPER(LTRIM(RTRIM(Nombre))) = UPPER(@Nombre)
          AND IdTipoTelefono <> @IdTipoTelefono
    )
    BEGIN
        SET @Mensaje = 'Ya existe otro tipo de teléfono con ese nombre.';
        RETURN;
    END;

    UPDATE dbo.TipoTelefono
    SET
        Nombre = @Nombre,
        Activo = @Activo
    WHERE IdTipoTelefono = @IdTipoTelefono;

    SET @Resultado = 1;
    SET @Mensaje = 'Tipo de teléfono actualizado correctamente.';
END;
GO

CREATE OR ALTER PROCEDURE Catalogos.sp_TipoTelefono_Inactivar
    @IdTipoTelefono INT,
    @Resultado BIT OUTPUT,
    @Mensaje VARCHAR(500) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    SET @Resultado = 0;
    SET @Mensaje = '';

    IF (@IdTipoTelefono <= 0)
    BEGIN
        SET @Mensaje = 'Debe seleccionar un tipo de teléfono válido.';
        RETURN;
    END;

    IF NOT EXISTS (
        SELECT 1
        FROM dbo.TipoTelefono
        WHERE IdTipoTelefono = @IdTipoTelefono
    )
    BEGIN
        SET @Mensaje = 'El tipo de teléfono no existe.';
        RETURN;
    END;

    IF EXISTS (
        SELECT 1
        FROM dbo.Telefono
        WHERE IdTipoTelefono = @IdTipoTelefono
          AND Activo = 1
    )
    BEGIN
        SET @Mensaje = 'No se puede inactivar el tipo de teléfono porque tiene teléfonos de personas activos asociados.';
        RETURN;
    END;

    IF EXISTS (
        SELECT 1
        FROM dbo.TelefonoProveedor
        WHERE IdTipoTelefono = @IdTipoTelefono
          AND Activo = 1
    )
    BEGIN
        SET @Mensaje = 'No se puede inactivar el tipo de teléfono porque tiene teléfonos de proveedores activos asociados.';
        RETURN;
    END;

    UPDATE dbo.TipoTelefono
    SET Activo = 0
    WHERE IdTipoTelefono = @IdTipoTelefono;

    SET @Resultado = 1;
    SET @Mensaje = 'Tipo de teléfono inactivado correctamente.';
END;
GO