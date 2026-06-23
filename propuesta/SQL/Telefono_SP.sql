IF NOT EXISTS (SELECT 1 FROM sys.schemas WHERE name = 'Persona')
BEGIN
    EXEC('CREATE SCHEMA Persona');
END;
GO

CREATE OR ALTER PROCEDURE Persona.sp_Telefono_Listar
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        t.Identificacion,
        t.NumeroTelefono,
        t.IdTipoTelefono,
        tt.Nombre AS TipoTelefonoNombre,
        t.EsPrincipal,
        t.Activo,

        p.Nombre,
        p.PrimerApellido,
        p.SegundoApellido
    FROM dbo.Telefono t
    INNER JOIN dbo.Persona p
        ON p.Identificacion = t.Identificacion
    INNER JOIN dbo.TipoTelefono tt
        ON tt.IdTipoTelefono = t.IdTipoTelefono
    ORDER BY
        p.Nombre,
        p.PrimerApellido,
        t.EsPrincipal DESC,
        t.NumeroTelefono;
END;
GO

CREATE OR ALTER PROCEDURE Persona.sp_Telefono_ListarPorPersona
    @Identificacion VARCHAR(45)
AS
BEGIN
    SET NOCOUNT ON;

    SET @Identificacion = LTRIM(RTRIM(ISNULL(@Identificacion, '')));

    SELECT
        t.Identificacion,
        t.NumeroTelefono,
        t.IdTipoTelefono,
        tt.Nombre AS TipoTelefonoNombre,
        t.EsPrincipal,
        t.Activo,

        p.Nombre,
        p.PrimerApellido,
        p.SegundoApellido
    FROM dbo.Telefono t
    INNER JOIN dbo.Persona p
        ON p.Identificacion = t.Identificacion
    INNER JOIN dbo.TipoTelefono tt
        ON tt.IdTipoTelefono = t.IdTipoTelefono
    WHERE t.Identificacion = @Identificacion
    ORDER BY
        t.EsPrincipal DESC,
        t.NumeroTelefono;
END;
GO

CREATE OR ALTER PROCEDURE Persona.sp_Telefono_Registrar
    @Identificacion VARCHAR(45),
    @NumeroTelefono VARCHAR(45),
    @IdTipoTelefono INT,
    @EsPrincipal BIT,
    @Resultado INT OUTPUT,
    @Mensaje VARCHAR(500) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    SET @Resultado = 0;
    SET @Mensaje = '';

    SET @Identificacion = LTRIM(RTRIM(ISNULL(@Identificacion, '')));
    SET @NumeroTelefono = LTRIM(RTRIM(ISNULL(@NumeroTelefono, '')));

    IF (@Identificacion = '')
    BEGIN
        SET @Mensaje = 'Debe seleccionar una persona.';
        RETURN;
    END;

    IF (@NumeroTelefono = '')
    BEGIN
        SET @Mensaje = 'El número de teléfono es obligatorio.';
        RETURN;
    END;

IF (@NumeroTelefono LIKE '%[^0-9]%' OR LEN(@NumeroTelefono) <> 8)
BEGIN
    SET @Mensaje = 'El número de teléfono debe contener exactamente 8 dígitos numéricos, sin prefijo ni caracteres especiales.';
    RETURN;
END;

    IF (@IdTipoTelefono <= 0)
    BEGIN
        SET @Mensaje = 'Debe seleccionar un tipo de teléfono.';
        RETURN;
    END;

    IF NOT EXISTS (
        SELECT 1
        FROM dbo.Persona
        WHERE Identificacion = @Identificacion
          AND Activo = 1
    )
    BEGIN
        SET @Mensaje = 'La persona seleccionada no existe o está inactiva.';
        RETURN;
    END;

    IF NOT EXISTS (
        SELECT 1
        FROM dbo.TipoTelefono
        WHERE IdTipoTelefono = @IdTipoTelefono
          AND Activo = 1
    )
    BEGIN
        SET @Mensaje = 'El tipo de teléfono seleccionado no existe o está inactivo.';
        RETURN;
    END;

    IF EXISTS (
        SELECT 1
        FROM dbo.Telefono
        WHERE Identificacion = @Identificacion
          AND NumeroTelefono = @NumeroTelefono
    )
    BEGIN
        SET @Mensaje = 'Ya existe ese número de teléfono para la persona seleccionada.';
        RETURN;
    END;

    BEGIN TRY
        BEGIN TRANSACTION;

        IF (@EsPrincipal = 1)
        BEGIN
            UPDATE dbo.Telefono
            SET EsPrincipal = 0
            WHERE Identificacion = @Identificacion;
        END;

        INSERT INTO dbo.Telefono
        (
            Identificacion,
            NumeroTelefono,
            IdTipoTelefono,
            EsPrincipal,
            Activo
        )
        VALUES
        (
            @Identificacion,
            @NumeroTelefono,
            @IdTipoTelefono,
            @EsPrincipal,
            1
        );

        COMMIT TRANSACTION;

        SET @Resultado = 1;
        SET @Mensaje = 'Teléfono registrado correctamente.';
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;

        SET @Resultado = 0;
        SET @Mensaje = ERROR_MESSAGE();
    END CATCH;
END;
GO

