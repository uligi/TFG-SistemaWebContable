/* =========================================================
   CREACIÓN DE BASE DE DATOS
   ========================================================= */
IF DB_ID('SistemaWebContableGraditas') IS NULL
BEGIN
    CREATE DATABASE SistemaWebContableGraditas;
END
GO

USE SistemaWebContableGraditas;
GO

/* =========================================================
   LIMPIEZA OPCIONAL SI NECESITAS RECREAR
   =========================================================
-- DROP TABLE dbo.HistorialCambio;x
-- DROP TABLE dbo.IntentoAcceso;x
-- DROP TABLE dbo.DetallePresupuestoMensual;x
-- DROP TABLE dbo.PresupuestoMensual;x
-- DROP TABLE dbo.AbonoCuentaPorPagar;x
-- DROP TABLE dbo.CuentaPorPagar;
-- DROP TABLE dbo.AbonoCuentaPorCobrar;
-- DROP TABLE dbo.CuentaPorCobrar;
-- DROP TABLE dbo.NotaCredito;
-- DROP TABLE dbo.DetalleFactura;
-- DROP TABLE dbo.Factura;
-- DROP TABLE dbo.DetalleAsientoContable;
-- DROP TABLE dbo.AsientoContable;
-- DROP TABLE dbo.Ingreso;
-- DROP TABLE dbo.Gasto;
-- DROP TABLE dbo.Producto;
-- DROP TABLE dbo.PeriodoContable;
-- DROP TABLE dbo.CuentaContable;
-- DROP TABLE dbo.CorreoProveedor;
-- DROP TABLE dbo.TelefonoProveedor;
-- DROP TABLE dbo.Proveedor;
-- DROP TABLE dbo.Correo;
-- DROP TABLE dbo.Telefono;
-- DROP TABLE dbo.Empleado;
-- DROP TABLE dbo.Cliente;
-- DROP TABLE dbo.Persona;
-- DROP TABLE dbo.TipoDeObservacion;
-- DROP TABLE dbo.TipoFactura;
-- DROP TABLE dbo.TipoPago;
-- DROP TABLE dbo.TipoCorreo;
-- DROP TABLE dbo.TipoTelefono;
-- DROP TABLE dbo.Rol;
-- DROP TABLE dbo.Distrito;
-- DROP TABLE dbo.Canton;
-- DROP TABLE dbo.Provincia;
   ========================================================= */

/* =========================================================
   TABLAS DE UBICACIÓN
   ========================================================= */
CREATE TABLE dbo.Provincia
(
    IdProvincia      INT IDENTITY(1,1) NOT NULL,
    Nombre           VARCHAR(45) NOT NULL,
    Activo           BIT NOT NULL CONSTRAINT DF_Provincia_Activo DEFAULT (1),
    CONSTRAINT PK_Provincia PRIMARY KEY (IdProvincia),
    CONSTRAINT UQ_Provincia_Nombre UNIQUE (Nombre)
);
GO

CREATE TABLE dbo.Canton
(
    IdCanton         INT IDENTITY(1,1) NOT NULL,
    IdProvincia      INT NOT NULL,
    Nombre           VARCHAR(45) NOT NULL,
    Activo           BIT NOT NULL CONSTRAINT DF_Canton_Activo DEFAULT (1),
    CONSTRAINT PK_Canton PRIMARY KEY (IdCanton),
    CONSTRAINT FK_Canton_Provincia
        FOREIGN KEY (IdProvincia) REFERENCES dbo.Provincia(IdProvincia)
);
GO

CREATE TABLE dbo.Distrito
(
    IdDistrito       INT IDENTITY(1,1) NOT NULL,
    IdCanton         INT NOT NULL,
    Nombre           VARCHAR(45) NOT NULL,
    Activo           BIT NOT NULL CONSTRAINT DF_Distrito_Activo DEFAULT (1),
    CONSTRAINT PK_Distrito PRIMARY KEY (IdDistrito),
    CONSTRAINT FK_Distrito_Canton
        FOREIGN KEY (IdCanton) REFERENCES dbo.Canton(IdCanton)
);
GO

/* =========================================================
   CATÁLOGOS
   ========================================================= */
CREATE TABLE dbo.Rol
(
    IdRol            INT IDENTITY(1,1) NOT NULL,
    Nombre           VARCHAR(45) NOT NULL,
    Descripcion      VARCHAR(150) NULL,
    Activo           BIT NOT NULL CONSTRAINT DF_Rol_Activo DEFAULT (1),
    CONSTRAINT PK_Rol PRIMARY KEY (IdRol),
    CONSTRAINT UQ_Rol_Nombre UNIQUE (Nombre)
);
GO

CREATE TABLE dbo.TipoTelefono
(
    IdTipoTelefono   INT IDENTITY(1,1) NOT NULL,
    Nombre           VARCHAR(45) NOT NULL,
    Activo           BIT NOT NULL CONSTRAINT DF_TipoTelefono_Activo DEFAULT (1),
    CONSTRAINT PK_TipoTelefono PRIMARY KEY (IdTipoTelefono),
    CONSTRAINT UQ_TipoTelefono_Nombre UNIQUE (Nombre)
);
GO

CREATE TABLE dbo.TipoCorreo
(
    IdTipoCorreo     INT IDENTITY(1,1) NOT NULL,
    Nombre           VARCHAR(45) NOT NULL,
    Activo           BIT NOT NULL CONSTRAINT DF_TipoCorreo_Activo DEFAULT (1),
    CONSTRAINT PK_TipoCorreo PRIMARY KEY (IdTipoCorreo),
    CONSTRAINT UQ_TipoCorreo_Nombre UNIQUE (Nombre)
);
GO

CREATE TABLE dbo.TipoPago
(
    IdTipoPago       INT IDENTITY(1,1) NOT NULL,
    Nombre           VARCHAR(45) NOT NULL,
    Activo           BIT NOT NULL CONSTRAINT DF_TipoPago_Activo DEFAULT (1),
    CONSTRAINT PK_TipoPago PRIMARY KEY (IdTipoPago),
    CONSTRAINT UQ_TipoPago_Nombre UNIQUE (Nombre)
);
GO

