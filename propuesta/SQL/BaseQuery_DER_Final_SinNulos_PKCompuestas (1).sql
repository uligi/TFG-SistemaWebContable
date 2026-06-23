/* =========================================================
   BASE DE DATOS: SistemaWebContableGraditas
   MODELO FINAL DER - SIN NULOS Y CON PK SEMÁNTICAS

   Criterios aplicados:
   - No hay columnas NULL en el modelo.
   - Las PK representan la no duplicidad del hecho de negocio.
   - Se usan PK compuestas donde corresponde.
   - Provincia, Canton y Distrito usan código como PK.
   - Se agregan registros generales/raíz para evitar NULLs.
   ========================================================= */

IF DB_ID('SistemaWebContableGraditas') IS NULL
BEGIN
    CREATE DATABASE SistemaWebContableGraditas;
END
GO

USE SistemaWebContableGraditas;
GO

/* =========================================================
   LIMPIEZA OPCIONAL - usar solo si querés recrear todo
   =========================================================
DROP TABLE IF EXISTS dbo.HistorialCambio;
DROP TABLE IF EXISTS dbo.IntentoAcceso;
DROP TABLE IF EXISTS dbo.DetallePresupuestoMensual;
DROP TABLE IF EXISTS dbo.PresupuestoMensual;
DROP TABLE IF EXISTS dbo.Gasto;
DROP TABLE IF EXISTS dbo.Ingreso;
DROP TABLE IF EXISTS dbo.AbonoCuentaPorPagar;
DROP TABLE IF EXISTS dbo.CuentaPorPagar;
DROP TABLE IF EXISTS dbo.AbonoCuentaPorCobrar;
DROP TABLE IF EXISTS dbo.CuentaPorCobrar;
DROP TABLE IF EXISTS dbo.NotaCredito;
DROP TABLE IF EXISTS dbo.DetalleFactura;
DROP TABLE IF EXISTS dbo.Factura;
DROP TABLE IF EXISTS dbo.DetalleAsientoContable;
DROP TABLE IF EXISTS dbo.AsientoContable;
DROP TABLE IF EXISTS dbo.ConfiguracionCuentaContable;
DROP TABLE IF EXISTS dbo.Producto;
DROP TABLE IF EXISTS dbo.CuentaContable;
DROP TABLE IF EXISTS dbo.PeriodoContable;
DROP TABLE IF EXISTS dbo.CorreoProveedor;
DROP TABLE IF EXISTS dbo.TelefonoProveedor;
DROP TABLE IF EXISTS dbo.Proveedor;
DROP TABLE IF EXISTS dbo.Correo;
DROP TABLE IF EXISTS dbo.Telefono;
DROP TABLE IF EXISTS dbo.Empleado;
DROP TABLE IF EXISTS dbo.Cliente;
DROP TABLE IF EXISTS dbo.Persona;
DROP TABLE IF EXISTS dbo.Descuento;
DROP TABLE IF EXISTS dbo.Impuesto;
DROP TABLE IF EXISTS dbo.TipoProducto;
DROP TABLE IF EXISTS dbo.TipoCuentaContable;
DROP TABLE IF EXISTS dbo.TipoIngreso;
DROP TABLE IF EXISTS dbo.TipoGasto;
DROP TABLE IF EXISTS dbo.NaturalezaCuentaContable;
DROP TABLE IF EXISTS dbo.Puesto;
DROP TABLE IF EXISTS dbo.TipoDeObservacion;
DROP TABLE IF EXISTS dbo.TipoFactura;
DROP TABLE IF EXISTS dbo.TipoPago;
DROP TABLE IF EXISTS dbo.TipoCorreo;
DROP TABLE IF EXISTS dbo.TipoTelefono;
DROP TABLE IF EXISTS dbo.Rol;
DROP TABLE IF EXISTS dbo.Distrito;
DROP TABLE IF EXISTS dbo.Canton;
DROP TABLE IF EXISTS dbo.Provincia;
   ========================================================= */

/* =========================================================
   UBICACIÓN
   ========================================================= */
CREATE TABLE dbo.Provincia
(
    CodigoProvincia INT NOT NULL,
    Nombre          VARCHAR(45) NOT NULL,
    Activo          BIT NOT NULL CONSTRAINT DF_Provincia_Activo DEFAULT (1),
    CONSTRAINT PK_Provincia PRIMARY KEY (CodigoProvincia),
    CONSTRAINT UQ_Provincia_Nombre UNIQUE (Nombre),
    CONSTRAINT CK_Provincia_Codigo CHECK (CodigoProvincia > 0)
);
GO

CREATE TABLE dbo.Canton
(
    CodigoCanton    INT NOT NULL,
    CodigoProvincia INT NOT NULL,
    Nombre          VARCHAR(45) NOT NULL,
    Activo          BIT NOT NULL CONSTRAINT DF_Canton_Activo DEFAULT (1),
    CONSTRAINT PK_Canton PRIMARY KEY (CodigoCanton),
    CONSTRAINT UQ_Canton_Provincia_Nombre UNIQUE (CodigoProvincia, Nombre),
    CONSTRAINT FK_Canton_Provincia FOREIGN KEY (CodigoProvincia) REFERENCES dbo.Provincia(CodigoProvincia),
    CONSTRAINT CK_Canton_Codigo CHECK (CodigoCanton > 0)
);
GO

CREATE TABLE dbo.Distrito
(
    CodigoDistrito INT NOT NULL,
    CodigoCanton   INT NOT NULL,
    Nombre         VARCHAR(45) NOT NULL,
    Activo         BIT NOT NULL CONSTRAINT DF_Distrito_Activo DEFAULT (1),
    CONSTRAINT PK_Distrito PRIMARY KEY (CodigoDistrito),
    CONSTRAINT UQ_Distrito_Canton_Nombre UNIQUE (CodigoCanton, Nombre),
    CONSTRAINT FK_Distrito_Canton FOREIGN KEY (CodigoCanton) REFERENCES dbo.Canton(CodigoCanton),
    CONSTRAINT CK_Distrito_Codigo CHECK (CodigoDistrito > 0)
);
GO

/* =========================================================
   CATÁLOGOS
   ========================================================= */
CREATE TABLE dbo.Rol
(
    IdRol       INT IDENTITY(1,1) NOT NULL,
    Nombre      VARCHAR(45) NOT NULL,
    Descripcion VARCHAR(150) NOT NULL CONSTRAINT DF_Rol_Descripcion DEFAULT (''),
    Activo      BIT NOT NULL CONSTRAINT DF_Rol_Activo DEFAULT (1),
    CONSTRAINT PK_Rol PRIMARY KEY (IdRol),
    CONSTRAINT UQ_Rol_Nombre UNIQUE (Nombre)
);
GO

CREATE TABLE dbo.TipoTelefono
(
    IdTipoTelefono INT IDENTITY(1,1) NOT NULL,
    Nombre         VARCHAR(45) NOT NULL,
    Activo         BIT NOT NULL CONSTRAINT DF_TipoTelefono_Activo DEFAULT (1),
    CONSTRAINT PK_TipoTelefono PRIMARY KEY (IdTipoTelefono),
    CONSTRAINT UQ_TipoTelefono_Nombre UNIQUE (Nombre)
);
GO

CREATE TABLE dbo.TipoCorreo
(
    IdTipoCorreo INT IDENTITY(1,1) NOT NULL,
    Nombre       VARCHAR(45) NOT NULL,
    Activo       BIT NOT NULL CONSTRAINT DF_TipoCorreo_Activo DEFAULT (1),
    CONSTRAINT PK_TipoCorreo PRIMARY KEY (IdTipoCorreo),
    CONSTRAINT UQ_TipoCorreo_Nombre UNIQUE (Nombre)
);
GO

CREATE TABLE dbo.TipoPago
(
    IdTipoPago INT IDENTITY(1,1) NOT NULL,
    Nombre     VARCHAR(45) NOT NULL,
    Activo     BIT NOT NULL CONSTRAINT DF_TipoPago_Activo DEFAULT (1),
    CONSTRAINT PK_TipoPago PRIMARY KEY (IdTipoPago),
    CONSTRAINT UQ_TipoPago_Nombre UNIQUE (Nombre)
);
GO

CREATE TABLE dbo.TipoFactura
(
    IdTipoFactura INT IDENTITY(1,1) NOT NULL,
    TipoFactura   VARCHAR(45) NOT NULL,
    Descripcion   VARCHAR(150) NOT NULL CONSTRAINT DF_TipoFactura_Descripcion DEFAULT (''),
    Activo        BIT NOT NULL CONSTRAINT DF_TipoFactura_Activo DEFAULT (1),
    CONSTRAINT PK_TipoFactura PRIMARY KEY (IdTipoFactura),
    CONSTRAINT UQ_TipoFactura_TipoFactura UNIQUE (TipoFactura)
);
GO

