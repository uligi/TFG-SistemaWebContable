IF NOT EXISTS (SELECT 1 FROM sys.schemas WHERE name = 'Ubicacion')
BEGIN
    EXEC('CREATE SCHEMA Ubicacion');
END;
GO

CREATE OR ALTER PROCEDURE Ubicacion.sp_Distrito_Listar
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        d.CodigoDistrito,
        d.CodigoCanton,
        d.Nombre,
        d.Activo,
        c.Nombre AS CantonNombre,
        c.CodigoProvincia,
        p.Nombre AS ProvinciaNombre
    FROM dbo.Distrito d
    INNER JOIN dbo.Canton c
        ON c.CodigoCanton = d.CodigoCanton
    INNER JOIN dbo.Provincia p
        ON p.CodigoProvincia = c.CodigoProvincia
    ORDER BY 
        p.CodigoProvincia,
        c.CodigoCanton,
        d.CodigoDistrito;
END;
GO

CREATE OR ALTER PROCEDURE Ubicacion.sp_Distrito_Registrar
    @CodigoDistrito INT,
    @CodigoCanton INT,
    @Nombre VARCHAR(45),
    @Resultado INT OUTPUT,
    @Mensaje VARCHAR(500) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    SET @Resultado = 0;
    SET @Mensaje = '';

    SET @Nombre = LTRIM(RTRIM(@Nombre));

    IF (@CodigoDistrito <= 0)
    BEGIN
        SET @Mensaje = 'El código de distrito debe ser mayor a cero.';
        RETURN;
    END;

    IF (@CodigoCanton <= 0)
    BEGIN
        SET @Mensaje = 'Debe seleccionar un cantón.';
        RETURN;
    END;

    IF (@Nombre = '')
    BEGIN
        SET @Mensaje = 'El nombre del distrito es obligatorio.';
        RETURN;
    END;

    IF EXISTS (
        SELECT 1
        FROM dbo.Distrito
        WHERE CodigoDistrito = @CodigoDistrito
    )
    BEGIN
        SET @Mensaje = 'Ya existe un distrito con ese código.';
        RETURN;
    END;

    IF NOT EXISTS (
        SELECT 1
        FROM dbo.Canton
        WHERE CodigoCanton = @CodigoCanton
          AND Activo = 1
    )
    BEGIN
        SET @Mensaje = 'El cantón seleccionado no existe o está inactivo.';
        RETURN;
    END;

    IF EXISTS (
        SELECT 1
        FROM dbo.Distrito
        WHERE CodigoCanton = @CodigoCanton
          AND UPPER(LTRIM(RTRIM(Nombre))) = UPPER(@Nombre)
    )
    BEGIN
        SET @Mensaje = 'Ya existe un distrito con ese nombre para el cantón seleccionado.';
        RETURN;
    END;

    INSERT INTO dbo.Distrito
    (
        CodigoDistrito,
        CodigoCanton,
        Nombre,
        Activo
    )
    VALUES
    (
        @CodigoDistrito,
        @CodigoCanton,
        @Nombre,
        1
    );

    SET @Resultado = @CodigoDistrito;
    SET @Mensaje = 'Distrito registrado correctamente.';
END;
GO

CREATE OR ALTER PROCEDURE Ubicacion.sp_Distrito_Editar
    @CodigoDistrito INT,
    @CodigoCanton INT,
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

    IF (@CodigoDistrito <= 0)
    BEGIN
        SET @Mensaje = 'Debe seleccionar un distrito válido.';
        RETURN;
    END;

    IF (@CodigoCanton <= 0)
    BEGIN
        SET @Mensaje = 'Debe seleccionar un cantón.';
        RETURN;
    END;

    IF (@Nombre = '')
    BEGIN
        SET @Mensaje = 'El nombre del distrito es obligatorio.';
        RETURN;
    END;

    IF NOT EXISTS (
        SELECT 1
        FROM dbo.Distrito
        WHERE CodigoDistrito = @CodigoDistrito
    )
    BEGIN
        SET @Mensaje = 'El distrito no existe.';
        RETURN;
    END;

    IF NOT EXISTS (
        SELECT 1
        FROM dbo.Canton
        WHERE CodigoCanton = @CodigoCanton
          AND Activo = 1
    )
    BEGIN
        SET @Mensaje = 'El cantón seleccionado no existe o está inactivo.';
        RETURN;
    END;

    IF EXISTS (
        SELECT 1
        FROM dbo.Distrito
        WHERE CodigoCanton = @CodigoCanton
          AND UPPER(LTRIM(RTRIM(Nombre))) = UPPER(@Nombre)
          AND CodigoDistrito <> @CodigoDistrito
    )
    BEGIN
        SET @Mensaje = 'Ya existe otro distrito con ese nombre para el cantón seleccionado.';
        RETURN;
    END;

    UPDATE dbo.Distrito
    SET 
        CodigoCanton = @CodigoCanton,
        Nombre = @Nombre,
        Activo = @Activo
    WHERE CodigoDistrito = @CodigoDistrito;

    SET @Resultado = 1;
    SET @Mensaje = 'Distrito actualizado correctamente.';
END;
GO

CREATE OR ALTER PROCEDURE Ubicacion.sp_Distrito_Inactivar
    @CodigoDistrito INT,
    @Resultado BIT OUTPUT,
    @Mensaje VARCHAR(500) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    SET @Resultado = 0;
    SET @Mensaje = '';

    IF (@CodigoDistrito <= 0)
    BEGIN
        SET @Mensaje = 'Debe seleccionar un distrito válido.';
        RETURN;
    END;

    IF NOT EXISTS (
        SELECT 1
        FROM dbo.Distrito
        WHERE CodigoDistrito = @CodigoDistrito
    )
    BEGIN
        SET @Mensaje = 'El distrito no existe.';
        RETURN;
    END;

    UPDATE dbo.Distrito
    SET Activo = 0
    WHERE CodigoDistrito = @CodigoDistrito;

    SET @Resultado = 1;
    SET @Mensaje = 'Distrito inactivado correctamente.';
END;
GO

CREATE OR ALTER PROCEDURE Ubicacion.sp_Distrito_ListarActivosPorCanton
    @CodigoCanton INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        CodigoDistrito,
        CodigoCanton,
        Nombre,
        Activo
    FROM dbo.Distrito
    WHERE CodigoCanton = @CodigoCanton
      AND Activo = 1
    ORDER BY CodigoDistrito;
END;
GO