/* =========================================================
   SEGURIDAD_SP.sql
   Seguridad, permisos por rol, acceso, historial y cambio de clave.
   Script limpio sin duplicados ni arreglos posteriores.
   ========================================================= */

IF NOT EXISTS (SELECT 1 FROM sys.schemas WHERE name = 'Seguridad')
BEGIN
    EXEC('CREATE SCHEMA Seguridad');
END;
GO

IF NOT EXISTS (SELECT 1 FROM sys.schemas WHERE name = 'Persona')
BEGIN
    EXEC('CREATE SCHEMA Persona');
END;
GO

/* =========================================================
   TABLA: ModuloSistema
   Catálogo de pantallas o módulos permitibles.
   ========================================================= */
IF NOT EXISTS (
    SELECT 1
    FROM sys.tables
    WHERE name = 'ModuloSistema'
      AND schema_id = SCHEMA_ID('dbo')
)
BEGIN
    CREATE TABLE dbo.ModuloSistema
    (
        CodigoModulo VARCHAR(80) NOT NULL,
        NombreModulo VARCHAR(100) NOT NULL,
        AreaSistema VARCHAR(80) NOT NULL,
        Controlador VARCHAR(100) NOT NULL,
        Accion VARCHAR(100) NOT NULL,
        Url VARCHAR(200) NOT NULL,
        Icono VARCHAR(80) NOT NULL,
        Orden INT NOT NULL,
        FechaCreacion DATETIME NOT NULL CONSTRAINT DF_ModuloSistema_FechaCreacion DEFAULT (GETDATE()),
        FechaModificacion DATETIME NOT NULL CONSTRAINT DF_ModuloSistema_FechaModificacion DEFAULT (GETDATE()),
        Activo BIT NOT NULL CONSTRAINT DF_ModuloSistema_Activo DEFAULT (1),

        CONSTRAINT PK_ModuloSistema PRIMARY KEY (CodigoModulo)
    );
END;
GO

/* =========================================================
   TABLA: PermisoRolModulo
   Permisos por rol y módulo.
   Todo se inactiva con Activo = 0.
   ========================================================= */
IF NOT EXISTS (
    SELECT 1
    FROM sys.tables
    WHERE name = 'PermisoRolModulo'
      AND schema_id = SCHEMA_ID('dbo')
)
BEGIN
    CREATE TABLE dbo.PermisoRolModulo
    (
        IdRol INT NOT NULL,
        CodigoModulo VARCHAR(80) NOT NULL,

        PuedeVer BIT NOT NULL CONSTRAINT DF_PermisoRolModulo_PuedeVer DEFAULT (0),
        PuedeCrear BIT NOT NULL CONSTRAINT DF_PermisoRolModulo_PuedeCrear DEFAULT (0),
        PuedeEditar BIT NOT NULL CONSTRAINT DF_PermisoRolModulo_PuedeEditar DEFAULT (0),
        PuedeEliminar BIT NOT NULL CONSTRAINT DF_PermisoRolModulo_PuedeEliminar DEFAULT (0),

        FechaCreacion DATETIME NOT NULL CONSTRAINT DF_PermisoRolModulo_FechaCreacion DEFAULT (GETDATE()),
        FechaModificacion DATETIME NOT NULL CONSTRAINT DF_PermisoRolModulo_FechaModificacion DEFAULT (GETDATE()),
        Activo BIT NOT NULL CONSTRAINT DF_PermisoRolModulo_Activo DEFAULT (1),

        CONSTRAINT PK_PermisoRolModulo PRIMARY KEY (IdRol, CodigoModulo),

        CONSTRAINT FK_PermisoRolModulo_Rol FOREIGN KEY (IdRol)
            REFERENCES dbo.Rol(IdRol),

        CONSTRAINT FK_PermisoRolModulo_ModuloSistema FOREIGN KEY (CodigoModulo)
            REFERENCES dbo.ModuloSistema(CodigoModulo)
    );
END;
GO

/* =========================================================
   TIPOS DE OBSERVACIÓN BASE
   Usa la tabla real TipoDeObservacion.
   ========================================================= */
IF NOT EXISTS (
    SELECT 1
    FROM dbo.TipoDeObservacion
    WHERE TipoDeObservacion = 'Seguridad'
)
BEGIN
    INSERT INTO dbo.TipoDeObservacion
    (
        TipoDeObservacion,
        Descripcion,
        Activo
    )
    VALUES
    (
        'Seguridad',
        'Cambios realizados en roles, permisos, usuarios y accesos del sistema.',
        1
    );
END;
GO

IF NOT EXISTS (
    SELECT 1
    FROM dbo.TipoDeObservacion
    WHERE TipoDeObservacion = 'Acceso'
)
BEGIN
    INSERT INTO dbo.TipoDeObservacion
    (
        TipoDeObservacion,
        Descripcion,
        Activo
    )
    VALUES
    (
        'Acceso',
        'Inicio y cierre de sesión de usuarios.',
        1
    );
END;
GO

IF NOT EXISTS (
    SELECT 1
    FROM dbo.TipoDeObservacion
    WHERE TipoDeObservacion = 'Cambio de clave'
)
BEGIN
    INSERT INTO dbo.TipoDeObservacion
    (
        TipoDeObservacion,
        Descripcion,
        Activo
    )
    VALUES
    (
        'Cambio de clave',
        'Cambios de contraseña realizados por usuarios o administradores.',
        1
    );
END;
GO

/* =========================================================
   CATÁLOGO DE MÓDULOS DEL SISTEMA
   Ya viene con los nombres reales de tus controllers.
   ========================================================= */