CREATE TABLE dbo.TipoDeObservacion
(
    IdTipoDeObservacion INT IDENTITY(1,1) NOT NULL,
    TipoDeObservacion   VARCHAR(45) NOT NULL,
    Descripcion         VARCHAR(150) NOT NULL CONSTRAINT DF_TipoDeObservacion_Descripcion DEFAULT (''),
    Activo              BIT NOT NULL CONSTRAINT DF_TipoDeObservacion_Activo DEFAULT (1),
    CONSTRAINT PK_TipoDeObservacion PRIMARY KEY (IdTipoDeObservacion),
    CONSTRAINT UQ_TipoDeObservacion_Tipo UNIQUE (TipoDeObservacion)
);
GO

CREATE TABLE dbo.Puesto
(
    IdPuesto    INT IDENTITY(1,1) NOT NULL,
    Nombre      VARCHAR(45) NOT NULL,
    Descripcion VARCHAR(255) NOT NULL CONSTRAINT DF_Puesto_Descripcion DEFAULT (''),
    Activo      BIT NOT NULL CONSTRAINT DF_Puesto_Activo DEFAULT (1),
    CONSTRAINT PK_Puesto PRIMARY KEY (IdPuesto),
    CONSTRAINT UQ_Puesto_Nombre UNIQUE (Nombre)
);
GO

CREATE TABLE dbo.NaturalezaCuentaContable
(
    IdNaturalezaCuentaContable INT IDENTITY(1,1) NOT NULL,
    Naturaleza                 VARCHAR(45) NOT NULL,
    Descripcion                VARCHAR(255) NOT NULL CONSTRAINT DF_NaturalezaCuentaContable_Descripcion DEFAULT (''),
    Activo                     BIT NOT NULL CONSTRAINT DF_NaturalezaCuentaContable_Activo DEFAULT (1),
    CONSTRAINT PK_NaturalezaCuentaContable PRIMARY KEY (IdNaturalezaCuentaContable),
    CONSTRAINT UQ_NaturalezaCuentaContable_Naturaleza UNIQUE (Naturaleza)
);
GO

CREATE TABLE dbo.TipoGasto
(
    IdTipoGasto INT IDENTITY(1,1) NOT NULL,
    Nombre      VARCHAR(45) NOT NULL,
    Descripcion VARCHAR(255) NOT NULL CONSTRAINT DF_TipoGasto_Descripcion DEFAULT (''),
    Activo      BIT NOT NULL CONSTRAINT DF_TipoGasto_Activo DEFAULT (1),
    CONSTRAINT PK_TipoGasto PRIMARY KEY (IdTipoGasto),
    CONSTRAINT UQ_TipoGasto_Nombre UNIQUE (Nombre)
);
GO

CREATE TABLE dbo.TipoIngreso
(
    IdTipoIngreso INT IDENTITY(1,1) NOT NULL,
    Nombre        VARCHAR(45) NOT NULL,
    Descripcion   VARCHAR(255) NOT NULL CONSTRAINT DF_TipoIngreso_Descripcion DEFAULT (''),
    Activo        BIT NOT NULL CONSTRAINT DF_TipoIngreso_Activo DEFAULT (1),
    CONSTRAINT PK_TipoIngreso PRIMARY KEY (IdTipoIngreso),
    CONSTRAINT UQ_TipoIngreso_Nombre UNIQUE (Nombre)
);
GO

CREATE TABLE dbo.TipoCuentaContable
(
    IdTipoCuentaContable INT IDENTITY(1,1) NOT NULL,
    Nombre               VARCHAR(45) NOT NULL,
    Descripcion          VARCHAR(255) NOT NULL CONSTRAINT DF_TipoCuentaContable_Descripcion DEFAULT (''),
    Activo               BIT NOT NULL CONSTRAINT DF_TipoCuentaContable_Activo DEFAULT (1),
    CONSTRAINT PK_TipoCuentaContable PRIMARY KEY (IdTipoCuentaContable),
    CONSTRAINT UQ_TipoCuentaContable_Nombre UNIQUE (Nombre)
);
GO

CREATE TABLE dbo.TipoProducto
(
    IdTipoProducto INT IDENTITY(1,1) NOT NULL,
    Nombre         VARCHAR(100) NOT NULL,
    Descripcion    VARCHAR(150) NOT NULL CONSTRAINT DF_TipoProducto_Descripcion DEFAULT (''),
    Activo         BIT NOT NULL CONSTRAINT DF_TipoProducto_Activo DEFAULT (1),
    CONSTRAINT PK_TipoProducto PRIMARY KEY (IdTipoProducto),
    CONSTRAINT UQ_TipoProducto_Nombre UNIQUE (Nombre)
);
GO

CREATE TABLE dbo.Impuesto
(
    IdImpuesto  INT IDENTITY(1,1) NOT NULL,
    Nombre      VARCHAR(100) NOT NULL,
    Porcentaje  DECIMAL(5,2) NOT NULL,
    Descripcion VARCHAR(255) NOT NULL CONSTRAINT DF_Impuesto_Descripcion DEFAULT (''),
    Activo      BIT NOT NULL CONSTRAINT DF_Impuesto_Activo DEFAULT (1),
    CONSTRAINT PK_Impuesto PRIMARY KEY (IdImpuesto),
    CONSTRAINT UQ_Impuesto_Nombre UNIQUE (Nombre),
    CONSTRAINT CK_Impuesto_Porcentaje CHECK (Porcentaje >= 0 AND Porcentaje <= 100)
);
GO

CREATE TABLE dbo.Descuento
(
    IdDescuento          INT IDENTITY(1,1) NOT NULL,
    Nombre               VARCHAR(100) NOT NULL,
    Porcentaje           DECIMAL(5,2) NOT NULL,
    RequiereAutorizacion BIT NOT NULL CONSTRAINT DF_Descuento_RequiereAutorizacion DEFAULT (0),
    Descripcion          VARCHAR(255) NOT NULL CONSTRAINT DF_Descuento_Descripcion DEFAULT (''),
    Activo               BIT NOT NULL CONSTRAINT DF_Descuento_Activo DEFAULT (1),
    CONSTRAINT PK_Descuento PRIMARY KEY (IdDescuento),
    CONSTRAINT UQ_Descuento_Nombre UNIQUE (Nombre),
    CONSTRAINT CK_Descuento_Porcentaje CHECK (Porcentaje >= 0 AND Porcentaje <= 100)
);
GO

/* =========================================================
   PERSONAS, CLIENTES, EMPLEADOS Y PROVEEDORES
   ========================================================= */
CREATE TABLE dbo.Persona
(
    Identificacion     VARCHAR(45) NOT NULL,
    Nombre             VARCHAR(45) NOT NULL,
    PrimerApellido     VARCHAR(45) NOT NULL,
    SegundoApellido    VARCHAR(45) NOT NULL CONSTRAINT DF_Persona_SegundoApellido DEFAULT (''),
    FechaNacimiento    DATE NOT NULL CONSTRAINT DF_Persona_FechaNacimiento DEFAULT ('19000101'),
    CodigoDistrito     INT NOT NULL,
    DireccionExacta    VARCHAR(250) NOT NULL,
    FechaCreacion      DATETIME NOT NULL CONSTRAINT DF_Persona_FechaCreacion DEFAULT (GETDATE()),
    FechaModificacion  DATETIME NOT NULL CONSTRAINT DF_Persona_FechaModificacion DEFAULT (GETDATE()),
    Activo             BIT NOT NULL CONSTRAINT DF_Persona_Activo DEFAULT (1),
    CONSTRAINT PK_Persona PRIMARY KEY (Identificacion),
    CONSTRAINT FK_Persona_Distrito FOREIGN KEY (CodigoDistrito) REFERENCES dbo.Distrito(CodigoDistrito)
);
GO

CREATE TABLE dbo.Cliente
(
    Identificacion     VARCHAR(45) NOT NULL,
    CodigoCliente      VARCHAR(45) NOT NULL,
    LimiteCredito      DECIMAL(18,2) NOT NULL CONSTRAINT DF_Cliente_LimiteCredito DEFAULT (0),
    DiasCredito        INT NOT NULL CONSTRAINT DF_Cliente_DiasCredito DEFAULT (0),
    FechaCreacion      DATETIME NOT NULL CONSTRAINT DF_Cliente_FechaCreacion DEFAULT (GETDATE()),
    FechaModificacion  DATETIME NOT NULL CONSTRAINT DF_Cliente_FechaModificacion DEFAULT (GETDATE()),
    Activo             BIT NOT NULL CONSTRAINT DF_Cliente_Activo DEFAULT (1),
    CONSTRAINT PK_Cliente PRIMARY KEY (Identificacion),
    CONSTRAINT UQ_Cliente_CodigoCliente UNIQUE (CodigoCliente),
    CONSTRAINT FK_Cliente_Persona FOREIGN KEY (Identificacion) REFERENCES dbo.Persona(Identificacion),
    CONSTRAINT CK_Cliente_LimiteCredito CHECK (LimiteCredito >= 0),
    CONSTRAINT CK_Cliente_DiasCredito CHECK (DiasCredito >= 0)
);
GO

