IF NOT EXISTS (SELECT 1 FROM sys.schemas WHERE name = 'Persona')
BEGIN
    EXEC('CREATE SCHEMA Persona');
END;
GO

CREATE OR ALTER PROCEDURE Persona.sp_Cliente_Listar
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        p.Identificacion,
        p.Nombre,
        p.PrimerApellido,
        p.SegundoApellido,
        p.FechaNacimiento,
        p.CodigoDistrito,
        d.Nombre AS DistritoNombre,
        c.CodigoCanton,
        c.Nombre AS CantonNombre,
        pr.CodigoProvincia,
        pr.Nombre AS ProvinciaNombre,
        p.DireccionExacta AS Direccion,

        cl.CodigoCliente,
        cl.LimiteCredito,
        cl.DiasCredito,
        cl.FechaCreacion,
        cl.FechaModificacion,
        cl.Activo
    FROM dbo.Cliente cl
    INNER JOIN dbo.Persona p
        ON p.Identificacion = cl.Identificacion
    INNER JOIN dbo.Distrito d
        ON d.CodigoDistrito = p.CodigoDistrito
    INNER JOIN dbo.Canton c
        ON c.CodigoCanton = d.CodigoCanton
    INNER JOIN dbo.Provincia pr
        ON pr.CodigoProvincia = c.CodigoProvincia
    ORDER BY cl.FechaCreacion DESC;
END;
GO

CREATE OR ALTER PROCEDURE Persona.sp_Cliente_Registrar
    @Identificacion VARCHAR(45),
    @Nombre VARCHAR(45),
    @PrimerApellido VARCHAR(45),
    @SegundoApellido VARCHAR(45),
    @FechaNacimiento DATE,
    @CodigoDistrito INT,
    @Direccion VARCHAR(250),
    @LimiteCredito DECIMAL(18,2),
    @DiasCredito INT,
    @Resultado INT OUTPUT,
    @Mensaje VARCHAR(500) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @NuevoNumero INT;
    DECLARE @CodigoCliente VARCHAR(45);

    SET @Resultado = 0;
    SET @Mensaje = '';

    SET @Identificacion = LTRIM(RTRIM(@Identificacion));
    SET @Nombre = LTRIM(RTRIM(@Nombre));
    SET @PrimerApellido = LTRIM(RTRIM(@PrimerApellido));
    SET @SegundoApellido = LTRIM(RTRIM(ISNULL(@SegundoApellido, '')));
    SET @Direccion = LTRIM(RTRIM(ISNULL(@Direccion, '')));

    SET @LimiteCredito = ISNULL(@LimiteCredito, 0);
    SET @DiasCredito = ISNULL(@DiasCredito, 0);

    IF (@Identificacion = '')
    BEGIN
        SET @Mensaje = 'La identificación es obligatoria.';
        RETURN;
    END;

    IF (@Nombre = '')
    BEGIN
        SET @Mensaje = 'El nombre es obligatorio.';
        RETURN;
    END;

    IF (@PrimerApellido = '')
    BEGIN
        SET @Mensaje = 'El primer apellido es obligatorio.';
        RETURN;
    END;

IF (@FechaNacimiento IS NULL)
BEGIN
    SET @Mensaje = 'La fecha de nacimiento es obligatoria.';
    RETURN;
END;

IF (@FechaNacimiento >= CAST(GETDATE() AS DATE))
BEGIN
    SET @Mensaje = 'La fecha de nacimiento no puede ser igual o mayor a la fecha actual.';
    RETURN;
END;

IF (DATEADD(YEAR, 18, @FechaNacimiento) > CAST(GETDATE() AS DATE))
BEGIN
    SET @Mensaje = 'Solo se pueden registrar empleados mayores de edad.';
    RETURN;
END;

    IF (@CodigoDistrito <= 0)
    BEGIN
        SET @Mensaje = 'Debe seleccionar un distrito.';
        RETURN;
    END;

    IF (@Direccion = '')
    BEGIN
        SET @Mensaje = 'La dirección exacta es obligatoria.';
        RETURN;
    END;

    IF (@LimiteCredito < 0)
    BEGIN
        SET @Mensaje = 'El límite de crédito no puede ser negativo.';
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
        FROM dbo.Cliente
        WHERE Identificacion = @Identificacion
          AND Activo = 1
    )
    BEGIN
        SET @Mensaje = 'Ya existe un cliente activo con esa identificación.';
        RETURN;
    END;

    BEGIN TRY
        BEGIN TRANSACTION;

        IF NOT EXISTS (
            SELECT 1
            FROM dbo.Persona
            WHERE Identificacion = @Identificacion
        )
        BEGIN
            INSERT INTO dbo.Persona
            (
                Identificacion,
                Nombre,
                PrimerApellido,
                SegundoApellido,
                FechaNacimiento,
                CodigoDistrito,
                DireccionExacta,
                FechaCreacion,
                FechaModificacion,
                Activo
            )
            VALUES
            (
                @Identificacion,
                @Nombre,
                @PrimerApellido,
                @SegundoApellido,
                @FechaNacimiento,
                @CodigoDistrito,
                @Direccion,
                GETDATE(),
                GETDATE(),
                1
            );
        END
        ELSE
        BEGIN
            UPDATE dbo.Persona
            SET
                Nombre = @Nombre,
                PrimerApellido = @PrimerApellido,
                SegundoApellido = @SegundoApellido,
                FechaNacimiento = @FechaNacimiento,
                CodigoDistrito = @CodigoDistrito,
                DireccionExacta = @Direccion,
                FechaModificacion = GETDATE(),
                Activo = 1
            WHERE Identificacion = @Identificacion;
        END;

        SELECT @NuevoNumero = ISNULL(MAX(CAST(REPLACE(CodigoCliente, 'CLI-', '') AS INT)), 0) + 1
        FROM dbo.Cliente
        WHERE CodigoCliente LIKE 'CLI-%'
          AND ISNUMERIC(REPLACE(CodigoCliente, 'CLI-', '')) = 1;

        SET @CodigoCliente = 'CLI-' + RIGHT('000000' + CAST(@NuevoNumero AS VARCHAR(10)), 6);

        INSERT INTO dbo.Cliente
        (
            Identificacion,
            CodigoCliente,
            LimiteCredito,
            DiasCredito,
            FechaCreacion,
            FechaModificacion,
            Activo
        )
        VALUES
        (
            @Identificacion,
            @CodigoCliente,
            @LimiteCredito,
            @DiasCredito,
            GETDATE(),
            GETDATE(),
            1
        );

        COMMIT TRANSACTION;

        SET @Resultado = 1;
        SET @Mensaje = 'Cliente registrado correctamente.';
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;

        SET @Resultado = 0;
        SET @Mensaje = ERROR_MESSAGE();
    END CATCH;