MERGE dbo.ModuloSistema AS destino
USING
(
    SELECT 'HOME' AS CodigoModulo, 'Inicio' AS NombreModulo, 'General' AS AreaSistema, 'Home' AS Controlador, 'Index' AS Accion, '/Home/Index' AS Url, 'feather icon-home' AS Icono, 1 AS Orden

    UNION ALL SELECT 'FACTURAS', 'Facturación', 'Operación diaria', 'Facturacion', 'Facturas', '/Facturacion/Facturas', 'feather icon-shopping-cart', 10
    UNION ALL SELECT 'NOTAS_CREDITO', 'Notas de crédito', 'Operación diaria', 'Facturacion', 'NotasDeCredito', '/Facturacion/NotasDeCredito', 'feather icon-file-minus', 20
    UNION ALL SELECT 'INGRESOS', 'Ingresos', 'Operación diaria', 'GestionarIngresos', 'Ingresos', '/GestionarIngresos/Ingresos', 'feather icon-arrow-down-circle', 30
    UNION ALL SELECT 'GASTOS', 'Gastos', 'Operación diaria', 'GestionarGastos', 'Gastos', '/GestionarGastos/Gastos', 'feather icon-arrow-up-circle', 40

    UNION ALL SELECT 'CUENTAS_COBRAR', 'Cuentas por cobrar', 'Cobros y pagos', 'GestionarCuentasPorCobrar', 'CuentasPorCobrar', '/GestionarCuentasPorCobrar/CuentasPorCobrar', 'feather icon-credit-card', 50
    UNION ALL SELECT 'CUENTAS_PAGAR', 'Cuentas por pagar', 'Cobros y pagos', 'GestionarCuentasPorPagar', 'CuentasPorPagar', '/GestionarCuentasPorPagar/CuentasPorPagar', 'feather icon-file-text', 60

    UNION ALL SELECT 'ASIENTOS', 'Asientos contables', 'Control contable', 'GestionarAsientoContable', 'AsientosContables', '/GestionarAsientoContable/AsientosContables', 'feather icon-book-open', 70
    UNION ALL SELECT 'CUENTAS_CONTABLES', 'Cuentas contables', 'Control contable', 'GestionarCuentaContable', 'CuentasContables', '/GestionarCuentaContable/CuentasContables', 'feather icon-book', 80
    UNION ALL SELECT 'CONFIG_CUENTA_CONTABLE', 'Configuración cuenta contable', 'Control contable', 'ConfiguracionCuentaContable', 'ConfiguracionCuentaContable', '/ConfiguracionCuentaContable/ConfiguracionCuentaContable', 'feather icon-settings', 85
    UNION ALL SELECT 'PRESUPUESTOS', 'Presupuesto mensual', 'Control contable', 'GestionarPresupuestoMensual', 'PresupuestoMensual', '/GestionarPresupuestoMensual/PresupuestoMensual', 'feather icon-pie-chart', 90
    UNION ALL SELECT 'PERIODO_CONTABLE', 'Periodo contable', 'Control contable', 'GestionarPeriodoContable', 'PeriodoContable', '/GestionarPeriodoContable/PeriodoContable', 'feather icon-calendar', 95

    UNION ALL SELECT 'CATALOGOS', 'Catálogos', 'Mantenimientos', 'Catalogos', 'Index', '/Catalogos/Index', 'feather icon-list', 100
    UNION ALL SELECT 'PRODUCTOS', 'Productos', 'Mantenimientos', 'Producto', 'Productos', '/Producto/Productos', 'feather icon-package', 110
    UNION ALL SELECT 'CLIENTES', 'Clientes', 'Mantenimientos', 'Persona', 'Clientes', '/Persona/Clientes', 'feather icon-users', 120
    UNION ALL SELECT 'EMPLEADOS', 'Empleados', 'Mantenimientos', 'Persona', 'Empleados', '/Persona/Empleados', 'feather icon-user-check', 130
    UNION ALL SELECT 'PROVEEDORES', 'Proveedores', 'Mantenimientos', 'Proveedor', 'Proveedores', '/Proveedor/Proveedores', 'feather icon-truck', 140
    UNION ALL SELECT 'UBICACIONES', 'Ubicaciones', 'Mantenimientos', 'Ubicacion', 'Index', '/Ubicacion/Index', 'feather icon-map-pin', 150

    UNION ALL SELECT 'REPORTES', 'Reportes', 'Información', 'Reportes', 'Reportes', '/Reportes/Reportes', 'feather icon-bar-chart-2', 160
    UNION ALL SELECT 'CONSULTAS', 'Consultas generales', 'Información', 'Consultas', 'Consultas', '/Consultas/Consultas', 'feather icon-search', 170

    UNION ALL SELECT 'ROLES', 'Roles y permisos', 'Seguridad', 'Seguridad', 'Rol', '/Seguridad/Rol', 'feather icon-shield', 180
    UNION ALL SELECT 'CAMBIAR_CLAVE', 'Cambiar clave', 'Seguridad', 'Seguridad', 'CambiarClave', '/Seguridad/CambiarClave', 'feather icon-lock', 190
    UNION ALL SELECT 'HISTORIAL_CAMBIOS', 'Historial de cambios', 'Seguridad', 'Seguridad', 'HistorialCambios', '/Seguridad/HistorialCambios', 'feather icon-clock', 200
) AS origen
ON destino.CodigoModulo = origen.CodigoModulo
WHEN MATCHED THEN
    UPDATE SET
        NombreModulo = origen.NombreModulo,
        AreaSistema = origen.AreaSistema,
        Controlador = origen.Controlador,
        Accion = origen.Accion,
        Url = origen.Url,
        Icono = origen.Icono,
        Orden = origen.Orden,
        FechaModificacion = GETDATE(),
        Activo = 1
WHEN NOT MATCHED THEN
    INSERT
    (
        CodigoModulo,
        NombreModulo,
        AreaSistema,
        Controlador,
        Accion,
        Url,
        Icono,
        Orden,
        FechaCreacion,
        FechaModificacion,
        Activo
    )
    VALUES
    (
        origen.CodigoModulo,
        origen.NombreModulo,
        origen.AreaSistema,
        origen.Controlador,
        origen.Accion,
        origen.Url,
        origen.Icono,
        origen.Orden,
        GETDATE(),
        GETDATE(),
        1
    );
GO

/* =========================================================
   HISTORIAL - REGISTRAR CAMBIO
   Respeta la tabla real HistorialCambio:
   Identificacion, NombreTabla, IdTipoDeObservacion,
   FechaHoraCambio, Detalle, ValorAnterior, ValorNuevo,
   IdRegistro, Activo.
   ========================================================= */