CREATE TABLE dbo.Empleado
(
    Identificacion     VARCHAR(45) NOT NULL,
    CodigoEmpleado     VARCHAR(45) NOT NULL,
    IdPuesto           INT NOT NULL,
    FechaIngreso       DATE NOT NULL,
    IdRol              INT NOT NULL,
    NombreUsuario      VARCHAR(45) NOT NULL,
    ClaveHash          VARCHAR(255) NOT NULL,
    UltimoAcceso       DATETIME NOT NULL CONSTRAINT DF_Empleado_UltimoAcceso DEFAULT (GETDATE()),
    RestablecerClave   BIT NOT NULL CONSTRAINT DF_Empleado_RestablecerClave DEFAULT (1),
    FechaCreacion      DATETIME NOT NULL CONSTRAINT DF_Empleado_FechaCreacion DEFAULT (GETDATE()),
    FechaModificacion  DATETIME NOT NULL CONSTRAINT DF_Empleado_FechaModificacion DEFAULT (GETDATE()),
    Activo             BIT NOT NULL CONSTRAINT DF_Empleado_Activo DEFAULT (1),
    CONSTRAINT PK_Empleado PRIMARY KEY (Identificacion),
    CONSTRAINT UQ_Empleado_CodigoEmpleado UNIQUE (CodigoEmpleado),
    CONSTRAINT UQ_Empleado_NombreUsuario UNIQUE (NombreUsuario),
    CONSTRAINT FK_Empleado_Persona FOREIGN KEY (Identificacion) REFERENCES dbo.Persona(Identificacion),
    CONSTRAINT FK_Empleado_Rol FOREIGN KEY (IdRol) REFERENCES dbo.Rol(IdRol),
    CONSTRAINT FK_Empleado_Puesto FOREIGN KEY (IdPuesto) REFERENCES dbo.Puesto(IdPuesto)
);
GO

CREATE TABLE dbo.Proveedor
(
    IdentificacionProveedor VARCHAR(45) NOT NULL,
    RazonSocial             VARCHAR(200) NOT NULL,
    NombreContacto          VARCHAR(100) NOT NULL,
    CodigoDistrito          INT NOT NULL,
    DireccionExacta         VARCHAR(250) NOT NULL,
    DiasCredito             INT NOT NULL CONSTRAINT DF_Proveedor_DiasCredito DEFAULT (0),
    FechaCreacion           DATETIME NOT NULL CONSTRAINT DF_Proveedor_FechaCreacion DEFAULT (GETDATE()),
    FechaModificacion       DATETIME NOT NULL CONSTRAINT DF_Proveedor_FechaModificacion DEFAULT (GETDATE()),
    Activo                  BIT NOT NULL CONSTRAINT DF_Proveedor_Activo DEFAULT (1),
    CONSTRAINT PK_Proveedor PRIMARY KEY (IdentificacionProveedor),
    CONSTRAINT UQ_Proveedor_RazonSocial UNIQUE (RazonSocial),
    CONSTRAINT FK_Proveedor_Distrito FOREIGN KEY (CodigoDistrito) REFERENCES dbo.Distrito(CodigoDistrito),
    CONSTRAINT CK_Proveedor_DiasCredito CHECK (DiasCredito >= 0)
);
GO

/* =========================================================
   CONTACTOS
   ========================================================= */
CREATE TABLE dbo.Telefono
(
    Identificacion VARCHAR(45) NOT NULL,
    NumeroTelefono VARCHAR(45) NOT NULL,
    IdTipoTelefono INT NOT NULL,
    EsPrincipal    BIT NOT NULL CONSTRAINT DF_Telefono_EsPrincipal DEFAULT (0),
    Activo         BIT NOT NULL CONSTRAINT DF_Telefono_Activo DEFAULT (1),
    CONSTRAINT PK_Telefono PRIMARY KEY (Identificacion, NumeroTelefono),
    CONSTRAINT FK_Telefono_Persona FOREIGN KEY (Identificacion) REFERENCES dbo.Persona(Identificacion),
    CONSTRAINT FK_Telefono_TipoTelefono FOREIGN KEY (IdTipoTelefono) REFERENCES dbo.TipoTelefono(IdTipoTelefono)
);
GO

CREATE TABLE dbo.Correo
(
    Identificacion  VARCHAR(45) NOT NULL,
    DireccionCorreo VARCHAR(255) NOT NULL,
    IdTipoCorreo    INT NOT NULL,
    EsPrincipal     BIT NOT NULL CONSTRAINT DF_Correo_EsPrincipal DEFAULT (0),
    Activo          BIT NOT NULL CONSTRAINT DF_Correo_Activo DEFAULT (1),
    CONSTRAINT PK_Correo PRIMARY KEY (Identificacion, DireccionCorreo),
    CONSTRAINT FK_Correo_Persona FOREIGN KEY (Identificacion) REFERENCES dbo.Persona(Identificacion),
    CONSTRAINT FK_Correo_TipoCorreo FOREIGN KEY (IdTipoCorreo) REFERENCES dbo.TipoCorreo(IdTipoCorreo)
);
GO

CREATE TABLE dbo.TelefonoProveedor
(
    IdentificacionProveedor VARCHAR(45) NOT NULL,
    NumeroTelefono          VARCHAR(45) NOT NULL,
    IdTipoTelefono          INT NOT NULL,
    EsPrincipal             BIT NOT NULL CONSTRAINT DF_TelefonoProveedor_EsPrincipal DEFAULT (0),
    Activo                  BIT NOT NULL CONSTRAINT DF_TelefonoProveedor_Activo DEFAULT (1),
    CONSTRAINT PK_TelefonoProveedor PRIMARY KEY (IdentificacionProveedor, NumeroTelefono),
    CONSTRAINT FK_TelefonoProveedor_Proveedor FOREIGN KEY (IdentificacionProveedor) REFERENCES dbo.Proveedor(IdentificacionProveedor),
    CONSTRAINT FK_TelefonoProveedor_TipoTelefono FOREIGN KEY (IdTipoTelefono) REFERENCES dbo.TipoTelefono(IdTipoTelefono)
);
GO

CREATE TABLE dbo.CorreoProveedor
(
    IdentificacionProveedor VARCHAR(45) NOT NULL,
    DireccionCorreo         VARCHAR(150) NOT NULL,
    IdTipoCorreo            INT NOT NULL,
    EsPrincipal             BIT NOT NULL CONSTRAINT DF_CorreoProveedor_EsPrincipal DEFAULT (0),
    Activo                  BIT NOT NULL CONSTRAINT DF_CorreoProveedor_Activo DEFAULT (1),
    CONSTRAINT PK_CorreoProveedor PRIMARY KEY (IdentificacionProveedor, DireccionCorreo),
    CONSTRAINT FK_CorreoProveedor_Proveedor FOREIGN KEY (IdentificacionProveedor) REFERENCES dbo.Proveedor(IdentificacionProveedor),
    CONSTRAINT FK_CorreoProveedor_TipoCorreo FOREIGN KEY (IdTipoCorreo) REFERENCES dbo.TipoCorreo(IdTipoCorreo)
);
GO

/* =========================================================
   CONTABILIDAD
   ========================================================= */
CREATE TABLE dbo.PeriodoContable
(
    Anio        INT NOT NULL,
    Mes         INT NOT NULL,
    FechaInicio DATETIME NOT NULL,
    FechaFin    DATETIME NOT NULL,
    Estado      VARCHAR(45) NOT NULL,
    Activo      BIT NOT NULL CONSTRAINT DF_PeriodoContable_Activo DEFAULT (1),
    CONSTRAINT PK_PeriodoContable PRIMARY KEY (Anio, Mes),
    CONSTRAINT CK_PeriodoContable_Mes CHECK (Mes BETWEEN 1 AND 12),
    CONSTRAINT CK_PeriodoContable_Fechas CHECK (FechaFin >= FechaInicio)
);
GO

