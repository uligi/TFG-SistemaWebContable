DECLARE @Resultado INT;
DECLARE @Mensaje VARCHAR(500);

---------------------------------------------------------
-- TIPO PRODUCTO
---------------------------------------------------------

EXEC Catalogos.sp_TipoProducto_Registrar 
    @Nombre = 'Verduras',
    @Descripcion = 'Productos agrícolas frescos como tomate, papa, cebolla, chile dulce y culantro.',
    @Resultado = @Resultado OUTPUT,
    @Mensaje = @Mensaje OUTPUT;

EXEC Catalogos.sp_TipoProducto_Registrar 
    @Nombre = 'Frutas',
    @Descripcion = 'Frutas frescas nacionales o importadas vendidas por unidad o kilogramo.',
    @Resultado = @Resultado OUTPUT,
    @Mensaje = @Mensaje OUTPUT;

EXEC Catalogos.sp_TipoProducto_Registrar 
    @Nombre = 'Abarrotes',
    @Descripcion = 'Productos básicos de consumo diario como arroz, frijoles, azúcar, sal, aceite y enlatados.',
    @Resultado = @Resultado OUTPUT,
    @Mensaje = @Mensaje OUTPUT;

EXEC Catalogos.sp_TipoProducto_Registrar 
    @Nombre = 'Lácteos',
    @Descripcion = 'Productos derivados de la leche como leche, queso, natilla, yogurt y mantequilla.',
    @Resultado = @Resultado OUTPUT,
    @Mensaje = @Mensaje OUTPUT;

EXEC Catalogos.sp_TipoProducto_Registrar 
    @Nombre = 'Bebidas',
    @Descripcion = 'Bebidas embotelladas, gaseosas, jugos, agua y bebidas energéticas.',
    @Resultado = @Resultado OUTPUT,
    @Mensaje = @Mensaje OUTPUT;

EXEC Catalogos.sp_TipoProducto_Registrar 
    @Nombre = 'Panadería',
    @Descripcion = 'Productos de panadería, tortillas, repostería empacada y galletas.',
    @Resultado = @Resultado OUTPUT,
    @Mensaje = @Mensaje OUTPUT;

EXEC Catalogos.sp_TipoProducto_Registrar 
    @Nombre = 'Limpieza',
    @Descripcion = 'Productos para limpieza del hogar o negocio como cloro, detergente y desinfectante.',
    @Resultado = @Resultado OUTPUT,
    @Mensaje = @Mensaje OUTPUT;

EXEC Catalogos.sp_TipoProducto_Registrar 
    @Nombre = 'Higiene personal',
    @Descripcion = 'Productos de cuidado personal como jabón, pasta dental, papel higiénico y champú.',
    @Resultado = @Resultado OUTPUT,
    @Mensaje = @Mensaje OUTPUT;

EXEC Catalogos.sp_TipoProducto_Registrar 
    @Nombre = 'Ferretería',
    @Descripcion = 'Productos básicos de ferretería como clavos, tornillos, cinta aislante, brochas y bombillos.',
    @Resultado = @Resultado OUTPUT,
    @Mensaje = @Mensaje OUTPUT;

EXEC Catalogos.sp_TipoProducto_Registrar 
    @Nombre = 'Jardinería',
    @Descripcion = 'Productos para plantas, jardín y mantenimiento de áreas verdes.',
    @Resultado = @Resultado OUTPUT,
    @Mensaje = @Mensaje OUTPUT;

EXEC Catalogos.sp_TipoProducto_Registrar 
    @Nombre = 'Mascotas',
    @Descripcion = 'Productos para animales domésticos como alimentos, arena y accesorios básicos.',
    @Resultado = @Resultado OUTPUT,
    @Mensaje = @Mensaje OUTPUT;

EXEC Catalogos.sp_TipoProducto_Registrar 
    @Nombre = 'Congelados',
    @Descripcion = 'Productos que requieren conservación en frío o congelación.',
    @Resultado = @Resultado OUTPUT,
    @Mensaje = @Mensaje OUTPUT;

EXEC Catalogos.sp_TipoProducto_Registrar 
    @Nombre = 'Snacks',
    @Descripcion = 'Productos de consumo rápido como papas, chocolates, confites y bocadillos.',
    @Resultado = @Resultado OUTPUT,
    @Mensaje = @Mensaje OUTPUT;

