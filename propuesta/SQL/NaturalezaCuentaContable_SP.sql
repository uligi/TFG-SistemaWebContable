IF NOT EXISTS (SELECT 1 FROM sys.schemas WHERE name = 'Catalogos')
BEGIN
    EXEC('CREATE SCHEMA Catalogos');
END;
GO

CREATE OR ALTER PROCEDURE Catalogos.sp_NaturalezaCuentaContable_Listar
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        IdNaturalezaCuentaContable,
        Naturaleza,
        Descripcion,
        Activo
    FROM dbo.NaturalezaCuentaContable
    ORDER BY Naturaleza;
END;
GO

CREATE OR ALTER PROCEDURE Catalogos.sp_NaturalezaCuentaContable_ListarActivos
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        IdNaturalezaCuentaContable,
        Naturaleza,
        Descripcion,
        Activo
    FROM dbo.NaturalezaCuentaContable
    WHERE Activo = 1
    ORDER BY Naturaleza;
END;
GO

CREATE OR ALTER PROCEDURE Catalogos.sp_NaturalezaCuentaContable_Registrar
    @Naturaleza VARCHAR(45),
    @Descripcion VARCHAR(255),
    @Resultado INT OUTPUT,
    @Mensaje VARCHAR(500) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    SET @Resultado = 0;
    SET @Mensaje = '';

    SET @Naturaleza = LTRIM(RTRIM(@Naturaleza));
    SET @Descripcion = LTRIM(RTRIM(ISNULL(@Descripcion, '')));

    IF (@Naturaleza = '')
    BEGIN
        SET @Mensaje = 'La naturaleza es obligatoria.';
        RETURN;
    END;

    IF (@Descripcion = '')
    BEGIN
        SET @Mensaje = 'La descripción es obligatoria.';
        RETURN;
    END;

    IF EXISTS (
        SELECT 1
        FROM dbo.NaturalezaCuentaContable
        WHERE UPPER(LTRIM(RTRIM(Naturaleza))) = UPPER(@Naturaleza)
    )
    BEGIN
        SET @Mensaje = 'Ya existe una naturaleza con ese nombre.';
        RETURN;
    END;

    INSERT INTO dbo.NaturalezaCuentaContable
    (
        Naturaleza,
        Descripcion,
        Activo
    )
    VALUES
    (
        @Naturaleza,
        @Descripcion,
        1
    );

    SET @Resultado = SCOPE_IDENTITY();
    SET @Mensaje = 'Naturaleza registrada correctamente.';
END;
GO

CREATE OR ALTER PROCEDURE Catalogos.sp_NaturalezaCuentaContable_Editar
    @IdNaturalezaCuentaContable INT,
    @Naturaleza VARCHAR(45),
    @Descripcion VARCHAR(255),
    @Activo BIT,
    @Resultado BIT OUTPUT,
    @Mensaje VARCHAR(500) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    SET @Resultado = 0;
    SET @Mensaje = '';

    SET @Naturaleza = LTRIM(RTRIM(@Naturaleza));
    SET @Descripcion = LTRIM(RTRIM(ISNULL(@Descripcion, '')));

    IF (@IdNaturalezaCuentaContable <= 0)
    BEGIN
        SET @Mensaje = 'Debe seleccionar una naturaleza válida.';
        RETURN;
    END;

    IF (@Naturaleza = '')
    BEGIN
        SET @Mensaje = 'La naturaleza es obligatoria.';
        RETURN;
    END;

    IF (@Descripcion = '')
    BEGIN
        SET @Mensaje = 'La descripción es obligatoria.';
        RETURN;
    END;

    IF NOT EXISTS (
        SELECT 1
        FROM dbo.NaturalezaCuentaContable
        WHERE IdNaturalezaCuentaContable = @IdNaturalezaCuentaContable
    )
    BEGIN
        SET @Mensaje = 'La naturaleza no existe.';
        RETURN;
    END;

    IF EXISTS (
        SELECT 1
        FROM dbo.NaturalezaCuentaContable
        WHERE UPPER(LTRIM(RTRIM(Naturaleza))) = UPPER(@Naturaleza)
          AND IdNaturalezaCuentaContable <> @IdNaturalezaCuentaContable
    )
    BEGIN
        SET @Mensaje = 'Ya existe otra naturaleza con ese nombre.';
        RETURN;
    END;

    UPDATE dbo.NaturalezaCuentaContable
    SET
        Naturaleza = @Naturaleza,
        Descripcion = @Descripcion,
        Activo = @Activo
    WHERE IdNaturalezaCuentaContable = @IdNaturalezaCuentaContable;

    SET @Resultado = 1;
    SET @Mensaje = 'Naturaleza actualizada correctamente.';
END;
GO

CREATE OR ALTER PROCEDURE Catalogos.sp_NaturalezaCuentaContable_Inactivar
    @IdNaturalezaCuentaContable INT,
    @Resultado BIT OUTPUT,
    @Mensaje VARCHAR(500) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    SET @Resultado = 0;
    SET @Mensaje = '';

    IF (@IdNaturalezaCuentaContable <= 0)
    BEGIN
        SET @Mensaje = 'Debe seleccionar una naturaleza válida.';
        RETURN;
    END;

    IF NOT EXISTS (
        SELECT 1
        FROM dbo.NaturalezaCuentaContable
        WHERE IdNaturalezaCuentaContable = @IdNaturalezaCuentaContable
    )
    BEGIN
        SET @Mensaje = 'La naturaleza no existe.';
        RETURN;
    END;

    IF EXISTS (
        SELECT 1
        FROM dbo.CuentaContable
        WHERE IdNaturalezaCuentaContable = @IdNaturalezaCuentaContable
          AND Activo = 1
    )
    BEGIN
        SET @Mensaje = 'No se puede inactivar porque está asociada a cuentas contables activas.';
        RETURN;
    END;

    UPDATE dbo.NaturalezaCuentaContable
    SET Activo = 0
    WHERE IdNaturalezaCuentaContable = @IdNaturalezaCuentaContable;

    SET @Resultado = 1;
    SET @Mensaje = 'Naturaleza inactivada correctamente.';
END;
GO