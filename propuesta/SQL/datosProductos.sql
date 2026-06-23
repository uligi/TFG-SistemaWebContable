DECLARE @Resultado INT;
DECLARE @Mensaje VARCHAR(500);

DECLARE @Verduras INT = (SELECT IdTipoProducto FROM dbo.TipoProducto WHERE Nombre = 'Verduras');
DECLARE @Frutas INT = (SELECT IdTipoProducto FROM dbo.TipoProducto WHERE Nombre = 'Frutas');
DECLARE @Abarrotes INT = (SELECT IdTipoProducto FROM dbo.TipoProducto WHERE Nombre = 'Abarrotes');
DECLARE @Lacteos INT = (SELECT IdTipoProducto FROM dbo.TipoProducto WHERE Nombre = 'Lácteos');
DECLARE @Bebidas INT = (SELECT IdTipoProducto FROM dbo.TipoProducto WHERE Nombre = 'Bebidas');
DECLARE @Panaderia INT = (SELECT IdTipoProducto FROM dbo.TipoProducto WHERE Nombre = 'Panadería');
DECLARE @Limpieza INT = (SELECT IdTipoProducto FROM dbo.TipoProducto WHERE Nombre = 'Limpieza');
DECLARE @Higiene INT = (SELECT IdTipoProducto FROM dbo.TipoProducto WHERE Nombre = 'Higiene personal');
DECLARE @Ferreteria INT = (SELECT IdTipoProducto FROM dbo.TipoProducto WHERE Nombre = 'Ferretería');
DECLARE @Jardineria INT = (SELECT IdTipoProducto FROM dbo.TipoProducto WHERE Nombre = 'Jardinería');
DECLARE @Mascotas INT = (SELECT IdTipoProducto FROM dbo.TipoProducto WHERE Nombre = 'Mascotas');
DECLARE @Congelados INT = (SELECT IdTipoProducto FROM dbo.TipoProducto WHERE Nombre = 'Congelados');
DECLARE @Snacks INT = (SELECT IdTipoProducto FROM dbo.TipoProducto WHERE Nombre = 'Snacks');
DECLARE @Otros INT = (SELECT IdTipoProducto FROM dbo.TipoProducto WHERE Nombre = 'Otros');

EXEC Inventario.sp_Producto_Registrar 'VER-001', 'Tomate nacional', @Verduras, 'Tomate fresco nacional vendido por kilogramo.', 1200.00, 25.00, @Resultado OUTPUT, @Mensaje OUTPUT;
EXEC Inventario.sp_Producto_Registrar 'VER-002', 'Papa nacional', @Verduras, 'Papa nacional vendida por kilogramo.', 950.00, 40.00, @Resultado OUTPUT, @Mensaje OUTPUT;
EXEC Inventario.sp_Producto_Registrar 'VER-003', 'Cebolla amarilla', @Verduras, 'Cebolla amarilla fresca vendida por kilogramo.', 1100.00, 30.00, @Resultado OUTPUT, @Mensaje OUTPUT;
EXEC Inventario.sp_Producto_Registrar 'VER-004', 'Chile dulce', @Verduras, 'Chile dulce fresco vendido por unidad.', 250.00, 35.00, @Resultado OUTPUT, @Mensaje OUTPUT;

EXEC Inventario.sp_Producto_Registrar 'FRU-001', 'Banano criollo', @Frutas, 'Banano fresco vendido por unidad.', 150.00, 100.00, @Resultado OUTPUT, @Mensaje OUTPUT;
EXEC Inventario.sp_Producto_Registrar 'FRU-002', 'Naranja nacional', @Frutas, 'Naranja fresca vendida por unidad.', 200.00, 80.00, @Resultado OUTPUT, @Mensaje OUTPUT;
EXEC Inventario.sp_Producto_Registrar 'FRU-003', 'Manzana roja', @Frutas, 'Manzana roja vendida por unidad.', 450.00, 60.00, @Resultado OUTPUT, @Mensaje OUTPUT;
EXEC Inventario.sp_Producto_Registrar 'FRU-004', 'Piña dulce', @Frutas, 'Piña fresca vendida por unidad.', 1200.00, 15.00, @Resultado OUTPUT, @Mensaje OUTPUT;

