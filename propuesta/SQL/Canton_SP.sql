IF NOT EXISTS (SELECT 1 FROM sys.schemas WHERE name = 'Ubicacion')
BEGIN
    EXEC('CREATE SCHEMA Ubicacion');
END;
GO

CREATE OR ALTER PROCEDURE Ubicacion.sp_Canton_Listar
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        c.CodigoCanton,
        c.CodigoProvincia,
        c.Nombre,
        c.Activo,
        p.Nombre AS ProvinciaNombre
    FROM dbo.Canton c
    INNER JOIN dbo.Provincia p 
        ON p.CodigoProvincia = c.CodigoProvincia
    ORDER BY 
        p.CodigoProvincia,
        c.CodigoCanton;
END;
GO

CREATE OR ALTER PROCEDURE Ubicacion.sp_Canton_Registrar
    @CodigoCanton INT,
    @CodigoProvincia INT,
    @Nombre VARCHAR(45),
    @Resultado INT OUTPUT,
    @Mensaje VARCHAR(500) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    SET @Resultado = 0;
    SET @Mensaje = '';

    SET @Nombre = LTRIM(RTRIM(@Nombre));

    IF (@CodigoCanton <= 0)
    BEGIN
        SET @Mensaje = 'El código de cantón debe ser mayor a cero.';
        RETURN;
    END;

    IF (@CodigoProvincia <= 0)
    BEGIN
        SET @Mensaje = 'Debe seleccionar una provincia.';
        RETURN;
    END;

    IF (@Nombre = '')
    BEGIN
        SET @Mensaje = 'El nombre del cantón es obligatorio.';
        RETURN;
    END;

    IF EXISTS (
        SELECT 1
        FROM dbo.Canton
        WHERE CodigoCanton = @CodigoCanton
    )
    BEGIN
        SET @Mensaje = 'Ya existe un cantón con ese código.';
        RETURN;
    END;

    IF NOT EXISTS (
        SELECT 1
        FROM dbo.Provincia
        WHERE CodigoProvincia = @CodigoProvincia
          AND Activo = 1
    )
    BEGIN
        SET @Mensaje = 'La provincia seleccionada no existe o está inactiva.';
        RETURN;
    END;

    IF EXISTS (
        SELECT 1
        FROM dbo.Canton
        WHERE CodigoProvincia = @CodigoProvincia
          AND UPPER(LTRIM(RTRIM(Nombre))) = UPPER(@Nombre)
    )
    BEGIN
        SET @Mensaje = 'Ya existe un cantón con ese nombre para la provincia seleccionada.';
        RETURN;
    END;

    INSERT INTO dbo.Canton
    (
        CodigoCanton,
        CodigoProvincia,
        Nombre,
        Activo
    )
    VALUES
    (
        @CodigoCanton,
        @CodigoProvincia,
        @Nombre,
        1
    );

    SET @Resultado = @CodigoCanton;
    SET @Mensaje = 'Cantón registrado correctamente.';
END;
GO

CREATE OR ALTER PROCEDURE Ubicacion.sp_Canton_Editar
    @CodigoCanton INT,
    @CodigoProvincia INT,
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

    IF (@CodigoCanton <= 0)
    BEGIN
        SET @Mensaje = 'Debe seleccionar un cantón válido.';
        RETURN;
    END;

    IF (@CodigoProvincia <= 0)
    BEGIN
        SET @Mensaje = 'Debe seleccionar una provincia.';
        RETURN;
    END;

    IF (@Nombre = '')
    BEGIN
        SET @Mensaje = 'El nombre del cantón es obligatorio.';
        RETURN;
    END;

    IF NOT EXISTS (
        SELECT 1
        FROM dbo.Canton
        WHERE CodigoCanton = @CodigoCanton
    )
    BEGIN
        SET @Mensaje = 'El cantón no existe.';
        RETURN;
    END;

    IF NOT EXISTS (
        SELECT 1
        FROM dbo.Provincia
        WHERE CodigoProvincia = @CodigoProvincia
          AND Activo = 1
    )
    BEGIN
        SET @Mensaje = 'La provincia seleccionada no existe o está inactiva.';
        RETURN;
    END;

    IF EXISTS (
        SELECT 1
        FROM dbo.Canton
        WHERE CodigoProvincia = @CodigoProvincia
          AND UPPER(LTRIM(RTRIM(Nombre))) = UPPER(@Nombre)
          AND CodigoCanton <> @CodigoCanton
    )
    BEGIN
        SET @Mensaje = 'Ya existe otro cantón con ese nombre para la provincia seleccionada.';
        RETURN;
    END;

    UPDATE dbo.Canton
    SET 
        CodigoProvincia = @CodigoProvincia,
        Nombre = @Nombre,
        Activo = @Activo
    WHERE CodigoCanton = @CodigoCanton;

    SET @Resultado = 1;
    SET @Mensaje = 'Cantón actualizado correctamente.';
END;
GO

CREATE OR ALTER PROCEDURE Ubicacion.sp_Canton_Inactivar
    @CodigoCanton INT,
    @Resultado BIT OUTPUT,
    @Mensaje VARCHAR(500) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    SET @Resultado = 0;
    SET @Mensaje = '';

    IF (@CodigoCanton <= 0)
    BEGIN
        SET @Mensaje = 'Debe seleccionar un cantón válido.';
        RETURN;
    END;

    IF NOT EXISTS (
        SELECT 1
        FROM dbo.Canton
        WHERE CodigoCanton = @CodigoCanton
    )
    BEGIN
        SET @Mensaje = 'El cantón no existe.';
        RETURN;
    END;

    IF EXISTS (
        SELECT 1
        FROM dbo.Distrito
        WHERE CodigoCanton = @CodigoCanton
          AND Activo = 1
    )
    BEGIN
        SET @Mensaje = 'No se puede inactivar el cantón porque tiene distritos activos asociados.';
        RETURN;
    END;

    UPDATE dbo.Canton
    SET Activo = 0
    WHERE CodigoCanton = @CodigoCanton;

    SET @Resultado = 1;
    SET @Mensaje = 'Cantón inactivado correctamente.';
END;
GO

CREATE OR ALTER PROCEDURE Ubicacion.sp_Canton_ListarActivosPorProvincia
    @CodigoProvincia INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        CodigoCanton,
        CodigoProvincia,
        Nombre,
        Activo
    FROM dbo.Canton
    WHERE CodigoProvincia = @CodigoProvincia
      AND Activo = 1
    ORDER BY CodigoCanton;
END;
GO