CREATE OR ALTER PROCEDURE Seguridad.sp_HistorialCambio_Registrar
    @Identificacion VARCHAR(45),
    @NombreTabla VARCHAR(100),
    @TipoDeObservacion VARCHAR(45),
    @Detalle VARCHAR(300),
    @ValorAnterior VARCHAR(900),
    @ValorNuevo VARCHAR(900),
    @IdRegistro VARCHAR(150),
    @Resultado BIT OUTPUT,
    @Mensaje VARCHAR(500) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @IdTipoDeObservacion INT;

    SET @Resultado = 0;
    SET @Mensaje = '';

    SET @Identificacion = LTRIM(RTRIM(ISNULL(@Identificacion, '')));
    SET @NombreTabla = LTRIM(RTRIM(ISNULL(@NombreTabla, '')));
    SET @TipoDeObservacion = LTRIM(RTRIM(ISNULL(@TipoDeObservacion, '')));
    SET @Detalle = LTRIM(RTRIM(ISNULL(@Detalle, '')));
    SET @ValorAnterior = LTRIM(RTRIM(ISNULL(@ValorAnterior, '')));
    SET @ValorNuevo = LTRIM(RTRIM(ISNULL(@ValorNuevo, '')));
    SET @IdRegistro = LTRIM(RTRIM(ISNULL(@IdRegistro, '')));

    IF (@Identificacion = '')
    BEGIN
        SET @Identificacion = '000000000';
    END;

    IF NOT EXISTS (
        SELECT 1
        FROM dbo.Empleado
        WHERE Identificacion = @Identificacion
    )
    BEGIN
        SET @Identificacion = '000000000';
    END;

    IF (@NombreTabla = '')
    BEGIN
        SET @Mensaje = 'Debe indicar el nombre de la tabla.';
        RETURN;
    END;

    IF (@TipoDeObservacion = '')
    BEGIN
        SET @TipoDeObservacion = 'Seguridad';
    END;

    SELECT TOP 1
        @IdTipoDeObservacion = IdTipoDeObservacion
    FROM dbo.TipoDeObservacion
    WHERE TipoDeObservacion = @TipoDeObservacion
      AND Activo = 1;

    IF (@IdTipoDeObservacion IS NULL)
    BEGIN
        SELECT TOP 1
            @IdTipoDeObservacion = IdTipoDeObservacion
        FROM dbo.TipoDeObservacion
        WHERE TipoDeObservacion = 'Seguridad'
          AND Activo = 1;
    END;

    IF (@IdTipoDeObservacion IS NULL)
    BEGIN
        SET @Mensaje = 'No existe un tipo de observación válido.';
        RETURN;
    END;

    IF (@Detalle = '')
    BEGIN
        SET @Detalle = 'Cambio realizado en el sistema.';
    END;

    IF (@IdRegistro = '')
    BEGIN
        SET @IdRegistro = 'N/A';
    END;

    BEGIN TRY
        INSERT INTO dbo.HistorialCambio
        (
            Identificacion,
            NombreTabla,
            IdTipoDeObservacion,
            FechaHoraCambio,
            Detalle,
            ValorAnterior,
            ValorNuevo,
            IdRegistro,
            Activo
        )
        VALUES
        (
            @Identificacion,
            @NombreTabla,
            @IdTipoDeObservacion,
            GETDATE(),
            @Detalle,
            @ValorAnterior,
            @ValorNuevo,
            @IdRegistro,
            1
        );

        SET @Resultado = 1;
        SET @Mensaje = 'Historial registrado correctamente.';
    END TRY
    BEGIN CATCH
        SET @Resultado = 0;
        SET @Mensaje = ERROR_MESSAGE();
    END CATCH;
END;
GO

/* =========================================================
   HISTORIAL - LISTAR CAMBIOS
   Para la vista HistorialCambios.
   ========================================================= */
CREATE OR ALTER PROCEDURE Seguridad.sp_HistorialCambio_Listar
    @Filtro VARCHAR(100) = '',
    @FechaInicio DATETIME = NULL,
    @FechaFin DATETIME = NULL,
    @TipoDeObservacion VARCHAR(45) = ''
AS
BEGIN
    SET NOCOUNT ON;

    SET @Filtro = LTRIM(RTRIM(ISNULL(@Filtro, '')));
    SET @TipoDeObservacion = LTRIM(RTRIM(ISNULL(@TipoDeObservacion, '')));

    SELECT
        h.IdHistorialCambio,
        h.Identificacion,
        ISNULL(e.NombreUsuario, '') AS NombreUsuario,

        LTRIM(RTRIM(
            ISNULL(p.Nombre, '') + ' ' +
            ISNULL(p.PrimerApellido, '') + ' ' +
            ISNULL(p.SegundoApellido, '')
        )) AS NombreEmpleado,

        h.NombreTabla,
        h.IdTipoDeObservacion,
        t.TipoDeObservacion,
        h.FechaHoraCambio,
        h.Detalle,
        h.ValorAnterior,
        h.ValorNuevo,
        h.IdRegistro,
        h.Activo
    FROM dbo.HistorialCambio h
    INNER JOIN dbo.TipoDeObservacion t
        ON t.IdTipoDeObservacion = h.IdTipoDeObservacion
    LEFT JOIN dbo.Empleado e
        ON e.Identificacion = h.Identificacion
    LEFT JOIN dbo.Persona p
        ON p.Identificacion = h.Identificacion
    WHERE h.Activo = 1
      AND (
            @Filtro = ''
            OR h.Identificacion LIKE '%' + @Filtro + '%'
            OR ISNULL(e.NombreUsuario, '') LIKE '%' + @Filtro + '%'
            OR ISNULL(p.Nombre, '') LIKE '%' + @Filtro + '%'
            OR ISNULL(p.PrimerApellido, '') LIKE '%' + @Filtro + '%'
            OR ISNULL(p.SegundoApellido, '') LIKE '%' + @Filtro + '%'
            OR h.NombreTabla LIKE '%' + @Filtro + '%'
            OR h.Detalle LIKE '%' + @Filtro + '%'
            OR h.IdRegistro LIKE '%' + @Filtro + '%'
          )
      AND (@TipoDeObservacion = '' OR t.TipoDeObservacion = @TipoDeObservacion)
      AND (@FechaInicio IS NULL OR CAST(h.FechaHoraCambio AS DATE) >= CAST(@FechaInicio AS DATE))
      AND (@FechaFin IS NULL OR CAST(h.FechaHoraCambio AS DATE) <= CAST(@FechaFin AS DATE))
    ORDER BY
        h.FechaHoraCambio DESC;
