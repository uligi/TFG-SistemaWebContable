IF NOT EXISTS (SELECT 1 FROM sys.schemas WHERE name = 'Persona')
BEGIN
    EXEC('CREATE SCHEMA Persona');
END;
GO

CREATE OR ALTER PROCEDURE Persona.sp_Correo_Listar
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        c.Identificacion,
        c.DireccionCorreo,
        c.IdTipoCorreo,
        tc.Nombre AS TipoCorreoNombre,
        c.EsPrincipal,
        c.Activo,

        p.Nombre,
        p.PrimerApellido,
        p.SegundoApellido
    FROM dbo.Correo c
    INNER JOIN dbo.Persona p
        ON p.Identificacion = c.Identificacion
    INNER JOIN dbo.TipoCorreo tc
        ON tc.IdTipoCorreo = c.IdTipoCorreo
    ORDER BY
        p.Nombre,
        p.PrimerApellido,
        c.EsPrincipal DESC,
        c.DireccionCorreo;
END;
GO

CREATE OR ALTER PROCEDURE Persona.sp_Correo_ListarPorPersona
    @Identificacion VARCHAR(45)
AS
BEGIN
    SET NOCOUNT ON;

    SET @Identificacion = LTRIM(RTRIM(ISNULL(@Identificacion, '')));

    SELECT
        c.Identificacion,
        c.DireccionCorreo,
        c.IdTipoCorreo,
        tc.Nombre AS TipoCorreoNombre,
        c.EsPrincipal,
        c.Activo,

        p.Nombre,
        p.PrimerApellido,
        p.SegundoApellido
    FROM dbo.Correo c
    INNER JOIN dbo.Persona p
        ON p.Identificacion = c.Identificacion
    INNER JOIN dbo.TipoCorreo tc
        ON tc.IdTipoCorreo = c.IdTipoCorreo
    WHERE c.Identificacion = @Identificacion
    ORDER BY
        c.EsPrincipal DESC,
        c.DireccionCorreo;
END;
GO

CREATE OR ALTER PROCEDURE Persona.sp_Correo_Registrar
    @Identificacion VARCHAR(45),
    @DireccionCorreo VARCHAR(255),
    @IdTipoCorreo INT,
    @EsPrincipal BIT,
    @Resultado INT OUTPUT,
    @Mensaje VARCHAR(500) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    SET @Resultado = 0;
    SET @Mensaje = '';

    SET @Identificacion = LTRIM(RTRIM(ISNULL(@Identificacion, '')));
    SET @DireccionCorreo = LOWER(LTRIM(RTRIM(ISNULL(@DireccionCorreo, ''))));

    IF (@Identificacion = '')
    BEGIN
        SET @Mensaje = 'Debe seleccionar una persona.';
        RETURN;
    END;

    IF (@DireccionCorreo = '')
    BEGIN
        SET @Mensaje = 'La dirección de correo es obligatoria.';
        RETURN;
    END;

    IF (LEN(@DireccionCorreo) > 255)
    BEGIN
        SET @Mensaje = 'La dirección de correo no puede superar los 255 caracteres.';
        RETURN;
    END;

    IF (
        @DireccionCorreo NOT LIKE '_%@_%._%'
        OR @DireccionCorreo LIKE '% %'
        OR @DireccionCorreo LIKE '%..%'
        OR @DireccionCorreo LIKE '.%'
        OR @DireccionCorreo LIKE '%.'
        OR @DireccionCorreo LIKE '%@%@%'
    )
    BEGIN
        SET @Mensaje = 'Debe ingresar una dirección de correo válida.';
        RETURN;
    END;

    IF (@IdTipoCorreo <= 0)
    BEGIN
        SET @Mensaje = 'Debe seleccionar un tipo de correo.';
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
        FROM dbo.TipoCorreo
        WHERE IdTipoCorreo = @IdTipoCorreo
          AND Activo = 1
    )
    BEGIN
        SET @Mensaje = 'El tipo de correo seleccionado no existe o está inactivo.';
        RETURN;
    END;

    IF EXISTS (
        SELECT 1
        FROM dbo.Correo
        WHERE Identificacion = @Identificacion
          AND LOWER(DireccionCorreo) = @DireccionCorreo
    )
    BEGIN
        SET @Mensaje = 'Ya existe ese correo para la persona seleccionada.';
        RETURN;
    END;

    BEGIN TRY
        BEGIN TRANSACTION;

        IF (@EsPrincipal = 1)
        BEGIN
            UPDATE dbo.Correo
            SET EsPrincipal = 0
            WHERE Identificacion = @Identificacion;
        END;

        INSERT INTO dbo.Correo
        (
            Identificacion,
            DireccionCorreo,
            IdTipoCorreo,
            EsPrincipal,
            Activo
        )
        VALUES
        (
            @Identificacion,
            @DireccionCorreo,
            @IdTipoCorreo,
            @EsPrincipal,
            1
        );

        COMMIT TRANSACTION;

        SET @Resultado = 1;
        SET @Mensaje = 'Correo registrado correctamente.';
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;

        SET @Resultado = 0;
        SET @Mensaje = ERROR_MESSAGE();
    END CATCH;
END;
GO

