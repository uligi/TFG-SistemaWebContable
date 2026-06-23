IF NOT EXISTS (SELECT 1 FROM sys.schemas WHERE name = 'Persona')
BEGIN
    EXEC('CREATE SCHEMA Persona');
END;
GO

/* =========================================================
   PROVEEDOR - LISTAR
   ========================================================= */
CREATE OR ALTER PROCEDURE Persona.sp_Proveedor_Listar
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        p.IdentificacionProveedor,
        p.RazonSocial,
        p.NombreContacto,
        p.CodigoDistrito,
        d.Nombre AS DistritoNombre,
        c.CodigoCanton,
        c.Nombre AS CantonNombre,
        pr.CodigoProvincia,
        pr.Nombre AS ProvinciaNombre,
        p.DireccionExacta,
        p.DiasCredito,
        p.FechaCreacion,
        p.FechaModificacion,
        p.Activo
    FROM dbo.Proveedor p
    INNER JOIN dbo.Distrito d
        ON d.CodigoDistrito = p.CodigoDistrito
    INNER JOIN dbo.Canton c
        ON c.CodigoCanton = d.CodigoCanton
    INNER JOIN dbo.Provincia pr
        ON pr.CodigoProvincia = c.CodigoProvincia
    ORDER BY
        p.RazonSocial;
END;
GO

/* =========================================================
   PROVEEDOR - LISTAR ACTIVOS
   ========================================================= */
CREATE OR ALTER PROCEDURE Persona.sp_Proveedor_ListarActivos
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        p.IdentificacionProveedor,
        p.RazonSocial,
        p.NombreContacto,
        p.CodigoDistrito,
        d.Nombre AS DistritoNombre,
        c.CodigoCanton,
        c.Nombre AS CantonNombre,
        pr.CodigoProvincia,
        pr.Nombre AS ProvinciaNombre,
        p.DireccionExacta,
        p.DiasCredito,
        p.FechaCreacion,
        p.FechaModificacion,
        p.Activo
    FROM dbo.Proveedor p
    INNER JOIN dbo.Distrito d
        ON d.CodigoDistrito = p.CodigoDistrito
    INNER JOIN dbo.Canton c
        ON c.CodigoCanton = d.CodigoCanton
    INNER JOIN dbo.Provincia pr
        ON pr.CodigoProvincia = c.CodigoProvincia
    WHERE p.Activo = 1
    ORDER BY
        p.RazonSocial;
END;
GO

/* =========================================================
   PROVEEDOR - REGISTRAR
   ========================================================= */
