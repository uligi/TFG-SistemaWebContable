IF NOT EXISTS (SELECT 1 FROM sys.schemas WHERE name = 'Catalogos')
BEGIN
    EXEC('CREATE SCHEMA Catalogos');
END;
GO

CREATE OR ALTER PROCEDURE Catalogos.sp_TipoPago_Listar
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        IdTipoPago,
        Nombre,
        Activo
    FROM dbo.TipoPago
    ORDER BY Nombre;
END;
GO

CREATE OR ALTER PROCEDURE Catalogos.sp_TipoPago_Registrar
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
        SET @Mensaje = 'El nombre del tipo de pago es obligatorio.';
        RETURN;
    END;

    IF EXISTS (
        SELECT 1
        FROM dbo.TipoPago
        WHERE UPPER(LTRIM(RTRIM(Nombre))) = UPPER(@Nombre)
    )
    BEGIN
        SET @Mensaje = 'Ya existe un tipo de pago con ese nombre.';
        RETURN;
    END;

    INSERT INTO dbo.TipoPago
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
    SET @Mensaje = 'Tipo de pago registrado correctamente.';
END;
GO

CREATE OR ALTER PROCEDURE Catalogos.sp_TipoPago_Editar
    @IdTipoPago INT,
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

    IF (@IdTipoPago <= 0)
    BEGIN
        SET @Mensaje = 'Debe seleccionar un tipo de pago válido.';
        RETURN;
    END;

    IF (@Nombre = '')
    BEGIN
        SET @Mensaje = 'El nombre del tipo de pago es obligatorio.';
        RETURN;
    END;

    IF NOT EXISTS (
        SELECT 1
        FROM dbo.TipoPago
        WHERE IdTipoPago = @IdTipoPago
    )
    BEGIN
        SET @Mensaje = 'El tipo de pago no existe.';
        RETURN;
    END;

    IF EXISTS (
        SELECT 1
        FROM dbo.TipoPago
        WHERE UPPER(LTRIM(RTRIM(Nombre))) = UPPER(@Nombre)
          AND IdTipoPago <> @IdTipoPago
    )
    BEGIN
        SET @Mensaje = 'Ya existe otro tipo de pago con ese nombre.';
        RETURN;
    END;

    UPDATE dbo.TipoPago
    SET
        Nombre = @Nombre,
        Activo = @Activo
    WHERE IdTipoPago = @IdTipoPago;

    SET @Resultado = 1;
    SET @Mensaje = 'Tipo de pago actualizado correctamente.';
END;
GO

CREATE OR ALTER PROCEDURE Catalogos.sp_TipoPago_Inactivar
    @IdTipoPago INT,
    @Resultado BIT OUTPUT,
    @Mensaje VARCHAR(500) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    SET @Resultado = 0;
    SET @Mensaje = '';

    IF (@IdTipoPago <= 0)
    BEGIN
        SET @Mensaje = 'Debe seleccionar un tipo de pago válido.';
        RETURN;
    END;

    IF NOT EXISTS (
        SELECT 1
        FROM dbo.TipoPago
        WHERE IdTipoPago = @IdTipoPago
    )
    BEGIN
        SET @Mensaje = 'El tipo de pago no existe.';
        RETURN;
    END;

    IF EXISTS (
        SELECT 1
        FROM dbo.Factura
        WHERE IdTipoPago = @IdTipoPago
          AND Activo = 1
    )
    BEGIN
        SET @Mensaje = 'No se puede inactivar el tipo de pago porque tiene facturas activas asociadas.';
        RETURN;
    END;

    UPDATE dbo.TipoPago
    SET Activo = 0
    WHERE IdTipoPago = @IdTipoPago;

    SET @Resultado = 1;
    SET @Mensaje = 'Tipo de pago inactivado correctamente.';
END;
GO