IF NOT EXISTS (SELECT 1 FROM sys.schemas WHERE name = 'Catalogos')
BEGIN
    EXEC('CREATE SCHEMA Catalogos');
END;
GO

CREATE OR ALTER PROCEDURE Catalogos.sp_TipoFactura_Listar
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        IdTipoFactura,
        TipoFactura,
        Descripcion,
        Activo
    FROM dbo.TipoFactura
    ORDER BY TipoFactura;
END;
GO

CREATE OR ALTER PROCEDURE Catalogos.sp_TipoFactura_Registrar
    @TipoFactura VARCHAR(45),
    @Descripcion VARCHAR(150),
    @Resultado INT OUTPUT,
    @Mensaje VARCHAR(500) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    SET @Resultado = 0;
    SET @Mensaje = '';

    SET @TipoFactura = LTRIM(RTRIM(@TipoFactura));
    SET @Descripcion = LTRIM(RTRIM(ISNULL(@Descripcion, '')));

    IF (@TipoFactura = '')
    BEGIN
        SET @Mensaje = 'El nombre del tipo de factura es obligatorio.';
        RETURN;
    END;

    IF EXISTS (
        SELECT 1
        FROM dbo.TipoFactura
        WHERE UPPER(LTRIM(RTRIM(TipoFactura))) = UPPER(@TipoFactura)
    )
    BEGIN
        SET @Mensaje = 'Ya existe un tipo de factura con ese nombre.';
        RETURN;
    END;

    INSERT INTO dbo.TipoFactura
    (
        TipoFactura,
        Descripcion,
        Activo
    )
    VALUES
    (
        @TipoFactura,
        @Descripcion,
        1
    );

    SET @Resultado = SCOPE_IDENTITY();
    SET @Mensaje = 'Tipo de factura registrado correctamente.';
END;
GO

CREATE OR ALTER PROCEDURE Catalogos.sp_TipoFactura_Editar
    @IdTipoFactura INT,
    @TipoFactura VARCHAR(45),
    @Descripcion VARCHAR(150),
    @Activo BIT,
    @Resultado BIT OUTPUT,
    @Mensaje VARCHAR(500) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    SET @Resultado = 0;
    SET @Mensaje = '';

    SET @TipoFactura = LTRIM(RTRIM(@TipoFactura));
    SET @Descripcion = LTRIM(RTRIM(ISNULL(@Descripcion, '')));

    IF (@IdTipoFactura <= 0)
    BEGIN
        SET @Mensaje = 'Debe seleccionar un tipo de factura válido.';
        RETURN;
    END;

    IF (@TipoFactura = '')
    BEGIN
        SET @Mensaje = 'El nombre del tipo de factura es obligatorio.';
        RETURN;
    END;

    IF NOT EXISTS (
        SELECT 1
        FROM dbo.TipoFactura
        WHERE IdTipoFactura = @IdTipoFactura
    )
    BEGIN
        SET @Mensaje = 'El tipo de factura no existe.';
        RETURN;
    END;

    IF EXISTS (
        SELECT 1
        FROM dbo.TipoFactura
        WHERE UPPER(LTRIM(RTRIM(TipoFactura))) = UPPER(@TipoFactura)
          AND IdTipoFactura <> @IdTipoFactura
    )
    BEGIN
        SET @Mensaje = 'Ya existe otro tipo de factura con ese nombre.';
        RETURN;
    END;

    UPDATE dbo.TipoFactura
    SET
        TipoFactura = @TipoFactura,
        Descripcion = @Descripcion,
        Activo = @Activo
    WHERE IdTipoFactura = @IdTipoFactura;

    SET @Resultado = 1;
    SET @Mensaje = 'Tipo de factura actualizado correctamente.';
END;
GO

CREATE OR ALTER PROCEDURE Catalogos.sp_TipoFactura_Inactivar
    @IdTipoFactura INT,
    @Resultado BIT OUTPUT,
    @Mensaje VARCHAR(500) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    SET @Resultado = 0;
    SET @Mensaje = '';

    IF (@IdTipoFactura <= 0)
    BEGIN
        SET @Mensaje = 'Debe seleccionar un tipo de factura válido.';
        RETURN;
    END;

    IF NOT EXISTS (
        SELECT 1
        FROM dbo.TipoFactura
        WHERE IdTipoFactura = @IdTipoFactura
    )
    BEGIN
        SET @Mensaje = 'El tipo de factura no existe.';
        RETURN;
    END;

    IF EXISTS (
        SELECT 1
        FROM dbo.Factura
        WHERE IdTipoFactura = @IdTipoFactura
          AND Activo = 1
    )
    BEGIN
        SET @Mensaje = 'No se puede inactivar el tipo de factura porque tiene facturas activas asociadas.';
        RETURN;
    END;

    UPDATE dbo.TipoFactura
    SET Activo = 0
    WHERE IdTipoFactura = @IdTipoFactura;

    SET @Resultado = 1;
    SET @Mensaje = 'Tipo de factura inactivado correctamente.';
END;
GO