CREATE OR ALTER PROCEDURE Persona.sp_Correo_Editar
    @Identificacion VARCHAR(45),
    @DireccionCorreoAnterior VARCHAR(255),
    @DireccionCorreoNuevo VARCHAR(255),
    @IdTipoCorreo INT,
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
    SET @DireccionCorreoAnterior = LOWER(LTRIM(RTRIM(ISNULL(@DireccionCorreoAnterior, ''))));
    SET @DireccionCorreoNuevo = LOWER(LTRIM(RTRIM(ISNULL(@DireccionCorreoNuevo, ''))));

    IF (@Identificacion = '')
    BEGIN
        SET @Mensaje = 'Debe seleccionar una persona válida.';
        RETURN;
    END;

    IF (@DireccionCorreoAnterior = '')
    BEGIN
        SET @Mensaje = 'Debe seleccionar un correo válido.';
        RETURN;
    END;

    IF (@DireccionCorreoNuevo = '')
    BEGIN
        SET @Mensaje = 'La dirección de correo es obligatoria.';
        RETURN;
    END;

    IF (LEN(@DireccionCorreoNuevo) > 255)
    BEGIN
        SET @Mensaje = 'La dirección de correo no puede superar los 255 caracteres.';
        RETURN;
    END;

    IF (
        @DireccionCorreoNuevo NOT LIKE '_%@_%._%'
        OR @DireccionCorreoNuevo LIKE '% %'
        OR @DireccionCorreoNuevo LIKE '%..%'
        OR @DireccionCorreoNuevo LIKE '.%'
        OR @DireccionCorreoNuevo LIKE '%.'
        OR @DireccionCorreoNuevo LIKE '%@%@%'
    )
    BEGIN
        SET @Mensaje = 'Debe ingresar una dirección de correo válida.';
        RETURN;
    END;

    IF (@IdTipoCorreo <= 0)
    BEGIN
        SET @Mensaje = 'Debe seleccionar un tipo de correo.';
        RETURN;
    END;

    IF NOT EXISTS (
        SELECT 1
        FROM dbo.Correo
        WHERE Identificacion = @Identificacion
          AND LOWER(DireccionCorreo) = @DireccionCorreoAnterior
    )
    BEGIN
        SET @Mensaje = 'El correo seleccionado no existe.';
        RETURN;
    END;

    IF NOT EXISTS (
        SELECT 1
        FROM dbo.TipoCorreo
        WHERE IdTipoCorreo = @IdTipoCorreo
          AND Activo = 1
    )
    BEGIN
        SET @Mensaje = 'El tipo de correo seleccionado no existe o está inactivo.';
        RETURN;
    END;

    IF EXISTS (
        SELECT 1
        FROM dbo.Correo
        WHERE Identificacion = @Identificacion
          AND LOWER(DireccionCorreo) = @DireccionCorreoNuevo
          AND LOWER(DireccionCorreo) <> @DireccionCorreoAnterior
    )
    BEGIN
        SET @Mensaje = 'Ya existe ese correo para la persona seleccionada.';
        RETURN;
    END;

    BEGIN TRY
        BEGIN TRANSACTION;

        IF (@EsPrincipal = 1)
        BEGIN
            UPDATE dbo.Correo
            SET EsPrincipal = 0
            WHERE Identificacion = @Identificacion;
        END;

        UPDATE dbo.Correo
        SET
            DireccionCorreo = @DireccionCorreoNuevo,
            IdTipoCorreo = @IdTipoCorreo,
            EsPrincipal = @EsPrincipal,
            Activo = @Activo
        WHERE Identificacion = @Identificacion
          AND LOWER(DireccionCorreo) = @DireccionCorreoAnterior;

        COMMIT TRANSACTION;

        SET @Resultado = 1;
        SET @Mensaje = 'Correo actualizado correctamente.';
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;

        SET @Resultado = 0;
        SET @Mensaje = ERROR_MESSAGE();
    END CATCH;
END;
GO

CREATE OR ALTER PROCEDURE Persona.sp_Correo_Inactivar
    @Identificacion VARCHAR(45),
    @DireccionCorreo VARCHAR(255),
    @Resultado BIT OUTPUT,
    @Mensaje VARCHAR(500) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    SET @Resultado = 0;
    SET @Mensaje = '';

    SET @Identificacion = LTRIM(RTRIM(ISNULL(@Identificacion, '')));
    SET @DireccionCorreo = LOWER(LTRIM(RTRIM(ISNULL(@DireccionCorreo, ''))));

    IF (@Identificacion = '')
    BEGIN
        SET @Mensaje = 'Debe seleccionar una persona válida.';
        RETURN;
    END;

    IF (@DireccionCorreo = '')
    BEGIN
        SET @Mensaje = 'Debe seleccionar un correo válido.';
        RETURN;
    END;

    IF NOT EXISTS (
        SELECT 1
        FROM dbo.Correo
        WHERE Identificacion = @Identificacion
          AND LOWER(DireccionCorreo) = @DireccionCorreo
    )
    BEGIN
        SET @Mensaje = 'El correo no existe.';
        RETURN;
    END;

    UPDATE dbo.Correo
    SET
        Activo = 0,
        EsPrincipal = 0
    WHERE Identificacion = @Identificacion
      AND LOWER(DireccionCorreo) = @DireccionCorreo;

    SET @Resultado = 1;
    SET @Mensaje = 'Correo inactivado correctamente.';
END;
GO