IF NOT EXISTS (SELECT 1 FROM sys.schemas WHERE name = 'Presupuesto')
BEGIN
    EXEC('CREATE SCHEMA Presupuesto');
END;
GO

/* =========================================================
   DETALLE PRESUPUESTO MENSUAL - LISTAR POR PRESUPUESTO
   ========================================================= */
CREATE OR ALTER PROCEDURE Presupuesto.sp_DetallePresupuestoMensual_ListarPorPresupuesto
    @Anio INT,
    @Mes INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        d.Anio,
        d.Mes,
        d.CodigoCuenta,
        cc.NombreCuenta,
        d.TipoMovimiento,
        d.MontoPresupuestado,
        d.FechaCreacion,
        d.FechaModificacion,
        d.Activo,

        CASE d.Mes
            WHEN 1 THEN 'Enero'
            WHEN 2 THEN 'Febrero'
            WHEN 3 THEN 'Marzo'
            WHEN 4 THEN 'Abril'
            WHEN 5 THEN 'Mayo'
            WHEN 6 THEN 'Junio'
            WHEN 7 THEN 'Julio'
            WHEN 8 THEN 'Agosto'
            WHEN 9 THEN 'Septiembre'
            WHEN 10 THEN 'Octubre'
            WHEN 11 THEN 'Noviembre'
            WHEN 12 THEN 'Diciembre'
            ELSE ''
        END AS MesNombre
    FROM dbo.DetallePresupuestoMensual d
    INNER JOIN dbo.CuentaContable cc
        ON cc.CodigoCuenta = d.CodigoCuenta
    WHERE d.Anio = @Anio
      AND d.Mes = @Mes
    ORDER BY
        d.TipoMovimiento,
        d.CodigoCuenta;
END;
GO

/* =========================================================
   DETALLE PRESUPUESTO MENSUAL - OBTENER
   ========================================================= */
CREATE OR ALTER PROCEDURE Presupuesto.sp_DetallePresupuestoMensual_Obtener
    @Anio INT,
    @Mes INT,
    @CodigoCuenta VARCHAR(45),
    @TipoMovimiento VARCHAR(45)
AS
BEGIN
    SET NOCOUNT ON;

    SET @CodigoCuenta = LTRIM(RTRIM(ISNULL(@CodigoCuenta, '')));
    SET @TipoMovimiento = LTRIM(RTRIM(ISNULL(@TipoMovimiento, '')));

    SELECT
        d.Anio,
        d.Mes,
        d.CodigoCuenta,
        cc.NombreCuenta,
        d.TipoMovimiento,
        d.MontoPresupuestado,
        d.FechaCreacion,
        d.FechaModificacion,
        d.Activo,

        CASE d.Mes
            WHEN 1 THEN 'Enero'
            WHEN 2 THEN 'Febrero'
            WHEN 3 THEN 'Marzo'
            WHEN 4 THEN 'Abril'
            WHEN 5 THEN 'Mayo'
            WHEN 6 THEN 'Junio'
            WHEN 7 THEN 'Julio'
            WHEN 8 THEN 'Agosto'
            WHEN 9 THEN 'Septiembre'
            WHEN 10 THEN 'Octubre'
            WHEN 11 THEN 'Noviembre'
            WHEN 12 THEN 'Diciembre'
            ELSE ''
        END AS MesNombre
    FROM dbo.DetallePresupuestoMensual d
    INNER JOIN dbo.CuentaContable cc
        ON cc.CodigoCuenta = d.CodigoCuenta
    WHERE d.Anio = @Anio
      AND d.Mes = @Mes
      AND d.CodigoCuenta = @CodigoCuenta
      AND d.TipoMovimiento = @TipoMovimiento;
END;
GO

/* =========================================================
   DETALLE PRESUPUESTO MENSUAL - REGISTRAR
   ========================================================= */