END;
GO

/* =========================================================
   MÓDULOS - LISTAR ACTIVOS
   ========================================================= */
CREATE OR ALTER PROCEDURE Seguridad.sp_ModuloSistema_Listar
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        CodigoModulo,
        NombreModulo,
        AreaSistema,
        Controlador,
        Accion,
        Url,
        Icono,
        Orden,
        Activo
    FROM dbo.ModuloSistema
    WHERE Activo = 1
    ORDER BY
        AreaSistema,
        Orden,
        NombreModulo;
END;
GO

/* =========================================================
   MÓDULOS - INACTIVAR
   Borrado lógico.
   ========================================================= */
CREATE OR ALTER PROCEDURE Seguridad.sp_ModuloSistema_Inactivar
    @CodigoModulo VARCHAR(80),
    @IdentificacionEmpleado VARCHAR(45),
    @Resultado BIT OUTPUT,
    @Mensaje VARCHAR(500) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @ResultadoHistorial BIT;
    DECLARE @MensajeHistorial VARCHAR(500);

    SET @Resultado = 0;
    SET @Mensaje = '';

    SET @CodigoModulo = LTRIM(RTRIM(ISNULL(@CodigoModulo, '')));
    SET @IdentificacionEmpleado = LTRIM(RTRIM(ISNULL(@IdentificacionEmpleado, '')));

    IF NOT EXISTS (
        SELECT 1
        FROM dbo.ModuloSistema
        WHERE CodigoModulo = @CodigoModulo
          AND Activo = 1
    )
    BEGIN
        SET @Mensaje = 'El módulo no existe o ya está inactivo.';
        RETURN;
    END;

    BEGIN TRY
        UPDATE dbo.ModuloSistema
        SET
            Activo = 0,
            FechaModificacion = GETDATE()
        WHERE CodigoModulo = @CodigoModulo;

        UPDATE dbo.PermisoRolModulo
        SET
            Activo = 0,
            PuedeVer = 0,
            PuedeCrear = 0,
            PuedeEditar = 0,
            PuedeEliminar = 0,
            FechaModificacion = GETDATE()
        WHERE CodigoModulo = @CodigoModulo
          AND Activo = 1;

        EXEC Seguridad.sp_HistorialCambio_Registrar
            @Identificacion = @IdentificacionEmpleado,
            @NombreTabla = 'ModuloSistema',
            @TipoDeObservacion = 'Seguridad',
            @Detalle = 'Se inactivó un módulo del sistema.',
            @ValorAnterior = 'Activo=1',
            @ValorNuevo = 'Activo=0',
            @IdRegistro = @CodigoModulo,
            @Resultado = @ResultadoHistorial OUTPUT,
            @Mensaje = @MensajeHistorial OUTPUT;

        SET @Resultado = 1;
        SET @Mensaje = 'Módulo inactivado correctamente.';
    END TRY
    BEGIN CATCH
        SET @Resultado = 0;
        SET @Mensaje = ERROR_MESSAGE();
    END CATCH;
END;
GO

/* =========================================================
   PERMISOS - LISTAR POR ROL
   Muestra todos los módulos activos aunque no tengan permiso.
   ========================================================= */
CREATE OR ALTER PROCEDURE Seguridad.sp_PermisoRolModulo_ListarPorRol
    @IdRol INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        m.CodigoModulo,
        m.NombreModulo,
        m.AreaSistema,
        m.Controlador,
        m.Accion,
        m.Url,
        m.Icono,
        m.Orden,

        @IdRol AS IdRol,
        ISNULL(p.PuedeVer, 0) AS PuedeVer,
        ISNULL(p.PuedeCrear, 0) AS PuedeCrear,
        ISNULL(p.PuedeEditar, 0) AS PuedeEditar,
        ISNULL(p.PuedeEliminar, 0) AS PuedeEliminar,

        ISNULL(p.Activo, 1) AS Activo
    FROM dbo.ModuloSistema m
    LEFT JOIN dbo.PermisoRolModulo p
        ON p.CodigoModulo = m.CodigoModulo
       AND p.IdRol = @IdRol
       AND p.Activo = 1
    WHERE m.Activo = 1
    ORDER BY
        m.AreaSistema,
        m.Orden,
        m.NombreModulo;
END;
GO

/* =========================================================
   PERMISOS - GUARDAR
   Si PuedeVer = 0, apaga crear/editar/eliminar.
   No elimina físicamente.
   ========================================================= */