CREATE OR ALTER PROCEDURE Persona.sp_Proveedor_Registrar
    @RazonSocial VARCHAR(200),
    @NombreContacto VARCHAR(100),
    @CodigoDistrito INT,
    @DireccionExacta VARCHAR(250),
    @DiasCredito INT,
    @IdentificacionProveedorGenerada VARCHAR(45) OUTPUT,
    @Resultado INT OUTPUT,
    @Mensaje VARCHAR(500) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @NuevoNumero INT;

    SET @Resultado = 0;
    SET @Mensaje = '';
    SET @IdentificacionProveedorGenerada = '';

    SET @RazonSocial = LTRIM(RTRIM(ISNULL(@RazonSocial, '')));
    SET @NombreContacto = LTRIM(RTRIM(ISNULL(@NombreContacto, '')));
    SET @DireccionExacta = LTRIM(RTRIM(ISNULL(@DireccionExacta, '')));
    SET @DiasCredito = ISNULL(@DiasCredito, 0);

    IF (@RazonSocial = '')
    BEGIN
        SET @Mensaje = 'La razón social es obligatoria.';
        RETURN;
    END;

    IF (@NombreContacto = '')
    BEGIN
        SET @Mensaje = 'El nombre de contacto es obligatorio.';
        RETURN;
    END;

    IF (@CodigoDistrito <= 0)
    BEGIN
        SET @Mensaje = 'Debe seleccionar un distrito.';
        RETURN;
    END;

    IF (@DireccionExacta = '')
    BEGIN
        SET @Mensaje = 'La dirección exacta es obligatoria.';
        RETURN;
    END;

    IF (@DiasCredito < 0)
    BEGIN
        SET @Mensaje = 'Los días de crédito no pueden ser negativos.';
        RETURN;
    END;

    IF NOT EXISTS (
        SELECT 1
        FROM dbo.Distrito
        WHERE CodigoDistrito = @CodigoDistrito
          AND Activo = 1
    )
    BEGIN
        SET @Mensaje = 'El distrito seleccionado no existe o está inactivo.';
        RETURN;
    END;

    IF EXISTS (
        SELECT 1
        FROM dbo.Proveedor
        WHERE UPPER(LTRIM(RTRIM(RazonSocial))) = UPPER(@RazonSocial)
    )
    BEGIN
        SET @Mensaje = 'Ya existe un proveedor con esa razón social.';
        RETURN;
    END;

    BEGIN TRY
        BEGIN TRANSACTION;

        SELECT @NuevoNumero =
            ISNULL(MAX(TRY_CAST(REPLACE(IdentificacionProveedor, 'PROV-', '') AS INT)), 0) + 1
        FROM dbo.Proveedor
        WHERE IdentificacionProveedor LIKE 'PROV-%';

        SET @IdentificacionProveedorGenerada =
            'PROV-' + RIGHT('000000' + CAST(@NuevoNumero AS VARCHAR(10)), 6);

        INSERT INTO dbo.Proveedor
        (
            IdentificacionProveedor,
            RazonSocial,
            NombreContacto,
            CodigoDistrito,
            DireccionExacta,
            DiasCredito,
            FechaCreacion,
            FechaModificacion,
            Activo
        )
        VALUES
        (
            @IdentificacionProveedorGenerada,
            @RazonSocial,
            @NombreContacto,
            @CodigoDistrito,
            @DireccionExacta,
            @DiasCredito,
            GETDATE(),
            GETDATE(),
            1
        );

        COMMIT TRANSACTION;

        SET @Resultado = 1;
        SET @Mensaje = 'Proveedor registrado correctamente.';
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;

        SET @Resultado = 0;
        SET @Mensaje = ERROR_MESSAGE();
        SET @IdentificacionProveedorGenerada = '';
    END CATCH;
END;
GO

/* =========================================================
   PROVEEDOR - EDITAR
   ========================================================= */
CREATE OR ALTER PROCEDURE Persona.sp_Proveedor_Editar
    @IdentificacionProveedor VARCHAR(45),
    @RazonSocial VARCHAR(200),
    @NombreContacto VARCHAR(100),
    @CodigoDistrito INT,
    @DireccionExacta VARCHAR(250),
    @DiasCredito INT,
    @Activo BIT,
    @Resultado BIT OUTPUT,
    @Mensaje VARCHAR(500) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    SET @Resultado = 0;
    SET @Mensaje = '';

    SET @IdentificacionProveedor = LTRIM(RTRIM(ISNULL(@IdentificacionProveedor, '')));
    SET @RazonSocial = LTRIM(RTRIM(ISNULL(@RazonSocial, '')));
    SET @NombreContacto = LTRIM(RTRIM(ISNULL(@NombreContacto, '')));
    SET @DireccionExacta = LTRIM(RTRIM(ISNULL(@DireccionExacta, '')));
    SET @DiasCredito = ISNULL(@DiasCredito, 0);

    IF (@IdentificacionProveedor = '')
    BEGIN
        SET @Mensaje = 'Debe seleccionar un proveedor válido.';
        RETURN;
    END;

    IF (@RazonSocial = '')
    BEGIN
        SET @Mensaje = 'La razón social es obligatoria.';
        RETURN;
    END;

    IF (@NombreContacto = '')
    BEGIN
        SET @Mensaje = 'El nombre de contacto es obligatorio.';
        RETURN;
    END;

    IF (@CodigoDistrito <= 0)
    BEGIN
        SET @Mensaje = 'Debe seleccionar un distrito.';
        RETURN;
    END;

    IF (@DireccionExacta = '')
    BEGIN
        SET @Mensaje = 'La dirección exacta es obligatoria.';
        RETURN;
    END;

    IF (@DiasCredito < 0)
    BEGIN
        SET @Mensaje = 'Los días de crédito no pueden ser negativos.';
        RETURN;
    END;

    IF NOT EXISTS (
        SELECT 1
        FROM dbo.Proveedor
        WHERE IdentificacionProveedor = @IdentificacionProveedor
    )
    BEGIN
        SET @Mensaje = 'El proveedor no existe.';
        RETURN;
    END;

    IF NOT EXISTS (
        SELECT 1
        FROM dbo.Distrito
        WHERE CodigoDistrito = @CodigoDistrito
          AND Activo = 1
    )
    BEGIN
        SET @Mensaje = 'El distrito seleccionado no existe o está inactivo.';
        RETURN;
    END;

    IF EXISTS (
        SELECT 1
        FROM dbo.Proveedor
        WHERE UPPER(LTRIM(RTRIM(RazonSocial))) = UPPER(@RazonSocial)
          AND IdentificacionProveedor <> @IdentificacionProveedor
    )
    BEGIN
        SET @Mensaje = 'Ya existe otro proveedor con esa razón social.';
        RETURN;
    END;

    UPDATE dbo.Proveedor
    SET
        RazonSocial = @RazonSocial,
        NombreContacto = @NombreContacto,
        CodigoDistrito = @CodigoDistrito,
        DireccionExacta = @DireccionExacta,
        DiasCredito = @DiasCredito,
        FechaModificacion = GETDATE(),
        Activo = @Activo
    WHERE IdentificacionProveedor = @IdentificacionProveedor;

    SET @Resultado = 1;
    SET @Mensaje = 'Proveedor actualizado correctamente.';