CREATE OR ALTER PROCEDURE Presupuesto.sp_DetallePresupuestoMensual_Registrar
    @Anio INT,
    @Mes INT,
    @CodigoCuenta VARCHAR(45),
    @TipoMovimiento VARCHAR(45),
    @MontoPresupuestado DECIMAL(18,2),
    @Resultado INT OUTPUT,
    @Mensaje VARCHAR(500) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    SET @Resultado = 0;
    SET @Mensaje = '';

    SET @CodigoCuenta = LTRIM(RTRIM(ISNULL(@CodigoCuenta, '')));
    SET @TipoMovimiento = LTRIM(RTRIM(ISNULL(@TipoMovimiento, '')));

    IF (@Anio < 2000 OR @Anio > 2100)
    BEGIN
        SET @Mensaje = 'Debe ingresar un año válido.';
        RETURN;
    END;

    IF (@Mes < 1 OR @Mes > 12)
    BEGIN
        SET @Mensaje = 'Debe ingresar un mes válido.';
        RETURN;
    END;

    IF NOT EXISTS (
        SELECT 1
        FROM dbo.PresupuestoMensual
        WHERE Anio = @Anio
          AND Mes = @Mes
          AND Activo = 1
          AND Estado NOT IN ('Cerrado', 'Anulado', 'Inactivo')
    )
    BEGIN
        SET @Mensaje = 'El presupuesto no existe, está inactivo, cerrado o anulado.';
        RETURN;
    END;

    IF (@CodigoCuenta = '')
    BEGIN
        SET @Mensaje = 'Debe seleccionar una cuenta contable.';
        RETURN;
    END;

    IF NOT EXISTS (
        SELECT 1
        FROM dbo.CuentaContable
        WHERE CodigoCuenta = @CodigoCuenta
          AND Activo = 1
          AND AceptaMovimientos = 1
    )
    BEGIN
        SET @Mensaje = 'La cuenta contable seleccionada no existe, está inactiva o no acepta movimientos.';
        RETURN;
    END;

    IF (@TipoMovimiento NOT IN ('Ingreso', 'Gasto'))
    BEGIN
        SET @Mensaje = 'El tipo de movimiento debe ser Ingreso o Gasto.';
        RETURN;
    END;

    IF (@MontoPresupuestado < 0)
    BEGIN
        SET @Mensaje = 'El monto presupuestado no puede ser negativo.';
        RETURN;
    END;

    IF EXISTS (
        SELECT 1
        FROM dbo.DetallePresupuestoMensual
        WHERE Anio = @Anio
          AND Mes = @Mes
          AND CodigoCuenta = @CodigoCuenta
          AND TipoMovimiento = @TipoMovimiento
    )
    BEGIN
        SET @Mensaje = 'Ya existe un detalle para esa cuenta y tipo de movimiento en este presupuesto.';
        RETURN;
    END;

    BEGIN TRY
        INSERT INTO dbo.DetallePresupuestoMensual
        (
            Anio,
            Mes,
            CodigoCuenta,
            TipoMovimiento,
            MontoPresupuestado,
            FechaCreacion,
            FechaModificacion,
            Activo
        )
        VALUES
        (
            @Anio,
            @Mes,
            @CodigoCuenta,
            @TipoMovimiento,
            @MontoPresupuestado,
            GETDATE(),
            GETDATE(),
            1
        );

        SET @Resultado = 1;
        SET @Mensaje = 'Detalle de presupuesto registrado correctamente.';
    END TRY
    BEGIN CATCH
        SET @Resultado = 0;
        SET @Mensaje = ERROR_MESSAGE();
    END CATCH;
END;
GO

/* =========================================================
   DETALLE PRESUPUESTO MENSUAL - EDITAR
   ========================================================= */
CREATE OR ALTER PROCEDURE Presupuesto.sp_DetallePresupuestoMensual_Editar
    @Anio INT,
    @Mes INT,
    @CodigoCuenta VARCHAR(45),
    @TipoMovimiento VARCHAR(45),
    @MontoPresupuestado DECIMAL(18,2),
    @Activo BIT,
    @Resultado BIT OUTPUT,
    @Mensaje VARCHAR(500) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    SET @Resultado = 0;
    SET @Mensaje = '';

    SET @CodigoCuenta = LTRIM(RTRIM(ISNULL(@CodigoCuenta, '')));
    SET @TipoMovimiento = LTRIM(RTRIM(ISNULL(@TipoMovimiento, '')));

    IF (@Anio < 2000 OR @Anio > 2100)
    BEGIN
        SET @Mensaje = 'Debe ingresar un año válido.';
        RETURN;
    END;

    IF (@Mes < 1 OR @Mes > 12)
    BEGIN
        SET @Mensaje = 'Debe ingresar un mes válido.';
        RETURN;
    END;

    IF NOT EXISTS (
        SELECT 1
        FROM dbo.PresupuestoMensual
        WHERE Anio = @Anio
          AND Mes = @Mes
          AND Activo = 1
          AND Estado NOT IN ('Cerrado', 'Anulado', 'Inactivo')
    )
    BEGIN
        SET @Mensaje = 'No se puede modificar un presupuesto cerrado, anulado o inactivo.';
        RETURN;
    END;

    IF (@CodigoCuenta = '')
    BEGIN
        SET @Mensaje = 'Debe seleccionar una cuenta contable.';
        RETURN;
    END;

    IF NOT EXISTS (
        SELECT 1
        FROM dbo.CuentaContable
        WHERE CodigoCuenta = @CodigoCuenta
          AND Activo = 1
          AND AceptaMovimientos = 1
    )
    BEGIN
        SET @Mensaje = 'La cuenta contable seleccionada no existe, está inactiva o no acepta movimientos.';
        RETURN;
    END;

    IF (@TipoMovimiento NOT IN ('Ingreso', 'Gasto'))
    BEGIN
        SET @Mensaje = 'El tipo de movimiento debe ser Ingreso o Gasto.';
        RETURN;
    END;

    IF (@MontoPresupuestado < 0)
    BEGIN
        SET @Mensaje = 'El monto presupuestado no puede ser negativo.';
        RETURN;
    END;

    IF NOT EXISTS (
        SELECT 1
        FROM dbo.DetallePresupuestoMensual
        WHERE Anio = @Anio
          AND Mes = @Mes
          AND CodigoCuenta = @CodigoCuenta
          AND TipoMovimiento = @TipoMovimiento
    )
    BEGIN
        SET @Mensaje = 'El detalle de presupuesto no existe.';
        RETURN;
    END;

    UPDATE dbo.DetallePresupuestoMensual
    SET
        MontoPresupuestado = @MontoPresupuestado,
        FechaModificacion = GETDATE(),
        Activo = @Activo
    WHERE Anio = @Anio
      AND Mes = @Mes
      AND CodigoCuenta = @CodigoCuenta
      AND TipoMovimiento = @TipoMovimiento;

    SET @Resultado = 1;
    SET @Mensaje = 'Detalle de presupuesto actualizado correctamente.';
