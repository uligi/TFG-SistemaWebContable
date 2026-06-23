IF NOT EXISTS (SELECT 1 FROM sys.schemas WHERE name = 'Presupuesto')
BEGIN
    EXEC('CREATE SCHEMA Presupuesto');
END;
GO

/* =========================================================
   PRESUPUESTO MENSUAL - LISTAR
   ========================================================= */
CREATE OR ALTER PROCEDURE Presupuesto.sp_PresupuestoMensual_Listar
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        pm.Anio,
        pm.Mes,
        pm.Estado,
        pm.FechaCreacion,
        pm.FechaModificacion,
        pm.Activo,

        CASE pm.Mes
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
    FROM dbo.PresupuestoMensual pm
    ORDER BY
        pm.Anio DESC,
        pm.Mes DESC;
END;
GO

/* =========================================================
   PRESUPUESTO MENSUAL - OBTENER
   ========================================================= */
CREATE OR ALTER PROCEDURE Presupuesto.sp_PresupuestoMensual_Obtener
    @Anio INT,
    @Mes INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        pm.Anio,
        pm.Mes,
        pm.Estado,
        pm.FechaCreacion,
        pm.FechaModificacion,
        pm.Activo,

        CASE pm.Mes
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
    FROM dbo.PresupuestoMensual pm
    WHERE pm.Anio = @Anio
      AND pm.Mes = @Mes;
END;
GO

/* =========================================================
   PRESUPUESTO MENSUAL - REGISTRAR
   ========================================================= */
CREATE OR ALTER PROCEDURE Presupuesto.sp_PresupuestoMensual_Registrar
    @Anio INT,
    @Mes INT,
    @Estado VARCHAR(45),
    @Resultado INT OUTPUT,
    @Mensaje VARCHAR(500) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    SET @Resultado = 0;
    SET @Mensaje = '';

    SET @Estado = LTRIM(RTRIM(ISNULL(@Estado, '')));

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

    IF (@Estado = '')
    BEGIN
        SET @Estado = 'Abierto';
    END;

    IF (LEN(@Estado) > 45)
    BEGIN
        SET @Mensaje = 'El estado no puede superar los 45 caracteres.';
        RETURN;
    END;

    IF EXISTS (
        SELECT 1
        FROM dbo.PresupuestoMensual
        WHERE Anio = @Anio
          AND Mes = @Mes
    )
    BEGIN
        SET @Mensaje = 'Ya existe un presupuesto mensual para ese mes y año.';
        RETURN;
    END;

    BEGIN TRY
        INSERT INTO dbo.PresupuestoMensual
        (
            Anio,
            Mes,
            Estado,
            FechaCreacion,
            FechaModificacion,
            Activo
        )
        VALUES
        (
            @Anio,
            @Mes,
            @Estado,
            GETDATE(),
            GETDATE(),
            1
        );

        SET @Resultado = 1;
        SET @Mensaje = 'Presupuesto mensual registrado correctamente.';
    END TRY
    BEGIN CATCH
        SET @Resultado = 0;
        SET @Mensaje = ERROR_MESSAGE();
    END CATCH;
END;
GO

/* =========================================================
   PRESUPUESTO MENSUAL - EDITAR
   ========================================================= */
CREATE OR ALTER PROCEDURE Presupuesto.sp_PresupuestoMensual_Editar
    @Anio INT,
    @Mes INT,
    @Estado VARCHAR(45),
    @Activo BIT,
    @Resultado BIT OUTPUT,
    @Mensaje VARCHAR(500) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    SET @Resultado = 0;
    SET @Mensaje = '';

    SET @Estado = LTRIM(RTRIM(ISNULL(@Estado, '')));

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
    )
    BEGIN
        SET @Mensaje = 'El presupuesto mensual no existe.';
        RETURN;
    END;

    IF (@Estado = '')
    BEGIN
        SET @Mensaje = 'Debe ingresar el estado.';
        RETURN;
    END;

    IF (LEN(@Estado) > 45)
    BEGIN
        SET @Mensaje = 'El estado no puede superar los 45 caracteres.';
        RETURN;
    END;

    UPDATE dbo.PresupuestoMensual
    SET
        Estado = @Estado,
        FechaModificacion = GETDATE(),
        Activo = @Activo
    WHERE Anio = @Anio
      AND Mes = @Mes;

    SET @Resultado = 1;
    SET @Mensaje = 'Presupuesto mensual actualizado correctamente.';
END;
GO

/* =========================================================
   PRESUPUESTO MENSUAL - INACTIVAR
   ========================================================= */