EXEC Inventario.sp_Producto_Registrar 'ABA-001', 'Arroz Tío Pelón 1 kg', @Abarrotes, 'Bolsa de arroz Tío Pelón de un kilogramo.', 1100.00, 50.00, @Resultado OUTPUT, @Mensaje OUTPUT;
EXEC Inventario.sp_Producto_Registrar 'ABA-002', 'Arroz Luisiana 1 kg', @Abarrotes, 'Bolsa de arroz Luisiana de un kilogramo.', 1050.00, 45.00, @Resultado OUTPUT, @Mensaje OUTPUT;
EXEC Inventario.sp_Producto_Registrar 'ABA-003', 'Frijoles Don Pedro 900 g', @Abarrotes, 'Bolsa de frijoles Don Pedro de 900 gramos.', 1500.00, 35.00, @Resultado OUTPUT, @Mensaje OUTPUT;
EXEC Inventario.sp_Producto_Registrar 'ABA-004', 'Azúcar Doña María 1 kg', @Abarrotes, 'Bolsa de azúcar Doña María de un kilogramo.', 900.00, 40.00, @Resultado OUTPUT, @Mensaje OUTPUT;
EXEC Inventario.sp_Producto_Registrar 'ABA-005', 'Sal Sol 500 g', @Abarrotes, 'Bolsa de sal Sol de 500 gramos.', 450.00, 30.00, @Resultado OUTPUT, @Mensaje OUTPUT;
EXEC Inventario.sp_Producto_Registrar 'ABA-006', 'Aceite Clover 1 L', @Abarrotes, 'Botella de aceite Clover de un litro.', 1900.00, 25.00, @Resultado OUTPUT, @Mensaje OUTPUT;
EXEC Inventario.sp_Producto_Registrar 'ABA-007', 'Sardina Sardimar', @Abarrotes, 'Lata de sardina Sardimar.', 1200.00, 30.00, @Resultado OUTPUT, @Mensaje OUTPUT;
EXEC Inventario.sp_Producto_Registrar 'ABA-008', 'Atún Sardimar', @Abarrotes, 'Lata de atún Sardimar.', 1500.00, 28.00, @Resultado OUTPUT, @Mensaje OUTPUT;
EXEC Inventario.sp_Producto_Registrar 'ABA-009', 'Pasta Roma 250 g', @Abarrotes, 'Paquete de pasta Roma de 250 gramos.', 650.00, 40.00, @Resultado OUTPUT, @Mensaje OUTPUT;
EXEC Inventario.sp_Producto_Registrar 'ABA-010', 'Café Rey 250 g', @Abarrotes, 'Paquete de café Rey molido de 250 gramos.', 1800.00, 22.00, @Resultado OUTPUT, @Mensaje OUTPUT;
EXEC Inventario.sp_Producto_Registrar 'ABA-011', 'Salsa Lizano 280 ml', @Abarrotes, 'Botella de salsa Lizano de 280 mililitros.', 1600.00, 20.00, @Resultado OUTPUT, @Mensaje OUTPUT;

EXEC Inventario.sp_Producto_Registrar 'LAC-001', 'Leche Dos Pinos 1 L', @Lacteos, 'Caja de leche Dos Pinos de un litro.', 1100.00, 35.00, @Resultado OUTPUT, @Mensaje OUTPUT;
EXEC Inventario.sp_Producto_Registrar 'LAC-002', 'Leche Coronado 1 L', @Lacteos, 'Caja de leche Coronado de un litro.', 1050.00, 25.00, @Resultado OUTPUT, @Mensaje OUTPUT;
EXEC Inventario.sp_Producto_Registrar 'LAC-003', 'Natilla Dos Pinos 400 g', @Lacteos, 'Natilla Dos Pinos en presentación de 400 gramos.', 1650.00, 18.00, @Resultado OUTPUT, @Mensaje OUTPUT;
EXEC Inventario.sp_Producto_Registrar 'LAC-004', 'Queso Turrialba 500 g', @Lacteos, 'Queso Turrialba empacado de 500 gramos.', 2800.00, 12.00, @Resultado OUTPUT, @Mensaje OUTPUT;