END;
GO

/* =========================================================
   PROVEEDOR - INACTIVAR
   ========================================================= */
CREATE OR ALTER PROCEDURE Persona.sp_Proveedor_Inactivar
    @IdentificacionProveedor VARCHAR(45),
    @Resultado BIT OUTPUT,
    @Mensaje VARCHAR(500) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    SET @Resultado = 0;
    SET @Mensaje = '';

    SET @IdentificacionProveedor = LTRIM(RTRIM(ISNULL(@IdentificacionProveedor, '')));

    IF (@IdentificacionProveedor = '')
    BEGIN
        SET @Mensaje = 'Debe seleccionar un proveedor válido.';
        RETURN;
    END;

    IF (@IdentificacionProveedor = '000000000')
    BEGIN
        SET @Mensaje = 'No se puede inactivar el proveedor general del sistema.';
        RETURN;
    END;

    IF NOT EXISTS (
        SELECT 1
        FROM dbo.Proveedor
        WHERE IdentificacionProveedor = @IdentificacionProveedor
    )
    BEGIN
        SET @Mensaje = 'El proveedor no existe.';
        RETURN;
    END;

    IF EXISTS (
        SELECT 1
        FROM dbo.CuentaPorPagar
        WHERE IdentificacionProveedor = @IdentificacionProveedor
          AND Activo = 1
    )
    BEGIN
        SET @Mensaje = 'No se puede inactivar el proveedor porque tiene cuentas por pagar activas.';
        RETURN;
    END;

    IF EXISTS (
        SELECT 1
        FROM dbo.Gasto
        WHERE IdentificacionProveedor = @IdentificacionProveedor
          AND Activo = 1
    )
    BEGIN
        SET @Mensaje = 'No se puede inactivar el proveedor porque tiene gastos activos asociados.';
        RETURN;
    END;

    BEGIN TRY
        BEGIN TRANSACTION;

        UPDATE dbo.Proveedor
        SET
            Activo = 0,
            FechaModificacion = GETDATE()
        WHERE IdentificacionProveedor = @IdentificacionProveedor;

        UPDATE dbo.TelefonoProveedor
        SET
            Activo = 0,
            EsPrincipal = 0
        WHERE IdentificacionProveedor = @IdentificacionProveedor;

        UPDATE dbo.CorreoProveedor
        SET
            Activo = 0,
            EsPrincipal = 0
        WHERE IdentificacionProveedor = @IdentificacionProveedor;

        COMMIT TRANSACTION;

        SET @Resultado = 1;
        SET @Mensaje = 'Proveedor inactivado correctamente.';
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;

        SET @Resultado = 0;
        SET @Mensaje = ERROR_MESSAGE();
    END CATCH;
END;
GO