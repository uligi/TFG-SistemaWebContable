IF NOT EXISTS (SELECT 1 FROM sys.schemas WHERE name = 'Persona')
BEGIN
    EXEC('CREATE SCHEMA Persona');
END;
GO

CREATE OR ALTER PROCEDURE Persona.sp_Empleado_Listar
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

        e.CodigoEmpleado,
        e.IdPuesto,
        pu.Nombre AS PuestoNombre,
        e.FechaIngreso,
        e.IdRol,
        r.Nombre AS RolNombre,
        e.NombreUsuario,
        e.UltimoAcceso,
        e.FechaCreacion,
        e.FechaModificacion,
        e.RestablecerClave,
        e.Activo
    FROM dbo.Empleado e
    INNER JOIN dbo.Persona p
        ON p.Identificacion = e.Identificacion
    INNER JOIN dbo.Rol r
        ON r.IdRol = e.IdRol
    INNER JOIN dbo.Puesto pu
        ON pu.IdPuesto = e.IdPuesto
    INNER JOIN dbo.Distrito d
        ON d.CodigoDistrito = p.CodigoDistrito
    INNER JOIN dbo.Canton c
        ON c.CodigoCanton = d.CodigoCanton
    INNER JOIN dbo.Provincia pr
        ON pr.CodigoProvincia = c.CodigoProvincia
    ORDER BY
        p.Nombre,
        p.PrimerApellido;
END;
GO

CREATE OR ALTER PROCEDURE Persona.sp_Empleado_Registrar
    @Identificacion VARCHAR(45),
    @Nombre VARCHAR(45),
    @PrimerApellido VARCHAR(45),
    @SegundoApellido VARCHAR(45),
    @FechaNacimiento DATE,
    @CodigoDistrito INT,
    @Direccion VARCHAR(250),

    @IdPuesto INT,
    @FechaIngreso DATE,
    @IdRol INT,
    @ClaveHash VARCHAR(255),

    @NombreUsuarioGenerado VARCHAR(45) OUTPUT,
    @CodigoEmpleadoGenerado VARCHAR(45) OUTPUT,
    @Resultado INT OUTPUT,
    @Mensaje VARCHAR(500) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @NuevoNumero INT;
    DECLARE @BaseUsuario VARCHAR(45);
    DECLARE @UsuarioFinal VARCHAR(45);
    DECLARE @Contador INT;

    SET @Resultado = 0;
    SET @Mensaje = '';
    SET @NombreUsuarioGenerado = '';
    SET @CodigoEmpleadoGenerado = '';

    SET @Identificacion = LTRIM(RTRIM(ISNULL(@Identificacion, '')));
    SET @Nombre = LTRIM(RTRIM(ISNULL(@Nombre, '')));
    SET @PrimerApellido = LTRIM(RTRIM(ISNULL(@PrimerApellido, '')));
    SET @SegundoApellido = LTRIM(RTRIM(ISNULL(@SegundoApellido, '')));
    SET @Direccion = LTRIM(RTRIM(ISNULL(@Direccion, '')));
    SET @ClaveHash = LTRIM(RTRIM(ISNULL(@ClaveHash, '')));

    IF (@Identificacion = '')
    BEGIN
        SET @Mensaje = 'La identificación es obligatoria.';
        RETURN;
    END;

