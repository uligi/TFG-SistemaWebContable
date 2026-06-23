IF NOT EXISTS (SELECT 1 FROM sys.schemas WHERE name = 'Persona')
BEGIN
    EXEC('CREATE SCHEMA Persona');
END;
GO

/* =========================================================
   CORREO PROVEEDOR - LISTAR
   ========================================================= */
CREATE OR ALTER PROCEDURE Persona.sp_CorreoProveedor_Listar
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        cp.IdentificacionProveedor,
        p.RazonSocial,
        cp.DireccionCorreo,
        cp.IdTipoCorreo,
        tc.Nombre AS TipoCorreoNombre,
        cp.EsPrincipal,
        cp.Activo
    FROM dbo.CorreoProveedor cp
    INNER JOIN dbo.Proveedor p
        ON p.IdentificacionProveedor = cp.IdentificacionProveedor
    INNER JOIN dbo.TipoCorreo tc
        ON tc.IdTipoCorreo = cp.IdTipoCorreo
    ORDER BY
        p.RazonSocial,
        cp.EsPrincipal DESC,
        cp.DireccionCorreo;
END;
GO

/* =========================================================
   CORREO PROVEEDOR - LISTAR POR PROVEEDOR
   ========================================================= */
CREATE OR ALTER PROCEDURE Persona.sp_CorreoProveedor_ListarPorProveedor
    @IdentificacionProveedor VARCHAR(45)
AS
BEGIN
    SET NOCOUNT ON;

    SET @IdentificacionProveedor = LTRIM(RTRIM(ISNULL(@IdentificacionProveedor, '')));

    SELECT
        cp.IdentificacionProveedor,
        p.RazonSocial,
        cp.DireccionCorreo,
        cp.IdTipoCorreo,
        tc.Nombre AS TipoCorreoNombre,
        cp.EsPrincipal,
        cp.Activo
    FROM dbo.CorreoProveedor cp
    INNER JOIN dbo.Proveedor p
        ON p.IdentificacionProveedor = cp.IdentificacionProveedor
    INNER JOIN dbo.TipoCorreo tc
        ON tc.IdTipoCorreo = cp.IdTipoCorreo
    WHERE cp.IdentificacionProveedor = @IdentificacionProveedor
    ORDER BY
        cp.EsPrincipal DESC,
        cp.DireccionCorreo;
END;
GO

/* =========================================================
   CORREO PROVEEDOR - REGISTRAR
   ========================================================= */