EXEC Inventario.sp_Producto_Registrar 'BEB-001', 'Agua Cristal 600 ml', @Bebidas, 'Botella de agua Cristal de 600 mililitros.', 500.00, 60.00, @Resultado OUTPUT, @Mensaje OUTPUT;
EXEC Inventario.sp_Producto_Registrar 'BEB-002', 'Coca-Cola 600 ml', @Bebidas, 'Botella de Coca-Cola de 600 mililitros.', 850.00, 45.00, @Resultado OUTPUT, @Mensaje OUTPUT;
EXEC Inventario.sp_Producto_Registrar 'BEB-003', 'Coca-Cola 2.5 L', @Bebidas, 'Botella de Coca-Cola familiar de 2.5 litros.', 2200.00, 25.00, @Resultado OUTPUT, @Mensaje OUTPUT;
EXEC Inventario.sp_Producto_Registrar 'BEB-004', 'Fanta Naranja 600 ml', @Bebidas, 'Botella de Fanta naranja de 600 mililitros.', 800.00, 35.00, @Resultado OUTPUT, @Mensaje OUTPUT;
EXEC Inventario.sp_Producto_Registrar 'BEB-005', 'Fresco Tropical 355 ml', @Bebidas, 'Lata de Fresco Tropical de 355 mililitros.', 700.00, 40.00, @Resultado OUTPUT, @Mensaje OUTPUT;

EXEC Inventario.sp_Producto_Registrar 'PAN-001', 'Pan cuadrado Bimbo', @Panaderia, 'Paquete de pan cuadrado Bimbo.', 1900.00, 18.00, @Resultado OUTPUT, @Mensaje OUTPUT;
EXEC Inventario.sp_Producto_Registrar 'PAN-002', 'Tortillas TortiRicas', @Panaderia, 'Paquete de tortillas TortiRicas.', 950.00, 25.00, @Resultado OUTPUT, @Mensaje OUTPUT;
EXEC Inventario.sp_Producto_Registrar 'PAN-003', 'Galletas Pozuelo María', @Panaderia, 'Paquete de galletas María Pozuelo.', 850.00, 35.00, @Resultado OUTPUT, @Mensaje OUTPUT;
EXEC Inventario.sp_Producto_Registrar 'PAN-004', 'Galletas Chiky', @Panaderia, 'Paquete de galletas Chiky.', 650.00, 40.00, @Resultado OUTPUT, @Mensaje OUTPUT;

EXEC Inventario.sp_Producto_Registrar 'LIM-001', 'Cloro Magia Blanca 1 L', @Limpieza, 'Botella de cloro Magia Blanca de un litro.', 850.00, 25.00, @Resultado OUTPUT, @Mensaje OUTPUT;
EXEC Inventario.sp_Producto_Registrar 'LIM-002', 'Desinfectante Irex 1 L', @Limpieza, 'Botella de desinfectante Irex de un litro.', 1500.00, 18.00, @Resultado OUTPUT, @Mensaje OUTPUT;
EXEC Inventario.sp_Producto_Registrar 'LIM-003', 'Detergente Rinso 500 g', @Limpieza, 'Bolsa de detergente Rinso de 500 gramos.', 1300.00, 20.00, @Resultado OUTPUT, @Mensaje OUTPUT;
EXEC Inventario.sp_Producto_Registrar 'LIM-004', 'Lavaplatos Axion', @Limpieza, 'Lavaplatos Axion en pasta.', 1200.00, 22.00, @Resultado OUTPUT, @Mensaje OUTPUT;

EXEC Inventario.sp_Producto_Registrar 'HIG-001', 'Papel higiénico Scott 4 rollos', @Higiene, 'Paquete de papel higiénico Scott de cuatro rollos.', 2400.00, 20.00, @Resultado OUTPUT, @Mensaje OUTPUT;
EXEC Inventario.sp_Producto_Registrar 'HIG-002', 'Pasta dental Colgate', @Higiene, 'Pasta dental Colgate en presentación mediana.', 1700.00, 25.00, @Resultado OUTPUT, @Mensaje OUTPUT;
EXEC Inventario.sp_Producto_Registrar 'HIG-003', 'Jabón Protex', @Higiene, 'Jabón de baño Protex en barra.', 800.00, 30.00, @Resultado OUTPUT, @Mensaje OUTPUT;