CREATE TABLE dbo.TipoFactura
(
    IdTipoFactura    INT IDENTITY(1,1) NOT NULL,
    TipoFactura      VARCHAR(45) NOT NULL,
    Descripcion      VARCHAR(150) NULL,
    Activo           BIT NOT NULL CONSTRAINT DF_TipoFactura_Activo DEFAULT (1),
    CONSTRAINT PK_TipoFactura PRIMARY KEY (IdTipoFactura),
    CONSTRAINT UQ_TipoFactura_Tipo UNIQUE (TipoFactura)
);
GO

CREATE TABLE dbo.TipoDeObservacion
(
    IdTipoDeObservacion INT IDENTITY(1,1) NOT NULL,
    TipoDeObservacion   VARCHAR(45) NOT NULL,
    Descripcion         VARCHAR(150) NULL,
    Activo              BIT NOT NULL CONSTRAINT DF_TipoDeObservacion_Activo DEFAULT (1),
    CONSTRAINT PK_TipoDeObservacion PRIMARY KEY (IdTipoDeObservacion),
    CONSTRAINT UQ_TipoDeObservacion_Tipo UNIQUE (TipoDeObservacion)
);
GO
/* =========================================================
   CATÁLOGOS ADICIONALES
   ========================================================= */

CREATE TABLE dbo.Puesto
(
    IdPuesto     INT IDENTITY(1,1) NOT NULL,
    Nombre       VARCHAR(45) NOT NULL,
    Descripcion  VARCHAR(255) NOT NULL,
    Activo       BIT NOT NULL CONSTRAINT DF_Puesto_Activo DEFAULT (1),
    CONSTRAINT PK_Puesto PRIMARY KEY (IdPuesto),
    CONSTRAINT UQ_Puesto_Nombre UNIQUE (Nombre)
);
GO

CREATE TABLE dbo.NaturalezaCuentaContable
(
    IdNaturalezaCuentaContable INT IDENTITY(1,1) NOT NULL,
    Naturaleza                 VARCHAR(45) NOT NULL,
    Descripcion                VARCHAR(255) NOT NULL,
    Activo                     BIT NOT NULL CONSTRAINT DF_NaturalezaCuentaContable_Activo DEFAULT (1),
    CONSTRAINT PK_NaturalezaCuentaContable PRIMARY KEY (IdNaturalezaCuentaContable),
    CONSTRAINT UQ_NaturalezaCuentaContable_Naturaleza UNIQUE (Naturaleza)
);
GO

CREATE TABLE dbo.TipoGasto
(
    IdTipoGasto INT IDENTITY(1,1) NOT NULL,
    Nombre      VARCHAR(45) NOT NULL,
    Descripcion VARCHAR(255) NOT NULL,
    Activo      BIT NOT NULL CONSTRAINT DF_TipoGasto_Activo DEFAULT (1),
    CONSTRAINT PK_TipoGasto PRIMARY KEY (IdTipoGasto),
    CONSTRAINT UQ_TipoGasto_Nombre UNIQUE (Nombre)
);
GO

CREATE TABLE dbo.TipoIngreso
(
    IdTipoIngreso INT IDENTITY(1,1) NOT NULL,
    Nombre        VARCHAR(45) NOT NULL,
    Descripcion   VARCHAR(255) NOT NULL,
    Activo        BIT NOT NULL CONSTRAINT DF_TipoIngreso_Activo DEFAULT (1),
    CONSTRAINT PK_TipoIngreso PRIMARY KEY (IdTipoIngreso),
    CONSTRAINT UQ_TipoIngreso_Nombre UNIQUE (Nombre)
);
GO

CREATE TABLE dbo.TipoCuentaContable
(
    IdTipoCuentaContable INT IDENTITY(1,1) NOT NULL,
    Nombre               VARCHAR(45) NOT NULL,
    Descripcion          VARCHAR(255) NOT NULL,
    Activo               BIT NOT NULL CONSTRAINT DF_TipoCuentaContable_Activo DEFAULT (1),
    CONSTRAINT PK_TipoCuentaContable PRIMARY KEY (IdTipoCuentaContable),
    CONSTRAINT UQ_TipoCuentaContable_Nombre UNIQUE (Nombre)
);
GO

/* =========================================================
   PERSONAS, CLIENTES, EMPLEADOS
   ========================================================= */
CREATE TABLE dbo.Persona
(
    Identificacion   VARCHAR(45) NOT NULL,
    Nombre           VARCHAR(45) NOT NULL,
    PrimerApellido   VARCHAR(45) NOT NULL,
    SegundoApellido  VARCHAR(45) NULL,
    FechaNacimiento  DATE NULL,
    IdDistrito       INT NULL,
    DireccionExacta  VARCHAR(250) NULL,
    FechaCreacion    DATETIME NOT NULL CONSTRAINT DF_Persona_FechaCreacion DEFAULT (GETDATE()),
    FechaModificacion DATETIME NULL,
    Activo           BIT NOT NULL CONSTRAINT DF_Persona_Activo DEFAULT (1),
    CONSTRAINT PK_Persona PRIMARY KEY (Identificacion),
    CONSTRAINT FK_Persona_Distrito
        FOREIGN KEY (IdDistrito) REFERENCES dbo.Distrito(IdDistrito)
);
GO

CREATE TABLE dbo.Cliente
(
    Identificacion   VARCHAR(45) NOT NULL,
    CodigoCliente    VARCHAR(45) NOT NULL,
    LimiteCredito    DECIMAL(18,2) NULL,
    DiasCredito      INT NULL,
    FechaCreacion    DATETIME NOT NULL CONSTRAINT DF_Cliente_FechaCreacion DEFAULT (GETDATE()),
    FechaModificacion DATETIME NULL,
    Activo           BIT NOT NULL CONSTRAINT DF_Cliente_Activo DEFAULT (1),
    CONSTRAINT PK_Cliente PRIMARY KEY (Identificacion),
    CONSTRAINT UQ_Cliente_CodigoCliente UNIQUE (CodigoCliente),
    CONSTRAINT FK_Cliente_Persona
        FOREIGN KEY (Identificacion) REFERENCES dbo.Persona(Identificacion)
);
GO