EXEC Catalogos.sp_TipoProducto_Registrar 
    @Nombre = 'Otros',
    @Descripcion = 'Productos que no pertenecen a una categoría específica.',
    @Resultado = @Resultado OUTPUT,
    @Mensaje = @Mensaje OUTPUT;


---------------------------------------------------------
-- TIPO PAGO
---------------------------------------------------------

EXEC Catalogos.sp_TipoPago_Registrar 
    @Nombre = 'Efectivo',
    @Resultado = @Resultado OUTPUT,
    @Mensaje = @Mensaje OUTPUT;

EXEC Catalogos.sp_TipoPago_Registrar 
    @Nombre = 'Tarjeta de débito',
    @Resultado = @Resultado OUTPUT,
    @Mensaje = @Mensaje OUTPUT;

EXEC Catalogos.sp_TipoPago_Registrar 
    @Nombre = 'Tarjeta de crédito',
    @Resultado = @Resultado OUTPUT,
    @Mensaje = @Mensaje OUTPUT;

EXEC Catalogos.sp_TipoPago_Registrar 
    @Nombre = 'SINPE Móvil',
    @Resultado = @Resultado OUTPUT,
    @Mensaje = @Mensaje OUTPUT;

EXEC Catalogos.sp_TipoPago_Registrar 
    @Nombre = 'Transferencia bancaria',
    @Resultado = @Resultado OUTPUT,
    @Mensaje = @Mensaje OUTPUT;

EXEC Catalogos.sp_TipoPago_Registrar 
    @Nombre = 'Crédito interno',
    @Resultado = @Resultado OUTPUT,
    @Mensaje = @Mensaje OUTPUT;

EXEC Catalogos.sp_TipoPago_Registrar 
    @Nombre = 'Mixto',
    @Resultado = @Resultado OUTPUT,
    @Mensaje = @Mensaje OUTPUT;


---------------------------------------------------------
-- TIPO FACTURA
---------------------------------------------------------

EXEC Catalogos.sp_TipoFactura_Registrar 
    @TipoFactura = 'Contado',
    @Descripcion = 'Factura generada cuando el cliente paga la totalidad de la compra al momento de la venta.',
    @Resultado = @Resultado OUTPUT,
    @Mensaje = @Mensaje OUTPUT;

EXEC Catalogos.sp_TipoFactura_Registrar 
    @TipoFactura = 'Crédito',
    @Descripcion = 'Factura generada cuando la venta queda pendiente de pago total o parcial.',
    @Resultado = @Resultado OUTPUT,
    @Mensaje = @Mensaje OUTPUT;

EXEC Catalogos.sp_TipoFactura_Registrar 
    @TipoFactura = 'Electrónica',
    @Descripcion = 'Factura emitida en formato digital para respaldar una venta registrada.',
    @Resultado = @Resultado OUTPUT,
    @Mensaje = @Mensaje OUTPUT;

EXEC Catalogos.sp_TipoFactura_Registrar 
    @TipoFactura = 'Física',
    @Descripcion = 'Factura emitida en papel como respaldo de la venta realizada.',
    @Resultado = @Resultado OUTPUT,
    @Mensaje = @Mensaje OUTPUT;

EXEC Catalogos.sp_TipoFactura_Registrar 
    @TipoFactura = 'Proforma',
    @Descripcion = 'Documento preliminar con detalle estimado antes de generar la factura definitiva.',
    @Resultado = @Resultado OUTPUT,
    @Mensaje = @Mensaje OUTPUT;

EXEC Catalogos.sp_TipoFactura_Registrar 
    @TipoFactura = 'Nota de crédito',
    @Descripcion = 'Documento utilizado para corregir, anular o disminuir el monto de una factura.',
    @Resultado = @Resultado OUTPUT,
    @Mensaje = @Mensaje OUTPUT;

EXEC Catalogos.sp_TipoFactura_Registrar 
    @TipoFactura = 'Tiquete electrónico',
    @Descripcion = 'Comprobante de venta utilizado para transacciones de consumo final.',
    @Resultado = @Resultado OUTPUT,
    @Mensaje = @Mensaje OUTPUT;


---------------------------------------------------------
-- TIPO DE OBSERVACIÓN
---------------------------------------------------------