EXEC Inventario.sp_Producto_Registrar 'FER-001', 'Clavos de acero 1 pulgada', @Ferreteria, 'Paquete de clavos de acero de una pulgada.', 1000.00, 30.00, @Resultado OUTPUT, @Mensaje OUTPUT;
EXEC Inventario.sp_Producto_Registrar 'FER-002', 'Tornillos pequeños', @Ferreteria, 'Paquete de tornillos pequeños para uso general.', 1200.00, 25.00, @Resultado OUTPUT, @Mensaje OUTPUT;
EXEC Inventario.sp_Producto_Registrar 'FER-003', 'Cinta aislante 3M', @Ferreteria, 'Rollo de cinta aislante 3M.', 1100.00, 20.00, @Resultado OUTPUT, @Mensaje OUTPUT;
EXEC Inventario.sp_Producto_Registrar 'FER-004', 'Bombillo LED Sylvania', @Ferreteria, 'Bombillo LED Sylvania para uso doméstico.', 1800.00, 18.00, @Resultado OUTPUT, @Mensaje OUTPUT;

EXEC Inventario.sp_Producto_Registrar 'MAS-001', 'Alimento Superkan 1 kg', @Mascotas, 'Bolsa de alimento para perro Superkan de un kilogramo.', 2300.00, 18.00, @Resultado OUTPUT, @Mensaje OUTPUT;
EXEC Inventario.sp_Producto_Registrar 'MAS-002', 'Alimento Gati 1 kg', @Mascotas, 'Bolsa de alimento para gato Gati de un kilogramo.', 2500.00, 15.00, @Resultado OUTPUT, @Mensaje OUTPUT;

EXEC Inventario.sp_Producto_Registrar 'CON-001', 'Helado Dos Pinos familiar', @Congelados, 'Envase de helado Dos Pinos familiar.', 3200.00, 10.00, @Resultado OUTPUT, @Mensaje OUTPUT;
EXEC Inventario.sp_Producto_Registrar 'CON-002', 'Salchichas Cinta Azul', @Congelados, 'Paquete de salchichas Cinta Azul.', 1900.00, 16.00, @Resultado OUTPUT, @Mensaje OUTPUT;
EXEC Inventario.sp_Producto_Registrar 'CON-003', 'Mortadela Cinta Azul', @Congelados, 'Paquete de mortadela Cinta Azul.', 1600.00, 14.00, @Resultado OUTPUT, @Mensaje OUTPUT;

EXEC Inventario.sp_Producto_Registrar 'SNA-001', 'Papas tostadas Diana', @Snacks, 'Bolsa de papas tostadas Diana.', 750.00, 45.00, @Resultado OUTPUT, @Mensaje OUTPUT;
EXEC Inventario.sp_Producto_Registrar 'SNA-002', 'Tosty', @Snacks, 'Snack Tosty en presentación individual.', 500.00, 50.00, @Resultado OUTPUT, @Mensaje OUTPUT;
EXEC Inventario.sp_Producto_Registrar 'SNA-003', 'Meneitos', @Snacks, 'Snack Meneitos en presentación individual.', 500.00, 50.00, @Resultado OUTPUT, @Mensaje OUTPUT;
EXEC Inventario.sp_Producto_Registrar 'SNA-004', 'Chocolate Gallito', @Snacks, 'Chocolate Gallito individual.', 600.00, 45.00, @Resultado OUTPUT, @Mensaje OUTPUT;

EXEC Inventario.sp_Producto_Registrar 'OTR-001', 'Bolsa reutilizable', @Otros, 'Bolsa reutilizable para compras.', 500.00, 30.00, @Resultado OUTPUT, @Mensaje OUTPUT;
EXEC Inventario.sp_Producto_Registrar 'OTR-002', 'Fósforos', @Otros, 'Caja de fósforos para uso doméstico.', 300.00, 40.00, @Resultado OUTPUT, @Mensaje OUTPUT;
EXEC Inventario.sp_Producto_Registrar 'OTR-003', 'Baterías AA', @Otros, 'Paquete de baterías AA.', 1800.00, 15.00, @Resultado OUTPUT, @Mensaje OUTPUT;