CREATE TABLE dbo.Empleado
(
    Identificacion    VARCHAR(45) NOT NULL,
    CodigoEmpleado    VARCHAR(45) NOT NULL,
    IdPuesto          INT NULL,
    FechaIngreso      DATE NULL,
    IdRol             INT NOT NULL,
    NombreUsuario     VARCHAR(45) NOT NULL,
    ClaveHash         VARCHAR(255) NOT NULL,
    UltimoAcceso      DATETIME NULL,
    FechaCreacion     DATETIME NOT NULL CONSTRAINT DF_Empleado_FechaCreacion DEFAULT (GETDATE()),
    FechaModificacion DATETIME NULL,
    RestablecerClave  BIT NOT NULL CONSTRAINT DF_Empleado_RestablecerClave DEFAULT (1),
    Activo            BIT NOT NULL CONSTRAINT DF_Empleado_Activo DEFAULT (1),
    CONSTRAINT PK_Empleado PRIMARY KEY (Identificacion),
    CONSTRAINT UQ_Empleado_CodigoEmpleado UNIQUE (CodigoEmpleado),
    CONSTRAINT UQ_Empleado_NombreUsuario UNIQUE (NombreUsuario),
    CONSTRAINT FK_Empleado_Persona
        FOREIGN KEY (Identificacion) REFERENCES dbo.Persona(Identificacion),
    CONSTRAINT FK_Empleado_Rol
        FOREIGN KEY (IdRol) REFERENCES dbo.Rol(IdRol),
    CONSTRAINT FK_Empleado_Puesto
        FOREIGN KEY (IdPuesto) REFERENCES dbo.Puesto(IdPuesto)
);
GO

/* =========================================================
   CONTACTO PERSONA
   ========================================================= */
CREATE TABLE dbo.Telefono
(
    NumeroTelefono   VARCHAR(45) NOT NULL,
    Identificacion   VARCHAR(45) NOT NULL,
    IdTipoTelefono   INT NOT NULL,
    EsPrincipal      BIT NOT NULL CONSTRAINT DF_Telefono_EsPrincipal DEFAULT (0),
    Activo           BIT NOT NULL CONSTRAINT DF_Telefono_Activo DEFAULT (1),
    CONSTRAINT PK_Telefono PRIMARY KEY (NumeroTelefono, Identificacion),
    CONSTRAINT FK_Telefono_Persona
        FOREIGN KEY (Identificacion) REFERENCES dbo.Persona(Identificacion),
    CONSTRAINT FK_Telefono_TipoTelefono
        FOREIGN KEY (IdTipoTelefono) REFERENCES dbo.TipoTelefono(IdTipoTelefono)
);
GO

CREATE TABLE dbo.Correo
(
    Identificacion   VARCHAR(45) NOT NULL,
    DireccionCorreo  VARCHAR(255) NOT NULL,
    IdTipoCorreo     INT NOT NULL,
    EsPrincipal      BIT NOT NULL CONSTRAINT DF_Correo_EsPrincipal DEFAULT (0),
    Activo           BIT NOT NULL CONSTRAINT DF_Correo_Activo DEFAULT (1),
    CONSTRAINT PK_Correo PRIMARY KEY (Identificacion, DireccionCorreo),
    CONSTRAINT FK_Correo_Persona
        FOREIGN KEY (Identificacion) REFERENCES dbo.Persona(Identificacion),
    CONSTRAINT FK_Correo_TipoCorreo
        FOREIGN KEY (IdTipoCorreo) REFERENCES dbo.TipoCorreo(IdTipoCorreo)
);
GO

/* =========================================================
   PROVEEDORES Y CONTACTO
   ========================================================= */
CREATE TABLE dbo.Proveedor
(
    IdentificacionProveedor VARCHAR(45) NOT NULL,
    RazonSocial             VARCHAR(200) NOT NULL,
    NombreContacto          VARCHAR(100) NULL,
    IdDistrito              INT NULL,
    DireccionExacta         VARCHAR(250) NULL,
    DiasCredito             INT NULL,
    FechaCreacion           DATETIME NOT NULL CONSTRAINT DF_Proveedor_FechaCreacion DEFAULT (GETDATE()),
    FechaModificacion       DATETIME NULL,
    Activo                  BIT NOT NULL CONSTRAINT DF_Proveedor_Activo DEFAULT (1),
    CONSTRAINT PK_Proveedor PRIMARY KEY (IdentificacionProveedor),
    CONSTRAINT FK_Proveedor_Distrito
        FOREIGN KEY (IdDistrito) REFERENCES dbo.Distrito(IdDistrito)
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
    CONSTRAINT FK_TelefonoProveedor_Proveedor
        FOREIGN KEY (IdentificacionProveedor) REFERENCES dbo.Proveedor(IdentificacionProveedor),
    CONSTRAINT FK_TelefonoProveedor_TipoTelefono
        FOREIGN KEY (IdTipoTelefono) REFERENCES dbo.TipoTelefono(IdTipoTelefono)
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
    CONSTRAINT FK_CorreoProveedor_Proveedor
        FOREIGN KEY (IdentificacionProveedor) REFERENCES dbo.Proveedor(IdentificacionProveedor),
    CONSTRAINT FK_CorreoProveedor_TipoCorreo
        FOREIGN KEY (IdTipoCorreo) REFERENCES dbo.TipoCorreo(IdTipoCorreo)
);
GO

/* =========================================================
   CONTABILIDAD
   ========================================================= */
CREATE TABLE dbo.PeriodoContable
(
    IdPeriodoContable INT IDENTITY(1,1) NOT NULL,
    Anio              INT NOT NULL,
    Mes               INT NOT NULL,
    FechaInicio       DATETIME NOT NULL,
    FechaFin          DATETIME NOT NULL,
    Estado            VARCHAR(45) NOT NULL,
    Activo            BIT NOT NULL CONSTRAINT DF_PeriodoContable_Activo DEFAULT (1),
    CONSTRAINT PK_PeriodoContable PRIMARY KEY (IdPeriodoContable)
);
GO