END;
GO

CREATE OR ALTER PROCEDURE Persona.sp_Cliente_Editar
    @Identificacion VARCHAR(45),
    @Nombre VARCHAR(45),
    @PrimerApellido VARCHAR(45),
    @SegundoApellido VARCHAR(45),
    @FechaNacimiento DATE,
    @CodigoDistrito INT,
    @Direccion VARCHAR(250),
    @LimiteCredito DECIMAL(18,2),
    @DiasCredito INT,
    @Activo BIT,
    @Resultado BIT OUTPUT,
    @Mensaje VARCHAR(500) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    SET @Resultado = 0;
    SET @Mensaje = '';

    SET @Identificacion = LTRIM(RTRIM(@Identificacion));
    SET @Nombre = LTRIM(RTRIM(@Nombre));
    SET @PrimerApellido = LTRIM(RTRIM(@PrimerApellido));
    SET @SegundoApellido = LTRIM(RTRIM(ISNULL(@SegundoApellido, '')));
    SET @Direccion = LTRIM(RTRIM(ISNULL(@Direccion, '')));

    SET @LimiteCredito = ISNULL(@LimiteCredito, 0);
    SET @DiasCredito = ISNULL(@DiasCredito, 0);

    IF (@Identificacion = '')
    BEGIN
        SET @Mensaje = 'Debe seleccionar un cliente válido.';
        RETURN;
    END;

    IF (@Nombre = '')
    BEGIN
        SET @Mensaje = 'El nombre es obligatorio.';
        RETURN;
    END;

    IF (@PrimerApellido = '')
    BEGIN
        SET @Mensaje = 'El primer apellido es obligatorio.';
        RETURN;
    END;

    IF (@FechaNacimiento IS NULL)
    BEGIN
        SET @FechaNacimiento = '19000101';
    END;

    IF (@CodigoDistrito <= 0)
    BEGIN
        SET @Mensaje = 'Debe seleccionar un distrito.';
        RETURN;
    END;

    IF (@Direccion = '')
    BEGIN
        SET @Mensaje = 'La dirección exacta es obligatoria.';
        RETURN;
    END;

    IF (@LimiteCredito < 0)
    BEGIN
        SET @Mensaje = 'El límite de crédito no puede ser negativo.';
        RETURN;
    END;

    IF (@DiasCredito < 0)
    BEGIN
        SET @Mensaje = 'Los días de crédito no pueden ser negativos.';
        RETURN;
    END;

    IF NOT EXISTS (
        SELECT 1
        FROM dbo.Cliente
        WHERE Identificacion = @Identificacion
    )
    BEGIN
        SET @Mensaje = 'El cliente no existe.';
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

    BEGIN TRY
        BEGIN TRANSACTION;

        UPDATE dbo.Persona
        SET
            Nombre = @Nombre,
            PrimerApellido = @PrimerApellido,
            SegundoApellido = @SegundoApellido,
            FechaNacimiento = @FechaNacimiento,
            CodigoDistrito = @CodigoDistrito,
            DireccionExacta = @Direccion,
            FechaModificacion = GETDATE(),
            Activo = @Activo
        WHERE Identificacion = @Identificacion;

        UPDATE dbo.Cliente
        SET
            LimiteCredito = @LimiteCredito,
            DiasCredito = @DiasCredito,
            FechaModificacion = GETDATE(),
            Activo = @Activo
        WHERE Identificacion = @Identificacion;

        COMMIT TRANSACTION;

        SET @Resultado = 1;
        SET @Mensaje = 'Cliente actualizado correctamente.';
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;

        SET @Resultado = 0;
        SET @Mensaje = ERROR_MESSAGE();
    END CATCH;
END;
GO

CREATE OR ALTER PROCEDURE Persona.sp_Cliente_Inactivar
    @Identificacion VARCHAR(45),
    @Resultado BIT OUTPUT,
    @Mensaje VARCHAR(500) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    SET @Resultado = 0;
    SET @Mensaje = '';

    SET @Identificacion = LTRIM(RTRIM(@Identificacion));

    IF (@Identificacion = '')
    BEGIN
        SET @Mensaje = 'Debe seleccionar un cliente válido.';
        RETURN;
    END;

    IF NOT EXISTS (
        SELECT 1
        FROM dbo.Cliente
        WHERE Identificacion = @Identificacion
    )
    BEGIN
        SET @Mensaje = 'El cliente no existe.';
        RETURN;
    END;

    UPDATE dbo.Cliente
    SET
        Activo = 0,
        FechaModificacion = GETDATE()
    WHERE Identificacion = @Identificacion;

    SET @Resultado = 1;
    SET @Mensaje = 'Cliente inactivado correctamente.';
END;
GO