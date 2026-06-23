-- Persona del cliente
INSERT INTO dbo.Persona
(
    Identificacion,
    Nombre,
    PrimerApellido,
    SegundoApellido,
    FechaNacimiento,
    IdDistrito,
    DireccionExacta,
    Activo
)
VALUES
(
    '105550999',
    'Jose',
    'Rodríguez',
    'Sánchez',
    '1992-04-15',
    27,
    'Cartago centro, 100 metros norte del parque central.',
    1
);

-- Registro como cliente
INSERT INTO dbo.Cliente
(
    Identificacion,
    CodigoCliente,
    LimiteCredito,
    DiasCredito,
    Activo
)
VALUES
(
    '105550999',
    'CLI-0001',
    150000.00,
    30,
    1
);

INSERT INTO dbo.Rol (Nombre, Descripcion, Activo)
VALUES
('Administrador', 'Usuario con acceso completo a todos los módulos del sistema.', 1),
('Colaborador', 'Usuario operativo con acceso limitado a los módulos permitidos.', 1);

-- Persona del empleado
INSERT INTO dbo.Persona
(
    Identificacion,
    Nombre,
    PrimerApellido,
    SegundoApellido,
    FechaNacimiento,
    IdDistrito,
    DireccionExacta,
    Activo
)
VALUES
(
    '104440888',
    'Charlie',
    'Mora',
    'Jiménez',
    '1988-09-22',
    26,
    'Cartago, distrito Oriental, cerca del abastecedor.',
    1
);

-- Registro como empleado
INSERT INTO dbo.Empleado
(
    Identificacion,
    CodigoEmpleado,
    Puesto,
    FechaIngreso,
    IdRol,
    NombreUsuario,
    ClaveHash,
    RestablecerClave,
    Activo
)
VALUES
(
    '104440888',
    'EMP-0002',
    'Administrador',
    GETDATE(),
    2,
    'cmoro',
    '12346',
    1,
    1
);