CREATE TABLE dbo.CuentaContable
(
    IdCuentaContable  INT IDENTITY(1,1) NOT NULL,
    CodigoCuenta      VARCHAR(45) NOT NULL,
    NombreCuenta      VARCHAR(100) NOT NULL,
    IdTipoCuentaContable INT NOT NULL,
    IdNaturalezaCuentaContable INT NOT NULL,
    AceptaMovimientos BIT NOT NULL CONSTRAINT DF_CuentaContable_AceptaMov DEFAULT (1),
    Activo            BIT NOT NULL CONSTRAINT DF_CuentaContable_Activo DEFAULT (1),
    IdCuentaPadre     INT NULL,
    CONSTRAINT PK_CuentaContable PRIMARY KEY (IdCuentaContable),
    CONSTRAINT UQ_CuentaContable_Codigo UNIQUE (CodigoCuenta),
    CONSTRAINT FK_CuentaContable_TipoCuentaContable
    FOREIGN KEY (IdTipoCuentaContable) REFERENCES dbo.TipoCuentaContable(IdTipoCuentaContable),
    CONSTRAINT FK_CuentaContable_NaturalezaCuentaContable
    FOREIGN KEY (IdNaturalezaCuentaContable) REFERENCES dbo.NaturalezaCuentaContable(IdNaturalezaCuentaContable),
    CONSTRAINT FK_CuentaContable_CuentaPadre
        FOREIGN KEY (IdCuentaPadre) REFERENCES dbo.CuentaContable(IdCuentaContable)
);
GO

CREATE TABLE dbo.ConfiguracionCuentaContable
(
    IdConfiguracionCuentaContable INT IDENTITY(1,1) PRIMARY KEY,
    CodigoOperacion VARCHAR(45) NOT NULL,
    NombreOperacion VARCHAR(100) NOT NULL,
    Descripcion VARCHAR(200) NULL,
    IdCuentaContable INT NOT NULL,
    FechaCreacion DATETIME NOT NULL DEFAULT GETDATE(),
    FechaModificacion DATETIME NULL,
    Activo BIT NOT NULL DEFAULT 1,

    CONSTRAINT FK_ConfiguracionCuentaContable_CuentaContable
    FOREIGN KEY (IdCuentaContable)
    REFERENCES dbo.CuentaContable(IdCuentaContable)
);
GO

CREATE UNIQUE INDEX UX_ConfiguracionCuentaContable_CodigoOperacion_Activo
ON dbo.ConfiguracionCuentaContable(CodigoOperacion)
WHERE Activo = 1;
GO

CREATE TABLE dbo.AsientoContable
(
    IdAsientoContable INT IDENTITY(1,1) NOT NULL,
    IdPeriodoContable INT NOT NULL,
    NumeroAsiento     VARCHAR(45) NOT NULL,
    FechaAsiento      DATETIME NOT NULL,
    TipoAsiento       VARCHAR(45) NOT NULL,
    Concepto          VARCHAR(45) NOT NULL,
    TotalDebe         DECIMAL(18,2) NOT NULL,
    TotalHaber        DECIMAL(18,2) NOT NULL,
    Activo            BIT NOT NULL CONSTRAINT DF_AsientoContable_Activo DEFAULT (1),
    CONSTRAINT PK_AsientoContable PRIMARY KEY (IdAsientoContable),
    CONSTRAINT UQ_AsientoContable_Numero UNIQUE (NumeroAsiento),
    CONSTRAINT FK_AsientoContable_PeriodoContable
        FOREIGN KEY (IdPeriodoContable) REFERENCES dbo.PeriodoContable(IdPeriodoContable)
);
GO

CREATE TABLE dbo.DetalleAsientoContable
(
    IdDetalleAsientoContable INT IDENTITY(1,1) NOT NULL,
    IdAsientoContable        INT NOT NULL,
    IdCuentaContable         INT NOT NULL,
    Debe                     DECIMAL(18,2) NOT NULL CONSTRAINT DF_DetalleAsientoContable_Debe DEFAULT (0),
    Haber                    DECIMAL(18,2) NOT NULL CONSTRAINT DF_DetalleAsientoContable_Haber DEFAULT (0),
    DescripcionLinea         VARCHAR(150) NULL,
    Activo                   BIT NOT NULL CONSTRAINT DF_DetalleAsientoContable_Activo DEFAULT (1),
    CONSTRAINT PK_DetalleAsientoContable PRIMARY KEY (IdDetalleAsientoContable),
    CONSTRAINT FK_DetalleAsientoContable_Asiento
        FOREIGN KEY (IdAsientoContable) REFERENCES dbo.AsientoContable(IdAsientoContable),
    CONSTRAINT FK_DetalleAsientoContable_Cuenta
        FOREIGN KEY (IdCuentaContable) REFERENCES dbo.CuentaContable(IdCuentaContable)
);
GO


/* =========================================================
   PRODUCTOS
   ========================================================= */

CREATE TABLE dbo.TipoProducto
(
    IdTipoProducto   INT IDENTITY(1,1) NOT NULL,
    Nombre           VARCHAR(100) NOT NULL,
    Descripcion      VARCHAR(150) NULL,
    Activo           BIT NOT NULL CONSTRAINT DF_TipoProducto_Activo DEFAULT (1),
    CONSTRAINT PK_TipoProducto PRIMARY KEY (IdTipoProducto),
    CONSTRAINT UQ_TipoProducto_Nombre UNIQUE (Nombre)
);
GO

CREATE TABLE dbo.Impuesto
(
    IdImpuesto     INT IDENTITY(1,1) NOT NULL,
    Nombre         VARCHAR(100) NOT NULL,
    Porcentaje     DECIMAL(5,2) NOT NULL,
    Descripcion    VARCHAR(255) NULL,
    Activo         BIT NOT NULL CONSTRAINT DF_Impuesto_Activo DEFAULT (1),
    CONSTRAINT PK_Impuesto PRIMARY KEY (IdImpuesto),
    CONSTRAINT UQ_Impuesto_Nombre UNIQUE (Nombre),
    CONSTRAINT CK_Impuesto_Porcentaje CHECK (Porcentaje >= 0)
);
GO

CREATE TABLE dbo.Descuento
(
    IdDescuento             INT IDENTITY(1,1) NOT NULL,
    Nombre                  VARCHAR(100) NOT NULL,
    Porcentaje              DECIMAL(5,2) NOT NULL,
    RequiereAutorizacion    BIT NOT NULL CONSTRAINT DF_Descuento_RequiereAutorizacion DEFAULT (0),
    Descripcion             VARCHAR(255) NULL,
    Activo                  BIT NOT NULL CONSTRAINT DF_Descuento_Activo DEFAULT (1),
    CONSTRAINT PK_Descuento PRIMARY KEY (IdDescuento),
    CONSTRAINT UQ_Descuento_Nombre UNIQUE (Nombre),
    CONSTRAINT CK_Descuento_Porcentaje CHECK (Porcentaje >= 0 AND Porcentaje <= 100)
);
GO