CREATE TABLE dbo.CuentaContable
(
    CodigoCuenta                  VARCHAR(45) NOT NULL,
    NombreCuenta                  VARCHAR(100) NOT NULL,
    IdTipoCuentaContable          INT NOT NULL,
    IdNaturalezaCuentaContable    INT NOT NULL,
    AceptaMovimientos             BIT NOT NULL CONSTRAINT DF_CuentaContable_AceptaMov DEFAULT (1),
    Activo                        BIT NOT NULL CONSTRAINT DF_CuentaContable_Activo DEFAULT (1),
    CodigoCuentaPadre             VARCHAR(45) NOT NULL,
    CONSTRAINT PK_CuentaContable PRIMARY KEY (CodigoCuenta),
    CONSTRAINT UQ_CuentaContable_Nombre UNIQUE (NombreCuenta),
    CONSTRAINT FK_CuentaContable_TipoCuentaContable FOREIGN KEY (IdTipoCuentaContable) REFERENCES dbo.TipoCuentaContable(IdTipoCuentaContable),
    CONSTRAINT FK_CuentaContable_NaturalezaCuentaContable FOREIGN KEY (IdNaturalezaCuentaContable) REFERENCES dbo.NaturalezaCuentaContable(IdNaturalezaCuentaContable),
    CONSTRAINT FK_CuentaContable_CuentaPadre FOREIGN KEY (CodigoCuentaPadre) REFERENCES dbo.CuentaContable(CodigoCuenta)
);
GO

CREATE TABLE dbo.ConfiguracionCuentaContable
(
    CodigoOperacion   VARCHAR(45) NOT NULL,
    NombreOperacion   VARCHAR(100) NOT NULL,
    Descripcion       VARCHAR(200) NOT NULL CONSTRAINT DF_ConfiguracionCuentaContable_Descripcion DEFAULT (''),
    CodigoCuenta      VARCHAR(45) NOT NULL,
    FechaCreacion     DATETIME NOT NULL CONSTRAINT DF_ConfiguracionCuentaContable_FechaCreacion DEFAULT (GETDATE()),
    FechaModificacion DATETIME NOT NULL CONSTRAINT DF_ConfiguracionCuentaContable_FechaModificacion DEFAULT (GETDATE()),
    Activo            BIT NOT NULL CONSTRAINT DF_ConfiguracionCuentaContable_Activo DEFAULT (1),
    CONSTRAINT PK_ConfiguracionCuentaContable PRIMARY KEY (CodigoOperacion),
    CONSTRAINT FK_ConfiguracionCuentaContable_CuentaContable FOREIGN KEY (CodigoCuenta) REFERENCES dbo.CuentaContable(CodigoCuenta)
);
GO

CREATE TABLE dbo.AsientoContable
(
    NumeroAsiento VARCHAR(45) NOT NULL,
    Anio          INT NOT NULL,
    Mes           INT NOT NULL,
    FechaAsiento  DATETIME NOT NULL,
    TipoAsiento   VARCHAR(45) NOT NULL,
    Concepto      VARCHAR(150) NOT NULL,
    TotalDebe     DECIMAL(18,2) NOT NULL,
    TotalHaber    DECIMAL(18,2) NOT NULL,
    Activo        BIT NOT NULL CONSTRAINT DF_AsientoContable_Activo DEFAULT (1),
    CONSTRAINT PK_AsientoContable PRIMARY KEY (NumeroAsiento),
    CONSTRAINT FK_AsientoContable_PeriodoContable FOREIGN KEY (Anio, Mes) REFERENCES dbo.PeriodoContable(Anio, Mes),
    CONSTRAINT CK_AsientoContable_Totales CHECK (TotalDebe >= 0 AND TotalHaber >= 0)
);
GO

CREATE TABLE dbo.DetalleAsientoContable
(
    NumeroAsiento    VARCHAR(45) NOT NULL,
    NumeroLinea      INT NOT NULL,
    CodigoCuenta     VARCHAR(45) NOT NULL,
    Debe             DECIMAL(18,2) NOT NULL CONSTRAINT DF_DetalleAsientoContable_Debe DEFAULT (0),
    Haber            DECIMAL(18,2) NOT NULL CONSTRAINT DF_DetalleAsientoContable_Haber DEFAULT (0),
    DescripcionLinea VARCHAR(150) NOT NULL CONSTRAINT DF_DetalleAsientoContable_Descripcion DEFAULT (''),
    Activo           BIT NOT NULL CONSTRAINT DF_DetalleAsientoContable_Activo DEFAULT (1),
    CONSTRAINT PK_DetalleAsientoContable PRIMARY KEY (NumeroAsiento, NumeroLinea),
    CONSTRAINT FK_DetalleAsientoContable_Asiento FOREIGN KEY (NumeroAsiento) REFERENCES dbo.AsientoContable(NumeroAsiento),
    CONSTRAINT FK_DetalleAsientoContable_Cuenta FOREIGN KEY (CodigoCuenta) REFERENCES dbo.CuentaContable(CodigoCuenta),
    CONSTRAINT CK_DetalleAsientoContable_NumeroLinea CHECK (NumeroLinea > 0),
    CONSTRAINT CK_DetalleAsientoContable_Montos CHECK (Debe >= 0 AND Haber >= 0)
);
GO