END;
GO

/* =========================================================
   DETALLE PRESUPUESTO MENSUAL - INACTIVAR
   ========================================================= */
CREATE OR ALTER PROCEDURE Presupuesto.sp_DetallePresupuestoMensual_Inactivar
    @Anio INT,
    @Mes INT,
    @CodigoCuenta VARCHAR(45),
    @TipoMovimiento VARCHAR(45),
    @Resultado BIT OUTPUT,
    @Mensaje VARCHAR(500) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    SET @Resultado = 0;
    SET @Mensaje = '';

    SET @CodigoCuenta = LTRIM(RTRIM(ISNULL(@CodigoCuenta, '')));
    SET @TipoMovimiento = LTRIM(RTRIM(ISNULL(@TipoMovimiento, '')));

    IF (@Anio < 2000 OR @Anio > 2100)
    BEGIN
        SET @Mensaje = 'Debe ingresar un año válido.';
        RETURN;
    END;

    IF (@Mes < 1 OR @Mes > 12)
    BEGIN
        SET @Mensaje = 'Debe ingresar un mes válido.';
        RETURN;
    END;

    IF (@CodigoCuenta = '' OR @TipoMovimiento = '')
    BEGIN
        SET @Mensaje = 'Debe seleccionar un detalle válido.';
        RETURN;
    END;

    IF NOT EXISTS (
        SELECT 1
        FROM dbo.DetallePresupuestoMensual
        WHERE Anio = @Anio
          AND Mes = @Mes
          AND CodigoCuenta = @CodigoCuenta
          AND TipoMovimiento = @TipoMovimiento
          AND Activo = 1
    )
    BEGIN
        SET @Mensaje = 'El detalle de presupuesto no existe o ya está inactivo.';
        RETURN;
    END;

    UPDATE dbo.DetallePresupuestoMensual
    SET
        Activo = 0,
        FechaModificacion = GETDATE()
    WHERE Anio = @Anio
      AND Mes = @Mes
      AND CodigoCuenta = @CodigoCuenta
      AND TipoMovimiento = @TipoMovimiento;

    SET @Resultado = 1;
    SET @Mensaje = 'Detalle de presupuesto inactivado correctamente.';
END;
GO

/* =========================================================
   DETALLE PRESUPUESTO MENSUAL - ACTIVAR
   ========================================================= */
CREATE OR ALTER PROCEDURE Presupuesto.sp_DetallePresupuestoMensual_Activar
    @Anio INT,
    @Mes INT,
    @CodigoCuenta VARCHAR(45),
    @TipoMovimiento VARCHAR(45),
    @Resultado BIT OUTPUT,
    @Mensaje VARCHAR(500) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    SET @Resultado = 0;
    SET @Mensaje = '';

    SET @CodigoCuenta = LTRIM(RTRIM(ISNULL(@CodigoCuenta, '')));
    SET @TipoMovimiento = LTRIM(RTRIM(ISNULL(@TipoMovimiento, '')));

    IF NOT EXISTS (
        SELECT 1
        FROM dbo.PresupuestoMensual
        WHERE Anio = @Anio
          AND Mes = @Mes
          AND Activo = 1
          AND Estado NOT IN ('Cerrado', 'Anulado', 'Inactivo')
    )
    BEGIN
        SET @Mensaje = 'No se puede activar un detalle de un presupuesto cerrado, anulado o inactivo.';
        RETURN;
    END;

    IF NOT EXISTS (
        SELECT 1
        FROM dbo.DetallePresupuestoMensual
        WHERE Anio = @Anio
          AND Mes = @Mes
          AND CodigoCuenta = @CodigoCuenta
          AND TipoMovimiento = @TipoMovimiento
    )
    BEGIN
        SET @Mensaje = 'El detalle de presupuesto no existe.';
        RETURN;
    END;

    UPDATE dbo.DetallePresupuestoMensual
    SET
        Activo = 1,
        FechaModificacion = GETDATE()
    WHERE Anio = @Anio
      AND Mes = @Mes
      AND CodigoCuenta = @CodigoCuenta
      AND TipoMovimiento = @TipoMovimiento;

    SET @Resultado = 1;
    SET @Mensaje = 'Detalle de presupuesto activado correctamente.';
END;
GO