CREATE TABLE dbo.Producto
(
    IdProducto         INT IDENTITY(1,1) NOT NULL,
    CodigoProducto     VARCHAR(45) NOT NULL,
    NombreProducto     VARCHAR(150) NOT NULL,
    IdTipoProducto     INT NOT NULL,
    IdImpuesto         INT NOT NULL,
    Descripcion        VARCHAR(250) NULL,
    PrecioVenta        DECIMAL(18,2) NOT NULL,
    StockActual        DECIMAL(18,2) NOT NULL CONSTRAINT DF_Producto_StockActual DEFAULT (0),
    FechaCreacion      DATETIME NOT NULL CONSTRAINT DF_Producto_FechaCreacion DEFAULT (GETDATE()),
    FechaModificacion  DATETIME NULL,
    Activo             BIT NOT NULL CONSTRAINT DF_Producto_Activo DEFAULT (1),
    CONSTRAINT PK_Producto PRIMARY KEY (IdProducto),
    CONSTRAINT UQ_Producto_Codigo UNIQUE (CodigoProducto),
    CONSTRAINT FK_Producto_TipoProducto
        FOREIGN KEY (IdTipoProducto) REFERENCES dbo.TipoProducto(IdTipoProducto),
    CONSTRAINT FK_Producto_Impuesto
        FOREIGN KEY (IdImpuesto) REFERENCES dbo.Impuesto(IdImpuesto),
    CONSTRAINT CK_Producto_PrecioVenta CHECK (PrecioVenta >= 0),
    CONSTRAINT CK_Producto_StockActual CHECK (StockActual >= 0)
);
GO

/* =========================================================
   FACTURACIÓN
   ========================================================= */
CREATE TABLE dbo.Factura
(
    IdFactura               INT IDENTITY(1,1) NOT NULL,
    IdentificacionCliente   VARCHAR(45) NOT NULL,
    IdentificacionEmpleado  VARCHAR(45) NOT NULL,
    NumeroFactura           VARCHAR(45) NOT NULL,
    IdTipoPago              INT NOT NULL,
    IdTipoFactura           INT NOT NULL,
    FechaFactura            DATETIME NOT NULL,
    Subtotal                DECIMAL(18,2) NOT NULL,
    TotalImpuesto           DECIMAL(18,2) NOT NULL,
    TotalDescuento          DECIMAL(18,2) NOT NULL,
    TotalFactura            DECIMAL(18,2) NOT NULL,
    Estado                  VARCHAR(45) NOT NULL,
    FechaCreacion           DATETIME NOT NULL CONSTRAINT DF_Factura_FechaCreacion DEFAULT (GETDATE()),
    FechaModificacion       DATETIME NULL,
    Activo                  BIT NOT NULL CONSTRAINT DF_Factura_Activo DEFAULT (1),
    CONSTRAINT PK_Factura PRIMARY KEY (IdFactura),
    CONSTRAINT UQ_Factura_Numero UNIQUE (NumeroFactura),
    CONSTRAINT FK_Factura_Cliente
        FOREIGN KEY (IdentificacionCliente) REFERENCES dbo.Cliente(Identificacion),
    CONSTRAINT FK_Factura_Empleado
        FOREIGN KEY (IdentificacionEmpleado) REFERENCES dbo.Empleado(Identificacion),
    CONSTRAINT FK_Factura_TipoPago
        FOREIGN KEY (IdTipoPago) REFERENCES dbo.TipoPago(IdTipoPago),
    CONSTRAINT FK_Factura_TipoFactura
        FOREIGN KEY (IdTipoFactura) REFERENCES dbo.TipoFactura(IdTipoFactura)
);
GO

CREATE TABLE dbo.DetalleFactura
(
    IdDetalleFactura        INT IDENTITY(1,1) NOT NULL,
    IdFactura               INT NOT NULL,
    IdProducto              INT NOT NULL,
    IdImpuesto              INT NOT NULL,
    IdDescuento             INT NULL,
    DescripcionItem         VARCHAR(150) NULL,
    Cantidad                DECIMAL(18,2) NOT NULL,
    PrecioUnitario          DECIMAL(18,2) NOT NULL,
    PorcentajeImpuesto      DECIMAL(5,2) NOT NULL CONSTRAINT DF_DetalleFactura_PorcImp DEFAULT (0),
    PorcentajeDescuento     DECIMAL(5,2) NOT NULL CONSTRAINT DF_DetalleFactura_PorcDesc DEFAULT (0),
    SubtotalLinea           DECIMAL(18,2) NOT NULL,
    TotalLinea              DECIMAL(18,2) NOT NULL,
    FechaCreacion           DATETIME NOT NULL CONSTRAINT DF_DetalleFactura_FechaCreacion DEFAULT (GETDATE()),
    FechaModificacion       DATETIME NULL,
    Activo                  BIT NOT NULL CONSTRAINT DF_DetalleFactura_Activo DEFAULT (1),
    CONSTRAINT PK_DetalleFactura PRIMARY KEY (IdDetalleFactura),
    CONSTRAINT FK_DetalleFactura_Factura
        FOREIGN KEY (IdFactura) REFERENCES dbo.Factura(IdFactura),
    CONSTRAINT FK_DetalleFactura_Producto
        FOREIGN KEY (IdProducto) REFERENCES dbo.Producto(IdProducto),
    CONSTRAINT FK_DetalleFactura_Impuesto
        FOREIGN KEY (IdImpuesto) REFERENCES dbo.Impuesto(IdImpuesto),
    CONSTRAINT FK_DetalleFactura_Descuento
        FOREIGN KEY (IdDescuento) REFERENCES dbo.Descuento(IdDescuento),
    CONSTRAINT CK_DetalleFactura_Cantidad CHECK (Cantidad > 0),
    CONSTRAINT CK_DetalleFactura_PrecioUnitario CHECK (PrecioUnitario >= 0),
    CONSTRAINT CK_DetalleFactura_PorcImp CHECK (PorcentajeImpuesto >= 0),
    CONSTRAINT CK_DetalleFactura_PorcDesc CHECK (PorcentajeDescuento >= 0 AND PorcentajeDescuento <= 100)
);
GO

