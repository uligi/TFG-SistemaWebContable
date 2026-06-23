IF NOT EXISTS (SELECT 1 FROM sys.schemas WHERE name = 'Catalogos')
BEGIN
    EXEC('CREATE SCHEMA Catalogos');
END;
GO

CREATE OR ALTER PROCEDURE Catalogos.sp_Impuesto_Listar
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        IdImpuesto,
        Nombre,
        Porcentaje,
        Descripcion,
        Activo
    FROM dbo.Impuesto
    ORDER BY Nombre;
END;
GO

CREATE OR ALTER PROCEDURE Catalogos.sp_Impuesto_ListarActivos
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        IdImpuesto,
        Nombre,
        Porcentaje,
        Descripcion,
        Activo
    FROM dbo.Impuesto
    WHERE Activo = 1
    ORDER BY Nombre;
END;
GO

CREATE OR ALTER PROCEDURE Catalogos.sp_Impuesto_Registrar
    @Nombre VARCHAR(100),
    @Porcentaje DECIMAL(5,2),
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
        SET @Mensaje = 'El nombre del impuesto es obligatorio.';
        RETURN;
    END;

    IF (@Descripcion = '')
    BEGIN
        SET @Mensaje = 'La descripción es obligatoria.';
        RETURN;
    END;

    IF (@Porcentaje < 0 OR @Porcentaje > 100)
    BEGIN
        SET @Mensaje = 'El porcentaje del impuesto debe estar entre 0 y 100.';
        RETURN;
    END;

    IF EXISTS (
        SELECT 1
        FROM dbo.Impuesto
        WHERE UPPER(LTRIM(RTRIM(Nombre))) = UPPER(@Nombre)
    )
    BEGIN
        SET @Mensaje = 'Ya existe un impuesto con ese nombre.';
        RETURN;
    END;

    INSERT INTO dbo.Impuesto
    (
        Nombre,
        Porcentaje,
        Descripcion,
        Activo
    )
    VALUES
    (
        @Nombre,
        @Porcentaje,
        @Descripcion,
        1
    );

    SET @Resultado = SCOPE_IDENTITY();
    SET @Mensaje = 'Impuesto registrado correctamente.';
END;
GO

CREATE OR ALTER PROCEDURE Catalogos.sp_Impuesto_Editar
    @IdImpuesto INT,
    @Nombre VARCHAR(100),
    @Porcentaje DECIMAL(5,2),
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

    IF (@IdImpuesto <= 0)
    BEGIN
        SET @Mensaje = 'Debe seleccionar un impuesto válido.';
        RETURN;
    END;

    IF (@Nombre = '')
    BEGIN
        SET @Mensaje = 'El nombre del impuesto es obligatorio.';
        RETURN;
    END;

    IF (@Descripcion = '')
    BEGIN
        SET @Mensaje = 'La descripción es obligatoria.';
        RETURN;
    END;

    IF (@Porcentaje < 0 OR @Porcentaje > 100)
    BEGIN
        SET @Mensaje = 'El porcentaje del impuesto debe estar entre 0 y 100.';
        RETURN;
    END;

    IF NOT EXISTS (
        SELECT 1
        FROM dbo.Impuesto
        WHERE IdImpuesto = @IdImpuesto
    )
    BEGIN
        SET @Mensaje = 'El impuesto no existe.';
        RETURN;
    END;

    IF EXISTS (
        SELECT 1
        FROM dbo.Impuesto
        WHERE UPPER(LTRIM(RTRIM(Nombre))) = UPPER(@Nombre)
          AND IdImpuesto <> @IdImpuesto
    )
    BEGIN
        SET @Mensaje = 'Ya existe otro impuesto con ese nombre.';
        RETURN;
    END;

    UPDATE dbo.Impuesto
    SET
        Nombre = @Nombre,
        Porcentaje = @Porcentaje,
        Descripcion = @Descripcion,
        Activo = @Activo
    WHERE IdImpuesto = @IdImpuesto;

    SET @Resultado = 1;
    SET @Mensaje = 'Impuesto actualizado correctamente.';
END;
GO

CREATE OR ALTER PROCEDURE Catalogos.sp_Impuesto_Inactivar
    @IdImpuesto INT,
    @Resultado BIT OUTPUT,
    @Mensaje VARCHAR(500) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    SET @Resultado = 0;
    SET @Mensaje = '';

    IF (@IdImpuesto <= 0)
    BEGIN
        SET @Mensaje = 'Debe seleccionar un impuesto válido.';
        RETURN;
    END;

    IF NOT EXISTS (
        SELECT 1
        FROM dbo.Impuesto
        WHERE IdImpuesto = @IdImpuesto
    )
    BEGIN
        SET @Mensaje = 'El impuesto no existe.';
        RETURN;
    END;

    IF EXISTS (
        SELECT 1
        FROM dbo.Producto
        WHERE IdImpuesto = @IdImpuesto
          AND Activo = 1
    )
    BEGIN
        SET @Mensaje = 'No se puede inactivar el impuesto porque está asociado a productos activos.';
        RETURN;
    END;

    UPDATE dbo.Impuesto
    SET Activo = 0
    WHERE IdImpuesto = @IdImpuesto;

    SET @Resultado = 1;
    SET @Mensaje = 'Impuesto inactivado correctamente.';
END;
GO