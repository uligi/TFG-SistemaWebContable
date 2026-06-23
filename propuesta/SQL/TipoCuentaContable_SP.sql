IF NOT EXISTS (SELECT 1 FROM sys.schemas WHERE name = 'Catalogos')
BEGIN
    EXEC('CREATE SCHEMA Catalogos');
END;
GO

CREATE OR ALTER PROCEDURE Catalogos.sp_TipoCuentaContable_Listar
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        IdTipoCuentaContable,
        Nombre,
        Descripcion,
        Activo
    FROM dbo.TipoCuentaContable
    ORDER BY Nombre;
END;
GO

CREATE OR ALTER PROCEDURE Catalogos.sp_TipoCuentaContable_ListarActivos
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        IdTipoCuentaContable,
        Nombre,
        Descripcion,
        Activo
    FROM dbo.TipoCuentaContable
    WHERE Activo = 1
    ORDER BY Nombre;
END;
GO

CREATE OR ALTER PROCEDURE Catalogos.sp_TipoCuentaContable_Registrar
    @Nombre VARCHAR(45),
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
        SET @Mensaje = 'El nombre del tipo de cuenta contable es obligatorio.';
        RETURN;
    END;

    IF (@Descripcion = '')
    BEGIN
        SET @Mensaje = 'La descripción es obligatoria.';
        RETURN;
    END;

    IF EXISTS (
        SELECT 1
        FROM dbo.TipoCuentaContable
        WHERE UPPER(LTRIM(RTRIM(Nombre))) = UPPER(@Nombre)
    )
    BEGIN
        SET @Mensaje = 'Ya existe un tipo de cuenta contable con ese nombre.';
        RETURN;
    END;

    INSERT INTO dbo.TipoCuentaContable
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
    SET @Mensaje = 'Tipo de cuenta contable registrado correctamente.';
END;
GO

CREATE OR ALTER PROCEDURE Catalogos.sp_TipoCuentaContable_Editar
    @IdTipoCuentaContable INT,
    @Nombre VARCHAR(45),
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

    IF (@IdTipoCuentaContable <= 0)
    BEGIN
        SET @Mensaje = 'Debe seleccionar un tipo de cuenta contable válido.';
        RETURN;
    END;

    IF (@Nombre = '')
    BEGIN
        SET @Mensaje = 'El nombre del tipo de cuenta contable es obligatorio.';
        RETURN;
    END;

    IF (@Descripcion = '')
    BEGIN
        SET @Mensaje = 'La descripción es obligatoria.';
        RETURN;
    END;

    IF NOT EXISTS (
        SELECT 1
        FROM dbo.TipoCuentaContable
        WHERE IdTipoCuentaContable = @IdTipoCuentaContable
    )
    BEGIN
        SET @Mensaje = 'El tipo de cuenta contable no existe.';
        RETURN;
    END;

    IF EXISTS (
        SELECT 1
        FROM dbo.TipoCuentaContable
        WHERE UPPER(LTRIM(RTRIM(Nombre))) = UPPER(@Nombre)
          AND IdTipoCuentaContable <> @IdTipoCuentaContable
    )
    BEGIN
        SET @Mensaje = 'Ya existe otro tipo de cuenta contable con ese nombre.';
        RETURN;
    END;

    UPDATE dbo.TipoCuentaContable
    SET
        Nombre = @Nombre,
        Descripcion = @Descripcion,
        Activo = @Activo
    WHERE IdTipoCuentaContable = @IdTipoCuentaContable;

    SET @Resultado = 1;
    SET @Mensaje = 'Tipo de cuenta contable actualizado correctamente.';
END;
GO

CREATE OR ALTER PROCEDURE Catalogos.sp_TipoCuentaContable_Inactivar
    @IdTipoCuentaContable INT,
    @Resultado BIT OUTPUT,
    @Mensaje VARCHAR(500) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    SET @Resultado = 0;
    SET @Mensaje = '';

    IF (@IdTipoCuentaContable <= 0)
    BEGIN
        SET @Mensaje = 'Debe seleccionar un tipo de cuenta contable válido.';
        RETURN;
    END;

    IF NOT EXISTS (
        SELECT 1
        FROM dbo.TipoCuentaContable
        WHERE IdTipoCuentaContable = @IdTipoCuentaContable
    )
    BEGIN
        SET @Mensaje = 'El tipo de cuenta contable no existe.';
        RETURN;
    END;

    IF EXISTS (
        SELECT 1
        FROM dbo.CuentaContable
        WHERE IdTipoCuentaContable = @IdTipoCuentaContable
          AND Activo = 1
    )
    BEGIN
        SET @Mensaje = 'No se puede inactivar porque está asociado a cuentas contables activas.';
        RETURN;
    END;

    UPDATE dbo.TipoCuentaContable
    SET Activo = 0
    WHERE IdTipoCuentaContable = @IdTipoCuentaContable;

    SET @Resultado = 1;
    SET @Mensaje = 'Tipo de cuenta contable inactivado correctamente.';
END;
GO