CREATE TABLE dbo.NotaCredito
(
    NumeroNotaCredito   VARCHAR(45) NOT NULL,
    IdFactura           INT NOT NULL,
    FechaNotaCredito    DATETIME NOT NULL,
    Motivo              VARCHAR(250) NOT NULL,
    Subtotal            DECIMAL(18,2) NOT NULL,
    TotalImpuesto       DECIMAL(18,2) NOT NULL,
    TotalNotaCredito    DECIMAL(18,2) NOT NULL,
    Estado              VARCHAR(45) NOT NULL,
    FechaCreacion       DATETIME NOT NULL CONSTRAINT DF_NotaCredito_FechaCreacion DEFAULT (GETDATE()),
    FechaModificacion   DATETIME NULL,
    Activo              BIT NOT NULL CONSTRAINT DF_NotaCredito_Activo DEFAULT (1),
    CONSTRAINT PK_NotaCredito PRIMARY KEY (NumeroNotaCredito),
    CONSTRAINT FK_NotaCredito_Factura
        FOREIGN KEY (IdFactura) REFERENCES dbo.Factura(IdFactura)
);
GO

/* =========================================================
   INGRESOS Y GASTOS
   ========================================================= */
CREATE TABLE dbo.Ingreso
(
    IdIngreso           INT IDENTITY(1,1) NOT NULL,
    IdFactura           INT NULL,
    FechaIngreso        DATETIME NOT NULL,
    Descripcion         VARCHAR(150) NOT NULL,
    IdTipoIngreso       INT NULL,
    IdCuentaContable    INT NULL,
    IdAbonoCuentaPorCobrar INT NULL,
    OrigenIngreso       VARCHAR(45) NULL,
    Monto               DECIMAL(18,2) NOT NULL,
    FechaCreacion       DATETIME NOT NULL CONSTRAINT DF_Ingreso_FechaCreacion DEFAULT (GETDATE()),
    FechaModificacion   DATETIME NULL,
    Activo              BIT NOT NULL CONSTRAINT DF_Ingreso_Activo DEFAULT (1),
    CONSTRAINT PK_Ingreso PRIMARY KEY (IdIngreso),
    CONSTRAINT FK_Ingreso_Factura
        FOREIGN KEY (IdFactura) REFERENCES dbo.Factura(IdFactura),
    CONSTRAINT FK_Ingreso_TipoIngreso
        FOREIGN KEY (IdTipoIngreso) REFERENCES dbo.TipoIngreso(IdTipoIngreso),
    CONSTRAINT FK_Ingreso_AbonoCuentaPorCobrar
        FOREIGN KEY (IdAbonoCuentaPorCobrar) REFERENCES dbo.AbonoCuentaPorCobrar(IdAbonoCuentaPorCobrar),
        CONSTRAINT FK_Ingreso_CuentaContable
FOREIGN KEY (IdCuentaContable) REFERENCES dbo.CuentaContable(IdCuentaContable)
);
GO




CREATE TABLE dbo.Gasto
(
    IdGasto                    INT IDENTITY(1,1) NOT NULL,
    FechaGasto                 DATETIME NOT NULL,
    Descripcion                VARCHAR(150) NOT NULL,
    IdTipoGasto                INT NULL,
    Monto                      DECIMAL(18,2) NOT NULL,
    IdentificacionProveedor    VARCHAR(45) NULL,
    IdCuentaContable           INT NULL,
    IdAbonoCuentaPorPagar      INT NULL,
    NumeroComprobante          VARCHAR(45) NULL,
    NombreArchivoComprobante   VARCHAR(150) NULL,
    RutaComprobante            VARCHAR(300) NULL,
    FechaCreacion              DATETIME NOT NULL CONSTRAINT DF_Gasto_FechaCreacion DEFAULT (GETDATE()),
    FechaModificacion          DATETIME NULL,
    Activo                     BIT NOT NULL CONSTRAINT DF_Gasto_Activo DEFAULT (1),
    CONSTRAINT PK_Gasto PRIMARY KEY (IdGasto),
    CONSTRAINT FK_Gasto_Proveedor
        FOREIGN KEY (IdentificacionProveedor) REFERENCES dbo.Proveedor(IdentificacionProveedor),
    CONSTRAINT FK_Gasto_TipoGasto
        FOREIGN KEY (IdTipoGasto) REFERENCES dbo.TipoGasto(IdTipoGasto),
        CONSTRAINT FK_Gasto_AbonoCuentaPorPagar
FOREIGN KEY (IdAbonoCuentaPorPagar) REFERENCES dbo.AbonoCuentaPorPagar(IdAbonoCuentaPorPagar),
CONSTRAINT FK_Gasto_CuentaContable
FOREIGN KEY (IdCuentaContable) REFERENCES dbo.CuentaContable(IdCuentaContable)
);
GO



/* =========================================================
   CUENTAS POR COBRAR
   ========================================================= */
CREATE TABLE dbo.CuentaPorCobrar
(
    IdCuentaPorCobrar      INT IDENTITY(1,1) NOT NULL,
    IdentificacionCliente  VARCHAR(45) NOT NULL,
    IdFactura              INT NOT NULL,
    FechaEmision           DATETIME NOT NULL,
    FechaVencimiento       DATETIME NOT NULL,
    MontoOriginal          DECIMAL(18,2) NOT NULL,
    SaldoActual            DECIMAL(18,2) NOT NULL,
    Estado                 VARCHAR(45) NOT NULL,
    FechaCreacion          DATETIME NOT NULL CONSTRAINT DF_CuentaPorCobrar_FechaCreacion DEFAULT (GETDATE()),
    FechaModificacion      DATETIME NULL,
    Activo                 BIT NOT NULL CONSTRAINT DF_CuentaPorCobrar_Activo DEFAULT (1),
    CONSTRAINT PK_CuentaPorCobrar PRIMARY KEY (IdCuentaPorCobrar),
    CONSTRAINT FK_CuentaPorCobrar_Cliente
        FOREIGN KEY (IdentificacionCliente) REFERENCES dbo.Cliente(Identificacion),
    CONSTRAINT FK_CuentaPorCobrar_Factura
        FOREIGN KEY (IdFactura) REFERENCES dbo.Factura(IdFactura)
);
GO