CREATE OR ALTER PROCEDURE Persona.sp_Telefono_Editar
    @Identificacion VARCHAR(45),
    @NumeroTelefonoAnterior VARCHAR(45),
    @NumeroTelefonoNuevo VARCHAR(45),
    @IdTipoTelefono INT,
    @EsPrincipal BIT,
    @Activo BIT,
    @Resultado BIT OUTPUT,
    @Mensaje VARCHAR(500) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    SET @Resultado = 0;
    SET @Mensaje = '';

    SET @Identificacion = LTRIM(RTRIM(ISNULL(@Identificacion, '')));
    SET @NumeroTelefonoAnterior = LTRIM(RTRIM(ISNULL(@NumeroTelefonoAnterior, '')));
    SET @NumeroTelefonoNuevo = LTRIM(RTRIM(ISNULL(@NumeroTelefonoNuevo, '')));

    IF (@Identificacion = '')
    BEGIN
        SET @Mensaje = 'Debe seleccionar una persona válida.';
        RETURN;
    END;

    IF (@NumeroTelefonoAnterior = '')
    BEGIN
        SET @Mensaje = 'Debe seleccionar un teléfono válido.';
        RETURN;
    END;

    IF (@NumeroTelefonoNuevo = '')
    BEGIN
        SET @Mensaje = 'El número de teléfono es obligatorio.';
        RETURN;
    END;

    IF (@NumeroTelefonoNuevo LIKE '%[^0-9]%' OR LEN(@NumeroTelefonoNuevo) < 8 OR LEN(@NumeroTelefonoNuevo) > 15)
    BEGIN
        SET @Mensaje = 'El número de teléfono debe contener solo números y tener entre 8 y 15 dígitos.';
        RETURN;
    END;

    IF (@IdTipoTelefono <= 0)
    BEGIN
        SET @Mensaje = 'Debe seleccionar un tipo de teléfono.';
        RETURN;
    END;

    IF NOT EXISTS (
        SELECT 1
        FROM dbo.Telefono
        WHERE Identificacion = @Identificacion
          AND NumeroTelefono = @NumeroTelefonoAnterior
    )
    BEGIN
        SET @Mensaje = 'El teléfono seleccionado no existe.';
        RETURN;
    END;

    IF NOT EXISTS (
        SELECT 1
        FROM dbo.TipoTelefono
        WHERE IdTipoTelefono = @IdTipoTelefono
          AND Activo = 1
    )
    BEGIN
        SET @Mensaje = 'El tipo de teléfono seleccionado no existe o está inactivo.';
        RETURN;
    END;

    IF EXISTS (
        SELECT 1
        FROM dbo.Telefono
        WHERE Identificacion = @Identificacion
          AND NumeroTelefono = @NumeroTelefonoNuevo
          AND NumeroTelefono <> @NumeroTelefonoAnterior
    )
    BEGIN
        SET @Mensaje = 'Ya existe ese número de teléfono para la persona seleccionada.';
        RETURN;
    END;

    BEGIN TRY
        BEGIN TRANSACTION;

        IF (@EsPrincipal = 1)
        BEGIN
            UPDATE dbo.Telefono
            SET EsPrincipal = 0
            WHERE Identificacion = @Identificacion;
        END;

        UPDATE dbo.Telefono
        SET
            NumeroTelefono = @NumeroTelefonoNuevo,
            IdTipoTelefono = @IdTipoTelefono,
            EsPrincipal = @EsPrincipal,
            Activo = @Activo
        WHERE Identificacion = @Identificacion
          AND NumeroTelefono = @NumeroTelefonoAnterior;

        COMMIT TRANSACTION;

        SET @Resultado = 1;
        SET @Mensaje = 'Teléfono actualizado correctamente.';
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;

        SET @Resultado = 0;
        SET @Mensaje = ERROR_MESSAGE();
    END CATCH;
END;
GO

CREATE OR ALTER PROCEDURE Persona.sp_Telefono_Inactivar
    @Identificacion VARCHAR(45),
    @NumeroTelefono VARCHAR(45),
    @Resultado BIT OUTPUT,
    @Mensaje VARCHAR(500) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    SET @Resultado = 0;
    SET @Mensaje = '';

    SET @Identificacion = LTRIM(RTRIM(ISNULL(@Identificacion, '')));
    SET @NumeroTelefono = LTRIM(RTRIM(ISNULL(@NumeroTelefono, '')));

    IF (@Identificacion = '')
    BEGIN
        SET @Mensaje = 'Debe seleccionar una persona válida.';
        RETURN;
    END;

    IF (@NumeroTelefono = '')
    BEGIN
        SET @Mensaje = 'Debe seleccionar un teléfono válido.';
        RETURN;
    END;

    IF NOT EXISTS (
        SELECT 1
        FROM dbo.Telefono
        WHERE Identificacion = @Identificacion
          AND NumeroTelefono = @NumeroTelefono
    )
    BEGIN
        SET @Mensaje = 'El teléfono no existe.';
        RETURN;
    END;

    UPDATE dbo.Telefono
    SET
        Activo = 0,
        EsPrincipal = 0
    WHERE Identificacion = @Identificacion
      AND NumeroTelefono = @NumeroTelefono;

    SET @Resultado = 1;
    SET @Mensaje = 'Teléfono inactivado correctamente.';
END;
GO