IF (@Identificacion LIKE '%[^0-9]%' OR LEN(@Identificacion) NOT IN (9, 11))
BEGIN
    SET @Mensaje = 'La identificación debe contener solo números: 9 dígitos para cédula nacional o 11 dígitos para cédula extranjera.';
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
    SET @Mensaje = 'Solo se pueden registrar personas mayores de edad.';
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

    IF (@IdPuesto <= 0)
    BEGIN
        SET @Mensaje = 'Debe seleccionar un puesto.';
        RETURN;
    END;

    IF (@FechaIngreso IS NULL)
    BEGIN
        SET @Mensaje = 'La fecha de ingreso es obligatoria.';
        RETURN;
    END;

    IF (@IdRol <= 0)
    BEGIN
        SET @Mensaje = 'Debe seleccionar un rol.';
        RETURN;
    END;

    IF (@ClaveHash = '')
    BEGIN
        SET @Mensaje = 'No se recibió la contraseña encriptada.';
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

    IF NOT EXISTS (
        SELECT 1
        FROM dbo.Puesto
        WHERE IdPuesto = @IdPuesto
          AND Activo = 1
    )
    BEGIN
        SET @Mensaje = 'El puesto seleccionado no existe o está inactivo.';
        RETURN;
    END;

    IF NOT EXISTS (
        SELECT 1
        FROM dbo.Rol
        WHERE IdRol = @IdRol
          AND Activo = 1
    )
    BEGIN
        SET @Mensaje = 'El rol seleccionado no existe o está inactivo.';
        RETURN;
    END;

    IF EXISTS (
        SELECT 1
        FROM dbo.Empleado
        WHERE Identificacion = @Identificacion
          AND Activo = 1
    )
    BEGIN
        SET @Mensaje = 'Ya existe un empleado activo con esa identificación.';
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

        IF EXISTS (
            SELECT 1
            FROM dbo.Empleado
            WHERE Identificacion = @Identificacion
        )
        BEGIN
            SELECT
                @CodigoEmpleadoGenerado = CodigoEmpleado,
                @NombreUsuarioGenerado = NombreUsuario
            FROM dbo.Empleado
            WHERE Identificacion = @Identificacion;

            UPDATE dbo.Empleado
            SET
                IdPuesto = @IdPuesto,
                FechaIngreso = @FechaIngreso,
                IdRol = @IdRol,
                ClaveHash = @ClaveHash,
                RestablecerClave = 1,
                FechaModificacion = GETDATE(),
                Activo = 1
            WHERE Identificacion = @Identificacion;
        END
        ELSE
        BEGIN
            SELECT @NuevoNumero = ISNULL(MAX(TRY_CAST(REPLACE(CodigoEmpleado, 'EMP-', '') AS INT)), 0) + 1
            FROM dbo.Empleado
            WHERE CodigoEmpleado LIKE 'EMP-%';

            SET @CodigoEmpleadoGenerado = 'EMP-' + RIGHT('000000' + CAST(@NuevoNumero AS VARCHAR(10)), 6);

            SET @BaseUsuario =
                LOWER(
                    REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(
                        LEFT(@Nombre, 1) + @PrimerApellido + LEFT(@SegundoApellido, 1),
                    'á','a'),'é','e'),'í','i'),'ó','o'),'ú','u'),
                    'Á','a'),'É','e'),'Í','i'),'Ó','o'),'Ú','u')
                );

            SET @BaseUsuario = REPLACE(@BaseUsuario, ' ', '');

            IF (@BaseUsuario = '')
            BEGIN
                SET @BaseUsuario = 'usuario';
            END;

            SET @UsuarioFinal = LEFT(@BaseUsuario, 45);
            SET @Contador = 1;

            WHILE EXISTS (
                SELECT 1
                FROM dbo.Empleado
                WHERE NombreUsuario = @UsuarioFinal
            )
            BEGIN
                SET @Contador = @Contador + 1;
                SET @UsuarioFinal = LEFT(@BaseUsuario, 40) + CAST(@Contador AS VARCHAR(5));
            END;

            SET @NombreUsuarioGenerado = @UsuarioFinal;

            INSERT INTO dbo.Empleado
            (
                Identificacion,
                CodigoEmpleado,
                IdPuesto,
                FechaIngreso,
                IdRol,
                NombreUsuario,
                ClaveHash,
                RestablecerClave,
                FechaCreacion,
                FechaModificacion,
                Activo
            )
            VALUES
            (
                @Identificacion,
                @CodigoEmpleadoGenerado,
                @IdPuesto,
                @FechaIngreso,
                @IdRol,
                @NombreUsuarioGenerado,
                @ClaveHash,
                1,
                GETDATE(),
                GETDATE(),
                1
            );
        END;

        COMMIT TRANSACTION;

        SET @Resultado = 1;
        SET @Mensaje = 'Empleado registrado correctamente.';
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;

        SET @Resultado = 0;
        SET @Mensaje = ERROR_MESSAGE();
    END CATCH;
END;
GO