CREATE TABLE dbo.AbonoCuentaPorCobrar
(
    IdAbonoCuentaPorCobrar INT IDENTITY(1,1) NOT NULL,
    IdCuentaPorCobrar      INT NOT NULL,
    FechaAbono             DATETIME NOT NULL,
    MontoAbono             DECIMAL(18,2) NOT NULL,
    Observacion            VARCHAR(150) NULL,
    FechaCreacion          DATETIME NOT NULL CONSTRAINT DF_AbonoCxC_FechaCreacion DEFAULT (GETDATE()),
    FechaModificacion      DATETIME NULL,
    Activo                 BIT NOT NULL CONSTRAINT DF_AbonoCxC_Activo DEFAULT (1),
    CONSTRAINT PK_AbonoCuentaPorCobrar PRIMARY KEY (IdAbonoCuentaPorCobrar),
    CONSTRAINT FK_AbonoCuentaPorCobrar_Cuenta
        FOREIGN KEY (IdCuentaPorCobrar) REFERENCES dbo.CuentaPorCobrar(IdCuentaPorCobrar)
);
GO

/* =========================================================
   CUENTAS POR PAGAR
   ========================================================= */
CREATE TABLE dbo.CuentaPorPagar
(
    IdCuentaPorPagar       INT IDENTITY(1,1) NOT NULL,
    IdentificacionProveedor VARCHAR(45) NOT NULL,
    NumeroDocumento        VARCHAR(45) NOT NULL,
    FechaEmision           DATETIME NOT NULL,
    FechaVencimiento       DATETIME NOT NULL,
    Concepto               VARCHAR(45) NOT NULL,
    MontoOriginal          DECIMAL(18,2) NOT NULL,
    SaldoActual            DECIMAL(18,2) NOT NULL,
    Estado                 VARCHAR(45) NOT NULL,
    FechaCreacion          DATETIME NOT NULL CONSTRAINT DF_CuentaPorPagar_FechaCreacion DEFAULT (GETDATE()),
    FechaModificacion      DATETIME NULL,
    Activo                 BIT NOT NULL CONSTRAINT DF_CuentaPorPagar_Activo DEFAULT (1),
    CONSTRAINT PK_CuentaPorPagar PRIMARY KEY (IdCuentaPorPagar),
    CONSTRAINT FK_CuentaPorPagar_Proveedor
        FOREIGN KEY (IdentificacionProveedor) REFERENCES dbo.Proveedor(IdentificacionProveedor)
);
GO

CREATE TABLE dbo.AbonoCuentaPorPagar
(
    IdAbonoCuentaPorPagar INT IDENTITY(1,1) NOT NULL,
    IdCuentaPorPagar      INT NOT NULL,
    FechaAbono            DATETIME NOT NULL,
    MontoAbono            DECIMAL(18,2) NOT NULL,
    Observacion           VARCHAR(150) NULL,
    FechaCreacion         DATETIME NOT NULL CONSTRAINT DF_AbonoCxP_FechaCreacion DEFAULT (GETDATE()),
    FechaModificacion     DATETIME NULL,
    Activo                BIT NOT NULL CONSTRAINT DF_AbonoCxP_Activo DEFAULT (1),
    CONSTRAINT PK_AbonoCuentaPorPagar PRIMARY KEY (IdAbonoCuentaPorPagar),
    CONSTRAINT FK_AbonoCuentaPorPagar_Cuenta
        FOREIGN KEY (IdCuentaPorPagar) REFERENCES dbo.CuentaPorPagar(IdCuentaPorPagar)
);
GO

/* =========================================================
   PRESUPUESTO
   ========================================================= */
CREATE TABLE dbo.PresupuestoMensual
(
    IdPresupuestoMensual  INT IDENTITY(1,1) NOT NULL,
    Anio                  INT NOT NULL,
    Mes                   INT NOT NULL,
    Estado                VARCHAR(45) NOT NULL,
    FechaCreacion         DATETIME NOT NULL CONSTRAINT DF_PresupuestoMensual_FechaCreacion DEFAULT (GETDATE()),
    FechaModificacion     DATETIME NULL,
    Activo                BIT NOT NULL CONSTRAINT DF_PresupuestoMensual_Activo DEFAULT (1),
    CONSTRAINT PK_PresupuestoMensual PRIMARY KEY (IdPresupuestoMensual),
    CONSTRAINT UQ_PresupuestoMensual_Periodo UNIQUE (Anio, Mes)
);
GO

CREATE TABLE dbo.DetallePresupuestoMensual
(
    IdDetallePresupuestoMensual INT IDENTITY(1,1) NOT NULL,
    IdPresupuestoMensual        INT NOT NULL,
    IdCuentaContable            INT NOT NULL,
    TipoMovimiento              VARCHAR(45) NOT NULL,
    MontoPresupuestado          DECIMAL(18,2) NOT NULL,
    FechaCreacion               DATETIME NOT NULL CONSTRAINT DF_DetPresupuesto_FechaCreacion DEFAULT (GETDATE()),
    FechaModificacion           DATETIME NULL,
    Activo                      BIT NOT NULL CONSTRAINT DF_DetPresupuesto_Activo DEFAULT (1),
    CONSTRAINT PK_DetallePresupuestoMensual PRIMARY KEY (IdDetallePresupuestoMensual),
    CONSTRAINT FK_DetallePresupuestoMensual_Presupuesto
        FOREIGN KEY (IdPresupuestoMensual) REFERENCES dbo.PresupuestoMensual(IdPresupuestoMensual),
    CONSTRAINT FK_DetallePresupuestoMensual_Cuenta
        FOREIGN KEY (IdCuentaContable) REFERENCES dbo.CuentaContable(IdCuentaContable)
);
GO

/* =========================================================
   SEGURIDAD Y AUDITORÍA
   ========================================================= */
CREATE TABLE dbo.IntentoAcceso
(
    IdIntentoAcceso      INT IDENTITY(1,1) NOT NULL,
    NombreUsuario        VARCHAR(45) NOT NULL,
    FechaHoraIntento     DATETIME NOT NULL CONSTRAINT DF_IntentoAcceso_Fecha DEFAULT (GETDATE()),
    Exitoso              BIT NOT NULL,
    IPEquipo             VARCHAR(50) NULL,
    Observacion          VARCHAR(200) NULL,
    Identificacion       VARCHAR(45) NULL,
    CONSTRAINT PK_IntentoAcceso PRIMARY KEY (IdIntentoAcceso),
    CONSTRAINT FK_IntentoAcceso_Empleado
        FOREIGN KEY (Identificacion) REFERENCES dbo.Empleado(Identificacion)
);
GO