CREATE OR ALTER PROCEDURE Seguridad.sp_PermisoRolModulo_Guardar
    @IdRol INT,
    @CodigoModulo VARCHAR(80),
    @PuedeVer BIT,
    @PuedeCrear BIT,
    @PuedeEditar BIT,
    @PuedeEliminar BIT,
    @IdentificacionEmpleado VARCHAR(45),
    @Resultado BIT OUTPUT,
    @Mensaje VARCHAR(500) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @ValorAnterior VARCHAR(900);
    DECLARE @ValorNuevo VARCHAR(900);
    DECLARE @ResultadoHistorial BIT;
    DECLARE @MensajeHistorial VARCHAR(500);

    SET @Resultado = 0;
    SET @Mensaje = '';

    SET @CodigoModulo = LTRIM(RTRIM(ISNULL(@CodigoModulo, '')));
    SET @IdentificacionEmpleado = LTRIM(RTRIM(ISNULL(@IdentificacionEmpleado, '')));

    IF (@PuedeVer = 0)
    BEGIN
        SET @PuedeCrear = 0;
        SET @PuedeEditar = 0;
        SET @PuedeEliminar = 0;
    END;

    IF (@IdRol <= 0)
    BEGIN
        SET @Mensaje = 'Debe seleccionar un rol válido.';
        RETURN;
    END;

    IF NOT EXISTS (
        SELECT 1
        FROM dbo.Rol
        WHERE IdRol = @IdRol
          AND Activo = 1
    )
    BEGIN
        SET @Mensaje = 'El rol no existe o está inactivo.';
        RETURN;
    END;

    IF NOT EXISTS (
        SELECT 1
        FROM dbo.ModuloSistema
        WHERE CodigoModulo = @CodigoModulo
          AND Activo = 1
    )
    BEGIN
        SET @Mensaje = 'El módulo no existe o está inactivo.';
        RETURN;
    END;

    SELECT
        @ValorAnterior =
            'IdRol=' + CAST(IdRol AS VARCHAR(20)) +
            ', Modulo=' + CodigoModulo +
            ', Ver=' + CAST(PuedeVer AS VARCHAR(1)) +
            ', Crear=' + CAST(PuedeCrear AS VARCHAR(1)) +
            ', Editar=' + CAST(PuedeEditar AS VARCHAR(1)) +
            ', Eliminar=' + CAST(PuedeEliminar AS VARCHAR(1)) +
            ', Activo=' + CAST(Activo AS VARCHAR(1))
    FROM dbo.PermisoRolModulo
    WHERE IdRol = @IdRol
      AND CodigoModulo = @CodigoModulo;

    IF (@ValorAnterior IS NULL)
    BEGIN
        SET @ValorAnterior = 'Sin permiso anterior';
    END;

    SET @ValorNuevo =
        'IdRol=' + CAST(@IdRol AS VARCHAR(20)) +
        ', Modulo=' + @CodigoModulo +
        ', Ver=' + CAST(@PuedeVer AS VARCHAR(1)) +
        ', Crear=' + CAST(@PuedeCrear AS VARCHAR(1)) +
        ', Editar=' + CAST(@PuedeEditar AS VARCHAR(1)) +
        ', Eliminar=' + CAST(@PuedeEliminar AS VARCHAR(1)) +
        ', Activo=1';

    BEGIN TRY
        IF EXISTS (
            SELECT 1
            FROM dbo.PermisoRolModulo
            WHERE IdRol = @IdRol
              AND CodigoModulo = @CodigoModulo
        )
        BEGIN
            UPDATE dbo.PermisoRolModulo
            SET
                PuedeVer = @PuedeVer,
                PuedeCrear = @PuedeCrear,
                PuedeEditar = @PuedeEditar,
                PuedeEliminar = @PuedeEliminar,
                FechaModificacion = GETDATE(),
                Activo = 1
            WHERE IdRol = @IdRol
              AND CodigoModulo = @CodigoModulo;
        END
        ELSE
        BEGIN
            INSERT INTO dbo.PermisoRolModulo
            (
                IdRol,
                CodigoModulo,
                PuedeVer,
                PuedeCrear,
                PuedeEditar,
                PuedeEliminar,
                FechaCreacion,
                FechaModificacion,
                Activo
            )
            VALUES
            (
                @IdRol,
                @CodigoModulo,
                @PuedeVer,
                @PuedeCrear,
                @PuedeEditar,
                @PuedeEliminar,
                GETDATE(),
                GETDATE(),
                1
            );
        END;

        EXEC Seguridad.sp_HistorialCambio_Registrar
            @Identificacion = @IdentificacionEmpleado,
            @NombreTabla = 'PermisoRolModulo',
            @TipoDeObservacion = 'Seguridad',
            @Detalle = 'Se actualizaron permisos de rol.',
            @ValorAnterior = @ValorAnterior,
            @ValorNuevo = @ValorNuevo,
            @IdRegistro = @CodigoModulo,
            @Resultado = @ResultadoHistorial OUTPUT,
            @Mensaje = @MensajeHistorial OUTPUT;

        SET @Resultado = 1;
        SET @Mensaje = 'Permiso guardado correctamente.';
    END TRY
    BEGIN CATCH
        SET @Resultado = 0;
        SET @Mensaje = ERROR_MESSAGE();
    END CATCH;
END;
GO

/* =========================================================
   PERMISOS - INACTIVAR
   Borrado lógico.
   ========================================================= */
CREATE OR ALTER PROCEDURE Seguridad.sp_PermisoRolModulo_Inactivar
    @IdRol INT,
    @CodigoModulo VARCHAR(80),
    @IdentificacionEmpleado VARCHAR(45),
    @Resultado BIT OUTPUT,
    @Mensaje VARCHAR(500) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @ValorAnterior VARCHAR(900);
    DECLARE @ResultadoHistorial BIT;
    DECLARE @MensajeHistorial VARCHAR(500);

    SET @Resultado = 0;
    SET @Mensaje = '';

    SET @CodigoModulo = LTRIM(RTRIM(ISNULL(@CodigoModulo, '')));
    SET @IdentificacionEmpleado = LTRIM(RTRIM(ISNULL(@IdentificacionEmpleado, '')));

    IF NOT EXISTS (
        SELECT 1
        FROM dbo.PermisoRolModulo
        WHERE IdRol = @IdRol
          AND CodigoModulo = @CodigoModulo
          AND Activo = 1
    )
    BEGIN
        SET @Mensaje = 'El permiso no existe o ya está inactivo.';
        RETURN;
    END;

    SELECT
        @ValorAnterior =
            'IdRol=' + CAST(IdRol AS VARCHAR(20)) +
            ', Modulo=' + CodigoModulo +
            ', Ver=' + CAST(PuedeVer AS VARCHAR(1)) +
            ', Crear=' + CAST(PuedeCrear AS VARCHAR(1)) +
            ', Editar=' + CAST(PuedeEditar AS VARCHAR(1)) +
            ', Eliminar=' + CAST(PuedeEliminar AS VARCHAR(1)) +
            ', Activo=' + CAST(Activo AS VARCHAR(1))
    FROM dbo.PermisoRolModulo
    WHERE IdRol = @IdRol
      AND CodigoModulo = @CodigoModulo;

    BEGIN TRY
        UPDATE dbo.PermisoRolModulo
        SET
            Activo = 0,
            PuedeVer = 0,
            PuedeCrear = 0,
            PuedeEditar = 0,
            PuedeEliminar = 0,
            FechaModificacion = GETDATE()
        WHERE IdRol = @IdRol
          AND CodigoModulo = @CodigoModulo;

        EXEC Seguridad.sp_HistorialCambio_Registrar
            @Identificacion = @IdentificacionEmpleado,
            @NombreTabla = 'PermisoRolModulo',
            @TipoDeObservacion = 'Seguridad',
            @Detalle = 'Se inactivó un permiso de rol.',
            @ValorAnterior = @ValorAnterior,
            @ValorNuevo = 'Activo=0',
            @IdRegistro = @CodigoModulo,
            @Resultado = @ResultadoHistorial OUTPUT,
            @Mensaje = @MensajeHistorial OUTPUT;

        SET @Resultado = 1;
        SET @Mensaje = 'Permiso inactivado correctamente.';
    END TRY
    BEGIN CATCH
        SET @Resultado = 0;
        SET @Mensaje = ERROR_MESSAGE();
    END CATCH;