CREATE OR ALTER PROCEDURE Presupuesto.sp_PresupuestoMensual_Inactivar
    @Anio INT,
    @Mes INT,
    @Resultado BIT OUTPUT,
    @Mensaje VARCHAR(500) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    SET @Resultado = 0;
    SET @Mensaje = '';

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
    )
    BEGIN
        SET @Mensaje = 'El presupuesto mensual no existe o ya está inactivo.';
        RETURN;
    END;

    UPDATE dbo.PresupuestoMensual
    SET
        Activo = 0,
        Estado = 'Inactivo',
        FechaModificacion = GETDATE()
    WHERE Anio = @Anio
      AND Mes = @Mes;

    UPDATE dbo.DetallePresupuestoMensual
    SET
        Activo = 0,
        FechaModificacion = GETDATE()
    WHERE Anio = @Anio
      AND Mes = @Mes
      AND Activo = 1;

    SET @Resultado = 1;
    SET @Mensaje = 'Presupuesto mensual inactivado correctamente.';
END;
GO

/* =========================================================
   PRESUPUESTO MENSUAL - CERRAR
   ========================================================= */
CREATE OR ALTER PROCEDURE Presupuesto.sp_PresupuestoMensual_Cerrar
    @Anio INT,
    @Mes INT,
    @Resultado BIT OUTPUT,
    @Mensaje VARCHAR(500) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    SET @Resultado = 0;
    SET @Mensaje = '';

    IF NOT EXISTS (
        SELECT 1
        FROM dbo.PresupuestoMensual
        WHERE Anio = @Anio
          AND Mes = @Mes
          AND Activo = 1
    )
    BEGIN
        SET @Mensaje = 'El presupuesto mensual no existe o está inactivo.';
        RETURN;
    END;

    UPDATE dbo.PresupuestoMensual
    SET
        Estado = 'Cerrado',
        FechaModificacion = GETDATE()
    WHERE Anio = @Anio
      AND Mes = @Mes;

    SET @Resultado = 1;
    SET @Mensaje = 'Presupuesto mensual cerrado correctamente.';
END;
GO

/* =========================================================
   PRESUPUESTO MENSUAL - REABRIR
   ========================================================= */
CREATE OR ALTER PROCEDURE Presupuesto.sp_PresupuestoMensual_Reabrir
    @Anio INT,
    @Mes INT,
    @Resultado BIT OUTPUT,
    @Mensaje VARCHAR(500) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    SET @Resultado = 0;
    SET @Mensaje = '';

    IF NOT EXISTS (
        SELECT 1
        FROM dbo.PresupuestoMensual
        WHERE Anio = @Anio
          AND Mes = @Mes
          AND Activo = 1
    )
    BEGIN
        SET @Mensaje = 'El presupuesto mensual no existe o está inactivo.';
        RETURN;
    END;

    UPDATE dbo.PresupuestoMensual
    SET
        Estado = 'Abierto',
        FechaModificacion = GETDATE()
    WHERE Anio = @Anio
      AND Mes = @Mes;

    SET @Resultado = 1;
    SET @Mensaje = 'Presupuesto mensual reabierto correctamente.';
END;
GO

/* =========================================================
   PRESUPUESTO MENSUAL - RESUMEN VS REAL
   Conecta el presupuesto con Ingreso y Gasto reales.
   ========================================================= */
CREATE OR ALTER PROCEDURE Presupuesto.sp_PresupuestoMensual_ResumenVsReal
    @Anio INT,
    @Mes INT
AS
BEGIN
    SET NOCOUNT ON;

    ;WITH RealesIngreso AS
    (
        SELECT
            i.CodigoCuenta,
            CAST('Ingreso' AS VARCHAR(45)) AS TipoMovimiento,
            SUM(i.Monto) AS MontoReal
        FROM dbo.Ingreso i
        WHERE YEAR(i.FechaIngreso) = @Anio
          AND MONTH(i.FechaIngreso) = @Mes
          AND i.Activo = 1
        GROUP BY i.CodigoCuenta
    ),
    RealesGasto AS
    (
        SELECT
            g.CodigoCuenta,
            CAST('Gasto' AS VARCHAR(45)) AS TipoMovimiento,
            SUM(g.Monto) AS MontoReal
        FROM dbo.Gasto g
        WHERE YEAR(g.FechaGasto) = @Anio
          AND MONTH(g.FechaGasto) = @Mes
          AND g.Activo = 1
        GROUP BY g.CodigoCuenta
    ),
    Reales AS
    (
        SELECT CodigoCuenta, TipoMovimiento, MontoReal
        FROM RealesIngreso

        UNION ALL

        SELECT CodigoCuenta, TipoMovimiento, MontoReal
        FROM RealesGasto
    ),
    BaseResumen AS
    (
        SELECT
            d.TipoMovimiento,
            d.CodigoCuenta,
            cc.NombreCuenta,
            d.MontoPresupuestado,
            ISNULL(SUM(r.MontoReal), 0) AS MontoReal
        FROM dbo.DetallePresupuestoMensual d
        INNER JOIN dbo.CuentaContable cc
            ON cc.CodigoCuenta = d.CodigoCuenta
        LEFT JOIN Reales r
            ON r.CodigoCuenta = d.CodigoCuenta
           AND r.TipoMovimiento = d.TipoMovimiento
        WHERE d.Anio = @Anio
          AND d.Mes = @Mes
          AND d.Activo = 1
        GROUP BY
            d.TipoMovimiento,
            d.CodigoCuenta,
            cc.NombreCuenta,
            d.MontoPresupuestado
    )
    SELECT
        TipoMovimiento,
        CodigoCuenta,
        NombreCuenta,
        MontoPresupuestado,
        MontoReal,
        MontoPresupuestado - MontoReal AS Diferencia,

        CASE
            WHEN MontoPresupuestado = 0 AND MontoReal > 0 THEN 100
            WHEN MontoPresupuestado = 0 AND MontoReal = 0 THEN 0
            ELSE ROUND((MontoReal / MontoPresupuestado) * 100, 2)
        END AS PorcentajeEjecucion,

        CASE
            WHEN TipoMovimiento = 'Ingreso' AND MontoReal < MontoPresupuestado THEN 'Por debajo'
            WHEN TipoMovimiento = 'Ingreso' AND MontoReal >= MontoPresupuestado THEN 'Cumplido'
            WHEN TipoMovimiento = 'Gasto' AND MontoReal > MontoPresupuestado THEN 'Excedido'
            WHEN TipoMovimiento = 'Gasto' AND MontoReal <= MontoPresupuestado THEN 'Dentro del presupuesto'
            ELSE 'Sin estado'
        END AS EstadoEjecucion
    FROM BaseResumen
    ORDER BY
        TipoMovimiento,
        CodigoCuenta;
