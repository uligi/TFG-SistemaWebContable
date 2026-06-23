IF NOT EXISTS (SELECT 1 FROM sys.schemas WHERE name = 'Catalogos')
BEGIN
    EXEC('CREATE SCHEMA Catalogos');
END;
GO

CREATE OR ALTER PROCEDURE Catalogos.sp_Descuento_Listar
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        IdDescuento,
        Nombre,
        Porcentaje,
        RequiereAutorizacion,
        Descripcion,
        Activo
    FROM dbo.Descuento
    ORDER BY Nombre;
END;
GO

CREATE OR ALTER PROCEDURE Catalogos.sp_Descuento_ListarActivos
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        IdDescuento,
        Nombre,
        Porcentaje,
        RequiereAutorizacion,
        Descripcion,
        Activo
    FROM dbo.Descuento
    WHERE Activo = 1
    ORDER BY Nombre;
END;
GO

CREATE OR ALTER PROCEDURE Catalogos.sp_Descuento_Registrar
    @Nombre VARCHAR(100),
    @Porcentaje DECIMAL(5,2),
    @RequiereAutorizacion BIT,
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
        SET @Mensaje = 'El nombre del descuento es obligatorio.';
        RETURN;
    END;

    IF (@Descripcion = '')
    BEGIN
        SET @Mensaje = 'La descripción es obligatoria.';
        RETURN;
    END;

    IF (@Porcentaje < 0 OR @Porcentaje > 100)
    BEGIN
        SET @Mensaje = 'El porcentaje del descuento debe estar entre 0 y 100.';
        RETURN;
    END;

    IF EXISTS (
        SELECT 1
        FROM dbo.Descuento
        WHERE UPPER(LTRIM(RTRIM(Nombre))) = UPPER(@Nombre)
    )
    BEGIN
        SET @Mensaje = 'Ya existe un descuento con ese nombre.';
        RETURN;
    END;

    INSERT INTO dbo.Descuento
    (
        Nombre,
        Porcentaje,
        RequiereAutorizacion,
        Descripcion,
        Activo
    )
    VALUES
    (
        @Nombre,
        @Porcentaje,
        @RequiereAutorizacion,
        @Descripcion,
        1
    );

    SET @Resultado = SCOPE_IDENTITY();
    SET @Mensaje = 'Descuento registrado correctamente.';
END;
GO

CREATE OR ALTER PROCEDURE Catalogos.sp_Descuento_Editar
    @IdDescuento INT,
    @Nombre VARCHAR(100),
    @Porcentaje DECIMAL(5,2),
    @RequiereAutorizacion BIT,
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

    IF (@IdDescuento <= 0)
    BEGIN
        SET @Mensaje = 'Debe seleccionar un descuento válido.';
        RETURN;
    END;

    IF (@Nombre = '')
    BEGIN
        SET @Mensaje = 'El nombre del descuento es obligatorio.';
        RETURN;
    END;

    IF (@Descripcion = '')
    BEGIN
        SET @Mensaje = 'La descripción es obligatoria.';
        RETURN;
    END;

    IF (@Porcentaje < 0 OR @Porcentaje > 100)
    BEGIN
        SET @Mensaje = 'El porcentaje del descuento debe estar entre 0 y 100.';
        RETURN;
    END;

    IF NOT EXISTS (
        SELECT 1
        FROM dbo.Descuento
        WHERE IdDescuento = @IdDescuento
    )
    BEGIN
        SET @Mensaje = 'El descuento no existe.';
        RETURN;
    END;

    IF EXISTS (
        SELECT 1
        FROM dbo.Descuento
        WHERE UPPER(LTRIM(RTRIM(Nombre))) = UPPER(@Nombre)
          AND IdDescuento <> @IdDescuento
    )
    BEGIN
        SET @Mensaje = 'Ya existe otro descuento con ese nombre.';
        RETURN;
    END;

    UPDATE dbo.Descuento
    SET
        Nombre = @Nombre,
        Porcentaje = @Porcentaje,
        RequiereAutorizacion = @RequiereAutorizacion,
        Descripcion = @Descripcion,
        Activo = @Activo
    WHERE IdDescuento = @IdDescuento;

    SET @Resultado = 1;
    SET @Mensaje = 'Descuento actualizado correctamente.';
END;
GO

CREATE OR ALTER PROCEDURE Catalogos.sp_Descuento_Inactivar
    @IdDescuento INT,
    @Resultado BIT OUTPUT,
    @Mensaje VARCHAR(500) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    SET @Resultado = 0;
    SET @Mensaje = '';

    IF (@IdDescuento <= 0)
    BEGIN
        SET @Mensaje = 'Debe seleccionar un descuento válido.';
        RETURN;
    END;

    IF NOT EXISTS (
        SELECT 1
        FROM dbo.Descuento
        WHERE IdDescuento = @IdDescuento
    )
    BEGIN
        SET @Mensaje = 'El descuento no existe.';
        RETURN;
    END;

    IF EXISTS (
        SELECT 1
        FROM dbo.DetalleFactura
        WHERE IdDescuento = @IdDescuento
    )
    BEGIN
        SET @Mensaje = 'No se puede inactivar el descuento porque está asociado a detalles de factura.';
        RETURN;
    END;

    UPDATE dbo.Descuento
    SET Activo = 0
    WHERE IdDescuento = @IdDescuento;

    SET @Resultado = 1;
    SET @Mensaje = 'Descuento inactivado correctamente.';
END;
GO