END;
GO

/* =========================================================
   PERMISOS - OBTENER VISIBLES POR ROL
   Para cargar menú y sesión.
   ========================================================= */
CREATE OR ALTER PROCEDURE Seguridad.sp_PermisoRolModulo_ObtenerPorRol
    @IdRol INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        p.IdRol,
        p.CodigoModulo,
        m.NombreModulo,
        m.AreaSistema,
        m.Controlador,
        m.Accion,
        m.Url,
        m.Icono,
        m.Orden,

        p.PuedeVer,
        p.PuedeCrear,
        p.PuedeEditar,
        p.PuedeEliminar,
        p.Activo
    FROM dbo.PermisoRolModulo p
    INNER JOIN dbo.ModuloSistema m
        ON m.CodigoModulo = p.CodigoModulo
    WHERE p.IdRol = @IdRol
      AND p.Activo = 1
      AND m.Activo = 1
      AND p.PuedeVer = 1
    ORDER BY
        m.AreaSistema,
        m.Orden,
        m.NombreModulo;
END;
GO

/* =========================================================
   PERMISOS - VALIDAR ACCESO POR CONTROLADOR Y ACCIÓN
   Para [PermisoAuthorize].
   ========================================================= */
CREATE OR ALTER PROCEDURE Seguridad.sp_PermisoRolModulo_ValidarAcceso
    @IdRol INT,
    @Controlador VARCHAR(100),
    @Accion VARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;

    SET @Controlador = LTRIM(RTRIM(ISNULL(@Controlador, '')));
    SET @Accion = LTRIM(RTRIM(ISNULL(@Accion, '')));

    SELECT
        CASE
            WHEN EXISTS (
                SELECT 1
                FROM dbo.PermisoRolModulo p
                INNER JOIN dbo.ModuloSistema m
                    ON m.CodigoModulo = p.CodigoModulo
                WHERE p.IdRol = @IdRol
                  AND p.Activo = 1
                  AND p.PuedeVer = 1
                  AND m.Activo = 1
                  AND m.Controlador = @Controlador
                  AND
                  (
                        m.Accion = @Accion
                     OR @Accion LIKE 'Listar%'
                     OR @Accion LIKE 'Obtener%'
                     OR @Accion LIKE 'Buscar%'
                     OR @Accion LIKE 'Consultar%'
                     OR @Accion LIKE 'Registrar%'
                     OR @Accion LIKE 'Guardar%'
                     OR @Accion LIKE 'Editar%'
                     OR @Accion LIKE 'Inactivar%'
                     OR @Accion LIKE 'Eliminar%'
                  )
            )
            THEN 1
            ELSE 0
        END AS TieneAcceso;
END;
GO

/* =========================================================
   ACCESO - INICIO DE SESIÓN
   ========================================================= */
CREATE OR ALTER PROCEDURE Seguridad.sp_Acceso_IniciarSesion
    @NombreUsuario VARCHAR(45),
    @ClaveHash VARCHAR(255)
AS
BEGIN
    SET NOCOUNT ON;

    SET @NombreUsuario = LTRIM(RTRIM(ISNULL(@NombreUsuario, '')));
    SET @ClaveHash = LTRIM(RTRIM(ISNULL(@ClaveHash, '')));

    SELECT
        e.Identificacion,
        e.NombreUsuario,
        e.IdRol,
        r.Nombre AS NombreRol,

        LTRIM(RTRIM(
            ISNULL(p.Nombre, '') + ' ' +
            ISNULL(p.PrimerApellido, '') + ' ' +
            ISNULL(p.SegundoApellido, '')
        )) AS NombreCompleto,

        e.Activo,
        e.RestablecerClave
    FROM dbo.Empleado e
    INNER JOIN dbo.Persona p
        ON p.Identificacion = e.Identificacion
    INNER JOIN dbo.Rol r
        ON r.IdRol = e.IdRol
    WHERE e.NombreUsuario = @NombreUsuario
      AND e.ClaveHash = @ClaveHash
      AND e.Activo = 1
      AND r.Activo = 1;
END;
GO

/* =========================================================
   ACCESO - ACTUALIZAR ÚLTIMO ACCESO
   ========================================================= */
CREATE OR ALTER PROCEDURE Seguridad.sp_Acceso_ActualizarUltimoAcceso
    @Identificacion VARCHAR(45)
AS
BEGIN
    SET NOCOUNT ON;

    SET @Identificacion = LTRIM(RTRIM(ISNULL(@Identificacion, '')));

    UPDATE dbo.Empleado
    SET
        UltimoAcceso = GETDATE(),
        FechaModificacion = GETDATE()
    WHERE Identificacion = @Identificacion
      AND Activo = 1;
END;
GO