END;
GO

/* =========================================================
   PRESUPUESTO MENSUAL - RESUMEN GENERAL
   Totales de ingreso/gasto presupuestado vs real.
   CORREGIDO: evita duplicar MontoPresupuestado
   ========================================================= */
CREATE OR ALTER PROCEDURE Presupuesto.sp_PresupuestoMensual_ResumenGeneral
    @Anio INT,
    @Mes INT
AS
BEGIN
    SET NOCOUNT ON;

    ;WITH PresupuestoDetalle AS
    (
        SELECT
            d.TipoMovimiento,
            d.CodigoCuenta,
            d.MontoPresupuestado
        FROM dbo.DetallePresupuestoMensual d
        WHERE d.Anio = @Anio
          AND d.Mes = @Mes
          AND d.Activo = 1
    ),
    IngresoReal AS
    (
        SELECT
            i.CodigoCuenta,
            SUM(i.Monto) AS MontoReal
        FROM dbo.Ingreso i
        WHERE YEAR(i.FechaIngreso) = @Anio
          AND MONTH(i.FechaIngreso) = @Mes
          AND i.Activo = 1
        GROUP BY i.CodigoCuenta
    ),
    GastoReal AS
    (
        SELECT
            g.CodigoCuenta,
            SUM(g.Monto) AS MontoReal
        FROM dbo.Gasto g
        WHERE YEAR(g.FechaGasto) = @Anio
          AND MONTH(g.FechaGasto) = @Mes
          AND g.Activo = 1
        GROUP BY g.CodigoCuenta
    ),
    Resumen AS
    (
        SELECT
            p.TipoMovimiento,
            p.CodigoCuenta,
            p.MontoPresupuestado,
            CASE
                WHEN p.TipoMovimiento = 'Ingreso' THEN ISNULL(i.MontoReal, 0)
                WHEN p.TipoMovimiento = 'Gasto' THEN ISNULL(g.MontoReal, 0)
                ELSE 0
            END AS MontoReal
        FROM PresupuestoDetalle p
        LEFT JOIN IngresoReal i
            ON i.CodigoCuenta = p.CodigoCuenta
           AND p.TipoMovimiento = 'Ingreso'
        LEFT JOIN GastoReal g
            ON g.CodigoCuenta = p.CodigoCuenta
           AND p.TipoMovimiento = 'Gasto'
    )
    SELECT
        ISNULL(SUM(CASE WHEN TipoMovimiento = 'Ingreso' THEN MontoPresupuestado ELSE 0 END), 0) AS TotalIngresoPresupuestado,
        ISNULL(SUM(CASE WHEN TipoMovimiento = 'Ingreso' THEN MontoReal ELSE 0 END), 0) AS TotalIngresoReal,

        ISNULL(SUM(CASE WHEN TipoMovimiento = 'Gasto' THEN MontoPresupuestado ELSE 0 END), 0) AS TotalGastoPresupuestado,
        ISNULL(SUM(CASE WHEN TipoMovimiento = 'Gasto' THEN MontoReal ELSE 0 END), 0) AS TotalGastoReal,

        ISNULL(SUM(CASE WHEN TipoMovimiento = 'Ingreso' THEN MontoPresupuestado ELSE 0 END), 0) -
        ISNULL(SUM(CASE WHEN TipoMovimiento = 'Gasto' THEN MontoPresupuestado ELSE 0 END), 0) AS UtilidadPresupuestada,

        ISNULL(SUM(CASE WHEN TipoMovimiento = 'Ingreso' THEN MontoReal ELSE 0 END), 0) -
        ISNULL(SUM(CASE WHEN TipoMovimiento = 'Gasto' THEN MontoReal ELSE 0 END), 0) AS UtilidadReal
    FROM Resumen;
END;
GO