EXEC Catalogos.sp_TipoDeObservacion_Registrar 
    @TipoDeObservacion = 'Inserción',
    @Descripcion = 'Registro creado por el usuario dentro del sistema.',
    @Resultado = @Resultado OUTPUT,
    @Mensaje = @Mensaje OUTPUT;

EXEC Catalogos.sp_TipoDeObservacion_Registrar 
    @TipoDeObservacion = 'Modificación',
    @Descripcion = 'Registro actualizado por el usuario dentro del sistema.',
    @Resultado = @Resultado OUTPUT,
    @Mensaje = @Mensaje OUTPUT;

EXEC Catalogos.sp_TipoDeObservacion_Registrar 
    @TipoDeObservacion = 'Eliminación lógica',
    @Descripcion = 'Registro desactivado sin ser eliminado físicamente de la base de datos.',
    @Resultado = @Resultado OUTPUT,
    @Mensaje = @Mensaje OUTPUT;

EXEC Catalogos.sp_TipoDeObservacion_Registrar 
    @TipoDeObservacion = 'Reactivación',
    @Descripcion = 'Registro previamente inactivo que fue habilitado nuevamente.',
    @Resultado = @Resultado OUTPUT,
    @Mensaje = @Mensaje OUTPUT;

EXEC Catalogos.sp_TipoDeObservacion_Registrar 
    @TipoDeObservacion = 'Inicio de sesión',
    @Descripcion = 'Registro relacionado con el acceso exitoso de un usuario.',
    @Resultado = @Resultado OUTPUT,
    @Mensaje = @Mensaje OUTPUT;

EXEC Catalogos.sp_TipoDeObservacion_Registrar 
    @TipoDeObservacion = 'Intento fallido',
    @Descripcion = 'Registro relacionado con un intento de acceso no exitoso.',
    @Resultado = @Resultado OUTPUT,
    @Mensaje = @Mensaje OUTPUT;

EXEC Catalogos.sp_TipoDeObservacion_Registrar 
    @TipoDeObservacion = 'Cambio de estado',
    @Descripcion = 'Actualización del estado de una factura, cuenta, presupuesto u otro registro.',
    @Resultado = @Resultado OUTPUT,
    @Mensaje = @Mensaje OUTPUT;


---------------------------------------------------------
-- TIPO INGRESO
---------------------------------------------------------

EXEC Catalogos.sp_TipoIngreso_Registrar 
    @Nombre = 'Venta al contado',
    @Descripcion = 'Ingreso generado por ventas pagadas al momento de la facturación.',
    @Resultado = @Resultado OUTPUT,
    @Mensaje = @Mensaje OUTPUT;

EXEC Catalogos.sp_TipoIngreso_Registrar 
    @Nombre = 'Abono de cliente',
    @Descripcion = 'Ingreso generado por un abono realizado a una cuenta por cobrar.',
    @Resultado = @Resultado OUTPUT,
    @Mensaje = @Mensaje OUTPUT;

EXEC Catalogos.sp_TipoIngreso_Registrar 
    @Nombre = 'Otro ingreso',
    @Descripcion = 'Ingreso no relacionado directamente con una venta ordinaria.',
    @Resultado = @Resultado OUTPUT,
    @Mensaje = @Mensaje OUTPUT;


---------------------------------------------------------
-- TIPO GASTO
---------------------------------------------------------

EXEC Catalogos.sp_TipoGasto_Registrar 
    @Nombre = 'Servicios públicos',
    @Descripcion = 'Gastos por electricidad, agua, internet, telefonía u otros servicios básicos.',
    @Resultado = @Resultado OUTPUT,
    @Mensaje = @Mensaje OUTPUT;

EXEC Catalogos.sp_TipoGasto_Registrar 
    @Nombre = 'Compra de mercadería',
    @Descripcion = 'Gastos asociados a la compra de productos para la venta.',
    @Resultado = @Resultado OUTPUT,
    @Mensaje = @Mensaje OUTPUT;

EXEC Catalogos.sp_TipoGasto_Registrar 
    @Nombre = 'Salarios',
    @Descripcion = 'Gastos relacionados con el pago de colaboradores.',
    @Resultado = @Resultado OUTPUT,
    @Mensaje = @Mensaje OUTPUT;

EXEC Catalogos.sp_TipoGasto_Registrar 
    @Nombre = 'Alquiler',
    @Descripcion = 'Gasto por alquiler del local o instalaciones.',
    @Resultado = @Resultado OUTPUT,
    @Mensaje = @Mensaje OUTPUT;