/* =========================================================
   ACCESO - REGISTRAR EVENTO DE SESIÓN
   Inicio sesión / cierre sesión.
   ========================================================= */
CREATE OR ALTER PROCEDURE Seguridad.sp_Acceso_RegistrarEventoSesion
    @Identificacion VARCHAR(45),
    @Evento VARCHAR(100),
    @Resultado BIT OUTPUT,
    @Mensaje VARCHAR(500) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @ResultadoHistorial BIT;
    DECLARE @MensajeHistorial VARCHAR(500);

    SET @Resultado = 0;
    SET @Mensaje = '';

    SET @Identificacion = LTRIM(RTRIM(ISNULL(@Identificacion, '')));
    SET @Evento = LTRIM(RTRIM(ISNULL(@Evento, '')));

    IF (@Evento = '')
    BEGIN
        SET @Evento = 'Evento de sesión';
    END;

    EXEC Seguridad.sp_HistorialCambio_Registrar
        @Identificacion = @Identificacion,
        @NombreTabla = 'Empleado',
        @TipoDeObservacion = 'Acceso',
        @Detalle = @Evento,
        @ValorAnterior = '',
        @ValorNuevo = '',
        @IdRegistro = @Identificacion,
        @Resultado = @ResultadoHistorial OUTPUT,
        @Mensaje = @MensajeHistorial OUTPUT;

    SET @Resultado = @ResultadoHistorial;
    SET @Mensaje = @MensajeHistorial;
END;
GO

/* =========================================================
   ACCESO - CAMBIAR CLAVE PROPIA
   Al cambiar correctamente:
   RestablecerClave = 0
   ========================================================= */
CREATE OR ALTER PROCEDURE Seguridad.sp_Acceso_CambiarClavePropia
    @Identificacion VARCHAR(45),
    @ClaveActualHash VARCHAR(255),
    @ClaveNuevaHash VARCHAR(255),
    @Resultado BIT OUTPUT,
    @Mensaje VARCHAR(500) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @ResultadoHistorial BIT;
    DECLARE @MensajeHistorial VARCHAR(500);

    SET @Resultado = 0;
    SET @Mensaje = '';

    SET @Identificacion = LTRIM(RTRIM(ISNULL(@Identificacion, '')));
    SET @ClaveActualHash = LTRIM(RTRIM(ISNULL(@ClaveActualHash, '')));
    SET @ClaveNuevaHash = LTRIM(RTRIM(ISNULL(@ClaveNuevaHash, '')));

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

    IF NOT EXISTS (
        SELECT 1
        FROM dbo.Empleado
        WHERE Identificacion = @Identificacion
          AND ClaveHash = @ClaveActualHash
          AND Activo = 1
    )
    BEGIN
        SET @Mensaje = 'La clave actual no es correcta.';
        RETURN;
    END;

    IF (@ClaveActualHash = @ClaveNuevaHash)
    BEGIN
        SET @Mensaje = 'La nueva clave no puede ser igual a la clave actual.';
        RETURN;
    END;

    BEGIN TRY
        UPDATE dbo.Empleado
        SET
            ClaveHash = @ClaveNuevaHash,
            RestablecerClave = 0,
            FechaModificacion = GETDATE()
        WHERE Identificacion = @Identificacion
          AND Activo = 1;

        EXEC Seguridad.sp_HistorialCambio_Registrar
            @Identificacion = @Identificacion,
            @NombreTabla = 'Empleado',
            @TipoDeObservacion = 'Cambio de clave',
            @Detalle = 'El empleado cambió su clave de acceso.',
            @ValorAnterior = 'Clave anterior protegida',
            @ValorNuevo = 'Clave nueva protegida; RestablecerClave=0',
            @IdRegistro = @Identificacion,
            @Resultado = @ResultadoHistorial OUTPUT,
            @Mensaje = @MensajeHistorial OUTPUT;

        SET @Resultado = 1;
        SET @Mensaje = 'Clave actualizada correctamente.';
    END TRY
    BEGIN CATCH
        SET @Resultado = 0;
        SET @Mensaje = ERROR_MESSAGE();
    END CATCH;
END;
GO

/* =========================================================
   EMPLEADO - RESTABLECER CLAVE
   Lo usa el administrador.
   RestablecerClave = 1 para obligar cambio al entrar.
   ========================================================= */
CREATE OR ALTER PROCEDURE Persona.sp_Empleado_RestablecerClave
    @Identificacion VARCHAR(45),
    @ClaveHash VARCHAR(255),
    @Resultado BIT OUTPUT,
    @Mensaje VARCHAR(500) OUTPUT,
    @NombreUsuario VARCHAR(45) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @ResultadoHistorial BIT;
    DECLARE @MensajeHistorial VARCHAR(500);

    SET @Resultado = 0;
    SET @Mensaje = '';
    SET @NombreUsuario = '';

    SET @Identificacion = LTRIM(RTRIM(ISNULL(@Identificacion, '')));
    SET @ClaveHash = LTRIM(RTRIM(ISNULL(@ClaveHash, '')));

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

    BEGIN TRY
        UPDATE dbo.Empleado
        SET
            ClaveHash = @ClaveHash,
            RestablecerClave = 1,
            FechaModificacion = GETDATE()
        WHERE Identificacion = @Identificacion
          AND Activo = 1;

        SELECT
            @NombreUsuario = NombreUsuario
        FROM dbo.Empleado
        WHERE Identificacion = @Identificacion;

        EXEC Seguridad.sp_HistorialCambio_Registrar
            @Identificacion = @Identificacion,
            @NombreTabla = 'Empleado',
            @TipoDeObservacion = 'Cambio de clave',
            @Detalle = 'Se restableció la clave del empleado.',
            @ValorAnterior = 'Clave anterior protegida',
            @ValorNuevo = 'Clave temporal protegida; RestablecerClave=1',
            @IdRegistro = @Identificacion,
            @Resultado = @ResultadoHistorial OUTPUT,
            @Mensaje = @MensajeHistorial OUTPUT;

        SET @Resultado = 1;
        SET @Mensaje = 'Clave restablecida correctamente.';
    END TRY
    BEGIN CATCH
        SET @Resultado = 0;
        SET @Mensaje = ERROR_MESSAGE();
        SET @NombreUsuario = '';
    END CATCH;