CREATE OR ALTER PROCEDURE Persona.sp_Empleado_Editar
    @Identificacion VARCHAR(45),
    @Nombre VARCHAR(45),
    @PrimerApellido VARCHAR(45),
    @SegundoApellido VARCHAR(45),
    @FechaNacimiento DATE,
    @CodigoDistrito INT,
    @Direccion VARCHAR(250),

    @IdPuesto INT,
    @FechaIngreso DATE,
    @IdRol INT,
    @Activo BIT,

    @Resultado BIT OUTPUT,
    @Mensaje VARCHAR(500) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    SET @Resultado = 0;
    SET @Mensaje = '';

    SET @Identificacion = LTRIM(RTRIM(ISNULL(@Identificacion, '')));
    SET @Nombre = LTRIM(RTRIM(ISNULL(@Nombre, '')));
    SET @PrimerApellido = LTRIM(RTRIM(ISNULL(@PrimerApellido, '')));
    SET @SegundoApellido = LTRIM(RTRIM(ISNULL(@SegundoApellido, '')));
    SET @Direccion = LTRIM(RTRIM(ISNULL(@Direccion, '')));

    IF (@Identificacion = '')
    BEGIN
        SET @Mensaje = 'Debe seleccionar un empleado válido.';
        RETURN;
    END;

    IF (@Identificacion LIKE '%[^0-9]%' OR LEN(@Identificacion) < 8 OR LEN(@Identificacion) > 11)
    BEGIN
        SET @Mensaje = 'La identificación debe contener solo números y tener entre 8 y 11 dígitos.';
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

    IF (@IdPuesto <= 0)
    BEGIN
        SET @Mensaje = 'Debe seleccionar un puesto.';
        RETURN;
    END;

    IF (@FechaIngreso IS NULL)
    BEGIN
        SET @Mensaje = 'La fecha de ingreso es obligatoria.';
        RETURN;
    END;

    IF (@IdRol <= 0)
    BEGIN
        SET @Mensaje = 'Debe seleccionar un rol.';
        RETURN;
    END;

    IF NOT EXISTS (
        SELECT 1
        FROM dbo.Empleado
        WHERE Identificacion = @Identificacion
    )
    BEGIN
        SET @Mensaje = 'El empleado no existe.';
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

    IF NOT EXISTS (
        SELECT 1
        FROM dbo.Puesto
        WHERE IdPuesto = @IdPuesto
          AND Activo = 1
    )
    BEGIN
        SET @Mensaje = 'El puesto seleccionado no existe o está inactivo.';
        RETURN;
    END;

    IF NOT EXISTS (
        SELECT 1
        FROM dbo.Rol
        WHERE IdRol = @IdRol
          AND Activo = 1
    )
    BEGIN
        SET @Mensaje = 'El rol seleccionado no existe o está inactivo.';
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

        UPDATE dbo.Empleado
        SET
            IdPuesto = @IdPuesto,
            FechaIngreso = @FechaIngreso,
            IdRol = @IdRol,
            FechaModificacion = GETDATE(),
            Activo = @Activo
        WHERE Identificacion = @Identificacion;

        COMMIT TRANSACTION;

        SET @Resultado = 1;
        SET @Mensaje = 'Empleado actualizado correctamente.';
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;

        SET @Resultado = 0;
        SET @Mensaje = ERROR_MESSAGE();
    END CATCH;
END;
GO

CREATE OR ALTER PROCEDURE Persona.sp_Empleado_Inactivar
    @Identificacion VARCHAR(45),
    @Resultado BIT OUTPUT,
    @Mensaje VARCHAR(500) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    SET @Resultado = 0;
    SET @Mensaje = '';

    SET @Identificacion = LTRIM(RTRIM(ISNULL(@Identificacion, '')));

    IF (@Identificacion = '')
    BEGIN
        SET @Mensaje = 'Debe seleccionar un empleado válido.';
        RETURN;
    END;

    IF NOT EXISTS (
        SELECT 1
        FROM dbo.Empleado
        WHERE Identificacion = @Identificacion
    )
    BEGIN
        SET @Mensaje = 'El empleado no existe.';
        RETURN;
    END;

    UPDATE dbo.Empleado
    SET
        Activo = 0,
        FechaModificacion = GETDATE()
    WHERE Identificacion = @Identificacion;

    SET @Resultado = 1;
    SET @Mensaje = 'Empleado inactivado correctamente.';
END;
GO

CREATE OR ALTER PROCEDURE Persona.sp_Empleado_RestablecerClave
    @Identificacion VARCHAR(45),
    @ClaveHash VARCHAR(255),
    @Resultado BIT OUTPUT,
    @Mensaje VARCHAR(500) OUTPUT,
    @NombreUsuario VARCHAR(45) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    SET @Resultado = 0;
    SET @Mensaje = '';
    SET @NombreUsuario = '';

    SET @Identificacion = LTRIM(RTRIM(ISNULL(@Identificacion, '')));
    SET @ClaveHash = LTRIM(RTRIM(ISNULL(@ClaveHash, '')));

    IF (@Identificacion = '')
    BEGIN
        SET @Mensaje = 'Debe seleccionar un empleado válido.';
        RETURN;
    END;

    IF (@ClaveHash = '')
    BEGIN
        SET @Mensaje = 'No se recibió la contraseña encriptada.';
        RETURN;
    END;

    IF NOT EXISTS (
        SELECT 1
        FROM dbo.Empleado
        WHERE Identificacion = @Identificacion
          AND Activo = 1
    )
    BEGIN
        SET @Mensaje = 'El empleado no existe o está inactivo.';
        RETURN;
    END;

    UPDATE dbo.Empleado
    SET
        ClaveHash = @ClaveHash,
        RestablecerClave = 1,
        FechaModificacion = GETDATE()
    WHERE Identificacion = @Identificacion;

    SELECT @NombreUsuario = NombreUsuario
    FROM dbo.Empleado
    WHERE Identificacion = @Identificacion;

    SET @Resultado = 1;
    SET @Mensaje = 'Contraseña restablecida correctamente.';
END;
GO