-- =========================
-- PROVINCIAS
-- =========================

INSERT INTO Provincia (Nombre, Activo) VALUES
('San José', 1),
('Alajuela', 1),
('Cartago', 1),
('Heredia', 1),
('Guanacaste', 1),
('Puntarenas', 1),
('Limón', 1);


-- =========================
-- CANTONES
-- =========================

INSERT INTO Canton (IdProvincia, Nombre, Activo) VALUES
-- San José = 1
(1, 'San José', 1),
(1, 'Escazú', 1),
(1, 'Desamparados', 1),
(1, 'Curridabat', 1),

-- Alajuela = 2
(2, 'Alajuela', 1),
(2, 'San Ramón', 1),
(2, 'Grecia', 1),

-- Cartago = 3
(3, 'Cartago', 1),
(3, 'Paraíso', 1),
(3, 'La Unión', 1),
(3, 'Oreamuno', 1),

-- Heredia = 4
(4, 'Heredia', 1),
(4, 'Barva', 1),
(4, 'Santo Domingo', 1),

-- Guanacaste = 5
(5, 'Liberia', 1),
(5, 'Nicoya', 1),
(5, 'Santa Cruz', 1),

-- Puntarenas = 6
(6, 'Puntarenas', 1),
(6, 'Esparza', 1),
(6, 'Garabito', 1),

-- Limón = 7
(7, 'Limón', 1),
(7, 'Pococí', 1),
(7, 'Siquirres', 1);

-- =========================
-- DISTRITOS
-- =========================

INSERT INTO Distrito (IdCanton, Nombre, Activo) VALUES
-- Cantón San José = 1
(1, 'Carmen', 1),
(1, 'Merced', 1),
(1, 'Hospital', 1),
(1, 'Catedral', 1),
(1, 'Zapote', 1),
(1, 'San Francisco de Dos Ríos', 1),

-- Cantón Escazú = 2
(2, 'Escazú', 1),
(2, 'San Antonio', 1),
(2, 'San Rafael', 1),

-- Cantón Desamparados = 3
(3, 'Desamparados', 1),
(3, 'San Miguel', 1),
(3, 'San Juan de Dios', 1),

-- Cantón Curridabat = 4
(4, 'Curridabat', 1),
(4, 'Granadilla', 1),
(4, 'Sánchez', 1),
(4, 'Tirrases', 1),

-- Cantón Alajuela = 5
(5, 'Alajuela', 1),
(5, 'San José', 1),
(5, 'Carrizal', 1),
(5, 'San Antonio', 1),

-- Cantón San Ramón = 6
(6, 'San Ramón', 1),
(6, 'Santiago', 1),
(6, 'San Juan', 1),

-- Cantón Grecia = 7
(7, 'Grecia', 1),
(7, 'San Isidro', 1),
(7, 'San José', 1),

-- Cantón Cartago = 8
(8, 'Oriental', 1),
(8, 'Occidental', 1),
(8, 'Carmen', 1),
(8, 'San Nicolás', 1),
(8, 'Agua Caliente', 1),
(8, 'Guadalupe', 1),
(8, 'Corralillo', 1),
(8, 'Tierra Blanca', 1),
(8, 'Dulce Nombre', 1),
(8, 'Llano Grande', 1),
(8, 'Quebradilla', 1),

-- Cantón Paraíso = 9
(9, 'Paraíso', 1),
(9, 'Santiago', 1),
(9, 'Orosi', 1),
(9, 'Cachí', 1),
(9, 'Llanos de Santa Lucía', 1),

-- Cantón La Unión = 10
(10, 'Tres Ríos', 1),
(10, 'San Diego', 1),
(10, 'San Juan', 1),
(10, 'San Rafael', 1),
(10, 'Concepción', 1),
(10, 'Dulce Nombre', 1),
(10, 'San Ramón', 1),
(10, 'Río Azul', 1),

-- Cantón Oreamuno = 11
(11, 'San Rafael', 1),
(11, 'Cot', 1),
(11, 'Potrero Cerrado', 1),
(11, 'Cipreses', 1),
(11, 'Santa Rosa', 1);