END;
GO

/* =========================================================
   PERMISOS TOTALES PARA ADMINISTRADOR / DUEÑO
   Se ejecuta una sola vez al final, después de crear módulos.
   ========================================================= */
DECLARE @IdRolAdministrador INT;

SELECT TOP 1
    @IdRolAdministrador = IdRol
FROM dbo.Rol
WHERE Activo = 1
  AND (
        Nombre COLLATE Latin1_General_CI_AI LIKE '%admin%'
     OR Nombre COLLATE Latin1_General_CI_AI LIKE '%dueño%'
     OR Nombre COLLATE Latin1_General_CI_AI LIKE '%dueno%'
     OR Nombre COLLATE Latin1_General_CI_AI LIKE '%propietario%'
  )
ORDER BY IdRol;

IF (@IdRolAdministrador IS NOT NULL)
BEGIN
    MERGE dbo.PermisoRolModulo AS destino
    USING
    (
        SELECT
            @IdRolAdministrador AS IdRol,
            CodigoModulo
        FROM dbo.ModuloSistema
        WHERE Activo = 1
    ) AS origen
    ON destino.IdRol = origen.IdRol
   AND destino.CodigoModulo = origen.CodigoModulo
    WHEN MATCHED THEN
        UPDATE SET
            PuedeVer = 1,
            PuedeCrear = 1,
            PuedeEditar = 1,
            PuedeEliminar = 1,
            FechaModificacion = GETDATE(),
            Activo = 1
    WHEN NOT MATCHED THEN
        INSERT
        (
            IdRol,
            CodigoModulo,
            PuedeVer,
            PuedeCrear,
            PuedeEditar,
            PuedeEliminar,
            FechaCreacion,
            FechaModificacion,
            Activo
        )
        VALUES
        (
            origen.IdRol,
            origen.CodigoModulo,
            1,
            1,
            1,
            1,
            GETDATE(),
            GETDATE(),
            1
        );
END;
GO

------------------------------------------------------------
-- AGREGAR MÓDULOS FALTANTES PARA CATÁLOGOS CONTABLES
------------------------------------------------------------

MERGE dbo.ModuloSistema AS destino
USING
(
    SELECT 
        'TIPOS_INGRESO' AS CodigoModulo,
        'Tipos de ingreso' AS NombreModulo,
        'Mantenimientos' AS AreaSistema,
        'Catalogos' AS Controlador,
        'TiposIngreso' AS Accion,
        '/Catalogos/TiposIngreso' AS Url,
        'feather icon-arrow-down-circle' AS Icono,
        101 AS Orden

    UNION ALL

    SELECT 
        'TIPOS_GASTO' AS CodigoModulo,
        'Tipos de gasto' AS NombreModulo,
        'Mantenimientos' AS AreaSistema,
        'Catalogos' AS Controlador,
        'TiposGasto' AS Accion,
        '/Catalogos/TiposGasto' AS Url,
        'feather icon-arrow-up-circle' AS Icono,
        102 AS Orden
) AS origen
ON destino.CodigoModulo = origen.CodigoModulo
WHEN MATCHED THEN
    UPDATE SET
        NombreModulo = origen.NombreModulo,
        AreaSistema = origen.AreaSistema,
        Controlador = origen.Controlador,
        Accion = origen.Accion,
        Url = origen.Url,
        Icono = origen.Icono,
        Orden = origen.Orden,
        FechaModificacion = GETDATE(),
        Activo = 1
WHEN NOT MATCHED THEN
    INSERT
    (
        CodigoModulo,
        NombreModulo,
        AreaSistema,
        Controlador,
        Accion,
        Url,
        Icono,
        Orden,
        FechaCreacion,
        FechaModificacion,
        Activo
    )
    VALUES
    (
        origen.CodigoModulo,
        origen.NombreModulo,
        origen.AreaSistema,
        origen.Controlador,
        origen.Accion,
        origen.Url,
        origen.Icono,
        origen.Orden,
        GETDATE(),
        GETDATE(),
        1
    );


    ------------------------------------------------------------
-- DAR PERMISOS AL ADMINISTRADOR SOBRE LOS NUEVOS MÓDULOS
------------------------------------------------------------

DECLARE @IdRolAdministrador INT;

SELECT TOP 1
    @IdRolAdministrador = IdRol
FROM dbo.Rol
WHERE Activo = 1
  AND (
        Nombre COLLATE Latin1_General_CI_AI LIKE '%admin%'
     OR Nombre COLLATE Latin1_General_CI_AI LIKE '%dueño%'
     OR Nombre COLLATE Latin1_General_CI_AI LIKE '%dueno%'
     OR Nombre COLLATE Latin1_General_CI_AI LIKE '%propietario%'
  )
ORDER BY IdRol;

IF (@IdRolAdministrador IS NOT NULL)
BEGIN
    MERGE dbo.PermisoRolModulo AS destino
    USING
    (
        SELECT
            @IdRolAdministrador AS IdRol,
            CodigoModulo
        FROM dbo.ModuloSistema
        WHERE CodigoModulo IN ('TIPOS_INGRESO', 'TIPOS_GASTO')
    ) AS origen
    ON destino.IdRol = origen.IdRol
   AND destino.CodigoModulo = origen.CodigoModulo
    WHEN MATCHED THEN
        UPDATE SET
            PuedeVer = 1,
            PuedeCrear = 1,
            PuedeEditar = 1,
            PuedeEliminar = 1,
            FechaModificacion = GETDATE(),
            Activo = 1
    WHEN NOT MATCHED THEN
        INSERT
        (
            IdRol,
            CodigoModulo,
            PuedeVer,
            PuedeCrear,
            PuedeEditar,
            PuedeEliminar,
            FechaCreacion,
            FechaModificacion,
            Activo
        )
        VALUES
        (
            origen.IdRol,
            origen.CodigoModulo,
            1,
            1,
            1,
            1,
            GETDATE(),
            GETDATE(),
            1
        );
END;