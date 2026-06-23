IF NOT EXISTS (SELECT 1 FROM sys.schemas WHERE name = 'Ubicacion')
BEGIN
    EXEC('CREATE SCHEMA Ubicacion');
END;
GO

CREATE OR ALTER PROCEDURE Ubicacion.sp_Provincia_Listar
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        CodigoProvincia,
        Nombre,
        Activo
    FROM dbo.Provincia
    ORDER BY CodigoProvincia;
END;
GO

CREATE OR ALTER PROCEDURE Ubicacion.sp_Provincia_Registrar
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

    IF (@CodigoProvincia <= 0)
    BEGIN
        SET @Mensaje = 'El código de provincia debe ser mayor a cero.';
        RETURN;
    END;

    IF (@Nombre = '')
    BEGIN
        SET @Mensaje = 'El nombre de la provincia es obligatorio.';
        RETURN;
    END;

    IF EXISTS (
        SELECT 1
        FROM dbo.Provincia
        WHERE CodigoProvincia = @CodigoProvincia
    )
    BEGIN
        SET @Mensaje = 'Ya existe una provincia con ese código.';
        RETURN;
    END;

    IF EXISTS (
        SELECT 1
        FROM dbo.Provincia
        WHERE UPPER(LTRIM(RTRIM(Nombre))) = UPPER(@Nombre)
    )
    BEGIN
        SET @Mensaje = 'Ya existe una provincia con ese nombre.';
        RETURN;
    END;

    INSERT INTO dbo.Provincia
    (
        CodigoProvincia,
        Nombre,
        Activo
    )
    VALUES
    (
        @CodigoProvincia,
        @Nombre,
        1
    );

    SET @Resultado = @CodigoProvincia;
    SET @Mensaje = 'Provincia registrada correctamente.';
END;
GO

CREATE OR ALTER PROCEDURE Ubicacion.sp_Provincia_Editar
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

    IF (@CodigoProvincia <= 0)
    BEGIN
        SET @Mensaje = 'Debe seleccionar una provincia válida.';
        RETURN;
    END;

    IF (@Nombre = '')
    BEGIN
        SET @Mensaje = 'El nombre de la provincia es obligatorio.';
        RETURN;
    END;

    IF NOT EXISTS (
        SELECT 1
        FROM dbo.Provincia
        WHERE CodigoProvincia = @CodigoProvincia
    )
    BEGIN
        SET @Mensaje = 'La provincia no existe.';
        RETURN;
    END;

    IF EXISTS (
        SELECT 1
        FROM dbo.Provincia
        WHERE UPPER(LTRIM(RTRIM(Nombre))) = UPPER(@Nombre)
          AND CodigoProvincia <> @CodigoProvincia
    )
    BEGIN
        SET @Mensaje = 'Ya existe otra provincia con ese nombre.';
        RETURN;
    END;

    UPDATE dbo.Provincia
    SET 
        Nombre = @Nombre,
        Activo = @Activo
    WHERE CodigoProvincia = @CodigoProvincia;

    SET @Resultado = 1;
    SET @Mensaje = 'Provincia actualizada correctamente.';
END;
GO

CREATE OR ALTER PROCEDURE Ubicacion.sp_Provincia_Inactivar
    @CodigoProvincia INT,
    @Resultado BIT OUTPUT,
    @Mensaje VARCHAR(500) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    SET @Resultado = 0;
    SET @Mensaje = '';

    IF (@CodigoProvincia <= 0)
    BEGIN
        SET @Mensaje = 'Debe seleccionar una provincia válida.';
        RETURN;
    END;

    IF NOT EXISTS (
        SELECT 1
        FROM dbo.Provincia
        WHERE CodigoProvincia = @CodigoProvincia
    )
    BEGIN
        SET @Mensaje = 'La provincia no existe.';
        RETURN;
    END;

    IF EXISTS (
        SELECT 1
        FROM dbo.Canton
        WHERE CodigoProvincia = @CodigoProvincia
          AND Activo = 1
    )
    BEGIN
        SET @Mensaje = 'No se puede inactivar la provincia porque tiene cantones activos asociados.';
        RETURN;
    END;

    UPDATE dbo.Provincia
    SET Activo = 0
    WHERE CodigoProvincia = @CodigoProvincia;

    SET @Resultado = 1;
    SET @Mensaje = 'Provincia inactivada correctamente.';
END;
GO