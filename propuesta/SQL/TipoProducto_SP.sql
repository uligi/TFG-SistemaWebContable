IF NOT EXISTS (SELECT 1 FROM sys.schemas WHERE name = 'Catalogos')
BEGIN
    EXEC('CREATE SCHEMA Catalogos');
END;
GO

CREATE OR ALTER PROCEDURE Catalogos.sp_TipoProducto_Listar
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        IdTipoProducto,
        Nombre,
        Descripcion,
        Activo
    FROM dbo.TipoProducto
    ORDER BY Nombre;
END;
GO

CREATE OR ALTER PROCEDURE Catalogos.sp_TipoProducto_ListarActivos
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        IdTipoProducto,
        Nombre,
        Descripcion,
        Activo
    FROM dbo.TipoProducto
    WHERE Activo = 1
    ORDER BY Nombre;
END;
GO

CREATE OR ALTER PROCEDURE Catalogos.sp_TipoProducto_Registrar
    @Nombre VARCHAR(100),
    @Descripcion VARCHAR(150),
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
        SET @Mensaje = 'El nombre del tipo de producto es obligatorio.';
        RETURN;
    END;

    IF (@Descripcion = '')
    BEGIN
        SET @Mensaje = 'La descripción es obligatoria.';
        RETURN;
    END;

    IF EXISTS (
        SELECT 1
        FROM dbo.TipoProducto
        WHERE UPPER(LTRIM(RTRIM(Nombre))) = UPPER(@Nombre)
    )
    BEGIN
        SET @Mensaje = 'Ya existe un tipo de producto con ese nombre.';
        RETURN;
    END;

    INSERT INTO dbo.TipoProducto
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
    SET @Mensaje = 'Tipo de producto registrado correctamente.';
END;
GO

CREATE OR ALTER PROCEDURE Catalogos.sp_TipoProducto_Editar
    @IdTipoProducto INT,
    @Nombre VARCHAR(100),
    @Descripcion VARCHAR(150),
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

    IF (@IdTipoProducto <= 0)
    BEGIN
        SET @Mensaje = 'Debe seleccionar un tipo de producto válido.';
        RETURN;
    END;

    IF (@Nombre = '')
    BEGIN
        SET @Mensaje = 'El nombre del tipo de producto es obligatorio.';
        RETURN;
    END;

    IF (@Descripcion = '')
    BEGIN
        SET @Mensaje = 'La descripción es obligatoria.';
        RETURN;
    END;

    IF NOT EXISTS (
        SELECT 1
        FROM dbo.TipoProducto
        WHERE IdTipoProducto = @IdTipoProducto
    )
    BEGIN
        SET @Mensaje = 'El tipo de producto no existe.';
        RETURN;
    END;

    IF EXISTS (
        SELECT 1
        FROM dbo.TipoProducto
        WHERE UPPER(LTRIM(RTRIM(Nombre))) = UPPER(@Nombre)
          AND IdTipoProducto <> @IdTipoProducto
    )
    BEGIN
        SET @Mensaje = 'Ya existe otro tipo de producto con ese nombre.';
        RETURN;
    END;

    UPDATE dbo.TipoProducto
    SET
        Nombre = @Nombre,
        Descripcion = @Descripcion,
        Activo = @Activo
    WHERE IdTipoProducto = @IdTipoProducto;

    SET @Resultado = 1;
    SET @Mensaje = 'Tipo de producto actualizado correctamente.';
END;
GO

CREATE OR ALTER PROCEDURE Catalogos.sp_TipoProducto_Inactivar
    @IdTipoProducto INT,
    @Resultado BIT OUTPUT,
    @Mensaje VARCHAR(500) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    SET @Resultado = 0;
    SET @Mensaje = '';

    IF (@IdTipoProducto <= 0)
    BEGIN
        SET @Mensaje = 'Debe seleccionar un tipo de producto válido.';
        RETURN;
    END;

    IF NOT EXISTS (
        SELECT 1
        FROM dbo.TipoProducto
        WHERE IdTipoProducto = @IdTipoProducto
    )
    BEGIN
        SET @Mensaje = 'El tipo de producto no existe.';
        RETURN;
    END;

    IF EXISTS (
        SELECT 1
        FROM dbo.Producto
        WHERE IdTipoProducto = @IdTipoProducto
          AND Activo = 1
    )
    BEGIN
        SET @Mensaje = 'No se puede inactivar porque está asociado a productos activos.';
        RETURN;
    END;

    UPDATE dbo.TipoProducto
    SET Activo = 0
    WHERE IdTipoProducto = @IdTipoProducto;

    SET @Resultado = 1;
    SET @Mensaje = 'Tipo de producto inactivado correctamente.';
END;
GO