IF OBJECT_ID('dbo.ReferenciaAsientoContable', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.ReferenciaAsientoContable
    (
        IdReferenciaAsiento INT IDENTITY(1,1) NOT NULL,
        NumeroAsiento VARCHAR(45) NOT NULL,
        ModuloOrigen VARCHAR(45) NOT NULL,
        DocumentoOrigen VARCHAR(45) NOT NULL,
        TipoMovimiento VARCHAR(45) NOT NULL,
        FechaCreacion DATETIME NOT NULL DEFAULT GETDATE(),
        Activo BIT NOT NULL DEFAULT 1,

        CONSTRAINT PK_ReferenciaAsientoContable
            PRIMARY KEY (IdReferenciaAsiento),

        CONSTRAINT FK_ReferenciaAsientoContable_AsientoContable
            FOREIGN KEY (NumeroAsiento)
            REFERENCES dbo.AsientoContable(NumeroAsiento),

        CONSTRAINT UQ_ReferenciaAsientoContable_Origen
            UNIQUE (ModuloOrigen, DocumentoOrigen, TipoMovimiento)
    );
END;
GO

/* =========================================================
   PRODUCTOS
   ========================================================= */
CREATE TABLE dbo.Producto
(
    CodigoProducto    VARCHAR(45) NOT NULL,
    NombreProducto    VARCHAR(150) NOT NULL,
    IdTipoProducto    INT NOT NULL,
    IdImpuesto        INT NOT NULL,
    Descripcion       VARCHAR(250) NOT NULL CONSTRAINT DF_Producto_Descripcion DEFAULT (''),
    PrecioVenta       DECIMAL(18,2) NOT NULL,
    StockActual       DECIMAL(18,2) NOT NULL CONSTRAINT DF_Producto_StockActual DEFAULT (0),
    FechaCreacion     DATETIME NOT NULL CONSTRAINT DF_Producto_FechaCreacion DEFAULT (GETDATE()),
    FechaModificacion DATETIME NOT NULL CONSTRAINT DF_Producto_FechaModificacion DEFAULT (GETDATE()),
    Activo            BIT NOT NULL CONSTRAINT DF_Producto_Activo DEFAULT (1),
    CONSTRAINT PK_Producto PRIMARY KEY (CodigoProducto),
    CONSTRAINT FK_Producto_TipoProducto FOREIGN KEY (IdTipoProducto) REFERENCES dbo.TipoProducto(IdTipoProducto),
    CONSTRAINT FK_Producto_Impuesto FOREIGN KEY (IdImpuesto) REFERENCES dbo.Impuesto(IdImpuesto),
    CONSTRAINT CK_Producto_PrecioVenta CHECK (PrecioVenta >= 0),
    CONSTRAINT CK_Producto_StockActual CHECK (StockActual >= 0)
);
GO

/* =========================================================
   FACTURACIÓN
   ========================================================= */
CREATE TABLE dbo.Factura
(
    NumeroFactura          VARCHAR(45) NOT NULL,
    IdentificacionCliente  VARCHAR(45) NOT NULL,
    IdentificacionEmpleado VARCHAR(45) NOT NULL,
    IdTipoPago             INT NOT NULL,
    IdTipoFactura          INT NOT NULL,
    FechaFactura           DATETIME NOT NULL,
    Subtotal               DECIMAL(18,2) NOT NULL,
    TotalImpuesto          DECIMAL(18,2) NOT NULL,
    TotalDescuento         DECIMAL(18,2) NOT NULL,
    TotalFactura           DECIMAL(18,2) NOT NULL,
    Estado                 VARCHAR(45) NOT NULL,
    FechaCreacion          DATETIME NOT NULL CONSTRAINT DF_Factura_FechaCreacion DEFAULT (GETDATE()),
    FechaModificacion      DATETIME NOT NULL CONSTRAINT DF_Factura_FechaModificacion DEFAULT (GETDATE()),
    Activo                 BIT NOT NULL CONSTRAINT DF_Factura_Activo DEFAULT (1),
    CONSTRAINT PK_Factura PRIMARY KEY (NumeroFactura),
    CONSTRAINT UQ_Factura_Cliente_Numero UNIQUE (IdentificacionCliente, NumeroFactura),
    CONSTRAINT FK_Factura_Cliente FOREIGN KEY (IdentificacionCliente) REFERENCES dbo.Cliente(Identificacion),
    CONSTRAINT FK_Factura_Empleado FOREIGN KEY (IdentificacionEmpleado) REFERENCES dbo.Empleado(Identificacion),
    CONSTRAINT FK_Factura_TipoPago FOREIGN KEY (IdTipoPago) REFERENCES dbo.TipoPago(IdTipoPago),
    CONSTRAINT FK_Factura_TipoFactura FOREIGN KEY (IdTipoFactura) REFERENCES dbo.TipoFactura(IdTipoFactura),
    CONSTRAINT CK_Factura_Totales CHECK (Subtotal >= 0 AND TotalImpuesto >= 0 AND TotalDescuento >= 0 AND TotalFactura >= 0)
);
GO

CREATE TABLE dbo.DetalleFactura
(
    NumeroFactura       VARCHAR(45) NOT NULL,
    CodigoProducto      VARCHAR(45) NOT NULL,
    IdImpuesto          INT NOT NULL,
    IdDescuento         INT NOT NULL,
    DescripcionItem     VARCHAR(150) NOT NULL CONSTRAINT DF_DetalleFactura_DescripcionItem DEFAULT (''),
    Cantidad            DECIMAL(18,2) NOT NULL,
    PrecioUnitario      DECIMAL(18,2) NOT NULL,
    PorcentajeImpuesto  DECIMAL(5,2) NOT NULL CONSTRAINT DF_DetalleFactura_PorcImp DEFAULT (0),
    PorcentajeDescuento DECIMAL(5,2) NOT NULL CONSTRAINT DF_DetalleFactura_PorcDesc DEFAULT (0),
    SubtotalLinea       DECIMAL(18,2) NOT NULL,
    TotalLinea          DECIMAL(18,2) NOT NULL,
    FechaCreacion       DATETIME NOT NULL CONSTRAINT DF_DetalleFactura_FechaCreacion DEFAULT (GETDATE()),
    FechaModificacion   DATETIME NOT NULL CONSTRAINT DF_DetalleFactura_FechaModificacion DEFAULT (GETDATE()),
    Activo              BIT NOT NULL CONSTRAINT DF_DetalleFactura_Activo DEFAULT (1),
    CONSTRAINT PK_DetalleFactura PRIMARY KEY (NumeroFactura, CodigoProducto),
    CONSTRAINT FK_DetalleFactura_Factura FOREIGN KEY (NumeroFactura) REFERENCES dbo.Factura(NumeroFactura),
    CONSTRAINT FK_DetalleFactura_Producto FOREIGN KEY (CodigoProducto) REFERENCES dbo.Producto(CodigoProducto),
    CONSTRAINT FK_DetalleFactura_Impuesto FOREIGN KEY (IdImpuesto) REFERENCES dbo.Impuesto(IdImpuesto),
    CONSTRAINT FK_DetalleFactura_Descuento FOREIGN KEY (IdDescuento) REFERENCES dbo.Descuento(IdDescuento),
    CONSTRAINT CK_DetalleFactura_Cantidad CHECK (Cantidad > 0),
    CONSTRAINT CK_DetalleFactura_PrecioUnitario CHECK (PrecioUnitario >= 0),
    CONSTRAINT CK_DetalleFactura_PorcImp CHECK (PorcentajeImpuesto >= 0 AND PorcentajeImpuesto <= 100),
    CONSTRAINT CK_DetalleFactura_PorcDesc CHECK (PorcentajeDescuento >= 0 AND PorcentajeDescuento <= 100),
    CONSTRAINT CK_DetalleFactura_Totales CHECK (SubtotalLinea >= 0 AND TotalLinea >= 0)
);
GO

CREATE TABLE dbo.NotaCredito
(
    NumeroNotaCredito      VARCHAR(45) NOT NULL,
    NumeroFactura          VARCHAR(45) NOT NULL,
    IdentificacionEmpleado VARCHAR(45) NOT NULL,
    FechaNotaCredito       DATETIME NOT NULL,
    Motivo                 VARCHAR(250) NOT NULL,
    Subtotal               DECIMAL(18,2) NOT NULL,
    TotalImpuesto          DECIMAL(18,2) NOT NULL,
    TotalNotaCredito       DECIMAL(18,2) NOT NULL,
    Estado                 VARCHAR(45) NOT NULL,
    FechaCreacion          DATETIME NOT NULL CONSTRAINT DF_NotaCredito_FechaCreacion DEFAULT (GETDATE()),
    FechaModificacion      DATETIME NOT NULL CONSTRAINT DF_NotaCredito_FechaModificacion DEFAULT (GETDATE()),
    Activo                 BIT NOT NULL CONSTRAINT DF_NotaCredito_Activo DEFAULT (1),
    CONSTRAINT PK_NotaCredito PRIMARY KEY (NumeroNotaCredito),
    CONSTRAINT FK_NotaCredito_Factura FOREIGN KEY (NumeroFactura) REFERENCES dbo.Factura(NumeroFactura),
    CONSTRAINT FK_NotaCredito_Empleado FOREIGN KEY (IdentificacionEmpleado) REFERENCES dbo.Empleado(Identificacion),
    CONSTRAINT CK_NotaCredito_Totales CHECK (Subtotal >= 0 AND TotalImpuesto >= 0 AND TotalNotaCredito >= 0)
);
GO

/* =========================================================
   CUENTAS POR COBRAR
   ========================================================= */
CREATE TABLE dbo.CuentaPorCobrar
(
    IdentificacionCliente VARCHAR(45) NOT NULL,
    NumeroFactura         VARCHAR(45) NOT NULL,
    FechaEmision          DATETIME NOT NULL,
    FechaVencimiento      DATETIME NOT NULL,
    MontoOriginal         DECIMAL(18,2) NOT NULL,
    SaldoActual           DECIMAL(18,2) NOT NULL,
    Estado                VARCHAR(45) NOT NULL,
    FechaCreacion         DATETIME NOT NULL CONSTRAINT DF_CuentaPorCobrar_FechaCreacion DEFAULT (GETDATE()),
    FechaModificacion     DATETIME NOT NULL CONSTRAINT DF_CuentaPorCobrar_FechaModificacion DEFAULT (GETDATE()),
    Activo                BIT NOT NULL CONSTRAINT DF_CuentaPorCobrar_Activo DEFAULT (1),
    CONSTRAINT PK_CuentaPorCobrar PRIMARY KEY (IdentificacionCliente, NumeroFactura),
    CONSTRAINT FK_CuentaPorCobrar_FacturaCliente FOREIGN KEY (IdentificacionCliente, NumeroFactura) REFERENCES dbo.Factura(IdentificacionCliente, NumeroFactura),
    CONSTRAINT CK_CuentaPorCobrar_Fechas CHECK (FechaVencimiento >= FechaEmision),
    CONSTRAINT CK_CuentaPorCobrar_Montos CHECK (MontoOriginal >= 0 AND SaldoActual >= 0)
);
GO

CREATE TABLE dbo.AbonoCuentaPorCobrar
(
    IdentificacionCliente VARCHAR(45) NOT NULL,
    NumeroFactura         VARCHAR(45) NOT NULL,
    NumeroAbono           INT NOT NULL,
    FechaAbono            DATETIME NOT NULL,
    MontoAbono            DECIMAL(18,2) NOT NULL,
    Observacion           VARCHAR(150) NOT NULL CONSTRAINT DF_AbonoCxC_Observacion DEFAULT (''),
    FechaCreacion         DATETIME NOT NULL CONSTRAINT DF_AbonoCxC_FechaCreacion DEFAULT (GETDATE()),
    FechaModificacion     DATETIME NOT NULL CONSTRAINT DF_AbonoCxC_FechaModificacion DEFAULT (GETDATE()),
    Activo                BIT NOT NULL CONSTRAINT DF_AbonoCxC_Activo DEFAULT (1),
    CONSTRAINT PK_AbonoCuentaPorCobrar PRIMARY KEY (IdentificacionCliente, NumeroFactura, NumeroAbono),
    CONSTRAINT FK_AbonoCuentaPorCobrar_Cuenta FOREIGN KEY (IdentificacionCliente, NumeroFactura) REFERENCES dbo.CuentaPorCobrar(IdentificacionCliente, NumeroFactura),
    CONSTRAINT CK_AbonoCuentaPorCobrar_NumeroAbono CHECK (NumeroAbono >= 0),
    CONSTRAINT CK_AbonoCuentaPorCobrar_Monto CHECK (MontoAbono >= 0)
);
GO

/* =========================================================
   CUENTAS POR PAGAR
   ========================================================= */
CREATE TABLE dbo.CuentaPorPagar
(
    IdentificacionProveedor VARCHAR(45) NOT NULL,
    NumeroDocumento         VARCHAR(45) NOT NULL,
    FechaEmision            DATETIME NOT NULL,
    FechaVencimiento        DATETIME NOT NULL,
    Concepto                VARCHAR(100) NOT NULL,
    MontoOriginal           DECIMAL(18,2) NOT NULL,
    SaldoActual             DECIMAL(18,2) NOT NULL,
    Estado                  VARCHAR(45) NOT NULL,
    FechaCreacion           DATETIME NOT NULL CONSTRAINT DF_CuentaPorPagar_FechaCreacion DEFAULT (GETDATE()),
    FechaModificacion       DATETIME NOT NULL CONSTRAINT DF_CuentaPorPagar_FechaModificacion DEFAULT (GETDATE()),
    Activo                  BIT NOT NULL CONSTRAINT DF_CuentaPorPagar_Activo DEFAULT (1),
    CONSTRAINT PK_CuentaPorPagar PRIMARY KEY (IdentificacionProveedor, NumeroDocumento),
    CONSTRAINT FK_CuentaPorPagar_Proveedor FOREIGN KEY (IdentificacionProveedor) REFERENCES dbo.Proveedor(IdentificacionProveedor),
    CONSTRAINT CK_CuentaPorPagar_Fechas CHECK (FechaVencimiento >= FechaEmision),
    CONSTRAINT CK_CuentaPorPagar_Montos CHECK (MontoOriginal >= 0 AND SaldoActual >= 0)
);
GO

CREATE TABLE dbo.AbonoCuentaPorPagar
(
    IdentificacionProveedor VARCHAR(45) NOT NULL,
    NumeroDocumento         VARCHAR(45) NOT NULL,
    NumeroAbono             INT NOT NULL,
    FechaAbono              DATETIME NOT NULL,
    MontoAbono              DECIMAL(18,2) NOT NULL,
    Observacion             VARCHAR(150) NOT NULL CONSTRAINT DF_AbonoCxP_Observacion DEFAULT (''),
    FechaCreacion           DATETIME NOT NULL CONSTRAINT DF_AbonoCxP_FechaCreacion DEFAULT (GETDATE()),
    FechaModificacion       DATETIME NOT NULL CONSTRAINT DF_AbonoCxP_FechaModificacion DEFAULT (GETDATE()),
    Activo                  BIT NOT NULL CONSTRAINT DF_AbonoCxP_Activo DEFAULT (1),
    CONSTRAINT PK_AbonoCuentaPorPagar PRIMARY KEY (IdentificacionProveedor, NumeroDocumento, NumeroAbono),
    CONSTRAINT FK_AbonoCuentaPorPagar_Cuenta FOREIGN KEY (IdentificacionProveedor, NumeroDocumento) REFERENCES dbo.CuentaPorPagar(IdentificacionProveedor, NumeroDocumento),
    CONSTRAINT CK_AbonoCuentaPorPagar_NumeroAbono CHECK (NumeroAbono >= 0),
    CONSTRAINT CK_AbonoCuentaPorPagar_Monto CHECK (MontoAbono >= 0)
);
GO

/* =========================================================
   INGRESOS Y GASTOS
   ========================================================= */
CREATE TABLE dbo.Ingreso
(
    NumeroIngreso                      VARCHAR(45) NOT NULL,
    FechaIngreso                       DATETIME NOT NULL,
    Descripcion                        VARCHAR(150) NOT NULL,
    IdTipoIngreso                      INT NOT NULL,
    CodigoCuenta                       VARCHAR(45) NOT NULL,
    IdentificacionCliente              VARCHAR(45) NOT NULL CONSTRAINT DF_Ingreso_IdentificacionCliente DEFAULT ('000000000'),
    NumeroFactura                      VARCHAR(45) NOT NULL CONSTRAINT DF_Ingreso_NumeroFactura DEFAULT ('FAC-GENERAL-000000'),
    IdentificacionClienteAbono         VARCHAR(45) NOT NULL CONSTRAINT DF_Ingreso_IdentificacionClienteAbono DEFAULT ('000000000'),
    NumeroFacturaAbono                 VARCHAR(45) NOT NULL CONSTRAINT DF_Ingreso_NumeroFacturaAbono DEFAULT ('FAC-GENERAL-000000'),
    NumeroAbonoCuentaPorCobrar         INT NOT NULL CONSTRAINT DF_Ingreso_NumeroAbonoCxC DEFAULT (0),
    OrigenIngreso                      VARCHAR(45) NOT NULL CONSTRAINT DF_Ingreso_Origen DEFAULT ('Manual'),
    Monto                              DECIMAL(18,2) NOT NULL,
    FechaCreacion                      DATETIME NOT NULL CONSTRAINT DF_Ingreso_FechaCreacion DEFAULT (GETDATE()),
    FechaModificacion                  DATETIME NOT NULL CONSTRAINT DF_Ingreso_FechaModificacion DEFAULT (GETDATE()),
    Activo                             BIT NOT NULL CONSTRAINT DF_Ingreso_Activo DEFAULT (1),
    CONSTRAINT PK_Ingreso PRIMARY KEY (NumeroIngreso),
    CONSTRAINT FK_Ingreso_TipoIngreso FOREIGN KEY (IdTipoIngreso) REFERENCES dbo.TipoIngreso(IdTipoIngreso),
    CONSTRAINT FK_Ingreso_CuentaContable FOREIGN KEY (CodigoCuenta) REFERENCES dbo.CuentaContable(CodigoCuenta),
    CONSTRAINT FK_Ingreso_Factura FOREIGN KEY (IdentificacionCliente, NumeroFactura) REFERENCES dbo.Factura(IdentificacionCliente, NumeroFactura),
    CONSTRAINT FK_Ingreso_AbonoCuentaPorCobrar FOREIGN KEY (IdentificacionClienteAbono, NumeroFacturaAbono, NumeroAbonoCuentaPorCobrar) REFERENCES dbo.AbonoCuentaPorCobrar(IdentificacionCliente, NumeroFactura, NumeroAbono),
    CONSTRAINT CK_Ingreso_Monto CHECK (Monto >= 0)
);
GO

CREATE TABLE dbo.Gasto
(
    NumeroGasto                        VARCHAR(45) NOT NULL,
    FechaGasto                         DATETIME NOT NULL,
    Descripcion                        VARCHAR(150) NOT NULL,
    IdTipoGasto                        INT NOT NULL,
    CodigoCuenta                       VARCHAR(45) NOT NULL,
    Monto                              DECIMAL(18,2) NOT NULL,
    IdentificacionProveedor            VARCHAR(45) NOT NULL CONSTRAINT DF_Gasto_IdentificacionProveedor DEFAULT ('000000000'),
    NumeroDocumento                    VARCHAR(45) NOT NULL CONSTRAINT DF_Gasto_NumeroDocumento DEFAULT ('CXP-GENERAL-000000'),
    IdentificacionProveedorAbono       VARCHAR(45) NOT NULL CONSTRAINT DF_Gasto_IdentificacionProveedorAbono DEFAULT ('000000000'),
    NumeroDocumentoAbono               VARCHAR(45) NOT NULL CONSTRAINT DF_Gasto_NumeroDocumentoAbono DEFAULT ('CXP-GENERAL-000000'),
    NumeroAbonoCuentaPorPagar          INT NOT NULL CONSTRAINT DF_Gasto_NumeroAbonoCxP DEFAULT (0),
    NumeroComprobante                  VARCHAR(45) NOT NULL CONSTRAINT DF_Gasto_NumeroComprobante DEFAULT (''),
    NombreArchivoComprobante           VARCHAR(150) NOT NULL CONSTRAINT DF_Gasto_NombreArchivo DEFAULT (''),
    RutaComprobante                    VARCHAR(300) NOT NULL CONSTRAINT DF_Gasto_Ruta DEFAULT (''),
    FechaCreacion                      DATETIME NOT NULL CONSTRAINT DF_Gasto_FechaCreacion DEFAULT (GETDATE()),
    FechaModificacion                  DATETIME NOT NULL CONSTRAINT DF_Gasto_FechaModificacion DEFAULT (GETDATE()),
    Activo                             BIT NOT NULL CONSTRAINT DF_Gasto_Activo DEFAULT (1),
    CONSTRAINT PK_Gasto PRIMARY KEY (NumeroGasto),
    CONSTRAINT FK_Gasto_TipoGasto FOREIGN KEY (IdTipoGasto) REFERENCES dbo.TipoGasto(IdTipoGasto),
    CONSTRAINT FK_Gasto_CuentaContable FOREIGN KEY (CodigoCuenta) REFERENCES dbo.CuentaContable(CodigoCuenta),
    CONSTRAINT FK_Gasto_Proveedor FOREIGN KEY (IdentificacionProveedor) REFERENCES dbo.Proveedor(IdentificacionProveedor),
    CONSTRAINT FK_Gasto_CuentaPorPagar FOREIGN KEY (IdentificacionProveedor, NumeroDocumento) REFERENCES dbo.CuentaPorPagar(IdentificacionProveedor, NumeroDocumento),
    CONSTRAINT FK_Gasto_AbonoCuentaPorPagar FOREIGN KEY (IdentificacionProveedorAbono, NumeroDocumentoAbono, NumeroAbonoCuentaPorPagar) REFERENCES dbo.AbonoCuentaPorPagar(IdentificacionProveedor, NumeroDocumento, NumeroAbono),
    CONSTRAINT CK_Gasto_Monto CHECK (Monto >= 0)
);
GO

/* =========================================================
   PRESUPUESTO
   ========================================================= */
CREATE TABLE dbo.PresupuestoMensual
(
    Anio              INT NOT NULL,
    Mes               INT NOT NULL,
    Estado            VARCHAR(45) NOT NULL,
    FechaCreacion     DATETIME NOT NULL CONSTRAINT DF_PresupuestoMensual_FechaCreacion DEFAULT (GETDATE()),
    FechaModificacion DATETIME NOT NULL CONSTRAINT DF_PresupuestoMensual_FechaModificacion DEFAULT (GETDATE()),
    Activo            BIT NOT NULL CONSTRAINT DF_PresupuestoMensual_Activo DEFAULT (1),
    CONSTRAINT PK_PresupuestoMensual PRIMARY KEY (Anio, Mes),
    CONSTRAINT CK_PresupuestoMensual_Mes CHECK (Mes BETWEEN 1 AND 12)
);
GO

CREATE TABLE dbo.DetallePresupuestoMensual
(
    Anio               INT NOT NULL,
    Mes                INT NOT NULL,
    CodigoCuenta       VARCHAR(45) NOT NULL,
    TipoMovimiento     VARCHAR(45) NOT NULL,
    MontoPresupuestado DECIMAL(18,2) NOT NULL,
    FechaCreacion      DATETIME NOT NULL CONSTRAINT DF_DetPresupuesto_FechaCreacion DEFAULT (GETDATE()),
    FechaModificacion  DATETIME NOT NULL CONSTRAINT DF_DetPresupuesto_FechaModificacion DEFAULT (GETDATE()),
    Activo             BIT NOT NULL CONSTRAINT DF_DetPresupuesto_Activo DEFAULT (1),
    CONSTRAINT PK_DetallePresupuestoMensual PRIMARY KEY (Anio, Mes, CodigoCuenta, TipoMovimiento),
    CONSTRAINT FK_DetallePresupuestoMensual_Presupuesto FOREIGN KEY (Anio, Mes) REFERENCES dbo.PresupuestoMensual(Anio, Mes),
    CONSTRAINT FK_DetallePresupuestoMensual_Cuenta FOREIGN KEY (CodigoCuenta) REFERENCES dbo.CuentaContable(CodigoCuenta),
    CONSTRAINT CK_DetPresupuesto_Monto CHECK (MontoPresupuestado >= 0),
    CONSTRAINT CK_DetPresupuesto_TipoMovimiento CHECK (TipoMovimiento IN ('Ingreso', 'Gasto'))
);
GO

/* =========================================================
   SEGURIDAD Y AUDITORÍA
   ========================================================= */


CREATE TABLE dbo.HistorialCambio
(
    IdHistorialCambio   INT IDENTITY(1,1) NOT NULL,
    Identificacion      VARCHAR(45) NOT NULL CONSTRAINT DF_HistorialCambio_Identificacion DEFAULT ('000000000'),
    NombreTabla         VARCHAR(100) NOT NULL,
    IdTipoDeObservacion INT NOT NULL,
    FechaHoraCambio     DATETIME NOT NULL CONSTRAINT DF_HistorialCambio_Fecha DEFAULT (GETDATE()),
    Detalle             VARCHAR(300) NOT NULL CONSTRAINT DF_HistorialCambio_Detalle DEFAULT (''),
    ValorAnterior       VARCHAR(900) NOT NULL CONSTRAINT DF_HistorialCambio_ValorAnterior DEFAULT (''),
    ValorNuevo          VARCHAR(900) NOT NULL CONSTRAINT DF_HistorialCambio_ValorNuevo DEFAULT (''),
    IdRegistro          VARCHAR(150) NOT NULL,
    Activo              BIT NOT NULL CONSTRAINT DF_HistorialCambio_Activo DEFAULT (1),
    CONSTRAINT PK_HistorialCambio PRIMARY KEY (IdHistorialCambio),
    CONSTRAINT FK_HistorialCambio_Empleado FOREIGN KEY (Identificacion) REFERENCES dbo.Empleado(Identificacion),
    CONSTRAINT FK_HistorialCambio_TipoDeObservacion FOREIGN KEY (IdTipoDeObservacion) REFERENCES dbo.TipoDeObservacion(IdTipoDeObservacion)
);
GO
/* =========================================================
   SEGURIDAD BASE
   Permisos por rol, módulos del sistema e historial respetando DER.
   ========================================================= */

IF NOT EXISTS (SELECT 1 FROM sys.schemas WHERE name = 'Seguridad')
BEGIN
    EXEC('CREATE SCHEMA Seguridad');
END;
GO

/* =========================================================
   1. TABLA MODULO SISTEMA
   Catálogo de pantallas o módulos disponibles.
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
   2. TABLA PERMISO ROL MODULO
   Permisos por rol. También tiene Activo para borrado lógico.
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
   DATOS BASE OBLIGATORIOS PARA EVITAR NULLS
   ========================================================= */
INSERT INTO dbo.Provincia (CodigoProvincia, Nombre) VALUES (0 + 1, 'San José');
INSERT INTO dbo.Canton (CodigoCanton, CodigoProvincia, Nombre) VALUES (101, 1, 'San José');
INSERT INTO dbo.Distrito (CodigoDistrito, CodigoCanton, Nombre) VALUES (10101, 101, 'Carmen');
GO

INSERT INTO dbo.Rol (Nombre, Descripcion) VALUES ('Administrador', 'Rol administrador general');
INSERT INTO dbo.Puesto (Nombre, Descripcion) VALUES ('Administrador', 'Puesto administrador general');
INSERT INTO dbo.TipoTelefono (Nombre) VALUES ('General');
INSERT INTO dbo.TipoCorreo (Nombre) VALUES ('General');
INSERT INTO dbo.TipoPago (Nombre) VALUES ('Efectivo');
INSERT INTO dbo.TipoFactura (TipoFactura, Descripcion) VALUES ('Contado', 'Factura de contado');
INSERT INTO dbo.TipoDeObservacion (TipoDeObservacion, Descripcion) VALUES ('General', 'Observación general');
INSERT INTO dbo.TipoGasto (Nombre, Descripcion) VALUES ('General', 'Gasto general');
INSERT INTO dbo.TipoIngreso (Nombre, Descripcion) VALUES ('General', 'Ingreso general');
INSERT INTO dbo.TipoCuentaContable (Nombre, Descripcion) VALUES ('General', 'Tipo general');
INSERT INTO dbo.NaturalezaCuentaContable (Naturaleza, Descripcion) VALUES ('General', 'Naturaleza general');
INSERT INTO dbo.TipoProducto (Nombre, Descripcion) VALUES ('General', 'Producto general');
INSERT INTO dbo.Impuesto (Nombre, Porcentaje, Descripcion) VALUES ('Sin impuesto', 0, 'Impuesto general 0%');
INSERT INTO dbo.Descuento (Nombre, Porcentaje, RequiereAutorizacion, Descripcion) VALUES ('Sin descuento', 0, 0, 'Descuento general 0%');
GO

INSERT INTO dbo.Persona
(
    Identificacion, Nombre, PrimerApellido, SegundoApellido, FechaNacimiento, CodigoDistrito, DireccionExacta
)
VALUES
('000000000', 'Usuario', 'General', '', '19000101', 10101, 'Dirección general');

INSERT INTO dbo.Cliente
(
    Identificacion, CodigoCliente, LimiteCredito, DiasCredito
)
VALUES
('000000000', 'CLI-GENERAL', 0, 0);

INSERT INTO dbo.Empleado
(
    Identificacion, CodigoEmpleado, IdPuesto, FechaIngreso, IdRol, NombreUsuario, ClaveHash, RestablecerClave
)
VALUES
('000000000', 'EMP-GENERAL', 1, GETDATE(), 1, 'admin', 'HASH_PENDIENTE', 1);

INSERT INTO dbo.Proveedor
(
    IdentificacionProveedor, RazonSocial, NombreContacto, CodigoDistrito, DireccionExacta, DiasCredito
)
VALUES
('000000000', 'PROVEEDOR GENERAL', 'Contacto general', 10101, 'Dirección general', 0);
GO

INSERT INTO dbo.PeriodoContable (Anio, Mes, FechaInicio, FechaFin, Estado)
VALUES (1900, 1, '19000101', '19000131', 'General');
GO

INSERT INTO dbo.CuentaContable
(
    CodigoCuenta, NombreCuenta, IdTipoCuentaContable, IdNaturalezaCuentaContable, AceptaMovimientos, Activo, CodigoCuentaPadre
)
VALUES
('0000', 'CUENTA RAIZ', 1, 1, 0, 1, '0000');
GO

INSERT INTO dbo.Producto
(
    CodigoProducto, NombreProducto, IdTipoProducto, IdImpuesto, Descripcion, PrecioVenta, StockActual
)
VALUES
('PROD-GENERAL', 'PRODUCTO GENERAL', 1, 1, 'Producto general para relaciones obligatorias', 0, 0);
GO

INSERT INTO dbo.Factura
(
    NumeroFactura, IdentificacionCliente, IdentificacionEmpleado, IdTipoPago, IdTipoFactura,
    FechaFactura, Subtotal, TotalImpuesto, TotalDescuento, TotalFactura, Estado
)
VALUES
('FAC-GENERAL-000000', '000000000', '000000000', 1, 1, GETDATE(), 0, 0, 0, 0, 'General');
GO

INSERT INTO dbo.CuentaPorCobrar
(
    IdentificacionCliente, NumeroFactura, FechaEmision, FechaVencimiento, MontoOriginal, SaldoActual, Estado
)
VALUES
('000000000', 'FAC-GENERAL-000000', GETDATE(), GETDATE(), 0, 0, 'General');

INSERT INTO dbo.AbonoCuentaPorCobrar
(
    IdentificacionCliente, NumeroFactura, NumeroAbono, FechaAbono, MontoAbono, Observacion
)
VALUES
('000000000', 'FAC-GENERAL-000000', 0, GETDATE(), 0, 'Abono general');
GO

INSERT INTO dbo.CuentaPorPagar
(
    IdentificacionProveedor, NumeroDocumento, FechaEmision, FechaVencimiento, Concepto, MontoOriginal, SaldoActual, Estado
)
VALUES
('000000000', 'CXP-GENERAL-000000', GETDATE(), GETDATE(), 'Cuenta por pagar general', 0, 0, 'General');

INSERT INTO dbo.AbonoCuentaPorPagar
(
    IdentificacionProveedor, NumeroDocumento, NumeroAbono, FechaAbono, MontoAbono, Observacion
)
VALUES
('000000000', 'CXP-GENERAL-000000', 0, GETDATE(), 0, 'Abono general');
GO

/* =========================================================
   ÍNDICES ADICIONALES PARA CONSULTA
   ========================================================= */
CREATE INDEX IX_Canton_CodigoProvincia ON dbo.Canton(CodigoProvincia);
CREATE INDEX IX_Distrito_CodigoCanton ON dbo.Distrito(CodigoCanton);
CREATE INDEX IX_Persona_CodigoDistrito ON dbo.Persona(CodigoDistrito);
CREATE INDEX IX_Proveedor_CodigoDistrito ON dbo.Proveedor(CodigoDistrito);
CREATE INDEX IX_Empleado_IdRol ON dbo.Empleado(IdRol);
CREATE INDEX IX_Empleado_IdPuesto ON dbo.Empleado(IdPuesto);
CREATE INDEX IX_Telefono_Identificacion ON dbo.Telefono(Identificacion);
CREATE INDEX IX_Correo_Identificacion ON dbo.Correo(Identificacion);
CREATE INDEX IX_TelefonoProveedor_IdentificacionProveedor ON dbo.TelefonoProveedor(IdentificacionProveedor);
CREATE INDEX IX_CorreoProveedor_IdentificacionProveedor ON dbo.CorreoProveedor(IdentificacionProveedor);
CREATE INDEX IX_CuentaContable_Tipo ON dbo.CuentaContable(IdTipoCuentaContable);
CREATE INDEX IX_CuentaContable_Naturaleza ON dbo.CuentaContable(IdNaturalezaCuentaContable);
CREATE INDEX IX_CuentaContable_Padre ON dbo.CuentaContable(CodigoCuentaPadre);
CREATE INDEX IX_AsientoContable_Periodo ON dbo.AsientoContable(Anio, Mes);
CREATE INDEX IX_DetalleAsientoContable_CodigoCuenta ON dbo.DetalleAsientoContable(CodigoCuenta);
CREATE INDEX IX_Producto_Tipo ON dbo.Producto(IdTipoProducto);
CREATE INDEX IX_Producto_Impuesto ON dbo.Producto(IdImpuesto);
CREATE INDEX IX_Factura_Cliente ON dbo.Factura(IdentificacionCliente);
CREATE INDEX IX_Factura_Empleado ON dbo.Factura(IdentificacionEmpleado);
CREATE INDEX IX_Factura_TipoPago ON dbo.Factura(IdTipoPago);
CREATE INDEX IX_Factura_TipoFactura ON dbo.Factura(IdTipoFactura);
CREATE INDEX IX_DetalleFactura_CodigoProducto ON dbo.DetalleFactura(CodigoProducto);
CREATE INDEX IX_DetalleFactura_IdImpuesto ON dbo.DetalleFactura(IdImpuesto);
CREATE INDEX IX_DetalleFactura_IdDescuento ON dbo.DetalleFactura(IdDescuento);
CREATE INDEX IX_NotaCredito_NumeroFactura ON dbo.NotaCredito(NumeroFactura);
CREATE INDEX IX_CuentaPorCobrar_NumeroFactura ON dbo.CuentaPorCobrar(NumeroFactura);
CREATE INDEX IX_AbonoCuentaPorCobrar_Cuenta ON dbo.AbonoCuentaPorCobrar(IdentificacionCliente, NumeroFactura);
CREATE INDEX IX_CuentaPorPagar_Proveedor ON dbo.CuentaPorPagar(IdentificacionProveedor);
CREATE INDEX IX_AbonoCuentaPorPagar_Cuenta ON dbo.AbonoCuentaPorPagar(IdentificacionProveedor, NumeroDocumento);
CREATE INDEX IX_Ingreso_TipoIngreso ON dbo.Ingreso(IdTipoIngreso);
CREATE INDEX IX_Ingreso_CodigoCuenta ON dbo.Ingreso(CodigoCuenta);
CREATE INDEX IX_Ingreso_Factura ON dbo.Ingreso(IdentificacionCliente, NumeroFactura);
CREATE INDEX IX_Ingreso_AbonoCXC ON dbo.Ingreso(IdentificacionClienteAbono, NumeroFacturaAbono, NumeroAbonoCuentaPorCobrar);
CREATE INDEX IX_Gasto_TipoGasto ON dbo.Gasto(IdTipoGasto);
CREATE INDEX IX_Gasto_CodigoCuenta ON dbo.Gasto(CodigoCuenta);
CREATE INDEX IX_Gasto_CuentaPorPagar ON dbo.Gasto(IdentificacionProveedor, NumeroDocumento);
CREATE INDEX IX_Gasto_AbonoCXP ON dbo.Gasto(IdentificacionProveedorAbono, NumeroDocumentoAbono, NumeroAbonoCuentaPorPagar);
CREATE INDEX IX_DetPresupuesto_CodigoCuenta ON dbo.DetallePresupuestoMensual(CodigoCuenta);


CREATE INDEX IX_HistorialCambio_Identificacion ON dbo.HistorialCambio(Identificacion);
CREATE INDEX IX_HistorialCambio_TablaRegistro ON dbo.HistorialCambio(NombreTabla, IdRegistro);
GO