EXEC Catalogos.sp_TipoGasto_Registrar 
    @Nombre = 'Transporte',
    @Descripcion = 'Gastos por transporte de productos, entregas o gestiones del negocio.',
    @Resultado = @Resultado OUTPUT,
    @Mensaje = @Mensaje OUTPUT;

EXEC Catalogos.sp_TipoGasto_Registrar 
    @Nombre = 'Mantenimiento',
    @Descripcion = 'Gastos por reparación o mantenimiento del local, equipo o mobiliario.',
    @Resultado = @Resultado OUTPUT,
    @Mensaje = @Mensaje OUTPUT;


---------------------------------------------------------
-- TIPO CUENTA CONTABLE
---------------------------------------------------------

EXEC Catalogos.sp_TipoCuentaContable_Registrar 
    @Nombre = 'Activo',
    @Descripcion = 'Cuentas que representan bienes, derechos y recursos controlados por el negocio.',
    @Resultado = @Resultado OUTPUT,
    @Mensaje = @Mensaje OUTPUT;

EXEC Catalogos.sp_TipoCuentaContable_Registrar 
    @Nombre = 'Pasivo',
    @Descripcion = 'Cuentas que representan obligaciones o deudas del negocio.',
    @Resultado = @Resultado OUTPUT,
    @Mensaje = @Mensaje OUTPUT;

EXEC Catalogos.sp_TipoCuentaContable_Registrar 
    @Nombre = 'Patrimonio',
    @Descripcion = 'Cuentas que representan el capital y los resultados acumulados del negocio.',
    @Resultado = @Resultado OUTPUT,
    @Mensaje = @Mensaje OUTPUT;

EXEC Catalogos.sp_TipoCuentaContable_Registrar 
    @Nombre = 'Ingreso',
    @Descripcion = 'Cuentas que representan entradas económicas generadas por la operación.',
    @Resultado = @Resultado OUTPUT,
    @Mensaje = @Mensaje OUTPUT;

EXEC Catalogos.sp_TipoCuentaContable_Registrar 
    @Nombre = 'Gasto',
    @Descripcion = 'Cuentas que representan salidas económicas o consumo de recursos.',
    @Resultado = @Resultado OUTPUT,
    @Mensaje = @Mensaje OUTPUT;


---------------------------------------------------------
-- NATURALEZA CUENTA CONTABLE
---------------------------------------------------------

EXEC Catalogos.sp_NaturalezaCuentaContable_Registrar 
    @Naturaleza = 'Deudora',
    @Descripcion = 'Naturaleza utilizada principalmente para activos y gastos.',
    @Resultado = @Resultado OUTPUT,
    @Mensaje = @Mensaje OUTPUT;

EXEC Catalogos.sp_NaturalezaCuentaContable_Registrar 
    @Naturaleza = 'Acreedora',
    @Descripcion = 'Naturaleza utilizada principalmente para pasivos, patrimonio e ingresos.',
    @Resultado = @Resultado OUTPUT,
    @Mensaje = @Mensaje OUTPUT;


---------------------------------------------------------
-- PUESTO
---------------------------------------------------------

EXEC Catalogos.sp_Puesto_Registrar 
    @Nombre = 'Dueño',
    @Descripcion = 'Persona propietaria del negocio con acceso a funciones administrativas.',
    @Resultado = @Resultado OUTPUT,
    @Mensaje = @Mensaje OUTPUT;

EXEC Catalogos.sp_Puesto_Registrar 
    @Nombre = 'Administrador',
    @Descripcion = 'Persona encargada de supervisar operaciones, registros y control del sistema.',
    @Resultado = @Resultado OUTPUT,
    @Mensaje = @Mensaje OUTPUT;

EXEC Catalogos.sp_Puesto_Registrar 
    @Nombre = 'Cajero',
    @Descripcion = 'Colaborador encargado de registrar ventas y atender pagos de clientes.',
    @Resultado = @Resultado OUTPUT,
    @Mensaje = @Mensaje OUTPUT;

EXEC Catalogos.sp_Puesto_Registrar 
    @Nombre = 'Colaborador',
    @Descripcion = 'Empleado que realiza labores generales del abastecedor.',
    @Resultado = @Resultado OUTPUT,
    @Mensaje = @Mensaje OUTPUT;