CREATE TABLE dbo.HistorialCambio
(
    IdHistorialCambio    INT IDENTITY(1,1) NOT NULL,
    Identificacion       VARCHAR(45) NULL,
    NombreTabla          VARCHAR(100) NOT NULL,
    IdTipoDeObservacion  INT NOT NULL,
    FechaHoraCambio      DATETIME NOT NULL CONSTRAINT DF_HistorialCambio_Fecha DEFAULT (GETDATE()),
    Detalle              VARCHAR(300) NULL,
    ValorAnterior        VARCHAR(900) NULL,
    ValorNuevo           VARCHAR(900) NULL,
    Activo               BIT NOT NULL CONSTRAINT DF_HistorialCambio_Activo DEFAULT (1),
    IdRegistro           INT NOT NULL,
    CONSTRAINT PK_HistorialCambio PRIMARY KEY (IdHistorialCambio),
    CONSTRAINT FK_HistorialCambio_Empleado
        FOREIGN KEY (Identificacion) REFERENCES dbo.Empleado(Identificacion),
    CONSTRAINT FK_HistorialCambio_TipoDeObservacion
        FOREIGN KEY (IdTipoDeObservacion) REFERENCES dbo.TipoDeObservacion(IdTipoDeObservacion)
);
GO

/* =========================================================
   ÍNDICES ADICIONALES
   ========================================================= */
CREATE INDEX IX_Canton_IdProvincia ON dbo.Canton(IdProvincia);
CREATE INDEX IX_Distrito_IdCanton ON dbo.Distrito(IdCanton);
CREATE INDEX IX_Persona_IdDistrito ON dbo.Persona(IdDistrito);
CREATE INDEX IX_Empleado_IdRol ON dbo.Empleado(IdRol);

CREATE INDEX IX_Telefono_Identificacion ON dbo.Telefono(Identificacion);
CREATE INDEX IX_Correo_Identificacion ON dbo.Correo(Identificacion);

CREATE INDEX IX_Proveedor_IdDistrito ON dbo.Proveedor(IdDistrito);
CREATE INDEX IX_TelefonoProveedor_IdentificacionProveedor ON dbo.TelefonoProveedor(IdentificacionProveedor);
CREATE INDEX IX_CorreoProveedor_IdentificacionProveedor ON dbo.CorreoProveedor(IdentificacionProveedor);

CREATE INDEX IX_CuentaContable_IdCuentaPadre ON dbo.CuentaContable(IdCuentaPadre);
CREATE INDEX IX_AsientoContable_IdPeriodoContable ON dbo.AsientoContable(IdPeriodoContable);
CREATE INDEX IX_DetalleAsientoContable_IdAsientoContable ON dbo.DetalleAsientoContable(IdAsientoContable);
CREATE INDEX IX_DetalleAsientoContable_IdCuentaContable ON dbo.DetalleAsientoContable(IdCuentaContable);

CREATE INDEX IX_Factura_IdentificacionCliente ON dbo.Factura(IdentificacionCliente);
CREATE INDEX IX_Factura_IdentificacionEmpleado ON dbo.Factura(IdentificacionEmpleado);
CREATE INDEX IX_Factura_IdTipoPago ON dbo.Factura(IdTipoPago);
CREATE INDEX IX_Factura_IdTipoFactura ON dbo.Factura(IdTipoFactura);

CREATE INDEX IX_DetalleFactura_IdFactura ON dbo.DetalleFactura(IdFactura);
CREATE INDEX IX_DetalleFactura_IdProducto ON dbo.DetalleFactura(IdProducto);

CREATE INDEX IX_NotaCredito_IdFactura ON dbo.NotaCredito(IdFactura);

CREATE INDEX IX_Ingreso_IdFactura ON dbo.Ingreso(IdFactura);
CREATE INDEX IX_Gasto_IdentificacionProveedor ON dbo.Gasto(IdentificacionProveedor);

CREATE INDEX IX_CuentaPorCobrar_IdentificacionCliente ON dbo.CuentaPorCobrar(IdentificacionCliente);
CREATE INDEX IX_CuentaPorCobrar_IdFactura ON dbo.CuentaPorCobrar(IdFactura);
CREATE INDEX IX_AbonoCuentaPorCobrar_IdCuentaPorCobrar ON dbo.AbonoCuentaPorCobrar(IdCuentaPorCobrar);

CREATE INDEX IX_CuentaPorPagar_IdentificacionProveedor ON dbo.CuentaPorPagar(IdentificacionProveedor);
CREATE INDEX IX_AbonoCuentaPorPagar_IdCuentaPorPagar ON dbo.AbonoCuentaPorPagar(IdCuentaPorPagar);

CREATE INDEX IX_DetallePresupuestoMensual_IdPresupuestoMensual ON dbo.DetallePresupuestoMensual(IdPresupuestoMensual);
CREATE INDEX IX_DetallePresupuestoMensual_IdCuentaContable ON dbo.DetallePresupuestoMensual(IdCuentaContable);

CREATE INDEX IX_IntentoAcceso_Identificacion ON dbo.IntentoAcceso(Identificacion);
CREATE INDEX IX_IntentoAcceso_FechaHoraIntento ON dbo.IntentoAcceso(FechaHoraIntento);

CREATE INDEX IX_HistorialCambio_Identificacion ON dbo.HistorialCambio(Identificacion);
CREATE INDEX IX_HistorialCambio_IdTipoDeObservacion ON dbo.HistorialCambio(IdTipoDeObservacion);
CREATE INDEX IX_HistorialCambio_NombreTabla_IdRegistro ON dbo.HistorialCambio(NombreTabla, IdRegistro);
CREATE INDEX IX_Empleado_IdPuesto ON dbo.Empleado(IdPuesto);
CREATE INDEX IX_CuentaContable_IdTipoCuentaContable ON dbo.CuentaContable(IdTipoCuentaContable);
CREATE INDEX IX_CuentaContable_IdNaturalezaCuentaContable ON dbo.CuentaContable(IdNaturalezaCuentaContable);
CREATE INDEX IX_Ingreso_IdTipoIngreso ON dbo.Ingreso(IdTipoIngreso);
CREATE UNIQUE INDEX UX_ConfiguracionCuentaContable_CodigoOperacion_Activo
ON dbo.ConfiguracionCuentaContable(CodigoOperacion)
WHERE Activo = 1;
GO
CREATE INDEX IX_Gasto_IdTipoGasto ON dbo.Gasto(IdTipoGasto);
GO