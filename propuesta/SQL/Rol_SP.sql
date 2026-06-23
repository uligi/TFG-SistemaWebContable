IF NOT EXISTS (SELECT 1 FROM sys.schemas WHERE name = 'Seguridad')
BEGIN
    EXEC('CREATE SCHEMA Seguridad');
END;
GO

CREATE OR ALTER PROCEDURE Seguridad.sp_Rol_Listar
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        IdRol,
        Nombre,
        Descripcion,
        Activo
    FROM dbo.Rol
    ORDER BY Nombre;
END;
GO

CREATE OR ALTER PROCEDURE Seguridad.sp_Rol_Registrar
    @Nombre VARCHAR(45),
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
        SET @Mensaje = 'El nombre del rol es obligatorio.';
        RETURN;
    END;

    IF EXISTS (
        SELECT 1
        FROM dbo.Rol
        WHERE UPPER(LTRIM(RTRIM(Nombre))) = UPPER(@Nombre)
    )
    BEGIN
        SET @Mensaje = 'Ya existe un rol con ese nombre.';
        RETURN;
    END;

    INSERT INTO dbo.Rol
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
    SET @Mensaje = 'Rol registrado correctamente.';
END;
GO

CREATE OR ALTER PROCEDURE Seguridad.sp_Rol_Editar
    @IdRol INT,
    @Nombre VARCHAR(45),
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

    IF (@IdRol <= 0)
    BEGIN
        SET @Mensaje = 'Debe seleccionar un rol válido.';
        RETURN;
    END;

    IF (@Nombre = '')
    BEGIN
        SET @Mensaje = 'El nombre del rol es obligatorio.';
        RETURN;
    END;

    IF NOT EXISTS (
        SELECT 1
        FROM dbo.Rol
        WHERE IdRol = @IdRol
    )
    BEGIN
        SET @Mensaje = 'El rol no existe.';
        RETURN;
    END;

    IF EXISTS (
        SELECT 1
        FROM dbo.Rol
        WHERE UPPER(LTRIM(RTRIM(Nombre))) = UPPER(@Nombre)
          AND IdRol <> @IdRol
    )
    BEGIN
        SET @Mensaje = 'Ya existe otro rol con ese nombre.';
        RETURN;
    END;

    UPDATE dbo.Rol
    SET
        Nombre = @Nombre,
        Descripcion = @Descripcion,
        Activo = @Activo
    WHERE IdRol = @IdRol;

    SET @Resultado = 1;
    SET @Mensaje = 'Rol actualizado correctamente.';
END;
GO

CREATE OR ALTER PROCEDURE Seguridad.sp_Rol_Inactivar
    @IdRol INT,
    @Resultado BIT OUTPUT,
    @Mensaje VARCHAR(500) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    SET @Resultado = 0;
    SET @Mensaje = '';

    IF (@IdRol <= 0)
    BEGIN
        SET @Mensaje = 'Debe seleccionar un rol válido.';
        RETURN;
    END;

    IF NOT EXISTS (
        SELECT 1
        FROM dbo.Rol
        WHERE IdRol = @IdRol
    )
    BEGIN
        SET @Mensaje = 'El rol no existe.';
        RETURN;
    END;

    IF EXISTS (
        SELECT 1
        FROM dbo.Empleado
        WHERE IdRol = @IdRol
          AND Activo = 1
    )
    BEGIN
        SET @Mensaje = 'No se puede inactivar el rol porque tiene empleados activos asociados.';
        RETURN;
    END;

    UPDATE dbo.Rol
    SET Activo = 0
    WHERE IdRol = @IdRol;

    SET @Resultado = 1;
    SET @Mensaje = 'Rol inactivado correctamente.';
END;
GO