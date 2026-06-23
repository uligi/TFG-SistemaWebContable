DECLARE @Resultado INT;
DECLARE @Mensaje VARCHAR(500);

EXEC Catalogos.sp_NaturalezaCuentaContable_Registrar
    @Naturaleza = 'Deudora',
    @Descripcion = 'Naturaleza utilizada para cuentas que aumentan por el Debe y disminuyen por el Haber.',
    @Resultado = @Resultado OUTPUT,
    @Mensaje = @Mensaje OUTPUT;

EXEC Catalogos.sp_NaturalezaCuentaContable_Registrar
    @Naturaleza = 'Acreedora',
    @Descripcion = 'Naturaleza utilizada para cuentas que aumentan por el Haber y disminuyen por el Debe.',
    @Resultado = @Resultado OUTPUT,
    @Mensaje = @Mensaje OUTPUT;

    DECLARE @Resultado INT;
DECLARE @Mensaje VARCHAR(500);

EXEC Catalogos.sp_TipoCuentaContable_Registrar
    @Nombre = 'Activo',
    @Descripcion = 'Agrupa los recursos económicos que posee el negocio, como caja, bancos, inventario y cuentas por cobrar.',
    @Resultado = @Resultado OUTPUT,
    @Mensaje = @Mensaje OUTPUT;

EXEC Catalogos.sp_TipoCuentaContable_Registrar
    @Nombre = 'Pasivo',
    @Descripcion = 'Agrupa las obligaciones económicas del negocio, como cuentas por pagar, préstamos y deudas pendientes.',
    @Resultado = @Resultado OUTPUT,
    @Mensaje = @Mensaje OUTPUT;

EXEC Catalogos.sp_TipoCuentaContable_Registrar
    @Nombre = 'Patrimonio',
    @Descripcion = 'Agrupa las cuentas relacionadas con el capital, aportes y resultados acumulados del negocio.',
    @Resultado = @Resultado OUTPUT,
    @Mensaje = @Mensaje OUTPUT;

EXEC Catalogos.sp_TipoCuentaContable_Registrar
    @Nombre = 'Ingreso',
    @Descripcion = 'Agrupa las entradas económicas generadas por ventas, servicios u otros ingresos del negocio.',
    @Resultado = @Resultado OUTPUT,
    @Mensaje = @Mensaje OUTPUT;

EXEC Catalogos.sp_TipoCuentaContable_Registrar
    @Nombre = 'Gasto',
    @Descripcion = 'Agrupa las salidas económicas necesarias para la operación del negocio, como servicios públicos, compras y gastos administrativos.',
    @Resultado = @Resultado OUTPUT,
    @Mensaje = @Mensaje OUTPUT;



DECLARE @Resultado INT;
DECLARE @Mensaje VARCHAR(500);

EXEC Contabilidad.sp_CuentaContable_Registrar
    @CodigoCuenta = '1',
    @NombreCuenta = 'Activos',
    @IdTipoCuentaContable = 1,
    @IdNaturalezaCuentaContable = 1,
    @AceptaMovimientos = 0,
    @IdCuentaPadre = 0,
    @Resultado = @Resultado OUTPUT,
    @Mensaje = @Mensaje OUTPUT;

EXEC Contabilidad.sp_CuentaContable_Registrar
    @CodigoCuenta = '2',
    @NombreCuenta = 'Pasivos',
    @IdTipoCuentaContable = 2,
    @IdNaturalezaCuentaContable = 2,
    @AceptaMovimientos = 0,
    @IdCuentaPadre = 0,
    @Resultado = @Resultado OUTPUT,
    @Mensaje = @Mensaje OUTPUT;

EXEC Contabilidad.sp_CuentaContable_Registrar
    @CodigoCuenta = '3',
    @NombreCuenta = 'Patrimonio',
    @IdTipoCuentaContable = 3,
    @IdNaturalezaCuentaContable = 2,
    @AceptaMovimientos = 0,
    @IdCuentaPadre = 0,
    @Resultado = @Resultado OUTPUT,
    @Mensaje = @Mensaje OUTPUT;



















EXEC Contabilidad.sp_CuentaContable_Registrar
    @CodigoCuenta = '4',
    @NombreCuenta = 'Ingresos',
    @IdTipoCuentaContable = 4,
    @IdNaturalezaCuentaContable = 2,
    @AceptaMovimientos = 0,
    @IdCuentaPadre = 0,
    @Resultado = @Resultado OUTPUT,
    @Mensaje = @Mensaje OUTPUT;

EXEC Contabilidad.sp_CuentaContable_Registrar
    @CodigoCuenta = '5',
    @NombreCuenta = 'Gastos',
    @IdTipoCuentaContable = 5,
    @IdNaturalezaCuentaContable = 1,
    @AceptaMovimientos = 0,
    @IdCuentaPadre = 0,
    @Resultado = @Resultado OUTPUT,
    @Mensaje = @Mensaje OUTPUT;