CREATE OR ALTER PROCEDURE Persona.sp_CorreoProveedor_Registrar
    @IdentificacionProveedor VARCHAR(45),
    @DireccionCorreo VARCHAR(150),
    @IdTipoCorreo INT,
    @EsPrincipal BIT,
    @Resultado INT OUTPUT,
    @Mensaje VARCHAR(500) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    SET @Resultado = 0;
    SET @Mensaje = '';

    SET @IdentificacionProveedor = LTRIM(RTRIM(ISNULL(@IdentificacionProveedor, '')));
    SET @DireccionCorreo = LOWER(LTRIM(RTRIM(ISNULL(@DireccionCorreo, ''))));

    IF (@IdentificacionProveedor = '')
    BEGIN
        SET @Mensaje = 'Debe seleccionar un proveedor.';
        RETURN;
    END;

    IF (@DireccionCorreo = '')
    BEGIN
        SET @Mensaje = 'La dirección de correo es obligatoria.';
        RETURN;
    END;

    IF (LEN(@DireccionCorreo) > 150)
    BEGIN
        SET @Mensaje = 'La dirección de correo no puede superar los 150 caracteres.';
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
        FROM dbo.Proveedor
        WHERE IdentificacionProveedor = @IdentificacionProveedor
          AND Activo = 1
    )
    BEGIN
        SET @Mensaje = 'El proveedor seleccionado no existe o está inactivo.';
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
        FROM dbo.CorreoProveedor
        WHERE IdentificacionProveedor = @IdentificacionProveedor
          AND LOWER(DireccionCorreo) = @DireccionCorreo
    )
    BEGIN
        SET @Mensaje = 'Ya existe ese correo para el proveedor seleccionado.';
        RETURN;
    END;

    BEGIN TRY
        BEGIN TRANSACTION;

        IF (@EsPrincipal = 1)
        BEGIN
            UPDATE dbo.CorreoProveedor
            SET EsPrincipal = 0
            WHERE IdentificacionProveedor = @IdentificacionProveedor;
        END;

        INSERT INTO dbo.CorreoProveedor
        (
            IdentificacionProveedor,
            DireccionCorreo,
            IdTipoCorreo,
            EsPrincipal,
            Activo
        )
        VALUES
        (
            @IdentificacionProveedor,
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

/* =========================================================
   CORREO PROVEEDOR - EDITAR
   ========================================================= */
CREATE OR ALTER PROCEDURE Persona.sp_CorreoProveedor_Editar
    @IdentificacionProveedor VARCHAR(45),
    @DireccionCorreoAnterior VARCHAR(150),
    @DireccionCorreoNuevo VARCHAR(150),
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

    SET @IdentificacionProveedor = LTRIM(RTRIM(ISNULL(@IdentificacionProveedor, '')));
    SET @DireccionCorreoAnterior = LOWER(LTRIM(RTRIM(ISNULL(@DireccionCorreoAnterior, ''))));
    SET @DireccionCorreoNuevo = LOWER(LTRIM(RTRIM(ISNULL(@DireccionCorreoNuevo, ''))));

    IF (@IdentificacionProveedor = '')
    BEGIN
        SET @Mensaje = 'Debe seleccionar un proveedor válido.';
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

    IF (LEN(@DireccionCorreoNuevo) > 150)
    BEGIN
        SET @Mensaje = 'La dirección de correo no puede superar los 150 caracteres.';
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
        FROM dbo.CorreoProveedor
        WHERE IdentificacionProveedor = @IdentificacionProveedor
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
        FROM dbo.CorreoProveedor
        WHERE IdentificacionProveedor = @IdentificacionProveedor
          AND LOWER(DireccionCorreo) = @DireccionCorreoNuevo
          AND LOWER(DireccionCorreo) <> @DireccionCorreoAnterior
    )
    BEGIN
        SET @Mensaje = 'Ya existe ese correo para el proveedor seleccionado.';
        RETURN;
    END;

    BEGIN TRY
        BEGIN TRANSACTION;

        IF (@EsPrincipal = 1)
        BEGIN
            UPDATE dbo.CorreoProveedor
            SET EsPrincipal = 0
            WHERE IdentificacionProveedor = @IdentificacionProveedor;
        END;

        UPDATE dbo.CorreoProveedor
        SET
            DireccionCorreo = @DireccionCorreoNuevo,
            IdTipoCorreo = @IdTipoCorreo,
            EsPrincipal = @EsPrincipal,
            Activo = @Activo
        WHERE IdentificacionProveedor = @IdentificacionProveedor
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

/* =========================================================
   CORREO PROVEEDOR - INACTIVAR
   ========================================================= */
CREATE OR ALTER PROCEDURE Persona.sp_CorreoProveedor_Inactivar
    @IdentificacionProveedor VARCHAR(45),
    @DireccionCorreo VARCHAR(150),
    @Resultado BIT OUTPUT,
    @Mensaje VARCHAR(500) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    SET @Resultado = 0;
    SET @Mensaje = '';

    SET @IdentificacionProveedor = LTRIM(RTRIM(ISNULL(@IdentificacionProveedor, '')));
    SET @DireccionCorreo = LOWER(LTRIM(RTRIM(ISNULL(@DireccionCorreo, ''))));

    IF (@IdentificacionProveedor = '')
    BEGIN
        SET @Mensaje = 'Debe seleccionar un proveedor válido.';
        RETURN;
    END;

    IF (@DireccionCorreo = '')
    BEGIN
        SET @Mensaje = 'Debe seleccionar un correo válido.';
        RETURN;
    END;

    IF NOT EXISTS (
        SELECT 1
        FROM dbo.CorreoProveedor
        WHERE IdentificacionProveedor = @IdentificacionProveedor
          AND LOWER(DireccionCorreo) = @DireccionCorreo
    )
    BEGIN
        SET @Mensaje = 'El correo no existe.';
        RETURN;
    END;

    UPDATE dbo.CorreoProveedor
    SET
        Activo = 0,
        EsPrincipal = 0
    WHERE IdentificacionProveedor = @IdentificacionProveedor
      AND LOWER(DireccionCorreo) = @DireccionCorreo;

    SET @Resultado = 1;
    SET @Mensaje = 'Correo inactivado correctamente.';
END;
GO